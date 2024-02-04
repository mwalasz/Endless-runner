using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.NetworkServicesCore
{
    internal sealed class UnityNetworkServicesInterface : NativeNetworkServicesInterfaceBase, INativeNetworkServicesInterface
    {
        #region Fields

        [SerializeField]
        private         IEnumerator         m_activeScheduler;

        private         bool                m_sendEventsOnStart;

        private         bool                m_isConnected;

        #endregion

        #region Constructors

        public UnityNetworkServicesInterface() 
            : base(isAvailable: true)
        { 
            // set properties
            m_activeScheduler   = null;
            m_sendEventsOnStart = true;
            m_isConnected       = true;
        }

        #endregion

        #region Base class methods

        public override void StartNotifier()
        {
            IEnumerator scheduler   = StatusCheckScheduler();

            // use surrogate to run scheduler
            SurrogateCoroutine.StartCoroutine(scheduler);

            // cache reference
            m_activeScheduler       = scheduler;
        }

        public override void StopNotifier()
        { 
            SurrogateCoroutine.StopCoroutine(m_activeScheduler);

            // reset property
            m_activeScheduler   = null;
        }

        #endregion

        #region Private methods
        
        private IEnumerator StatusCheckScheduler()
        {
            var     unitySettings       = EssentialKitSettings.Instance.NetworkServicesSettings;
            string  pingAddress         = unitySettings.HostAddress.IpV4;
            int     maxRetryCount       = unitySettings.PingSettings.MaxRetryCount;
            float   dt                  = unitySettings.PingSettings.TimeGapBetweenPolling;
            float   timeOutPeriod       = unitySettings.PingSettings.TimeOutPeriod;
            bool    isConnected         = m_isConnected;
            
            // send initial event
            if (m_sendEventsOnStart)
            {
                m_sendEventsOnStart     = false;
                OnPingStatusChange(isConnected);
            }

            // start ping test        
            while (true)
            {
                bool    nowConnected    = false;
#if !UNITY_WEBGL
                for (int iter = 0; iter < maxRetryCount; iter++)
                {
                    Ping    ping        = new Ping(pingAddress);
                    float   elapsedTime = 0f;

                    // ping test
                    while (!ping.isDone && elapsedTime < timeOutPeriod)
                    {
                        elapsedTime    += Time.deltaTime;
                        
                        // wait until next frame
                        yield return null;
                    }
                    
                    // check status
                    if (ping.isDone && (ping.time != -1) && elapsedTime < timeOutPeriod)
                    {
                        nowConnected    = true;
                        break;
                    }
                }
#else
                yield return null;
#endif

                // update others about state change
                if (!isConnected)
                {
                    if (nowConnected)
                    {
                        isConnected  = true;
                        OnPingStatusChange(isConnected);
                    }
                }
                else
                {
                    if (!nowConnected)
                    {
                        isConnected  = false;
                        OnPingStatusChange(isConnected);
                    }
                }
                
                // wait until we are ready to start next polling
                yield return new WaitForSeconds(dt);
            }
        }

        private void OnPingStatusChange(bool newStatus)
        {
            // update status flag
            m_isConnected   = newStatus;

            // send events
            SendHostReachabilityChangeEvent(newStatus);
            SendInternetConnectivityChangeEvent(newStatus);
        }

        #endregion
    }
}
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    public partial class NetworkServicesUnitySettings
    {
        [Serializable]
        public class PingTestSettings
        {
            #region Fields

            [SerializeField]
            [Tooltip ("The number of retries to be performed for failed tests.")]        
            private     int         m_maxRetryCount;

            [SerializeField]
            [Tooltip ("The time interval between consecutive polling.")]        
            private     float       m_timeGapBetweenPolling;

            [SerializeField]
            [Tooltip ("The time out period.")]        
            private     float       m_timeOutPeriod;

            [SerializeField]
            [Tooltip ("The connection port of the host. For DNS IP, it will be 53 or else 80.")]        
            private     int         m_port;

            #endregion

            #region Properties

            public int MaxRetryCount => m_maxRetryCount;

            public float TimeGapBetweenPolling => m_timeGapBetweenPolling;

            public float TimeOutPeriod => m_timeOutPeriod;

            public int Port => m_port;

            #endregion

            #region Constuctors

            public PingTestSettings(int maxRetryCount = 3, float timeGapBetweenPolling = 2f,
                float timeOutPeriod = 60f, int port = 53)
            {
                // set properties
                m_maxRetryCount             = maxRetryCount;
                m_timeGapBetweenPolling     = timeGapBetweenPolling;
                m_timeOutPeriod             = timeOutPeriod;
                m_port                      = port;
            }

            #endregion
        }
    }
}
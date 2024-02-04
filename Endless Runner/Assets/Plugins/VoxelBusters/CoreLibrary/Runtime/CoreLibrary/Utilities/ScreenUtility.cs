using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public class ScreenUtility : PrivateSingletonBehaviour<ScreenUtility>
    {
        #region Fields

        private     bool                    m_captureFrame      = false;
        
        private     Callback<Texture2D>     m_callback          = null;

        #endregion

        #region Static methods

        public static void CaptureFrame(Callback<Texture2D> callback)
        {
            var     instance        = GetSingleton();
            instance.m_captureFrame = true;
            instance.m_callback    += callback;
        }

        #endregion

        #region Unity methods

        private void LateUpdate()
        {
            if (m_captureFrame)
            {
                m_captureFrame  = false;
                StartCoroutine(CaptureFrameRoutine());
            }
        }

        private IEnumerator CaptureFrameRoutine()
        {
            yield return new WaitForEndOfFrame();
            var     texture     = ScreenCapture.CaptureScreenshotAsTexture();

            // send data
            m_callback?.Invoke(texture);

            // cleanup
            m_callback          = null;
            Object.Destroy(texture);
        }

        #endregion
    }
}
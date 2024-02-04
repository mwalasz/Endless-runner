using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace VoxelBusters.CoreLibrary.NativePlugins.UnityUI
{
    public sealed class DefaultUnityUIAlertDialog : UnityUIAlertDialog
    {
        #region Fields

        [SerializeField]
        private     Text                m_title     = null;   

        [SerializeField]
        private     Text                m_message   = null;             

        [SerializeField]
        private     Button[]            m_buttons   = null;

        #endregion

        #region Unity methods

        public override void Show()
        {
            base.Show();

            // set properties
            m_title.text    = Title;
            m_message.text  = Message;

            int     actionCount     = GetActionButtonCount();
            for (int iter = 0; iter < m_buttons.Length; iter++)
            {
                // update button info
                int     buttonIndex     = iter;
                var     button          = m_buttons[iter];
                if (buttonIndex < actionCount)
                {
                    var     actionInfo  = GetActionButtonAtIndex(iter);
                    button.gameObject.SetActive(true);
                    button.GetComponentInChildren<Text>().text = actionInfo.Title;
                    button.onClick.AddListener(() => SendCompletionResult(buttonIndex));
                }
                else
                {
                    button.gameObject.SetActive(false);
                }
            }
        }

        #endregion
    }
}
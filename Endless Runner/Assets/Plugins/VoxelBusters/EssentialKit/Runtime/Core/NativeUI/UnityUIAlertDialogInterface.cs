using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.UnityUI;

namespace VoxelBusters.EssentialKit.NativeUICore
{
    public sealed class UnityUIAlertDialogInterface : NativeAlertDialogInterfaceBase 
    {
        #region Fields

        private     UnityUIAlertDialog      m_unityDialog;

        #endregion

        #region Constructors

        public UnityUIAlertDialogInterface(UnityUIAlertDialog dialogPrefab, RectTransform parent)
        {
            // check argument
            Assert.IsArgNotNull(dialogPrefab, "dialogPrefab");
            Assert.IsArgNotNull(parent, "parent");

            // create object
            m_unityDialog           = Object.Instantiate(dialogPrefab, parent, false);

            // set default properties
            m_unityDialog.Title     = string.Empty;
            m_unityDialog.Message   = string.Empty;
            m_unityDialog.SetCompletionCallback(SendButtonClickEvent);
        }

        ~UnityUIAlertDialogInterface()
        {
            Dispose(false);
        }

        #endregion

        #region Base methods

        public override void SetTitle(string value)
        {
            m_unityDialog.Title    = value;
        }

        public override string GetTitle()
        {
            return m_unityDialog.Title;
        }
            
        public override void SetMessage(string value)
        {
            m_unityDialog.Message  = value;
        }

        public override string GetMessage()
        {
            return m_unityDialog.Message;
        }

        public override void AddButton(string text, bool isCancelType)
        {
            m_unityDialog.AddActionButton(text);
        }

        public override void Show()
        {
            m_unityDialog.Show();
        }

        public override void Dismiss()
        {
            m_unityDialog.Dismiss();
        }

        #endregion

        #region Private methods

        private void DestroyDialog()
        {
            // kill gameobject
            if (m_unityDialog)
            {
                Object.Destroy(m_unityDialog.gameObject);
                m_unityDialog   = null;
            }
        }

        protected override void Dispose(bool disposing)
        {
            // check whether object is released
            if (IsDisposed)
            {
                return;
            }

            if (disposing)
            {
                DestroyDialog();
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
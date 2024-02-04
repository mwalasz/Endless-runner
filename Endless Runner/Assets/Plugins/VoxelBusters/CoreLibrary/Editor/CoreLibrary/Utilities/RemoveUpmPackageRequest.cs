using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.Editor
{
    public class RemoveUpmPackageRequest : AsyncOperation<StatusCode>
    {
        #region Fields

        private     RemoveRequest           m_request;

        #endregion

        #region Properties

        public string Identifier { get; set; }

        #endregion

        #region Constructors

        public RemoveUpmPackageRequest(string identifier)
        {
            // Set properties
            Identifier  = identifier;
        }

        #endregion

        #region Base class methods

        protected override void OnStart()
        {
            m_request   = Client.Remove(Identifier);
        }

        protected override void OnUpdate()
        {
            if (!m_request.IsCompleted) return;

            // Process response
            if (m_request.Status == StatusCode.Success)
            {
                Debug.Log($"Installed package: {m_request.PackageIdOrName} successfully.");
                SetIsCompleted(m_request.Status);
            }
            else if (m_request.Status >= StatusCode.Failure)
            {
                Debug.Log($"Failed to install package: {m_request.PackageIdOrName}. Error {m_request.Error.message}.");
                SetIsCompleted(error: new Error(m_request.Error.message));
            }
        }

        #endregion
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.Editor
{
    public class AddUpmPackageRequest : AsyncOperation<StatusCode>
    {
        #region Fields

        private     AddRequest      m_request;

        #endregion

        #region Properties

        public string Identifier { get; set; }

        #endregion

        #region Constructors

        public AddUpmPackageRequest(string identifier)
        {
            // Set properties
            Identifier  = identifier;
        }

        #endregion

        #region Base class methods

        protected override void OnStart()
        {
            m_request   = Client.Add(Identifier);
        }

        protected override void OnUpdate()
        {
            if (!m_request.IsCompleted) return;

            // Process response
            if (m_request.Status == StatusCode.Success)
            {
                Debug.Log($"Installed package: {m_request.Result.packageId} successfully.");
                SetIsCompleted(StatusCode.Success);
            }
            else if (m_request.Status == StatusCode.Failure)
            {
                Debug.Log($"Failed to install package: {m_request.Result.packageId}. Error {m_request.Error.message}.");
                SetIsCompleted(error: new Error(m_request.Error.message));
            }
        }

        #endregion
    }
}
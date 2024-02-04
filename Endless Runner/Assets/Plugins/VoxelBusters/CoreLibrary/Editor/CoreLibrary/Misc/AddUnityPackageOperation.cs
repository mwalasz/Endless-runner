using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;

namespace VoxelBusters.CoreLibrary.Editor
{
    public class AddUnityPackageOperation
    {
        #region Fields

        private     string          m_package;

        private     Action          m_callback;

        private     AddRequest      m_addPackageRequest;

        private     ListRequest     m_getPackagesRequest;

        #endregion

        #region Constructors

        public AddUnityPackageOperation(string package, System.Action callback)
        {
            // set properties
            m_package               = package;
            m_callback              = callback;
            m_getPackagesRequest    = null;
            m_addPackageRequest     = null;
        }

        #endregion

        #region Public methods

        public void Start()
        {
            m_getPackagesRequest        = Client.List();

            // register for routine callbacks
            EditorApplication.update   += EditorUpdate;
        }

        #endregion

        #region Private methods

        private void EditorUpdate()
        {
            // check whether dependency packages are already installed
            if (m_getPackagesRequest != null)
            {
                if (m_getPackagesRequest.IsCompleted)
                {
                    bool    packageInstalled    = false;
                    foreach (var item in m_getPackagesRequest.Result)
                    {
                        if (string.Equals(item.name, m_package))
                        {
                            packageInstalled    = true;
                            break;
                        }
                    }

                    // reset state
                    m_getPackagesRequest        = null;

                    // create add request, incase if package is not installed
                    if (!packageInstalled)
                    {
                        Debug.LogFormat("[VoxelBusters] Creating request to add package {0}", m_package);
                        m_addPackageRequest     = Client.Add(m_package);
                    }
                    else
                    {
                        SendCompletionCallback();
                    }
                }
                return;
            }

            // import resources after required packages are installed
            if (m_addPackageRequest != null)
            {
                if (m_addPackageRequest.IsCompleted)
                {
                    SendCompletionCallback();
                }
            }
        }

        private void SendCompletionCallback()
        {
            try
            {
                m_callback();
            }
            finally
            {
                // reset state
                m_package                   = null;
                m_callback                  = null;
                m_addPackageRequest         = null;
                m_getPackagesRequest        = null;
                EditorApplication.update   -= EditorUpdate;
            }
        }

        #endregion
    }
}
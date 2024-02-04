using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEditor;
using System.Collections.Generic;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build
{
    [Serializable]
	public partial class NativePluginsExporterObject : ScriptableObject
	{
        #region Constants

        private     const   string      kBaseExporterName       = "Base";

        #endregion

        #region Fields

        [SerializeField]
        private     NativePluginsExporterObjectGroup    m_group;
        
        [SerializeField]
		private     bool			                    m_isEnabled			    = true;
        
		#endregion

        #region Properties

        public NativePluginsExporterObjectGroup Group
        {
            get { return m_group; }
            set { m_group   = value; }
        }

		public bool IsEnabled
		{
            get { return m_isEnabled; }
			set 
			{ 
				m_isEnabled = value; 
                UpdateInternalState(value);
			}
		}

        #endregion

        #region Static methods

        public static T[] FindObjects<T>(bool includeInactive = false) where T : NativePluginsExporterObject
        {
            var     availableObjects    = AssetDatabaseUtility.FindAssetObjects<T>();

            // Filter assets to required type objects
            bool    hasActiveObjects    = false;
            var     baseExporterObject  = default(T);
            var     finalObjectList     = new List<T>(availableObjects.Length);
            foreach (var exporterObject in availableObjects)
            {
                bool    isBaseObject    = string.Equals(exporterObject.name, kBaseExporterName);
                if (includeInactive || exporterObject.IsEnabled)
                {
                    finalObjectList.Add(exporterObject);
                }
                // Track object status
                if (isBaseObject)
                {
                    baseExporterObject  = exporterObject;
                }
                hasActiveObjects       |= !isBaseObject && exporterObject.IsEnabled;
            }

            // Ensure base type is included 
            if (!includeInactive)
            {
                baseExporterObject.IsEnabled    = true;
                finalObjectList.AddUnique(baseExporterObject);
            }

            return finalObjectList.ToArray();
        }

        #endregion

        #region Private methods

        protected virtual void UpdateInternalState(bool active)
        { }

        #endregion
    }
}
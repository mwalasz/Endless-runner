using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.Helpers
{
    public class DefaultLocalisationServiceProvider : ILocalisationServiceProvider
    {
        #region Static methods

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnLoad()
        {
            if (ExternalServiceProvider.LocalisationServiceProvider != null)
            {
                return;
            }

            // set default service provider
            ExternalServiceProvider.LocalisationServiceProvider = new DefaultLocalisationServiceProvider();
        }

        #endregion

        #region ILocalisationServiceProvider implementation

        public string GetLocalisedString(string key, string defaultValue)
        {
            return defaultValue;
        }

        #endregion
    }
}
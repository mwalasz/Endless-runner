using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace VoxelBusters.CoreLibrary
{
    internal class RestClient
    {
        #region Static fields

        [ClearOnReload]
        private static RestClient s_sharedInstance;

        #endregion

        #region Static properties

        public static RestClient SharedInstance => ObjectHelper.CreateInstanceIfNull(
            ref s_sharedInstance,
            () => new RestClient());

        #endregion

        #region Constructors

        public RestClient()
        {
            var     settings        = new JsonSerializerSettings
            {
                NullValueHandling       = NullValueHandling.Ignore,
                Formatting              = Formatting.None,
                MissingMemberHandling   = MissingMemberHandling.Ignore,
            };
            JsonConvert.DefaultSettings = () => settings;
        }

        #endregion

        #region Static methods

        public static string EscapeUrl(string url)
        {
            return System.Uri.EscapeUriString(url);
        }

        #endregion

        #region Public methods

        public void StartWebRequest<TResult>(UnityWebRequest request, System.Action<TResult> onSuccess,
            System.Action<string> onError)
        {
            DebugLogger.Log(CoreLibraryDomain.Default, $"Starting web request: {request.url}.");
            request.SendWebRequest().completed += (obj) =>
            {
                // Parse response
                if ((request.responseCode >= 200) && (request.responseCode <= 301))
                {
                    DebugLogger.Log(CoreLibraryDomain.Default, $"Status: {request.responseCode} Result: {request.downloadHandler.text}.");
                    var     result  = JsonConvert.DeserializeObject<TResult>(request.downloadHandler.text);
                    onSuccess(result);
                }
                else
                {
                    var     error   = request.downloadHandler.text;
                    DebugLogger.LogError(CoreLibraryDomain.Default, $"Status: {request.responseCode} Error: {error}.");
                    onError(error);
                }
            };
        }

        public byte[] ConvertObjectToBytes<TData>(TData data)
        {
            var     jsonStr = JsonConvert.SerializeObject(data);
            return System.Text.Encoding.UTF8.GetBytes(jsonStr);
        }

        #endregion
    }
}
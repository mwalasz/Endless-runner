using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.Editor;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins
{
    public partial class SimulatorServices
    {
        #region Constants

        private     const   string  kSimulatorDataFilePath  = "Library/VoxelBusters/NativePlugins/SimulatorData.json";

        #endregion

        #region Static

        private     static  Database    s_database;

        #endregion

        #region Public methods

        public static Texture2D GetRandomImage()
        {
            // select a random image file
            var     textureGuids    = AssetDatabase.FindAssets("t:texture");
            int     randomIndex     = Random.Range(0, textureGuids.Length);
            var     imagePath       = AssetDatabase.GUIDToAssetPath(textureGuids[randomIndex]);

            // create file from texture data
            var     fileData        = IOServices.ReadFileData(imagePath);
            var     texture         = new Texture2D(2, 2);
            texture.LoadImage(fileData, false);

            return texture;
        }

        #endregion

        #region Data management methods

        public static void SetObject(string key, object obj)
        {
            InitIfRequired();

            s_database.SetObject(key, obj);
            SetDirty();
        }

        public static T GetObject<T>(string key)
        {
            InitIfRequired();

            return s_database.GetObject<T>(key);
        }

        public static void RemoveObject(string key)
        {
            InitIfRequired();

            s_database.RemoveObject(key);
            SetDirty();
        }

        public static void RemoveAllObjects()
        {
            InitIfRequired();

            s_database.RemoveAllObjects();
            SetDirty();
        }

        #endregion

        #region Private methods

        private static void InitIfRequired()
        {
            if (s_database != null) return;

            // check whether serialized data exists
            s_database  = IOServices.FileExists(kSimulatorDataFilePath)
                ? JsonUtility.FromJson<Database>(IOServices.ReadFile(kSimulatorDataFilePath))
                : new Database();
        }

        private static void SetDirty()
        {
            var     parentFolder    = IOServices.GetDirectoryName(kSimulatorDataFilePath);
            IOServices.CreateDirectory(parentFolder, overwrite: false);

            // serializes the data
            var     fileData        = JsonUtility.ToJson(s_database);
            IOServices.CreateFile(kSimulatorDataFilePath, fileData);
        }

        #endregion
    }
}
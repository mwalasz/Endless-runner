using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins.UnityUI;

namespace VoxelBusters.EssentialKit.NativeUICore
{
    public sealed class UnityUIInterface : NativeUIInterfaceBase
    {
        #region Constructors

        public UnityUIInterface()
            : base(isAvailable: true)
        { }

        #endregion

        #region Base methods

        public override INativeAlertDialogInterface CreateAlertDialog(AlertDialogStyle style)
        {
            var     collection  = GetCustomUICollection();

            // create renderer
            CreateRenderIfRequired(collection);

            // create dialog
            var     parent      = (RectTransform)UnityUIRenderer.ActiveRenderer.transform;
            return new UnityUIAlertDialogInterface(collection.AlertDialogPrefab, parent);
        }

        public override INativeDatePickerInterface CreateDatePicker(DatePickerMode mode)
        {
            return new NullDatePickerInterface(mode);
            /*var     collection  = GetCustomUICollection();

            // create renderer
            CreateRenderIfRequired(collection);

            var     parent      = (RectTransform)UnityUIRenderer.ActiveRenderer.transform;
            return new UnityUIDatePickerInterface(mode, collection.DatePickerPrefab, parent);*/
        }

        #endregion

        #region Static methods

        private static NativeUIUnitySettings.UnityUICollection GetCustomUICollection()
        {
            return NativeUI.UnitySettings.CustomUICollection;
        }

        private static void CreateRenderIfRequired(NativeUIUnitySettings.UnityUICollection uiCollection)
        {
            // find renderer as specified in the settings
            int     targetDisplayOrder  = uiCollection.RendererPrefab.DisplayOrder;
            if (UnityUIRenderer.ActiveRenderer != null && UnityUIRenderer.ActiveRenderer.DisplayOrder == targetDisplayOrder)
            {
                return;
            }

            // find whether scene has required renderer
            UnityUIRenderer targetRenderer  = null;
            var             renderers       = Object.FindObjectsOfType<UnityUIRenderer>();
            if (renderers.Length > 0)
            {
                foreach (var current in renderers)
                {
                    if (current.DisplayOrder == targetDisplayOrder)
                    {
                        targetRenderer  = current;
                        break;
                    }
                }
            }

            // create object using prefab
            if (targetRenderer == null)
            {
                targetRenderer  = Object.Instantiate(uiCollection.RendererPrefab);
            }

            // set value
            UnityUIRenderer.ActiveRenderer  = targetRenderer;
        }

        #endregion
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoxelBusters.CoreLibrary;

using UnityObject = UnityEngine.Object;

namespace VoxelBusters.CoreLibrary.NativePlugins.DemoKit
{
	public static class DemoHelper 
	{
		#region Static methods

		public static string[] GetMultivaluedString(InputField inputField)
		{
			return inputField.text.Split(',');
		}

        #endregion

        #region Create objects

        public static void CreateItems(RectTransform parentNode, Toggle prefab, string[] items, Action<int> onItemSelect)
        {
            // remove existing objects
            TransformUtility.RemoveAllChilds(parentNode);

            // create new products
            ToggleGroup group   = parentNode.gameObject.AddComponentIfNotFound<ToggleGroup>();
            if (items != null)
            {
                for (int iter = 0; iter < items.Length; iter++)
                {
                    Toggle          child           = UnityObject.Instantiate(prefab, parentNode, false);
                    int             productIndex    = iter;

                    // update label
                    child.GetComponentInChildren<Text>().text   = items[iter];
                    child.gameObject.name                       = iter.ToString();

                    // register toggle
                    group.RegisterToggle(child);
                    child.group     = group;
                    child.isOn      = (iter == 0);

                    // register for event
                    child.onValueChanged.AddListener((isOn) =>
                    {
                        onItemSelect(productIndex);
                    });
                }

                onItemSelect(0);
            }
        }

        #endregion
    }
}

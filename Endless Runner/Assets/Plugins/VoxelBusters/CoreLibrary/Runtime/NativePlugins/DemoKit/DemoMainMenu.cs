using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.CoreLibrary.NativePlugins.DemoKit
{
	public class DemoMainMenu : DemoPanel 
	{
		#region Fields

		[SerializeField]
		private     RectTransform		m_optionsRect		    = null;

        [SerializeField]
		private	    MenuOption[]	    m_options			    = new MenuOption[0];

        [SerializeField]
        private     Button              m_optionButtonPrefab    = null;

		#endregion

		#region Unity methods

		private void Awake()
		{
			Rebuild();
		}

        #endregion

        #region Base methods

        public override void Rebuild()
        {
            // remove existing options
            Transform[] oldChildren = TransformUtility.GetImmediateChildren(m_optionsRect);
            for (int iter = 0; iter < oldChildren.Length; iter++)
            {
                Destroy(oldChildren[iter].gameObject);
            }

            // configure options
            for (int iter = 0; iter < m_options.Length; iter++)
            {
                MenuOption  menuOption  = m_options[iter];

                Button      newButton   = Instantiate(m_optionButtonPrefab, m_optionsRect, false);
                newButton.GetComponentInChildren<Text>().text = menuOption.displayName;
                newButton.onClick.AddListener(() => SceneManager.LoadScene(menuOption.sceneName));
            }
        }

        #endregion

        #region Nested types

        [Serializable]
		public class MenuOption
		{
			public string displayName;

			public string sceneName;
		}

		#endregion
	}
}

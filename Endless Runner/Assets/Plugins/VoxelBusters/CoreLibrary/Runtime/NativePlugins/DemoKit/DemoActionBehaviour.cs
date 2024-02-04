using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Serialization;

namespace VoxelBusters.CoreLibrary.NativePlugins.DemoKit
{
	public class DemoActionBehaviour<TActionType> : MonoBehaviour where TActionType : struct, IConvertible
    {
        #region Fields

        [SerializeField]
		private		TActionType 		m_actionType	= default(TActionType);

        #endregion

        #region Properties

        public Selectable Selectable 
        { 
            get; 
            private set; 
        }

        public TActionType ActionType
		{
			get
			{
				return m_actionType;
			}
		}

        #endregion

        #region Events

        [SerializeField, FormerlySerializedAs("onSelect")]
        private SelectEvent m_onSelect = new SelectEvent();
        public SelectEvent OnSelect
        {
            get
            {
                return m_onSelect;
            }
        }

        #endregion

        #region Unity methods

        private void Awake()
        {
            // cache components
            Selectable = GetComponent<Selectable>();

            RegisterForCallback(Selectable);
        }

        #endregion

        #region Private methods

        private void RegisterForCallback(Selectable selectable)
        {
            // cache component
            if (selectable is Button)
            {
                ((Button)selectable).onClick.AddListener(OnSelectInternal);
            }
            if (selectable is Toggle)
            {
                ((Toggle)selectable).onValueChanged.AddListener((value) => OnSelectInternal());
            }
            if (selectable is Dropdown)
            {
                ((Dropdown)selectable).onValueChanged.AddListener((value) => OnSelectInternal());
            }
        }

        private void OnSelectInternal()
        {
            // send event
            if (m_onSelect != null)
            {
                m_onSelect.Invoke(Selectable);
            }
        }

        #endregion

        #region Nested types

        [Serializable]
        public class SelectEvent : UnityEvent<Selectable>
        { }

        #endregion
    }
}

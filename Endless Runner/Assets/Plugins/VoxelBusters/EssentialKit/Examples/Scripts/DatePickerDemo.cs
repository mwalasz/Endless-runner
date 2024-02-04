using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// key namespaces
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.EssentialKit;
// internal namespace
using VoxelBusters.CoreLibrary.NativePlugins.DemoKit;
using System;

namespace VoxelBusters.EssentialKit.Demo
{
    public class DatePickerDemo : DemoActionPanelBase<DatePickerDemoAction, DatePickerDemoActionType>
    {
       #region Fields

        [SerializeField]
        private     RectTransform       m_presentationSection               = null;    

        private     DatePicker          m_activeDatePicker;

        [SerializeField]
        private     Dropdown            m_mode = null;

        #endregion

        #region Base methods

        protected override void Start() 
        {
            base.Start();

            // set default text values
            m_mode.options = new List<Dropdown.OptionData>(Array.ConvertAll(Enum.GetNames(typeof(DatePickerMode)), (item) => new Dropdown.OptionData(item)));

            SetPresentationState(false);
        }

        protected override string GetCreateInstanceCodeSnippet()
        {
            return "DatePicker.CreateInstance()";
        }

        protected override void OnActionSelectInternal(DatePickerDemoAction selectedAction)
        {
            switch (selectedAction.ActionType)
            {
               case DatePickerDemoActionType.New:
                    Log("Creating new date picker : " + GetMode());
                    m_activeDatePicker          = DatePicker.CreateInstance(GetMode());
                    m_activeDatePicker.OnCloseCallback = (result) =>
                    {
                        Log("Selected date : " + result.SelectedDate);
                    };
                    SetPresentationState(true);
                    break;

                case DatePickerDemoActionType.Show:
                    Log("Showing date picker.");
                    m_activeDatePicker.Show();
                    SetPresentationState(false);
                    break;

                case DatePickerDemoActionType.ResourcePage:
                    ProductResources.OpenResourcePage(NativeFeatureType.kNativeUI);
                    break;

                default:
                    break;
            }
        }

        #endregion

        #region Private methods

        private void SetPresentationState(bool value)
        {
            m_presentationSection.gameObject.SetActive(value);
        }

        private DatePickerMode GetMode()
        {
            return (DatePickerMode)Enum.GetValues(typeof(DatePickerMode)).GetValue(m_mode.value);
        }

        #endregion
    }
}
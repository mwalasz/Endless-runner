#if UNITY_ANDROID
namespace VoxelBusters.EssentialKit.NativeUICore.Android
{
    public sealed class UIInterface : NativeUIInterfaceBase, INativeUIInterface
    {
        #region Constructors

        public UIInterface()
            : base(isAvailable: true)
        { }

        #endregion

        #region Base methods

        public override INativeAlertDialogInterface CreateAlertDialog(AlertDialogStyle style)
        {
            return new AlertDialog(style);
        }

        public override INativeDatePickerInterface CreateDatePicker(DatePickerMode mode)
        {
            /*var     unityUIInterface    = new UnityUIInterface();
            return unityUIInterface.CreateDatePicker(mode);*/
            return new DateTimePicker(mode);
        }

        #endregion
    }
}
#endif
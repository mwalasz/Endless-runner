#if UNITY_IOS || UNITY_TVOS

namespace VoxelBusters.EssentialKit.NativeUICore.iOS
{
    public sealed class NativeUIInterface : NativeUIInterfaceBase, INativeUIInterface
    {
        #region Constructors

        public NativeUIInterface()
            : base(isAvailable: true)
        { }

        #endregion

        #region Base methods

        public override INativeAlertDialogInterface CreateAlertDialog(AlertDialogStyle style)
        {
            return new NativeAlertDialog(style);
        }

        public override INativeDatePickerInterface CreateDatePicker(DatePickerMode mode)
        {
            /*var     unityUIInterface    = new UnityUIInterface();
            return unityUIInterface.CreateDatePicker(mode);*/

            return new NativeDatePicker(mode);
        }

        #endregion
    }
}
#endif
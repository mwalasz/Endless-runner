using System;

namespace VoxelBusters.CoreLibrary
{
    public class VBException : Exception
    {
        #region Properties

        public string Domain { get; private set; }

        public int ErrorCode { get; private set; }

        #endregion

        #region Constructors

        public VBException(string message, int errorCode = -1, Exception innerException = null)
            : base(message, innerException)
        {
            // set properties
            ErrorCode       = errorCode;
        }

        public VBException(string message, Exception innerException)
            : this(message, -1, innerException)
        { }

        #endregion

        #region Create methods

        public static VBException NotImplemented(string messsage = "Not implemented.")
        {
            return new VBException(messsage);
        }

        public static VBException NotSupported(string messsage = "Not supported.")
        {
            return new VBException(messsage);
        }

        public static VBException InvalidOperation(string messsage = "Invalid operation.")
        {
            return new VBException(messsage);
        }

        public static VBException ArgumentNull(string property)
        {
            return new VBException(string.Format("{0} is null.", property));
        }
        
        public static VBException SwitchCaseNotImplemented(object value)
        {
            return new VBException(string.Format("Switch case for {0} is not implemented.", value));
        }

        #endregion
    }
}
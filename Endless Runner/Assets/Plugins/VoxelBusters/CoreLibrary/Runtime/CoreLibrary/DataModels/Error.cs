using System;

namespace VoxelBusters.CoreLibrary
{
    public class Error
    {
        #region Properties

        /// <summary>
        /// A description of the error occured (read-only).
        /// </summary>
        /// <value>The description of the error.</value>
        public string Domain { get; private set; }

        /// <summary>
        /// A value indicating the type of error occured (read-only).
        /// </summary>
        /// <value>The error code.</value>
        public int Code { get; private set; }

        /// <summary>
        /// A description of the error occured (read-only).
        /// </summary>
        /// <value>The description of the error.</value>
        public string Description { get; private set; }

        #endregion

        #region Constructors

        public Error(string description)
            : this(domain: null, code: 0, description: description)
        { }

        public Error(string domain, int code, string description)
        {
            // set properties
            Domain          = domain;
            Code            = code;
            Description     = description;
        }

        #endregion

        #region Static methods

        public static Error CreateNullableError(string description)
        {
            if (description == null)
            {
                return null;
            }

            return new Error(description);
        }

        #endregion

        #region Base class methods

        public override string ToString()
        {
            return string.Format("Error Domain: {0} Code: {1} Description: {2}", Domain, Code, Description);
        }

        #endregion
    }
}
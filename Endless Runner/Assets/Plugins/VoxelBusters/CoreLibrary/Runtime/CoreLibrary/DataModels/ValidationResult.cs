using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public class ValidationResult
    {
        #region Static properties

        public static ValidationResult Success { get; private set;}

        #endregion

        #region Properties

        public bool IsValid { get; private set; }

        public Error Error { get; private set; }

        #endregion

        #region Constructors

        static ValidationResult()
        {
            // set static property values
            Success     = new ValidationResult(isValid: true, error: null);
        }

        private ValidationResult(bool isValid, Error error = null)
        {
            // set properties
            IsValid     = isValid;
            Error       = error;
        }

        #endregion

        #region Create methods

        public static ValidationResult CreateError(Error error)
        {
            return new ValidationResult(
                isValid: false,
                error: error);
        }

        public static ValidationResult CreateError(string domain = null, int code = -1, string description = "")
        {
            return new ValidationResult(
                isValid: false,
                error: new Error(domain, code, description));
        }

        #endregion

        #region Base class methods

        public override string ToString()
        {
            return $"(IsValid: {IsValid} Error: {Error})";
        }

        #endregion
    }
}
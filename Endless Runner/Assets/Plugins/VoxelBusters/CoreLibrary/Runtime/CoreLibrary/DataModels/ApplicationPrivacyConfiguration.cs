using UnityEngine;
using System.Collections;

namespace VoxelBusters.CoreLibrary
{
    public class ApplicationPrivacyConfiguration
    {
        #region Properties

        public ConsentStatus UsageConsent { get; private set; }

        public bool? IsAgeRestrictedUser { get; private set; } 

        public ContentRating? PreferredContentRating { get; private set; } 

        public string Version { get; private set; }

        #endregion

        #region Constructors

        public ApplicationPrivacyConfiguration(ConsentStatus usageConsent,
                                               bool? isAgeRestrictedUser = null,
                                               ContentRating? preferredContentRating = null,
                                               string version = null)
        {
            // Set properties
            UsageConsent            = usageConsent;
            IsAgeRestrictedUser     = isAgeRestrictedUser;
            PreferredContentRating  = preferredContentRating;
            Version                 = version;
        }

        #endregion

        #region Public methods

        public bool? IsCoppaApplicable()
        {
            if (IsAgeRestrictedUser == null) return null;

            return (IsAgeRestrictedUser.Value == true) || (UsageConsent != ConsentStatus.Authorized);
        }

        #endregion

    }
}
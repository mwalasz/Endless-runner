using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit
{
    public static class AssemblyName
    {
        public  const   string      kMain                   = "VoxelBusters.EssentialKit";
        
        public  const   string      kIos                    = "VoxelBusters.EssentialKit.iOSModule";
        
        public  const   string      kAndroid                = "VoxelBusters.EssentialKit.AndroidModule";
        
        public  const   string      kSimulator              = "VoxelBusters.EssentialKit.SimulatorModule";
    }

    public static class NamespaceName
    {
        public  const   string      kRoot                   = "VoxelBusters.EssentialKit";
        
        public  const   string      kAddressBook            = kRoot + ".AddressBookCore";
        
        public  const   string      kBillingServices        = kRoot + ".BillingServicesCore";
        
        public  const   string      kCloudServices          = kRoot + ".CloudServicesCore";
        
        public  const   string      kGameServices           = kRoot + ".GameServicesCore";
        
        public  const   string      kMediaServices          = kRoot + ".MediaServicesCore";
        
        public  const   string      kNativeUI               = kRoot + ".NativeUICore";
        
        public  const   string      kNetworkServices        = kRoot + ".NetworkServicesCore";
        
        public  const   string      kNotificationServices   = kRoot + ".NotificationServicesCore";
        
        public  const   string      kSharingServices        = kRoot + ".SharingServicesCore";
        
        public  const   string      kWebView                = kRoot + ".WebViewCore";
        
        public  const   string      kExtras                 = kRoot + ".ExtrasCore";
        
        public  const   string      kDeepLinkServices       = kRoot + ".DeepLinkServicesCore";
    }

    internal static class ImplementationSchema
    {
        #region Static fields

        private static Dictionary<string, NativeFeatureRuntimeConfiguration>    s_configurationMap;

        #endregion

        #region Static properties

        public static NativeFeatureRuntimeConfiguration AddressBook 
        { 
            get => GetRuntimeConfiguration(NativeFeatureType.kAddressBook);
        }

        public static NativeFeatureRuntimeConfiguration BillingServices 
        { 
            get => GetRuntimeConfiguration(NativeFeatureType.kBillingServices);
        }

        public static NativeFeatureRuntimeConfiguration CloudServices 
        { 
            get => GetRuntimeConfiguration(NativeFeatureType.kCloudServices);
        }
        
        public static NativeFeatureRuntimeConfiguration GameServices
        {
            get => GetRuntimeConfiguration(NativeFeatureType.kGameServices);
        }
        
        public static NativeFeatureRuntimeConfiguration MediaServices
        {
            get => GetRuntimeConfiguration(NativeFeatureType.kMediaServices);
        }
        
        public static NativeFeatureRuntimeConfiguration NativeUI
        {
            get => GetRuntimeConfiguration(NativeFeatureType.kNativeUI);
        }
        
        public static NativeFeatureRuntimeConfiguration NetworkServices
        {
            get => GetRuntimeConfiguration(NativeFeatureType.kNetworkServices);
        }
        
        public static NativeFeatureRuntimeConfiguration NotificationServices
        {
            get => GetRuntimeConfiguration(NativeFeatureType.kNotificationServices);
        }
        
        public static NativeFeatureRuntimeConfiguration SharingServices
        {
            get => GetRuntimeConfiguration(NativeFeatureType.kSharingServices);
        }
        
        public static NativeFeatureRuntimeConfiguration WebView
        {
            get => GetRuntimeConfiguration(NativeFeatureType.kWebView);
        }
        
        public static NativeFeatureRuntimeConfiguration Extras
        {
            get => GetRuntimeConfiguration(NativeFeatureType.kExtras);
        }
        
        public static NativeFeatureRuntimeConfiguration DeepLinkServices
        {
            get => GetRuntimeConfiguration(NativeFeatureType.kDeepLinkServices);
        }
        
        #endregion

        #region Constructors

        static ImplementationSchema()
        {
            s_configurationMap  = new Dictionary<string, NativeFeatureRuntimeConfiguration>()
            {
                // Address Book
                {
                    NativeFeatureType.kAddressBook,
                    new NativeFeatureRuntimeConfiguration(packages: new NativeFeatureRuntimePackage[]
                                                          {
                                                            NativeFeatureRuntimePackage.iOS(assembly: AssemblyName.kIos,
                                                                                            ns: $"{NamespaceName.kAddressBook}.iOS",
                                                                                            nativeInterfaceType: "AddressBookInterface",
                                                                                            bindingTypes: new string[] { "AddressBookBinding" }),
                                                            NativeFeatureRuntimePackage.Android(assembly: AssemblyName.kAndroid,
                                                                                                ns: $"{NamespaceName.kAddressBook}.Android",
                                                                                                nativeInterfaceType: "AddressBookInterface"),
                                                          },
                                                          simulatorPackage: NativeFeatureRuntimePackage.Generic(assembly: AssemblyName.kSimulator,
                                                                                                                ns: $"{NamespaceName.kAddressBook}.Simulator",
                                                                                                                nativeInterfaceType: "AddressBookInterface"),
                                                          fallbackPackage: NativeFeatureRuntimePackage.Generic(assembly: AssemblyName.kMain,
                                                                                                               ns: NamespaceName.kAddressBook,
                                                                                                               nativeInterfaceType: "NullAddressBookInterface"))
                },

                // Native UI
                {
                    NativeFeatureType.kNativeUI,
                    new NativeFeatureRuntimeConfiguration(packages: new NativeFeatureRuntimePackage[]
                                                          {
                                                            NativeFeatureRuntimePackage.iOS(assembly: AssemblyName.kIos,
                                                                                            ns: $"{NamespaceName.kNativeUI}.iOS",
                                                                                            nativeInterfaceType: "NativeUIInterface",
                                                                                            bindingTypes: new string[] { "DatePickerControllerBinding", "AlertControllerBinding" }),
                                                            NativeFeatureRuntimePackage.Android(assembly: AssemblyName.kAndroid,
                                                                                                ns: $"{NamespaceName.kNativeUI}.Android",
                                                                                                nativeInterfaceType: "UIInterface"),
                                                          },
                                                          fallbackPackage: NativeFeatureRuntimePackage.Generic(assembly: AssemblyName.kMain,
                                                                                                               ns: NamespaceName.kNativeUI,
                                                                                                               nativeInterfaceType: "UnityUIInterface"))
                },

                // Network Services
                { 
                    NativeFeatureType.kNetworkServices,
                    new NativeFeatureRuntimeConfiguration(packages: new NativeFeatureRuntimePackage[]
                                                          {
                                                            NativeFeatureRuntimePackage.iOS(assembly: AssemblyName.kIos,
                                                                                            ns: $"{NamespaceName.kNetworkServices}.iOS",
                                                                                            nativeInterfaceType: "NetworkServicesInterface",
                                                                                            bindingTypes: new string[] { "NetworkServicesBinding" }),
                                                            NativeFeatureRuntimePackage.Android(assembly: AssemblyName.kAndroid,
                                                                                                ns: $"{NamespaceName.kNetworkServices}.Android",
                                                                                                nativeInterfaceType: "NetworkServicesInterface"),
                                                          },
                                                          fallbackPackage: NativeFeatureRuntimePackage.Generic(assembly: AssemblyName.kMain,
                                                                                                               ns: NamespaceName.kNetworkServices,
                                                                                                               nativeInterfaceType: "UnityNetworkServicesInterface"))
                },

                // Sharing Services
                {
                    NativeFeatureType.kSharingServices,
                    new NativeFeatureRuntimeConfiguration(packages: new NativeFeatureRuntimePackage[]
                                                          {
                                                            NativeFeatureRuntimePackage.iOS(assembly: AssemblyName.kIos,
                                                                                            ns: $"{NamespaceName.kSharingServices}.iOS",
                                                                                            nativeInterfaceType: "NativeSharingInterface",
                                                                                            bindingTypes: new string[] { "SocialShareComposerBinding", "ShareSheetBinding", "MessageComposerBinding", "MailComposerBinding" }),
                                                            NativeFeatureRuntimePackage.Android(assembly: AssemblyName.kAndroid,
                                                                                                ns: $"{NamespaceName.kSharingServices}.Android",
                                                                                                nativeInterfaceType: "SharingServicesInterface"),
                                                          },
                                                          simulatorPackage: NativeFeatureRuntimePackage.Generic(assembly: AssemblyName.kSimulator,
                                                                                                                ns: $"{NamespaceName.kSharingServices}.Simulator",
                                                                                                                nativeInterfaceType: "NativeSharingInterface"),
                                                          fallbackPackage: NativeFeatureRuntimePackage.Generic(assembly: AssemblyName.kMain,
                                                                                                               ns: NamespaceName.kSharingServices,
                                                                                                               nativeInterfaceType: "NullSharingInterface"))
                },

                // Extras
                {
                    NativeFeatureType.kExtras,
                    new NativeFeatureRuntimeConfiguration(packages: new NativeFeatureRuntimePackage[]
                                                          {
                                                            NativeFeatureRuntimePackage.iOS(assembly: AssemblyName.kIos,
                                                                                            ns: $"{NamespaceName.kExtras}.iOS",
                                                                                            nativeInterfaceType: "NativeUtilityInterface",
                                                                                            bindingTypes: new string[] { "NativeUtilityInterface" }),
                                                            NativeFeatureRuntimePackage.Android(assembly: AssemblyName.kAndroid,
                                                                                                ns: $"{NamespaceName.kExtras}.Android",
                                                                                                nativeInterfaceType: "UtilityInterface"),
                                                          },
                                                          fallbackPackage: NativeFeatureRuntimePackage.Generic(assembly: AssemblyName.kMain,
                                                                                                               ns: NamespaceName.kExtras,
                                                                                                               nativeInterfaceType: "NullNativeUtilityInterface"))
                }
            };
        }
            
        #endregion

        #region Public static methods

        public static KeyValuePair<string, NativeFeatureRuntimeConfiguration>[] GetAllRuntimeConfigurations(bool includeInactive = true, EssentialKitSettings settings = null)
        {
            Assert.IsTrue(includeInactive || (settings != null), "Arg settings is null.");

            var     configurations  = new List<KeyValuePair<string, NativeFeatureRuntimeConfiguration>>();
            foreach (var feature in s_configurationMap)
            {
                if (includeInactive || ((settings != null) && settings.IsFeatureUsed(feature.Key)))
                {
                    configurations.Add(feature);
                }
            }
            return configurations.ToArray();
        }

        public static NativeFeatureRuntimeConfiguration GetRuntimeConfiguration(string featureName)
        {
            s_configurationMap.TryGetValue(featureName, out NativeFeatureRuntimeConfiguration config);

            return config;
        }

        #endregion
    }
}
﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Arc4u.Cyphertool.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class HelpTexts {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal HelpTexts() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Arc4u.Cyphertool.Resources.HelpTexts", typeof(HelpTexts).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ----------------------------------
        ///|       Arc4u Cyphertool.        |
        ///----------------------------------
        ///
        ///Encrypt a string or a file with a certificate in the certificate store or the keychain.
        ///
        ///.
        /// </summary>
        internal static string EncryptFromCertificateHelper {
            get {
                return ResourceManager.GetString("EncryptFromCertificateHelper", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to .
        /// </summary>
        internal static string EncryptFromPfxHelper {
            get {
                return ResourceManager.GetString("EncryptFromPfxHelper", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ----------------------------------
        ///|       Arc4u Cyphertool.        |
        ///----------------------------------
        ///
        ///Let you encrypt a text or a file using a certificate or a pfx file.
        ///
        ///With a certificate in the certificate store or keychain.
        ///arc4u.cyphertool encrypt from text -c devCertName text &quot;&quot;clear text&quot;&quot;
        ///
        ///With a pfx file.
        ///arc4u.cyphertool encrypt from pfx -c &quot;&quot;C:\temp\devCert.pfx&quot;&quot; -p password text &quot;&quot;clear text&quot;&quot;
        ///.
        /// </summary>
        internal static string EncryptHelper {
            get {
                return ResourceManager.GetString("EncryptHelper", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ----------------------------------
        ///|       Arc4u Cyphertool.        |
        ///----------------------------------
        ///
        ///When you use the cypher tool and you want to encrypt a text based on a certificate,
        ///you have to provide the clear text via the text argument like this:
        ///
        ///=&gt;    arc4u.cyphertool encrypt from certificatestore &quot;certificate&quot; ... text &quot;clear text&quot;
        ///
        ///
        ///.
        /// </summary>
        internal static string EncryptTextHelper {
            get {
                return ResourceManager.GetString("EncryptTextHelper", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///      __        _______         ______     _    _     _____  _____ 
        ///     /  \      |_   __ \      .&apos; ___  |   | |  | |   |_   _||_   _|
        ///    / /\ \       | |__) |    / .&apos;   \_|   | |__| |_    | |    | |  
        ///   / ____ \      |  __ /     | |          |____   _|   | &apos;    &apos; |  
        /// _/ /    \ \_   _| |  \ \_   \ `.___.&apos;\       _| |_     \ `--&apos; /   
        ///|____|  |____| |____| |___|   `._____.&apos;      |_____|     `.__.&apos;    
        ///                                                                   
        ///
        ///######## ##    ##  ###### [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string RootHelper {
            get {
                return ResourceManager.GetString("RootHelper", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Arc4u Cyphertool {0}. .
        /// </summary>
        internal static string Version {
            get {
                return ResourceManager.GetString("Version", resourceCulture);
            }
        }
    }
}

﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GenerateAnswerFile.Properties {
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
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("GenerateAnswerFile.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to Generates answer files (unattend.xml and autounattend.xml) for unattended Windows installation..
        /// </summary>
        internal static string ApplicationDescription {
            get {
                return ResourceManager.GetString("ApplicationDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The number of times the user specified by -AutoLogonUser will be automatically logged on..
        /// </summary>
        internal static string AutoLogonCountDescription {
            get {
                return ResourceManager.GetString("AutoLogonCountDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The password of the user specified by -AutoLogonUser..
        /// </summary>
        internal static string AutoLogonPasswordDescription {
            get {
                return ResourceManager.GetString("AutoLogonPasswordDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The name of a user to automatically log on, in the format &apos;domain\user&apos;, or just &apos;user&apos; for local users. If not specified, automatic log-on will not be used..
        /// </summary>
        internal static string AutoLogonUserDescription {
            get {
                return ResourceManager.GetString("AutoLogonUserDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Domain options:.
        /// </summary>
        internal static string CategoryDomain {
            get {
                return ResourceManager.GetString("CategoryDomain", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Installation options:.
        /// </summary>
        internal static string CategoryInstall {
            get {
                return ResourceManager.GetString("CategoryInstall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Other setup options:.
        /// </summary>
        internal static string CategoryOther {
            get {
                return ResourceManager.GetString("CategoryOther", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User account options:.
        /// </summary>
        internal static string CategoryUserAccounts {
            get {
                return ResourceManager.GetString("CategoryUserAccounts", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The network name for the computer. If not specified, Windows will generate a default name. Any &apos;#&apos; characters in the name will be replaced with a random digit between 0 and 9. For example, &apos;PC-###&apos; would be replaced with &apos;PC-123&apos; (or some other random number)..
        /// </summary>
        internal static string ComputerNameDescription {
            get {
                return ResourceManager.GetString("ComputerNameDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Disable Windows cloud consumer features. This prevents auto-installation of recommended store apps..
        /// </summary>
        internal static string DisableCloudDescription {
            get {
                return ResourceManager.GetString("DisableCloudDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Disable Windows Defender virus and threat protection..
        /// </summary>
        internal static string DisableDefenderDesciption {
            get {
                return ResourceManager.GetString("DisableDefenderDesciption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Do not automatically start Server Manager when logging on (Windows Server only)..
        /// </summary>
        internal static string DisableServerManagerDescription {
            get {
                return ResourceManager.GetString("DisableServerManagerDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The display resolution, in the format &apos;width,height&apos;. For example, &apos;1920,1080&apos;. If not specified, the default resolution is determined by Windows..
        /// </summary>
        internal static string DisplayResolutionDescription {
            get {
                return ResourceManager.GetString("DisplayResolutionDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The name of a domain account to add to a local group, using the format &apos;group:domain\user&apos;, &apos;domain\user&apos;, &apos;group:user&apos; or &apos;user&apos;. If no group is specified, the user is added to the local Administrators group. You can specify multiple groups by separating them with semicolons. If no domain is specified, the user must be in the domain you&apos;re joining. Can have multiple values..
        /// </summary>
        internal static string DomainAccountsDescription {
            get {
                return ResourceManager.GetString("DomainAccountsDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [Group:][Domain\]User.
        /// </summary>
        internal static string DomainUserGroupValueDescription {
            get {
                return ResourceManager.GetString("DomainUserGroupValueDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Domain\User.
        /// </summary>
        internal static string DomainUserValueDescription {
            get {
                return ResourceManager.GetString("DomainUserValueDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Turn on remote desktop, and create a Windows Defender Firewall rule to allow incoming connections..
        /// </summary>
        internal static string EnableRemoteDesktopDescriptoin {
            get {
                return ResourceManager.GetString("EnableRemoteDesktopDescriptoin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The name of an optional feature to install. Use the PowerShell &apos;Get-WindowsOptionalFeature&apos; command to get a list of valid feature names. Can have multiple values..
        /// </summary>
        internal static string FeaturesDescription {
            get {
                return ResourceManager.GetString("FeaturesDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A command to run during first logon. Can have multiple values. All commands are executed before the scripts specified by -FirstLogonScript, in the order specified..
        /// </summary>
        internal static string FirstLogonCommandsDescription {
            get {
                return ResourceManager.GetString("FirstLogonCommandsDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The full path of a Windows PowerShell script to run during first log-on, plus arguments. Can have multiple values. Scripts are executed after the commands specified by -FirstLogonCommand, in the order specified..
        /// </summary>
        internal static string FirstLogonScriptsDescription {
            get {
                return ResourceManager.GetString("FirstLogonScriptsDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The index of the image in the WIM file to install. Use this for Windows editions not installed using a product key, such as those that use volume licensing. Use the PowerShell &apos;Get-WindowsImage&apos; command to list all images in a .wim or .esd file..
        /// </summary>
        internal static string ImageIndexDescription {
            get {
                return ResourceManager.GetString("ImageIndexDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The installation method to use..
        /// </summary>
        internal static string InstallDescription {
            get {
                return ResourceManager.GetString("InstallDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The zero-based ID of the disk to install to. This disk will be wiped and repartitioned according to -Partition, or using the default layout if -Partition is not specified..
        /// </summary>
        internal static string InstallToDiskDescription {
            get {
                return ResourceManager.GetString("InstallToDiskDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The one-based ID of the partition to install to, on the disk specified by -InstallToDisk. If not specified and -Install is CleanEfi or CleanBios, Windows will be installed on the first regular data partition. If -Install is ExistingPartition, the default value is 3, which is appropriate for UEFI systems with the default partition layout..
        /// </summary>
        internal static string InstallToPartitionDescription {
            get {
                return ResourceManager.GetString("InstallToPartitionDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to At least one installation method must be provided..
        /// </summary>
        internal static string InvalidMethodCount {
            get {
                return ResourceManager.GetString("InvalidMethodCount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The name of a domain to join. If not specified, the system will not be joined to a domain..
        /// </summary>
        internal static string JoinDomainDescription {
            get {
                return ResourceManager.GetString("JoinDomainDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Join the domain during the offlineServicing pass of Windows setup, rather than the specialize pass..
        /// </summary>
        internal static string JoinDomainOfflineDescription {
            get {
                return ResourceManager.GetString("JoinDomainOfflineDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The password of the user specified by -JoinDomainUser. This will be stored in plain text in the answer file..
        /// </summary>
        internal static string JoinDomainPasswordDescription {
            get {
                return ResourceManager.GetString("JoinDomainPasswordDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The path to a file containing provisioned account data to join the domain. This file can be created using the command &apos;djoin.exe /provision /domain domainname /machine machinename /savefile filename&apos;..
        /// </summary>
        internal static string JoinDomainProvisioningFileDescription {
            get {
                return ResourceManager.GetString("JoinDomainProvisioningFileDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The name of a user with permission to join the domain specified by -JoinDomain. Use the format &apos;domain\user&apos;, or just &apos;user&apos; if the user is a member of the domain you are joining..
        /// </summary>
        internal static string JoinDomainUserDescription {
            get {
                return ResourceManager.GetString("JoinDomainUserDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to JSON input was found; additional command line arguments are available if JSON input is not provided. For more information, see: https://github.com/SvenGroot/GenerateAnswerFile.
        /// </summary>
        internal static string JsonUsageFooter {
            get {
                return ResourceManager.GetString("JsonUsageFooter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The language used for the UI language, and the input, system and user locales..
        /// </summary>
        internal static string LanguageDescription {
            get {
                return ResourceManager.GetString("LanguageDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A local account to create, using the format &apos;group:name,password&apos; or &apos;name,password&apos;. Can have multiple values. If no group is specified, the user will be added to the Administrators group. You can specify multiple groups by separating them with semicolons.
        ///
        ///If no local accounts are created, the user will be asked to create one during OOBE, making setup not fully unattended..
        /// </summary>
        internal static string LocalAccountsDescription {
            get {
                return ResourceManager.GetString("LocalAccountsDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [Group:]Name,Password.
        /// </summary>
        internal static string LocalCredentialValueDescription {
            get {
                return ResourceManager.GetString("LocalCredentialValueDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Number.
        /// </summary>
        internal static string NumberValueDescription {
            get {
                return ResourceManager.GetString("NumberValueDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Shows additional help in your web browser, including example usage..
        /// </summary>
        internal static string OnlineHelpDescription {
            get {
                return ResourceManager.GetString("OnlineHelpDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to https://github.com/SvenGroot/GenerateAnswerFile.
        /// </summary>
        internal static string OnlineHelpUrl {
            get {
                return ResourceManager.GetString("OnlineHelpUrl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [Domain\]User.
        /// </summary>
        internal static string OptionalDomainUserValueDescription {
            get {
                return ResourceManager.GetString("OptionalDomainUserValueDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The organizational unit to use when joining the domain specified by -JoinDomain..
        /// </summary>
        internal static string OUPathDescription {
            get {
                return ResourceManager.GetString("OUPathDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The path and file name to write the answer file to. If not specified, the generated answer file is written to the console..
        /// </summary>
        internal static string OutputFileDescription {
            get {
                return ResourceManager.GetString("OutputFileDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A partition to create on the disk specified by -InstallToDisk. Can have multiple values.
        ///
        ///Use the format &apos;label:size&apos; or &apos;label:size[fs]&apos;, where label is the volume label, size is the size of the partition, and fs is an optional file system like FAT32 or NTFS. Sizes can use multiple-byte units such as GB, and will be truncated to whole megabytes. For example &apos;System:100MB&apos;, &apos;Windows:128GB&apos;, or &apos;Data:16GB[FAT32]&apos;.
        ///
        ///Use &apos;*&apos; as the size to extend the partition to fill the remainder of the disk (e.g. &apos;Windo [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string PartitionsDescription {
            get {
                return ResourceManager.GetString("PartitionsDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Label:Size.
        /// </summary>
        internal static string PartitionsValueDescription {
            get {
                return ResourceManager.GetString("PartitionsValueDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Path.
        /// </summary>
        internal static string PathValueDescription {
            get {
                return ResourceManager.GetString("PathValueDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The processor architecture of the Windows edition you&apos;re installing. Use &apos;amd64&apos; for 64 bit Intel and AMD processors, &apos;x86&apos; for 32 bit, and &apos;arm64&apos; for ARM-based devices..
        /// </summary>
        internal static string ProcessorArchitectureDescription {
            get {
                return ResourceManager.GetString("ProcessorArchitectureDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The product key used to select what edition to install, and to activate Windows..
        /// </summary>
        internal static string ProductKeyDescription {
            get {
                return ResourceManager.GetString("ProductKeyDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The argument &apos;{0}&apos; must be used together with one of: {1}..
        /// </summary>
        internal static string RequiresAnyOtherErrorFormat {
            get {
                return ResourceManager.GetString("RequiresAnyOtherErrorFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Must be used with one of: {0}..
        /// </summary>
        internal static string RequiresAnyOtherUsageHelpFormat {
            get {
                return ResourceManager.GetString("RequiresAnyOtherUsageHelpFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The time zone that Windows will use. Run &apos;tzutil /l&apos; for a list of valid values..
        /// </summary>
        internal static string TimeZoneDescription {
            get {
                return ResourceManager.GetString("TimeZoneDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Options can also be specified by piping a JSON file to the standard input. In this case, only the -OutputFile argument can be used.
        ///
        ///For more information, as well as usage examples, run &apos;{0} -OnlineHelp&apos;, or see: https://github.com/SvenGroot/GenerateAnswerFile.
        /// </summary>
        internal static string UsageHelpFooterFormat {
            get {
                return ResourceManager.GetString("UsageHelpFooterFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Run &apos;{0} -Help&apos; or &apos;{0} -OnlineHelp&apos; for more information, or see: https://github.com/SvenGroot/GenerateAnswerFile.
        /// </summary>
        internal static string UsageHelpMoreInfoFormat {
            get {
                return ResourceManager.GetString("UsageHelpMoreInfoFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The &apos;{0}&apos; argument may only be used if -Install is set to {1}..
        /// </summary>
        internal static string ValidateInstallMethodErrorFormat {
            get {
                return ResourceManager.GetString("ValidateInstallMethodErrorFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to May only be used if -Install is set to {0}..
        /// </summary>
        internal static string ValidateInstallMethodUsageFormat {
            get {
                return ResourceManager.GetString("ValidateInstallMethodUsageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The exact version and build number (e.g. &apos;10.0.22621.1&apos;) of the OS being installed. This argument is only used when -Feature is specified..
        /// </summary>
        internal static string WindowsVersionDescription {
            get {
                return ResourceManager.GetString("WindowsVersionDescription", resourceCulture);
            }
        }
    }
}

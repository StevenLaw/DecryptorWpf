﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Decryptor.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.2.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string Key {
            get {
                return ((string)(this["Key"]));
            }
            set {
                this["Key"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("12")]
        public int BCryptWorkFactor {
            get {
                return ((int)(this["BCryptWorkFactor"]));
            }
            set {
                this["BCryptWorkFactor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
        public byte HashAlgorithm {
            get {
                return ((byte)(this["HashAlgorithm"]));
            }
            set {
                this["HashAlgorithm"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("8")]
        public int Argon2DegreesOfParallelism {
            get {
                return ((int)(this["Argon2DegreesOfParallelism"]));
            }
            set {
                this["Argon2DegreesOfParallelism"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("4")]
        public int Argon2Iterations {
            get {
                return ((int)(this["Argon2Iterations"]));
            }
            set {
                this["Argon2Iterations"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1048576")]
        public int Argon2MemorySize {
            get {
                return ((int)(this["Argon2MemorySize"]));
            }
            set {
                this["Argon2MemorySize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("16384")]
        public int ScryptIterationCount {
            get {
                return ((int)(this["ScryptIterationCount"]));
            }
            set {
                this["ScryptIterationCount"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("8")]
        public int ScryptBlockCount {
            get {
                return ((int)(this["ScryptBlockCount"]));
            }
            set {
                this["ScryptBlockCount"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
        public int ScryptThreadCount {
            get {
                return ((int)(this["ScryptThreadCount"]));
            }
            set {
                this["ScryptThreadCount"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("16")]
        public int Argon2HashLength {
            get {
                return ((int)(this["Argon2HashLength"]));
            }
            set {
                this["Argon2HashLength"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("16")]
        public int Argon2SaltLength {
            get {
                return ((int)(this["Argon2SaltLength"]));
            }
            set {
                this["Argon2SaltLength"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
        public byte EncryptionAlgorithm {
            get {
                return ((byte)(this["EncryptionAlgorithm"]));
            }
            set {
                this["EncryptionAlgorithm"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public byte TripleDesKeySize {
            get {
                return ((byte)(this["TripleDesKeySize"]));
            }
            set {
                this["TripleDesKeySize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("20")]
        public int Pbkdf2HashSize {
            get {
                return ((int)(this["Pbkdf2HashSize"]));
            }
            set {
                this["Pbkdf2HashSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10000")]
        public int Pbkdf2Iterations {
            get {
                return ((int)(this["Pbkdf2Iterations"]));
            }
            set {
                this["Pbkdf2Iterations"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("$PFKDF2$V1$")]
        public string Pbkdf2Prefix {
            get {
                return ((string)(this["Pbkdf2Prefix"]));
            }
            set {
                this["Pbkdf2Prefix"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("16")]
        public int Pbkdf2SaltSize {
            get {
                return ((int)(this["Pbkdf2SaltSize"]));
            }
            set {
                this["Pbkdf2SaltSize"] = value;
            }
        }
    }
}

﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ConsoleApplicationSendAzureServiceBus.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("abeeventhubeventhub")]
        public string EventHubName {
            get {
                return ((string)(this["EventHubName"]));
            }
            set {
                this["EventHubName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Endpoint=sb://abeeventhub.servicebus.windows.net/;SharedAccessKeyName=SharePolicy" +
            ";SharedAccessKey=QGxueNplqFH+VYocUlO1e59WzT61WYVD9/BEJ4yzJcI=")]
        public string EventHubConnectionString {
            get {
                return ((string)(this["EventHubConnectionString"]));
            }
            set {
                this["EventHubConnectionString"] = value;
            }
        }
    }
}
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Compatibility;
using Microsoft.Win32;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data.Linq.SqlClient;
using System.Data.Linq.SqlClient.Implementation;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Management;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;
using TCBase;
using static TCBase.clsRegistry;
using static TCBase.clsSupport;
using static TCBase.clsTCBase;
using static TCBase.clsTrace;
using static TCBase.clsUI;
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------



namespace TCBase.My
{

	[System.Runtime.CompilerServices.CompilerGeneratedAttribute(), System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.6.0.0"), System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
	internal sealed partial class MySettings : global::System.Configuration.ApplicationSettingsBase
	{

		private static MySettings defaultInstance = (MySettings)global::System.Configuration.ApplicationSettingsBase.Synchronized(new MySettings());

		#region "My.Settings Auto-Save Functionality"
		#endregion

		public static MySettings Default {
			get {

				if (!addedHandler) {
					lock (addedHandlerLockObject) {
						if (!addedHandler) {
							MyProject.Application.Shutdown += AutoSaveSettings;
							addedHandler = true;
						}
					}
				}
				return defaultInstance;
			}
		}

		[System.Configuration.ApplicationScopedSettingAttribute(), System.Diagnostics.DebuggerNonUserCodeAttribute(), System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString), System.Configuration.DefaultSettingValueAttribute("Data Source=GGGSCP1;Initial Catalog=TreasureChest;Integrated Security=True")]
		public string ReportConnectionString {
			get { return Convert.ToString(this["ReportConnectionString"]); }
		}
	}
}

namespace TCBase.My
{

	[Microsoft.VisualBasic.HideModuleNameAttribute(), System.Diagnostics.DebuggerNonUserCodeAttribute(), System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
	static internal class MySettingsProperty
	{

		[System.ComponentModel.Design.HelpKeywordAttribute("My.Settings")]
		static internal global::TCBase.My.MySettings Settings {
			get { return global::TCBase.My.MySettings.Default; }
		}
	}
}
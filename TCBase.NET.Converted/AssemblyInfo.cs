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
[assembly: AssemblyTitle("TCBase")]
//<Assembly: CLSCompliant(True)> 
[assembly: Guid("9169BA26-DCD0-4251-BA4D-94F0EFF12CFD")]
namespace TCBase
{
	//Used by Helper Functions to access information from Assembly Attributes
	public class AssemblyInfo
	{
		private System.Reflection.Assembly CurrentAssembly;
		public AssemblyInfo(System.Reflection.Assembly objAssembly = null) : base()
		{
			if ((objAssembly == null)) {
				CurrentAssembly = Assembly.GetEntryAssembly();
				if ((CurrentAssembly == null))
					CurrentAssembly = System.Reflection.Assembly.GetCallingAssembly();
			} else {
				CurrentAssembly = objAssembly;
			}
		}
		#region "Properties"
		public string Name {
			get { return CurrentAssembly.GetName().Name.ToString(); }
		}
		public string FullName {
			get { return CurrentAssembly.GetName().FullName.ToString(); }
		}
		public string CodeBase {
			get { return CurrentAssembly.CodeBase; }
		}
		public string Copyright {
			get {
				Type at = typeof(AssemblyCopyrightAttribute);
				object[] r = CurrentAssembly.GetCustomAttributes(at, false);
				AssemblyCopyrightAttribute ct = (AssemblyCopyrightAttribute)r[0];
				return ct.Copyright;
			}
		}
		public string Company {
			get {
				Type at = typeof(AssemblyCompanyAttribute);
				object[] r = CurrentAssembly.GetCustomAttributes(at, false);
				AssemblyCompanyAttribute ct = (AssemblyCompanyAttribute)r[0];
				return ct.Company;
			}
		}
		public string Description {
			get {
				Type at = typeof(AssemblyDescriptionAttribute);
				object[] r = CurrentAssembly.GetCustomAttributes(at, false);
				AssemblyDescriptionAttribute da = (AssemblyDescriptionAttribute)r[0];
				return da.Description;
			}
		}
		public string Product {
			get {
				Type at = typeof(AssemblyProductAttribute);
				object[] r = CurrentAssembly.GetCustomAttributes(at, false);
				AssemblyProductAttribute pt = (AssemblyProductAttribute)r[0];
				return pt.Product;
			}
		}
		public string Title {
			get {
				Type at = typeof(AssemblyTitleAttribute);
				object[] r = CurrentAssembly.GetCustomAttributes(at, false);
				AssemblyTitleAttribute ta = (AssemblyTitleAttribute)r[0];
				return ta.Title;
			}
		}
		public string Version {
			get { return CurrentAssembly.GetName().Version.ToString(); }
		}
		#endregion
	}
}

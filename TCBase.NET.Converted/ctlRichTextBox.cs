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
//ctlRichTextBox.vb
//   TreasureChest2 Bind-able RichTextBox control...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   02/03/18    Created;
//=================================================================================================================================
//Notes to Self:
//=================================================================================================================================
 // ERROR: Not supported in C#: OptionDeclaration
namespace TCBase
{
	[ClassInterface(ClassInterfaceType.AutoDispatch), DefaultBindingProperty("Rtf"), Description("DescriptionRichTextBox"), ComVisible(true), Docking(DockingBehavior.Ask), Designer("System.Windows.Forms.Design.RichTextBoxDesigner, \tSystem.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public class ctlRichTextBox : System.Windows.Forms.RichTextBox
	{
		public event EventHandler RtfChanged;
		protected override void OnTextChanged(System.EventArgs e)
		{
			base.OnTextChanged(e);
			this.OnRtfChanged(e);
		}
		protected virtual void OnRtfChanged(System.EventArgs e)
		{
			if (RtfChanged != null) {
				RtfChanged(this, e);
			}
		}
		[Bindable(true), RefreshProperties(RefreshProperties.All), SettingsBindable(true), DefaultValue(""), Category("Appearance")]
		public string Rtf {
			get { return base.Rtf; }
			set {
				if (value != null && !object.ReferenceEquals(value, DBNull.Value) && value.StartsWith("{\\rtf", true, System.Globalization.CultureInfo.CurrentCulture)) {
					base.Rtf = value;
				} else {
					base.Text = value;
				}
			}
		}
	}
}

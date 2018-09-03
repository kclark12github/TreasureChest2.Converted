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
//ctlBPEErrorProvider.vb
//   TreasureChest2 Extended ErrorProvider control with OnError Event...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   04/27/12    Created;
//=================================================================================================================================
//Notes to Self:
//=================================================================================================================================
 // ERROR: Not supported in C#: OptionDeclaration
namespace TCBase
{
	public class ctlBPEErrorProvider : System.Windows.Forms.ErrorProvider
	{

		#region " Windows Form Designer generated code "

		public ctlBPEErrorProvider() : base()
		{

			//This call is required by the Windows Form Designer.
			InitializeComponent();

			//Add any initialization after the InitializeComponent() call

		}

		//UserControl overrides dispose to clean up the component list.
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if ((components != null)) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		//Required by the Windows Form Designer

		private System.ComponentModel.IContainer components;
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.  
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}

		#endregion
		public event ErrorEventHandler Error;
		public delegate void ErrorEventHandler(object sender, ctlBPEErrorProviderEventArgs e);
		#region "Events"
		public virtual void OnError(Control Control, string Value)
		{
			//Debug.WriteLine(vbTab & "Raising [Error] event")
			if (Error != null) {
				Error(this, new ctlBPEErrorProviderEventArgs(Control, Value));
			}
		}
		#endregion
		#region "Properties"
		//None at this time...
		#endregion
		#region "Methods"
		public new void SetError(System.Windows.Forms.Control control, string value)
		{
			//Debug.WriteLine(String.Format("SetError({0},""{1}"")", control.Name, value))
			if (value == base.GetError(control))
				return;
			base.SetError(control, value);
			if (value != clsSupport.bpeNullString)
				OnError(control, value);
		}
		#endregion
		#region "Event Handlers"
		//None at this time...
		#endregion
	}
}
namespace TCBase
{
	#region "EventArgs Class(es)"
	public class ctlBPEErrorProviderEventArgs : EventArgs
	{
		private Control mControl = null;
		private string mValue = clsSupport.bpeNullString;
		public string Value {
			get { return mValue; }
		}
		public Control Control {
			get { return mControl; }
		}
		[System.Diagnostics.DebuggerStepThrough()]
		internal ctlBPEErrorProviderEventArgs() : base()
		{
		}
		[System.Diagnostics.DebuggerStepThrough()]
		public ctlBPEErrorProviderEventArgs(Control Control, string Value) : base()
		{
			mControl = Control;
			mValue = Value;
		}
	}
}
#endregion

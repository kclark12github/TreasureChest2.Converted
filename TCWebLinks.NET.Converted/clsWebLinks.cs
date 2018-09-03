using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Compatibility;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TCBase;
using TCBase.clsRegistry;
using TCBase.clsSupport;
using TCBase.clsTrace;
//clsWebLinks.vb
//   Bridge Class Between Application and Component...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   09/22/16    Corrected initialization of niIcon.Icon in InitializeComponent;
//   09/18/16    Reworked to reflect architectural changes;
//   08/07/10    Converted to VB.NET;
//=================================================================================================================================
 // ERROR: Not supported in C#: OptionDeclaration
namespace TCWebLinks
{
	public class clsWebLinks : clsTCBase
	{
		public clsWebLinks(clsSupport objSupport, string ModuleName) : base(objSupport, ModuleName, "WebLinks")
		{

			//This call is required by the Component Designer.
			InitializeComponent();

			//Add any initialization after the InitializeComponent() call

		}
		#region " Component Designer generated code "
		public clsWebLinks(System.ComponentModel.IContainer Container) : this()
		{

			//Required for Windows.Forms Class Composition Designer support
			Container.Add(this);
		}
		public clsWebLinks() : base()
		{

			//This call is required by the Component Designer.
			InitializeComponent();

			//Add any initialization after the InitializeComponent() call

		}
		//Component overrides dispose to clean up the component list.
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if ((components != null)) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		//Required by the Component Designer
		private System.ComponentModel.IContainer components;
		//NOTE: The following procedure is required by the Component Designer
		//It can be modified using the Component Designer.
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(clsWebLinks));
			//
			//niIcon
			//
			base.niIcon.Icon = (System.Drawing.Icon)resources.GetObject("niIcon.Icon");
		}
		#endregion
		#region "Events"
		//None at this time...
		#endregion
		#region "Properties"
		#endregion
		#region "Methods"
		public override void Load(Form objParent, string Caption)
		{
			try {
				OnSplash(Caption, base.niIcon.Icon);
				mParent = objParent;
				mText = Caption;

				OnSplash("Connecting to database...");
				ConnectCommand();

				OnSplash("Loading frmWebLinks...");
				base.ActiveForm = new frmWebLinks(mSupport, this, mParent, mText);
				OnSplash("Complete");
			} finally {
				OnSplash(null);
			}
		}
		#endregion
		#region "Event Handlers"
		//None at this time...
		#endregion
	}
}

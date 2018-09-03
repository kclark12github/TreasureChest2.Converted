//clsCompanies.vb
//   Bridge Class Between Application and Component...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   09/22/16    Corrected initialization of niIcon.Icon in InitializeComponent;
//   09/18/16    Reworked to reflect architectural changes;
//   08/01/10    Converted to VB.NET;
//=================================================================================================================================
 // ERROR: Not supported in C#: OptionDeclaration
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
using static TCBase.clsRegistry;
using static TCBase.clsSupport;
using static TCBase.clsTrace;
namespace TCHobby
{
	public class clsCompanies : clsTCBase
	{
		public clsCompanies(clsSupport objSupport, string ModuleName) : base(objSupport, ModuleName, "Companies")
		{

			//This call is required by the Component Designer.
			InitializeComponent();

			//Add any initialization after the InitializeComponent() call

		}
		#region " Component Designer generated code "

		public clsCompanies(System.ComponentModel.IContainer Container) : this()
		{

			//Required for Windows.Forms Class Composition Designer support
			Container.Add(this);
		}

		public clsCompanies() : base()
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
				dvProductTypes = null;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(clsCompanies));
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
		private DataView dvProductTypes = null;
		public DataView ProductTypes {
			get { return dvProductTypes; }
			set { dvProductTypes = value; }
		}
		#endregion
		#region "Methods"
		public override void Load(Form objParent, string Caption)
		{
			try {
				OnSplash(Caption, base.niIcon.Icon);
				mParent = objParent;
				mText = Caption;
				mReportFileName = "Companies.rpt";

				OnSplash("Connecting to database...");
				ConnectCommand();

				//Need to follow ConnectCommand...
				mTableName = "Companies";
				mTableIDColumn = "ID";
				mTableKeyColumn = "Name";
				SQLFilter = clsSupport.bpeNullString;
                SQLSort = "Code";
                SQLMain = "Select ID, Name, ShortName, Code, Account, ProductType, Phone, Address, WebSite From [Companies] Order By Code;";
				string[] SQL = {
					SQLMain,
					"Select Distinct ProductType From [Companies] Order By ProductType;"
				};
				OnSplash(string.Format("Reading {0} Data...", TableName));
				MainDataSet = OpenDataSet(string.Format("{0}{1}", SQL));
				MainDataView = MainDataSet.Tables[0].DefaultView;
				dvProductTypes = MainDataSet.Tables[1].DefaultView;

				OnSplash("Loading frmCompanies...");
				base.ActiveForm = new frmCompanies(mSupport, this, mParent, mText);
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

//clsClasses.vb
//   Bridge Class Between Application and Component...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   10/21/16    Introduced new logic to defer reading images until the specific record is read;
//   09/22/16    Corrected initialization of niIcon.Icon in InitializeComponent;
//   09/18/16    Reworked to reflect architectural changes;
//   07/25/10    Converted to VB.NET;
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
namespace TCUSNavy
{
	public class clsClasses : clsTCBase
	{
		public clsClasses(clsSupport objSupport, string ModuleName) : base(objSupport, ModuleName, "USN Classes")
		{

			//This call is required by the Component Designer.
			InitializeComponent();

			//Add any initialization after the InitializeComponent() call

		}
		#region " Component Designer generated code "
		public clsClasses(System.ComponentModel.IContainer Container) : this()
		{

			//Required for Windows.Forms Class Composition Designer support
			Container.Add(this);
		}
		public clsClasses() : base()
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
				dvClassifications = null;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(clsClasses));
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
		private DataView dvClassifications = null;
		public DataView Classifications {
			get { return dvClassifications; }
		}
		#endregion
		#region "Methods"
		public override void Load(Form objParent, string Caption)
		{
			try {
				OnSplash(Caption, base.niIcon.Icon);
				mParent = objParent;
				mText = Caption;
				mReportFileName = "USN Ship Classes.rpt";

				OnSplash("Connecting to database...");
				ConnectCommand();

				//Need to follow ConnectCommand...
				mTableName = "Class";
				mTableIDColumn = "ID";
				mTableKeyColumn = "ID";
				SQLFilter = clsSupport.bpeNullString;
                SQLSort = "Classification, Name";
                //SQLMain = "Select ID,Name,Classification,Year,Displacement,Length,Beam,Draft,Propulsion,Boilers,Manning,Speed,Aircraft,Missiles,Guns,[ASW Weapons],Radars,Sonars,[Fire Control],EW,Description,Notes,ClassificationID,Image From [Class] Order By Classification, Name;"
                SQLMain = "Select ID,Name,Classification,Year,Displacement,Length,Beam,Draft,Propulsion,Boilers,Manning,Speed,Aircraft,Missiles,Guns,[ASW Weapons],Radars,Sonars,[Fire Control],EW,Description,Notes,ClassificationID,Convert(image,Null) As [Image] From [Class] Order By Classification, Name;";
				string[] SQL = {
					SQLMain,
					"Select ID,Description,Type From [Classification] Order By Type;"
				};
				OnSplash(string.Format("Reading {0} Data...", TableName));
				MainDataSet = OpenDataSet(string.Format("{0}{1}", SQL));
				MainDataView = MainDataSet.Tables[0].DefaultView;
				dvClassifications = MainDataSet.Tables[1].DefaultView;

                MainDataSet.Tables[mTableName].Columns["Image"].ReadOnly = false;
                //MainDataSet.Tables[mTableName].Columns["OtherImage"].ReadOnly = false;

                OnSplash("Loading frmClasses...");
				base.ActiveForm = new frmClasses(mSupport, this, mParent, mText);
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

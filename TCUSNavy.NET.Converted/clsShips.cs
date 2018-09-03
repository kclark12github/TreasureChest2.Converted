//clsShips.vb
//   Bridge Class Between Application and Component...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   10/21/16    Introduced new logic to defer reading images until the specific record is read;
//   09/22/16    Corrected initialization of niIcon.Icon in InitializeComponent;
//   09/18/16    Reworked to reflect architectural changes;
//   07/28/10    Converted to VB.NET;
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
	public class clsShips : clsTCBase
	{
		public clsShips(clsSupport objSupport, string ModuleName) : base(objSupport, ModuleName, "USN Ships")
		{

			//This call is required by the Component Designer.
			InitializeComponent();

			//Add any initialization after the InitializeComponent() call

		}
		#region " Component Designer generated code "
		public clsShips(System.ComponentModel.IContainer Container) : this()
		{

			//Required for Windows.Forms Class Composition Designer support
			Container.Add(this);
		}
		public clsShips() : base()
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
				dvClasses = null;
				dvClassifications = null;
				dvCommands = null;
				dvHomePorts = null;
				dvStatuses = null;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(clsShips));
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
		private DataView dvClasses = null;
		private DataView dvClassifications = null;
		private DataView dvCommands = null;
		private DataView dvHomePorts = null;
		private DataView dvStatuses = null;
		public DataView Classes {
			get { return dvClasses; }
		}
		public DataView Classifications {
			get { return dvClassifications; }
		}
		public DataView Commands {
			get { return dvCommands; }
		}
		public DataView HomePorts {
			get { return dvHomePorts; }
		}
		public DataView Statuses {
			get { return dvStatuses; }
		}
		#endregion
		#region "Methods"
		public override void Load(Form objParent, string Caption)
		{
			try {
				OnSplash(Caption, base.niIcon.Icon);
				mParent = objParent;
				mText = Caption;
				mReportFileName = "USN Ships.rpt";

				OnSplash("Connecting to database...");
				ConnectCommand();

				//Need to follow ConnectCommand...
				mTableName = "Ships";
				mTableIDColumn = "ID";
				mTableKeyColumn = "HullNumber";
				SQLFilter = clsSupport.bpeNullString;
                SQLSort = "Classification, Number";
                //SQLMain = "Select ID,HullNumber,Number,Name,Classification,ClassID,Commissioned,Status,Command,HomePort,[Zip Code],URL_Internet,URL_Local,Displacement,Length,Beam,Draft,Propulsion,Boilers,Manning,Speed,Aircraft,Missiles,Guns,[ASW Weapons],Radars,Sonars,[Fire Control],EW,History,[More History],Notes,Image,ClassificationID From Ships Order By Classification, Number;"
                SQLMain = "Select ID,HullNumber,Number,Name,Classification,ClassID,Commissioned,Status,Command,HomePort,[Zip Code],URL_Internet,URL_Local,Displacement,Length,Beam,Draft,Propulsion,Boilers,Manning,Speed,Aircraft,Missiles,Guns,[ASW Weapons],Radars,Sonars,[Fire Control],EW,History,[More History],Notes,ClassificationID,Convert(image,Null) As [Image] From Ships Order By Classification, Number;";
				string[] SQL = {
					SQLMain,
					"Select ID,Name,Classification,Year,Displacement,Length,Beam,Draft,Propulsion,Boilers,Manning,Speed,Aircraft,Missiles,Guns,[ASW Weapons],Radars,Sonars,[Fire Control],EW,Description,Notes,Image,ClassificationID From [Class] Order By Name;",
					"Select ID,Description,Type From [Classification] Order By Type;",
					"Select Distinct Status From [Ships] Order By Status;",
					"Select Distinct Command From [Ships] Order By Command;",
					"Select Distinct HomePort From [Ships] Order By HomePort;"
				};
				OnSplash(string.Format("Reading {0} Data...", TableName));
				MainDataSet = OpenDataSet(string.Format("{0}{1}{2}{3}{4}{5}", SQL));
				MainDataView = MainDataSet.Tables[0].DefaultView;
				dvClasses = MainDataSet.Tables[1].DefaultView;
				dvClassifications = MainDataSet.Tables[2].DefaultView;
				dvStatuses = MainDataSet.Tables[3].DefaultView;
				dvCommands = MainDataSet.Tables[4].DefaultView;
				dvHomePorts = MainDataSet.Tables[5].DefaultView;

                MainDataSet.Tables[mTableName].Columns["Image"].ReadOnly = false;
                //MainDataSet.Tables[mTableName].Columns["OtherImage"].ReadOnly = false;

                OnSplash("Loading frmShips...");
				base.ActiveForm = new frmShips(mSupport, this, mParent, mText);
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

//clsCollectables.vb
//   Bridge Class Between Application and Component...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   10/21/16    Introduced new logic to defer reading images until the specific record is read;
//   09/22/16    Corrected initialization of niIcon.Icon in InitializeComponent;
//   09/18/16    Reworked to reflect architectural changes;
//   10/19/14    Upgraded to VS2013;
//   01/04/10    Converted to VB.NET;
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
namespace TCCollectables
{
	public class clsCollectables : clsTCBase
	{
		public clsCollectables(clsSupport objSupport, string ModuleName) : base(objSupport, ModuleName, "Collectables")
		{

			//This call is required by the Component Designer.
			InitializeComponent();

			//Add any initialization after the InitializeComponent() call

		}
		#region " Component Designer generated code "
		public clsCollectables(System.ComponentModel.IContainer Container) : this()
		{

			//Required for Windows.Forms Class Composition Designer support
			Container.Add(this);
		}
		public clsCollectables() : base()
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
				dvConditions = null;
				dvLocations = null;
				dvManufacturers = null;
				dvSeries = null;
				dvTypes = null;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(clsCollectables));
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
		private DataView dvConditions = null;
		private DataView dvLocations = null;
		private DataView dvManufacturers = null;
		private DataView dvSeries = null;
		private DataView dvTypes = null;
		public DataView Conditions {
			get { return dvConditions; }
		}
		public DataView Locations {
			get { return dvLocations; }
		}
		public DataView Manufacturers {
			get { return dvManufacturers; }
		}
		public DataView Series {
			get { return dvSeries; }
		}
		public DataView Types {
			get { return dvTypes; }
		}
		#endregion
		#region "Methods"
		public override void Load(Form objParent, string Caption)
		{
			try {
				OnSplash(Caption, base.niIcon.Icon);
				mParent = objParent;
				mText = Caption;
				mReportFileName = "Collectables.rpt";

				OnSplash("Connecting to database...");
				ConnectCommand();

				//Need to follow ConnectCommand...
				mTableName = "Collectables";
				mTableIDColumn = "ID";
				mTableKeyColumn = "Reference";
				SQLFilter = clsSupport.bpeNullString;
                SQLSort = "Type, Series, Reference, Name, Manufacturer, ID";
                //SQLMain = "Select ID, [Type], Series, Reference, Name, WishList, Manufacturer, Location, DateInventoried, Condition, OutOfProduction, Price, DatePurchased, Value, DateVerified, Image, OtherImage, Notes From Collectables Order By Type, Series, Reference, Name, Manufacturer, ID;"
                SQLMain = "Select ID, [Type], Series, Reference, Name, WishList, Manufacturer, Location, DateInventoried, Condition, OutOfProduction, Price, DatePurchased, Value, DateVerified, Notes, Convert(image,Null) As [Image], Convert(image,Null) As [OtherImage] From Collectables Order By Type, Series, Reference, Name, Manufacturer, ID;";
				string[] SQL = {
					SQLMain,
					"Select Distinct Condition From Collectables Order By Condition;",
					"Select Distinct Location From Collectables Order By Location;",
					"Select Distinct Manufacturer From Collectables Order By Manufacturer;",
					"Select Distinct Series From Collectables Order By Series;",
					"Select Distinct Type From Collectables Order By Type;"
				};
				OnSplash(string.Format("Reading {0} Data...", TableName));
				MainDataSet = OpenDataSet(string.Format("{0}{1}{2}{3}{4}{5}", SQL));
				MainDataView = MainDataSet.Tables[0].DefaultView;
				dvConditions = MainDataSet.Tables[1].DefaultView;
				dvLocations = MainDataSet.Tables[2].DefaultView;
				dvManufacturers = MainDataSet.Tables[3].DefaultView;
				dvSeries = MainDataSet.Tables[4].DefaultView;
				dvTypes = MainDataSet.Tables[5].DefaultView;

                MainDataSet.Tables[mTableName].Columns["Image"].ReadOnly = false;
                MainDataSet.Tables[mTableName].Columns["OtherImage"].ReadOnly = false;

                OnSplash("Loading frmCollectables...");
				base.ActiveForm = new frmCollectables(mSupport, this, mParent, mText);
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

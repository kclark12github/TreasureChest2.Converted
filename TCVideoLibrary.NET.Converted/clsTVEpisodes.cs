//clsTVEpisodes.vb
//   Bridge Class Between Application and Component...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   10/21/16    Introduced new logic to defer reading images until the specific record is read;
//   09/22/16    Corrected initialization of niIcon.Icon in InitializeComponent;
//   09/18/16    Reworked to reflect architectural changes;
//   05/25/15    Added chkWMV;
//   10/02/10    Added Value and DateVerified;
//   01/16/10    Converted to VB.NET;
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
namespace TCVideoLibrary
{
	public class clsTVEpisodes : clsTCBase
	{
		public clsTVEpisodes(clsSupport objSupport, string ModuleName) : base(objSupport, ModuleName, "TV Episodes")
		{

			//This call is required by the Component Designer.
			InitializeComponent();

			//Add any initialization after the InitializeComponent() call

		}
		#region " Component Designer generated code "

		public clsTVEpisodes(System.ComponentModel.IContainer Container) : this()
		{

			//Required for Windows.Forms Class Composition Designer support
			Container.Add(this);
		}

		public clsTVEpisodes() : base()
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
				dvDistributors = null;
				dvFormats = null;
				dvLocations = null;
				dvSeries = null;
				dvSubjects = null;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(clsTVEpisodes));
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
		private DataView dvDistributors = null;
		private DataView dvFormats = null;
		private DataView dvLocations = null;
		private DataView dvSeries = null;
		private DataView dvSubjects = null;
		public DataView Distributors {
			get { return dvDistributors; }
		}
		public DataView Formats {
			get { return dvFormats; }
		}
		public DataView Locations {
			get { return dvLocations; }
		}
		public DataView Series {
			get { return dvSeries; }
		}
		public DataView Subjects {
			get { return dvSubjects; }
		}
		#endregion
		#region "Methods"
		public override void Load(Form objParent, string Caption)
		{
			try {
				OnSplash(Caption, base.niIcon.Icon);
				mParent = objParent;
				mText = Caption;
				mReportFileName = "TVEpisodes.rpt";

				OnSplash("Connecting to database...");
				ConnectCommand();

				//Need to follow ConnectCommand...
				mTableName = "Episodes";
				mTableIDColumn = "ID";
				mTableKeyColumn = "ID";
				SQLFilter = clsSupport.bpeNullString;
                SQLSort = "Series, Number";
                //SQLMain = "Select ID, Subject, Series, Title, Number, WishList, Format, WMV, ReleaseDate, Location, Distributor, DateInventoried, Value, DateVerified, StoreBought, Taped, Price, DatePurchased, Image, OtherImage, Notes From [Episodes] Order By Series, Number;"
                SQLMain = "Select ID, Subject, Series, Title, Number, WishList, Format, WMV, ReleaseDate, Location, Distributor, DateInventoried, Value, DateVerified, StoreBought, Taped, Price, DatePurchased, Notes, Convert(image,Null) As [Image], Convert(image,Null) As [OtherImage] From [Episodes] Order By Series, Number;";
				string[] SQL = {
					SQLMain,
					"Select Distinct Distributor From [Episodes] Order By Distributor;",
					"Select Distinct Location From [Episodes] Order By Location;",
					"Select Distinct Series From [Episodes] Order By Series;",
					"Select Distinct Subject From [Episodes] Order By Subject;",
					"Select Distinct Format From [Episodes] Order By Format;"
				};
				OnSplash(string.Format("Reading {0} Data...", TableName));
				MainDataSet = OpenDataSet(string.Format("{0}{1}{2}{3}{4}{5}", SQL));
				MainDataView = MainDataSet.Tables[0].DefaultView;
				dvDistributors = MainDataSet.Tables[1].DefaultView;
				dvLocations = MainDataSet.Tables[2].DefaultView;
				dvSeries = MainDataSet.Tables[3].DefaultView;
				dvSubjects = MainDataSet.Tables[4].DefaultView;
				dvFormats = MainDataSet.Tables[5].DefaultView;

                MainDataSet.Tables[mTableName].Columns["Image"].ReadOnly = false;
                MainDataSet.Tables[mTableName].Columns["OtherImage"].ReadOnly = false;

				OnSplash("Loading frmTVEpisodes...");
				base.ActiveForm = new frmTVEpisodes(mSupport, this, mParent, mText);
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

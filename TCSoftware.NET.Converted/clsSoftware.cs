//clsSoftware.vb
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
//   05/14/11    Added AlphaSort & OtherImage;
//   10/02/10    Added DateVerified;
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
namespace TCSoftware
{
	public class clsSoftware : clsTCBase
	{
		public clsSoftware(clsSupport objSupport, string ModuleName) : base(objSupport, ModuleName, "Software")
		{

			//This call is required by the Component Designer.
			InitializeComponent();

			//Add any initialization after the InitializeComponent() call

		}
		#region " Component Designer generated code "
		public clsSoftware(System.ComponentModel.IContainer Container) : this()
		{

			//Required for Windows.Forms Class Composition Designer support
			Container.Add(this);
		}
		public clsSoftware() : base()
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
				dvDevelopers = null;
				dvPublishers = null;
				dvPlatforms = null;
				dvMedia = null;
				dvLocations = null;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(clsSoftware));
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
		private DataView dvDevelopers = null;
		private DataView dvLocations = null;
		private DataView dvMedia = null;
		private DataView dvPlatforms = null;
		private DataView dvPublishers = null;
		private DataView dvTypes = null;
		public DataView Developers {
			get { return dvDevelopers; }
		}
		public DataView Locations {
			get { return dvLocations; }
		}
		public DataView Media {
			get { return dvMedia; }
		}
		public DataView Platforms {
			get { return dvPlatforms; }
		}
		public DataView Publishers {
			get { return dvPublishers; }
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

				OnSplash("Connecting to database...");
				ConnectCommand();

				//Need to follow ConnectCommand...
				mTableName = "Software";
				mTableIDColumn = "ID";
				mTableKeyColumn = "Title";
				mReportFileName = "Software.rpt";
				SQLFilter = clsSupport.bpeNullString;
                SQLSort = "AlphaSort";
                //SQLMain = "Select ID, AlphaSort, Type, Title, WishList, Version, Developer, Publisher, ISBN, DateReleased, Value, DateVerified, Platform, Media, Location, DateInventoried, CDkey, Cataloged, Price, DatePurchased, Image, OtherImage, Notes From [Software] Order By AlphaSort;"
                SQLMain = "Select ID, AlphaSort, Type, Title, WishList, Version, Developer, Publisher, ISBN, DateReleased, Value, DateVerified, Platform, Media, Location, DateInventoried, CDkey, Cataloged, Price, DatePurchased, Notes, Convert(image,Null) As [Image], Convert(image,Null) As [OtherImage] From [Software] Order By AlphaSort;";
				string[] SQL = {
					SQLMain,
					"Select Distinct Developer From [Software] Order By Developer;",
					"Select Distinct Publisher From [Software] Order By Publisher;",
					"Select Distinct Platform From [Software] Order By Platform;",
					"Select Distinct Media From [Software] Order By Media;",
					"Select Distinct Location From [Software] Order By Location;",
					"Select Distinct Type From [Software] Order By Type;"
				};
				OnSplash(string.Format("Reading {0} Data...", TableName));
				MainDataSet = OpenDataSet(string.Format("{0}{1}{2}{3}{4}{5}{6}", SQL));
				MainDataView = MainDataSet.Tables[0].DefaultView;
				dvDevelopers = MainDataSet.Tables[1].DefaultView;
				dvPublishers = MainDataSet.Tables[2].DefaultView;
				dvPlatforms = MainDataSet.Tables[3].DefaultView;
				dvMedia = MainDataSet.Tables[4].DefaultView;
				dvLocations = MainDataSet.Tables[5].DefaultView;
				dvTypes = MainDataSet.Tables[6].DefaultView;

                MainDataSet.Tables[mTableName].Columns["Image"].ReadOnly = false;
                MainDataSet.Tables[mTableName].Columns["OtherImage"].ReadOnly = false;

                OnSplash("Loading frmSoftware...");
				base.ActiveForm = new frmSoftware(mSupport, this, mParent, mText);
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

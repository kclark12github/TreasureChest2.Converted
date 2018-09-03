//clsMusic.vb
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
//   10/02/10    Added Value and DateVerified;
//   10/23/09    Converted to VB.NET;
//   06/27/09    Reordered columns in main SQL to display WishList closer to the left (for the List screen);
//   04/13/08    Added DatePurchased support;
//   03/24/09    Added Location DataCombo;
//   02/16/08    Removed BindField call for Notes to handle the migration of Notes field from individual tables into a centralized
//               [Notes] table where the field is read on-demand rather than as part of the vrsMain recordset which over time has
//               come to consume too much memory;
//   08/05/03    Replaced pvCurrency controls with SIASCurrency;
//   10/15/02    Created;
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
namespace TCMusic
{
	public class clsMusic : clsTCBase
	{
		public clsMusic(clsSupport objSupport, string ModuleName) : base(objSupport, ModuleName, "Music")
		{

			//This call is required by the Component Designer.
			InitializeComponent();

			//Add any initialization after the InitializeComponent() call
		}
		#region " Component Designer generated code "
		public clsMusic(System.ComponentModel.IContainer Container) : this()
		{

			//Required for Windows.Forms Class Composition Designer support
			Container.Add(this);
		}
		public clsMusic() : base()
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
				dvArtists = null;
				dvLocations = null;
				dvMedia = null;
				dvTypes = null;
			}
			base.Dispose(disposing);
		}
		//Required by the Component Designer
		private System.ComponentModel.IContainer components;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(clsMusic));
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
		private DataView dvArtists = null;
		private DataView dvLocations = null;
		private DataView dvMedia = null;
		private DataView dvTypes = null;
		public DataView Artists {
			get { return dvArtists; }
		}
		public DataView Locations {
			get { return dvLocations; }
		}
		public DataView Media {
			get { return dvMedia; }
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
				mReportFileName = "Music.rpt";

				OnSplash("Connecting to database...");
				ConnectCommand();

				//Need to follow ConnectCommand...
				mTableName = "Music";
				mTableIDColumn = "ID";
				mTableKeyColumn = "AlphaSort";
				SQLFilter = clsSupport.bpeNullString;
                SQLSort = "AlphaSort";
                SQLMain = "Select ID, AlphaSort, WishList, Artist, Title, Type, Year, Media, Location, DateInventoried, Price, DatePurchased, Value, DateVerified, Notes, Convert(image,Null) As [Image] From Music Order By AlphaSort;";
				string[] SQL = {
					SQLMain,
					"Select Distinct Artist From Music Order By Artist;",
					"Select Distinct Media From Music Order By Media;",
					"Select Distinct Location From Music Order By Location;",
					"Select Distinct Type From Music Order By Type;"
				};
				OnSplash(string.Format("Reading {0} Data...", TableName));
				MainDataSet = OpenDataSet(string.Format("{0}{1}{2}{3}{4}", SQL));
				MainDataView = MainDataSet.Tables[0].DefaultView;
				dvArtists = MainDataSet.Tables[1].DefaultView;
				dvMedia = MainDataSet.Tables[2].DefaultView;
				dvLocations = MainDataSet.Tables[3].DefaultView;
				dvTypes = MainDataSet.Tables[4].DefaultView;

                MainDataSet.Tables[mTableName].Columns["Image"].ReadOnly = false;
                //MainDataSet.Tables[mTableName].Columns["OtherImage"].ReadOnly = false;

                OnSplash("Loading frmMusic...");
				base.ActiveForm = new frmMusic(mSupport, this, mParent, mText);
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

//clsFinishingProducts.vb
//   Bridge Class Between Application and Component...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   10/21/16    Introduced new logic to defer reading images until the specific record is read;
//   09/22/16    Corrected initialization of niIcon.Icon in InitializeComponent;
//   09/18/16    Reworked to reflect architectural changes;
//   10/03/10    Wired-up new fields;
//   10/02/10    Added Value and DateVerified;
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
	public class clsFinishingProducts : clsTCBase
	{
		public clsFinishingProducts(clsSupport objSupport, string ModuleName) : base(objSupport, ModuleName, "Finishing Products")
		{

			//This call is required by the Component Designer.
			InitializeComponent();

			//Add any initialization after the InitializeComponent() call

		}
		#region " Component Designer generated code "
		public clsFinishingProducts(System.ComponentModel.IContainer Container) : this()
		{

			//Required for Windows.Forms Class Composition Designer support
			Container.Add(this);
		}
		public clsFinishingProducts() : base()
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
				dvLocations = null;
				dvManufacturers = null;
				dvProductCatalogs = null;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(clsFinishingProducts));
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
		private DataView dvLocations = null;
		private DataView dvManufacturers = null;
		private DataView dvProductCatalogs = null;
		private DataView dvTypes = null;
		public DataView Locations {
			get { return dvLocations; }
		}
		public DataView Manufacturers {
			get { return dvManufacturers; }
		}
		public DataView ProductCatalogs {
			get { return dvProductCatalogs; }
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
				mReportFileName = "FinishingProducts.rpt";

				OnSplash("Connecting to database...");
				ConnectCommand();

				//Need to follow ConnectCommand...
				mTableName = "Finishing Products";
				mTableIDColumn = "ID";
				mTableKeyColumn = "Reference";
				SQLFilter = clsSupport.bpeNullString;
                SQLSort = "Type, Name";
                //SQLMain = "Select ID, Name, Manufacturer, ProductCatalog, Reference, WishList, Type, Count, Location, DateInventoried, Price, DatePurchased, Value, DateVerified, Notes, Image From [Finishing Products] Order By Type,Name;"
                SQLMain = "Select ID, Name, Manufacturer, ProductCatalog, Reference, WishList, Type, Count, Location, DateInventoried, Price, DatePurchased, Value, DateVerified, Notes, Convert(image,Null) As [Image] From [Finishing Products] Order By Type,Name;";
				string[] SQL = {
					SQLMain,
					"Select Distinct Manufacturer From [Finishing Products] Order By Manufacturer;",
					"Select Distinct ProductCatalog From [Finishing Products] Order By ProductCatalog;",
					"Select Distinct Type From [Finishing Products] Order By Type;",
					"Select Distinct Location From [Finishing Products] Order By Location;"
				};
				OnSplash(string.Format("Reading {0} Data...", TableName));
				MainDataSet = OpenDataSet(string.Format("{0}{1}{2}{3}{4}", SQL));
				MainDataView = MainDataSet.Tables[0].DefaultView;
				dvManufacturers = MainDataSet.Tables[1].DefaultView;
				dvProductCatalogs = MainDataSet.Tables[2].DefaultView;
				dvTypes = MainDataSet.Tables[3].DefaultView;
				dvLocations = MainDataSet.Tables[4].DefaultView;

                MainDataSet.Tables[mTableName].Columns["Image"].ReadOnly = false;
                //MainDataSet.Tables[mTableName].Columns["OtherImage"].ReadOnly = false;

                OnSplash("Loading frmFinishingProducts...");
				base.ActiveForm = new frmFinishingProducts(mSupport, this, mParent, mText);
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

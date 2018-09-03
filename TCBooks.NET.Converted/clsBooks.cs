//clsBooks.vb
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
namespace TCBooks
{
	public class clsBooks : clsTCBase
	{
		public clsBooks(clsSupport objSupport, string ModuleName) : base(objSupport, ModuleName, "Books")
		{

			//This call is required by the Component Designer.
			InitializeComponent();

			//Add any initialization after the InitializeComponent() call

		}
		#region " Component Designer generated code "
		public clsBooks(System.ComponentModel.IContainer Container) : this()
		{

			//Required for Windows.Forms Class Composition Designer support
			Container.Add(this);
		}
		public clsBooks() : base()
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
				dvAuthors = null;
				dvLocations = null;
				dvMedia = null;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(clsBooks));
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
		private DataView dvAuthors = null;
		private DataView dvFormats = null;
		private DataView dvLocations = null;
		private DataView dvMedia = null;
		private DataView dvSubjects = null;
		public int AlphasortMaxLength {
			get { return MainDataSet.Tables[0].Columns["Alphasort"].MaxLength; }
		}
		public DataView Authors {
			get { return dvAuthors; }
		}
		public DataView Formats {
			get { return dvFormats; }
		}
		public DataView Locations {
			get { return dvLocations; }
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

				OnSplash("Connecting to database...");
				ConnectCommand();

				//Need to follow ConnectCommand...
				mTableName = "Books";
				mTableIDColumn = "ID";
				mTableKeyColumn = "Alphasort";
				mReportFileName = "Books.rpt";
				SQLFilter = clsSupport.bpeNullString;
                SQLSort = "Alphasort";
                //SQLMain = "Select ID, Alphasort, WishList, Title, Author, ISBN, Cataloged, Subject, Location, DateInventoried, Value, DateVerified, Price, DatePurchased, Misc, Notes, Image, OtherImage From [Books] Order By Alphasort;"
                SQLMain = "Select ID, Alphasort, WishList, Title, Author, ISBN, Cataloged, Subject, Location, DateInventoried, Value, DateVerified, Price, DatePurchased, Misc, Notes, Convert(image,Null) As [Image], Convert(image,Null) As [OtherImage], [Format] From [Books] Order By Alphasort;";
				string[] SQL = {
					SQLMain,
					"Select Distinct Author From [Books] Order By Author;",
					"Select Distinct Location From [Books] Order By Location;",
					"Select Distinct Subject From [Books] Order By Subject;",
					"Select Distinct [Format] From [Books] Order By [Format];"
				};
				OnSplash(string.Format("Reading {0} Data...", TableName));
				MainDataSet = OpenDataSet(string.Format("{0}{1}{2}{3}{4}", SQL), false, true);
				MainDataView = MainDataSet.Tables[0].DefaultView;
				dvAuthors = MainDataSet.Tables[1].DefaultView;
				dvLocations = MainDataSet.Tables[2].DefaultView;
				dvSubjects = MainDataSet.Tables[3].DefaultView;
				dvFormats = MainDataSet.Tables[4].DefaultView;

                MainDataSet.Tables[mTableName].Columns["Image"].ReadOnly = false;
                MainDataSet.Tables[mTableName].Columns["OtherImage"].ReadOnly = false;

				OnSplash("Loading frmBooks...");
				base.ActiveForm = new frmBooks(mSupport, this, mParent, mText);
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

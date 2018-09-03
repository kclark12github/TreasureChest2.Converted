//clsKits.vb
//   Bridge Class Between Application and Component...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   07/20/18    Added ID to [Detail Set] / [Decals] query;
//   06/25/18    Introduced more thorough handling of Decals and Detail Set related records;
//   06/07/18    Replaced DeleteInProcess event-handler with BeforeDelete;
//   04/22/18    Introduced functionality to link Decals and Detail Sets to a Kit enabling separate Location tracking;
//   10/21/16    Introduced new logic to defer reading images until the specific record is read;
//   09/22/16    Corrected initialization of niIcon.Icon in InitializeComponent;
//   09/18/16    Reworked to reflect architectural changes;
//   10/03/10    Wired-up new fields;
//   10/02/10    Added Value;
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
using System.Data.SqlClient;
using System.Text.RegularExpressions;
namespace TCHobby
{

	public class clsKits : clsTCBase
	{
		public clsKits(clsSupport objSupport, string ModuleName) : base(objSupport, ModuleName, "Kits")
		{
			UpdateInProcess += clsKits_UpdateInProcess;
			UpdateComplete += clsKits_UpdateComplete;
			BeforeDelete += clsKits_BeforeDelete;
			DeleteComplete += clsKits_DeleteComplete;
			AddInProcess += clsKits_AddInProcess;
			AddComplete += clsKits_AddComplete;

			//This call is required by the Component Designer.
			InitializeComponent();

			//Add any initialization after the InitializeComponent() call

		}
		#region " Component Designer generated code "
		public clsKits(System.ComponentModel.IContainer Container) : this()
		{

			//Required for Windows.Forms Class Composition Designer support
			Container.Add(this);
		}
		public clsKits() : base()
		{
			UpdateInProcess += clsKits_UpdateInProcess;
			UpdateComplete += clsKits_UpdateComplete;
			BeforeDelete += clsKits_BeforeDelete;
			DeleteComplete += clsKits_DeleteComplete;
			AddInProcess += clsKits_AddInProcess;
			AddComplete += clsKits_AddComplete;

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
				dvCatalogs = null;
				dvConditions = null;
				dvEras = null;
				dvLocations = null;
				dvManufacturers = null;
				dvNations = null;
				dvScales = null;
				dvServices = null;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(clsKits));
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
		private DataView dvCatalogs = null;
		private DataView dvConditions = null;
		private DataView dvDecals = null;
		private DataView dvDetailSets = null;
		private DataView dvEras = null;
		private DataView dvLocations = null;
		private DataView dvManufacturers = null;
		private DataView dvNations = null;
		private DataView dvScales = null;
		private DataView dvServices = null;
		private DataView dvTypes = null;
		public DataView Catalogs {
			get { return dvCatalogs; }
		}
		public DataView Conditions {
			get { return dvConditions; }
		}
		public DataView Eras {
			get { return dvEras; }
		}
		public DataView Locations {
			get { return dvLocations; }
		}
		public DataView Manufacturers {
			get { return dvManufacturers; }
		}
		public DataView Nations {
			get { return dvNations; }
		}
		public DataView Scales {
			get { return dvScales; }
		}
		public DataView Services {
			get { return dvServices; }
		}
		public DataView Types {
			get { return dvTypes; }
		}
		public DataView Decals {
			get { return dvDecals; }
		}
		public DataView DetailSets {
			get { return dvDetailSets; }
		}
		#endregion
		#region "Methods"
		public override void Load(Form objParent, string Caption)
		{
			try {
				OnSplash(Caption, base.niIcon.Icon);
				mParent = objParent;
				mText = Caption;
				mReportFileName = "Kits.rpt";

				OnSplash("Connecting to database...");
				ConnectCommand();

				//Need to follow ConnectCommand...
				mTableName = "Kits";
				mTableIDColumn = "ID";
				mTableKeyColumn = "Name";
				SQLFilter = clsSupport.bpeNullString;
                SQLSort = "Type, Scale, Manufacturer, Designation, Name, Reference, ID";
                //SQLMain = "Select ID, Type, Scale, Manufacturer, Reference, Designation, Name, WishList, ProductCatalog, OutOfProduction, Nation, Service, Era, Condition, Location, DateInventoried, Price, DatePurchased, Value, DateVerified, Image, OtherImage, Notes From [Kits] Order By Type, Scale, Manufacturer, Designation, Name, Reference, ID;"
                SQLMain = "Select ID, Type, Scale, Manufacturer, Reference, Designation, Name, WishList, ProductCatalog, OutOfProduction, Nation, Service, Era, Condition, Location, DateInventoried, Price, DatePurchased, Value, DateVerified, Notes, Convert(image,Null) As [Image], Convert(image,Null) As [OtherImage], DecalID, HasDecals, DetailSetID, HasDetailSet From [Kits] Order By Type, Scale, Manufacturer, Designation, Name, Reference, ID;";
				string[] SQL = {
					SQLMain,
					"Select Distinct Type From [Kits] Order By Type;",
					"Select Distinct Manufacturer From [Kits] Order By Manufacturer;",
					"Select Distinct ProductCatalog From [Kits] Order By ProductCatalog;",
					"Select Distinct Scale From [Kits] Order By Scale;",
					"Select Distinct Nation From [Kits] Order By Nation;",
					"Select Distinct Condition From [Kits] Order By Condition;",
					"Select Distinct Location From [Kits] Order By Location;",
					"Select Distinct Service From [Kits] Order By Service;",
					"Select Distinct Era From [Kits] Order By Era;"
				};
				OnSplash(string.Format("Reading {0} Data...", TableName));
				MainDataSet = OpenDataSet(string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}", SQL));
				MainDataView = MainDataSet.Tables[0].DefaultView;
				dvTypes = MainDataSet.Tables[1].DefaultView;
				dvManufacturers = MainDataSet.Tables[2].DefaultView;
				dvCatalogs = MainDataSet.Tables[3].DefaultView;
				dvScales = MainDataSet.Tables[4].DefaultView;
				dvNations = MainDataSet.Tables[5].DefaultView;
				dvConditions = MainDataSet.Tables[6].DefaultView;
				dvLocations = MainDataSet.Tables[7].DefaultView;
				dvServices = MainDataSet.Tables[8].DefaultView;
				dvEras = MainDataSet.Tables[9].DefaultView;

				var _with1 = MainDataSet.Tables[mTableName];
				_with1.Columns["Image"].ReadOnly = false;
				_with1.Columns["OtherImage"].ReadOnly = false;

				OnSplash("Loading frmKits...");
				base.ActiveForm = new frmKits(mSupport, this, mParent, mText);
				OnSplash("Complete");
			} finally {
				OnSplash(null);
			}
		}
		internal void AddRelated(string TableName, DataEventArgs e)
		{
			DataView dv = OpenDataView(string.Format("Select * From [{0}] Where 1=0;", TableName));
            int RecordsAffected = 0;
			int ID = (int)ExecuteScalarCommand(string.Format("Select MAX(ID)+1 From [{0}];", TableName));
            switch (TableName) {
				case "Decals":
					string SQLSource = string.Format("Insert Into [{0}] ([ID],[Name],[Scale],[Manufacturer],[ProductCatalog],[Reference],[Price],[Type],[Nation],[DateInventoried],[WishList],[Location],[DatePurchased],[Image],[OtherImage],[Value],[DateVerified],[Notes],[KitID],[Designation]) Values (@ID,@Name,@Scale,@Manufacturer,@ProductCatalog,@Reference,@Price,@Type,@Nation,@DateInventoried,@WishList,@Location,@DatePurchased,@Image,@OtherImage,@Value,@DateVerified,@Notes,@KitID,@Designation)", TableName);
					var _with2 = tcDataAdapter.InsertCommand;
					_with2.CommandText = SQLSource;
					_with2.CommandType = CommandType.Text;
					//Now for each column we intend to update, create and initialize its parameter...
					_with2.Parameters.Clear();
					_with2.Parameters.Add(GetNewSqlParameter("@ID", dv.Table.Columns["ID"], ID));
					_with2.Parameters.Add(GetNewSqlParameter("@Name", dv.Table.Columns["Name"], e.RowView["Name"]));
					_with2.Parameters.Add(GetNewSqlParameter("@Scale", dv.Table.Columns["Scale"], e.RowView["Scale"]));
					_with2.Parameters.Add(GetNewSqlParameter("@Manufacturer", dv.Table.Columns["Manufacturer"], e.RowView["Manufacturer"]));
					_with2.Parameters.Add(GetNewSqlParameter("@ProductCatalog", dv.Table.Columns["ProductCatalog"], e.RowView["ProductCatalog"]));
					_with2.Parameters.Add(GetNewSqlParameter("@Reference", dv.Table.Columns["Reference"], e.RowView["Reference"]));
					_with2.Parameters.Add(GetNewSqlParameter("@Price", dv.Table.Columns["Price"], 0));
					_with2.Parameters.Add(GetNewSqlParameter("@Type", dv.Table.Columns["Type"], e.RowView["Type"]));
					_with2.Parameters.Add(GetNewSqlParameter("@Nation", dv.Table.Columns["Nation"], e.RowView["Nation"]));
					_with2.Parameters.Add(GetNewSqlParameter("@DateInventoried", dv.Table.Columns["DateInventoried"], e.RowView["DateInventoried"]));
					_with2.Parameters.Add(GetNewSqlParameter("@WishList", dv.Table.Columns["WishList"], e.RowView["WishList"]));
					_with2.Parameters.Add(GetNewSqlParameter("@Location", dv.Table.Columns["Location"], "Included in kit"));
					_with2.Parameters.Add(GetNewSqlParameter("@DatePurchased", dv.Table.Columns["DatePurchased"], e.RowView["DatePurchased"]));
					_with2.Parameters.Add(GetNewSqlParameter("@Image", dv.Table.Columns["Image"], e.RowView["Image"]));
					_with2.Parameters.Add(GetNewSqlParameter("@OtherImage", dv.Table.Columns["OtherImage"], e.RowView["OtherImage"]));
					_with2.Parameters.Add(GetNewSqlParameter("@Value", dv.Table.Columns["Value"], 0));
					_with2.Parameters.Add(GetNewSqlParameter("@DateVerified", dv.Table.Columns["DateVerified"], e.RowView["DateVerified"]));
					_with2.Parameters.Add(GetNewSqlParameter("@Notes", dv.Table.Columns["Notes"], e.RowView["Notes"]));
					_with2.Parameters.Add(GetNewSqlParameter("@KitID", dv.Table.Columns["KitID"], e.RowView["ID"]));
					_with2.Parameters.Add(GetNewSqlParameter("@Designation", dv.Table.Columns["Designation"], e.RowView["Designation"]));
					_with2.UpdatedRowSource = UpdateRowSource.None;
					//Use the DataAdapter (through the InsertCommand we just set up) to update the database...
					LogSQL(tcDataAdapter.InsertCommand);
					_with2.ExecuteNonQuery();
					ExecuteCommand(string.Format("Update [{0}] Set DecalID={1} Where [ID]={2};", "Kits", ID, e.RowView["ID"]), ref RecordsAffected);
					break;
				case "Detail Sets":
					SQLSource = string.Format("Insert Into [{0}] ([ID],[Name],[Scale],[Manufacturer],[ProductCatalog],[Reference],[Price],[Type],[Nation],[DateInventoried],[WishList],[Location],[DatePurchased],[Image],[OtherImage],[Value],[DateVerified],[Notes],[KitID],[Designation]) Values (@ID,@Name,@Scale,@Manufacturer,@ProductCatalog,@Reference,@Price,@Type,@Nation,@DateInventoried,@WishList,@Location,@DatePurchased,@Image,@OtherImage,@Value,@DateVerified,@Notes,@KitID,@Designation)", TableName);
					var _with3 = tcDataAdapter.InsertCommand;
					_with3.CommandText = SQLSource;
					_with3.CommandType = CommandType.Text;
					//Now for each column we intend to update, create and initialize its parameter...
					_with3.Parameters.Clear();
					_with3.Parameters.Add(GetNewSqlParameter("@ID", dv.Table.Columns["ID"], ID));
					_with3.Parameters.Add(GetNewSqlParameter("@Name", dv.Table.Columns["Name"], e.RowView["Name"]));
					_with3.Parameters.Add(GetNewSqlParameter("@Scale", dv.Table.Columns["Scale"], e.RowView["Scale"]));
					_with3.Parameters.Add(GetNewSqlParameter("@Manufacturer", dv.Table.Columns["Manufacturer"], e.RowView["Manufacturer"]));
					_with3.Parameters.Add(GetNewSqlParameter("@ProductCatalog", dv.Table.Columns["ProductCatalog"], e.RowView["ProductCatalog"]));
					_with3.Parameters.Add(GetNewSqlParameter("@Reference", dv.Table.Columns["Reference"], e.RowView["Reference"]));
					_with3.Parameters.Add(GetNewSqlParameter("@Price", dv.Table.Columns["Price"], 0));
					_with3.Parameters.Add(GetNewSqlParameter("@Type", dv.Table.Columns["Type"], e.RowView["Type"]));
					_with3.Parameters.Add(GetNewSqlParameter("@Nation", dv.Table.Columns["Nation"], e.RowView["Nation"]));
					_with3.Parameters.Add(GetNewSqlParameter("@DateInventoried", dv.Table.Columns["DateInventoried"], e.RowView["DateInventoried"]));
					_with3.Parameters.Add(GetNewSqlParameter("@WishList", dv.Table.Columns["WishList"], e.RowView["WishList"]));
					_with3.Parameters.Add(GetNewSqlParameter("@Location", dv.Table.Columns["Location"], "Included in kit"));
					_with3.Parameters.Add(GetNewSqlParameter("@DatePurchased", dv.Table.Columns["DatePurchased"], e.RowView["DatePurchased"]));
					_with3.Parameters.Add(GetNewSqlParameter("@Image", dv.Table.Columns["Image"], e.RowView["Image"]));
					_with3.Parameters.Add(GetNewSqlParameter("@OtherImage", dv.Table.Columns["OtherImage"], e.RowView["OtherImage"]));
					_with3.Parameters.Add(GetNewSqlParameter("@Value", dv.Table.Columns["Value"], 0));
					_with3.Parameters.Add(GetNewSqlParameter("@DateVerified", dv.Table.Columns["DateVerified"], e.RowView["DateVerified"]));
					_with3.Parameters.Add(GetNewSqlParameter("@Notes", dv.Table.Columns["Notes"], e.RowView["Notes"]));
					_with3.Parameters.Add(GetNewSqlParameter("@KitID", dv.Table.Columns["KitID"], e.RowView["ID"]));
					_with3.Parameters.Add(GetNewSqlParameter("@Designation", dv.Table.Columns["Designation"], e.RowView["Designation"]));
					_with3.UpdatedRowSource = UpdateRowSource.None;
					//Use the DataAdapter (through the InsertCommand we just set up) to update the database...
					LogSQL(tcDataAdapter.InsertCommand);
					_with3.ExecuteNonQuery();
					ExecuteCommand(string.Format("Update [{0}] Set DetailSetID={1} Where [ID]={2};", "Kits", ID, e.RowView["ID"]), ref RecordsAffected);
					break;
			}
			CloseDataView(ref dv);
			dv = null;
		}
		internal void DeleteRelated(string TableName, DataEventArgs e)
		{
            DataView dv = null;
            string SQLSource = bpeNullString;
            switch (TableName) {
				case "Decals":
					dv = (DataView)OpenDataView(string.Format("Select * From [{0}] Where [ID]={1};", TableName, e.RowView["DecalID"]));
					if (dv.Count > 0) {
						SQLSource = string.Format("Delete From [{0}] Where [ID]=@ID;", TableName);
						var _with4 = tcDataAdapter.DeleteCommand;
						_with4.CommandText = SQLSource;
						_with4.CommandType = CommandType.Text;
						//Now for each column we intend to update, create and initialize its parameter...
						_with4.Parameters.Clear();
						_with4.Parameters.Add(GetNewSqlParameter("@ID", dv.Table.Columns["ID"], e.RowView["DecalID"]));
						_with4.UpdatedRowSource = UpdateRowSource.None;
						//Use the DataAdapter (through the DeleteCommand we just set up) to update the database...
						LogSQL(tcDataAdapter.DeleteCommand);
						_with4.ExecuteNonQuery();
					}
					CloseDataView(ref dv);
					dv = null;
					break;
				case "Detail Sets":
					dv = (DataView)OpenDataView(string.Format("Select * From [{0}] Where [ID]={1};", TableName, e.RowView["DetailSetID"]));
					if (dv.Count > 0) {
						SQLSource = string.Format("Delete From [{0}] Where [ID]=@ID;", TableName);
						var _with5 = tcDataAdapter.UpdateCommand;
						_with5.CommandText = SQLSource;
						_with5.CommandType = CommandType.Text;
						//Now for each column we intend to update, create and initialize its parameter...
						_with5.Parameters.Clear();
						_with5.Parameters.Add(GetNewSqlParameter("@ID", dv.Table.Columns["ID"], e.RowView["DetailSetID"]));
						_with5.UpdatedRowSource = UpdateRowSource.None;
						//Use the DataAdapter (through the DeleteCommand we just set up) to update the database...
						LogSQL(tcDataAdapter.DeleteCommand);
						_with5.ExecuteNonQuery();
					}
					CloseDataView(ref dv);
					dv = null;
					break;
			}
		}
		internal void PopulateRelated(DataRowView drv)
		{
			string SQLSource = clsSupport.bpeNullString;
			string SQLDesignation = "And (UPPER(Designation)='ANY' Or ";
			string Designation = (string)drv["Designation"];
			string[] Designations = { (string)drv["Designation"] };
			if (Designation.Contains("&")) {
				Designations = Designation.Split("&".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			} else if (Designation.Contains(",")) {
				Designations = Designation.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			}
			for (int iDes = 0; iDes <= Designations.Length - 1; iDes++) {
				Designations[iDes] = Designations[iDes].Trim();
				switch (Designations[iDes]) {
					case "UNKNOWN":
					case "N/A":
						break;
					default:
						//Use RegEx to decide whether to wildcard the Designation...
						switch (drv["Type"]) {
							case "Aircraft":
								//TODO: Make this a database parameter so we can make modifications without rebuilding...
								string Pattern = "(?<us>(?<modern>(?<base>(?<type>([A-Z])|([A-Z][//][A-Z])|([A-Z]{2}))-(?<number>[0-9]+))(?<subtype_delimiter>[ ]?)(?<subtype>[A-Z//0-9]*))(?<rest>[ A-Za-z//0-9()]*)|(?<legacy>(?<base>(?<type>[A-Z]+)(?<number>[0-9]*)(?<manufacturer>[A-Z]))(?<subtype_delimiter>[-]*)(?<subtype>[A-Z//0-9]*)))|(?<ussr>(?<base>(?<manufacturer>Ka|Mi|MiG|S|Su|Tu|T|Yak)-(?<number>[0-9]+))(?<subtype_delimiter>[ ]?)(?<subtype>[A-Z//0-9]*))(?<rest>[ A-Za-z//0-9()]*)|(?<japan>(?<base>(?<manufacturer>[A-Z]{1})(?<number>[0-9]+))(?<subtype_delimiter>[ ]?)(?<subtype>[A-Z//0-9]*))(?<rest>[ A-Za-z//0-9()]*)";
								Regex regExp = new Regex(Pattern, RegexOptions.Compiled);
								MatchCollection mc = regExp.Matches(Designations[iDes]);
								string @base = clsSupport.bpeNullString;
								foreach (Match m in mc) {
									@base = m.Groups["base"].Value;
								}

								if (@base != clsSupport.bpeNullString)
									Designations[iDes] = string.Format("{0}%", @base);
								break;
						}
						break;
				}
				SQLDesignation += string.Format("Designation Like '{0}' Or ", FixQuotes(Designations[iDes]));
			}
			SQLDesignation = SQLDesignation.Substring(0, SQLDesignation.Length - 4) + ")";
			SQLSource = string.Format("Select ID,Manufacturer,Reference,Designation,Name,WishList,Location,KitID,Convert(bit,Case When KitID={0} Then 1 Else 0 End) As [This Kit] From [<table-name>] Where KitID={0} Or (Type='{1}' And Scale='{2}' {3}) Order By Manufacturer,Reference,Name;", new object[] {
				drv["ID"],
				drv["Type"],
				drv["Scale"],
				SQLDesignation
			});

			if (dvDecals != null)
				CloseDataView(ref dvDecals);
			dvDecals = OpenDataView(SQLSource.Replace("<table-name>", "Decals"));

			if (dvDetailSets != null)
				CloseDataView(ref dvDetailSets);
			dvDetailSets = OpenDataView(SQLSource.Replace("<table-name>", "Detail Sets"));
		}
		internal void UpdateRelated(string TableName, DataEventArgs e)
		{
			switch (TableName) {
				case "Decals":
					DataView dv = OpenDataView(string.Format("Select * From [{0}] Where [ID]={1};", TableName, e.RowView["DecalID"]));
					string SQLSource = string.Format("Update [{0}] Set [Name]=@Name,[Scale]=@Scale,[Manufacturer]=@Manufacturer,[ProductCatalog]=@ProductCatalog,[Reference]=@Reference,[Type]=@Type,[Nation]=@Nation,[DateInventoried]=@DateInventoried,[WishList]=@WishList,[DatePurchased]=@DatePurchased,[DateVerified]=@DateVerified,[Designation]=@Designation Where [ID]=@ID;", TableName);
					var _with6 = tcDataAdapter.UpdateCommand;
					_with6.CommandText = SQLSource;
					_with6.CommandType = CommandType.Text;
					//Now for each column we intend to update, create and initialize its parameter...
					_with6.Parameters.Clear();
					_with6.Parameters.Add(GetNewSqlParameter("@ID", dv.Table.Columns["ID"], e.RowView["DecalID"]));
					_with6.Parameters.Add(GetNewSqlParameter("@Name", dv.Table.Columns["Name"], e.RowView["Name"]));
					_with6.Parameters.Add(GetNewSqlParameter("@Scale", dv.Table.Columns["Scale"], e.RowView["Scale"]));
					_with6.Parameters.Add(GetNewSqlParameter("@Manufacturer", dv.Table.Columns["Manufacturer"], e.RowView["Manufacturer"]));
					_with6.Parameters.Add(GetNewSqlParameter("@ProductCatalog", dv.Table.Columns["ProductCatalog"], e.RowView["ProductCatalog"]));
					_with6.Parameters.Add(GetNewSqlParameter("@Reference", dv.Table.Columns["Reference"], e.RowView["Reference"]));
					_with6.Parameters.Add(GetNewSqlParameter("@Type", dv.Table.Columns["Type"], e.RowView["Type"]));
					_with6.Parameters.Add(GetNewSqlParameter("@Nation", dv.Table.Columns["Nation"], e.RowView["Nation"]));
					_with6.Parameters.Add(GetNewSqlParameter("@DateInventoried", dv.Table.Columns["DateInventoried"], e.RowView["DateInventoried"]));
					_with6.Parameters.Add(GetNewSqlParameter("@WishList", dv.Table.Columns["WishList"], e.RowView["WishList"]));
					//.Parameters.Add(GetNewSqlParameter("@Location", dv.Table.Columns("Location"), "Included in kit"))
					_with6.Parameters.Add(GetNewSqlParameter("@DatePurchased", dv.Table.Columns["DatePurchased"], e.RowView["DatePurchased"]));
					_with6.Parameters.Add(GetNewSqlParameter("@DateVerified", dv.Table.Columns["DateVerified"], e.RowView["DateVerified"]));
					_with6.Parameters.Add(GetNewSqlParameter("@Designation", dv.Table.Columns["Designation"], e.RowView["Designation"]));
					_with6.UpdatedRowSource = UpdateRowSource.None;
					//Use the DataAdapter (through the UpdateCommand we just set up) to update the database...
					LogSQL(tcDataAdapter.UpdateCommand);
					_with6.ExecuteNonQuery();
					CloseDataView(ref dv);
					dv = null;
					break;
				case "Detail Sets":
					dv = (DataView)OpenDataView(string.Format("Select * From [{0}] Where [ID]={1};", TableName, e.RowView["DetailSetID"]));
					SQLSource = string.Format("Update [{0}] Set [Name]=@Name,[Scale]=@Scale,[Manufacturer]=@Manufacturer,[ProductCatalog]=@ProductCatalog,[Reference]=@Reference,[Type]=@Type,[Nation]=@Nation,[DateInventoried]=@DateInventoried,[WishList]=@WishList,[DatePurchased]=@DatePurchased,[DateVerified]=@DateVerified,[Designation]=@Designation Where [ID]=@ID;", TableName);
					var _with7 = tcDataAdapter.UpdateCommand;
					_with7.CommandText = SQLSource;
					_with7.CommandType = CommandType.Text;
					//Now for each column we intend to update, create and initialize its parameter...
					_with7.Parameters.Clear();
					_with7.Parameters.Add(GetNewSqlParameter("@ID", dv.Table.Columns["ID"], e.RowView["DetailSetID"]));
					_with7.Parameters.Add(GetNewSqlParameter("@Name", dv.Table.Columns["Name"], e.RowView["Name"]));
					_with7.Parameters.Add(GetNewSqlParameter("@Scale", dv.Table.Columns["Scale"], e.RowView["Scale"]));
					_with7.Parameters.Add(GetNewSqlParameter("@Manufacturer", dv.Table.Columns["Manufacturer"], e.RowView["Manufacturer"]));
					_with7.Parameters.Add(GetNewSqlParameter("@ProductCatalog", dv.Table.Columns["ProductCatalog"], e.RowView["ProductCatalog"]));
					_with7.Parameters.Add(GetNewSqlParameter("@Reference", dv.Table.Columns["Reference"], e.RowView["Reference"]));
					_with7.Parameters.Add(GetNewSqlParameter("@Type", dv.Table.Columns["Type"], e.RowView["Type"]));
					_with7.Parameters.Add(GetNewSqlParameter("@Nation", dv.Table.Columns["Nation"], e.RowView["Nation"]));
					_with7.Parameters.Add(GetNewSqlParameter("@DateInventoried", dv.Table.Columns["DateInventoried"], e.RowView["DateInventoried"]));
					_with7.Parameters.Add(GetNewSqlParameter("@WishList", dv.Table.Columns["WishList"], e.RowView["WishList"]));
					//.Parameters.Add(GetNewSqlParameter("@Location", dv.Table.Columns("Location"), "Included in kit"))
					_with7.Parameters.Add(GetNewSqlParameter("@DatePurchased", dv.Table.Columns["DatePurchased"], e.RowView["DatePurchased"]));
					_with7.Parameters.Add(GetNewSqlParameter("@DateVerified", dv.Table.Columns["DateVerified"], e.RowView["DateVerified"]));
					_with7.Parameters.Add(GetNewSqlParameter("@Designation", dv.Table.Columns["Designation"], e.RowView["Designation"]));
					_with7.UpdatedRowSource = UpdateRowSource.None;
					//Use the DataAdapter (through the UpdateCommand we just set up) to update the database...
					LogSQL(tcDataAdapter.UpdateCommand);
					_with7.ExecuteNonQuery();
					CloseDataView(ref dv);
					dv = null;
					break;
			}
		}
		#endregion
		#region "Event Handlers"
		private void clsKits_AddComplete(object sender, DataEventArgs e)
		{
			this.PopulateRelated(e.RowView);
		}
		private void clsKits_AddInProcess(object sender, DataEventArgs e)
		{
			if ((bool)e.RowView["HasDecals"])
				this.AddRelated("Decals", e);
			if ((bool)e.RowView["HasDetailSet"])
				this.AddRelated("Detail Sets", e);
		}
		private void clsKits_DeleteComplete(object sender, DataEventArgs e)
		{
			//Me.PopulateRelated(e.RowView)
		}
		private void clsKits_BeforeDelete(object sender, DataEventArgs e)
		{
            int RecordsAffected = 0;
			if (!Information.IsDBNull(e.RowView["DecalID"]))
				ExecuteCommand(string.Format("Delete From [{0}] Where [ID]={1};", "Decals", e.RowView["DecalID"]), ref RecordsAffected);
			if (!Information.IsDBNull(e.RowView["DetailSetID"]))
				ExecuteCommand(string.Format("Delete From [{0}] Where [ID]={1};", "DetailSets", e.RowView["DetailSetID"]), ref RecordsAffected);
		}
		private void clsKits_UpdateComplete(object sender, DataEventArgs e)
		{
			this.PopulateRelated(e.RowView);
		}
		private void clsKits_UpdateInProcess(object sender, DataEventArgs e)
		{
			if ((bool)e.RowView["HasDecals"]) {
				if ((bool)e.RowView.Row["HasDecals", DataRowVersion.Original])
					this.UpdateRelated("Decals", e);
				else
					this.AddRelated("Decals", e);
			} else {
				if ((bool)e.RowView.Row["HasDecals", DataRowVersion.Original])
					this.DeleteRelated("Decals", e);
			}
			if ((bool)e.RowView["HasDetailSet"]) {
				if ((bool)e.RowView.Row["HasDetailSet", DataRowVersion.Original])
					this.UpdateRelated("Detail Sets", e);
				else
					this.AddRelated("Detail Sets", e);
			} else {
				if ((bool)e.RowView.Row["HasDetailSet", DataRowVersion.Original])
					this.DeleteRelated("Detail Sets", e);
			}
		}
		#endregion
	}
}

using Microsoft.VisualBasic;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TCBase;
using static TCBase.clsRegistry;
using static TCBase.clsSupport;
using static TCBase.clsTCBase;
using static TCBase.clsTrace;
//frmMain.vb
//   Options/Properties Form...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   04/21/18    Modified to support data queries outside Load function without triggering NullReferenceExceptions in DoSpash;
//   09/18/16    Reworked to reflect architectural changes;
//   10/19/14    Upgraded to VS2013;
//   08/06/10    Corrected over-sized background scrolling;
//   07/24/10    Added History report;
//               Added UserId and Database labels;
//   07/20/10    Added Model Kits for Storage report;
//   10/25/09    Rewritten in VB.NET;
//   10/23/09    Added Reports menu;
//   10/17/07    Added mTCBase.ConnectCommand to Form_Activate long enough to get database name for title-bar display;
//   02/04/03    Support for mTCBase.UserID and .Password;
//   10/14/02    Added Error Handling;
//   10/06/02    More TreasureChest refitting;
//   09/17/02    Reworked DoMenu() to use TC* objects instead of individual Form references;
//               Implemented ShowSplash() & CancelSplash() from TCBase.clsTCBaseUtl;
//   08/20/02    Started History;
//=================================================================================================================================
//=================================================================================================================================
 // ERROR: Not supported in C#: OptionDeclaration
namespace TreasureChest2
{
	public class frmMain : frmTCBase
	{
		const string myFormName = "frmMain";
		public frmMain() : base()
		{
			Resize += frmMain_Resize;
			Closed += frmMain_Closed;
			Load += frmMain_Load;
			Activated += frmMain_Activated;

			//This call is required by the Windows Form Designer.
			InitializeComponent();

			//Add any initialization after the InitializeComponent() call
		}
		public frmMain(clsSupport objSupport, Form objParent) : base(objSupport, myFormName, objParent)
		{
			Resize += frmMain_Resize;
			Closed += frmMain_Closed;
			Load += frmMain_Load;
			Activated += frmMain_Activated;
			//This call is required by the Windows Form Designer.
			InitializeComponent();
		}
		public frmMain(clsSupport objSupport, TCBase.clsTCBase mTCBase, Form objParent) : base(objSupport, myFormName, mTCBase, objParent)
		{
			Resize += frmMain_Resize;
			Closed += frmMain_Closed;
			Load += frmMain_Load;
			Activated += frmMain_Activated;
			//This call is required by the Windows Form Designer.
			InitializeComponent();
		}
		#region " Windows Form Designer generated code "
		//Form overrides dispose to clean up the component list.
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if ((components != null)) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		//Required by the Windows Form Designer
		private System.ComponentModel.IContainer components;
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.  
		//Do not modify it using the code editor.
		private System.Windows.Forms.HScrollBar withEventsField_scrollH;
		internal System.Windows.Forms.HScrollBar scrollH {
			get { return withEventsField_scrollH; }
			set {
				if (withEventsField_scrollH != null) {
					withEventsField_scrollH.Scroll -= scrollH_Scroll;
				}
				withEventsField_scrollH = value;
				if (withEventsField_scrollH != null) {
					withEventsField_scrollH.Scroll += scrollH_Scroll;
				}
			}
		}
		private System.Windows.Forms.VScrollBar withEventsField_scrollV;
		internal System.Windows.Forms.VScrollBar scrollV {
			get { return withEventsField_scrollV; }
			set {
				if (withEventsField_scrollV != null) {
					withEventsField_scrollV.Scroll -= scrollV_Scroll;
				}
				withEventsField_scrollV = value;
				if (withEventsField_scrollV != null) {
					withEventsField_scrollV.Scroll += scrollV_Scroll;
				}
			}
		}
		internal System.Windows.Forms.StatusBar sbStatus;
		internal System.Windows.Forms.StatusBarPanel sbpTime;
		internal System.Windows.Forms.StatusBarPanel sbpEndBorder;
		internal System.Windows.Forms.StatusBarPanel sbpVersion;
		internal System.Windows.Forms.StatusBarPanel sbpStatus;
		private System.Windows.Forms.Timer withEventsField_timClock;
		internal System.Windows.Forms.Timer timClock {
			get { return withEventsField_timClock; }
			set {
				if (withEventsField_timClock != null) {
					withEventsField_timClock.Tick -= timClock_Tick;
				}
				withEventsField_timClock = value;
				if (withEventsField_timClock != null) {
					withEventsField_timClock.Tick += timClock_Tick;
				}
			}
		}
		private System.Windows.Forms.NotifyIcon withEventsField_niMain;
		internal System.Windows.Forms.NotifyIcon niMain {
			get { return withEventsField_niMain; }
			set {
				if (withEventsField_niMain != null) {
					withEventsField_niMain.DoubleClick -= niMain_DoubleClick;
				}
				withEventsField_niMain = value;
				if (withEventsField_niMain != null) {
					withEventsField_niMain.DoubleClick += niMain_DoubleClick;
				}
			}
		}
		internal System.Windows.Forms.MenuItem mnuFile;
		internal System.Windows.Forms.MenuItem mnuFileOptions;
		internal System.Windows.Forms.MenuItem mnuFileExit;
		internal System.Windows.Forms.MainMenu mnuMain;
		internal System.Windows.Forms.ContextMenu ctxMain;
		internal System.Windows.Forms.MenuItem mnuFileSep;
		internal System.Windows.Forms.MenuItem mnuDatabase;
		internal System.Windows.Forms.MenuItem mnuReports;
		internal System.Windows.Forms.MenuItem mnuHelp;
		internal System.Windows.Forms.MenuItem mnuHelpAbout;
		internal System.Windows.Forms.MenuItem mnuReportsDVDs;
		internal System.Windows.Forms.MenuItem mnuReportsHistory;
		internal System.Windows.Forms.MenuItem mnuReportsKitsByLocation;
		internal System.Windows.Forms.MenuItem mnuReportsKitsForStorage;
		internal System.Windows.Forms.MenuItem mnuReportsWishList;
		internal System.Windows.Forms.MenuItem mnuDatabaseBooks;
		internal System.Windows.Forms.MenuItem mnuDatabaseCollectables;
		internal System.Windows.Forms.MenuItem mnuDatabaseHobby;
		internal System.Windows.Forms.MenuItem mnuDatabaseImages;
		internal System.Windows.Forms.MenuItem mnuDatabaseMusic;
		internal System.Windows.Forms.MenuItem mnuDatabaseSoftware;
		internal System.Windows.Forms.MenuItem mnuDatabaseVideoLibrary;
		internal System.Windows.Forms.MenuItem mnuDatabaseWebLinks;
		internal System.Windows.Forms.MenuItem mnuDatabaseHobbyKits;
		internal System.Windows.Forms.MenuItem mnuDatabaseHobbyDecals;
		internal System.Windows.Forms.MenuItem mnuDatabaseHobbyDetailSets;
		internal System.Windows.Forms.MenuItem mnuDatabaseHobbyFinishingProducts;
		internal System.Windows.Forms.MenuItem mnuDatabaseHobbySep1;
		internal System.Windows.Forms.MenuItem mnuDatabaseHobbyTools;
		internal System.Windows.Forms.MenuItem mnuDatabaseHobbyVideoResearch;
		internal System.Windows.Forms.MenuItem mnuDatabaseHobbySep2;
		internal System.Windows.Forms.MenuItem mnuDatabaseHobbyRockets;
		internal System.Windows.Forms.MenuItem mnuDatabaseHobbyTrains;
		internal System.Windows.Forms.MenuItem mnuDatabaseHobbySep3;
		internal System.Windows.Forms.MenuItem mnuDatabaseHobbyCompanies;
		internal System.Windows.Forms.MenuItem mnuDatabaseHobbySep4;
		internal System.Windows.Forms.MenuItem mnuDatabaseHobbyAircraftDesignations;
		internal System.Windows.Forms.MenuItem mnuDatabaseHobbyBlueAngelsHistory;
		internal System.Windows.Forms.MenuItem mnuDatabaseUSNavy;
		internal System.Windows.Forms.MenuItem mnuDatabaseUSNavyShips;
		internal System.Windows.Forms.MenuItem mnuDatabaseUSNavySep;
		internal System.Windows.Forms.MenuItem mnuDatabaseUSNavyClasses;
		internal System.Windows.Forms.MenuItem mnuDatabaseUSNavyClassifications;
		internal System.Windows.Forms.MenuItem mnuDatabaseVideoLibraryMovies;
		internal System.Windows.Forms.MenuItem mnuDatabaseVideoLibrarySpecials;
		internal System.Windows.Forms.MenuItem mnuDatabaseVideoLibraryTVEpisodes;
		internal System.Windows.Forms.MenuItem mnuFileTrace;
		internal System.Windows.Forms.Panel pcViewPort;
		internal System.Windows.Forms.PictureBox pbImage;
		internal System.Windows.Forms.Label lblUserIDLabel;
		internal System.Windows.Forms.Label lblUserID;
		internal System.Windows.Forms.Label lblDatabaseLabel;
		internal StatusBarPanel sbpTrace;
		internal System.Windows.Forms.Label lblDatabase;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
			this.scrollH = new System.Windows.Forms.HScrollBar();
			this.scrollV = new System.Windows.Forms.VScrollBar();
			this.ctxMain = new System.Windows.Forms.ContextMenu();
			this.sbStatus = new System.Windows.Forms.StatusBar();
			this.sbpVersion = new System.Windows.Forms.StatusBarPanel();
			this.sbpStatus = new System.Windows.Forms.StatusBarPanel();
			this.sbpTime = new System.Windows.Forms.StatusBarPanel();
			this.sbpEndBorder = new System.Windows.Forms.StatusBarPanel();
			this.timClock = new System.Windows.Forms.Timer(this.components);
			this.niMain = new System.Windows.Forms.NotifyIcon(this.components);
			this.mnuMain = new System.Windows.Forms.MainMenu(this.components);
			this.mnuFile = new System.Windows.Forms.MenuItem();
			this.mnuFileOptions = new System.Windows.Forms.MenuItem();
			this.mnuFileTrace = new System.Windows.Forms.MenuItem();
			this.mnuFileSep = new System.Windows.Forms.MenuItem();
			this.mnuFileExit = new System.Windows.Forms.MenuItem();
			this.mnuDatabase = new System.Windows.Forms.MenuItem();
			this.mnuDatabaseBooks = new System.Windows.Forms.MenuItem();
			this.mnuDatabaseCollectables = new System.Windows.Forms.MenuItem();
			this.mnuDatabaseHobby = new System.Windows.Forms.MenuItem();
			this.mnuDatabaseHobbyKits = new System.Windows.Forms.MenuItem();
			this.mnuDatabaseHobbyDecals = new System.Windows.Forms.MenuItem();
			this.mnuDatabaseHobbyDetailSets = new System.Windows.Forms.MenuItem();
			this.mnuDatabaseHobbyFinishingProducts = new System.Windows.Forms.MenuItem();
			this.mnuDatabaseHobbySep1 = new System.Windows.Forms.MenuItem();
			this.mnuDatabaseHobbyTools = new System.Windows.Forms.MenuItem();
			this.mnuDatabaseHobbyVideoResearch = new System.Windows.Forms.MenuItem();
			this.mnuDatabaseHobbySep2 = new System.Windows.Forms.MenuItem();
			this.mnuDatabaseHobbyRockets = new System.Windows.Forms.MenuItem();
			this.mnuDatabaseHobbyTrains = new System.Windows.Forms.MenuItem();
			this.mnuDatabaseHobbySep3 = new System.Windows.Forms.MenuItem();
			this.mnuDatabaseHobbyCompanies = new System.Windows.Forms.MenuItem();
			this.mnuDatabaseHobbySep4 = new System.Windows.Forms.MenuItem();
			this.mnuDatabaseHobbyAircraftDesignations = new System.Windows.Forms.MenuItem();
			this.mnuDatabaseHobbyBlueAngelsHistory = new System.Windows.Forms.MenuItem();
			this.mnuDatabaseImages = new System.Windows.Forms.MenuItem();
			this.mnuDatabaseMusic = new System.Windows.Forms.MenuItem();
			this.mnuDatabaseSoftware = new System.Windows.Forms.MenuItem();
			this.mnuDatabaseUSNavy = new System.Windows.Forms.MenuItem();
			this.mnuDatabaseUSNavyShips = new System.Windows.Forms.MenuItem();
			this.mnuDatabaseUSNavySep = new System.Windows.Forms.MenuItem();
			this.mnuDatabaseUSNavyClasses = new System.Windows.Forms.MenuItem();
			this.mnuDatabaseUSNavyClassifications = new System.Windows.Forms.MenuItem();
			this.mnuDatabaseVideoLibrary = new System.Windows.Forms.MenuItem();
			this.mnuDatabaseVideoLibraryMovies = new System.Windows.Forms.MenuItem();
			this.mnuDatabaseVideoLibrarySpecials = new System.Windows.Forms.MenuItem();
			this.mnuDatabaseVideoLibraryTVEpisodes = new System.Windows.Forms.MenuItem();
			this.mnuDatabaseWebLinks = new System.Windows.Forms.MenuItem();
			this.mnuReports = new System.Windows.Forms.MenuItem();
			this.mnuReportsDVDs = new System.Windows.Forms.MenuItem();
			this.mnuReportsHistory = new System.Windows.Forms.MenuItem();
			this.mnuReportsKitsByLocation = new System.Windows.Forms.MenuItem();
			this.mnuReportsKitsForStorage = new System.Windows.Forms.MenuItem();
			this.mnuReportsWishList = new System.Windows.Forms.MenuItem();
			this.mnuHelp = new System.Windows.Forms.MenuItem();
			this.mnuHelpAbout = new System.Windows.Forms.MenuItem();
			this.pcViewPort = new System.Windows.Forms.Panel();
			this.lblUserIDLabel = new System.Windows.Forms.Label();
			this.lblUserID = new System.Windows.Forms.Label();
			this.lblDatabaseLabel = new System.Windows.Forms.Label();
			this.lblDatabase = new System.Windows.Forms.Label();
			this.pbImage = new System.Windows.Forms.PictureBox();
			this.sbpTrace = new System.Windows.Forms.StatusBarPanel();
			((System.ComponentModel.ISupportInitialize)this.epBase).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpVersion).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpStatus).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpTime).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpEndBorder).BeginInit();
			this.pcViewPort.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.pbImage).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.sbpTrace).BeginInit();
			this.SuspendLayout();
			//
			//imgBase
			//
			this.imgBase.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imgBase.ImageStream");
			this.imgBase.Images.SetKeyName(0, "");
			this.imgBase.Images.SetKeyName(1, "");
			this.imgBase.Images.SetKeyName(2, "");
			this.imgBase.Images.SetKeyName(3, "");
			this.imgBase.Images.SetKeyName(4, "");
			this.imgBase.Images.SetKeyName(5, "");
			this.imgBase.Images.SetKeyName(6, "");
			this.imgBase.Images.SetKeyName(7, "");
			this.imgBase.Images.SetKeyName(8, "");
			this.imgBase.Images.SetKeyName(9, "");
			this.imgBase.Images.SetKeyName(10, "");
			this.imgBase.Images.SetKeyName(11, "");
			this.imgBase.Images.SetKeyName(12, "");
			this.imgBase.Images.SetKeyName(13, "");
			this.imgBase.Images.SetKeyName(14, "");
			this.imgBase.Images.SetKeyName(15, "");
			this.imgBase.Images.SetKeyName(16, "");
			this.imgBase.Images.SetKeyName(17, "");
			this.imgBase.Images.SetKeyName(18, "");
			this.imgBase.Images.SetKeyName(19, "");
			this.imgBase.Images.SetKeyName(20, "");
			this.imgBase.Images.SetKeyName(21, "");
			this.imgBase.Images.SetKeyName(22, "");
			this.imgBase.Images.SetKeyName(23, "CHECKMRK.ICO");
			this.imgBase.Images.SetKeyName(24, "");
			//
			//scrollH
			//
			this.scrollH.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.scrollH.LargeChange = 1000;
			this.scrollH.Location = new System.Drawing.Point(0, 256);
			this.scrollH.Maximum = 32767;
			this.scrollH.Name = "scrollH";
			this.scrollH.Size = new System.Drawing.Size(396, 17);
			this.scrollH.SmallChange = 100;
			this.scrollH.TabIndex = 1;
			//
			//scrollV
			//
			this.scrollV.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Right);
			this.scrollV.LargeChange = 1000;
			this.scrollV.Location = new System.Drawing.Point(396, 0);
			this.scrollV.Maximum = 32767;
			this.scrollV.Name = "scrollV";
			this.scrollV.Size = new System.Drawing.Size(17, 256);
			this.scrollV.SmallChange = 100;
			this.scrollV.TabIndex = 2;
			//
			//sbStatus
			//
			this.sbStatus.Location = new System.Drawing.Point(0, 272);
			this.sbStatus.Name = "sbStatus";
			this.sbStatus.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
				this.sbpVersion,
				this.sbpStatus,
				this.sbpTrace,
				this.sbpTime,
				this.sbpEndBorder
			});
			this.sbStatus.ShowPanels = true;
			this.sbStatus.Size = new System.Drawing.Size(412, 22);
			this.sbStatus.SizingGrip = false;
			this.sbStatus.TabIndex = 9;
			//
			//sbpVersion
			//
			this.sbpVersion.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.sbpVersion.Name = "sbpVersion";
			this.sbpVersion.Text = "Version";
			this.sbpVersion.Width = 282;
			//
			//sbpStatus
			//
			this.sbpStatus.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.sbpStatus.MinWidth = 0;
			this.sbpStatus.Name = "sbpStatus";
			this.sbpStatus.Width = 0;
			//
			//sbpTrace
			//
			this.sbpTrace.MinWidth = 0;
			this.sbpTrace.Name = "sbpTrace";
			this.sbpTrace.Text = "";
			this.sbpTrace.Width = 0;
			//
			//sbpTime
			//
			this.sbpTime.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
			this.sbpTime.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.sbpTime.MinWidth = 70;
			this.sbpTime.Name = "sbpTime";
			this.sbpTime.Text = "12:59 PM";
			this.sbpTime.Width = 79;
			//
			//sbpEndBorder
			//
			this.sbpEndBorder.MinWidth = 1;
			this.sbpEndBorder.Name = "sbpEndBorder";
			this.sbpEndBorder.Width = 1;
			//
			//timClock
			//
			this.timClock.Enabled = true;
			this.timClock.Interval = 1000;
			//
			//niMain
			//
			this.niMain.ContextMenu = this.ctxMain;
			this.niMain.Icon = (System.Drawing.Icon)resources.GetObject("niMain.Icon");
			this.niMain.Text = "TreasureChest2";
			this.niMain.Visible = true;
			//
			//mnuMain
			//
			this.mnuMain.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
				this.mnuFile,
				this.mnuDatabase,
				this.mnuReports,
				this.mnuHelp
			});
			//
			//mnuFile
			//
			this.mnuFile.Index = 0;
			this.mnuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
				this.mnuFileOptions,
				this.mnuFileTrace,
				this.mnuFileSep,
				this.mnuFileExit
			});
			this.mnuFile.Text = "&File";
			//
			//mnuFileOptions
			//
			this.mnuFileOptions.Index = 0;
			this.mnuFileOptions.Text = "&Options...";
			//
			//mnuFileTrace
			//
			this.mnuFileTrace.Index = 1;
			this.mnuFileTrace.Text = "Trace";
			//
			//mnuFileSep
			//
			this.mnuFileSep.Index = 2;
			this.mnuFileSep.Text = "-";
			//
			//mnuFileExit
			//
			this.mnuFileExit.Index = 3;
			this.mnuFileExit.Text = "E&xit";
			//
			//mnuDatabase
			//
			this.mnuDatabase.Index = 1;
			this.mnuDatabase.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
				this.mnuDatabaseBooks,
				this.mnuDatabaseCollectables,
				this.mnuDatabaseHobby,
				this.mnuDatabaseImages,
				this.mnuDatabaseMusic,
				this.mnuDatabaseSoftware,
				this.mnuDatabaseUSNavy,
				this.mnuDatabaseVideoLibrary,
				this.mnuDatabaseWebLinks
			});
			this.mnuDatabase.Text = "&Database";
			//
			//mnuDatabaseBooks
			//
			this.mnuDatabaseBooks.Index = 0;
			this.mnuDatabaseBooks.Text = "&Books";
			//
			//mnuDatabaseCollectables
			//
			this.mnuDatabaseCollectables.Index = 1;
			this.mnuDatabaseCollectables.Text = "&Collectables";
			//
			//mnuDatabaseHobby
			//
			this.mnuDatabaseHobby.Index = 2;
			this.mnuDatabaseHobby.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
				this.mnuDatabaseHobbyKits,
				this.mnuDatabaseHobbyDecals,
				this.mnuDatabaseHobbyDetailSets,
				this.mnuDatabaseHobbyFinishingProducts,
				this.mnuDatabaseHobbySep1,
				this.mnuDatabaseHobbyTools,
				this.mnuDatabaseHobbyVideoResearch,
				this.mnuDatabaseHobbySep2,
				this.mnuDatabaseHobbyRockets,
				this.mnuDatabaseHobbyTrains,
				this.mnuDatabaseHobbySep3,
				this.mnuDatabaseHobbyCompanies,
				this.mnuDatabaseHobbySep4,
				this.mnuDatabaseHobbyAircraftDesignations,
				this.mnuDatabaseHobbyBlueAngelsHistory
			});
			this.mnuDatabaseHobby.Text = "&Hobby";
			//
			//mnuDatabaseHobbyKits
			//
			this.mnuDatabaseHobbyKits.Index = 0;
			this.mnuDatabaseHobbyKits.Text = "&Kits";
			//
			//mnuDatabaseHobbyDecals
			//
			this.mnuDatabaseHobbyDecals.Index = 1;
			this.mnuDatabaseHobbyDecals.Text = "&Decals";
			//
			//mnuDatabaseHobbyDetailSets
			//
			this.mnuDatabaseHobbyDetailSets.Index = 2;
			this.mnuDatabaseHobbyDetailSets.Text = "Detail &Sets";
			//
			//mnuDatabaseHobbyFinishingProducts
			//
			this.mnuDatabaseHobbyFinishingProducts.Index = 3;
			this.mnuDatabaseHobbyFinishingProducts.Text = "&Finishing Products";
			//
			//mnuDatabaseHobbySep1
			//
			this.mnuDatabaseHobbySep1.Index = 4;
			this.mnuDatabaseHobbySep1.Text = "-";
			//
			//mnuDatabaseHobbyTools
			//
			this.mnuDatabaseHobbyTools.Index = 5;
			this.mnuDatabaseHobbyTools.Text = "&Tools";
			//
			//mnuDatabaseHobbyVideoResearch
			//
			this.mnuDatabaseHobbyVideoResearch.Index = 6;
			this.mnuDatabaseHobbyVideoResearch.Text = "&Video Research";
			//
			//mnuDatabaseHobbySep2
			//
			this.mnuDatabaseHobbySep2.Index = 7;
			this.mnuDatabaseHobbySep2.Text = "-";
			//
			//mnuDatabaseHobbyRockets
			//
			this.mnuDatabaseHobbyRockets.Index = 8;
			this.mnuDatabaseHobbyRockets.Text = "&Rockets";
			//
			//mnuDatabaseHobbyTrains
			//
			this.mnuDatabaseHobbyTrains.Index = 9;
			this.mnuDatabaseHobbyTrains.Text = "&Trains";
			//
			//mnuDatabaseHobbySep3
			//
			this.mnuDatabaseHobbySep3.Index = 10;
			this.mnuDatabaseHobbySep3.Text = "-";
			//
			//mnuDatabaseHobbyCompanies
			//
			this.mnuDatabaseHobbyCompanies.Index = 11;
			this.mnuDatabaseHobbyCompanies.Text = "&Companies";
			//
			//mnuDatabaseHobbySep4
			//
			this.mnuDatabaseHobbySep4.Index = 12;
			this.mnuDatabaseHobbySep4.Text = "-";
			//
			//mnuDatabaseHobbyAircraftDesignations
			//
			this.mnuDatabaseHobbyAircraftDesignations.Index = 13;
			this.mnuDatabaseHobbyAircraftDesignations.Text = "&Aircraft Designations";
			//
			//mnuDatabaseHobbyBlueAngelsHistory
			//
			this.mnuDatabaseHobbyBlueAngelsHistory.Index = 14;
			this.mnuDatabaseHobbyBlueAngelsHistory.Text = "&Blue Angels History";
			//
			//mnuDatabaseImages
			//
			this.mnuDatabaseImages.Index = 3;
			this.mnuDatabaseImages.Text = "&Images";
			//
			//mnuDatabaseMusic
			//
			this.mnuDatabaseMusic.Index = 4;
			this.mnuDatabaseMusic.Text = "&Music";
			//
			//mnuDatabaseSoftware
			//
			this.mnuDatabaseSoftware.Index = 5;
			this.mnuDatabaseSoftware.Text = "&Software";
			//
			//mnuDatabaseUSNavy
			//
			this.mnuDatabaseUSNavy.Index = 6;
			this.mnuDatabaseUSNavy.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
				this.mnuDatabaseUSNavyShips,
				this.mnuDatabaseUSNavySep,
				this.mnuDatabaseUSNavyClasses,
				this.mnuDatabaseUSNavyClassifications
			});
			this.mnuDatabaseUSNavy.Text = "U.S. Navy";
			//
			//mnuDatabaseUSNavyShips
			//
			this.mnuDatabaseUSNavyShips.Index = 0;
			this.mnuDatabaseUSNavyShips.Text = "&Ships";
			//
			//mnuDatabaseUSNavySep
			//
			this.mnuDatabaseUSNavySep.Index = 1;
			this.mnuDatabaseUSNavySep.Text = "-";
			//
			//mnuDatabaseUSNavyClasses
			//
			this.mnuDatabaseUSNavyClasses.Index = 2;
			this.mnuDatabaseUSNavyClasses.Text = "&Classes";
			//
			//mnuDatabaseUSNavyClassifications
			//
			this.mnuDatabaseUSNavyClassifications.Index = 3;
			this.mnuDatabaseUSNavyClassifications.Text = "C&lassifications";
			//
			//mnuDatabaseVideoLibrary
			//
			this.mnuDatabaseVideoLibrary.Index = 7;
			this.mnuDatabaseVideoLibrary.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
				this.mnuDatabaseVideoLibraryMovies,
				this.mnuDatabaseVideoLibrarySpecials,
				this.mnuDatabaseVideoLibraryTVEpisodes
			});
			this.mnuDatabaseVideoLibrary.Text = "&Video Library";
			//
			//mnuDatabaseVideoLibraryMovies
			//
			this.mnuDatabaseVideoLibraryMovies.Index = 0;
			this.mnuDatabaseVideoLibraryMovies.Text = "&Movies";
			//
			//mnuDatabaseVideoLibrarySpecials
			//
			this.mnuDatabaseVideoLibrarySpecials.Index = 1;
			this.mnuDatabaseVideoLibrarySpecials.Text = "&Specials";
			//
			//mnuDatabaseVideoLibraryTVEpisodes
			//
			this.mnuDatabaseVideoLibraryTVEpisodes.Index = 2;
			this.mnuDatabaseVideoLibraryTVEpisodes.Text = "TV &Episodes";
			//
			//mnuDatabaseWebLinks
			//
			this.mnuDatabaseWebLinks.Index = 8;
			this.mnuDatabaseWebLinks.Text = "&WebLinks";
			//
			//mnuReports
			//
			this.mnuReports.Index = 2;
			this.mnuReports.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
				this.mnuReportsDVDs,
				this.mnuReportsHistory,
				this.mnuReportsKitsByLocation,
				this.mnuReportsKitsForStorage,
				this.mnuReportsWishList
			});
			this.mnuReports.Text = "&Reports";
			//
			//mnuReportsDVDs
			//
			this.mnuReportsDVDs.Index = 0;
			this.mnuReportsDVDs.Text = "DVDs";
			//
			//mnuReportsHistory
			//
			this.mnuReportsHistory.Index = 1;
			this.mnuReportsHistory.Text = "History";
			//
			//mnuReportsKitsByLocation
			//
			this.mnuReportsKitsByLocation.Index = 2;
			this.mnuReportsKitsByLocation.Text = "Model Kits by Location";
			//
			//mnuReportsKitsForStorage
			//
			this.mnuReportsKitsForStorage.Index = 3;
			this.mnuReportsKitsForStorage.Text = "Model Kits for Storage";
			//
			//mnuReportsWishList
			//
			this.mnuReportsWishList.Index = 4;
			this.mnuReportsWishList.Text = "WishList";
			//
			//mnuHelp
			//
			this.mnuHelp.Index = 3;
			this.mnuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] { this.mnuHelpAbout });
			this.mnuHelp.Text = "&Help";
			//
			//mnuHelpAbout
			//
			this.mnuHelpAbout.Index = 0;
			this.mnuHelpAbout.Text = "&About...";
			//
			//pcViewPort
			//
			this.pcViewPort.BackColor = System.Drawing.SystemColors.Desktop;
			this.pcViewPort.Controls.Add(this.lblUserIDLabel);
			this.pcViewPort.Controls.Add(this.lblUserID);
			this.pcViewPort.Controls.Add(this.lblDatabaseLabel);
			this.pcViewPort.Controls.Add(this.lblDatabase);
			this.pcViewPort.Controls.Add(this.pbImage);
			this.pcViewPort.Location = new System.Drawing.Point(0, 0);
			this.pcViewPort.Name = "pcViewPort";
			this.pcViewPort.Size = new System.Drawing.Size(396, 260);
			this.pcViewPort.TabIndex = 23;
			//
			//lblUserIDLabel
			//
			this.lblUserIDLabel.AutoSize = true;
			this.lblUserIDLabel.BackColor = System.Drawing.Color.Transparent;
			this.lblUserIDLabel.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.lblUserIDLabel.ForeColor = System.Drawing.Color.White;
			this.lblUserIDLabel.Location = new System.Drawing.Point(4, 4);
			this.lblUserIDLabel.Name = "lblUserIDLabel";
			this.lblUserIDLabel.Size = new System.Drawing.Size(66, 16);
			this.lblUserIDLabel.TabIndex = 24;
			this.lblUserIDLabel.Text = "User ID:";
			//
			//lblUserID
			//
			this.lblUserID.AutoSize = true;
			this.lblUserID.BackColor = System.Drawing.Color.Transparent;
			this.lblUserID.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.lblUserID.ForeColor = System.Drawing.Color.White;
			this.lblUserID.Location = new System.Drawing.Point(84, 4);
			this.lblUserID.Name = "lblUserID";
			this.lblUserID.Size = new System.Drawing.Size(74, 16);
			this.lblUserID.TabIndex = 26;
			this.lblUserID.Text = "lblUserID";
			//
			//lblDatabaseLabel
			//
			this.lblDatabaseLabel.AutoSize = true;
			this.lblDatabaseLabel.BackColor = System.Drawing.Color.Transparent;
			this.lblDatabaseLabel.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.lblDatabaseLabel.ForeColor = System.Drawing.Color.White;
			this.lblDatabaseLabel.Location = new System.Drawing.Point(4, 24);
			this.lblDatabaseLabel.Name = "lblDatabaseLabel";
			this.lblDatabaseLabel.Size = new System.Drawing.Size(82, 16);
			this.lblDatabaseLabel.TabIndex = 25;
			this.lblDatabaseLabel.Text = "Database:";
			//
			//lblDatabase
			//
			this.lblDatabase.AutoSize = true;
			this.lblDatabase.BackColor = System.Drawing.Color.Transparent;
			this.lblDatabase.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.lblDatabase.ForeColor = System.Drawing.Color.White;
			this.lblDatabase.Location = new System.Drawing.Point(84, 24);
			this.lblDatabase.Name = "lblDatabase";
			this.lblDatabase.Size = new System.Drawing.Size(94, 16);
			this.lblDatabase.TabIndex = 27;
			this.lblDatabase.Text = "lblDatabase";
			//
			//pbImage
			//
			this.pbImage.BackColor = System.Drawing.SystemColors.Desktop;
			this.pbImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pbImage.ContextMenu = this.ctxMain;
			this.pbImage.Image = (System.Drawing.Image)resources.GetObject("pbImage.Image");
			this.pbImage.Location = new System.Drawing.Point(-34, -44);
			this.pbImage.Name = "pbImage";
			this.pbImage.Size = new System.Drawing.Size(268, 188);
			this.pbImage.TabIndex = 23;
			this.pbImage.TabStop = false;
			//
			//frmMain
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(7, 16);
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(412, 294);
			this.Controls.Add(this.scrollV);
			this.Controls.Add(this.scrollH);
			this.Controls.Add(this.sbStatus);
			this.Controls.Add(this.pcViewPort);
			this.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			this.MaximizeBox = false;
			this.Menu = this.mnuMain;
			this.Name = "frmMain";
			this.Text = "TreasureChest";
			((System.ComponentModel.ISupportInitialize)this.epBase).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpVersion).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpStatus).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpTrace).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpTime).EndInit();
			((System.ComponentModel.ISupportInitialize)this.sbpEndBorder).EndInit();
			this.pcViewPort.ResumeLayout(false);
			this.pcViewPort.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.pbImage).EndInit();
			this.ResumeLayout(false);

		}
		#endregion
		#region "Events"
		//None at this time...
		#endregion
		#region "Properties"
		#region "Declarations"
		private bool fActivated = false;
			#endregion
		private frmSplash mSplash = null;
		#endregion
		#region "Methods"
		private void DoMenu(string ComponentName, string ClassName, string Caption)
		{
			string AssemblyName = null;
			System.Reflection.Assembly TCAssembly = null;
			//Dim objTCComponent As intTCComponent
			clsTCBase objTCComponent = null;
			try {
				this.Cursor = Cursors.WaitCursor;
				mTCBase.MainHeight = this.Height;
				mTCBase.MainWidth = this.Width;
				mTCBase.SaveTop = this.Top;
				mTCBase.SaveLeft = this.Left;
				mTCBase.SaveCaption = this.Text;
				mTCBase.SaveIcon = this.Icon;
				mTCBase.ActiveForm = this;

				//Load the target assembly...
				AssemblyName = FindComponent(ComponentName);
				if ((AssemblyName == null))
					throw new Exception("Unable to determine assembly location for component \"" + ComponentName + "\"");
				TCAssembly = Assembly.LoadFrom(AssemblyName);
				objTCComponent = (clsTCBase)TCAssembly.CreateInstance(ClassName, true, BindingFlags.CreateInstance, null, new object[] {
					mSupport,
					ClassName.Split('.')[1]
				}, null, null);
				if ((objTCComponent == null))
					throw new Exception(string.Format("Component not properly configured - Class \"{0}\" not found in Assembly \"{1}\"", ClassName, AssemblyName));

				objTCComponent.Splash += this.DoSplash;
				try {
					objTCComponent.Load(this, Caption);
				} finally {
					objTCComponent.Splash -= this.DoSplash;
				}

				this.Hide();
				this.niMain.Visible = true;
				this.niMain.Text = mSupport.ApplicationName;

				this.Text = mTCBase.SaveCaption + " - " + Caption;
				this.Icon = objTCComponent.Icon;
				this.Cursor = Cursors.Default;

				objTCComponent.Show();
			} finally {
				objTCComponent.Dispose();
				objTCComponent = null;
				TCAssembly = null;
				AssemblyName = null;
				ShowMain();
			}
			ShowMain();
		}
		private void LoadBackground(string ImagePath)
		{
			try {
				pbImage.Image = Image.FromFile(ImagePath);
			} catch (Exception ex) {
				ShowMsgBox(string.Format("{0}. Using default image...", ex.Message), MsgBoxStyle.OkOnly, this, this.Text);
				try {
					pbImage.Image = Image.FromFile(mTCBase.ImagePath);
				} catch (Exception ex2) {
					ShowMsgBox(string.Format("{0}. Bagging this image crap... We didn't need no stinking images anyway...", ex.Message), MsgBoxStyle.OkOnly, this, this.Text);
					return;
				}
			}
			SaveRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mTCBase.RegistryKey, "ImagePath", ImagePath);
			this.pcViewPort.BackgroundImage = this.pbImage.Image;

            int scWidth = Screen.PrimaryScreen.Bounds.Width;
            int scHeight = Screen.PrimaryScreen.Bounds.Height;
            int borderWidth = this.Width - this.ClientRectangle.Width;
            int borderHeight = this.Height - this.ClientRectangle.Height;
			int newWidth = 0;
			int newHeight = 0;

			//Everything is governed by the size of the picture...
			this.pbImage.Size = new Size(this.pbImage.Image.Size.Width, this.pbImage.Image.Size.Height);
			this.ClientSize = new Size(this.pbImage.Image.Size.Width, this.pbImage.Image.Size.Height);
			if (this.ClientSize.Width > scWidth) {
				this.scrollH.Visible = true;
				this.scrollH.Value = 0;
				this.scrollH.Minimum = 0;
				newWidth = scWidth;
			} else {
				this.scrollH.Visible = false;
				newWidth = this.ClientSize.Width + borderWidth;
			}
			if (this.ClientSize.Height > scHeight) {
				this.scrollV.Visible = true;
				this.scrollV.Value = 0;
				this.scrollV.Minimum = 0;
				newHeight = scHeight;
			} else {
				this.scrollV.Visible = false;
				newHeight = this.ClientSize.Height + borderHeight;
			}
			this.Size = new Size(newWidth, newHeight);

			//Adjust the viewport for the scroll bars (if necessary)...
			newWidth -= borderWidth;
			if (scrollV.Visible)
				newWidth = scrollV.Left;
			newHeight -= borderHeight;
			if (scrollH.Visible)
				newHeight = scrollH.Top;
			pcViewPort.SetBounds(0, 0, newWidth, newHeight);
			if (!scrollH.Visible && !scrollV.Visible) {
				pcViewPort.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

				pbImage.SetBounds(0, 0, newWidth, newHeight);
				pbImage.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
				pbImage.SizeMode = PictureBoxSizeMode.StretchImage;
			} else {
				pcViewPort.Anchor = AnchorStyles.Top | AnchorStyles.Left;

				pbImage.Anchor = AnchorStyles.Top | AnchorStyles.Left;
				pbImage.SizeMode = PictureBoxSizeMode.Normal;
			}

			//Center form...
			this.SetBounds((Screen.PrimaryScreen.Bounds.Width - this.Width) / 2, (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2, 0, 0, BoundsSpecified.X | BoundsSpecified.Y);
		}
		private string FindComponent(string ComponentName)
		{
			string functionReturnValue = null;
			functionReturnValue = null;
			//First look under where our main application lives (mSupport.ApplicationPath)...
			string testPath = string.Format("{0}\\{1}", mSupport.ApplicationPath, ComponentName);
			if (File.Exists(testPath)){functionReturnValue = testPath;return functionReturnValue;}
			//OK, now look under where the component would live if we were running in a development environment...
			FileInfo fi = new FileInfo(mSupport.ApplicationPath);
			testPath = string.Format("{0}\\{1}\\bin\\{2}", fi.DirectoryName, ParsePath(ComponentName, ParseParts.FileNameBase), ComponentName);
			if (File.Exists(testPath)){functionReturnValue = testPath;return functionReturnValue;}
			return functionReturnValue;
		}
		private void ProtectMissingMenuComponents()
		{
			this.mnuDatabaseBooks.Visible = (FindComponent("TCBooks.NET.dll") != null);
			this.mnuDatabaseCollectables.Visible = (FindComponent("TCCollectables.NET.dll") != null);
			this.mnuDatabaseHobby.Visible = (FindComponent("TCHobby.NET.dll") != null);
			this.mnuDatabaseImages.Visible = (FindComponent("TCImages.NET.dll") != null);
			this.mnuDatabaseMusic.Visible = (FindComponent("TCMusic.NET.dll") != null);
			this.mnuDatabaseSoftware.Visible = (FindComponent("TCSoftware.NET.dll") != null);
			this.mnuDatabaseVideoLibrary.Visible = (FindComponent("TCVideoLibrary.NET.dll") != null);
			this.mnuDatabaseWebLinks.Visible = (FindComponent("TCWebLinks.NET.dll") != null);
			this.mnuDatabaseUSNavy.Visible = (FindComponent("TCUSNavy.NET.dll") != null);
			this.mnuReportsDVDs.Visible = ReportExists("DVDs.rpt");
			this.mnuReportsHistory.Visible = ReportExists("History.rpt");
			this.mnuReportsKitsByLocation.Visible = ReportExists("KitsByLocation.rpt");
			this.mnuReportsKitsForStorage.Visible = ReportExists("KitsForStorage.rpt");
			this.mnuReportsWishList.Visible = ReportExists("WishList.rpt");
		}
		private bool ReportExists(string ReportName)
		{
			return File.Exists(string.Format("{0}\\{1}", mTCBase.ReportsDirectory, ReportName));
		}
		private void ShowMain()
		{
			if (this.Top < 0 || this.Top > Screen.PrimaryScreen.Bounds.Height)
				this.Top = mTCBase.SaveTop;
			else
				mTCBase.SaveTop = this.Top;
			if (this.Left < 0 || this.Left > Screen.PrimaryScreen.Bounds.Width)
				this.Left = mTCBase.SaveLeft;
			else
				mTCBase.SaveLeft = this.Left;
			this.Text = mTCBase.SaveCaption;
			this.Icon = mTCBase.SaveIcon;
			this.Cursor = Cursors.Default;
			this.niMain.Visible = false;
			mTCBase.ActiveForm = this;
			this.Show();
		}
		protected void Shutdown()
		{
			niMain.Visible = false;
			Application.Exit();
		}
		private void Test()
		{
			System.Reflection.Assembly SampleAssembly = null;
			SampleAssembly = System.Reflection.Assembly.LoadFrom("c:\\Sample.Assembly.dll");
			//Obtain a reference to a method known to exist in assembly.
			System.Reflection.MethodInfo Method = SampleAssembly.GetTypes()[0].GetMethod("Method1");
			//Obtain a reference to the parameters collection of the MethodInfo instance.
			System.Reflection.ParameterInfo[] Params = Method.GetParameters();
			//Display information about method parameters.
			//Param = sParam1
			//   Type = System.String
			//   Position = 0
			//   Optional=False
			System.Reflection.ParameterInfo Param = null;
			foreach (ParameterInfo Param_loopVariable in Params) {
				Param = Param_loopVariable;
				Console.WriteLine(("Param=" + Param.Name.ToString()));
				Console.WriteLine(("  Type=" + Param.ParameterType.ToString()));
				Console.WriteLine(("  Position=" + Param.Position.ToString()));
				Console.WriteLine(("  Optional=" + Param.IsOptional.ToString()));
			}
		}
		#endregion
		#region "Event Handlers"
		public void DoSplash(object sender, SplashEventArgs e)
		{
			string Caption = e.Message;
			if (Caption == null && mSplash != null) {
				mSplash.Hide();
				Application.DoEvents();
				mSplash.Close();
				mSplash = null;
				return;
			}
			if (mSplash == null) {
				mSplash = new frmSplash(Caption, this.Left, this.Top, this.Width, this.Height, e.IconImage);
			} else {
				mSplash.UpdateStatus(Caption);
			}
			mSplash.Show();
			Application.DoEvents();
		}
		private void frmMain_Activated(System.Object sender, System.EventArgs e)
		{
			try {
				this.lblUserID.Text = mTCBase.UserID;
				this.lblDatabase.Text = (mTCBase.ConnectionString != bpeNullString ? string.Format("{0}.{1}", mTCBase.ServerName, mTCBase.DatabaseName) : mTCBase.DatabaseName);
				if ((mTCBase.FileDSN == bpeNullString && mTCBase.ConnectionString == bpeNullString) || mTCBase.UserID == bpeNullString || mTCBase.Password == bpeNullString)
					mnuFileOptions_Click(null, null);
				this.Text = string.Format("{0} [DB: {1}]", mSupport.ApplicationName+"C#", (mTCBase.DatabaseName == bpeNullString ? "Not Connected" : string.Format("{0}.{1}", mTCBase.ServerName, mTCBase.DatabaseName)));
				if (fActivated)
					throw new ExitTryException();
				fActivated = true;
				this.Cursor = Cursors.WaitCursor;
				this.sbpVersion.Text = string.Format("Version {0}", mSupport.ApplicationVersion);
				mnuHelpAbout.Text = string.Format("About &{0}...", mSupport.ApplicationName + "C#");
				this.Cursor = Cursors.Default;
            } catch (ExitTryException) { 
            } catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, ex.GetType().Name);
			}
		}
		private void frmMain_Load(System.Object sender, System.EventArgs e)
		{
			try {
				int rTop = 0;
				int rLeft = 0;
				int rHeight = 0;
				int rWidth = 0;

				fActivated = false;
				LoadBackground(mTCBase.ImagePath);
				this.niMain.Visible = false;

				if (Convert.ToBoolean(GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, string.Format("{0}\\{1} Settings", mTCBase.RegistryKey, this.Name), "DimensionsSaved", false))) {
					rTop = (int)GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, string.Format("{0}\\{1} Settings", mTCBase.RegistryKey, this.Name), "Top", this.Top);
					rLeft = (int)GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, string.Format("{0}\\{1} Settings", mTCBase.RegistryKey, this.Name), "Left", this.Left);
					rHeight = (int)GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, string.Format("{0}\\{1} Settings", mTCBase.RegistryKey, this.Name), "Height", this.Height);
					rWidth = (int)GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, string.Format("{0}\\{1} Settings", mTCBase.RegistryKey, this.Name), "Width", this.Width);
				}
				if (rTop > 0 && rTop <= Screen.PrimaryScreen.Bounds.Height)
					this.Top = rTop;
				if (rLeft > 0 && rLeft <= Screen.PrimaryScreen.Bounds.Height)
					this.Left = rLeft;
				mTCBase.SaveTop = this.Top;
				mTCBase.SaveLeft = this.Left;
				mTCBase.SaveCaption = this.Text;
				mTCBase.SaveIcon = this.Icon;
				//Setup our ContextMenu...
				this.ctxMain.MenuItems.AddRange(new MenuItem[] {
					new MenuItem(this.mnuFileOptions.Text, mnuFileOptions_Click),
					new MenuItem(this.mnuFileTrace.Text, mnuFileTrace_Click),
					new MenuItem(this.mnuFileSep.Text),
					new MenuItem(this.mnuFileExit.Text, mnuFileExit_Click)
				});

				mnuFile.MenuItems.Clear();
				mnuFileOptions = mnuFile.MenuItems[mnuFile.MenuItems.Add(new clsIconMenuItem("&Options", "Verdana", 10, clsTCBase.ImageToIcon(base.imgBase.Images[(int)imgMainEnum.Options]), new EventHandler(this.mnuFileOptions_Click)))];
				mnuFileTrace = mnuFile.MenuItems[mnuFile.MenuItems.Add(new clsIconMenuItem("&Trace", "Verdana", 10, (mSupport.Trace.TraceMode ? clsTCBase.ImageToIcon(base.imgBase.Images[(int)imgMainEnum.Trace]) : null), new EventHandler(this.mnuFileTrace_Click)))];
				mnuFileTrace.Checked = mSupport.Trace.TraceMode;
				mnuFileSep = mnuFile.MenuItems[mnuFile.MenuItems.Add(new MenuItem("-"))];
				mnuFileExit = mnuFile.MenuItems[mnuFile.MenuItems.Add(new clsIconMenuItem("E&xit", "Verdana", 10, null, new EventHandler(this.mnuFileExit_Click), Shortcut.AltF4))];

				mnuDatabase.MenuItems.Clear();
				mnuDatabaseBooks = mnuDatabase.MenuItems[mnuDatabase.MenuItems.Add(new clsIconMenuItem("&Books", "Verdana", 10, clsTCBase.ImageToIcon(base.imgBase.Images[(int)imgMainEnum.Books]), new EventHandler(this.mnuDatabaseBooks_Click)))];
				mnuDatabaseCollectables = mnuDatabase.MenuItems[mnuDatabase.MenuItems.Add(new clsIconMenuItem("&Collectables", "Verdana", 10, clsTCBase.ImageToIcon(base.imgBase.Images[(int)imgMainEnum.Collectables]), new EventHandler(this.mnuDatabaseCollectables_Click)))];
				mnuDatabaseHobby.MenuItems.Clear();
				mnuDatabaseHobby = mnuDatabase.MenuItems[mnuDatabase.MenuItems.Add(new clsIconMenuItem("&Hobby", "Verdana", 10, clsTCBase.ImageToIcon(base.imgBase.Images[(int)imgMainEnum.HobbyKits])))];
				mnuDatabaseHobbyKits = mnuDatabaseHobby.MenuItems[mnuDatabaseHobby.MenuItems.Add(new clsIconMenuItem("&Kits", "Verdana", 10, clsTCBase.ImageToIcon(base.imgBase.Images[(int)imgMainEnum.HobbyKits]), new EventHandler(this.mnuDatabaseHobbyKits_Click)))];
				mnuDatabaseHobbyDecals = mnuDatabaseHobby.MenuItems[mnuDatabaseHobby.MenuItems.Add(new clsIconMenuItem("&Decals", "Verdana", 10, clsTCBase.ImageToIcon(base.imgBase.Images[(int)imgMainEnum.HobbyDecals]), new EventHandler(this.mnuDatabaseHobbyDecals_Click)))];
				mnuDatabaseHobbyDetailSets = mnuDatabaseHobby.MenuItems[mnuDatabaseHobby.MenuItems.Add(new clsIconMenuItem("Detail &Sets", "Verdana", 10, clsTCBase.ImageToIcon(base.imgBase.Images[(int)imgMainEnum.HobbyDetailSets]), new EventHandler(this.mnuDatabaseHobbyDetailSets_Click)))];
				mnuDatabaseHobbyFinishingProducts = mnuDatabaseHobby.MenuItems[mnuDatabaseHobby.MenuItems.Add(new clsIconMenuItem("&Finishing Products", "Verdana", 10, clsTCBase.ImageToIcon(base.imgBase.Images[(int)imgMainEnum.HobbyFinishingProducts]), new EventHandler(this.mnuDatabaseHobbyFinishingProducts_Click)))];
				mnuDatabaseHobbySep1 = mnuDatabaseHobby.MenuItems[mnuDatabaseHobby.MenuItems.Add(new MenuItem("-"))];
				mnuDatabaseHobbyTools = mnuDatabaseHobby.MenuItems[mnuDatabaseHobby.MenuItems.Add(new clsIconMenuItem("&Tools", "Verdana", 10, clsTCBase.ImageToIcon(base.imgBase.Images[(int)imgMainEnum.HobbyTools]), new EventHandler(this.mnuDatabaseHobbyTools_Click)))];
				mnuDatabaseHobbyVideoResearch = mnuDatabaseHobby.MenuItems[mnuDatabaseHobby.MenuItems.Add(new clsIconMenuItem("&Video Research", "Verdana", 10, clsTCBase.ImageToIcon(base.imgBase.Images[(int)imgMainEnum.HobbyVideoResearch]), new EventHandler(this.mnuDatabaseHobbyVideoResearch_Click)))];
				mnuDatabaseHobbySep2 = mnuDatabaseHobby.MenuItems[mnuDatabaseHobby.MenuItems.Add(new MenuItem("-"))];
				mnuDatabaseHobbyRockets = mnuDatabaseHobby.MenuItems[mnuDatabaseHobby.MenuItems.Add(new clsIconMenuItem("&Rockets", "Verdana", 10, clsTCBase.ImageToIcon(base.imgBase.Images[(int)imgMainEnum.HobbyRockets]), new EventHandler(this.mnuDatabaseHobbyRockets_Click)))];
				mnuDatabaseHobbyTrains = mnuDatabaseHobby.MenuItems[mnuDatabaseHobby.MenuItems.Add(new clsIconMenuItem("&Trains", "Verdana", 10, clsTCBase.ImageToIcon(base.imgBase.Images[(int)imgMainEnum.HobbyTrains]), new EventHandler(this.mnuDatabaseHobbyTrains_Click)))];
				mnuDatabaseHobbySep3 = mnuDatabaseHobby.MenuItems[mnuDatabaseHobby.MenuItems.Add(new MenuItem("-"))];
				mnuDatabaseHobbyCompanies = mnuDatabaseHobby.MenuItems[mnuDatabaseHobby.MenuItems.Add(new clsIconMenuItem("&Companies", "Verdana", 10, clsTCBase.ImageToIcon(base.imgBase.Images[(int)imgMainEnum.HobbyCompanies]), new EventHandler(this.mnuDatabaseHobbyCompanies_Click)))];
				mnuDatabaseHobbySep4 = mnuDatabaseHobby.MenuItems[mnuDatabaseHobby.MenuItems.Add(new MenuItem("-"))];
				mnuDatabaseHobbyAircraftDesignations = mnuDatabaseHobby.MenuItems[mnuDatabaseHobby.MenuItems.Add(new clsIconMenuItem("&Aircraft Designations", "Verdana", 10, clsTCBase.ImageToIcon(base.imgBase.Images[(int)imgMainEnum.HobbyAircraftDesignations]), new EventHandler(this.mnuDatabaseHobbyAircraftDesignations_Click)))];
				mnuDatabaseHobbyBlueAngelsHistory = mnuDatabaseHobby.MenuItems[mnuDatabaseHobby.MenuItems.Add(new clsIconMenuItem("&Blue Angels History", "Verdana", 10, clsTCBase.ImageToIcon(base.imgBase.Images[(int)imgMainEnum.HobbyBlueAngelsHistory]), new EventHandler(this.mnuDatabaseHobbyBlueAngelsHistory_Click)))];
				mnuDatabaseImages = mnuDatabase.MenuItems[mnuDatabase.MenuItems.Add(new clsIconMenuItem("&Images", "Verdana", 10, clsTCBase.ImageToIcon(base.imgBase.Images[(int)imgMainEnum.Images]), new EventHandler(this.mnuDatabaseImages_Click)))];
				mnuDatabaseMusic = mnuDatabase.MenuItems[mnuDatabase.MenuItems.Add(new clsIconMenuItem("&Music", "Verdana", 10, clsTCBase.ImageToIcon(base.imgBase.Images[(int)imgMainEnum.Music]), new EventHandler(this.mnuDatabaseMusic_Click)))];
				mnuDatabaseSoftware = mnuDatabase.MenuItems[mnuDatabase.MenuItems.Add(new clsIconMenuItem("&Software", "Verdana", 10, clsTCBase.ImageToIcon(base.imgBase.Images[(int)imgMainEnum.Software]), new EventHandler(this.mnuDatabaseSoftware_Click)))];
				mnuDatabaseUSNavy.MenuItems.Clear();
				mnuDatabaseUSNavy = mnuDatabase.MenuItems[mnuDatabase.MenuItems.Add(new clsIconMenuItem("&U.S. Navy", "Verdana", 10, clsTCBase.ImageToIcon(base.imgBase.Images[(int)imgMainEnum.USNavy])))];
				mnuDatabaseUSNavyShips = mnuDatabaseUSNavy.MenuItems[mnuDatabaseUSNavy.MenuItems.Add(new clsIconMenuItem("&Ships", "Verdana", 10, clsTCBase.ImageToIcon(base.imgBase.Images[(int)imgMainEnum.USNavy]), new EventHandler(this.mnuDatabaseUSNavyShips_Click)))];
				mnuDatabaseUSNavySep = mnuDatabaseUSNavy.MenuItems[mnuDatabaseUSNavy.MenuItems.Add(new MenuItem("-"))];
				mnuDatabaseUSNavyClasses = mnuDatabaseUSNavy.MenuItems[mnuDatabaseUSNavy.MenuItems.Add(new clsIconMenuItem("&Classes", "Verdana", 10, clsTCBase.ImageToIcon(base.imgBase.Images[(int)imgMainEnum.USNavy]), new EventHandler(this.mnuDatabaseUSNavyClasses_Click)))];
				mnuDatabaseUSNavyClassifications = mnuDatabaseUSNavy.MenuItems[mnuDatabaseUSNavy.MenuItems.Add(new clsIconMenuItem("&Classifications", "Verdana", 10, clsTCBase.ImageToIcon(base.imgBase.Images[(int)imgMainEnum.USNavy]), new EventHandler(this.mnuDatabaseUSNavyClassifications_Click)))];
				mnuDatabaseVideoLibrary.MenuItems.Clear();
				mnuDatabaseVideoLibrary = mnuDatabase.MenuItems[mnuDatabase.MenuItems.Add(new clsIconMenuItem("&Video Library", "Verdana", 10, clsTCBase.ImageToIcon(base.imgBase.Images[(int)imgMainEnum.VideoLibraryMovies])))];
				mnuDatabaseVideoLibraryMovies = mnuDatabaseVideoLibrary.MenuItems[mnuDatabaseVideoLibrary.MenuItems.Add(new clsIconMenuItem("&Movies", "Verdana", 10, clsTCBase.ImageToIcon(base.imgBase.Images[(int)imgMainEnum.VideoLibraryMovies]), new EventHandler(this.mnuDatabaseVideoLibraryMovies_Click)))];
				mnuDatabaseVideoLibrarySpecials = mnuDatabaseVideoLibrary.MenuItems[mnuDatabaseVideoLibrary.MenuItems.Add(new clsIconMenuItem("&Specials", "Verdana", 10, clsTCBase.ImageToIcon(base.imgBase.Images[(int)imgMainEnum.VideoLibrarySpecials]), new EventHandler(this.mnuDatabaseVideoLibrarySpecials_Click)))];
				mnuDatabaseVideoLibraryTVEpisodes = mnuDatabaseVideoLibrary.MenuItems[mnuDatabaseVideoLibrary.MenuItems.Add(new clsIconMenuItem("&TV Episodes", "Verdana", 10, clsTCBase.ImageToIcon(base.imgBase.Images[(int)imgMainEnum.VideoLibraryTVEpisodes]), new EventHandler(this.mnuDatabaseVideoLibraryTVEpisodes_Click)))];
				mnuDatabaseWebLinks = mnuDatabase.MenuItems[mnuDatabase.MenuItems.Add(new clsIconMenuItem("&Web Links", "Verdana", 10, clsTCBase.ImageToIcon(base.imgBase.Images[(int)imgMainEnum.WebLinks]), new EventHandler(this.mnuDatabaseWebLinks_Click)))];

				mnuReports.MenuItems.Clear();
				mnuReportsDVDs = mnuReports.MenuItems[mnuReports.MenuItems.Add(new clsIconMenuItem("&DVDs", "Verdana", 10, clsTCBase.ImageToIcon(base.imgBase.Images[(int)imgMainEnum.CrystalReportsXI]), new EventHandler(this.mnuReportsDVDs_Click)))];
				mnuReportsHistory = mnuReports.MenuItems[mnuReports.MenuItems.Add(new clsIconMenuItem("&History", "Verdana", 10, clsTCBase.ImageToIcon(base.imgBase.Images[(int)imgMainEnum.CrystalReportsXI]), new EventHandler(this.mnuReportsHistory_Click)))];
				mnuReportsKitsByLocation = mnuReports.MenuItems[mnuReports.MenuItems.Add(new clsIconMenuItem("Model Kits by &Location", "Verdana", 10, clsTCBase.ImageToIcon(base.imgBase.Images[(int)imgMainEnum.CrystalReportsXI]), new EventHandler(this.mnuReportsKitsByLocation_Click)))];
				mnuReportsKitsForStorage = mnuReports.MenuItems[mnuReports.MenuItems.Add(new clsIconMenuItem("Model &Kits for Storage", "Verdana", 10, clsTCBase.ImageToIcon(base.imgBase.Images[(int)imgMainEnum.CrystalReportsXI]), new EventHandler(this.mnuReportsKitsForStorage_Click)))];
				mnuReportsWishList = mnuReports.MenuItems[mnuReports.MenuItems.Add(new clsIconMenuItem("&WishList", "Verdana", 10, clsTCBase.ImageToIcon(base.imgBase.Images[(int)imgMainEnum.CrystalReportsXI]), new EventHandler(this.mnuReportsWishList_Click)))];

				mnuHelp.MenuItems.Clear();
				mnuHelpAbout = mnuHelp.MenuItems[mnuHelp.MenuItems.Add(new clsIconMenuItem("&About...", "Verdana", 10, clsTCBase.ImageToIcon(base.imgBase.Images[(int)imgMainEnum.TreasureChest]), new EventHandler(this.mnuHelpAbout_Click)))];

				ProtectMissingMenuComponents();
				this.FormBorderStyle = FormBorderStyle.None;
				mTCBase.DynamicMenuHeight = this.Size.Height - this.ClientSize.Height;
				this.FormBorderStyle = FormBorderStyle.Sizable;
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, ex.GetType().Name);
			}
		}
		private void frmMain_Closed(System.Object sender, System.EventArgs e)
		{
			try {
				Trace(mSupport.ApplicationName + " Exit.", trcOption.trcApplication);
				Trace(new string('=', 132), trcOption.trcApplication);
				ShowMain();
				SaveRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, string.Format("{0}\\{1} Settings", mTCBase.RegistryKey, this.Name), "DimensionsSaved", true);
				SaveRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, string.Format("{0}\\{1} Settings", mTCBase.RegistryKey, this.Name), "Top", this.Top);
				SaveRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, string.Format("{0}\\{1} Settings", mTCBase.RegistryKey, this.Name), "Left", this.Left);
				SaveRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, string.Format("{0}\\{1} Settings", mTCBase.RegistryKey, this.Name), "Height", this.Height);
				SaveRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, string.Format("{0}\\{1} Settings", mTCBase.RegistryKey, this.Name), "Width", this.Width);
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, ex.GetType().Name);
			}
			mTCBase = null;
		}
		private void frmMain_Resize(System.Object sender, System.EventArgs e)
		{
			try {
				if (this.WindowState != FormWindowState.Minimized) {
					if (scrollH.Visible) {
						scrollH.Maximum = pbImage.Width - this.ClientSize.Width;
						scrollH.SmallChange = pbImage.Width / 100;
						scrollH.LargeChange = pbImage.Width / 20;
					}

					if (scrollV.Visible) {
						scrollV.Maximum = pbImage.Height - this.ClientSize.Height;
						scrollV.SmallChange = pbImage.Height / 100;
						scrollV.LargeChange = pbImage.Height / 20;
					}
				}
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, ex.GetType().Name);
			}
		}
		#region "Menu Handlers"
		private void mnuDatabaseBooks_Click(object sender, System.EventArgs e)
		{
			try {
				this.DoMenu("TCBooks.NET.dll", "TCBooks.clsBooks", "Books");
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, ex.GetType().Name);
			}
		}
		private void mnuDatabaseCollectables_Click(object sender, System.EventArgs e)
		{
			try {
				this.DoMenu("TCCollectables.NET.dll", "TCCollectables.clsCollectables", "Collectables");
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, ex.GetType().Name);
			}
		}
		private void mnuDatabaseHobbyKits_Click(object sender, System.EventArgs e)
		{
			try {
				this.DoMenu("TCHobby.NET.dll", "TCHobby.clsKits", "Kits");
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, ex.GetType().Name);
			}
		}
		private void mnuDatabaseHobbyDecals_Click(object sender, System.EventArgs e)
		{
			try {
				this.DoMenu("TCHobby.NET.dll", "TCHobby.clsDecals", "Decals");
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, ex.GetType().Name);
			}
		}
		private void mnuDatabaseHobbyDetailSets_Click(object sender, System.EventArgs e)
		{
			try {
				this.DoMenu("TCHobby.NET.dll", "TCHobby.clsDetailSets", "Detail Sets");
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, ex.GetType().Name);
			}
		}
		private void mnuDatabaseHobbyFinishingProducts_Click(object sender, System.EventArgs e)
		{
			try {
				this.DoMenu("TCHobby.NET.dll", "TCHobby.clsFinishingProducts", "Finishing Products");
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, ex.GetType().Name);
			}
		}
		private void mnuDatabaseHobbyTools_Click(object sender, System.EventArgs e)
		{
			try {
				this.DoMenu("TCHobby.NET.dll", "TCHobby.clsTools", "Tools");
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, ex.GetType().Name);
			}
		}
		private void mnuDatabaseHobbyVideoResearch_Click(object sender, System.EventArgs e)
		{
			try {
				this.DoMenu("TCHobby.NET.dll", "TCHobby.clsVideoResearch", "Video Research");
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, ex.GetType().Name);
			}
		}
		private void mnuDatabaseHobbyRockets_Click(object sender, System.EventArgs e)
		{
			try {
				this.DoMenu("TCHobby.NET.dll", "TCHobby.clsRockets", "Rockets");
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, ex.GetType().Name);
			}
		}
		private void mnuDatabaseHobbyTrains_Click(object sender, System.EventArgs e)
		{
			try {
				this.DoMenu("TCHobby.NET.dll", "TCHobby.clsTrains", "Trains");
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, ex.GetType().Name);
			}
		}
		private void mnuDatabaseHobbyCompanies_Click(object sender, System.EventArgs e)
		{
			try {
				this.DoMenu("TCHobby.NET.dll", "TCHobby.clsCompanies", "Companies");
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, ex.GetType().Name);
			}
		}
		private void mnuDatabaseHobbyAircraftDesignations_Click(object sender, System.EventArgs e)
		{
			try {
				this.DoMenu("TCHobby.NET.dll", "TCHobby.clsAircraftDesignations", "Aircraft Designations");
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, ex.GetType().Name);
			}
		}
		private void mnuDatabaseHobbyBlueAngelsHistory_Click(object sender, System.EventArgs e)
		{
			try {
				this.DoMenu("TCHobby.NET.dll", "TCHobby.clsBlueAngelsHistory", "Blue Angels History");
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, ex.GetType().Name);
			}
		}
		private void mnuDatabaseImages_Click(object sender, System.EventArgs e)
		{
			try {
				this.DoMenu("TCImages.NET.dll", "TCImages.clsImages", "Images");
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, ex.GetType().Name);
			}
		}
		private void mnuDatabaseMusic_Click(object sender, System.EventArgs e)
		{
			try {
				this.DoMenu("TCMusic.NET.dll", "TCMusic.clsMusic", "Music");
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, ex.GetType().Name);
			}
		}
		private void mnuDatabaseSoftware_Click(object sender, System.EventArgs e)
		{
			try {
				this.DoMenu("TCSoftware.NET.dll", "TCSoftware.clsSoftware", "Software");
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, ex.GetType().Name);
			}
		}
		private void mnuDatabaseUSNavyClasses_Click(System.Object sender, System.EventArgs e)
		{
			try {
				this.DoMenu("TCUSNavy.NET.dll", "TCUSNavy.clsClasses", "Classes");
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, ex.GetType().Name);
			}
		}
		private void mnuDatabaseUSNavyClassifications_Click(System.Object sender, System.EventArgs e)
		{
			try {
				this.DoMenu("TCUSNavy.NET.dll", "TCUSNavy.clsClassifications", "Classifications");
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, ex.GetType().Name);
			}
		}
		private void mnuDatabaseUSNavyShips_Click(System.Object sender, System.EventArgs e)
		{
			try {
				this.DoMenu("TCUSNavy.NET.dll", "TCUSNavy.clsShips", "Ships");
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, ex.GetType().Name);
			}
		}
		private void mnuDatabaseVideoLibraryMovies_Click(object sender, System.EventArgs e)
		{
			try {
				this.DoMenu("TCVideoLibrary.NET.dll", "TCVideoLibrary.clsMovies", "Movies");
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, ex.GetType().Name);
			}
		}
		private void mnuDatabaseVideoLibrarySpecials_Click(object sender, System.EventArgs e)
		{
			try {
				this.DoMenu("TCVideoLibrary.NET.dll", "TCVideoLibrary.clsSpecials", "Specials");
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, ex.GetType().Name);
			}
		}
		private void mnuDatabaseVideoLibraryTVEpisodes_Click(object sender, System.EventArgs e)
		{
			try {
				this.DoMenu("TCVideoLibrary.NET.dll", "TCVideoLibrary.clsTVEpisodes", "TV Episodes");
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, ex.GetType().Name);
			}
		}
		private void mnuDatabaseWebLinks_Click(object sender, System.EventArgs e)
		{
			try {
				this.DoMenu("TCWebLinks.NET.dll", "TCWebLinks.clsWebLinks", "WebLinks");
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, ex.GetType().Name);
			}
		}
		private void mnuFileOptions_Click(System.Object sender, System.EventArgs e)
		{
			const string EntryName = "mnuFileOptions_Click";
			try {
				frmOptions frm = new frmOptions(mSupport, mTCBase, this);
				frm.ShowDialog();
				LoadBackground(mTCBase.ImagePath);
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, EntryName);
			}
		}
		private void mnuFileExit_Click(System.Object sender, System.EventArgs e)
		{
			const string EntryName = "mnuFileExit_Click";
			try {
				Shutdown();
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, EntryName);
			}
		}
		protected override void mnuFileTrace_Click(object sender, System.EventArgs e)
		{
			var _with1 = (MenuItem)sender;
			base.mnuFileTrace_Click(sender, e);
			this.sbpTrace.Text = base.TraceText;
			this.sbpTrace.Width = base.TraceWidth;
		}
		private void mnuHelpAbout_Click(object sender, System.EventArgs e)
		{
			try {
				frmAbout frm = new frmAbout(mSupport, mTCBase, this);
				frm.ShowDialog();
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, ex.GetType().Name);
			}
		}
		private void mnuReportsDVDs_Click(object sender, System.EventArgs e)
		{
			try {
				mTCBase.ActiveForm = this;
				mTCBase.ReportCommand(string.Format("{0}\\{1}", mTCBase.ReportsDirectory, "DVDs.rpt"), "DVDs Report");
				mTCBase.ActiveForm = null;
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, ex.GetType().Name);
			}
		}
		private void mnuReportsHistory_Click(object sender, System.EventArgs e)
		{
			try {
				mTCBase.ActiveForm = this;
				mTCBase.ReportCommand(string.Format("{0}\\{1}", mTCBase.ReportsDirectory, "History.rpt"), "History Report");
				mTCBase.ActiveForm = null;
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, ex.GetType().Name);
			}
		}
		private void mnuReportsKitsByLocation_Click(object sender, System.EventArgs e)
		{
			try {
				mTCBase.ActiveForm = this;
				mTCBase.ReportCommand(string.Format("{0}\\{1}", mTCBase.ReportsDirectory, "KitsByLocation.rpt"), "Model Kits by Location Report");
				mTCBase.ActiveForm = null;
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, ex.GetType().Name);
			}
		}
		private void mnuReportsKitsForStorage_Click(object sender, System.EventArgs e)
		{
			try {
				mTCBase.ActiveForm = this;
				mTCBase.ReportCommand(string.Format("{0}\\{1}", mTCBase.ReportsDirectory, "KitsForStorage.rpt"), "Model Kits for Storage Report");
				mTCBase.ActiveForm = null;
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, ex.GetType().Name);
			}
		}
		private void mnuReportsWishList_Click(object sender, System.EventArgs e)
		{
			try {
				mTCBase.ActiveForm = this;
				mTCBase.ReportCommand(string.Format("{0}\\{1}", mTCBase.ReportsDirectory, "WishList.rpt"), "WishList Report");
				mTCBase.ActiveForm = null;
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, ex.GetType().Name);
			}
		}
		#endregion
		private void niMain_DoubleClick(object sender, System.EventArgs e)
		{
			try {
				niMain.Visible = false;
				this.Show();
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, ex.GetType().Name);
			}
		}
		private void scrollH_Scroll(System.Object sender, ScrollEventArgs e)
		{
			try {
				switch (e.Type) {
					case ScrollEventType.EndScroll:
						int newScrollValue = e.NewValue;
						this.pbImage.Left = -newScrollValue;
						break;
				}
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, ex.GetType().Name);
			}
		}
		private void scrollV_Scroll(System.Object sender, ScrollEventArgs e)
		{
			try {
				switch (e.Type) {
					case ScrollEventType.EndScroll:
						int newScrollValue = e.NewValue;
						this.pbImage.Top = -newScrollValue;
						break;
				}
			} catch (Exception ex) {
				Trace(ex.Message, trcOption.trcAll);
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, ex.GetType().Name);
			}
		}
		protected virtual void timClock_Tick(System.Object sender, System.EventArgs e)
		{
			try {
				this.sbpTime.Text = string.Format("{0:t}", DateAndTime.Now);
			} catch (Exception ex) {
			}
		}
		#endregion
	}
}

//frmSupportBase.vb
//   TreasureChest2 Support Base Form Definition...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   12/19/17    Implemented additional Trace Support;
//   10/01/16    Changed GetReistrySetting's vDefault parameter to be passed ByVal;
//   12/03/15    Removed mSupport from destructor code so it will no longer rip the mSupport object out from under other 
//               components on the close of a single screen;
//   12/01/14    Reworked ParseStr and TokenCount to accommodate encapsulated sections of strings with embedded delimiters 
//               (correcting gaps in functionality);
//   03/13/14    Upgraded project to Visual Studio 2005;
//   01/25/14    Reviewed Memory-related tracing;
//   01/17/14    Changed optional TrackElapsedTime argument to RecordEntry from False to True;
//   02/27/13    Removed ill-advised IFF function as it behaves no different than VB's version;
//   01/25/13    Changed FixQuotes strA argument from String to Object to support handling of DBNull;
//   11/02/12    Introduced local IIF function to avoid VB's version that always evaluates both TruePart and FalsePart arguments;
//   07/12/12    Reverted to Visual Studio 2003/Visual Basic 7.1 due to incompatibility issues with Crystal Reports XI;
//   07/09/12    Upgraded to Visual Studio 2005/Visual Basic 8.0;
//   05/16/12    Introduced IsException function;
//   01/12/12    Introduced AlignRights(Control,Control);
//   01/11/12    Added TagRemove and TagSet methods;
//   11/16/05    Added new System.Exception-based RaiseError;
//   10/03/05    Added CleanUpTrace();
//   09/27/05    Added FormatBytes();
//   09/18/05    Added Thread and ThreadID properties;
//   05/17/05    Added Product & Title properties;
//   04/11/05    Created;
//=================================================================================================================================
 // ERROR: Not supported in C#: OptionDeclaration
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Compatibility;
using Microsoft.Win32;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data.Linq.SqlClient;
using System.Data.Linq.SqlClient.Implementation;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Management;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;
using TCBase;
using static TCBase.clsRegistry;
using static TCBase.clsString;
using static TCBase.clsSupport;
using static TCBase.clsTCBase;
using static TCBase.clsTrace;
using static TCBase.clsUI;
namespace TCBase
{
	public class frmSupportBase : System.Windows.Forms.Form
	{
		public frmSupportBase() : base()
		{
			mSupport = new clsSupport();
			mMyModuleName = "frmSupportBase";
			mMyParent = null;
			mMyTraceID = string.Format("{0}.{1}.{2}", mSupport.ApplicationName, mSupport.ExecutingComponentName, mMyModuleName);

			//This call is required by the Windows Form Designer.
			InitializeComponent();
		}
		public frmSupportBase(clsSupport objSupport, string ModuleName, Form objParent = null) : base()
		{
			mSupport = objSupport;
			mMyModuleName = ModuleName;
			mMyParent = objParent;
			mMyTraceID = string.Format("{0}.{1}.{2}", mSupport.ApplicationName, System.Reflection.Assembly.GetCallingAssembly().GetName().Name.ToString(), mMyModuleName);
			mMyAssemblyInfo = new AssemblyInfo(System.Reflection.Assembly.GetCallingAssembly());

			//This call is required by the Windows Form Designer.
			InitializeComponent();
		}

		#region " Windows Form Designer generated code "
		//Required by the Windows Form Designer

		private System.ComponentModel.IContainer components;
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.  
		//Do not modify it using the code editor.
		public ctlBPEErrorProvider epBase;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.epBase = new ctlBPEErrorProvider();
			//
			//epBase
			//
			this.epBase.ContainerControl = this;
			//
			//frmSupportBase
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Name = "frmSupportBase";
			this.Text = "frmSupportBase";

		}
		#endregion
		#region "Events"
		//None at this time...
		#endregion
		#region "Properties"
		#region "Trace Support"
		protected clsSupport mSupport;
		protected AssemblyInfo mMyAssemblyInfo;
		protected string mMyModuleName;
		protected Form mMyParent;
		protected string mMyTraceID;
		protected string MyCodeBase {
			get { return mMyAssemblyInfo.CodeBase; }
		}
		protected string MyCompany {
			get { return mMyAssemblyInfo.Company; }
		}
		protected string MyCopyright {
			get { return mMyAssemblyInfo.Copyright; }
		}
		protected string MyDescription {
			get { return mMyAssemblyInfo.Description; }
		}
		protected string MyFullName {
			get { return mMyAssemblyInfo.FullName; }
		}
		protected string MyModuleName {
			get { return mMyModuleName; }
		}
		protected string MyName {
			get { return mMyAssemblyInfo.Name; }
		}
		protected Form MyParent {
			get { return mMyParent; }
		}
		protected string MyProduct {
			get { return mMyAssemblyInfo.Product; }
		}
		protected string MyTitle {
			get { return mMyAssemblyInfo.Title; }
		}
		protected string MyTraceID {
			get { return mMyTraceID; }
		}
		protected string MyVersion {
			get { return mMyAssemblyInfo.Version; }
		}
		#endregion
		private Thread mThread = System.Threading.Thread.CurrentThread;
		private int mThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
		public const string bpeNullString = "";
		protected Thread Thread {
			get { return mThread; }
		}
		protected int ThreadID {
			get { return mThreadID; }
		}
		public string TraceFile {
			get { return mSupport.Trace.TraceFile; }
			set { mSupport.Trace.TraceFile = value; }
		}
		public bool TraceMode {
			get { return mSupport.Trace.TraceMode; }
			set { mSupport.Trace.TraceMode = value; }
		}
		public trcOption TraceOptions {
			get { return mSupport.Trace.TraceOptions; }
			set { mSupport.Trace.TraceOptions = value; }
		}
		#endregion
		#region "Methods"
		#region "Destructor"
		protected bool bDisposed = false;
		//Form overrides dispose to clean up the component list.
		protected void Dispose()
		{
			try {
				//Debug.WriteLine("[" & AppDomain.GetCurrentThreadId & "]" & vbTab & TraceID & ".Dispose()")
				Dispose(true);
				//Debug.WriteLine("[" & AppDomain.GetCurrentThreadId & "] " & mMyTraceID & ".Dispose(): GC.SuppressingFinalize(Me)")
				GC.SuppressFinalize(this);
			} catch (Exception ex) {
				throw;
			}
		}
		protected override void Dispose(bool Disposing)
		{
			try {
				if (bDisposed)
					return;
				if (Disposing) {
					//Object is being disposed, not finalized.
					//It is safe to access other objects (other than the base object) only from inside this block.
					Trace("Disposing: " + mMyTraceID, trcOption.trcMemory | trcOption.trcSupport);
					if ((components != null))
						components.Dispose();
				}
			} catch (Exception ex) {
				throw;
			} finally {
				base.Dispose(Disposing);
				mSupport = null;
				bDisposed = true;
			}
		}
		#endregion
		public void AlignRights(Control Control1, Control Control2)
		{
			mSupport.FieldEdits.AlignRights(Control1, Control2);
		}
		public void ControlEnabled(Control ctl, bool Enabled)
		{
			mSupport.FieldEdits.ControlEnabled(ctl, Enabled);
		}
		public void ControlSetFocus(Control ctl)
		{
			mSupport.FieldEdits.ControlSetFocus(ctl);
		}
		//public void xDisplay(string strMessage, ref clsUI.DisplayEnum Location, ref vbRGBColorConstants FontColor = vbRGBColorConstants.vbUndefinedColor, ref TriState FontBold = TriState.UseDefault, ref TriState FontItalic = TriState.UseDefault, ref string FontName = bpeNullString, ref float FontSize = 0)
		//{
		//	if ((mSupport != null)) {
		//		if ((mSupport.UI != null)) {
		//			mSupport.UI.Display(strMessage, Location, FontColor, FontBold, FontItalic, FontName, FontSize);
		//		}
		//	}
		//}
		public decimal FixMoney(decimal cAmount, short Decimals, clsFieldEdits.rrRoundingRule RoundingRule = clsFieldEdits.rrRoundingRule.rrStandard)
		{
			return mSupport.FieldEdits.FixMoney(cAmount, Decimals, RoundingRule);
		}
		public string FixQuotes(object strA, string strDelim = "'")
		{
			//, cntAdded As Integer) As Variant
			return mSupport.FieldEdits.FixQuotes(strA, strDelim);
		}
		public string FormatBytes(double TotalBytes)
		{
			return mSupport.FieldEdits.FormatBytes(TotalBytes);
		}
		public string GetINIKey(string File, string Section, string Key, string defaultValue)
		{
			string functionReturnValue = null;
			functionReturnValue = bpeNullString;
			if ((mSupport != null) && (mSupport.Registry != null))
				return mSupport.Registry.GetINIKey(File, Section, Key, defaultValue);
			return functionReturnValue;
		}
		public object GetRegistrySetting(clsRegistry.RootKeyConstants RootKey, string Key, string Value, object vDefault)
		{
			object functionReturnValue = null;
			functionReturnValue = null;
			if ((mSupport != null) && (mSupport.Registry != null))
				return mSupport.Registry.GetRegistrySetting(RootKey, Key, Value, vDefault);
			return functionReturnValue;
		}
		public void KeyPressUcase(ref short KeyAscii)
		{
			mSupport.FieldEdits.KeyPressUcase(ref KeyAscii);
		}
		public void KeyPressInteger(ref short KeyAscii)
		{
			mSupport.FieldEdits.KeyPressInteger(ref KeyAscii);
		}
		public void KeyPressReal(ref short KeyAscii)
		{
			mSupport.FieldEdits.KeyPressReal(ref KeyAscii);
		}
		public string LeftFill(string Source, short iLength, char FillChar = ' ')
		{
			return mSupport.FieldEdits.LeftFill(Source, iLength, FillChar);
		}
		public string LogMessage(string FileName, string Message, short Indent, bool fSkipFormat = false)
		{
			string functionReturnValue = null;
			functionReturnValue = bpeNullString;
			if ((mSupport != null) && (mSupport.Trace != null))
				return mSupport.Trace.LogMessage(FileName, Message, Indent, fSkipFormat);
			return functionReturnValue;
		}
        protected string ParsePath(string strPath, ParseParts intPart)
        {
            return mSupport.ParsePath(strPath, intPart);
        }
        public string ParseStr(string Source, int TokenNum, string Delimiter, string Encapsulator = bpeNullString, bool Preserve = false)
		{
			return mSupport.Strings.ParseStr(Source, TokenNum, Delimiter, Encapsulator, Preserve);
		}
		public string RightFill(string Source, short iLength, char FillChar = ' ')
		{
			return mSupport.FieldEdits.RightFill(Source, iLength, FillChar);
		}
		public void SaveINIKey(string File, string Section, string Key, string Value)
		{
			if ((mSupport != null)) {
				if ((mSupport.Registry != null)) {
					mSupport.Registry.SaveINIKey(File, Section, Key, Value);
				}
			}
		}
		public void SaveRegistrySetting(clsRegistry.RootKeyConstants RootKey, string Key, string Value, object Data)
		{
			if ((mSupport != null)) {
				if ((mSupport.Registry != null)) {
					mSupport.Registry.SaveRegistrySetting(RootKey, Key, Value, Data);
				}
			}
		}
		public void SetTitleCase(TextBox txtControl)
		{
			mSupport.FieldEdits.SetTitleCase(txtControl);
		}
		public void SetUpperCase(TextBox txtControl)
		{
			mSupport.FieldEdits.SetUpperCase(txtControl);
		}
		public DialogResult ShowMsgBox(string Message, MsgBoxStyle MsgBoxStyle, Form Parent = null, string Caption = bpeNullString, System.Drawing.Icon Icon = null)
		{
			if ((mSupport != null)) {
				if ((mSupport.UI != null)) {
					return mSupport.UI.ShowMsgBox(Message, MsgBoxStyle, Parent, Caption, Icon);
				}
			}
            return DialogResult.Cancel;
        }
		public bool TagContains(Control ctl, string SearchTag)
		{
			return mSupport.FieldEdits.TagContains(ctl, SearchTag);
		}
		public void TagRemove(Control ctl, string Value)
		{
			mSupport.FieldEdits.TagRemove(ctl, Value);
		}
		public void TagSet(Control ctl, string Value)
		{
			mSupport.FieldEdits.TagSet(ctl, Value);
		}
		public void TextKeyDown(TextBox txtControl, short KeyCode, short Shift)
		{
			mSupport.FieldEdits.TextKeyDown(txtControl, KeyCode, Shift);
		}
		public void TextSelected(Control ctl)
		{
			if ((mSupport != null)) {
				if ((mSupport.FieldEdits != null)) {
					mSupport.FieldEdits.TextSelected(ctl);
				}
			}
		}
		public int TokenCount(string Source, string Delimiter, string Encapsulator = bpeNullString)
		{
			if ((mSupport != null) && (mSupport.Strings != null))
				return mSupport.Strings.TokenCount(Source, Delimiter, Encapsulator);
            return 0;
		}
		public void Trace(string Message, trcOption TraceOption)
		{
			if ((mSupport != null) && (mSupport.Trace != null))
				mSupport.Trace.Trace(trcType.trcBody, Message, TraceOption);
		}
		public void Trace(string Format, object Arg1, trcOption TraceOption)
		{
			if ((mSupport != null) && (mSupport.Trace != null))
				mSupport.Trace.Trace(trcType.trcBody, string.Format(Format, Arg1), TraceOption);
		}
		public void Trace(string Format, object Arg1, object Arg2, trcOption TraceOption)
		{
			if ((mSupport != null) && (mSupport.Trace != null))
				mSupport.Trace.Trace(trcType.trcBody, string.Format(Format, Arg1, Arg2), TraceOption);
		}
		public void Trace(string Format, object Arg1, object Arg2, object Arg3, trcOption TraceOption)
		{
			if ((mSupport != null) && (mSupport.Trace != null))
				mSupport.Trace.Trace(trcType.trcBody, string.Format(Format, Arg1, Arg2, Arg3), TraceOption);
		}
		public void Trace(string Format, object[] ParmArray, trcOption TraceOption)
		{
			if ((mSupport != null) && (mSupport.Trace != null))
				mSupport.Trace.Trace(trcType.trcBody, string.Format(Format, ParmArray), TraceOption);
		}
		public void Trace(clsTrace.trcType trcTraceType, string Message, trcOption TraceOption)
		{
			if ((mSupport != null) && (mSupport.Trace != null))
				mSupport.Trace.Trace(trcTraceType, Message, TraceOption);
		}
		public void Trace(clsTrace.trcType trcTraceType, string Format, object Arg1, trcOption TraceOption)
		{
			if ((mSupport != null) && (mSupport.Trace != null))
				mSupport.Trace.Trace(trcTraceType, string.Format(Format, Arg1), TraceOption);
		}
		public void Trace(clsTrace.trcType trcTraceType, string Format, object Arg1, object Arg2, trcOption TraceOption)
		{
			if ((mSupport != null) && (mSupport.Trace != null))
				mSupport.Trace.Trace(trcTraceType, string.Format(Format, Arg1, Arg2), TraceOption);
		}
		public void Trace(clsTrace.trcType trcTraceType, string Format, object Arg1, object Arg2, object Arg3, trcOption TraceOption)
		{
			if ((mSupport != null) && (mSupport.Trace != null))
				mSupport.Trace.Trace(trcTraceType, string.Format(Format, Arg1, Arg2, Arg3), TraceOption);
		}
		public void Trace(clsTrace.trcType trcTraceType, string Format, object[] ParmArray, trcOption TraceOption)
		{
			if ((mSupport != null) && (mSupport.Trace != null))
				mSupport.Trace.Trace(trcTraceType, string.Format(Format, ParmArray), TraceOption);
		}
		public short UpCase(short uKey)
		{
			return mSupport.FieldEdits.UpCase(uKey);
		}
		public short Verify(string Source, string ValueList)
		{
			return mSupport.FieldEdits.Verify(Source, ValueList);
		}
		#endregion
		#region "Event Handlers"
		//None at this time...
		#endregion
	}
}

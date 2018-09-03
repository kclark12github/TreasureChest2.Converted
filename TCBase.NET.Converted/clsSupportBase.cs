//clsSupportBase.vb
//   TreasureChest2 Support Base Class Definition...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   04/05/18    Introduced TrimTabs;
//   12/19/17    Implemented additional Trace Support;
//   06/09/17    Overloaded LogMessage defaulting mSupport.LogFile as FileName;
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
//   04/24/12    Redefined dtBegin and dtEnd as Date constants so they can be used as Optional argument defaults (and there seems 
//               to be no adverse affects of using #M/d/yyyy# constant format over the former DateSerial() properties;
//   01/11/12    Added TagRemove and TagSet methods;
//   11/16/05    Added new System.Exception-based RaiseError;
//   10/03/05    Removed Dispose and Finalize methods as it was shown that they were never called;
//   10/03/05    Added CleanUpTrace();
//   09/27/05    Added FormatBytes();
//   09/22/05    Added meat to empty constructor to accommodate COM Interop usage;
//   09/18/05    Added Thread and ThreadID properties;
//   08/31/05    Added LogMessage;
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
using static TCBase.clsSupport;
using static TCBase.clsTCBase;
using static TCBase.clsTrace;
using static TCBase.clsUI;
namespace TCBase
{
	public class clsSupportBase : Component
	{
		public clsSupportBase() : base()
		{
			try {
				//Dim StackTrace As String
				//Dim Stack As New StackTrace(True)
				//For i As Integer = 0 To Stack.FrameCount - 1
				//    StackTrace &= Stack.GetFrame(i).ToString
				//Next
				//MessageBox.Show("StackTrace: " & vbCrLf & StackTrace)
				//Stack = Nothing

				//This call is required by the Component Designer.
				InitializeComponent();

				//Add any initialization after the InitializeComponent() call
				mSupport = new clsSupport();
				mMyTraceID = string.Format("{0}.{1}.{2}", mSupport.ApplicationName, System.Reflection.Assembly.GetCallingAssembly().GetName().Name.ToString(), mMyModuleName);
			} catch (Exception ex) {
				throw;
			}
		}
		public clsSupportBase(System.ComponentModel.IContainer Container) : this()
		{

			//Required for Windows.Forms Class Composition Designer support
			Container.Add(this);
		}
		public clsSupportBase(clsSupport objSupport, string ModuleName) : base()
		{
			mSupport = objSupport;
			mMyModuleName = ModuleName;
			mMyTraceID = string.Format("{0}.{1}.{2}", mSupport.ApplicationName, System.Reflection.Assembly.GetCallingAssembly().GetName().Name.ToString(), ModuleName);
			//Debug.WriteLine("Allocating New clsSupportBase for " & mMyTraceID)
			//This call is required by the Component Designer.
			InitializeComponent();
		}

		#region " Component Designer generated code "
		//Required by the Component Designer

		//NOTE: The following procedure is required by the Component Designer
		//It can be modified using the Component Designer.
		//Do not modify it using the code editor.
		private System.ComponentModel.IContainer components;
		protected internal System.Windows.Forms.Timer timTimer;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.timTimer = new System.Windows.Forms.Timer(this.components);
			//
			//timTimer
			//
			//
			//clsSupportBase
			//

		}
		#endregion
		#region "Events"
		//None at this time...
		#endregion
		#region "Properties"
		#region "Trace Support"
		protected AssemblyInfo mMyAssemblyInfo = new AssemblyInfo(System.Reflection.Assembly.GetCallingAssembly());
		protected string mMyModuleName = "clsSupportBase";
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
		protected clsSupport mSupport = null;
		private Thread mThread = System.Threading.Thread.CurrentThread;
		private int mThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
		protected bool bDisposed = false;
		public Thread Thread {
			get { return mThread; }
			set { mThread = value; }
		}
		public int ThreadID {
			get { return mThreadID; }
			set { mThreadID = value; }
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
		public void ControlEnabled(Control ctl, bool Enabled)
		{
			mSupport.FieldEdits.ControlEnabled(ctl, Enabled);
		}
		public void ControlSetFocus(Control ctl)
		{
			mSupport.FieldEdits.ControlSetFocus(ctl);
		}
		public void Display(string strMessage, clsUI.DisplayEnum Location, vbRGBColorConstants FontColor = vbRGBColorConstants.vbUndefinedColor, TriState FontBold = TriState.UseDefault, TriState FontItalic = TriState.UseDefault, string FontName = clsSupport.bpeNullString, float FontSize = 0)
		{
			if ((mSupport != null) && (mSupport.UI != null))
				mSupport.UI.Display(strMessage, Location, FontColor, FontBold, FontItalic, FontName, FontSize);
		}
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
			functionReturnValue = clsSupport.bpeNullString;
			if ((mSupport != null) && (mSupport.Registry != null))
				return mSupport.Registry.GetINIKey(File, Section, Key, defaultValue);
			return functionReturnValue;
		}
		public object GetRegistrySetting(string Key, string Value, object vDefault = null)
		{
			object functionReturnValue = null;
			functionReturnValue = null;
			if ((mSupport == null) || (mSupport.Registry == null))
				return null;
			functionReturnValue = mSupport.Registry.GetRegistrySetting(RootKeyConstants.HKEY_LOCAL_MACHINE, Key, Value, null);
			if (functionReturnValue == null)
				functionReturnValue = mSupport.Registry.GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, Key, Value, null);
			if (functionReturnValue == null)
				functionReturnValue = vDefault;
			return functionReturnValue;
		}
		public object GetRegistrySetting(clsRegistry.RootKeyConstants RootKey, string Key, string Value, object vDefault = null)
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
		public string LeftFill(object Source, int iLength, char FillChar = ' ')
		{
			return mSupport.FieldEdits.LeftFill(Source, iLength, FillChar);
		}
		public string LogMessage(string Message, short Indent, bool fSkipFormat = false)
		{
			string functionReturnValue = null;
			functionReturnValue = clsSupport.bpeNullString;
			if ((mSupport != null) && (mSupport.Trace != null))
				return mSupport.Trace.LogMessage(mSupport.LogFile, Message, Indent, fSkipFormat);
			return functionReturnValue;
		}
		public string LogMessage(string FileName, string Message, short Indent, bool fSkipFormat = false)
		{
			string functionReturnValue = null;
			functionReturnValue = clsSupport.bpeNullString;
			if ((mSupport != null) && (mSupport.Trace != null))
				return mSupport.Trace.LogMessage(FileName, Message, Indent, fSkipFormat);
			return functionReturnValue;
		}
		public string NothingToString(object Value)
		{
			if ((Value == null))
				return "Nothing";
			if (Value is string)
				return Value.ToString();
			return Value.ToString();
		}
        protected string ParsePath(string strPath, ParseParts intPart) {
			return mSupport.ParsePath(strPath, intPart);
		}
		public string ParseStr(string Source, int TokenNum, string Delimiter, string Encapsulator = clsSupport.bpeNullString, bool Preserve = false)
		{
			return mSupport.Strings.ParseStr(Source, TokenNum, Delimiter, Encapsulator, Preserve);
		}
		public string RightFill(object Source, int iLength, char FillChar = ' ')
		{
			return mSupport.FieldEdits.RightFill(Source, iLength, FillChar);
		}
		public void SaveINIKey(string File, string Section, string Key, string Value)
		{
			if ((mSupport != null) && (mSupport.Registry != null))
				mSupport.Registry.SaveINIKey(File, Section, Key, Value);
		}
		public void SaveRegistrySetting(clsRegistry.RootKeyConstants RootKey, string Key, string Value, object Data)
		{
			if ((mSupport != null) && (mSupport.Registry != null))
				mSupport.Registry.SaveRegistrySetting(RootKey, Key, Value, Data);
		}
		public void SetTitleCase(TextBox txtControl)
		{
			mSupport.FieldEdits.SetTitleCase(txtControl);
		}
		public void SetUpperCase(TextBox txtControl)
		{
			mSupport.FieldEdits.SetUpperCase(txtControl);
		}
		public DialogResult ShowMsgBox(string Message, MsgBoxStyle MsgBoxStyle, Form Parent = null, string Caption = clsSupport.bpeNullString, System.Drawing.Icon Icon = null)
		{
			if ((mSupport != null) && (mSupport.UI != null))
				return mSupport.UI.ShowMsgBox(Message, MsgBoxStyle, Parent, Caption, Icon);
            return DialogResult.Abort;
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
			mSupport.FieldEdits.TextSelected(ctl);
		}
		public int TokenCount(string Source, string Delimiter, string Encapsulator = clsSupport.bpeNullString)
		{
			return mSupport.Strings.TokenCount(Source, Delimiter, Encapsulator);
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
		public string TrimTabs(string Source)
		{
			return mSupport.Strings.TrimTabs(Source);
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

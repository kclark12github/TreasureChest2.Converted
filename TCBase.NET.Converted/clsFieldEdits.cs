//clsFieldEdits.vb
//   TreasureChest2 Field Edits Class...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   06/10/14    187134      Ken Clark       Introduced FormatElapsed;
//   01/25/14    192123      Ken Clark       Reviewed Memory-related tracing;
//   01/21/14    192123      Ken Clark       Eliminated use of RecordEntry/RecordExit for performance;
//   01/17/14    192123      Ken Clark       Reviewed TraceOptions;
//   10/17/13    187135      Ken Clark       Added FormatMoney and FormatPercentage overloads that take Color (VB.NET) structures
//                                           as arguments instead of translated integers;
//                                           Added ComboBox and ctlLookup to list of READONLY controls handled by ControlEnabled;
//   08/28/13    182389      Ken Clark       Modified ControlEnabled to properly update Nodes in a TreeView control;
//   06/10/13    182408      Ken Clark       Introduced GetUnselectedText and its use in ChkRNumber;
//   02/19/13    175365      Ken Clark       Added handling of Keys.Return/Enter in ChkINumber as the .NET TextBox KeyPress event 
//                                           seems to fire on Return/Enter although VB6 did not;
//   01/25/13    107551      Ken Clark       Changed FixQuotes Source argument from String to Object to support handling of DBNull;
//   10/17/12    168808      Ken Clark       Modified ChkINumber to handle Cut (Ctrl-X), Copy (Ctrl-C) & Paste (Ctrl-V) without 
//                                           triggering an unwarranted ArgumentException("Invalid integer data entered");
//   08/20/12    107551      Ken Clark       Corrected errant logic in ChkINumber methods;
//   02/09/12    107551      Ken Clark       Modified ControlEnabled to support the notion of a Read-Only control such that if the
//                                           control is to be considered Read-Only, ControlEnabled will never disable the control,
//                                           but will still change the .BackColor as if it were disabled (leaving it the 
//                                           responsibility of the application code to enforce the Read-Only-ness of the control);
//                                           Controls Supported:
//                                               CheckedListBox: "READONLY" Tag
//                                               RichTextBox:    .ReadOnly property
//                                               TextBox:        .ReadOnly property
//                                               TreeView:       "READONLY" Tag
//   01/12/12    107551      Ken Clark       Introduced AlignRights(Control,Control);
//   01/11/12    107551      Ken Clark       Added TagRemove and TagSet methods;
//   08/29/11    168806      Ken Clark       Changed FormatPercentage to a Sub (instead of Function as this make more sense), and
//                                           introduced an overload that just does the string value;
//   08/19/11    168804      Ken Clark       Corrected logic in ChkRNumber(TextBox,KeyPressEventArgs,Boolean) which was still using
//                                           the VB6 vbKeyBell KeyPress event convention of invalidating characters;
//   07/20/11    107551      Ken Clark       Reviewed KeyPress event-handlers and their use of the e.Handled property;
//   11/15/09    None        Ken Clark       Modified ControlEnabled to check the type of the control to be sure it's a Form before
//                                           attempting to set the Cursor attribute (it was handled, but caused unneeded exceptions);
//   06/08/06    109743      Ken Clark       Modified ChkSQLid() to allow all printable characters;
//   02/07/06    None        Ken Clark       Modified ChkSQLid() to allow lower-case characters as the first character and to allow
//                                           periods in all but the first character;
//   07/18/05    None        Ken Clark       Replaced use of QueryPerformanceFrequency() Win32 API function with .NET's 
//                                           Microsoft.VisualBasic.DateAndTime.Timer();
//   03/21/05    None        Ken Clark       Revised FixMoney calculation after significant testing (JAD);
//                                           Added "RadioButton" as Windows.Forms implementation of VB6 "OptionButton";
//   03/11/05    None        Ken Clark       Made New() constructor "Friend" in order to accomplish PublicNotCreatable VB6 Class 
//                                           Instancing behavior;
//   02/21/05    None        Ken Clark       Upgraded for use as a VB.NET Component;
//   01/17/05    95805       Ken Clark       Added "-" to digit list used to identify decimal position in FixMoney();
//   01/12/05    95626       Ken Clark       Added FixMoney and Verify functions (from SIASCurrency);
//   12/09/04    92634       Ken Clark       Removed "ctl.Activated" reference from ControlEnabled() because there is no
//                                           VB.Form.Activated property;
//   11/22/04    79979       Ken Clark       Added ChkKey();
//   06/24/04    89540       Ken Clark       Added FormatSeconds from clsFiRReApplication.cls;
//                                           Corrected, but left commented-out, the replacement of the desired currency symbol in
//                                           FormatMoney pending resolution of regional settings issues;
//   02/22/04    None        Ken Clark       Implemented ControlSetFocus(), StringToszString(), and szStringToString();
//   02/18/04    86836       Ken Clark       Removed generic StateCountry in lieu of a STATES table based solution within FiRRe;
//   02/18/04    86836       JAD             Moved MP from Canada to US in StateCountry
//   08/31/03    None        JAD             Changed Quebec from PQ to QC
//   07/10/03    79979       Ken Clark       Removed ChkSpecialChar because we cannot know for sure if a KeyPress function is
//                                           passed the first position of the TextBox control;
//   07/09/03    79979       Ken Clark       Added ChkSpecialChar;
//   06/27/03    79679       Ken Clark       Added InitPVCurrencyProperties();
//   04/27/03    73107       Ken Clark       Due to the nature of longer running processes, changed local variables in ElapsedTime
//                                           from Integers to Longs;
//   04/10/03    73107       Ken Clark       Added Line, SSActiveTabPanel, and SSActiveTabs to the list of "don't screw with"
//                                           .BackColor property in ControlEnabled();
//   01/23/03    None        Ken Clark       Removed RndDblCents();
//   12/02/02    72054       Ken Clark       Changed RndCents to use a string-based truncation of whole number from fraction
//                                           instead of integer division in order to avoid Overflow errors on large currency
//                                           values;
//   10/14/02    None        Ken Clark       Added KB, MB, GB, and TB properties;
//                                           Changed vbKey constants to formal properties;
//                                           Added FormatBytes();
//   09/02/02    69201       Ken Clark       Removed setting "ctl = Nothing" at the ExitSub label of ControlEnabled because it was
//                                           trashing DataPicker controls (why not others is a mystery);
//   08/23/02    68936       Ken Clark       Can no longer assume SQL-ID fields need to be forced to uppercase (SQL Server supports
//                                           mixed case);
//   05/10/02    55900       Ken Clark       Added Node processing for TreeView controls in ControlEnabled;
//   04/30/02    57056       Ken Clark       Corrected Form-related problems in ControlEnabled;
//   04/26/02    65135       Ken Clark       Added logic in ControlEnabled to handle processing of Forms;
//   12/12/01    None        Ken Clark       Corrected vbCr reference as Integer in CheckMark() causing Type Mismatch errors;
//   10/06/01    None        Ken Clark       Created from FiRRe's libFieldEdits;
//=================================================================================================================================
//Note to self...
//   - Need to replace MsgBox calls with err.Raise scheme to support Unattended mode...
//   - Screen control stuff might not be a good idea either...
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
	public enum vbKeyEnum : int
	{
		vbKeyBell = 7,
		vbKeyCopyC = 3,
		vbKeyCutX = 24,
		vbKeyMinus = 45,
		vbKeyPasteV = 22,
		vbKeyPeriod = 46,
		vbKeyUnderScore = 95,
		vbKeyTilde = 126
	}
}
namespace TCBase
{
	public class clsFieldEdits : clsSupportBase
	{
		internal clsFieldEdits(clsSupport objSupport) : base(objSupport, "clsFieldEdits")
		{
		}

		#region "Events"
		//None at this time...
		#endregion
		#region "Properties"
		public enum rrRoundingRule
		{
			rrNone = 0,
			//No Rounding
			rrStandard = 1,
			//Standard Rounding (.5 or greater rounds up, else round down)
			rrRoundUp = 2,
			//Always Round Up
			rrRoundDown = 3
			//Always Round Down (truncate)
		}
		#endregion
		#region "Methods"
		//Private Declare Function SendMessage Lib "user32" Alias "SendMessageA" (ByVal hWnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByRef lParam As Integer) As Integer
		//Private Declare Function GetWindowLong Lib "user32" Alias "GetWindowLongA" (ByVal hWnd As System.IntPtr, ByVal nIndex As Integer) As Integer
		//Private Declare Function SetWindowLong Lib "user32" Alias "SetWindowLongA" (ByVal hWnd As System.IntPtr, ByVal nIndex As Integer, ByVal dwNewLong As Integer) As Integer
		//Private Declare Function SetLocaleInfo Lib "kernel32" Alias "SetLocaleInfoA" (ByVal Locale As Integer, ByVal LCType As Integer, ByVal lpLCData As String) As Boolean
		//Private Declare Function GetSystemDefaultLCID Lib "kernel32" () As Integer
		//Private Declare Function GetUserDefaultLCID Lib "kernel32" () As Integer
		//Private Declare Function GetLocaleInfo Lib "kernel32" Alias "GetLocaleInfoA" (ByVal Locale As Integer, ByVal LCType As Integer, ByVal lpLCData As String, ByVal cchData As Integer) As Integer
		//Private Declare Function QueryPerformanceFrequency Lib "kernel32" (ByRef lpFrequency As Long) As Short
		//Private lpQueryPerformanceFrequency As Long
        public bool IsSpecialCharacter(char value)
        {
            switch (value)
            {
                case '+':
                case '.':
                case '/':
                case ' ':
                case '!':
                case '@':
                case '#':
                case '$':
                case '%':
                case '^':
                case '&':
                case '*':
                case '`':
                case '|':
                case '?':
                case ',':
                case '=':
                case '\"':
                case '{':
                case '}':
                case '[':
                case ']':
                case '\\':
                case '<':
                case '>':
                    return true;
                    break;
                default:
                    break;
            }
            return false;
        }
        //public bool IsSpecialCharacter(System.Windows.Forms.Keys value)
        //{
        //    switch(value) {
        //        case Keys.Decimal:
        //        case Keys.Divide:
        //        case Keys.Multiply:
        //        case Keys.Space:
        //    }
        //    return false;
        //}
        public void AlignRights(Control Control1, Control Control2)
		{
			Graphics g1 = Control1.CreateGraphics();
			Graphics g2 = Control2.CreateGraphics();

			int w1 = Convert.ToInt32(Math.Ceiling(g1.MeasureString(Control1.Text, Control1.Font).Width));
			int w2 = Convert.ToInt32(Math.Ceiling(g2.MeasureString(Control2.Text, Control2.Font).Width));

			//Realign the label...
			Control2.SetBounds(Control1.Left + (w1 - w2), 0, 0, 0, BoundsSpecified.X);
		}
		public short ChkHiNumber(TextBox sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			short functionReturnValue = 0;
			functionReturnValue = this.ChkHiNumber(sender, (short)Strings.Asc(e.KeyChar));
			if (Strings.Asc(Strings.UCase(e.KeyChar)) == (int)System.Windows.Forms.Keys.I)
				e.Handled = true;
			return functionReturnValue;
		}
		public short ChkHiNumber(TextBox sender, short cNum)
		{
			short functionReturnValue = 0;
			switch (Strings.Asc(Strings.UCase(Strings.Chr(cNum)))) {
				case (int)System.Windows.Forms.Keys.I:
					if (sender.SelectionLength == sender.Text.Length || sender.Text == clsSupport.bpeNullString || sender.Text.ToUpper().StartsWith("I")) {
						sender.Text = "Immeasurable";
						sender.SelectionLength = sender.Text.Length;
					} else {
						functionReturnValue = this.ChkRNumber(cNum, true, sender);
					}
					break;
				default:
					functionReturnValue = this.ChkRNumber(cNum, true, sender);
					break;
			}
			return functionReturnValue;
		}
		public CheckState CheckMark(CheckBox sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			CheckState functionReturnValue = default(CheckState);
			functionReturnValue = (CheckState)this.CheckMark((short)Strings.Asc(e.KeyChar), (short)sender.CheckState);
			e.Handled = true;
			return functionReturnValue;
		}
		public short CheckMark(short Key, short ChkVal)
		{
			short functionReturnValue = 0;
			switch (Strings.Asc(Strings.UCase(Strings.Chr(Key)))) {
				case (int)System.Windows.Forms.Keys.Space:
					//Since CheckMark is assumed to be called in the KeyPress Event of a CheckBox
					//if the user hit Space, Windows has already changed the value represented by
					//ChkVal... So let's try this...
					functionReturnValue = ChkVal;
					break;
				case (int)System.Windows.Forms.Keys.X:
				case (int)System.Windows.Forms.Keys.Y:
					functionReturnValue = (short)System.Windows.Forms.CheckState.Checked;
					break;
				case (int)System.Windows.Forms.Keys.Back:
				case (int)System.Windows.Forms.Keys.N:
					functionReturnValue = (short)System.Windows.Forms.CheckState.Unchecked;
					break;
                default:
                    if ((Strings.Asc(Strings.UCase(Strings.Chr(Key)))) == Strings.Asc(Constants.vbCr))
					    functionReturnValue = (short)(ChkVal == (short)System.Windows.Forms.CheckState.Unchecked ? System.Windows.Forms.CheckState.Checked : System.Windows.Forms.CheckState.Unchecked);
					break;
			}
			return functionReturnValue;
		}
		public short ChkKey(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			short functionReturnValue = 0;
			functionReturnValue = (short)Strings.Asc(e.KeyChar);
			this.ChkKey(ref functionReturnValue);
			if (functionReturnValue == (short)vbKeyEnum.vbKeyBell) {
				e.Handled = true;
				throw new ArgumentException(string.Format("Invalid data entered ({0})", e.KeyChar));
			}
			return functionReturnValue;
		}
		public void ChkKey(ref short cNum, bool UpCase = false)
		{
			short iNum = (short)vbKeyEnum.vbKeyBell;
			//First check KeyCodes, then specific characters...
			switch (cNum) {
				case (short)System.Windows.Forms.Keys.Decimal:
					break;
				case (short)System.Windows.Forms.Keys.Divide:
					break;
				case (short)System.Windows.Forms.Keys.Multiply:
					break;
				case (short)System.Windows.Forms.Keys.Space:
					break;
				default:
                    if (!IsSpecialCharacter(Strings.UCase(Strings.Chr(cNum))))
                        iNum = (short)(UpCase ? Strings.Asc(Strings.UCase(Strings.Chr(cNum))) : cNum);
					break;
			}
			cNum = iNum;
		}
		public short ChkINumber(object sender, System.Windows.Forms.KeyPressEventArgs e, bool NoNegative = false)
		{
			short functionReturnValue = 0;
            try
            {
                short cNum = (short)Strings.Asc(e.KeyChar);
                if (cNum >= (short)System.Windows.Forms.Keys.D0 && cNum <= (short)System.Windows.Forms.Keys.D9)
                {
                    functionReturnValue = cNum;
                    throw new ExitTryException(); 
                }
                switch ((System.Windows.Forms.Keys)cNum)
                {
                    case Keys.Back:
                    case Keys.Space:
                        //Why is Space OK?
                        functionReturnValue = cNum;
                        throw new ExitTryException(); 

                        break;
                    case Keys.Enter:
                        //case Keys.Return://Both defined as 13
                        e.Handled = false;
                        throw new ExitTryException(); 

                        break;
                    case Keys.Subtract:
                        if (!NoNegative)
                        {
                            functionReturnValue = cNum;
                            throw new ExitTryException();
                        }
                        break;
                }
                switch ((vbKeyEnum)cNum)
                {
                    case vbKeyEnum.vbKeyMinus:
                        if (!NoNegative)
                        {
                            functionReturnValue = cNum;
                            throw new ExitTryException();
                        }
                        break;
                    case vbKeyEnum.vbKeyCopyC:
                    case vbKeyEnum.vbKeyCutX:
                        e.Handled = false;
                        throw new ExitTryException();

                        break;
                    case vbKeyEnum.vbKeyPasteV:
                        System.Windows.Forms.DataObject cbData = new System.Windows.Forms.DataObject();
                        cbData = (DataObject)System.Windows.Forms.Clipboard.GetDataObject();
                        if (cbData.GetDataPresent(System.Windows.Forms.DataFormats.Text))
                        {
                            int iData = 0;
                            try
                            {
                                iData = (int)cbData.GetData(System.Windows.Forms.DataFormats.Text);
                            }
                            catch (Exception ex)
                            {
                                throw new ArgumentException("Invalid integer data entered");
                            }
                            //If we get this far, let the Paste operation do it's thing - we've confirmed it should be OK to do so...
                            e.Handled = false;
                            throw new ExitTryException();
                        }
                        break;
                }
                throw new ArgumentException("Invalid integer data entered");
            } catch (ExitTryException) { 
			} catch (Exception ex) {
				e.Handled = true;
				throw new Exception(ex.Message, ex);
			}
			return functionReturnValue;
		}
		public short ChkMaxNumber(TextBox sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			short functionReturnValue = 0;
			try {
				functionReturnValue = this.ChkMaxNumber(sender, (short)Strings.Asc(e.KeyChar));
				if (Strings.Asc(Strings.UCase(e.KeyChar)) == (int)System.Windows.Forms.Keys.U)
					e.Handled = true;
			} catch (Exception ex) {
				e.Handled = true;
				throw new Exception(ex.Message, ex);
			}
			return functionReturnValue;
		}
		public short ChkMaxNumber(TextBox sender, short cNum)
		{
			short functionReturnValue = 0;
			switch (Strings.Asc(Strings.UCase(Strings.Chr(cNum)))) {
				case (int)System.Windows.Forms.Keys.U:
					if (sender.SelectionLength == sender.Text.Length || sender.Text == clsSupport.bpeNullString || sender.Text.ToUpper().StartsWith("U")) {
						sender.Text = "Unlimited.";
						sender.SelectionLength = sender.Text.Length;
					} else {
						functionReturnValue = this.ChkRNumber(cNum, true, sender);
					}
					break;
				default:
					functionReturnValue = this.ChkRNumber(cNum, true, sender);
					break;
			}
			return functionReturnValue;
		}
		public short ChkRNumber(TextBox sender, System.Windows.Forms.KeyPressEventArgs e, bool NoNegative = false)
		{
			short functionReturnValue = 0;
			try {
				short cNum = (short)Strings.Asc(e.KeyChar);
				functionReturnValue = this.ChkRNumber(cNum, NoNegative, sender);
			} catch (Exception ex) {
				e.Handled = true;
				throw new Exception(ex.Message, ex);
			}
			return functionReturnValue;
		}
		public short ChkRNumber(short cNum, bool NoNegative = false, TextBox sender = null)
		{
			short functionReturnValue = 0;
			if (NoNegative && (cNum == (short)vbKeyEnum.vbKeyMinus || cNum == (short)System.Windows.Forms.Keys.Subtract) || (cNum != (short)System.Windows.Forms.Keys.Back && cNum != (short)System.Windows.Forms.Keys.Space && cNum != (short)vbKeyEnum.vbKeyMinus && (cNum < (short)vbKeyEnum.vbKeyPeriod || cNum > (short)System.Windows.Forms.Keys.D9)))
				throw new ArgumentException("Invalid real number data entered");
			functionReturnValue = cNum;

			//Check for multiple decimals, and embedded spaces in (in unselected Text - as allowing the current keystroke will replace any SelectedText)...
			if ((sender != null)) {
				string UnselectedText = this.GetUnselectedText(sender);
				if (UnselectedText.Trim().IndexOf(" ") != -1)
					throw new ArgumentException("Invalid real number data entered (embedded space)");
				if (cNum == (short)vbKeyEnum.vbKeyMinus && UnselectedText.IndexOf("-") > -1)
					throw new ArgumentException("Invalid real number data entered (multiple \"-\")");
				if (cNum == (short)vbKeyEnum.vbKeyPeriod && UnselectedText.IndexOf(".") > -1)
					throw new ArgumentException("Invalid real number data entered (multiple \".\")");
			}
			return functionReturnValue;
		}
		public void ClearDTP(DateTimePicker dtpControl)
		{
			dtpControl.Text = clsSupport.bpeNullString;
		}
		private void ControlEnabled(TreeNode Node, System.Drawing.Color BackColor)
		{
			Node.BackColor = BackColor;
			foreach (TreeNode ChildNode in Node.Nodes) {
				this.ControlEnabled(ChildNode, BackColor);
			}
		}
		public void ControlEnabled(Control ctl, bool Enabled)
		{
			System.Drawing.Color AlternatingBackColor = default(System.Drawing.Color);
			System.Drawing.Color BackColor = default(System.Drawing.Color);
			AlternatingBackColor = (Enabled ? SystemColors.ControlLight : SystemColors.Control);
			BackColor = (Enabled ? SystemColors.Window : SystemColors.Control);
            bool fReadOnly = false;
            switch (ctl.GetType().Name) {
				case "DateTimePicker":
				case "DomainUpDown":
				case "ListBox":
				case "NumericUpDown":
					ctl.Enabled = Enabled;
					ctl.BackColor = BackColor;
					break;
				//case "ctlLookup":
				//case "SIASCurrency":
				//	//If the control's got a ReadOnly tag, don't disable the control, but make it appear as if it is...
				//	var _with1 = (SIASCurrency)ctl;
				//	bool fReadOnly = Convert.ToBoolean(TagContains(ctl, "READONLY") || _with1.ReadOnly);
				//	_with1.Enabled = (fReadOnly ? true : Enabled);
				//	_with1.BackColor = (fReadOnly ? SystemColors.Control : BackColor);
				//	break;
				case "ComboBox":
					//If the control's got a ReadOnly tag, don't disable the control, but make it appear as if it is...
					var _with2 = (ComboBox)ctl;
					fReadOnly = TagContains(ctl, "READONLY");
					_with2.Enabled = (fReadOnly ? true : Enabled);
					_with2.BackColor = (fReadOnly ? SystemColors.Control : BackColor);
					break;
				case "CheckedListBox":
					//If the control's got a ReadOnly tag, don't disable the control, but make it appear as if it is...
					var _with3 = (CheckedListBox)ctl;
					fReadOnly = TagContains(ctl, "READONLY");
					_with3.Enabled = (fReadOnly ? true : Enabled);
					_with3.BackColor = (fReadOnly ? SystemColors.Control : BackColor);
					break;
				case "DataGrid":
					//By default, both the BackColor and the AlternatingBackColor properties are set to the same color. Setting the 
					//BackColor property affects only even-numbered rows, while setting the AlternatingBackColor affects only 
					//odd-numbered rows.
					var _with4 = (DataGrid)ctl;
					_with4.Enabled = Enabled;
					_with4.BackColor = BackColor;
					_with4.AlternatingBackColor = AlternatingBackColor;
					break;
				case "RichTextBox":
					//If the control's .ReadOnly property is True, don't disable the control, but make it appear as if it is...
					var _with5 = (RichTextBox)ctl;
					fReadOnly = Convert.ToBoolean(TagContains(ctl, "READONLY") || _with5.ReadOnly);
					_with5.Enabled = (fReadOnly ? true : Enabled);
					_with5.BackColor = (fReadOnly ? SystemColors.Control : BackColor);
					break;
				case "TextBox":
					//If the control's .ReadOnly property is True, don't disable the control, but make it appear as if it is...
					var _with6 = (TextBox)ctl;
					fReadOnly = Convert.ToBoolean(TagContains(ctl, "READONLY") || _with6.ReadOnly);
					_with6.Enabled = (fReadOnly ? true : Enabled);
					_with6.BackColor = (fReadOnly ? SystemColors.Control : BackColor);
					break;
				case "TreeView":
					//If the control's got a ReadOnly tag, don't disable the control, but make it appear as if it is...
					var _with7 = (TreeView)ctl;
					fReadOnly = Convert.ToBoolean(TagContains(ctl, "READONLY"));
					_with7.Enabled = (fReadOnly ? true : Enabled);
					_with7.BackColor = (fReadOnly ? SystemColors.Control : BackColor);
					foreach (TreeNode iNode in _with7.Nodes) {
						this.ControlEnabled(iNode, (fReadOnly ? SystemColors.Control : BackColor));
					}

					break;
				default:
					//VB6 Controls:      "CommandButton", "Frame", "Line", "OptionButton", "SSActiveTabPanel", "SSActiveTabs"  
					//VB.NET Controls:   "Button", "CheckBox", "GroupBox", "Label", "Panel", "RadioButton", "TabControl", "TabPage"                 
					ctl.Enabled = Enabled;
					try {
						if (object.ReferenceEquals(ctl.GetType(), typeof(Form))) {
							//If we're derived from a Windows.Forms.Form, then set the cursor accordingly... otherwise suppress the exception...
							((Form)ctl).Cursor = (Enabled ? Cursors.Default : Cursors.WaitCursor);
						}
					} catch (Exception ex) {
					}
					break;
			}
		}
		public void ControlSetFocus(Control ctl)
		{
			try {
				ctl.Focus();
			} catch (Exception ex) {
			}
		}
		public string ConvertToCSV(object Value)
		{
			string functionReturnValue = null;
			functionReturnValue = clsSupport.bpeNullString;
			if ((Value == null))
				Value = clsSupport.bpeNullString;
			if (Information.IsDBNull(Value))
				Value = clsSupport.bpeNullString;
			functionReturnValue = Convert.ToString(Value).Trim();
			functionReturnValue = functionReturnValue.Replace(Constants.vbCrLf, Constants.vbLf);
			if (functionReturnValue.Contains("\"") || functionReturnValue.Contains(",") || functionReturnValue.Contains(Constants.vbLf))
				functionReturnValue = "\"" + CSVQuote(functionReturnValue) + "\"";
			return functionReturnValue;
		}
		public string ConvertToFlat(object Value, short vLength, char FillChar = ' ', short Lines = 1)
		{
			string functionReturnValue = null;
			string Token = null;
			short i = 0;
			short iSpace = 0;
			short iCrLf = 0;
			functionReturnValue = clsSupport.bpeNullString;

			if ((Value == null) || Information.IsDBNull(Value))
				Value = clsSupport.bpeNullString;
			string tmpValue = Convert.ToString(Value).Trim();

			//Reduce any combinations of CR/LF to vbCrLf...
			tmpValue = tmpValue.Replace(Strings.Chr(13), Strings.Chr(4));
			tmpValue = tmpValue.Replace(Strings.Chr(10), Strings.Chr(4));
			tmpValue = tmpValue.Replace(new string(Strings.Chr(4), 2), Constants.vbCrLf);
			tmpValue = tmpValue.Replace(Strings.Chr(4).ToString(), Constants.vbCrLf);

			//The idea here is to parse the 'Value' field into 'Lines' blocks of 'vLength' characters filled
			//with 'FillChar'... Must consider a 'Value' parameter containing embedded vbCrLf sequences...

			for (i = 1; i <= Lines; i++) {
				Token = clsSupport.bpeNullString;
				if (tmpValue != clsSupport.bpeNullString) {
					iCrLf = (short)Strings.InStr(tmpValue, Constants.vbCrLf);
					if (iCrLf > 0 && iCrLf <= vLength) {
						Token = Strings.Trim(Strings.Mid(tmpValue, 1, iCrLf - 1));
						tmpValue = Strings.Mid(tmpValue, iCrLf + 2);
					} else if (tmpValue.Length <= vLength) {
						Token = tmpValue;
						tmpValue = clsSupport.bpeNullString;
					} else {
						iSpace = (short)Strings.InStrRev(tmpValue, " ", vLength);
						Token = Strings.Trim(Strings.Mid(tmpValue, 1, iSpace));
						tmpValue = Strings.Mid(tmpValue, iSpace + 1);
					}
				}
				switch (FillChar) {
					case '0':
						Token = this.RightFill(Token, vLength, FillChar);
						break;
					default:
						Token = this.LeftFill(Token, vLength, FillChar);
						break;
				}
				//Debug.Print Token
				functionReturnValue += Token;
			}
			return functionReturnValue;
		}
		public string CSVQuote(string Source)
		{
			string functionReturnValue = null;
			functionReturnValue = clsSupport.bpeNullString;
			if ((Source == null))
				Source = clsSupport.bpeNullString;
			string strTemp = Source;
			//Eliminate all double - double quotes
			short i = 0;
			do {
				i = (short)Strings.InStr(1, strTemp, "\"" + "\"");
				if (i != 0) {
					strTemp = Strings.Left(strTemp, i - 1) + "\"" + Strings.Mid(strTemp, i + 2);
				}
			} while (!(i == 0));
			//Replace all single quotes with double - single quotes
			i = (short)Strings.InStr(1, strTemp, "\"");
			while (i > 0) {
				strTemp = Strings.Left(strTemp, i - 1) + "\"" + "\"" + Strings.Mid(strTemp, i + 1);
				i = (short)Strings.InStr(i + 2, strTemp, "\"");
			}
			functionReturnValue = strTemp;
			return functionReturnValue;
		}
		public double DecodePercentage(string Percentage)
		{
			short i = 0;
			i = (short)Strings.InStr(Percentage, "%");
			if (i > 0)
				Percentage = Strings.Mid(Percentage, 1, i - 1);
			Percentage = Convert.ToString(Conversion.Val(Percentage));
			return Convert.ToDouble(Percentage) / 100;
		}
		public string ElapsedTime(System.DateTime StartTime, System.DateTime FinishTime, short tFormat = 0)
		{
			string functionReturnValue = null;
			int MM = 0;
			int HH = 0;
			int SS = 0;
			string strTime = null;
			functionReturnValue = clsSupport.bpeNullString;
			strTime = clsSupport.bpeNullString;
			SS = (int)DateAndTime.DateDiff(Microsoft.VisualBasic.DateInterval.Second, StartTime, FinishTime);
			HH = SS / 3600;
			SS -= (HH * 3600);
			MM = SS / 60;
			SS -= (MM * 60);
			if (HH > 0)
				strTime = string.Format("{0} Hours, ", HH);
			if (MM > 0)
				strTime += string.Format("{0} Minutes, ", MM);
			strTime += string.Format("{0} Seconds", SS);

			switch (tFormat) {
				case 0:
					functionReturnValue = string.Format("{0:00}:{1:00}:{2:00}", HH, MM, SS);
					break;
				default:
					functionReturnValue = strTime;
					break;
			}
			return functionReturnValue;
		}
		public string ExtractPhone(string inPhone)
		{
			string functionReturnValue = null;
			functionReturnValue = clsSupport.bpeNullString;
			functionReturnValue = clsSupport.bpeNullString;
			for (short i = 1; i <= Strings.Len(inPhone); i++) {
				int ichar = Strings.Asc(Strings.Mid(inPhone, i, 1));
				if ((ichar > 47 && ichar < 58) || (ichar > 64 && ichar < 91)) {
					functionReturnValue = functionReturnValue + Strings.Chr(ichar);
				}
			}
			return functionReturnValue;
		}
		public decimal FixMoney(decimal cAmount, short Decimals, clsFieldEdits.rrRoundingRule RoundingRule = rrRoundingRule.rrStandard)
		{
			decimal functionReturnValue = default(decimal);
			short iDecimal = 0;
			string sAmount = null;
			string sFraction = null;
			string sWhole = null;

			functionReturnValue = cAmount;
			sAmount = Convert.ToString(cAmount);
			iDecimal = Verify(sAmount, "-0123456789");
			//i.e. iPos = (first character in sAmount not in "0123456789")...
			//we have a decimal point (presumably), and therefore something to round...
			if (iDecimal > 0) {
				switch (RoundingRule) {
					case rrRoundingRule.rrNone:
						functionReturnValue = Math.Round(cAmount, Convert.ToInt32(Decimals));
						break;
					case rrRoundingRule.rrStandard:
						if (Decimals >= 4) {
						} else {
							cAmount = Convert.ToDecimal(cAmount) + Convert.ToDecimal((cAmount >= 0 ? (0.5 / (Math.Pow(10, Decimals))) : -(0.5 / (Math.Pow(10, Decimals)))));
						}
						sAmount = Convert.ToString(cAmount);
						iDecimal = Verify(sAmount, "-0123456789");
						if (iDecimal > 0) {
							sWhole = Strings.Left(sAmount, iDecimal - 1);
							sFraction = Strings.Mid(sAmount, iDecimal + 1);
							cAmount = Convert.ToDecimal(sWhole + Strings.Mid(sAmount, iDecimal, 1) + Strings.Left(sFraction, Decimals));
						}
						functionReturnValue = cAmount;
						break;
					case rrRoundingRule.rrRoundUp:
						functionReturnValue = Math.Round(cAmount, Convert.ToInt32(Decimals));
						break;
					case rrRoundingRule.rrRoundDown:
						functionReturnValue = Math.Round(cAmount, Convert.ToInt32(Decimals));
						break;
				}
			}
			return functionReturnValue;
		}
		public string FixQuotes(object Source, string Delim = "'", bool fSpaceForNull = true)
		{
			string functionReturnValue = null;
			functionReturnValue = clsSupport.bpeNullString;

			//Avoid countermanding a single space placed in the field by fmScreen.ChkFld to avoid Nulls...
			string strTemp = clsSupport.bpeNullString;
			if (fSpaceForNull) {
				if ((Source == null) || Information.IsDBNull(Source) || Convert.ToString(Source).Trim().Length == 0) {
					strTemp = " ";
				} else if (Convert.ToString(Source) != " ") {
					strTemp = Convert.ToString(Source).Trim();
				} else {
					strTemp = Convert.ToString(Source);
				}
			} else {
				if (Information.IsDBNull(Source))
					strTemp = clsSupport.bpeNullString;
				else
					strTemp = Strings.Trim((string)Source);
			}

			//First eliminate all double - single quotes
			int i = 0;
			do {
				i = Strings.InStr(1, strTemp, Delim + Delim);
				if (i != 0)
					strTemp = Strings.Left(strTemp, i - 1) + Delim + Strings.Mid(strTemp, i + 2);
			} while (!(i == 0));
			//Then replace all single quotes with double - single quotes
			i = Strings.InStr(1, strTemp, Delim);
			while (i > 0) {
				strTemp = Strings.Left(strTemp, i - 1) + Delim + Delim + Strings.Mid(strTemp, i + 1);
				i = Strings.InStr(i + 2, strTemp, Delim);
			}
			functionReturnValue = strTemp;
			return functionReturnValue;
		}
		public new string FormatBytes(double TotalBytes)
		{
			string functionReturnValue = null;
			functionReturnValue = clsSupport.bpeNullString;

			if (TotalBytes > (double)ByteEnum.TB) {
				functionReturnValue = string.Format("{0:#,##0.00} TB", TotalBytes / (double)ByteEnum.TB);
			} else if (TotalBytes > (double)ByteEnum.GB) {
				functionReturnValue = string.Format("{0:#,##0.00} GB", TotalBytes / (double)ByteEnum.GB);
			} else if (TotalBytes > (double)ByteEnum.MB) {
				functionReturnValue = string.Format("{0:#,##0.00} MB", TotalBytes / (double)ByteEnum.MB);
			} else if (TotalBytes > (double)ByteEnum.KB) {
				functionReturnValue = string.Format("{0:#,##0.00} KB", TotalBytes / (double)ByteEnum.KB);
			} else {
				functionReturnValue = string.Format("{0:#,##0} Bytes", TotalBytes);
			}
			return functionReturnValue;
		}
		public string FormatElapsed(int milliseconds, bool Format)
		{
			string functionReturnValue = null;
			functionReturnValue = clsSupport.bpeNullString;
			TimeSpan ts = new TimeSpan(0, 0, 0, 0, milliseconds);
			if (!Format) {
				if (ts.Days > 0)
					functionReturnValue += string.Format("{0} Days, ", ts.Days);
				if (ts.Hours > 0)
					functionReturnValue += string.Format("{0} Hours, ", ts.Hours);
				if (ts.Minutes > 0)
					functionReturnValue += string.Format("{0} Minutes, ", ts.Minutes);
				functionReturnValue += string.Format("{0}.{1:000} Seconds", ts.Seconds, ts.Milliseconds);
			} else {
				//By default TimeSpan.ToString displays 7 decimals, let's only go to 3...
				if (ts.Days > 0) {
					functionReturnValue = string.Format("{0}.{1:00}:{2:00}:{3:00}.{4:000}", new object[] {
						ts.Days,
						ts.Hours,
						ts.Minutes,
						ts.Seconds,
						ts.Milliseconds
					});
				} else {
					functionReturnValue = string.Format("{0:00}:{1:00}:{2:00}.{3:000}", new object[] {
						ts.Hours,
						ts.Minutes,
						ts.Seconds,
						ts.Milliseconds
					});
				}
			}
			return functionReturnValue;
		}
		public bool FormatMoney(TextBox txtControl, int NegColor = (int)vbRGBColorConstants.vbRed, int PosColor = (int)vbRGBColorConstants.vbBlack, string CurrencySymbol = "$")
		{
			System.Drawing.Color nColor = System.Drawing.ColorTranslator.FromOle(NegColor);
			System.Drawing.Color pColor = System.Drawing.ColorTranslator.FromOle(PosColor);
			return this.FormatMoney(txtControl, nColor, pColor, CurrencySymbol);
		}
		public bool FormatMoney(TextBox txtControl, System.Drawing.Color nColor, System.Drawing.Color pColor, string CurrencySymbol = "$")
		{
			bool functionReturnValue = false;
			functionReturnValue = false;
			if ((nColor == null))
				nColor = Color.Red;
			if ((pColor == null))
				pColor = Color.Black;

			string cSymbol = CurrencySymbol;
			string lCurrencySymbol = RegionInfo.CurrentRegion.CurrencySymbol;
			//GetRegionalSetting(Win.WinNLS.LOCALE_SCURRENCY)

			if (txtControl.Text == clsSupport.bpeNullString)
				txtControl.Text = string.Format("{0}0.00", cSymbol);
			string originalText = txtControl.Text;
			try {
				string newText = Convert.ToString(Convert.ToDecimal(txtControl.Text).ToString("c")).Trim();
				if (newText.Length > txtControl.MaxLength && txtControl.MaxLength != 0) {
					string Message = string.Format("Invalid currency value entered: {0}.{1}Value cannot exceed ", originalText, Constants.vbCr);
					int realLength = txtControl.MaxLength;
					if (Convert.ToDecimal(txtControl.Text) < 0)
						realLength = txtControl.MaxLength - 1;
					switch (realLength) {
						case 18:
							Message += "99,999,999,999";
							break;
						case 17:
							Message += "9,999,999,999";
							break;
						case 15:
						case 16:
							Message += "999,999,999";
							break;
						case 14:
							Message += "99,999,999";
							break;
						case 13:
							Message += "9,999,999";
							break;
						case 12:
						case 11:
							Message += "999,999";
							break;
						case 10:
							Message += "99,999";
							break;
						case 9:
							Message += "9,999";
							break;
						case 8:
						case 7:
							Message += "999";
							break;
						case 6:
							Message += "99";
							break;
						case 5:
							Message += "9";
							break;
						default:
							Message += ".99";
							break;
					}
					txtControl.Text = originalText;
					TextSelected(txtControl);
					throw new Exception(Message);
				}
				if (this.RndCents(Convert.ToDecimal(newText)) != RndCents(Convert.ToDecimal(originalText))) {
					txtControl.Text = originalText;
					TextSelected(txtControl);
					throw new Exception(string.Format("Invalid currency value entered: {0}.", originalText));
				}
				//Not ready to "prime-time" as adding a non-regional settings based currency symbol will trigger cataclysmic problems
				//in built-in VB type conversion routines that assume regional settings values...
				//        If CurrencySymbol <> lCurrencySymbol Then newText = Replace(newText, lCurrencySymbol, CurrencySymbol)
				txtControl.Text = newText;
				if (Convert.ToDecimal(txtControl.Text) < 0)
					txtControl.ForeColor = nColor;
				else
					txtControl.ForeColor = pColor;
				functionReturnValue = true;
			} catch (OverflowException ex) {
				throw new OverflowException(string.Format("Invalid currency value entered: {0}.{1}Value cannot exceed 900 Billion.", originalText, Constants.vbCr), ex);
			} catch (Exception ex) when (ex.Message.Contains("Value cannot exceed")) {
				throw new Exception(ex.Message, ex);
			} catch (Exception ex) {
				throw new Exception(string.Format("Invalid currency value entered: {0}.{1}Please enter a valid currency amount.", originalText, Constants.vbCr), ex);
			}
			return functionReturnValue;
		}
		public void FormatPercentage(ref string data)
		{
			string strTemp = null;
			string originalText = clsSupport.bpeNullString;
			try {
				string[] strArgs = { "Nothing" };
				if ((data != null))
					strArgs[0] = "\"" + data + "\"";
				else
					data = clsSupport.bpeNullString;

				originalText = data;
				strTemp = data;
				if (strTemp.IndexOf("%") > -1)
					strTemp = strTemp.Replace("%", clsSupport.bpeNullString);
				if (strTemp == clsSupport.bpeNullString)
					strTemp = "0";
				data = (Convert.ToDouble(strTemp) / 100).ToString("p2");
			} catch (Exception ex) {
				data = originalText;
				throw new ArgumentException(string.Format("Invalid percent value entered: {0}. Please enter a valid percentage.", originalText));
			}
		}
		public void FormatPercentage(TextBox txtControl, int NegColor = (int)vbRGBColorConstants.vbRed, int PosColor = (int)vbRGBColorConstants.vbBlack)
		{
			System.Drawing.Color nColor = System.Drawing.ColorTranslator.FromOle(NegColor);
			System.Drawing.Color pColor = System.Drawing.ColorTranslator.FromOle(PosColor);

			this.FormatPercentage(txtControl, nColor, pColor);
		}
		public void FormatPercentage(TextBox txtControl, System.Drawing.Color nColor, System.Drawing.Color pColor)
		{
			try {
				if ((nColor == null))
					nColor = Color.Red;
				if ((pColor == null))
					pColor = Color.Black;
                string tmpValue = txtControl.Text;
				this.FormatPercentage(ref tmpValue);
                txtControl.Text = tmpValue;
				txtControl.ForeColor = (DecodePercentage(txtControl.Text) < 0 ? nColor : pColor);
			} catch (Exception ex) {
				TextSelected(txtControl);
				throw new Exception(ex.Message, ex);
			}
		}
		public void FormatPhone(TextBox txtControl)
		{
			switch (txtControl.Text.Length) {
				case 1:
                    txtControl.Text = "(" + txtControl.Text;
                    txtControl.SelectionStart = 3;
					break;
				case 4:
                    txtControl.Text = txtControl.Text + ")";
                    txtControl.SelectionStart = 6;
					break;
				case 8:
                    txtControl.Text = txtControl.Text + "-";
                    txtControl.SelectionStart = 10;
					break;
			}
		}
		public string FormatSeconds(double Seconds, short tFormat = 0)
		{
			string functionReturnValue = null;
			functionReturnValue = clsSupport.bpeNullString;
			int SS = (int)Seconds;
			int HH = SS / 3600;
			SS -= (HH * 3600);
			int MM = SS / 60;
			SS -= (MM * 60);
			string strTime = clsSupport.bpeNullString;
			if (HH > 0)
				strTime = HH + " Hours, ";
			if (MM > 0)
				strTime += MM + " Minutes, ";
			strTime += SS + " Seconds";

			switch (tFormat) {
				case 0:
					functionReturnValue = string.Format("{0:000.00000}", Seconds);
					break;
				default:
					functionReturnValue = strTime;
					break;
			}
			return functionReturnValue;
		}
		public string GetUnselectedText(TextBox sender)
		{
			string functionReturnValue = null;
			functionReturnValue = clsSupport.bpeNullString;
			if ((sender == null))
				return functionReturnValue;
			if (sender.SelectionLength == sender.Text.Length)
				return functionReturnValue;
			//Nothing's "unselected"...
			if (sender.SelectionLength == 0){functionReturnValue = sender.Text;return functionReturnValue;}
			//Everything's "unselected"

			//Beginning
			if (sender.SelectionStart == 0) {
				functionReturnValue = sender.Text.Substring(sender.SelectionLength);
			//End
			} else if (sender.SelectionStart + sender.SelectionLength == sender.Text.Length) {
				functionReturnValue = sender.Text.Substring(0, sender.SelectionStart);
			//Middle
			} else {
				//For our purposes, concatenate the two pieces together - it doesn't need to make sense, just searching for characters here...
				functionReturnValue = (sender.Text.Substring(0, sender.SelectionStart)) + sender.Text.Substring(sender.SelectionLength);
			}
			return functionReturnValue;
		}
		public void KeyPressUcase(ref short KeyAscii)
		{
			KeyAscii = (short)Strings.Asc(Strings.UCase(Strings.Chr(KeyAscii)));
		}
		public void KeyPressInteger(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			short cNum = (short)Strings.Asc(e.KeyChar);
			this.KeyPressInteger(ref cNum);
			if (cNum == (short)vbKeyEnum.vbKeyBell) {
				e.Handled = true;
				throw new ArgumentException(string.Format("\"{0}\" is not a valid integer digit", e.KeyChar));
			}
		}
		public void KeyPressInteger(ref short KeyAscii)
		{
            if (KeyAscii != (short)Keys.Space && KeyAscii != (short)vbKeyEnum.vbKeyMinus && 
                KeyAscii != (short)Keys.Subtract && !(KeyAscii >= (short)Keys.D0 && KeyAscii <= (short)Keys.D0)) {
					KeyAscii = (short)vbKeyEnum.vbKeyBell;
            }
		}
		public void KeyPressReal(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			short cNum = (short)Strings.Asc(e.KeyChar);
			this.KeyPressReal(ref cNum);
			if (cNum == (short)vbKeyEnum.vbKeyBell) {
				e.Handled = true;
				throw new ArgumentException(string.Format("\"{0}\" is not a valid decimal-number digit", e.KeyChar));
			}
		}
		public void KeyPressReal(ref short KeyAscii)
		{
			switch (KeyAscii) {
				case (short)Keys.Back:
					break;
				case (short)vbKeyEnum.vbKeyPeriod:
					break;
				case (short)Keys.Space:
					break;
				case (short)vbKeyEnum.vbKeyMinus:
					break;
				case (short)Keys.Subtract:
					break;
				case (short)Keys.D0:
                case (short)Keys.D1:
                case (short)Keys.D2:
                case (short)Keys.D3:
                case (short)Keys.D4:
                case (short)Keys.D5:
                case (short)Keys.D6:
                case (short)Keys.D7:
                case (short)Keys.D8:
                case (short)Keys.D9:
                    break;
				default:
					KeyAscii = (short)vbKeyEnum.vbKeyBell;
					break;
			}
		}
		public string RightFill(object Source, int iLength, char FillChar = ' ')
		{
			string functionReturnValue = null;
			functionReturnValue = clsSupport.bpeNullString;

			if ((Source == null) || Information.IsDBNull(Source))
				Source = clsSupport.bpeNullString;
			string s = Convert.ToString(Source).Trim();
			if (s.Length > iLength)
				throw new ArgumentException(string.Format("Given string (\"{0}\") length ({1}) exceeds fill length ({2})", s, s.Length, iLength));
			bool fNegative = s.StartsWith("-");
			if (Information.IsNumeric(s) && FillChar == '0' && fNegative) {
				s = s.Substring(1);
				s = s.PadLeft(iLength - 1, FillChar);
				s = string.Format("-{0}", s);
				functionReturnValue = s;
			} else {
				functionReturnValue = s.PadLeft(iLength, FillChar);
			}
			return functionReturnValue;
		}
		public decimal RndCents(decimal Amt)
		{
			decimal functionReturnValue = default(decimal);
			string strAmt = string.Format("{0:0.######}", Amt);
			bool Negative = Convert.ToBoolean(Amt < 0);
			strAmt = strAmt.Replace("-", clsSupport.bpeNullString);
			short iDecimal = (short)strAmt.IndexOf(".");
			string strWhole = strAmt;
			if (iDecimal != -1)
				strWhole = strAmt.Substring(0, iDecimal);
			string strFraction = "0.0";
			if (iDecimal != -1)
				strFraction = string.Format("0{0}", strAmt.Substring(iDecimal));
			float sFraction = Convert.ToSingle(strFraction);
			decimal dWhole = Convert.ToDecimal(strWhole);
			short iAmt = (short)(sFraction * 100);
			sFraction = iAmt / 100;
            functionReturnValue = dWhole + (decimal)sFraction;
			if (Negative)
				functionReturnValue = -functionReturnValue;
			return functionReturnValue;
		}
		public string LeftFill(object Source, int iLength, char FillChar = ' ')
		{
			string functionReturnValue = null;
			functionReturnValue = clsSupport.bpeNullString;

			if ((Source == null) || Information.IsDBNull(Source))
				Source = clsSupport.bpeNullString;
			string s = Convert.ToString(Source).Trim();
			if (s.Length > iLength)
				throw new ArgumentException(string.Format("Given string (\"{0}\") length ({1}) exceeds fill length ({2})", s, s.Length, iLength));
			functionReturnValue = s.PadRight(iLength, FillChar);
			return functionReturnValue;
		}
		public void SetTitleCase(TextBox txtControl)
		{
			txtControl.Text = Strings.StrConv(txtControl.Text, VbStrConv.ProperCase);
		}
		public void SetUpperCase(TextBox txtControl)
		{
			txtControl.Text = Strings.StrConv(txtControl.Text, VbStrConv.Uppercase);
		}
		public string szStringToString(string szString)
		{
			string functionReturnValue = null;
			functionReturnValue = clsSupport.bpeNullString;
			string strString = null;
			if ((szString == null)){strString = "Nothing";szString = clsSupport.bpeNullString;}
			else
				strString = "\"" + szString + "\"";
			functionReturnValue = szString;
			short iPos = (short)Strings.InStr(szString, Strings.Chr(0).ToString());
			if (iPos > 0)
				functionReturnValue = Strings.Left(szString, iPos - 1);
			return functionReturnValue;
		}
		public bool TagContains(Control iControl, string SearchTag)
		{
			bool functionReturnValue = false;
			functionReturnValue = false;

			string strTag = (iControl.Tag == null ? clsSupport.bpeNullString : Convert.ToString(iControl.Tag));
			string[] tagParams = strTag.Split(",".ToCharArray());
			for (int i = 0; i <= tagParams.Length - 1; i++) {
				if (tagParams[i].ToUpper() == SearchTag.ToUpper()){functionReturnValue = true;return functionReturnValue;}
				//Try
			}
			return functionReturnValue;
		}

		public void TagRemove(Control iControl, string Value)
		{
			if ((iControl.Tag == null) || Convert.ToString(iControl.Tag).Trim() == clsSupport.bpeNullString || !this.TagContains(iControl, Value))
				return;
			//Try
			string strTag = (string)iControl.Tag;
			string[] tagParams = strTag.Split(",".ToCharArray());
			strTag = clsSupport.bpeNullString;
			for (int i = 0; i <= tagParams.Length - 1; i++) {
				if (tagParams[i].ToUpper() != Value.ToUpper())
					strTag += tagParams[i] + ",";
			}
			if (strTag.EndsWith(","))
				strTag = strTag.Substring(0, strTag.Length - 1);
			iControl.Tag = strTag;
		}
		public void TagSet(Control iControl, string Value)
		{
			this.TagRemove(iControl, Value);
			if ((iControl.Tag == null) || Convert.ToString(iControl.Tag).Trim() == clsSupport.bpeNullString) {
				iControl.Tag = Value;
			} else {
				iControl.Tag += "," + Value;
			}
		}
		public void TextKeyDown(TextBox txtControl, short KeyCode, short Shift)
		{
			switch (KeyCode) {
				case (short)Keys.F11:
					SetTitleCase(txtControl);
					break;
				case (short)Keys.F12:
					SetUpperCase(txtControl);
					break;
			}
		}
		public void TextSelected(Control ctl)
		{
			switch (ctl.GetType().Name) {
				case "ComboBox":
					var _with9 = (ComboBox)ctl;
					if (_with9.DropDownStyle != ComboBoxStyle.DropDownList)
						_with9.Select(0, _with9.Text.Length);
					break;
				case "TextBox":
					var _with10 = (TextBox)ctl;
					_with10.Select(0, _with10.Text.Length);
					break;
			}
		}
		public short UpCase(short uKey)
		{
			return (short)Strings.Asc(Strings.UCase(Strings.Chr(uKey)));
		}
		public short Verify(string Source, string ValueList)
		{
			short functionReturnValue = 0;
			functionReturnValue = 0;
            bool continueLoop = true;
			for (short i = 1; i <= Source.Length && continueLoop; i++) {
				if (Strings.InStr(ValueList, Strings.Mid(Source, i, 1)) == 0) {
                    functionReturnValue = i;
                    continueLoop = false;
                }
			}
			return functionReturnValue;
		}
		#endregion
		#region "Event Handlers"
		//None at this time...
		#endregion
	}
}

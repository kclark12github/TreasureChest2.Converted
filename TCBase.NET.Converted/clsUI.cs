//clsUI - clsUI.vb
//   TreasureChest2 User Interface Class...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   10/08/14    Upgraded project to Visual Studio 2013;
//   03/25/14    Upgraded UpdateRTF;
//   07/19/05    Set rtfMessage.Rtf property instead of .SelectedRtf in ShowMsgBoxRTF();
//   03/11/05    Made New() constructor "Friend" in order to accomplish PublicNotCreatable VB6 Class Instancing behavior;
//   03/03/05    Upgraded for use as a VB.NET Component;
//   03/26/04    Added logic to support remaining vbMsgBoxStyle values;
//   01/25/03    Added SetTopmostWindow;
//   11/19/02    Added UpdateRTF();
//   11/19/02    Updated ShowMsgBox() to support MsgBox-like button handling;
//               Added ShowMsgBoxRTF();
//   10/30/02    Added "On Error Resume Next" to functions without any error handling;
//   12/10/01    Added ShowMsgBox;
//   11/19/01    Added optional InitialDirectory argument to ChooseFolder;
//               Added Display functionality from clsFiRReApplication;
//               Added new arguments to Display;
//   11/14/01    Created;
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
	public class clsUI : clsSupportBase
	{
		internal clsUI(clsSupport objSupport) : base(objSupport, "clsUI")
		{
			Trace("objSupport:={" + objSupport.GetType().ToString() + "}", trcOption.trcSupport);
		}

		#region "Events"
		//None at this time...
		#endregion
		#region "Properties"
		[FlagsAttribute()]
		public enum DisplayEnum
		{
			deStatusBarOnly = 0,
			deTextBoxOnly = 1,
			deBothStatusBarAndTextBox = deStatusBarOnly | deTextBoxOnly
		}
		//const short SWP_NOMOVE = 2;
		//const short SWP_NOSIZE = 1;
		//const bool Flags = SWP_NOMOVE | SWP_NOSIZE;
		//const short HWND_TOPMOST = -1;

		//const short HWND_NOTOPMOST = -2;
		private System.Windows.Forms.RichTextBox mrtfDisplay = null;
		private StatusBarPanel msbStatusMessage = null;
		public RichTextBox drtfDisplay {
			get { return mrtfDisplay; }
			set { mrtfDisplay = value; }
		}
		public StatusBarPanel dsbStatusMessage {
			get { return msbStatusMessage; }
			set { msbStatusMessage = value; }
		}
		[DllImport("user32", EntryPoint = "SetWindowPos", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
		#endregion
		#region "Methods"
		private static extern int SetWindowPos(int hwnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);
		public string ChooseFolder(string Caption, string InitialDirectory = clsSupport.bpeNullString)
		{
			string functionReturnValue = null;
			System.Windows.Forms.FolderBrowserDialog fbd = null;
			functionReturnValue = clsSupport.bpeNullString;
			try {
				if (!InitialDirectory.EndsWith("\\"))
					InitialDirectory = InitialDirectory + "\\";
				if (InitialDirectory != clsSupport.bpeNullString && FileSystem.Dir(InitialDirectory, FileAttribute.Directory) != clsSupport.bpeNullString) {
					FileSystem.ChDir(InitialDirectory);
					if (mSupport.ParsePath(InitialDirectory, ParseParts.DrvOnly) != clsSupport.bpeNullString && mSupport.ParsePath(InitialDirectory, ParseParts.DrvOnly) != mSupport.ParsePath(FileSystem.CurDir(), ParseParts.DrvOnly)) {
						FileSystem.ChDrive(mSupport.ParsePath(InitialDirectory, ParseParts.DrvOnly));
					}
				}

				fbd = new System.Windows.Forms.FolderBrowserDialog();
				if (fbd.RootFolder.ToString() != InitialDirectory)
                    fbd.SelectedPath = InitialDirectory;
                fbd.Description = Caption;
                fbd.ShowNewFolderButton = true;
				switch (fbd.ShowDialog()) {
					case DialogResult.OK:
						functionReturnValue = fbd.SelectedPath;
						break;
				}
			} finally {
				fbd = null;
			}
			return functionReturnValue;
		}
		public void Display(string Message, DisplayEnum Location, Color FontColor, TriState FontBold = TriState.UseDefault, TriState FontItalic = TriState.UseDefault, string FontName = clsSupport.bpeNullString, float FontSize = 0)
		{
			string statusMessage = null;
			FontStyle fsFontStyle = default(FontStyle);
			Font saveFont = null;
			Color saveColor = default(Color);
			//Bail if we're not in the proper environment...
			//The next statements would trigger a [Control 'xxx' not found] error (#730)
			//if the screen controls are not found...
			if ((mrtfDisplay == null) & (Location & DisplayEnum.deStatusBarOnly) == DisplayEnum.deStatusBarOnly)
				throw new ArgumentException(string.Format("Must define RichTextBox (.rtfDisplay) if using Locations ({0} or {1})", DisplayEnum.deTextBoxOnly.ToString(), DisplayEnum.deBothStatusBarAndTextBox.ToString()));
			if ((msbStatusMessage == null) & (Location & DisplayEnum.deTextBoxOnly) == DisplayEnum.deTextBoxOnly)
				throw new ArgumentException(string.Format("Must define StatusBarPanel (.sbStatusMessage) if using Locations ({0} or {1})", DisplayEnum.deStatusBarOnly.ToString(), DisplayEnum.deBothStatusBarAndTextBox.ToString()));
			if (FontBold == TriState.UseDefault)
				FontBold = (mrtfDisplay.Font.Bold ? TriState.True : TriState.False);
			if (FontItalic == TriState.UseDefault)
				FontItalic = (mrtfDisplay.Font.Italic ? TriState.True : TriState.False);
			if (FontName == clsSupport.bpeNullString)
				FontName = mrtfDisplay.Font.Name;
			if (FontSize == 0)
				FontSize = mrtfDisplay.Font.SizeInPoints;

			statusMessage = Message;
			statusMessage = Strings.Replace(statusMessage, Constants.vbTab, " ");
			statusMessage = Strings.Replace(statusMessage, Constants.vbCrLf, " ");
			statusMessage = Strings.Replace(statusMessage, Constants.vbCr, " ");
			statusMessage = Strings.Replace(statusMessage, Constants.vbLf, " ");
			statusMessage = Strings.Trim(statusMessage);

			Message = Strings.Replace(Message, Constants.vbCr, Constants.vbCrLf);
			//strMessage = Replace(strMessage, vbLf, vbCrLf)
			Message = Strings.Replace(Message, Constants.vbCr + Constants.vbCr, Constants.vbCr);
			Message = Strings.Replace(Message, Constants.vbLf + Constants.vbLf, Constants.vbLf);
			Message = Strings.Trim(Message);

			if ((Location & DisplayEnum.deStatusBarOnly) == DisplayEnum.deStatusBarOnly)
				msbStatusMessage.Text = statusMessage;
			if ((Location & DisplayEnum.deTextBoxOnly) == DisplayEnum.deTextBoxOnly) {
				if (Message == clsSupport.bpeNullString) {
                    mrtfDisplay.Text = Message;
                    mrtfDisplay.Refresh();
					return;
				}
				//Make sure we don't get too big by truncating the top half of the display...
				if (mrtfDisplay.TextLength + Message.Length > mrtfDisplay.MaxLength) {
					int iKeepSize = mrtfDisplay.Lines.Length / 2;
					string[] TempArray = new string[iKeepSize + 2];
					Array.Copy(mrtfDisplay.Lines, iKeepSize, TempArray, 0, iKeepSize);
					mrtfDisplay.Lines = TempArray;
					TempArray = null;
				}

				saveFont = mrtfDisplay.SelectionFont;
				saveColor = mrtfDisplay.SelectionColor;

				fsFontStyle = (FontBold == TriState.True ? FontStyle.Bold : FontStyle.Regular) | (FontItalic == TriState.True ? FontStyle.Italic : FontStyle.Regular);
                mrtfDisplay.SelectionFont = new Font(FontName, FontSize, fsFontStyle);
                mrtfDisplay.SelectionFont = new Font(FontName, FontSize, fsFontStyle);
                mrtfDisplay.SelectionColor = FontColor;

                //.SelectedRtf = strMessage & vbCrLf
                //.SelectionStart = .TextLength   'Len(.Text)
                Message += Constants.vbCrLf;
                mrtfDisplay.AppendText(Message);
                mrtfDisplay.SelectionStart = mrtfDisplay.Text.Length;
                mrtfDisplay.SelectionLength = 0;
                mrtfDisplay.ScrollToCaret();

                mrtfDisplay.Font = saveFont;
                mrtfDisplay.SelectionColor = saveColor;
			}
		}
		public DialogResult ShowMsgBox(string Message, MsgBoxStyle MsgBoxStyle, Form Parent = null, string Caption = clsSupport.bpeNullString, System.Drawing.Icon Icon = null)
		{
            DialogResult functionReturnValue = default(DialogResult);
			frmMsgBox frm = null;
			try {
				frm = new frmMsgBox(mSupport, Parent);
				//Default from Parent Form...
				if ((Parent != null)) {
                    frm.Icon = ((Icon != null) ? Icon : Parent.Icon);
                    frm.Text = (Caption != clsSupport.bpeNullString ? Caption : Parent.Text);
				//Default from project/assembly (if possible)...
				} else {
					if ((Icon != null)) {
                        frm.Icon = Icon;
					} else if (mSupport.EntryComponentName != clsSupport.bpeNullString) {
						Trace(mMyTraceID + " Defaulting .Icon from project/assembly...", trcOption.trcSupport);
                        frm.Icon = mSupport.Icon(string.Format("{0}\\{1}.exe", mSupport.ApplicationPath, mSupport.EntryComponentName));
					}
                    frm.Text = (Caption != clsSupport.bpeNullString ? Caption : mSupport.ApplicationName);
				}
                frm.Message = Message;
                frm.MsgBoxStyle = MsgBoxStyle;
				Trace(mMyTraceID + "Calling frmShowMsgBox.ShowDialog()", trcOption.trcSupport);
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog(Parent);
				functionReturnValue = frm.DialogResult;
                frm.OKtoClose = true;
				Trace(mMyTraceID + "Calling frmShowMsgBox.Close()", trcOption.trcSupport);
                frm.Close();
			} finally {
				frm = null;
			}
			return functionReturnValue;
		}
		//public void UpdateRTF(RichTextBox rtfControl, string Message, string FontName, float FontSize, System.Drawing.Color vbColor, CheckState Bold, CheckState Italic, CheckState CharOffset, CheckState Underline, CheckState StrikeThru,
		//        HorizontalAlignment Alignment = HorizontalAlignment.Left, CheckState Bullet = CheckState.Indeterminate, float HangingIndent = 0, float Indent = 0, float RightIndent = 0)
		//{
		//	try {
		//		//Save current selection point settings...
		//		//Font attributes...
		//		string saveFontName = rtfControl.SelectionFont.Name;
		//		float saveFontSize = rtfControl.SelectionFont.SizeInPoints;
		//		System.Drawing.Color saveColor = rtfControl.SelectionColor;
		//		//Character style attributes
		//		bool saveBold = rtfControl.SelectionFont.Bold;
		//		bool saveItalic = rtfControl.SelectionFont.Italic;
		//		int saveCharOffset = rtfControl.SelectionCharOffset;
		//		bool saveUnderline = rtfControl.SelectionFont.Underline;
		//		bool saveStrikeThru = rtfControl.SelectionFont.Strikeout;
		//		//Paragraph attributes...
		//		HorizontalAlignment saveAlignment = rtfControl.SelectionAlignment;
		//		bool saveBullet = rtfControl.SelectionBullet;
		//		float saveHangingIndent = rtfControl.SelectionHangingIndent;
		//		float saveIndent = rtfControl.SelectionIndent;
		//		float saveRightIndent = rtfControl.SelectionRightIndent;

		//		//Default optional arguments...
		//		if (FontName == clsSupport.bpeNullString)
		//			FontName = saveFontName;
		//		if (FontSize == 0)
		//			FontSize = saveFontSize;
		//		if (vbColor == null)
		//			vbColor = saveColor;
		//		//Character style attributes
		//		bool NewBold = false;
  //              if (Bold == CheckState.Indeterminate)
  //                  NewBold = saveBold;
  //              else
  //                  NewBold = (Bold == CheckState.Checked);
		//		bool NewItalic = false;
  //              if (Italic == CheckState.Indeterminate)
  //                  NewItalic = saveItalic;
  //              else
  //                  NewItalic = (Italic == CheckState.Checked);
		//		//bool NewCharOffset = false;
		//		//if (CharOffset == CheckState.Indeterminate)
		//		//	NewCharOffset = saveCharOffset;
		//		//else
		//		//	if (CharOffset == CheckState.Unchecked)
		//		//		NewCharOffset = false;
		//		//	else
		//		//		NewCharOffset = true;
		//		bool NewUnderline = false;
		//		if (Underline == CheckState.Indeterminate)
		//			NewUnderline = saveUnderline;
		//		else
  //                  NewUnderline = (Underline == CheckState.Checked);
		//		bool NewStrikeThru = false;
		//		if (StrikeThru == CheckState.Indeterminate)
		//			NewStrikeThru = saveStrikeThru;
		//		else
  //                  NewStrikeThru = (StrikeThru == CheckState.Checked);
		//		//Paragraph attributes...
		//		bool NewBullet = false;
		//		if (Bullet == CheckState.Indeterminate)
		//			NewBullet = saveBullet;
		//		else
  //                  NewBullet = (Bullet == CheckState.Checked);
		//		if (HangingIndent == 0)
		//			HangingIndent = saveHangingIndent;
		//		if (Indent == 0)
		//			Indent = saveIndent;
		//		if (RightIndent == 0)
		//			RightIndent = saveRightIndent;

  //              //Reset Selection point to the end of the existing .Text...
  //              rtfControl.SelectionStart = Strings.Len(rtfControl.Text);

		//		//Define attributes at selection point...
		//		FontStyle fsFontStyle = default(FontStyle);
		//		fsFontStyle = (NewBold ? FontStyle.Bold : FontStyle.Regular) | (NewItalic ? FontStyle.Italic : FontStyle.Regular) | (NewUnderline ? FontStyle.Underline : FontStyle.Regular) | (NewStrikeThru ? FontStyle.Strikeout : FontStyle.Regular);
  //              rtfControl.SelectionFont = new Font(FontName, FontSize, fsFontStyle);
  //              rtfControl.SelectionColor = vbColor;
  //              rtfControl.SelectionCharOffset = NewCharOffset;
  //              //Paragraph attributes...
  //              rtfControl.SelectionAlignment = Alignment;
  //              rtfControl.SelectionBullet = NewBullet;
  //              rtfControl.SelectionHangingIndent = HangingIndent;
  //              rtfControl.SelectionIndent = Indent;
  //              rtfControl.SelectionRightIndent = RightIndent;

  //              //Write message...
  //              rtfControl.SelectedText = Message;

		//		//Reset attributes back to default values...

		//		//Font attributes...
		//		fsFontStyle = (saveBold ? FontStyle.Bold : FontStyle.Regular) | (saveItalic ? FontStyle.Italic : FontStyle.Regular) + (saveUnderline ? FontStyle.Underline : FontStyle.Regular) + (saveStrikeThru ? FontStyle.Strikeout : FontStyle.Regular);
  //              rtfControl.SelectionFont = new Font(saveFontName, saveFontSize, fsFontStyle);
  //              rtfControl.SelectionColor = saveColor;
  //              rtfControl.SelectionCharOffset = saveCharOffset;
  //              //Character style attributes
  //              //Paragraph attributes...
  //              rtfControl.SelectionAlignment = saveAlignment;
  //              rtfControl.SelectionBullet = saveBullet;
  //              rtfControl.SelectionHangingIndent = saveHangingIndent;
  //              rtfControl.SelectionIndent = saveIndent;
  //              rtfControl.SelectionRightIndent = saveRightIndent;
		//	} finally {
		//	}
		//}
		#endregion
		#region "Event Handlers"
		//None at this time...
		#endregion
	}
}

//frmSelect.vb
//   Selection/Filter Form for TreasureChest2 Project...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   07/21/18    Corrected issue with square bracket in wild carded filters (replacing "[" with "[[]" in lieu 
//               of "\[" with ESCAPE '\');
//   12/22/17    Reworked CheckState progression;
//   11/19/17    Force display upper-case LIKE operands;
//   09/18/16    Updated object references to reflect architectural changes;
//   09/23/11    Commented code in Clear method that eliminated the .DataSource property from ComboBox controls as this effectively
//               cleared the list of selections - which is not the purpose of the Clear;
//   03/13/11    Modified ParseFilter to include same ComboBox/TextBox logic as Control_Validating (so operations other than "="
//               and "Like" would be properly handled);
//   07/26/10    Reorganized registry settings;
//   01/01/10    Created in VB.NET;
//=================================================================================================================================
//TODO: Test filter strings containing ":" characters and trap any resulting errors before failing more seriously...
//TODO: Add Functionality to "filter" ComboBox display based on data in the other controls...
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
	public class frmSelect : frmTCBase
	{
		public frmSelect(clsSupport objSupport, clsTCBase objTCBase, Form objParent = null, string Caption = null) : base(objSupport, "frmSelect", objTCBase, objParent)
		{
			Closing += frmSelect_Closing;
			Load += frmSelect_Load;
			InitializeComponent();
			mRegistryKey = string.Format("{0}\\{1} Settings\\Select Settings", mTCBase.RegistryKey, mTCBase.ActiveForm.Name);
			if ((Caption != null))
				this.Text = Caption;

			this.SuspendLayout();
			//Build a sorted list of control names (sorted on TabIndex)...
			string[] aControls = null;
			aControls = new string[mTCBase.CurrencyManager.Bindings.Count];
			for (int i = 0; i <= mTCBase.CurrencyManager.Bindings.Count - 1; i++) {
				Binding iBinding = mTCBase.CurrencyManager.Bindings[i];
				aControls[i] = string.Format("{0:0000},{1}", iBinding.Control.TabIndex, iBinding.Control.Name);
			}
			Array.Sort(aControls);
			for (int i = 0; i <= mTCBase.CurrencyManager.Bindings.Count - 1; i++) {
				//Debug.WriteLine(aControls(i))
				NewControl(FindBindingByControlName(aControls[i].Split(",".ToCharArray())[1]));
			}
			this.btnApply.Location = new System.Drawing.Point(this.btnApply.Location.X, stdSpacer + (iControlLines * (stdHeight + stdSpacer)) + (2 * stdSpacer));
			this.btnClear.Location = new System.Drawing.Point(this.btnClear.Location.X, stdSpacer + (iControlLines * (stdHeight + stdSpacer)) + (2 * stdSpacer));
			this.btnCancel.Location = new System.Drawing.Point(this.btnCancel.Location.X, stdSpacer + (iControlLines * (stdHeight + stdSpacer)) + (2 * stdSpacer));
			this.ClientSize = new Size(this.ClientSize.Width, stdSpacer + (iControlLines * (stdHeight + stdSpacer)) + (2 * stdSpacer) + this.btnApply.Height + stdSpacer);
			this.btnApply.TabIndex = iTab;
			iTab += 1;
			this.btnClear.TabIndex = iTab;
			iTab += 1;
			this.btnCancel.TabIndex = iTab;
			iTab += 1;
			this.MinimumSize = new Size(this.Width, this.Height);
			this.ResumeLayout(false);

			this.Icon = mTCBase.ActiveForm.Icon;
		}
		#region " Windows Form Designer generated code "
		public frmSelect() : base()
		{
			Closing += frmSelect_Closing;
			Load += frmSelect_Load;

			//This call is required by the Windows Form Designer.
			InitializeComponent();
			//Add any initialization after the InitializeComponent() call
		}
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
		private System.Windows.Forms.Button withEventsField_btnApply;
		protected internal System.Windows.Forms.Button btnApply {
			get { return withEventsField_btnApply; }
			set {
				if (withEventsField_btnApply != null) {
					withEventsField_btnApply.Click -= btnApply_Click;
				}
				withEventsField_btnApply = value;
				if (withEventsField_btnApply != null) {
					withEventsField_btnApply.Click += btnApply_Click;
				}
			}
		}
		private System.Windows.Forms.Button withEventsField_btnClear;
		protected internal System.Windows.Forms.Button btnClear {
			get { return withEventsField_btnClear; }
			set {
				if (withEventsField_btnClear != null) {
					withEventsField_btnClear.Click -= btnClear_Click;
				}
				withEventsField_btnClear = value;
				if (withEventsField_btnClear != null) {
					withEventsField_btnClear.Click += btnClear_Click;
				}
			}
		}
		private System.Windows.Forms.Button withEventsField_btnCancel;
		protected internal System.Windows.Forms.Button btnCancel {
			get { return withEventsField_btnCancel; }
			set {
				if (withEventsField_btnCancel != null) {
					withEventsField_btnCancel.Click -= btnCancel_Click;
				}
				withEventsField_btnCancel = value;
				if (withEventsField_btnCancel != null) {
					withEventsField_btnCancel.Click += btnCancel_Click;
				}
			}
		}
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.btnApply = new System.Windows.Forms.Button();
			this.btnClear = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			//
			//btnApply
			//
			this.btnApply.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnApply.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnApply.Location = new System.Drawing.Point(85, 8);
			this.btnApply.Name = "btnApply";
			this.btnApply.Size = new System.Drawing.Size(75, 25);
			this.btnApply.TabIndex = 19;
			this.btnApply.Text = "&Apply";
			//
			//btnClear
			//
			this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnClear.Location = new System.Drawing.Point(169, 8);
			this.btnClear.Name = "btnClear";
			this.btnClear.Size = new System.Drawing.Size(75, 25);
			this.btnClear.TabIndex = 20;
			this.btnClear.Text = "&Clear";
			//
			//btnCancel
			//
			this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnCancel.CausesValidation = false;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(253, 8);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 25);
			this.btnCancel.TabIndex = 21;
			this.btnCancel.Text = "Cancel";
			//
			//frmSelect
			//
			this.AcceptButton = this.btnApply;
			this.AutoScaleBaseSize = new System.Drawing.Size(7, 15);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(412, 42);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnClear);
			this.Controls.Add(this.btnApply);
			this.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmSelect";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "frmSelect";
			this.ResumeLayout(false);

		}
		#endregion
		#region "Properties"
		#region "Declarations"
		const int stdHeight = 24;
		const int stdSpacer = 4;
		const int stdTab = 4 * stdSpacer;
		private int iControlLines = 0;
		private int iTab = 0;
		private bool mOKtoClose = false;
			#endregion
		private string mRegistryKey;
		#endregion
		#region "Methods"
		private void Clear(bool Closing = false)
		{
			foreach (Control ctl in this.Controls) {
				switch (Information.TypeName(ctl)) {
					case "CheckBox":
						((CheckBox)ctl).CheckState = CheckState.Indeterminate;
						break;
					case "ComboBox":
						//Don't know why, but simply setting SelectedIndex to -1 isn't always reliable...
						var _with1 = (ComboBox)ctl;
						//If Closing Then .DataSource = Nothing : .DisplayMember = Nothing
						_with1.SelectedIndex = -1;
						if (_with1.Text != bpeNullString)
							_with1.Text = bpeNullString;
						break;
					case "DateTimePicker":
						((DateTimePicker)ctl).Value = new System.DateTime(System.DateTime.Today.Year, System.DateTime.Today.Month, System.DateTime.Today.Day, 0, 0, 0, 0);
						((DateTimePicker)ctl).Checked = false;
						break;
					case "TextBox":
						((TextBox)ctl).Text = bpeNullString;
						break;
				}
			}
		}
		private string DetermineOperator(string strData, string FieldName)
		{
			if (strData.StartsWith("="))
				return "=";
			if (strData.StartsWith("<>") || strData.StartsWith("!="))
				return "<>";
			if (strData.StartsWith(">="))
				return ">=";
			if (strData.StartsWith(">"))
				return ">";
			if (strData.StartsWith("<="))
				return "<=";
			if (strData.StartsWith("<"))
				return "<";
			if (strData.ToUpper().StartsWith("LIKE "))
				return "Like";
			if (strData.StartsWith("%") || strData.EndsWith("%"))
				return "Like";
			if (strData.ToUpper().EndsWith(" IS NOT NULL"))
				return "Is Not Null";
			if (strData.ToUpper().EndsWith(" IS NULL"))
				return "Is Null";
			if (strData.ToUpper().StartsWith(FieldName.ToUpper()))
				return FieldName;
			return bpeNullString;
		}
		private void DetermineOperator(string strData, ref string Operator, ref string Operand)
		{
			Operator = "=";
			Operand = strData;
			if (strData.StartsWith("=")) {
				Operator = "=";
				Operand = strData.Substring(Operator.Length).Trim();
			} else if (strData.StartsWith("<>") || strData.StartsWith("!=")) {
				Operator = "<>";
				Operand = strData.Substring(Operator.Length).Trim();
			} else if (strData.StartsWith("<=")) {
				Operator = "<=";
				Operand = strData.Substring(Operator.Length).Trim();
			} else if (strData.StartsWith(">=")) {
				Operator = ">=";
				Operand = strData.Substring(Operator.Length).Trim();
			} else if (strData.StartsWith("<")) {
				Operator = "<";
				Operand = strData.Substring(Operator.Length).Trim();
			} else if (strData.StartsWith(">")) {
				Operator = ">";
				Operand = strData.Substring(Operator.Length).Trim();
			} else if (strData.ToUpper().StartsWith("NOT LIKE ")) {
				Operator = "Not Like";
				Operand = strData.Substring(Operator.Length).Trim().ToUpper();
			} else if (strData.ToUpper().StartsWith("LIKE ")) {
				Operator = "Like";
				Operand = strData.Substring(Operator.Length).Trim().ToUpper();
			} else if (strData.StartsWith("%") || strData.EndsWith("%")) {
				Operator = "Like";
				Operand = strData.Trim().ToUpper();
			} else if (strData.ToUpper().EndsWith(" IS NOT NULL")) {
				Operator = "Is Not Null";
				Operand = strData.Substring(0, strData.Length - Operator.Length).Trim();
			} else if (strData.ToUpper().EndsWith(" IS NULL")) {
				Operator = "Is Null";
				Operand = strData.Substring(0, strData.Length - Operator.Length).Trim();
			}
		}
		//Private Function FindBindingByControl(ByVal ctl As Control) As Binding
		//        Select Case TypeName(ctl)
		//            Case "ComboBox"
		//                FindBindingByControl = ctl.DataBindings("Text") 'Simple-Bound
		//                If IsNothing(FindBindingByControl) Then FindBindingByControl = ctl.DataBindings("SelectedValue") 'Complex-Bound
		//            Case "CheckBox"
		//                FindBindingByControl = ctl.DataBindings("Checked")
		//            Case "DateTimePicker"
		//                FindBindingByControl = ctl.DataBindings("Value")
		//            Case "TextBox"
		//                FindBindingByControl = ctl.DataBindings("Text")
		//        End Select
		//End Function
		private Binding FindBindingByControlName(string controlName)
		{
			Binding functionReturnValue = null;
			functionReturnValue = null;
			foreach (Binding iBinding in mTCBase.CurrencyManager.Bindings) {
				if (iBinding.Control.Name == controlName){functionReturnValue = iBinding;return functionReturnValue;}
			}
			return functionReturnValue;
		}
		private Control FindControlByName(string ControlName)
		{
			Control functionReturnValue = null;
			functionReturnValue = null;
			foreach (Control ctl in this.Controls) {
				if (ctl.Name == ControlName){functionReturnValue = ctl;return functionReturnValue;}
			}
			return functionReturnValue;
		}
		private Control FindControlByTag(string Tag)
		{
			Control functionReturnValue = null;
			functionReturnValue = null;
			foreach (Control ctl in this.Controls) {
				if ((ctl.Tag != null)) {
					string testTag = (string)ctl.Tag;
					if (testTag.IndexOf(";") >= 0 && testTag.Split(";".ToCharArray())[1] == "Boolean")
						testTag = testTag.Split(";".ToCharArray())[0];
					if (testTag.ToUpper() == Tag.ToUpper()){functionReturnValue = ctl;return functionReturnValue;}
				}
			}
			return functionReturnValue;
		}
		private string FindLabel(Binding iBinding)
		{
			string functionReturnValue = null;
			functionReturnValue = bpeNullString;
			functionReturnValue = bpeNullString;
			int LowY = iBinding.Control.Location.Y - (2 * stdSpacer);
			int HighY = iBinding.Control.Location.Y + (2 * stdSpacer);
			int LowX = iBinding.Control.Location.X - (4 * stdSpacer);
			int HighX = iBinding.Control.Location.X + (4 * stdSpacer);
			foreach (Control iControl in iBinding.Control.Parent.Controls) {
				if (Information.TypeName(iControl) == "Label" && iControl.Location.Y >= LowY && iControl.Location.Y <= HighY & iControl.Location.X >= LowX && iControl.Location.X <= HighX) {
					functionReturnValue = iControl.Text;
				}
			}
			if (functionReturnValue == bpeNullString)
				functionReturnValue = iBinding.BindingMemberInfo.BindingMember;
			return functionReturnValue;
		}
		private void NewControl(Binding iBinding)
		{
			const string EntryName = "NewControl";
			Control ctl = iBinding.Control;
			string strTag = (ctl.Tag == null ? "" : Convert.ToString(ctl.Tag));
			string[] tagParams = strTag.Split(",".ToCharArray());
			for (int i = 0; i <= tagParams.Length - 1; i++) {
				switch (tagParams[i].ToUpper()) {
					case "IGNORE":
						return;

						break;
				}
			}

			switch (Information.TypeName(ctl)) {
				case "CheckBox":
					ctl = PlaceNewControl(ctl, FindLabel(iBinding));
					ctl.Tag = iBinding.BindingMemberInfo.BindingMember;
					break;
				case "ComboBox":
					System.Windows.Forms.ComboBox mcb = (System.Windows.Forms.ComboBox)ctl;
					ctl = PlaceNewControl(ctl, FindLabel(iBinding));
					ctl.Tag = iBinding.BindingMemberInfo.BindingMember;
					//OK, we have a new control. If the original control was simple-bound (like an AlphaSort field) where
					//no underlying DataView exists, PlaceNewControl would have returned us a TextBox. So if the original
					//control was complex-bound, we should have a ComboBox. If that's the case, we need to bind it to the
					//same underlying DataView as the original control...
					if (Information.TypeName(ctl) == "ComboBox") {
						var _with2 = (ComboBox)ctl;
						_with2.DataSource = mcb.DataSource;
						_with2.DisplayMember = mcb.DisplayMember;
						_with2.ValueMember = mcb.ValueMember;
						_with2.SelectedIndex = -1;
					}
					break;
				case "DateTimePicker":
					ctl = PlaceNewControl(ctl, FindLabel(iBinding));
					ctl.Tag = iBinding.BindingMemberInfo.BindingMember;
					break;
				case "Label":
					return;

					break;
				case "PictureBox":
					ctl = PlaceNewControl(ctl, FindLabel(iBinding));
					ctl.Tag = iBinding.BindingMemberInfo.BindingMember + ";Boolean";
					break;
				case "RichTextBox":
					return;

					break;
				case "TextBox":
					ctl = PlaceNewControl(ctl, FindLabel(iBinding));
					ctl.Tag = iBinding.BindingMemberInfo.BindingMember;
					break;
				default:
					throw new Exception(string.Format("Unexpected control type ({0}) encountered in {1}(). Control: {2}", Information.TypeName(ctl), EntryName, ctl.Name));
			}
			iControlLines += 1;
		}
		private Label NewLabel(Control ctl, string strLabel, Font font, AnchorStyles anchorStyle)
		{
			Label functionReturnValue = null;
			functionReturnValue = null;
			Label lbl = new Label();
			switch (Information.TypeName(ctl)) {
				case "CheckBox":
					lbl.Name = "lbl" + ctl.Name.Substring("CHK".Length);
					break;
				case "ComboBox":
					lbl.Name = "lbl" + ctl.Name.Substring("CB".Length);
					break;
				case "DateTimePicker":
					lbl.Name = "lbl" + ctl.Name.Substring("DTP".Length);
					break;
				case "TextBox":
					lbl.Name = "lbl" + ctl.Name.Substring("TXT".Length);
					break;
				default:
					break;
			}
			lbl.Font = new Font(font, font.Style);
			lbl.Text = strLabel;
			Graphics g = lbl.CreateGraphics();
			lbl.Size = new System.Drawing.Size(Convert.ToInt32(Math.Ceiling(g.MeasureString(strLabel, lbl.Font).Width)), stdHeight);
			lbl.TextAlign = ContentAlignment.MiddleLeft;
			lbl.Anchor = anchorStyle;
			this.Controls.Add(lbl);
			functionReturnValue = lbl;
			return functionReturnValue;
		}
		private void ParseFilter(string strFilter)
		{
			string Operator = bpeNullString;
			string Operand = bpeNullString;
			string FieldName = bpeNullString;
			for (int i = 1; i <= TokenCount(strFilter.ToUpper(), " AND "); i++) {
				string Token = ParseStr(strFilter, i, " And ", "\"");
				int FirstQuotePos = Strings.InStr(Token, "'");
				string TestToken = Token;
				if (FirstQuotePos > 0)
					TestToken = Strings.Mid(Token, 1, FirstQuotePos);

				Operator = bpeNullString;
				if (TestToken.ToUpper().IndexOf("NOT LIKE ") >= 0) {
					int iPos = TestToken.ToUpper().IndexOf("NOT LIKE ");
					Operator = TestToken.Substring(iPos, "NOT LIKE".Length);
				} else if (TestToken.ToUpper().IndexOf("LIKE ") >= 0) {
					int iPos = TestToken.ToUpper().IndexOf("LIKE ");
					Operator = TestToken.Substring(iPos, "LIKE".Length);
				} else if (TestToken.ToUpper().IndexOf("IS NOT NULL") >= 0) {
					int iPos = TestToken.ToUpper().IndexOf("IS NOT NULL");
					Operator = TestToken.Substring(iPos, "IS".Length);
				} else if (TestToken.ToUpper().IndexOf("IS NULL") >= 0) {
					int iPos = TestToken.ToUpper().IndexOf("IS NULL");
					Operator = TestToken.Substring(iPos, "IS".Length);
				} else if (TestToken.IndexOf("!=") >= 0) {
					Operator = "!=";
				} else if (TestToken.IndexOf("<>") >= 0) {
					Operator = "<>";
				} else if (TestToken.IndexOf(">=") >= 0) {
					Operator = ">=";
				} else if (TestToken.IndexOf(">") >= 0) {
					Operator = ">";
				} else if (TestToken.IndexOf("<=") >= 0) {
					Operator = "<=";
				} else if (TestToken.IndexOf("<") >= 0) {
					Operator = "<";
				} else if (TestToken.IndexOf("=") >= 0) {
					Operator = "=";
				}
				if (Operator != bpeNullString) {
					FieldName = ParseStr(Token, 1, Operator, "'").Trim();
					if (FirstQuotePos > 0) {
						Operand = Strings.Mid(Token, FirstQuotePos + 1, Strings.Len(Token) - FirstQuotePos - 1);
					} else {
						Operand = Strings.Mid(Token, Strings.InStr(Token, Operator) + Operator.Length).Trim();
					}
					if (Operator.ToUpper().Contains("LIKE"))
						Operand = Operand.ToUpper();
				}

				Operand = mTCBase.StripBraces(Operand, "'").Replace("''", "'");
				Control ctl = FindControlByTag(FieldName);
				switch (Information.TypeName(ctl)) {
					case "CheckBox":
						var _with3 = (CheckBox)ctl;
						if (Operator.ToUpper() == "IS") {
							switch (Operand.ToUpper()) {
								case "NULL":
									_with3.CheckState = CheckState.Unchecked;
									break;
								case "NOT NULL":
									_with3.CheckState = CheckState.Checked;
									break;
								default:
									_with3.CheckState = CheckState.Indeterminate;
									break;
							}
						} else {
							switch (Conversion.Val(Operand)) {
								case 0:
									_with3.CheckState = CheckState.Unchecked;
									break;
								case 1:
									_with3.CheckState = CheckState.Checked;
									break;
								default:
									_with3.CheckState = CheckState.Indeterminate;
									break;
							}
						}
						break;
					case "ComboBox":
					case "TextBox":
						if (Operand != bpeNullString) {
							Binding iBinding = FindBindingByControlName(ctl.Name);
							DataView dv = (DataView)iBinding.DataSource;
							switch (dv.Table.Columns[iBinding.BindingMemberInfo.BindingMember].DataType.Name) {
								case "String":
									//Operator will really never be bpeNullString 'cause we should have already defaulted it to "="
									if (Operator == bpeNullString) {
										if (!Operand.EndsWith("%"))
											Operand = Operand + "%";
										//Wildcard everything...
										if (Operand.EndsWith("%"))
											Operator = "Like";
										//...but don't overwrite our Operator
									}
									Operand = "'" + FixQuotes(Operand) + "'";
									break;
								case "Date":
									Operand = "#" + Operand + "#";
									break;
								default:
									Operand = FixQuotes(Operand);
									break;
							}
						}
						ctl.Text = Operator + " " + Operand;
						break;
					case "DateTimePicker":
						DateTimePicker dtp = null;
						switch (Operator) {
							case ">=":
								dtp = (DateTimePicker)ctl;
								break;
							case "<=":
								dtp = (DateTimePicker)FindControlByName(ctl.Name.Substring(0, ctl.Name.Length - "From".Length) + "Through");
								break;
						}
						dtp.Checked = true;
                        dtp.Text = Operand; //TODO: was dtp.Value = Operand;
                        break;
				}
			}
		}
		private Control PlaceNewControl(Control masterControl, string strLabel)
		{
			Control functionReturnValue = null;
			Control newControl = null;
			functionReturnValue = null;
			switch (Information.TypeName(masterControl)) {
				case "CheckBox":
					newControl = new CheckBox();
					break;
				case "ComboBox":
					if (((System.Windows.Forms.ComboBox)masterControl).DisplayMember == bpeNullString) {
						//If the ComboBox is simple-bound, then collect any Select information via a TextBox...
						newControl = new TextBox();
						//I'd prefer to get casing from the ComboBox, but it doesn't seem to support it...
						((TextBox)newControl).CharacterCasing = CharacterCasing.Normal;
					} else {
						newControl = new System.Windows.Forms.ComboBox();
					}
					break;
				case "DateTimePicker":
					newControl = new DateTimePicker();
					break;
				case "PictureBox":
					newControl = new CheckBox();
					break;
				case "TextBox":
					newControl = new TextBox();
					((TextBox)newControl).CharacterCasing = ((TextBox)masterControl).CharacterCasing;
					break;
				default:
					break;
			}

			Label lbl = null;
            newControl.Font = masterControl.Font;
            newControl.Name = masterControl.Name;
			if (Information.TypeName(newControl) != "CheckBox") {
				lbl = NewLabel(newControl, strLabel, newControl.Font, AnchorStyles.Left);
				lbl.Location = new System.Drawing.Point(stdSpacer, stdSpacer + (iControlLines * (stdHeight + stdSpacer)));
				//Align to a standard tab-stop-like concept...
				int newX = lbl.Location.X + lbl.Size.Width + stdSpacer;
				int adjustment = newX % stdTab;
				//Debug.Write(.Name & ": newX: " & newX)
				if (adjustment != 0)
					newX = newX + (stdTab - adjustment);
                //Debug.WriteLine("; Adjusted newX: " & newX)
                newControl.Location = new System.Drawing.Point(newX, stdSpacer + (iControlLines * (stdHeight + stdSpacer)));
				if ((masterControl.Anchor & AnchorStyles.Right) == AnchorStyles.Right) {
                    newControl.Size = new System.Drawing.Size((this.ClientSize.Width - newControl.Location.X - (2 * stdSpacer)), stdHeight);
                    newControl.Anchor = (AnchorStyles)(AnchorStyles.Left | AnchorStyles.Right);
				} else {
                    newControl.Size = new System.Drawing.Size(masterControl.Width, stdHeight);
                    newControl.Anchor = AnchorStyles.Left;
				}
			} else if (Information.TypeName(masterControl) == "CheckBox") {
                newControl.Text = ((CheckBox)masterControl).Text;
                newControl.Location = new System.Drawing.Point(stdSpacer, stdSpacer + (iControlLines * (stdHeight + stdSpacer)));
                newControl.Size = new System.Drawing.Size((this.ClientSize.Width - newControl.Location.X - stdSpacer), stdHeight);
                newControl.Anchor = (AnchorStyles)(AnchorStyles.Left | AnchorStyles.Right);
			//TypeName(masterControl) = "PictureBox" Then
			} else {
                newControl.Text = strLabel;
                newControl.Location = new System.Drawing.Point(stdSpacer, stdSpacer + (iControlLines * (stdHeight + stdSpacer)));
                newControl.Size = new System.Drawing.Size((this.ClientSize.Width - newControl.Location.X - stdSpacer), stdHeight);
                newControl.Anchor = (AnchorStyles)(AnchorStyles.Left | AnchorStyles.Right);
			}
            newControl.TabIndex = iTab;
			iTab += 1;

			//Set Control-Specific properties...
			switch (Information.TypeName(newControl)) {
				case "CheckBox":
					((CheckBox)newControl).TextAlign = ContentAlignment.MiddleLeft;
					((CheckBox)newControl).ThreeState = true;
					((CheckBox)newControl).CheckState = CheckState.Indeterminate;
					((CheckBox)newControl).CheckStateChanged += CheckStateChanged;
					((CheckBox)newControl).Click += CheckBoxClick;
					this.Controls.Add(newControl);
					functionReturnValue = newControl;
					break;
				case "ComboBox":
					((System.Windows.Forms.ComboBox)newControl).KeyPress += AutoComplete_KeyPress;
					((System.Windows.Forms.ComboBox)newControl).KeyDown += AutoComplete_KeyDown;
					((System.Windows.Forms.ComboBox)newControl).Validating += Control_Validating;
					((System.Windows.Forms.ComboBox)newControl).Enter += Control_Enter;
					this.Controls.Add(newControl);
					functionReturnValue = newControl;
					break;
				case "DateTimePicker":
                    newControl.Name = masterControl.Name + "From";
                    ((DateTimePicker)newControl).Size = new Size(newControl.Size.Width + 2, newControl.Size.Height);
                    ((DateTimePicker)newControl).Format = System.Windows.Forms.DateTimePickerFormat.Short;
					//((DateTimePicker)newControl).MinDate = new System.DateTime(1963, 7, 31, 0, 0, 0, 0);
					((DateTimePicker)newControl).Value = new System.DateTime(System.DateTime.Today.Year, System.DateTime.Today.Month, System.DateTime.Today.Day, 0, 0, 0, 0);
					((DateTimePicker)newControl).ShowUpDown = true;
					((DateTimePicker)newControl).ShowCheckBox = true;
					((DateTimePicker)newControl).Checked = false;
					((DateTimePicker)newControl).KeyUp += Control_KeyUp;
					((DateTimePicker)newControl).KeyUp += Control_KeyUp;
					this.Controls.Add(newControl);
					functionReturnValue = newControl;

					DateTimePicker dtpFrom = (DateTimePicker)newControl;
					newControl = new DateTimePicker();
					newControl.Name = masterControl.Name + "Through";
					lbl = NewLabel(newControl, "Through", lbl.Font, AnchorStyles.Left);
					lbl.Location = new System.Drawing.Point(dtpFrom.Location.X + dtpFrom.Size.Width + stdSpacer, stdSpacer + (iControlLines * (stdHeight + stdSpacer)));
					newControl.Location = new System.Drawing.Point(lbl.Location.X + lbl.Size.Width + stdSpacer, stdSpacer + (iControlLines * (stdHeight + stdSpacer)));
					if ((masterControl.Anchor & AnchorStyles.Right) == AnchorStyles.Right) {
						newControl.Size = new System.Drawing.Size((this.ClientSize.Width - newControl.Location.X - (2 * stdSpacer)), stdHeight);
					} else {
						newControl.Size = new System.Drawing.Size(masterControl.Width, stdHeight);
					}

					((DateTimePicker)newControl).Format = System.Windows.Forms.DateTimePickerFormat.Short;
					//((DateTimePicker)newControl).MinDate = new System.DateTime(1963, 7, 31, 0, 0, 0, 0);
					((DateTimePicker)newControl).Value = new System.DateTime(System.DateTime.Today.Year, System.DateTime.Today.Month, System.DateTime.Today.Day, 0, 0, 0, 0);
					((DateTimePicker)newControl).ShowUpDown = true;
					((DateTimePicker)newControl).ShowCheckBox = true;
					((DateTimePicker)newControl).Checked = false;
					((DateTimePicker)newControl).KeyUp += Control_KeyUp;
					((DateTimePicker)newControl).KeyUp += Control_KeyUp;
					this.Controls.Add(newControl);
					break;
				case "Label":
					break;
				case "TextBox":
					//CType(newControl, TextBox).CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
					((TextBox)newControl).Text = bpeNullString;
					((TextBox)newControl).KeyDown += Control_KeyDown;
					((TextBox)newControl).KeyPress += Control_KeyPress;
					((TextBox)newControl).Validating += Control_Validating;
					((TextBox)newControl).Enter += Control_Enter;
					this.Controls.Add(newControl);
					functionReturnValue = newControl;
					break;
				default:
					break;
			}
			int newWidth = newControl.Location.X + newControl.Size.Width + stdSpacer;
			if (this.ClientSize.Width < newWidth) {
				int adjustment = (newWidth - this.ClientSize.Width) / 2;
				this.ClientSize = new Size(newWidth, this.ClientSize.Height);
				this.btnApply.Location = new Point(this.btnApply.Location.X + adjustment, this.btnApply.Location.Y);
				this.btnClear.Location = new Point(this.btnClear.Location.X + adjustment, this.btnClear.Location.Y);
				this.btnCancel.Location = new Point(this.btnCancel.Location.X + adjustment, this.btnCancel.Location.Y);
			}
			return functionReturnValue;
		}
		private string UpdateSQL(string data, string memberName)
		{
			string functionReturnValue = null;
			string SQLSource = bpeNullString;
			try {
				if ((data == null))
                    throw new ExitTryException(); 
				if (data.Trim() == bpeNullString)
                    throw new ExitTryException(); 
				string Operator = DetermineOperator(data, memberName);
				if (Operator == "Like" && data.Contains("[") && !data.Contains("[[]"))
					data = data.Replace("[", "[[]");

				//Aren't we jumping through hoops to make sure the Validating event formats these fields to be ready-to-use?
				//Dim Operator As String = DetermineOperator(data, memberName)
				//'if the control already contains the field name, then assume it needs no formatting here...
				//If Operator = memberName Then
				//    SQLSource = data & " And "
				//ElseIf Operator = bpeNullString Then
				//    SQLSource = memberName & "='" & FixQuotes(data).Trim & "' And "
				//ElseIf Operator = "Like" Then
				//    If data.ToUpper.Trim.StartsWith("LIKE") Then
				//        SQLSource = memberName & " " & Operator & " '" & FixQuotes(mTCBase.StripBraces(data.Substring("Like".Length).Trim, "'")).Trim & "' And "
				//    Else    'implicit...
				//        SQLSource = memberName & " " & Operator & " '" & FixQuotes(data) & "' And "
				//    End If
				//Else
				//    'Assume FixQuotes has already been applied (as it would from TextBox_Validating)...
				//    SQLSource = memberName & Operator & ParseStr(data, 2, Operator).Trim & " And "
				//End If

				if (data.ToUpper().StartsWith(memberName.ToUpper())) {
					SQLSource = string.Format("{0} And ", data);
				} else {
					SQLSource = string.Format("{0} {1} And ", memberName, data);
				}
            } catch (ExitTryException) { 
            } finally {
				functionReturnValue = SQLSource;
			}
			return functionReturnValue;
		}
		#endregion
		#region "Event Handlers"
		private void btnApply_Click(object sender, System.EventArgs e)
		{
			string Operator = bpeNullString;
			string SQLSource = bpeNullString;
			DataView dvFilter = null;
			try {
				Operator = "=";
				foreach (Control ctl in this.Controls) {
					switch (Information.TypeName(ctl)) {
						case "CheckBox":
						case "ComboBox":
						case "DateTimePicker":
						case "TextBox":
							string memberName = Convert.ToString(ctl.Tag);
							switch (Information.TypeName(ctl)) {
								case "CheckBox":
									string CheckedTest = "=1";
									string UncheckedTest = "=0";
									//We have a PictureBox...
									if (memberName.IndexOf(";") >= 0 && memberName.Split(";".ToCharArray())[1] == "Boolean") {
										CheckedTest = " Is Not Null";
										UncheckedTest = " Is Null";
										memberName = memberName.Split(";".ToCharArray())[0];
									}
									switch (((CheckBox)ctl).CheckState) {
										case CheckState.Checked:
											SQLSource += string.Format("{0}{1} And ", memberName, CheckedTest);
											break;
										case CheckState.Indeterminate:
											break;
										case CheckState.Unchecked:
											SQLSource += string.Format("{0}{1} And ", memberName, UncheckedTest);
											break;
									}
									break;
								case "ComboBox":
									//'Dim iBinding As Binding = FindBindingByControl(ctl)
									//'Dim memberName As String = iBinding.BindingMemberInfo.BindingMember
									//SQLSource &= UpdateSQL(CType(ctl, Windows.Forms.ComboBox).ValueMember, memberName)
									bool cancel = false;
									Control_Validating(ctl, new CancelEventArgs(cancel));
									if (((System.Windows.Forms.ComboBox)ctl).Text != bpeNullString) {
										if ((((System.Windows.Forms.ComboBox)ctl).SelectedValue == null)) {
											SQLSource += UpdateSQL(((System.Windows.Forms.ComboBox)ctl).Text, memberName);
										} else {
											//I hate these fucking ComboBox controls... Control_Validating should have prepared our text such that
											//it would be ready to go, and in doing so shouldn't match anything in the list (i.e. SelectedValue s/b
											//Nothing and we shouldn't be here)... Since that didn't happen for some unknown/misunderstood reason,
											//fix it here...
											SQLSource += UpdateSQL(string.Format("= '{0}'", FixQuotes(((System.Windows.Forms.ComboBox)ctl).SelectedValue)), memberName);
										}
									}
									break;
								case "DateTimePicker":
									if (ctl.Name.EndsWith("From")) {
										DateTimePicker dtpFromDate = (DateTimePicker)ctl;
										DateTimePicker dtpThroughDate = (DateTimePicker)FindControlByName(ctl.Name.Substring(0, ctl.Name.Length - "From".Length) + "Through");
										if (dtpFromDate.Checked)
											SQLSource += string.Format("{0}>='{1:yyyy-MM-dd} 00:00:00' And ", memberName, dtpFromDate.Value);
										if (dtpThroughDate.Checked)
											SQLSource += string.Format("{0}<='{1:yyyy-MM-dd} 23:59:59' And ", memberName, dtpThroughDate.Value);
									}
									break;
								case "TextBox":
									bool cancel2 = false;
									Control_Validating(ctl, new CancelEventArgs(cancel2));
									if (((TextBox)ctl).Text != bpeNullString)
										SQLSource += UpdateSQL(ctl.Text, memberName);
									break;
							}
							break;
						default:
							break;
					}
				}

				if (SQLSource.Length > 0)
					SQLSource = SQLSource.Substring(0, SQLSource.Length - " And ".Length);
				//Get rid of the final " And "...
				dvFilter = new DataView(mTCBase.MainDataView.Table, SQLSource, mTCBase.MainDataView.Sort, DataViewRowState.CurrentRows);
				if (dvFilter.Count == 0) {
					mOKtoClose = false;
					MessageBox.Show("Zero records returned - please adjust your selection.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
				} else {
					mTCBase.SQLFilter = SQLSource;
					mOKtoClose = true;
				}
			} catch (Exception ex) {
				mTCBase.GenericErrorHandler(ex, true);
			} finally {
				dvFilter.Dispose();
				dvFilter = null;
			}
		}
		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			try {
				base.epBase.SetError((Control)sender, bpeNullString);
				mOKtoClose = true;
			} catch (Exception ex) {
				mTCBase.GenericErrorHandler(ex, true);
			}
		}
		private void btnClear_Click(object sender, System.EventArgs e)
		{
			try {
				base.epBase.SetError((Control)sender, bpeNullString);
				Clear(true);
				mTCBase.SQLFilter = bpeNullString;
			} catch (Exception ex) {
				mTCBase.GenericErrorHandler(ex, true);
			}
		}
		private void CheckBoxClick(object sender, EventArgs e)
		{
			try {
				base.epBase.SetError((Control)sender, bpeNullString);
				//Windows' progression is from Indeterminate, goes Unchecked, then Checked
				//Note: Windows will have already changed the value before this event is fired.
				CheckBox chk = (CheckBox)sender;
				//Prior state would have been Indeterminate
				if (chk.CheckState == CheckState.Unchecked) {
					chk.CheckState = CheckState.Checked;
				//Prior state would have been Checked
				} else if (chk.CheckState == CheckState.Indeterminate) {
					chk.CheckState = CheckState.Unchecked;
				//Prior state would have been Unchecked
				} else if (chk.CheckState == CheckState.Checked) {
					chk.CheckState = CheckState.Indeterminate;
				}
			} catch (Exception ex) {
				base.epBase.SetError((Control)sender, ex.Message);
			}
		}
		private void CheckStateChanged(object sender, EventArgs e)
		{
			try {
				base.epBase.SetError((Control)sender, bpeNullString);
			} catch (Exception ex) {
				base.epBase.SetError((Control)sender, ex.Message);
			}
		}
		private void Control_Enter(object sender, System.EventArgs e)
		{
			string strText = bpeNullString;
			try {
				base.epBase.SetError((Control)sender, bpeNullString);

				//Decode our entry as best we can to reset it back to what the user first entered before we "SQLized" it...
				strText = ((Control)sender).Text.Trim();
				((Control)sender).Text = strText;
				if (strText == bpeNullString)
                    throw new ExitTryException();
				if (strText.StartsWith("=")) {
					strText = strText.Substring(1).Trim();
					if (strText.StartsWith("'")) {
						strText = mTCBase.StripBraces(strText, "'").Replace("''", "'");
					} else if (strText.StartsWith("#")) {
						strText = strText.Substring(1, strText.Length - 2);
					}
				}
				((Control)sender).Text = strText;
				switch (Information.TypeName(sender)) {
					case "ComboBox":
						((System.Windows.Forms.ComboBox)sender).SelectAll();
						break;
					case "TextBox":
						((TextBox)sender).SelectAll();
						break;
				}
            } catch (ExitTryException) { 
            } catch (Exception ex) {
				base.epBase.SetError((Control)sender, ex.Message);
			}
		}
		private void Control_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			string Operand = bpeNullString;
			string Operator = bpeNullString;
			string strText = bpeNullString;
			try {
				base.epBase.SetError((Control)sender, bpeNullString);

				strText = ((Control)sender).Text.Trim();
				((Control)sender).Text = strText;
				if (strText == bpeNullString)
                    throw new ExitTryException();

				DetermineOperator(strText, ref Operator, ref Operand);
				if (Operand != bpeNullString) {
					if (Operand.StartsWith("'") || Operand.StartsWith("#"))
						Operand = Operand.Substring(1);
					if (Operand.EndsWith("'") || Operand.EndsWith("#"))
						Operand = Operand.Substring(0, Operand.Length - 1);
					Binding iBinding = FindBindingByControlName(((Control)sender).Name);
					DataView dv = (DataView)iBinding.DataSource;
					switch (dv.Table.Columns[iBinding.BindingMemberInfo.BindingMember].DataType.Name) {
						case "String":
							//Operator will really never be bpeNullString 'cause we should have already defaulted it to "="
							if (Operator == bpeNullString) {
								if (!Operand.EndsWith("%"))
									Operand = Operand + "%";
								//Wildcard everything...
								if (Operand.EndsWith("%"))
									Operator = "Like";
								//...but don't overwrite our Operator
							}
							Operand = "'" + FixQuotes(Operand) + "'";
							break;
						case "Date":
							Operand = "#" + Operand + "#";
							break;
						default:
							Operand = FixQuotes(Operand);
							break;
					}
				}
				((Control)sender).Text = Operator + " " + Operand;
            } catch (ExitTryException) { 
            } catch (Exception ex) {
				base.epBase.SetError((Control)sender, ex.Message);
			}
		}
		private void frmSelect_Load(object sender, System.EventArgs e)
		{
			try {
				int iLeft = (int)GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mRegistryKey, "Form Left", this.Left);
				int iTop = (int)GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mRegistryKey, "Form Top", this.Top);
				int iWidth = (int)GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mRegistryKey, "Form Width", this.Width);
				int iHeight = (int)GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mRegistryKey, "Form Height", this.Height);
				this.SetBounds(iLeft, iTop, iWidth, iHeight);

				Clear();
				mTCBase.SQLFilter = (string)GetRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mRegistryKey, "SQLFilter", bpeNullString);
				if (mTCBase.SQLFilter != bpeNullString)
					ParseFilter(mTCBase.SQLFilter);

				if ((this.ActiveControl != null)) {
					switch (Information.TypeName(this.ActiveControl)) {
						case "ComboBox":
							((System.Windows.Forms.ComboBox)this.ActiveControl).SelectAll();
							break;
						case "TextBox":
							((TextBox)this.ActiveControl).SelectAll();
							break;
					}
				}
			} catch (Exception ex) {
				mTCBase.GenericErrorHandler(ex, true);
			}
		}
		private void frmSelect_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!mOKtoClose){e.Cancel = true;return;
}
			mTCBase.SaveBounds(mRegistryKey, this.Left, this.Top, this.Width, this.Height);
			SaveRegistrySetting(RootKeyConstants.HKEY_CURRENT_USER, mRegistryKey, "SQLFilter", ((mTCBase.SQLFilter == null) ? bpeNullString : mTCBase.SQLFilter));
		}
		#endregion
	}
}

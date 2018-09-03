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
//clsIconMenuItem.vb
//   TreasureChest2 IconMenuItem Class Definition ...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   11/28/11    Corrected Text clipping problem by adjusting the measured string width by the space used to draw the icon as this 
//               was shifting the string (i.e. menu caption) to the right by that width;
//   06/16/11    Corrected menu backgrounds;
//   06/04/11    Added Font and Icon properties for public access;
//   05/08/06    Created;
//=================================================================================================================================
//Notes to Self:
//=================================================================================================================================
namespace TCBase
{
	public class clsIconMenuItem : MenuItem
	{
		public clsIconMenuItem() : base()
		{
		}
		public clsIconMenuItem(string Text, string FontFace, float FontSize, Icon Icon, EventHandler onClick = null, Shortcut Shortcut = Shortcut.None) : base(Text, onClick, Shortcut)
		{
			// Owner Draw Property allows you to modify the menu item by handling OnMeasureItem and OnDrawItem
			OwnerDraw = true;
			mFont = new Font(FontFace, FontSize);
			mIcon = Icon;
		}
		public clsIconMenuItem(string Text, string FontFace, float FontSize, System.Drawing.Color GradientColor1, System.Drawing.Color GradientColor2, Icon Icon, EventHandler onClick, Shortcut Shortcut) : base(Text, onClick, Shortcut)
		{
			// Owner Draw Property allows you to modify the menu item by handling OnMeasureItem and OnDrawItem
			OwnerDraw = true;
			mFont = new Font(FontFace, FontSize);
			mGradientColor1 = GradientColor1;
			mGradientColor2 = GradientColor2;
			mIcon = Icon;
		}

		#region "Events"
		//None at this time...
		#endregion
		#region "Properties"
		const int maxWidth = 10000;
		private Icon mIcon;
		private Font mFont;
		private System.Drawing.Color mGradientColor1 = SystemColors.Highlight;
		private System.Drawing.Color mGradientColor2 = SystemColors.Control;
		public Icon Icon {
			get { return mIcon; }
			set { mIcon = value; }
		}
		public Font Font {
			get { return mFont; }
			set { mFont = value; }
		}
		#endregion
		#region "Methods"
		private string GetShortcutText()
		{
			string functionReturnValue = null;
			functionReturnValue = "";
			//Append shortcut if one is set and it should be visible
			if (ShowShortcut && Shortcut != Shortcut.None) {
				//To get a string representation of a Shortcut value, cast it into a Keys value and use the KeysConverter class (via TypeDescriptor).
				Keys k = (Keys)Shortcut;
				functionReturnValue = System.ComponentModel.TypeDescriptor.GetConverter(typeof(Keys)).ConvertToString(k);
			}
			return functionReturnValue;
		}
		#endregion
		#region "EventHandlers"
		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			//OnDrawItem perfoms the task of actually drawing the item after measurement is complete
			base.OnDrawItem(e);

			Brush br = SystemBrushes.Control;

			//Let the Icon area have the same background as the rest of the menu entry...
			Rectangle rectCheck = e.Bounds;
			Rectangle rectText = e.Bounds;
			rectCheck.Width = rectCheck.Height;
			//Draw the main rectangle
			e.Graphics.FillRectangle(br, rectText);

			//Draw a background to the menu item with a linear gradient. This will use system defaults unless colors and have been
			//passed on menu item instantiation
			if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
				br = new System.Drawing.Drawing2D.LinearGradientBrush(rectText, mGradientColor1, mGradientColor2, System.Drawing.Drawing2D.LinearGradientMode.Horizontal);

			//Adjust rectangle for icon/CheckMark area and fill...
			float adjust = Convert.ToSingle(Microsoft.VisualBasic.Interaction.IIf(base.Parent is MainMenu, 0, rectCheck.Width));
			rectText.X += Convert.ToInt32(adjust);
			e.Graphics.FillRectangle(br, rectText);

			//Now that the menuItem background has been filled, draw any Icon we may need...
			if ((mIcon != null))
				e.Graphics.DrawIcon(mIcon, e.Bounds.Left + 2, e.Bounds.Top + 2);

			//Leave room for accelerator key
			StringFormat sf = new StringFormat();
			sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show;

			//Draw the actual menu text (adjusting for icon-space for all but top-level menuItems)...
			br = new SolidBrush(e.ForeColor);
			if (!base.Enabled)
				br = SystemBrushes.FromSystemColor(SystemColors.GrayText);
			e.Graphics.DrawString(Text, mFont, br, e.Bounds.Left + 2 + adjust, e.Bounds.Top + 2, sf);
			//Debug.WriteLine(String.Format("OnDrawItem(e): e.Graphics.DrawString(""{0}"", {{{1}}}:={2}, {{{3}}}, e.Bounds.Left + 2 + adjust ({4} + 2 + {5} = {6}), e.Bounds.Top + 2  ({7} + 2 = {8}), {{{9}}})", New Object() {Text, mFont.GetType.Name, mFont.Name, br.GetType.Name, e.Bounds.Left, adjust, e.Bounds.Left + 2 + adjust, e.Bounds.Top, e.Bounds.Top + 2, sf.GetType.Name}))
			string ShortcutText = GetShortcutText();
			if (!string.IsNullOrEmpty(ShortcutText)) {
				float ShortCutWidth = e.Graphics.MeasureString(ShortcutText, mFont, maxWidth, sf).Width;
				e.Graphics.DrawString(ShortcutText, mFont, br, e.Bounds.Right - ShortCutWidth - 10, e.Bounds.Top + 2, sf);
				//Debug.WriteLine(String.Format("OnDrawItem(e): e.Graphics.DrawString(""{0}"", {{{1}}}:={2}, {{{3}}}, e.Bounds.Right - ShortCutWidth - 10 ({4} - {5} - 10 = {6}), e.Bounds.Top + 2  ({7} + 2 = {8}), {{{9}}})", New Object() {ShortcutText, mFont.GetType.Name, mFont.Name, br.GetType.Name, e.Bounds.Right, ShortCutWidth, e.Bounds.Right - ShortCutWidth - 10, e.Bounds.Top, e.Bounds.Top + 2, sf.GetType.Name}))
			}
		}
		protected override void OnMeasureItem(MeasureItemEventArgs e)
		{
			//Debug.WriteLine(String.Format("OnMeasureItem: {0}", Text))
			//The MeasureItem event along with the OnDrawItem event are the two key events that need to be handled in order to create owner drawn menus.
			//Measure the string that makes up a given menu item and use it to set the size of the menu item being drawn.
			StringFormat sf = new StringFormat();
			sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show;
			base.OnMeasureItem(e);
			//Debug.WriteLine(vbTab & String.Format("On Return from MyBase.OnMeasureItem(e); .ItemHeight={0}; .ItemWidth={1}", e.ItemHeight, e.ItemWidth))
			e.ItemHeight = 22;

			//When we actually draw the string, we'll leave room for a CheckMark/Icon, so adjust our length to account for that space...
			int adjust = (int)Microsoft.VisualBasic.Interaction.IIf(base.Parent is MainMenu, 0, e.ItemHeight);
			string s = string.Format("{0} {1}", this.Text, GetShortcutText());
			e.ItemWidth = (int)Math.Ceiling(e.Graphics.MeasureString(s, mFont, maxWidth, sf).Width + adjust);
			//Debug.WriteLine(vbTab & String.Format("On Return from OnMeasureItem(e); .Text:=""{0}""; .ItemHeight={1}; .ItemWidth={2}", Text, e.ItemHeight, e.ItemWidth))
		}
		#endregion
	}
}

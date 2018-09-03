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
//frmImage.vb
//   Image Display Form...
//   Copyright © 1998-2018, Ken Clark
//*********************************************************************************************************************************
//
//   Modification History:
//   Date:       Description:
//   08/06/10    Corrected over-sized background scrolling;
//   12/05/09    Started History;
//=================================================================================================================================
//=================================================================================================================================
 // ERROR: Not supported in C#: OptionDeclaration
namespace TCBase
{
	public class frmImage : TCBase.frmTCBase
	{
		const string myFormName = "frmImage";
		public frmImage(clsSupport objSupport, TCBase.clsTCBase mTCBase, Form objParent, Image image) : base(objSupport, myFormName, mTCBase, objParent)
		{
			Resize += frmImage_Resize;
			Load += frmImage_Load;
			//This call is required by the Windows Form Designer.
			InitializeComponent();
			pbImage.Image = image;
		}
		#region " Windows Form Designer generated code "

		public frmImage() : base()
		{
			Resize += frmImage_Resize;
			Load += frmImage_Load;

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
		protected internal System.Windows.Forms.ContextMenu ctxImage;
		private System.Windows.Forms.MenuItem withEventsField_mnuImageCopy;
		protected internal System.Windows.Forms.MenuItem mnuImageCopy {
			get { return withEventsField_mnuImageCopy; }
			set {
				if (withEventsField_mnuImageCopy != null) {
					withEventsField_mnuImageCopy.Click -= mnuImageCopy_Click;
				}
				withEventsField_mnuImageCopy = value;
				if (withEventsField_mnuImageCopy != null) {
					withEventsField_mnuImageCopy.Click += mnuImageCopy_Click;
				}
			}
		}
		private System.Windows.Forms.MenuItem withEventsField_mnuImageSaveAs;
		protected internal System.Windows.Forms.MenuItem mnuImageSaveAs {
			get { return withEventsField_mnuImageSaveAs; }
			set {
				if (withEventsField_mnuImageSaveAs != null) {
					withEventsField_mnuImageSaveAs.Click -= mnuImageSaveAs_Click;
				}
				withEventsField_mnuImageSaveAs = value;
				if (withEventsField_mnuImageSaveAs != null) {
					withEventsField_mnuImageSaveAs.Click += mnuImageSaveAs_Click;
				}
			}
		}
		protected internal System.Windows.Forms.SaveFileDialog sfdTCStandard;
		internal System.Windows.Forms.Panel pcViewPort;
		internal System.Windows.Forms.PictureBox pbImage;
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmImage));
			this.ctxImage = new System.Windows.Forms.ContextMenu();
			this.mnuImageCopy = new System.Windows.Forms.MenuItem();
			this.mnuImageSaveAs = new System.Windows.Forms.MenuItem();
			this.scrollV = new System.Windows.Forms.VScrollBar();
			this.scrollH = new System.Windows.Forms.HScrollBar();
			this.sfdTCStandard = new System.Windows.Forms.SaveFileDialog();
			this.pcViewPort = new System.Windows.Forms.Panel();
			this.pbImage = new System.Windows.Forms.PictureBox();
			this.pcViewPort.SuspendLayout();
			this.SuspendLayout();
			//
			//ctxImage
			//
			this.ctxImage.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
				this.mnuImageCopy,
				this.mnuImageSaveAs
			});
			//
			//mnuImageCopy
			//
			this.mnuImageCopy.Index = 0;
			this.mnuImageCopy.Text = "&Copy";
			//
			//mnuImageSaveAs
			//
			this.mnuImageSaveAs.Index = 1;
			this.mnuImageSaveAs.Text = "Save &As";
			//
			//scrollV
			//
			this.scrollV.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Right);
			this.scrollV.LargeChange = 1000;
			this.scrollV.Location = new System.Drawing.Point(428, 0);
			this.scrollV.Maximum = 32767;
			this.scrollV.Name = "scrollV";
			this.scrollV.Size = new System.Drawing.Size(17, 248);
			this.scrollV.SmallChange = 100;
			this.scrollV.TabIndex = 8;
			//
			//scrollH
			//
			this.scrollH.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.scrollH.LargeChange = 1000;
			this.scrollH.Location = new System.Drawing.Point(0, 248);
			this.scrollH.Maximum = 32767;
			this.scrollH.Name = "scrollH";
			this.scrollH.Size = new System.Drawing.Size(428, 17);
			this.scrollH.SmallChange = 100;
			this.scrollH.TabIndex = 7;
			//
			//pcViewPort
			//
			this.pcViewPort.BackColor = System.Drawing.SystemColors.Desktop;
			this.pcViewPort.Controls.Add(this.pbImage);
			this.pcViewPort.Location = new System.Drawing.Point(0, 0);
			this.pcViewPort.Name = "pcViewPort";
			this.pcViewPort.Size = new System.Drawing.Size(428, 248);
			this.pcViewPort.TabIndex = 10;
			//
			//pbImage
			//
			this.pbImage.BackColor = System.Drawing.SystemColors.Desktop;
			this.pbImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pbImage.ContextMenu = this.ctxImage;
			this.pbImage.Image = (System.Drawing.Image)resources.GetObject("pbImage.Image");
			this.pbImage.Location = new System.Drawing.Point(0, 0);
			this.pbImage.Name = "pbImage";
			this.pbImage.Size = new System.Drawing.Size(488, 332);
			this.pbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pbImage.TabIndex = 7;
			this.pbImage.TabStop = false;
			//
			//frmImage
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(444, 266);
			this.Controls.Add(this.scrollV);
			this.Controls.Add(this.scrollH);
			this.Controls.Add(this.pcViewPort);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmImage";
			this.ShowInTaskbar = false;
			this.Text = "frmImage";
			this.pcViewPort.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		#region "Event Handlers"
		private void frmImage_Load(object sender, System.EventArgs e)
		{
			try {
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
			} catch (Exception ex) {
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, "Exception");
			}
		}
		private void frmImage_Resize(System.Object sender, System.EventArgs e)
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
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, "Exception");
			}
		}
		protected void mnuImageCopy_Click(object sender, System.EventArgs e)
		{
			try {
				base.epBase.SetError(pbImage, bpeNullString);

				DataObject objImage = new DataObject();
				objImage.SetData(DataFormats.Bitmap, true, this.pbImage.Image);
				Clipboard.SetDataObject(objImage, true);
			} catch (Exception ex) {
				base.epBase.SetError(pbImage, ex.Message);
			}
		}
		protected void mnuImageSaveAs_Click(object sender, System.EventArgs e)
		{
			try {
				base.epBase.SetError(pbImage, bpeNullString);

				string CurrentPath = null;
				string CurrentDrive = null;
				string CurrentImage = null;
				CurrentPath = ParsePath(mTCBase.ImagePath, ParseParts.DrvDirNoSlash);
				CurrentDrive = ParsePath(mTCBase.ImagePath, ParseParts.DrvOnly);
				CurrentImage = ParsePath(mTCBase.ImagePath, ParseParts.FileNameBaseExt);
				FileSystem.ChDrive(CurrentDrive);
				DirectoryInfo diCurrent = new DirectoryInfo(CurrentPath);
				if (diCurrent.Exists)
					FileSystem.ChDir(CurrentPath);
				var _with1 = this.sfdTCStandard;
				_with1.AddExtension = true;
				_with1.CheckFileExists = false;
				_with1.CheckPathExists = true;
				_with1.Title = "Save Image As";
				_with1.InitialDirectory = (mTCBase.ImagePath != bpeNullString ? ParsePath(mTCBase.ImagePath, ParseParts.DrvDirNoSlash) : mSupport.ApplicationPath);
				string filter = "All Picture Files|*.bmp;*.emf;*.exif;*.gif;*.ico;*.jpg;*.png;*.tiff;*.wmf|";
				filter += "Windows bitmap image (BMP) format (*.bmp)|*.bmp|";
				filter += "Enhanced Windows metafile (EMF) image format (*.emf)|*.emf|";
				filter += "Exchangeable Image File (Exif) format (*.exif)|*.exif|";
				filter += "Graphics Interchange Format (GIF) image format (*.gif)|*.gif|";
				filter += "Windows icon (ICO) image format (*.ico)|*.ico|";
				filter += "Joint Photographic Experts Group (JPEG) image format (*.jpg)|*.jpg|";
				filter += "W3C Portable Network Graphics (PNG) image format (*.png)|*.png|";
				filter += "Tag Image File Format (TIFF) image format (*.tiff)|*.tiff|";
				filter += "Windows metafile (WMF) image format (*.wmf)|*.wmf";
				_with1.Filter = filter;
				_with1.FilterIndex = 7;
				if (_with1.ShowDialog(this) == DialogResult.Cancel)
                    throw new ExitTryException(); 
				FileInfo fi = new FileInfo(_with1.FileName);

				string dataMember = this.pbImage.DataBindings[0].BindingMemberInfo.BindingMember;
				byte[] arrPicture = (byte[])mTCBase.CurrentRow[dataMember];
				MemoryStream ms = new MemoryStream(arrPicture);
				Image tmpImage = Image.FromStream(ms);
				switch (fi.Extension.ToLower()) {
					case ".bmp":
						tmpImage.Save(_with1.FileName, ImageFormat.Bmp);
						break;
					case ".emf":
						tmpImage.Save(_with1.FileName, ImageFormat.Emf);
						break;
					case ".exif":
						tmpImage.Save(_with1.FileName, ImageFormat.Exif);
						break;
					case ".gif":
						tmpImage.Save(_with1.FileName, ImageFormat.Gif);
						break;
					case ".ico":
						tmpImage.Save(_with1.FileName, ImageFormat.Icon);
						break;
					case ".jpg":
						tmpImage.Save(_with1.FileName, ImageFormat.Jpeg);
						break;
					case ".png":
						tmpImage.Save(_with1.FileName, ImageFormat.Png);
						break;
					case ".tiff":
						tmpImage.Save(_with1.FileName, ImageFormat.Tiff);
						break;
					case ".wmf":
						tmpImage.Save(_with1.FileName, ImageFormat.Wmf);
						break;
				}
				tmpImage = null;
				ms.Close();
            } catch (ExitTryException) { 
            } catch (Exception ex) {
				base.epBase.SetError(pbImage, ex.Message);
			}
		}
		private void scrollH_Scroll(System.Object sender, ScrollEventArgs e)
		{
			try {
				switch (e.Type) {
					case ScrollEventType.EndScroll:
						int newScrollValue = e.NewValue;
						pbImage.Left = -newScrollValue;
						break;
				}
			} catch (Exception ex) {
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, "Exception");
			}
		}
		private void scrollV_Scroll(System.Object sender, ScrollEventArgs e)
		{
			try {
				switch (e.Type) {
					case ScrollEventType.EndScroll:
						int newScrollValue = e.NewValue;
						pbImage.Top = -newScrollValue;
						break;
				}
			} catch (Exception ex) {
				ShowMsgBox(ex.Message, MsgBoxStyle.OkOnly, this, "Exception");
			}
		}
		#endregion
	}
}

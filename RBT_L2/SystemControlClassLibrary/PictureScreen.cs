//**********************************************************************************************************************
//  ���� class �̹Ƿ� ���� ������ ���� ���ʽÿ�.
//**********************************************************************************************************************
//  Class Name  : PictureScreen																							
//  Description : ȭ���� Capture�Ͽ� Printer ��� �Ǵ� �׸����Ϸ� ����													
//**********************************************************************************************************************
//  ��������    : 2008.																							
//  �� �� ��    :  																							
//  ��������    : 																							
//  �� �� ��    : 																										
//  Version     : 1.0																								
//**********************************************************************************************************************

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;

namespace SystemControlClassLibrary
{
    
 
    #region [Class] NativeCalls : Dll Import ����
    /// <summary>
    /// Dll Import ����
    /// </summary>
    internal class NativeCalls
	{
		[DllImport("user32.dll")]
		internal extern static IntPtr GetDesktopWindow();

		[DllImport("user32.dll")]
		internal extern static IntPtr GetDC(IntPtr windowHandle);

		[DllImport("gdi32.dll")]
		internal extern static IntPtr GetCurrentObject(IntPtr hdc, ushort objectType);

		[DllImport("user32.dll")]
		internal extern static void ReleaseDC(IntPtr hdc);

		[DllImport("user32.dll")]
		internal extern static void UpdateWindow(IntPtr hwnd);

        
    }
	#endregion

	#region [Class] PictureScreen : ȭ���� Capture�Ͽ� Printer ��� �Ǵ� �׸����Ϸ� ����
	/// <summary>
	/// ȭ���� Capture�Ͽ� Printer ��� �Ǵ� �׸����Ϸ� ����
	/// </summary>
	public class PictureScreen
	{
        int x = 0;
        int y = 0;
        int H = 0;
        int W = 0;

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
		public extern static long BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);

		private Image imgScreen;

		#region [Method] ScreenSave : ȭ�� �׸����Ϸ� ����
		/// <summary>
		/// ȭ�� �׸����Ϸ� ����
		/// </summary>
		/// <param name="sFileName">Save ��� File Name</param>
		public void ScreenSave(string strFileName)
		{
			try
			{
				imgScreen = this.CaptureScreen();
				PictureSave(strFileName);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message.Trim(), "Error : PictureScreen", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// ȭ�� �׸����Ϸ� ����
		/// </summary>
		/// <param name="sFileName">Save ��� File Name</param>
		/// <param name="frmScreen">Capture ��� Form Class Name</param>
		public void ScreenSave(string strFileName, Form frmScreen)
		{
			try
			{
				frmScreen.Refresh();
				imgScreen = this.CaptureScreen(frmScreen);
				PictureSave(strFileName);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message.Trim(), "Error : PictureScreen", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		#endregion

		#region [Method] PictureSave : ȭ�� �׸����Ϸ� ���� Dialog Show
		/// <summary>
		/// ȭ�� �׸����Ϸ� ���� Dialog Show
		/// </summary>
		/// <param name="sFileName">Save ��� File Name</param>
		private void PictureSave(string strFileName)
		{
			try
			{
				if (imgScreen != null)
				{
					SaveFileDialog saveFile		= new SaveFileDialog();

					saveFile.Title				= "�׸����� ����";
					saveFile.FileName			= strFileName;
					saveFile.Filter				= "Bitmap Files (*.bmp)|*.bmp|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif|PNG Files (*.png)|*.png|WMF Files (*.wmf)|*.wmf";
					saveFile.FilterIndex		= 2;
					saveFile.DefaultExt			= "jpg";
					saveFile.AddExtension		= true;
                    saveFile.InitialDirectory = "C:\\";
					//saveFile.InitialDirectory	= Application.StartupPath;

					if (saveFile.ShowDialog() == DialogResult.OK)
					{
						switch (saveFile.FilterIndex)
						{
							case 2:		imgScreen.Save(saveFile.FileName, ImageFormat.Jpeg);		return;
							case 3:		imgScreen.Save(saveFile.FileName, ImageFormat.Gif);			return;
							case 4:		imgScreen.Save(saveFile.FileName, ImageFormat.Png);			return;
							case 5:		imgScreen.Save(saveFile.FileName, ImageFormat.Wmf);			return;
							default:	imgScreen.Save(saveFile.FileName, ImageFormat.Bmp);			return;
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message.Trim(), "Error : PictureScreen", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		#endregion

		#region [Method] Capture Screen (1/3) : ��üȭ��(MDI Main) Capture Form Capture
		/// <summary>
		/// ��üȭ��(MDI Main) Capture Form Capture
		/// </summary>
		/// <returns>Capture Image</returns>
		public Image CaptureScreen()
		{
			IntPtr captureWindow	= NativeCalls.GetDesktopWindow();
			IntPtr captureDC		= NativeCalls.GetDC(captureWindow);
			IntPtr captureBitmap	= NativeCalls.GetCurrentObject(captureDC, 7);
			Image captureImage		= Image.FromHbitmap(captureBitmap);

			try
			{
				//NativeCalls.ReleaseDC(captureDC);
			}			
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message.Trim(), "Error : PictureScreen", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return captureImage;
		}
		#endregion

		#region [Method] Capture Screen (2/3) : ����ȭ��(MDI Child, Popup) Capture
		/// <summary>
		/// ����ȭ��(MDI Child, Popup) Capture
		/// </summary>
		/// <param name="frmScreen">MDI Child, Popup ȭ�� Name</param>
		/// <returns></returns>
		public Image CaptureScreen(Form frmScreen)
		{
			frmScreen.Refresh();

			Bitmap captureImage = new Bitmap(frmScreen.Width, frmScreen.Height);

			try
			{
				Graphics g = Graphics.FromImage(captureImage);
				Rectangle destRect = new Rectangle(0, 0, frmScreen.Width, frmScreen.Height);
				imgScreen = this.CaptureScreen();
				if (frmScreen.WindowState == FormWindowState.Maximized)
					g.DrawImage(imgScreen, destRect, frmScreen.Left+4, frmScreen.Top+4, frmScreen.Width+4, frmScreen.Height+36, GraphicsUnit.Pixel);
				else
					g.DrawImage(imgScreen, destRect, frmScreen.Left, frmScreen.Top, frmScreen.Width, frmScreen.Height+64, GraphicsUnit.Pixel);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message.Trim(), "Error : PictureScreen", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return captureImage;
		}
		#endregion

		#region [Method] Capture Screen (3/3) : Rectangle Capture
		/// <summary>
		/// Rectangle Capture
		/// </summary>
		/// <param name="rtCapture">Capture�Ϸ��� Rectangle</param>
		/// <returns></returns>
		public Image CaptureScreen(Rectangle rtCapture)
		{
			Bitmap captureImage = new Bitmap(rtCapture.Width, rtCapture.Height);

			try
			{
				Graphics g = Graphics.FromImage(captureImage);
				imgScreen = this.CaptureScreen();
				g.DrawImage(imgScreen, 0, 0, rtCapture, GraphicsUnit.Pixel);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message.Trim(), "Error : PictureScreen", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return captureImage;
		}
		#endregion

		#region [Method] PrintScreen : ȭ�� �����ͷ� ���
		/// <summary>
		/// ȭ�� �����ͷ� ���
		/// </summary>
		/// <param name="frmScreen">ȭ�� �����ͷ� ���</param>
		public void PrintScreen(Form frmScreen)
		{
            x = frmScreen.Location.X;
            y = frmScreen.Location.Y;
            H = frmScreen.Height;
            W = frmScreen.Width;

			try
			{
				imgScreen = this.CaptureScreen(frmScreen);

				PrintDocument prnDoc = new PrintDocument();
				PrintPreviewDialog prnPrev = new PrintPreviewDialog();

                //PaperSize paperSize = new PaperSize("First custom size", 1024, 768);
                PaperSize paperSize = new PaperSize("First custom size", 1273, 960);
                //PaperSize paperSize = new PaperSize("First custom size", 1600, 1200);
                //PaperSize paperSize = new PaperSize("First custom size", W, H);

                
                prnDoc.DefaultPageSettings.PaperSize = paperSize;
				prnDoc.DefaultPageSettings.Landscape = true;


                prnDoc.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);
				prnPrev.Document = prnDoc;

				prnDoc.Print();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message.Trim(), "Error : PictureScreen", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
		{
			try
			{                
				Graphics grpScreen = e.Graphics;
                grpScreen.DrawImage(imgScreen, 50, 10, Convert.ToInt32(imgScreen.Width * 0.70), Convert.ToInt32(imgScreen.Height * 0.70));
                //grpScreen.DrawImage(imgScreen, x, y, Convert.ToInt32(imgScreen.Width * 0.75), Convert.ToInt32(imgScreen.Height * 0.80));
            }
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message.Trim(), "Error : PictureScreen", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		#endregion

		#region [Method] Control Clipboard�� ����
		public void ClipBoardCopy(Control objControl)
		{
			try
			{
				objControl.Refresh();
				Point screen = objControl.PointToScreen(new Point(0, 0));
				Rectangle rtControl = new Rectangle(screen.X, screen.Y, objControl.Width, objControl.Height);
				Image imgPic = this.CaptureScreen(rtControl);
				Clipboard.SetDataObject(imgPic);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message.Trim(), "Error : PictureScreen", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		#endregion
	}
	#endregion
}
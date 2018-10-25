using System.Windows.Media.Imaging;

using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

namespace PasswordKeeper
{
    class SnapshotHelper
    {
        public static BitmapImage CaptureScreenShot(string url, int width, float scale)
        {
            BitmapImage image = new BitmapImage();
            WebBrowser wb = new WebBrowser();
            wb.ScrollBarsEnabled = false;
            wb.ScriptErrorsSuppressed = true;
            wb.Navigate(url);
            wb.Width = width;
            wb.DocumentCompleted += delegate
            {
                if (wb.Height != wb.Document.Body.ScrollRectangle.Height)
                {
                    wb.Height = wb.Document.Body.ScrollRectangle.Height;
                }
            };
            while (true)
            {
                if (wb.ReadyState == WebBrowserReadyState.Complete)
                {
                    wb.Height = wb.Document.Body.ScrollRectangle.Height;
                    using (Bitmap bitmap = new Bitmap(wb.Width, wb.Height))
                    {
                        wb.DrawToBitmap(bitmap, new Rectangle(0, 0, wb.Width, wb.Height));
                        MemoryStream ms = new MemoryStream();
                        bitmap.Save(ms, ImageFormat.Bmp);
                        image.BeginInit();
                        //ms.Seek(0, SeekOrigin.Begin);
                        image.StreamSource = ms;
                        image.DecodePixelWidth = (int)(wb.Width * scale);
                        image.DecodePixelHeight = (int)(wb.Height * scale);
                        image.EndInit();
                    }
                    break;
                }
                Application.DoEvents();
            }
            wb.Dispose();
            return image;
        }

        public static BitmapImage CaptureScreenShot(string url, int width)
        {
            return CaptureScreenShot(url, width, 1f);
        }

        public static void SaveScreenShot(string url, int width, string fileName)
        {
            WebBrowser wb = new WebBrowser();
            wb.ScrollBarsEnabled = false;
            wb.ScriptErrorsSuppressed = true;
            wb.Navigate(url);
            wb.Width = width;
            wb.DocumentCompleted += delegate
            {
                if (wb.Height != wb.Document.Body.ScrollRectangle.Height)
                {
                    wb.Height = wb.Document.Body.ScrollRectangle.Height;
                }
            };
            while (true)
            {
                if (wb.ReadyState == WebBrowserReadyState.Complete)
                {
                    wb.Height = wb.Document.Body.ScrollRectangle.Height;
                    using (Bitmap bitmap = new Bitmap(wb.Width, wb.Height))
                    {
                        wb.DrawToBitmap(bitmap, new Rectangle(0, 0, wb.Width, wb.Height));
                        bitmap.Save(fileName, ImageFormat.Png);
                    }
                    break;
                }
                Application.DoEvents();
            }
            wb.Dispose();
        }
    }
}
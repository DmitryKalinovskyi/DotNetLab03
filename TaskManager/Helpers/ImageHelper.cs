using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace TaskManager.Helpers
{
    public static class ImageHelper
    {
        public static Icon GetTransparentIcon()
        {
            // Create a transparent icon
            Bitmap bitmap = new(32, 32, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.Transparent);
            }

            return Icon.FromHandle(bitmap.GetHicon());
        }

        public static Icon GetIconFromProcess(Process process)
        {
            Icon? icon = null;
            try
            {
                icon = Icon.ExtractAssociatedIcon(process.MainModule.FileName);
            }
            catch { }

            return icon ?? GetTransparentIcon();
        }

        public static Icon GetIconFromFilename(string filename)
        {
            Icon? icon = null;
            try
            {
                icon = Icon.ExtractAssociatedIcon(filename);
            }
            catch { }

            return  icon ?? GetTransparentIcon();
        }

        public static System.Windows.Media.ImageSource GetImageSourceFromIcon(Icon icon)
        {
            using (MemoryStream stream = new())
            {
                icon.ToBitmap().Save(stream, ImageFormat.Png);
                stream.Seek(0, SeekOrigin.Begin);

                BitmapImage bitmapImage = new();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = stream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        public static System.Windows.Media.ImageSource GetImageSourceFromProcess(Process process)
        {
            return GetImageSourceFromIcon(GetIconFromProcess(process));
        }

        public static System.Windows.Media.ImageSource GetImageSourceFromFilename(string filename)
        {
            return GetImageSourceFromIcon(GetIconFromFilename(filename));
        }
    }
}

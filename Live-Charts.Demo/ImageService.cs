using Svg;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;

namespace Live_Charts.Demo
{
    public class ImageService
    {
        public static ImageSource GetSVGBitmap(string filePath)
        {
            if (!File.Exists(filePath))
                return null;
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            SvgDocument svgDoc = SvgDocument.Open(doc);
            using (Bitmap bmp = svgDoc.Draw())
            {
                return BitmapToImageSource(bmp);
            }
        }

        public static BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png); // Was .Bmp, but this did not show a transparent background.

                stream.Position = 0;
                BitmapImage result = new BitmapImage();
                result.BeginInit();
                // According to MSDN, "The default OnDemand cache option retains access to the stream until the image is needed."
                // Force the bitmap to load right now so we can dispose the stream.
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();
                result.Freeze();
                return result;
            }
        }
    }
}
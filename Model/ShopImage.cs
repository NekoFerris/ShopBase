using ShopBase.Persistenz;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ShopBase
{
    /// <summary>
    /// Stelle ein Bild bereit
    /// </summary>
    ///
    public class ShopImage
    {
        public int Id { get; set; }
        public byte[] ImageData { get; set; }
        public Image GetImage { get => GetImageInt(); }

        public ShopImage()
        { }

        public ShopImage(string path)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
#pragma warning disable CA1416
                if (String.IsNullOrEmpty(path))
                    return;
                Image image = Image.FromFile(path);
                double ratioX = (double)180 / (double)image.Height;
                double ratioY = (double)180 / (double)image.Width;
                // use whichever multiplier is smaller
                double ratio = ratioX < ratioY ? ratioX : ratioY;

                // now we can get the new height and width
                int newHeight = Convert.ToInt32(image.Height * ratio);
                int newWidth = Convert.ToInt32(image.Width * ratio);
                Bitmap destImage = new(newWidth, newHeight);
                Rectangle destRect = new(0, 0, newWidth, newHeight);
                using Graphics graphics = Graphics.FromImage(destImage);
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
                MemoryStream ms = new();
                destImage.Save(ms, ImageFormat.Jpeg);
                ImageData = ms.ToArray();
            }
            throw new NotSupportedException("Einfügen von Fotos wird nur unter Windows unterstützt");
        }

        public Image GetImageInt()
        {
            MemoryStream ms = new(ImageData);
            return Image.FromStream(ms);
        }

        public void Anlegen()
        {
            DBShopImage.Anlegen(this);
        }

        public void Aktualisieren()
        {
            DBShopImage.Aktualisieren(this);
        }

        public void Loeschen()
        {
            DBShopImage.Loeschen(this);
        }

        public static ShopImage Lesen(int id)
        {
            return DBShopImage.Lesen(id);
        }
    }
}
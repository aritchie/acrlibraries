using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;


namespace Acr.Drawing {
    
    public static class ImageUtil {

        public static byte[] ToByteArray(this Image image) {
            byte[] buffer = null;
            using (var stream = new MemoryStream()) {
                image.Save(stream, image.RawFormat);
                buffer = stream.ToArray();
            }
            return buffer;
        }
        

        public static Image ToImage(this byte[] buffer) {
            Image image = null;
            using (var stream = new MemoryStream(buffer)) {
                image = Image.FromStream(stream, true);
            }
            return image;
        }

        
        public static string ToBase64HtmlString(this Image image) {
            return String.Format(
                "data:image/{0};base64,{1}",
                image.GetImageFormat(),
                Convert.ToBase64String(image.ToByteArray())
            );
        }


        public static Image ToImageFromBase64HtmlString(string encode) {
            string base64 = encode.Substring(encode.IndexOf(','));
            var buffer = Convert.FromBase64String(base64);
            return buffer.ToImage();
        }


        public static string GetImageFormat(this Image image) {
            var r = image.RawFormat;
            string v = "";

            if (r.Equals(ImageFormat.Bmp)) {
                v = "bmp";
            }
            else if (r.Equals(ImageFormat.Emf)) {
                v = "emf";
            }
            else if (r.Equals(ImageFormat.Exif)) {
                v = "exif";
            }
            else if (r.Equals(ImageFormat.Gif)) {
                v = "gif";
            }
            else if (r.Equals(ImageFormat.Icon)) {
                v = "icon";
            }
            else if (r.Equals(ImageFormat.Jpeg)) {
                v = "jpeg";
            }
            else if (r.Equals(ImageFormat.MemoryBmp)) {
                v = "bmp";
            }
            else if (r.Equals(ImageFormat.Png)) {
                v = "png";
            }
            else if (r.Equals(ImageFormat.Tiff)) {
                v = "tiff";
            }
            else if (r.Equals(ImageFormat.Wmf)) {
                v = "wmf";
            }
            return v;
        }
    }
}


/*
public class ImageResizer
{
    /// <summary>
    /// Maximum width of resized image.
    /// </summary>
    public int MaxX { get; set; }

    /// <summary>
    /// Maximum height of resized image.
    /// </summary>
    public int MaxY { get; set; }

    /// <summary>
    /// If true, resized image is trimmed to exactly fit
    /// maximum width and height dimensions.
    /// </summary>
    public bool TrimImage { get; set; }

    /// <summary>
    /// Format used to save resized image.
    /// </summary>
    public ImageFormat SaveFormat { get; set; }

    /// <summary>
    /// Constructor.
    /// </summary>
    public ImageResizer()
    {
        MaxX = MaxY = 150;
        TrimImage = false;
        SaveFormat = ImageFormat.Jpeg;
    }

    /// <summary>
    /// Resizes the image from the source file according to the
    /// current settings and saves the result to the targe file.
    /// </summary>
    /// <param name="source">Path containing image to resize</param>
    /// <param name="target">Path to save resized image</param>
    /// <returns>True if successful, false otherwise.</returns>
    public bool Resize(string source, string target)
    {
        using (Image src = Image.FromFile(source, true))
        {
            // Check that we have an image
            if (src != null)
            {
                int origX, origY, newX, newY;
                int trimX = 0, trimY = 0;

                // Default to size of source image
                newX = origX = src.Width;
                newY = origY = src.Height;

                // Does image exceed maximum dimensions?
                if (origX > MaxX || origY > MaxY)
                {
                    // Need to resize image
                    if (TrimImage)
                    {
                        // Trim to exactly fit maximum dimensions
                        double factor = Math.Max((double)MaxX / (double)origX,
                            (double)MaxY / (double)origY);
                        newX = (int)Math.Ceiling((double)origX * factor);
                        newY = (int)Math.Ceiling((double)origY * factor);
                        trimX = newX - MaxX;
                        trimY = newY - MaxY;
                    }
                    else
                    {
                        // Resize (no trim) to keep within maximum dimensions
                        double factor = Math.Min((double)MaxX / (double)origX,
                            (double)MaxY / (double)origY);
                        newX = (int)Math.Ceiling((double)origX * factor);
                        newY = (int)Math.Ceiling((double)origY * factor);
                    }
                }

                // Create destination image
                using (Image dest = new Bitmap(newX - trimX, newY - trimY))
                {
                    Graphics graph = Graphics.FromImage(dest);
                    graph.InterpolationMode =
                        System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graph.DrawImage(src, -(trimX / 2), -(trimY / 2), newX, newY);
                    dest.Save(target, SaveFormat);
                    // Indicate success
                    return true;
                }
            }
        }
        // Indicate failure
        return false;
    }
}
*/
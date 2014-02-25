using System;
using System.Drawing;
using System.IO;


namespace Acr.Mail {
    
    public class TemplateHelper {

        public bool IsEmpty(string @string) {
            return String.IsNullOrWhiteSpace(@string);
        }


        public bool IsDateSet(DateTime? date) {
            return (date != null && date.Value != DateTime.MinValue);
        }


        public string ToBase64ImageFromPath(string fileName, bool includeTag, bool sizeFromDimensions) {
            return ToBase64ImageFromBytes(File.ReadAllBytes(fileName), includeTag, sizeFromDimensions);
        }


        public string ToBase64ImageFromBytes(byte[] imageBytes, bool includeTag, bool sizeFromDimensions) {
            var img = "";
            using (var ms = new MemoryStream(imageBytes)) {
                using (var image = Image.FromStream(ms)) {
                    var base64 = String.Format(
                        "data:image/{0};base64,{1}", 
                        image.RawFormat,
                        Convert.ToBase64String(imageBytes)
                    );

                    if (!includeTag) {
                        img = base64;
                    }
                    else {
                        img = String.Format("<img src=\"{0}\" ", base64);
                        if (sizeFromDimensions) {
                            img += String.Format(
                                "width=\"{0}\" height=\"{1}\" ",
                                image.Width,
                                image.Height
                            );
                        }
                    }
                }
            }
            return img;
        }
    }
}

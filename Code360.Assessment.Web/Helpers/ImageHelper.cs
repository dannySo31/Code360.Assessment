using ImageResizer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace Code360.Assessment.Web.Helpers
{
    public static class ImageHelper
    {
        public const int QUALITY = 90;
        public static byte[] ConvertImageToByteArray(string imagePath)
        {
            if (imagePath == string.Empty)
                return new byte[0];
            var img = Image.FromFile(imagePath);
            using (MemoryStream mStream = new MemoryStream())
            {
                img.Save(mStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                return mStream.ToArray();
            }
        }
        public static byte[] ConvertUploadedFile(Stream postedFile, long limit)
        {
            Stream stream = postedFile;
            using (MemoryStream outStream = new MemoryStream()) { 
            //resize image
                var settings = new ResizeSettings
                {
                    MaxWidth= ValidationHelper.MAX_UPLOAD_FILE_WIDTH_PX,
                    MaxHeight= ValidationHelper.MAX_UPLOAD_FILE_HEIGHT_PX,
                    Mode= FitMode.Max,
                    Format="jpg"
                };
            settings.Add("quality", QUALITY.ToString());

            ImageBuilder.Current.Build(stream, outStream, settings);
        
            var result= outStream.ToArray() ;
            outStream.Dispose();
            if (result.Length > limit)
            {
                
                ConvertUploadedFile(outStream, limit);
            }
            return result;
        }
        }
    }
}
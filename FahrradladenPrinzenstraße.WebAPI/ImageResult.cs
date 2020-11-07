using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstraße.WebAPI
{
    public class ImageResult : ActionResult
    {
        public ImageResult() { }
        public int? Quality { get; set; }
        public Image Image { get; set; }
        public ImageFormat ImageFormat { get; set; }
        public byte[] EncodedImageBytes { get; set; }

        public override Task ExecuteResultAsync(ActionContext context)
        {
         
            // verify properties 
            if (EncodedImageBytes == null)
            {
                if (Image == null)
                {
                    throw new ArgumentNullException("Image");
                }
            }
            if (ImageFormat == null)
            {
                throw new ArgumentNullException("ImageFormat");
            }
            // output 
            context.HttpContext.Response.Clear();

            if (ImageFormat.Equals(ImageFormat.Bmp)) context.HttpContext.Response.ContentType = "image/bmp";
            if (ImageFormat.Equals(ImageFormat.Gif)) context.HttpContext.Response.ContentType = "image/gif";
            if (ImageFormat.Equals(ImageFormat.Icon)) context.HttpContext.Response.ContentType = "image/vnd.microsoft.icon";
            if (ImageFormat.Equals(ImageFormat.Jpeg)) context.HttpContext.Response.ContentType = "image/jpeg";
            if (ImageFormat.Equals(ImageFormat.Png)) context.HttpContext.Response.ContentType = "image/png";
            if (ImageFormat.Equals(ImageFormat.Tiff)) context.HttpContext.Response.ContentType = "image/tiff";
            if (ImageFormat.Equals(ImageFormat.Wmf)) context.HttpContext.Response.ContentType = "image/wmf";

            // output stream
            Stream outputStream = context.HttpContext.Response.Body;
            if (EncodedImageBytes != null)
                outputStream.WriteAsync(EncodedImageBytes, 0, EncodedImageBytes.Length);

            return base.ExecuteResultAsync(context);
        }

    }
}

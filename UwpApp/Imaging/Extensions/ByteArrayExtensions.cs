using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;

namespace UwpApp.Imaging.Extensions
{
    static class ByteArrayExtensions
    {
        public static SoftwareBitmap ToSoftwareBitmap(this byte[] pixelData, int width, int height)
        {
            return SoftwareBitmap.CreateCopyFromBuffer(pixelData.AsBuffer(), BitmapPixelFormat.Bgra8, width, height);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace UwpApp.Imaging.Extensions
{
    static class PixelArrayExtensions
    {
        public static byte[] ToBgra8Array(this Pixel[] pixels)
        {
            var result = new byte[pixels.Length * 4];

            for (int i = 0; i < pixels.Length; i++)
            {
                result[i * 4] = pixels[i].B;
                result[i * 4 + 1] = pixels[i].G;
                result[i * 4 + 2] = pixels[i].R;
                result[i * 4 + 3] = byte.MaxValue;
            }

            return result;
        }

        public static byte[] ToBgr8Array(this Pixel[] pixels)
        {
            var result = new byte[pixels.Length * 3];

            for (int i = 0; i < pixels.Length; i++)
            {
                result[i * 3] = pixels[i].B;
                result[i * 3 + 1] = pixels[i].G;
                result[i * 3 + 2] = pixels[i].R;
            }

            return result;
        }

        public static SoftwareBitmap ToSoftwareBitmap(this Pixel[] pixels, int width, int height)
        {
            return pixels.ToBgra8Array().ToSoftwareBitmap(width, height);
        }

        public static async Task<SoftwareBitmapSource> ToSoftwareBitmapSourceAsync(this Pixel[] pixels, int width, int height)
        {
            return await pixels.ToBgra8Array().ToSoftwareBitmap(width, height).ToSoftwareBitmapSourceAsync();
        }
    }
}

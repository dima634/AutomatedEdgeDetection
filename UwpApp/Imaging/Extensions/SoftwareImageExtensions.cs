using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace UwpApp.Imaging.Extensions
{
    static class SoftwareBitmapExtensions
    {
        public static async Task<SoftwareBitmapSource> ToSoftwareBitmapSourceAsync(this SoftwareBitmap softwareBitmap)
        {
            if (softwareBitmap.BitmapPixelFormat != BitmapPixelFormat.Bgra8 ||
                softwareBitmap.BitmapAlphaMode == BitmapAlphaMode.Straight)
            {
                softwareBitmap = SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
            }

            var imageSource = new SoftwareBitmapSource();
            await imageSource.SetBitmapAsync(softwareBitmap);
            return imageSource;
        }

        //public static async Task<SoftwareBitmap> ResizeAsync(this SoftwareBitmap softwareBitmap, int newWidth, int newHeight)
        //{
        //    var transform = new BitmapTransform()
        //    {
        //        ScaledWidth = (uint)newWidth,
        //        ScaledHeight = (uint)newHeight
        //    };

        //    var buf = new WriteableBitmap(softwareBitmap.PixelWidth, softwareBitmap.PixelHeight);
        //    var stream = buf.PixelBuffer.AsStream();
        //    var decoder = await BitmapDecoder.CreateAsync((stream.AsRandomAccessStream());
        //    var pixelData = await decoder.GetPixelDataAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied, transform, ExifOrientationMode.RespectExifOrientation, ColorManagementMode.DoNotColorManage);
        //    var pixels = pixelData.DetachPixelData();

        //    var wBitmap = new WriteableBitmap((int)decoder.PixelWidth, (int)decoder.PixelHeight);
        //    await wBitmap.SetSourceAsync(stream.AsRandomAccessStream());

        //    var sBitmap = SoftwareBitmap.CreateCopyFromBuffer(wBitmap.PixelBuffer, BitmapPixelFormat.Bgra8, newWidth, newHeight, BitmapAlphaMode.Premultiplied);

        //    return sBitmap;
        //}
    }
}

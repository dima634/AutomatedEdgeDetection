using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

using UwpApp.Imaging.Extensions;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

namespace UwpApp.Imaging
{
    class Utils
    {
        public static async Task<StorageFile> PickImageAsync()
        {
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            return await picker.PickSingleFileAsync();
        }

        public static async Task<SoftwareBitmap> LoadAndResizeImageAsync(StorageFile file, int newWidth, int newHeight)
        {
            var transform = new BitmapTransform()
            {
                ScaledHeight = (uint)newHeight,
                ScaledWidth = (uint)newWidth
            };

            var stream = await file.OpenReadAsync();
            var decoder = await BitmapDecoder.CreateAsync(stream);
            var pixelData = await decoder.GetPixelDataAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied, transform, ExifOrientationMode.RespectExifOrientation, ColorManagementMode.ColorManageToSRgb);
            var pixels = pixelData.DetachPixelData();

            return pixels.ToSoftwareBitmap(newWidth, newHeight);
        }

        public static async Task<SoftwareBitmap> LoadAndResizeImageAsync(StorageFile file, int mod)
        {

            var stream = await file.OpenReadAsync();
            var decoder = await BitmapDecoder.CreateAsync(stream);

            int newWidth = (int)(decoder.PixelWidth - (decoder.PixelWidth % mod));
            int newHeight = (int)(decoder.PixelHeight - (decoder.PixelHeight % mod));

            var transform = new BitmapTransform()
            {
                ScaledHeight = (uint)newHeight,
                ScaledWidth = (uint)newWidth
            };

            var pixelData = await decoder.GetPixelDataAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied, transform, ExifOrientationMode.RespectExifOrientation, ColorManagementMode.ColorManageToSRgb);
            var pixels = pixelData.DetachPixelData();

            return pixels.ToSoftwareBitmap(newWidth, newHeight);
        }

        public static async Task<SoftwareBitmap> LoadImageAsync(StorageFile file)
        {
            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
            {
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);

                return await decoder.GetSoftwareBitmapAsync();
            }
        }

        public static Pixel GetPixel(Pixel[] pixels, int row, int col, int imageWidth)
        {
            return pixels[row * imageWidth + col];
        }
        public static int GetPixelIndex(int row, int col, int imageWidth)
        {
            return row * imageWidth + col;
        }
    }
}

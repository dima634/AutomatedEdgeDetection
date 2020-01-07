using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;

namespace UwpApp.Imaging
{
    class Gallery
    {
        public async Task<List<SoftwareBitmap>> LoadImagesAsync()
        {
            var files = (await (await ApplicationData.Current.LocalFolder.GetFolderAsync("Gallery")).GetFilesAsync()).Where(i => IsImageFile(i));
            var images = new List<SoftwareBitmap>();

            foreach (var file in files)
            {
                images.Add(await Utils.LoadImageAsync(file));
            }

            return images;
        }

        public async void AddImageAsync(SoftwareBitmap image, string imageName)
        {
            var outputFile = await (await ApplicationData.Current.LocalFolder.GetFolderAsync("Gallery")).CreateFileAsync($"{imageName}.png");

            using (IRandomAccessStream stream = await outputFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);

                encoder.SetSoftwareBitmap(image);

                try
                {
                    await encoder.FlushAsync();
                }
                catch (Exception err)
                {
                    const int WINCODEC_ERR_UNSUPPORTEDOPERATION = unchecked((int)0x88982F81);
                    switch (err.HResult)
                    {
                        case WINCODEC_ERR_UNSUPPORTEDOPERATION:
                            encoder.IsThumbnailGenerated = false;
                            break;
                        default:
                            throw;
                    }
                }
            }
        }

        public async void DeleteImageAsync(string imageName)
        {
            await (await (await ApplicationData.Current.LocalFolder.GetFolderAsync("Gallery")).GetFileAsync($"{imageName}.png")).DeleteAsync();
        }

        string[] _allowedFileTypes = new string[] { ".png", ".jpg", ".jpeg" };
        private bool IsImageFile(StorageFile file)
        {
            return _allowedFileTypes.Contains(file.FileType);
        }
    }
}

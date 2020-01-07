using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Media.Imaging;

namespace UwpApp.Imaging
{
    class ImageSplitter
    {
        private int _slidingWindowSize;
        private SoftwareBitmap _image;
        private Pixel[] _pixels;
        private bool _useSingleStepSpliting;

        public ImageSplitter(SoftwareBitmap softwareBitmap, int slidingWindowSize, bool useSingleStepSpliting = false)
        {
            _image = softwareBitmap;
            _slidingWindowSize = slidingWindowSize;
            _useSingleStepSpliting = useSingleStepSpliting;

            if (_image.PixelHeight % _slidingWindowSize != 0 ||
                _image.PixelWidth % _slidingWindowSize != 0 && !useSingleStepSpliting)
            {
                throw new ArgumentException("Image size is not valid", nameof(softwareBitmap));
            }

            byte[] components = new byte[4 * _image.PixelHeight * _image.PixelWidth];
            _image.CopyToBuffer(components.AsBuffer());
            _pixels = ExtractPixels(components);
        }

        public Pixel[] GetRect(int partHorizontalOffset, int partVerticalOffset)
        {
            var result = new Pixel[_slidingWindowSize * _slidingWindowSize];

            for (int j = 0, horizontalOffset = 0, verticalOffset = 0;
                j < result.Length;
                j++, horizontalOffset++)
            {
                result[j] = Utils.GetPixel(_pixels, partVerticalOffset + verticalOffset, partHorizontalOffset + horizontalOffset, _image.PixelWidth);

                if (horizontalOffset == _slidingWindowSize - 1)
                {
                    horizontalOffset = -1;
                    verticalOffset++;
                }
            }

            return result; 
        }

        public Pixel[][] Split()
        {

            int step = _slidingWindowSize, resultSize = _image.PixelHeight / _slidingWindowSize * _image.PixelWidth / _slidingWindowSize;

            if (_useSingleStepSpliting)
            {
                step = 1;
                resultSize = (_image.PixelHeight - _slidingWindowSize + 1) *
                             (_image.PixelWidth - _slidingWindowSize + 1);
            }

            var splitted = new Pixel[resultSize][];

            for (int i = 0, hOffset = 0, vOffset = 0; i < splitted.Length; i++, hOffset += step)
            {
                splitted[i] = GetRect(hOffset, vOffset);

                if(hOffset == _image.PixelWidth - _slidingWindowSize)
                {
                    hOffset = -step;
                    vOffset += step;
                }
            }

            return splitted;
        }

        private Pixel[] ExtractPixels(byte[] components)
        {
            Pixel[] pixels = new Pixel[components.Length / 4];

            for (int i = 0, offset = 0; i < pixels.Length; i++, offset += 4)
            {
                pixels[i] = new Pixel()
                {
                    B = components[offset],
                    G = components[offset + 1],
                    R = components[offset + 2]
                };
            }

            return pixels;
        }
    }

    public struct Pixel
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
    }
}

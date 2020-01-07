using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UwpApp.Imaging.Extensions;
using UwpApp.MachineLearning.Datasets;
using Windows.Graphics.Imaging;

namespace UwpApp.Imaging
{
    class SlidingWindowSizeStepConstruction : IConstructingAlgorithm
    {
        public static Pixel Black => new Pixel()
        {
            B = 0,
            G = 0,
            R = 0
        };
        public static Pixel White => new Pixel()
        {
            B = 255,
            G = 255,
            R = 255
        };

        private int _slidingWindowSize;
        private int _width;
        private int _height;

        private void Init(int slidingWindowSize, int outputImagePixelHeight, int outputImagePixelWidth)
        {
            _slidingWindowSize = slidingWindowSize;
            _width = outputImagePixelWidth;
            _height = outputImagePixelHeight;

            WhitePixels = new Pixel[_slidingWindowSize * _slidingWindowSize];
            Array.Fill(WhitePixels, White);

            Pixel[] pixels = new Pixel[_slidingWindowSize * _slidingWindowSize];
            WhitePixels.CopyTo(pixels, 0);
            for (int i = 1; i <= _slidingWindowSize; i++)
            {
                pixels[i * _slidingWindowSize - i] = Black;
            }
            ReverseDiagonalLine = pixels;

            pixels = new Pixel[_slidingWindowSize * _slidingWindowSize];
            WhitePixels.CopyTo(pixels, 0);
            for (int i = 0; i < _slidingWindowSize; i++)
            {
                pixels[i * _slidingWindowSize + i] = Black;
            }
            DiagonalLine = pixels;

            pixels = new Pixel[_slidingWindowSize * _slidingWindowSize];
            int minIdx = (_slidingWindowSize / 2 - (_slidingWindowSize % 2 == 0 ? 1 : 0)) * _slidingWindowSize - 1,
                maxIdx = ((_slidingWindowSize / 2) + 1) * _slidingWindowSize;

            for (int i = 0; i < pixels.Length; i++)
            {
                if (!(i > minIdx && i < maxIdx))
                {
                    pixels[i] = White;
                }
            }
            HorizontalLine = pixels;

            pixels = new Pixel[_slidingWindowSize * _slidingWindowSize];
            HorizontalLine.CopyTo(pixels, 0);
            VerticalLine = Transpone(pixels);
        }

        public Pixel[] HorizontalLine { get; private set; }

        public Pixel[] VerticalLine { get; private set; }

        public Pixel[] DiagonalLine { get; private set; }

        public Pixel[] ReverseDiagonalLine { get; private set; }

        public Pixel[] WhitePixels { get; private set; }

        private Pixel[] Transpone(Pixel[] array)
        {
            for (int i = 0; i < _slidingWindowSize; ++i)
            {
                for (int j = i + 1; j < _slidingWindowSize; ++j)
                {
                    var buffer = array[_slidingWindowSize * i + j];
                    array[_slidingWindowSize * i + j] = array[_slidingWindowSize * j + i];
                    array[_slidingWindowSize * j + i] = buffer;
                }
            }

            return array;
        }

        public SoftwareBitmap CreateImage(ImagePrediction[] predictions, int slidingWindowSize, int outputImagePixelHeight, int outputImagePixelWidth)
        {
            Init(slidingWindowSize, outputImagePixelHeight, outputImagePixelWidth);

            byte[] pixels = new byte[_height * _width * 4];

            for (int i = 0; i < predictions.Length; i++)
            {
                Pixel[] image = GetEdgeImage((EdgeType)predictions[i].PredictedEdgeType);

                for (int j = 0; j < image.Length; j++)
                {
                    int index = GetPixelIndex(i, j);

                    pixels[index * 4] = image[j].B;
                    pixels[index * 4 + 1] = image[j].G;
                    pixels[index * 4 + 2] = image[j].R;
                    pixels[index * 4 + 3] = byte.MaxValue;
                }
            }

            return pixels.ToSoftwareBitmap(_width, _height);
        }

        private int GetPixelIndex(int predictionIndex, int pixelIndex)
        {
            int rectCol = predictionIndex % (_width / _slidingWindowSize),
                rectRow = predictionIndex / (_width / _slidingWindowSize),
                pixelRow = pixelIndex / _slidingWindowSize,
                pixelCol = pixelIndex % _slidingWindowSize;

            return Utils.GetPixelIndex(rectRow * _slidingWindowSize + pixelRow, rectCol * _slidingWindowSize + pixelCol, _width);
        }

        private Pixel[] GetEdgeImage(EdgeType edgeType)
        {
            switch (edgeType)
            {
                case EdgeType.Horizontal:
                    return HorizontalLine;
                case EdgeType.Vertical:
                    return VerticalLine;
                case EdgeType.Diagonal:
                    return DiagonalLine;
                case EdgeType.ReverseDiagonal:
                    return ReverseDiagonalLine;
                case EdgeType.NonEdge:
                    return WhitePixels;
                default:
                    throw new ArgumentException("Wrong argument value", nameof(edgeType));
            }
        }
    }

    class SingleStepConstruction : IConstructingAlgorithm
    {
        public SoftwareBitmap CreateImage(ImagePrediction[] predictions, int slidingWindowSize, int outputImagePixelHeight, int outputImagePixelWidth)
        {
            byte[] pixels = new byte[outputImagePixelWidth * outputImagePixelHeight * 4];
            Array.Fill(pixels, byte.MaxValue);

            for (int i = 0; i < predictions.Length; i++)
            {
                if (predictions[i].PredictedEdgeType != (uint)EdgeType.NonEdge)
                {
                    var index = GetPixelIndex(i, outputImagePixelWidth, slidingWindowSize);

                    pixels[index * 4] = 0;
                    pixels[index * 4 + 1] = 0;
                    pixels[index * 4 + 2] = 0;
                    pixels[index * 4 + 3] = byte.MaxValue;
                }
            }

            return pixels.ToSoftwareBitmap(outputImagePixelWidth, outputImagePixelHeight);
        }

        private int GetPixelIndex(int predictionIndex, int width, int slidingWindowSize)
        {
            int row = predictionIndex / (width - slidingWindowSize + 1) + slidingWindowSize / 2,
                col = predictionIndex % (width - slidingWindowSize + 1) + slidingWindowSize / 2;

            return Utils.GetPixelIndex(row, col, width);
        }
    }

    interface IConstructingAlgorithm
    {
        SoftwareBitmap CreateImage(ImagePrediction[] predictions, int slidingWindowSize, int outputImagePixelHeight, int outputImagePixelWidth);
    }
}

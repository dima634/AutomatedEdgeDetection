using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UwpApp.Imaging;
using UwpApp.Imaging.Extensions;

namespace UwpApp.MachineLearning.Datasets
{
    class Dataset
    {
        private List<(byte[] pixels, EdgeType edgeType)> _patternsToAdd = new List<(byte[], EdgeType)>();

        public static string CsvFilePath { get; } = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "testdataset.csv");

        public const int ImageSize = 5;

        public void SaveData()
        {
            StringBuilder stringBuilder = new StringBuilder();
            
            foreach (var (pixels, edgeType) in _patternsToAdd)
            {
                stringBuilder.AppendLine(string.Join(',', pixels) + $",{(int)edgeType}");
            }

            using(var streamWriter = new StreamWriter(CsvFilePath, true))
            {
                streamWriter.Write(stringBuilder.ToString());
            }

            _patternsToAdd.Clear();
        }

        public void Add(Pixel[] pixels, EdgeType edgeType)
        {
            _patternsToAdd.Add((pixels.ToBgr8Array(), edgeType));
        }

        public List<ImageData> Load()
        {
            var result = new List<ImageData>();

            using (var streamReader = new StreamReader(CsvFilePath, true))
            {
                string line = null;
                while ((line = streamReader.ReadLine()) != null)
                {
                    var splitted = line.Split(',');
                    List<byte> pixels = new List<byte>();
                    for (int i = 0; i < ImageSize * ImageSize * 3; i++)
                    {
                        pixels.Add(byte.Parse(splitted[i]));
                    }

                    result.Add(new ImageData()
                    {
                        Pixels = pixels.Select(i => float.Parse(i.ToString())).ToArray(),
                        EdgeType = uint.Parse(splitted[ImageSize * ImageSize * 3])
                    });
                }
            }
            return result;
        }
    }

    enum EdgeType
    {
        Horizontal,
        Vertical,
        Diagonal,
        ReverseDiagonal,
        NonEdge
    }
}

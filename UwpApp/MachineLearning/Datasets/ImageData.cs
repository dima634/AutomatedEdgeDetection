using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UwpApp.Imaging;

namespace UwpApp.MachineLearning.Datasets
{
    class ImageData
    {
        /// <summary>
        ///     Array of bgr pixels
        /// </summary>
        [ColumnName("Features")]
        [VectorType(Dataset.ImageSize * Dataset.ImageSize * 3)]
        [LoadColumn(0, Dataset.ImageSize * Dataset.ImageSize * 3 - 1)]
        public float[] Pixels { get; set; }


        [ColumnName("Label")]
        [LoadColumn(Dataset.ImageSize * Dataset.ImageSize * 3)]
        public uint EdgeType { get; set; }
    }

    class ImagePrediction : ImageData
    {
        [ColumnName("PredictedLabel")]
        public uint PredictedEdgeType { get;set; }

        [ColumnName("Score")]
        public float[] Score { get; set; }

        public override string ToString()
        {
            return $"Predicted label: {(EdgeType)PredictedEdgeType}, Lable: {(EdgeType)EdgeType}";
        }
    }
}

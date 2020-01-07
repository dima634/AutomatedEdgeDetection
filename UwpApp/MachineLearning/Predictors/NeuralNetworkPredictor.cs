using Microsoft.ML;
using Microsoft.ML.Transforms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UwpApp.MachineLearning.Datasets;
using UwpApp.Settings;

namespace UwpApp.MachineLearning.Predictors
{
    class NeuralNetworkPredictor : NeuralNetworkPredictorBase, IPredictor
    {
        public override string ModelFilePath => AppSettings.Local["NeuralNetworkPredictorModelPath"].ToString();

        protected override string InputName => "sequential_1_input_01";

        protected override string OutputName => "dense_3_Softmax_0";

        protected override string OnnxModelPath => AppSettings.Local["NeuralNetworkOnnxModelPath"].ToString();
    }
}

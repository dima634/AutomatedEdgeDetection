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
    class ConvNeuralNetworkPredictor : NeuralNetworkPredictorBase, IPredictor
    {
        public override string ModelFilePath => AppSettings.Local["ConvNeuralNetworkPredictorModelPath"].ToString();

        protected override string InputName => "conv2d_1_input_01";

        protected override string OutputName => "activation_1_Softmax_0";

        protected override string OnnxModelPath => AppSettings.Local["ConvNeuralNetworkOnnxModelPath"].ToString();
    }
}

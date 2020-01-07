using Microsoft.ML;
using Microsoft.ML.Trainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UwpApp.MachineLearning.Datasets;
using Microsoft.ML.Runtime;
using System.IO;
using UwpApp.Settings;

namespace UwpApp.MachineLearning.Predictors
{
    class LbfgsMaximumEntropyPredictor : RetrainablePredictorBase
    {
        public override string ModelFilePath => AppSettings.Local["LbfgsMaximumEntropyPredictorModelPath"].ToString();

        protected override IEstimator<ITransformer> GetTrainer()
        {
            return MLContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy()
                .Append(MLContext.Transforms.Conversion.MapKeyToValue("Label"))
                .Append(MLContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));
        }
    }
}

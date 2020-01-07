using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UwpApp.Settings;

namespace UwpApp.MachineLearning.Predictors
{
    class SdcaMaximumEntropyPredictor : RetrainablePredictorBase
    {
        public override string ModelFilePath => AppSettings.Local["SdcaMaximumEntropyPredictorModelPath"].ToString();

        protected override IEstimator<ITransformer> GetTrainer()
        {
            return MLContext.MulticlassClassification.Trainers.SdcaMaximumEntropy(maximumNumberOfIterations: 1000)
                .Append(MLContext.Transforms.Conversion.MapKeyToValue("Label"))
                .Append(MLContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));
        }
    }
}

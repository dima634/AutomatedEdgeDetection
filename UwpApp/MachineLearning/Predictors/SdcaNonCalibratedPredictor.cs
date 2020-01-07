using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpApp.MachineLearning.Predictors
{
    //class SdcaNonCalibratedPredictor : PredictorBase, IPredictor
    //{
    //    public override string ModelFilePath => Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "SdcaNonCalibratedPredictor.zip");

    //    protected override IEstimator<ITransformer> GetTrainer()
    //    {
    //        return MLContext.MulticlassClassification.Trainers.SdcaNonCalibrated()
    //            .Append(MLContext.Transforms.Conversion.MapKeyToValue("Label"))
    //            .Append(MLContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));
    //    }
    //}
}

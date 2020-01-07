using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UwpApp.MachineLearning.Datasets;

namespace UwpApp.MachineLearning.Predictors
{
    interface IPredictor
    {
        ImagePrediction PredictOne(ImageData data);
        ImagePrediction[] PredictBatch(ImageData[] data);
    }

    interface IRetrain
    {
        void Retrain();
    }

    abstract class PredictorBase : IPredictor
    {
        protected MLContext MLContext { get; set; } = DependencyInjection.Resolve<MLContext>();
        private PredictionEngine<ImageData, ImagePrediction> _model;
        private ITransformer _transformer;
        private DataViewSchema _columns;

        public PredictorBase()
        {
            if (!TryLoadModel())
            {
                BuildModel();
                SaveModel();
            }
        }

        public abstract string ModelFilePath { get; }

        protected abstract IEstimator<ITransformer> GetTrainer();

        public ImagePrediction PredictOne(ImageData data)
        {
            return _model.Predict(data);
        }

        public ImagePrediction[] PredictBatch(ImageData[] data)
        {
            var dataView = MLContext.Data.LoadFromEnumerable(data);
            var transformed = _transformer.Transform(dataView);
            return MLContext.Data.CreateEnumerable<ImagePrediction>(transformed, false).ToArray();
        }

        protected virtual bool TryLoadModel()
        {
            if (File.Exists(ModelFilePath))
            {
                _transformer = MLContext.Model.Load(ModelFilePath, out _columns);
                _model = MLContext.Model.CreatePredictionEngine<ImageData, ImagePrediction>(_transformer);
                return true;
            }
            else return false;
        }

        protected void SaveModel()
        {
            MLContext.Model.Save(_transformer, _columns, ModelFilePath);
        }

        protected void BuildModel()
        {
            var data = MLContext.Data.LoadFromEnumerable(new Dataset().Load());
            _columns = data.Schema;
            var estimator =
                MLContext.Transforms.Conversion.MapValueToKey("Label")
                .Append(GetTrainer());
            ITransformer categoricalTransformer = estimator.Fit(data);
            _transformer = categoricalTransformer;
            _model = MLContext.Model.CreatePredictionEngine<ImageData, ImagePrediction>(categoricalTransformer);
        }
    }

    abstract class RetrainablePredictorBase : PredictorBase, IRetrain
    {
        public void Retrain()
        {
            BuildModel();
            SaveModel();
        }
    }

    abstract class NeuralNetworkPredictorBase : PredictorBase
    {
        protected abstract string InputName { get; }

        protected abstract string OutputName { get; }

        protected abstract string OnnxModelPath { get; }

        protected override bool TryLoadModel()
        {
            MLContext.ComponentCatalog.RegisterAssembly(typeof(PredictionMappingFactory).Assembly);
            return base.TryLoadModel();
        }

        protected override IEstimator<ITransformer> GetTrainer()
        {
            return MLContext.Transforms.NormalizeMinMax("Features")
                    .Append(MLContext.Transforms.CopyColumns(InputName, "Features"))
                    .Append(MLContext.Transforms.ApplyOnnxModel(OutputName, InputName, OnnxModelPath))
                    .Append(MLContext.Transforms.Conversion.MapKeyToValue("Label"))
                    .Append(MLContext.Transforms.CopyColumns("NetworkPredictions", OutputName))
                    .Append(MLContext.Transforms.CustomMapping<NetworkOutputData, ImagePrediction>(PredictionMappingFactory.PredictionsMapping, nameof(PredictionMappingFactory.PredictionsMapping)));
        }

        public class NetworkOutputData : ImageData
        {
            public float[] NetworkPredictions { get; set; }
        }

        [CustomMappingFactoryAttribute(nameof(PredictionMappingFactory.PredictionsMapping))]
        public class PredictionMappingFactory : CustomMappingFactory<NetworkOutputData, ImagePrediction>
        {
            public static void PredictionsMapping(NetworkOutputData input, ImagePrediction output) =>
                output.PredictedEdgeType = Converter.Convert(input.NetworkPredictions);

            public override Action<NetworkOutputData, ImagePrediction> GetMapping()
            {
                return PredictionsMapping;
            }
        }

        class Converter
        {
            public static uint Convert(float[] array)
            {
                var val = (uint)Array.IndexOf(array, array.Max());

                switch (val)
                {
                    case 0:
                        return (uint)EdgeType.Diagonal;
                    case 1:
                        return (uint)EdgeType.ReverseDiagonal;
                    case 2:
                        return (uint)EdgeType.Horizontal;
                    case 3:
                        return (uint)EdgeType.Vertical;
                    case 4:
                        return (uint)EdgeType.NonEdge;
                    default:
                        throw new Exception();
                }
            }
        }
    }
}

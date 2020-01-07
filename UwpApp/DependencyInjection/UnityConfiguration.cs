using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Microsoft.ML;
using UwpApp.MachineLearning.Predictors;
using UwpApp.Imaging;
using UwpApp.ViewModels;
using UwpApp.Mvvm.ViewModels;

namespace UwpApp
{
    interface IUnityConfiguration
    {
        void RegisterTypes(UnityContainer unityContainer);
    }

    class UnityConfiguration : IUnityConfiguration
    {
        public void RegisterTypes(UnityContainer unityContainer)
        {
            unityContainer.RegisterSingleton<MLContext>();

            unityContainer.RegisterSingleton<IPredictor, NeuralNetworkPredictor>("Neural network");
            unityContainer.RegisterSingleton<IPredictor, ConvNeuralNetworkPredictor>("Conv neural network");
            unityContainer.RegisterSingleton<IPredictor, SdcaMaximumEntropyPredictor>("Sdca");
            unityContainer.RegisterSingleton<IPredictor, LbfgsMaximumEntropyPredictor>("Lbfgs");

            unityContainer.RegisterType<IConstructingAlgorithm, SlidingWindowSizeStepConstruction>("Sliding window size");
            unityContainer.RegisterType<IConstructingAlgorithm, SingleStepConstruction>("Single");

            unityContainer.RegisterSingleton<GalleryPageViewModel>();
            unityContainer.RegisterSingleton<DatasetGenerationPageViewModel>();
            unityContainer.RegisterSingleton<EdgeDetectionPageViewModel>();
        }
    }
}

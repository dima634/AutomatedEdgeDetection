using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UwpApp.ViewModels;

namespace UwpApp.Mvvm.ViewModels
{
    class ViewModelLocator
    {
        public DatasetGenerationPageViewModel DatasetGenerationPageViewModel => DependencyInjection.Resolve<DatasetGenerationPageViewModel>();

        public EdgeDetectionPageViewModel EdgeDetectionPageViewModel => DependencyInjection.Resolve<EdgeDetectionPageViewModel>();

        public GalleryPageViewModel GalleryPageViewModel => DependencyInjection.Resolve<GalleryPageViewModel>();
    }
}
 
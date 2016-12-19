using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Platform.IoC;
using SymMath.Core.ViewModels;

namespace SymMath.Core
{
    public class App : MvxApplication
    {
        public App()
        {
            // set the start object
            //var startApplicationObject = new LetterSelectionViewModel();

            //RegisterAppStart<LetterSelectionViewModel>();
        }

        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart<LetterSelectionViewModel>();
        }
    }
}

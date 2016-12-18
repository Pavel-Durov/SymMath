using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MvvmCross.Wpf.Views;
using System.Windows.Threading;

namespace SymMath
{
    public class Setup : MvvmCross.Wpf.Platform.MvxWpfSetup
    {
        public Setup(Dispatcher uiThreadDispatcher, IMvxWpfViewPresenter presenter) : base(uiThreadDispatcher, presenter)
        {

        }

        protected override IMvxApplication CreateApp()
        {    
            return new Core.App();
        }
    }
}

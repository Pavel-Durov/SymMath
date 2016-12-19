using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Wpf.Views;
using SymMath.Core.Providers;
using SymMath.Provider;
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
            Mvx.RegisterSingleton<IKeyboardListenerProvider>(() => new KeyboardListenerProvider());

            return new Core.App();
        }
    }
}

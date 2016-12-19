using MvvmCross.Core.ViewModels;
using SymMath.Core.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymMath.Core.ViewModels
{
    class LetterSelectionViewModel : MvxViewModel
    {
        IKeyboardListenerProvider _keyboardProvider;

        public LetterSelectionViewModel(IKeyboardListenerProvider keyboardProvider)
        {
            _keyboardProvider = keyboardProvider;
        }

        public override void Start()
        {
            base.Start();
        }
    }
}


using SymMath.Keyboard;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace SymMath
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static LetterSelector Selector;

        public MainWindow()
        {
            this.Visibility = System.Windows.Visibility.Hidden;
            this.ShowInTaskbar = false;

            InitializeComponent();

            // Hook keyboard events.
            Handler.ValidateCAPSLOCKState();

            LetterMappings.InitializeWindowsAndBindings();

            // Register keys.
            foreach (var letter in LetterMappings.KeysMap.Keys)
                LowLevelListener.HookedKeys.Add(letter);

            // Hook left, right arrow keys to move the selector.
            LowLevelListener.HookedKeys.Add(Key.Left);
            LowLevelListener.HookedKeys.Add(Key.Right);

            // Hook our "hot key".
            LowLevelListener.HookedKeys.Add(Key.CapsLock);
            LowLevelListener.HookedKeys.Add(Key.LeftShift);
            LowLevelListener.HookedKeys.Add(Key.RightShift);
            LowLevelListener.Register();

            LowLevelListener.KeyDown += new LowLevelListener.KeyHookEventHandler(e => Handler.HandleKeyPress(true, e));
            LowLevelListener.KeyUp += new LowLevelListener.KeyHookEventHandler(e => Handler.HandleKeyPress(false, e));

            try
            {
                // Keep the app responsive even if system is busy.
                // I found that AboveNormal does not keep the app responsive enough if the system is particularly busy,
                // although even at High there can be a noticeable (but generally acceptable) lag for activation.
                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            }
            catch { }
        }
    }
}

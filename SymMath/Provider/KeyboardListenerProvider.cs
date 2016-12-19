using SymMath.Core.Providers;
using SymMath.Keyboard;
using SymMath.Provider;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SymMath.Provider
{
    public class KeyboardListenerProvider : IKeyboardListenerProvider
    {
        public KeyboardListenerProvider()
        {
            LowLevelListener.KeyDown += new LowLevelListener.KeyHookEventHandler(e => HandleKeyPress(true, e));
            LowLevelListener.KeyUp += new LowLevelListener.KeyHookEventHandler(e => HandleKeyPress(false, e));
        }

        private static Boolean _sEnabled = true;
        private static IntPtr _sActiveKeyboardWindow;
        private static LetterSelector _sActiveSelectorWindow;
        private static Stopwatch _sShiftTimer = Stopwatch.StartNew();
        private const Int32 _kReleaseErrorMargin = 500; // millis

        private static Boolean _IsLetterPress(Key key)
        {
            return key != Key.CapsLock && key != Key.LeftShift && key != Key.RightShift && key != Key.Left && key != Key.Right;
        }

        public static Boolean HandleKeyPress(Boolean isDown, LowLevelListener.KeyHookEventArgs e)
        {
            if (!_sEnabled) return false;

            // If we get here with a letter without our hotkey, exit pronto.
            if (e.Key != Key.CapsLock && !e.ModifierCapsLock) return false;

            // Check if the letter has changed whilst the popup is showing, in this case we'll show another selector window.
            if (_sActiveSelectorWindow != null && _IsLetterPress(e.Key))
            {
                if (_sActiveSelectorWindow.Key != e.Key)
                {
                    var left = _sActiveSelectorWindow.Left;
                    var top = _sActiveSelectorWindow.Top;

                    _HidePopup();

                    if (!LetterMappings.KeyToWindowMap.TryGetValue(e.Key, out _sActiveSelectorWindow))
                        return false;

                    _ShowPopup(e.ModifierAnyShift, (Int32)left, (Int32)top);
                    return true;
                }
            }

            if (_sActiveSelectorWindow == null)
            {
                if (e.Key == Key.CapsLock)
                    return true; // disable capslock

                if (!LetterMappings.KeyToWindowMap.TryGetValue(e.Key, out _sActiveSelectorWindow))
                    return false;
            }

            if (_sActiveSelectorWindow == null) return false;

            var selectorShowing = _sActiveSelectorWindow.IsVisible;

            // Change case if shift is used.
            // First we need an additional shift key press to activate it, then we'll handle it as a modifier.
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {
                if (selectorShowing)
                {
                    if (isDown)
                    {
                        _sActiveSelectorWindow.ToUpper();
                        _sShiftTimer.Restart();
                    }
                    else
                        _sActiveSelectorWindow.ToLower();
                }

                return selectorShowing;
            }
            else if (selectorShowing && !e.ModifierAnyShift)
            {
                _sActiveSelectorWindow.ToLower();
            }

            // If the selector is showing, and this is a down press, go to next key.
            if (selectorShowing && isDown && e.Key != Key.CapsLock)
            {
                if (e.Key == Key.Left || (e.Key != Key.Right && e.ModifierAnyAlt))
                    _sActiveSelectorWindow.SelectPrevious();
                else
                    _sActiveSelectorWindow.SelectNext();
                return true;
            }

            // If selector is not showing and this is a down press, show it.
            if (!selectorShowing && isDown && e.Key != Key.CapsLock)
            {
                _sActiveKeyboardWindow = NativeInputHandler.GetForegroundWindow();

                var position = Caret.GetPosition(_sActiveKeyboardWindow);

                // Get the monitor on which we are and see if we need to adjust location to avoid falling off.
                IntPtr monitor;
                if (position.X == 0 && position.Y == 0)
                    monitor = NativeInputHandler.MonitorFromWindow(_sActiveKeyboardWindow, NativeInputHandler.MONITOR_DEFAULTTONEAREST);
                else
                    monitor = NativeInputHandler.MonitorFromPoint(position, NativeInputHandler.MONITOR_DEFAULTTONEAREST);

                if (monitor != IntPtr.Zero)
                {
                    // Get monitor size.
                    NativeInputHandler.MONITORINFO info = new NativeInputHandler.MONITORINFO();
                    info.cbSize = Marshal.SizeOf(info);
                    if (NativeInputHandler.GetMonitorInfo(monitor, ref info))
                    {
                        if (position.X == 0 && position.Y == 0)
                        {
                            // This can happen in two cases:
                            // - there is no caret
                            // - the application does not use win32 caret api
                            // Slap the popup in the middle of the screen as workaround for now.
                            position.X = info.rcWork.Left + (info.rcWork.Right - info.rcWork.Left) / 2 - (Int32)(_sActiveSelectorWindow.ActualWidth / 2.0);
                            position.Y = info.rcWork.Top + (info.rcWork.Bottom - info.rcWork.Top) / 2 - (Int32)(_sActiveSelectorWindow.ActualHeight / 2.0);
                        }

                        // Adjust position to fit within the given dimensions.
                        if (position.X + _sActiveSelectorWindow.ActualWidth > info.rcWork.Right)
                            position.X = info.rcWork.Right - (Int32)_sActiveSelectorWindow.ActualWidth;
                        if (position.Y + _sActiveSelectorWindow.ActualHeight > info.rcWork.Bottom)
                            position.Y = info.rcWork.Bottom - (Int32)_sActiveSelectorWindow.ActualHeight;
                    }
                }

                // If we got nowhere reasonable to put the popup, mark as handled and don't show it.
                if (position.X == 0 && position.Y == 0)
                    return true;

                //_ShowPopup(e.ModifierAnyShift, position.X / 2 , 0);
                _ShowPopup(e.ModifierAnyShift, left: 100, top: 15);
                return true;
            }

            // If the selector is showing, mark key as handled.
            if (selectorShowing && (e.Key != Key.CapsLock || isDown))
                return true;

            // If the selector is not showing, disable the activator key.
            if (!selectorShowing) return true;

            // Using the shift key is difficult because the user has to release many keys at once.
            // Often the shift key is release slightly before the rest which causes the letters to be
            // lower cased just before selecting. We allow a little time interval in which we ignore the shift up.
            if (_sShiftTimer.ElapsedMilliseconds < _kReleaseErrorMargin)
                _sActiveSelectorWindow.ToUpper();

            // As with the shift key, if the alt key is pressed when the key is sent this really
            // sends a alt+key shortcut combination to the target application. We allow for an error
            // margin and delay the sending of input if alt is used (but not enough to be really noticable).
            // Todo: instead send an alt up?
            _SendSelectedLetterAsKeyPress(delayInput: e.ModifierAnyAlt);
            return true;
        }

        public static void HandleMouseUp()
        {
            if (_sActiveSelectorWindow == null) return;
            if (!_sActiveSelectorWindow.IsActive || !_sActiveSelectorWindow.IsVisible) return;
            _SendSelectedLetterAsKeyPress();
        }

        private static void _HidePopup()
        {
            if (_sActiveSelectorWindow == null) return;

            _sActiveSelectorWindow.Visibility = Visibility.Hidden;
            _sActiveSelectorWindow = null;
        }

        private static void _ShowPopup(Boolean isUpper, Int32 left, Int32 top)
        {
            _sActiveSelectorWindow.Left = left;
            _sActiveSelectorWindow.Top = top;

            // If the user had shift pressed before pressing letter, ensure to show upper case.
            if (isUpper)
                _sActiveSelectorWindow.ToUpper();
            else
                _sActiveSelectorWindow.ToLower();

            _sActiveSelectorWindow.Visibility = Visibility.Visible;
        }

        private static void _SendSelectedLetterAsKeyPress(Boolean delayInput = false)
        {
            var pos = Caret.GetPosition(_sActiveKeyboardWindow);
            var letter = _sActiveSelectorWindow.SelectedLetter;

            _HidePopup();

            try
            {
                if (!NativeInputHandler.SetForegroundWindow(_sActiveKeyboardWindow))
                    // Something went wrong, ignore.
                    return;
            }
            catch (Win32Exception e)
            {
                // For reasons not yet understood we sometimes get a 0 error code turned into an exception (operation succeeded).
                if (e.NativeErrorCode != 0)
                    return;
            }

            var keyboardInput = new NativeInputHandler.KEYBDINPUT();
            keyboardInput.wVk = 0; // required by unicode event
            keyboardInput.wScan = (Int16)letter;
            keyboardInput.dwFlags = NativeInputHandler.KEYEVENTF.UNICODE;
            keyboardInput.dwExtraInfo = NativeInputHandler.GetMessageExtraInfo();
            keyboardInput.time = 0;

            var keyDown = new NativeInputHandler.INPUT();
            keyDown.type = NativeInputHandler.INPUT_KEYBOARD;
            keyDown.U.ki = keyboardInput;

            var keyUp = keyDown;
            keyUp.U.ki.dwFlags |= NativeInputHandler.KEYEVENTF.KEYUP;

            // If an error happens here it's probably due to UIPI, i.e. the target application is of higher integrity.
            // We ignore the error.
            if (delayInput)
                ThreadPool.QueueUserWorkItem(context =>
                {
                    // Just block for a bit.
                    Thread.Sleep(_kReleaseErrorMargin);

                    ((SynchronizationContext)context).Post(_ =>
                    {
                        NativeInputHandler.SendInput(1, new[] { keyDown }, Marshal.SizeOf(keyDown));
                        NativeInputHandler.SendInput(1, new[] { keyUp }, Marshal.SizeOf(keyDown));
                    }, null);
                }, SynchronizationContext.Current);
            else
            {
                NativeInputHandler.SendInput(1, new[] { keyDown, keyUp }, Marshal.SizeOf(keyDown));
                NativeInputHandler.SendInput(1, new[] { keyUp }, Marshal.SizeOf(keyDown));
            }
        }
    }
}

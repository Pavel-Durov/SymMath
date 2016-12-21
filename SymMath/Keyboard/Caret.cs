﻿/*
 * Based on https://github.com/mjvh80/SymWin Repository.
 * 
 * © Marcus van Houdt 2014
 */

using System;
using System.Runtime.InteropServices;

namespace SymMath.Keyboard
{
   [StructLayout(LayoutKind.Sequential)]
   internal struct Point
   {
      public Int32 X;
      public Int32 Y;
   }

   [StructLayout(LayoutKind.Sequential)]
   internal struct RECT
   {
      public Int32 Left, Top, Right, Bottom;
   }

   [StructLayout(LayoutKind.Sequential)]
   internal struct GUITHREADINFO
   {
      public UInt32 cbSize;
      public UInt32 flags;
      public IntPtr hwndActive;
      public IntPtr hwndFocus;
      public IntPtr hwndCapture;
      public IntPtr hwndMenuOwner;
      public IntPtr hwndMoveSize;
      public IntPtr hwndCaret;
      public RECT rcCaret;
   }

   internal static class Caret
   {
      #region Interop

      [DllImport("user32.dll")]
      static extern Boolean ClientToScreen(IntPtr hWnd, ref Point lpPoint);

      [DllImport("user32.dll")]
      private static extern Boolean GetGUIThreadInfo(UInt32 idThread, out GUITHREADINFO lpgui);

      #endregion

      // Based on http://www.codeproject.com/Articles/34520/Getting-Caret-Position-Inside-Any-Application.
      public static Point GetPosition(IntPtr window)
      {
         GUITHREADINFO info = new GUITHREADINFO();
         info.cbSize = (UInt32)Marshal.SizeOf(info);
         GetGUIThreadInfo(0, out info);

         Point caretPos;
         caretPos.X = info.rcCaret.Left;
         caretPos.Y = info.rcCaret.Bottom;

         ClientToScreen(info.hwndCaret, ref caretPos);

         return caretPos;
      }
   }
}

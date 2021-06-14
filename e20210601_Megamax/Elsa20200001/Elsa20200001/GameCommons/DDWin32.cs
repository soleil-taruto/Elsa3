using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DxLibDLL;
using Charlotte.Commons;

namespace Charlotte.GameCommons
{
	public static class DDWin32
	{
		[StructLayout(LayoutKind.Sequential)]
		public struct POINT
		{
			public int X;
			public int Y;
		}

		[DllImport("user32.dll")]
		private static extern bool
			ClientToScreen // KeepComment:@^_ConfuserElsa // NoRename:@^_ConfuserElsa
			(IntPtr hWnd, out POINT lpPoint);

		public static bool W_ClientToScreen(IntPtr hWnd, out POINT lpPoint)
		{
			return
				ClientToScreen // KeepComment:@^_ConfuserElsa // NoRename:@^_ConfuserElsa
				(hWnd, out lpPoint);
		}

		public delegate bool EnumWindowsCallback(IntPtr hWnd, IntPtr lParam);

		[DllImport("user32.dll")]
		private static extern bool
			EnumWindows // KeepComment:@^_ConfuserElsa // NoRename:@^_ConfuserElsa
			(EnumWindowsCallback callback, IntPtr lParam);

		public static bool W_EnumWindows(EnumWindowsCallback callback, IntPtr lParam)
		{
			return
				EnumWindows // KeepComment:@^_ConfuserElsa // NoRename:@^_ConfuserElsa
				(callback, lParam);
		}

		[DllImport("user32.dll")]
		private static extern int
			GetWindowText // KeepComment:@^_ConfuserElsa // NoRename:@^_ConfuserElsa
			(IntPtr hWnd, StringBuilder buff, int buffLenMax);

		public static int W_GetWindowText(IntPtr hWnd, StringBuilder buff, int buffLenMax)
		{
			return
				GetWindowText // KeepComment:@^_ConfuserElsa // NoRename:@^_ConfuserElsa
				(hWnd, buff, buffLenMax);
		}

		public static string GetWindowTitleByHandle(IntPtr hWnd)
		{
			const int BUFF_SIZE = 1000;
			const int MARGIN = 10;

			StringBuilder buff = new StringBuilder(BUFF_SIZE + MARGIN);

			W_GetWindowText(hWnd, buff, BUFF_SIZE);

			return buff.ToString();
		}

		public static void EnumWindowsHandleTitle(Func<IntPtr, string, bool> routine)
		{
			W_EnumWindows((hWnd, lParam) => routine(hWnd, GetWindowTitleByHandle(hWnd)), IntPtr.Zero);
		}

		private static IntPtr? MainWindowHandle = null;

		public static IntPtr GetMainWindowHandle()
		{
			if (MainWindowHandle == null)
			{
				string markTitle = Guid.NewGuid().ToString("B");
				IntPtr handle = IntPtr.Zero;
				bool handleFound = false;

				DX.SetMainWindowText(markTitle);

				EnumWindowsHandleTitle((hWnd, title) =>
				{
					if (title == markTitle)
					{
						handle = hWnd;
						handleFound = true;
						return false;
					}
					return true;
				});

				if (!handleFound)
					throw new DDError();

				DDMain.SetMainWindowTitle();

				MainWindowHandle = handle;
			}
			return MainWindowHandle.Value;
		}

		public const uint FR_PRIVATE = 0x10;

		[DllImport("gdi32.dll")]
		private static extern int
			AddFontResourceEx // KeepComment:@^_ConfuserElsa // NoRename:@^_ConfuserElsa
			(string file, uint fl, IntPtr res);

		public static int W_AddFontResourceEx(string file, uint fl, IntPtr res)
		{
			return
				AddFontResourceEx // KeepComment:@^_ConfuserElsa // NoRename:@^_ConfuserElsa
				(file, fl, res);
		}

		[DllImport("gdi32.dll")]
		private static extern int
			RemoveFontResourceEx // KeepComment:@^_ConfuserElsa // NoRename:@^_ConfuserElsa
			(string file, uint fl, IntPtr res);

		public static int W_RemoveFontResourceEx(string file, uint fl, IntPtr res)
		{
			return
				RemoveFontResourceEx // KeepComment:@^_ConfuserElsa // NoRename:@^_ConfuserElsa
				(file, fl, res);
		}

		public static I2Point GetMousePosition()
		{
			return new I2Point(
				Cursor.Position.X, Cursor.Position.Y // KeepComment:@^_ConfuserElsa // NoRename:@^_ConfuserElsa
				);
		}

		private static I4Rect[] Monitors = null;

		public static I4Rect[] GetAllMonitor()
		{
			if (Monitors == null)
				Monitors =
					Screen.AllScreens.Select( // KeepComment:@^_ConfuserElsa // NoRename:@^_ConfuserElsa
						screen => new I4Rect(
							screen
								.Bounds.Left, // KeepComment:@^_ConfuserElsa // NoRename:@^_ConfuserElsa
							screen
								.Bounds.Top, // KeepComment:@^_ConfuserElsa // NoRename:@^_ConfuserElsa
							screen
								.Bounds.Width, // KeepComment:@^_ConfuserElsa // NoRename:@^_ConfuserElsa
							screen
								.Bounds.Height // KeepComment:@^_ConfuserElsa // NoRename:@^_ConfuserElsa
							))
							.ToArray();

			return Monitors;
		}
	}
}

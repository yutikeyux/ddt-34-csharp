using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;

namespace MetroFramework.Native
{
	[SuppressUnmanagedCodeSecurity]
	internal class DwmApi
	{
		[StructLayout(LayoutKind.Explicit)]
		public struct RECT
		{
			[FieldOffset(12)]
			public int bottom;

			[FieldOffset(0)]
			public int left;

			[FieldOffset(8)]
			public int right;

			[FieldOffset(4)]
			public int top;

			internal static object wob6hV8dQNlarT9dAcy;

			public int Height => bottom - top;

			public Size Size => new Size(Width, Height);

			public int Width => right - left;

			public RECT(Rectangle rect)
			{
				left = rect.Left;
				top = rect.Top;
				right = rect.Right;
				bottom = rect.Bottom;
			}

			public RECT(int left, int top, int right, int bottom)
			{
				this.left = left;
				this.top = top;
				this.right = right;
				this.bottom = bottom;
			}

			public void Set()
			{
				left = smethod_0(ref top, smethod_0(ref right, smethod_0(ref bottom, 0)));
			}

			public void Set(Rectangle rect)
			{
				left = rect.Left;
				top = rect.Top;
				right = rect.Right;
				bottom = rect.Bottom;
			}

			public void Set(int left, int top, int right, int bottom)
			{
				this.left = left;
				this.top = top;
				this.right = right;
				this.bottom = bottom;
			}

			public Rectangle ToRectangle()
			{
				return new Rectangle(left, top, right - left, bottom - top);
			}

			private static iIsDFOpbIdh1RrFvy0 smethod_0<iIsDFOpbIdh1RrFvy0>(ref iIsDFOpbIdh1RrFvy0 gparam_0, iIsDFOpbIdh1RrFvy0 vj30PVkhgwf64CKhyf)
			{
				gparam_0 = vj30PVkhgwf64CKhyf;
				return vj30PVkhgwf64CKhyf;
			}

			internal static bool YkRUBZ8irRmKKs61dyx()
			{
				return wob6hV8dQNlarT9dAcy == null;
			}

			internal static void wXwUGS80ZI6VvGv2uTa()
			{
			}
		}

		public struct DWM_BLURBEHIND
		{
			public const int DWM_BB_ENABLE = 1;

			public const int DWM_BB_BLURREGION = 2;

			public const int DWM_BB_TRANSITIONONMAXIMIZED = 4;

			public int dwFlags;

			public int fEnable;

			public IntPtr hRgnBlur;

			public int fTransitionOnMaximized;

			public static DWM_BLURBEHIND Enable;

			public static DWM_BLURBEHIND Disable;

			private static object AEZaoe8uwvtjBvCqN5i;

			private DWM_BLURBEHIND(bool enable)
			{
				dwFlags = 1;
				fEnable = (enable ? 1 : 0);
				hRgnBlur = IntPtr.Zero;
				fTransitionOnMaximized = 0;
			}

			static DWM_BLURBEHIND()
			{
				Enable = new DWM_BLURBEHIND(enable: true);
				Disable = new DWM_BLURBEHIND(enable: false);
			}

			internal static bool zMcLuy8kjbeFSFJZstq()
			{
				return AEZaoe8uwvtjBvCqN5i == null;
			}

			internal static void AVsW2G8EWqKZkbNnOVw()
			{
			}
		}

		public struct DWM_PRESENT_PARAMETERS
		{
			public int cbSize;

			public int fQueue;

			public long cRefreshStart;

			public int cBuffer;

			public int fUseSourceRate;

			public UNSIGNED_RATIO rateSource;

			public int cRefreshesPerFrame;

			public DWM_SOURCE_FRAME_SAMPLING eSampling;
		}

		public struct DWM_THUMBNAIL_PROPERTIES
		{
			public int dwFlags;

			public RECT rcDestination;

			public RECT rcSource;

			public byte opacity;

			public int fVisible;

			public int fSourceClientAreaOnly;
		}

		public struct DWM_TIMING_INFO
		{
			public int cbSize;

			public UNSIGNED_RATIO rateRefresh;

			public UNSIGNED_RATIO rateCompose;

			public long long_0;

			public long cRefresh;

			public long qpcCompose;

			public long cFrame;

			public long cRefreshFrame;

			public long cRefreshConfirmed;

			public int cFlipsOutstanding;

			public long cFrameCurrent;

			public long cFramesAvailable;

			public long cFrameCleared;

			public long cFramesReceived;

			public long cFramesDisplayed;

			public long cFramesDropped;

			public long cFramesMissed;
		}

		public struct UNSIGNED_RATIO
		{
			public int uiNumerator;

			public int uiDenominator;
		}

		public struct MARGINS
		{
			public int cxLeftWidth;

			public int cxRightWidth;

			public int cyTopHeight;

			public int cyBottomHeight;

			internal static object DRZALvKIKOyUAKBeevF;

			public MARGINS(int Left, int Right, int Top, int Bottom)
			{
				cxLeftWidth = Left;
				cxRightWidth = Right;
				cyTopHeight = Top;
				cyBottomHeight = Bottom;
			}

			internal static void VEai6KK7N5tPXyHQMTk()
			{
			}

			internal static bool ds6ghWK1VZkwD5xLsgf()
			{
				return DRZALvKIKOyUAKBeevF == null;
			}
		}

		public struct WTA_OPTIONS
		{
			public uint Flags;

			public uint Mask;
		}

		public enum DWM_SOURCE_FRAME_SAMPLING
		{
			DWM_SOURCE_FRAME_SAMPLING_POINT,
			DWM_SOURCE_FRAME_SAMPLING_COVERAGE,
			DWM_SOURCE_FRAME_SAMPLING_LAST
		}

		public enum DWMNCRENDERINGPOLICY
		{
			DWMNCRP_USEWINDOWSTYLE,
			DWMNCRP_DISABLED,
			DWMNCRP_ENABLED
		}

		public enum DWMWINDOWATTRIBUTE
		{
			DWMWA_ALLOW_NCPAINT = 4,
			DWMWA_CAPTION_BUTTON_BOUNDS = 5,
			DWMWA_FLIP3D_POLICY = 8,
			DWMWA_FORCE_ICONIC_REPRESENTATION = 7,
			DWMWA_LAST = 9,
			DWMWA_NCRENDERING_ENABLED = 1,
			DWMWA_NCRENDERING_POLICY = 2,
			DWMWA_NONCLIENT_RTL_LAYOUT = 6,
			DWMWA_TRANSITIONS_FORCEDISABLED = 3
		}

		public enum WindowThemeAttributeType
		{
			WTA_NONCLIENT = 1
		}

		public const int DWM_BB_BLURREGION = 2;

		public const int DWM_BB_ENABLE = 1;

		public const int DWM_BB_TRANSITIONONMAXIMIZED = 4;

		public const string DWM_COMPOSED_EVENT_BASE_NAME = "DwmComposedEvent_";

		public const string DWM_COMPOSED_EVENT_NAME_FORMAT = "%s%d";

		public const int DWM_COMPOSED_EVENT_NAME_MAX_LENGTH = 64;

		public const int DWM_FRAME_DURATION_DEFAULT = -1;

		public const int DWM_TNP_OPACITY = 4;

		public const int DWM_TNP_RECTDESTINATION = 1;

		public const int DWM_TNP_RECTSOURCE = 2;

		public const int DWM_TNP_SOURCECLIENTAREAONLY = 16;

		public const int DWM_TNP_VISIBLE = 8;

		public const int WM_DWMCOMPOSITIONCHANGED = 798;

		public static uint WTNCA_NODRAWCAPTION;

		public static uint WTNCA_NODRAWICON;

		public static uint WTNCA_NOSYSMENU;

		public static uint WTNCA_NOMIRRORHELP;

		public static readonly bool DwmApiAvailable;

		internal static DwmApi LpYDP08vc3EmKVXNyas;

		[DllImport("dwmapi.dll")]
		public static extern int DwmDefWindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref IntPtr result);

		[DllImport("dwmapi.dll")]
		public static extern int DwmEnableComposition(int fEnable);

		[DllImport("dwmapi.dll")]
		public static extern int DwmEnableMMCSS(int fEnableMMCSS);

		[DllImport("dwmapi.dll")]
		public static extern int DwmExtendFrameIntoClientArea(IntPtr hdc, ref MARGINS marInset);

		[DllImport("dwmapi.dll")]
		public static extern int DwmGetColorizationColor(ref int pcrColorization, ref int pfOpaqueBlend);

		[DllImport("dwmapi.dll")]
		public static extern int DwmGetCompositionTimingInfo(IntPtr hwnd, ref DWM_TIMING_INFO pTimingInfo);

		[DllImport("dwmapi.dll")]
		public static extern int DwmGetWindowAttribute(IntPtr hwnd, int dwAttribute, IntPtr pvAttribute, int cbAttribute);

		[DllImport("dwmapi.dll")]
		public static extern int DwmIsCompositionEnabled(ref int pfEnabled);

		[DllImport("dwmapi.dll")]
		public static extern int DwmIsCompositionEnabled(out bool pfEnabled);

		[DllImport("dwmapi.dll")]
		public static extern int DwmModifyPreviousDxFrameDuration(IntPtr hwnd, int cRefreshes, int fRelative);

		[DllImport("dwmapi.dll")]
		public static extern int DwmQueryThumbnailSourceSize(IntPtr hThumbnail, ref Size pSize);

		[DllImport("dwmapi.dll")]
		public static extern int DwmRegisterThumbnail(IntPtr hwndDestination, IntPtr hwndSource, ref Size pMinimizedSize, ref IntPtr phThumbnailId);

		[DllImport("dwmapi.dll")]
		public static extern int DwmSetDxFrameDuration(IntPtr hwnd, int cRefreshes);

		[DllImport("dwmapi.dll")]
		public static extern int DwmSetPresentParameters(IntPtr hwnd, ref DWM_PRESENT_PARAMETERS pPresentParams);

		[DllImport("dwmapi.dll")]
		public static extern int DwmSetWindowAttribute(IntPtr hwnd, int dwAttribute, IntPtr pvAttribute, int cbAttribute);

		[DllImport("dwmapi.dll")]
		public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

		[DllImport("dwmapi.dll")]
		public static extern int DwmUnregisterThumbnail(IntPtr hThumbnailId);

		[DllImport("dwmapi.dll")]
		public static extern int DwmUpdateThumbnailProperties(IntPtr hThumbnailId, ref DWM_THUMBNAIL_PROPERTIES ptnProperties);

		[DllImport("dwmapi.dll")]
		public static extern int DwmEnableBlurBehindWindow(IntPtr hWnd, ref DWM_BLURBEHIND pBlurBehind);

		[DllImport("uxtheme.dll")]
		public static extern int SetWindowThemeAttribute(IntPtr hWnd, WindowThemeAttributeType wtype, ref WTA_OPTIONS attributes, uint size);

		static DwmApi()
		{
			WTNCA_NODRAWCAPTION = 1u;
			WTNCA_NODRAWICON = 2u;
			WTNCA_NOSYSMENU = 4u;
			WTNCA_NOMIRRORHELP = 8u;
			DwmApiAvailable = Environment.OSVersion.Version.Major >= 6;
		}

		internal static bool KT9Ao589yoXbLnC5h8r()
		{
			return LpYDP08vc3EmKVXNyas == null;
		}
	}
}

using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace hoiuclib
{
	public class cedrus__wrapper
	{
		internal delegate IntPtr pFPC_LoadOCXCodeFromMemory(IntPtr pData, uint dwSize);

		internal delegate IntPtr pFPC_LoadRegisteredOCX();

		internal delegate int pFPC_UnloadCode(IntPtr hFPC);

		internal delegate IntPtr pFPC_GetClassNameW(IntPtr hFPC);

		internal delegate int Delegate0(IntPtr hWnd, IntPtr bstrRequest, ref IntPtr bstrResponse);

		internal delegate int Delegate1(IntPtr hWnd, IntPtr strValue);

		internal delegate int pFPC_SetSoundVolume(IntPtr hFPC, int nVolume);

		internal delegate int pFPC_GetSoundVolume(IntPtr hFPC);

		internal delegate void pFPC_EnableSound(IntPtr hFPC, int bEnable);

		internal delegate int pFPC_IsSoundEnabled(IntPtr hFPC);

		internal delegate int pFPC_GetVersionEx(IntPtr hFPC, out VersionStruct version);

		internal delegate int Delegate2(IntPtr hWnd, EventsListener pEventsListener, IntPtr lParam);

		public delegate int EventsListener(IntPtr hWnd, IntPtr lParam, IntPtr pNMHDR);

		internal delegate int pFPCPutMovieUsingStream(IntPtr hWnd, out IntPtr stream);

		internal delegate int pFPCLoadMovieUsingStream(IntPtr hWnd, int layer, out IntPtr stream);

		internal delegate int pFPC_AddOnLoadExternalResourceHandlerW(IntPtr hFPC, LoadExternalResourceHandler pHandler, IntPtr lParam);

		public delegate int LoadExternalResourceHandler(IntPtr lpszURL, ref IntPtr stream, IntPtr hFPC, IntPtr lParam);

		internal delegate int pFPC_PutStandardMenu(IntPtr hWnd, int bEnable);

		internal delegate int pFPC_GetStandardMenu(IntPtr hWnd, out int bEnabled);

		internal delegate int Delegate3();

		internal delegate int pFPCIsTransparentAvailable();

		internal delegate bool pGetInstalledFlashVersionEx(out VersionStruct version);

		internal delegate IntPtr pFPC_GetAxHWND(IntPtr hWnd);

		internal delegate int pFPC_IStream_AddRef(IntPtr pStream);

		internal delegate int pFPC_IStream_Release(IntPtr pStream);

		internal delegate int pFPC_IStream_Write(IntPtr pStream, IntPtr pBuffer, int Size, out uint WrittenBytes);

		internal delegate int pFPC_IStream_SetSize(IntPtr pStream, uint Size);

		internal delegate void pFPC_SetContext(IntPtr hWnd, [MarshalAs(UnmanagedType.LPStr)] string Context);

		public struct VersionStruct
		{
			public short v0;

			public short v1;

			public short v2;

			public short v3;
		}

		internal struct STGMEDIUM
		{
			public int tymed;

			public IntPtr hGlobal;

			public IntPtr pUnkForRelease;
		}

		internal struct SECURITY_ATTRIBUTES
		{
			public int nLength;

			public IntPtr lpSecurityDescriptor;

			public int bInheritHandle;
		}

		internal struct BINDINFO
		{
			public int cbSize;

			public IntPtr szExtraInfo;

			public STGMEDIUM stgmedData;

			public int grfBindInfoF;

			public int dwBindVerb;

			public IntPtr szCustomVerb;

			public int cbstgmedData;

			public int dwOptions;

			public int dwOptionsFlags;

			public int dwCodePage;

			public SECURITY_ATTRIBUTES securityAttributes;

			public Guid iid;

			public IntPtr pUnk;

			public int dwReserved;
		}

		internal delegate int GetBindInfoHandler(IntPtr hFPC, ref int grfBINDF, ref BINDINFO pbindinfo, IntPtr lParam);

		internal delegate int pFPC_AddGetBindInfoHandler(IntPtr hFPC, GetBindInfoHandler pGetBindInfoHandler, IntPtr lParam);

		internal delegate int pFPC_RemoveGetBindInfoHandler(IntPtr hFPC, int dwCookie);

		internal delegate IntPtr pFPC_StartMinimizeMemoryTimer(int nInterval);

		internal delegate void pFPC_StopMinimizeMemoryTimer(IntPtr nTimerId);

		internal delegate void pFPC_StopFPCMinimizeMemoryTimer(IntPtr hFPC);

		internal delegate int pFPC_SetZoomRect(IntPtr hWnd, int left, int top, int right, int bottom);

		internal delegate int pFPC_Zoom(IntPtr hWnd, int factor);

		internal delegate int pFPC_Pan(IntPtr hWnd, int x, int y, int mode);

		internal delegate int pFPC_Play(IntPtr hWnd);

		internal delegate int pFPC_Stop(IntPtr hWnd);

		internal delegate int pFPC_Back(IntPtr hWnd);

		internal delegate int pFPC_Forward(IntPtr hWnd);

		internal delegate int pFPC_Rewind(IntPtr hWnd);

		internal delegate int pFPC_StopPlay(IntPtr hWnd);

		internal delegate int pFPC_GotoFrame(IntPtr hWnd, int FrameNum);

		internal delegate int pFPC_CurrentFrame(IntPtr hWnd, ref int Result);

		internal delegate int pFPC_IsPlaying(IntPtr hWnd, ref int Result);

		internal delegate int pFPC_PercentLoaded(IntPtr hWnd, ref int Result);

		internal delegate int pFPC_FrameLoaded(IntPtr hWnd, int FrameNum, ref int Result);

		internal delegate int pFPC_FlashVersion(IntPtr hWnd, ref int Result);

		internal delegate int pFPC_LoadMovie(IntPtr hWnd, int layer, string url);

		internal delegate int pFPC_TGotoFrame(IntPtr hWnd, string target, int FrameNum);

		internal delegate int pFPC_TGotoLabel(IntPtr hWnd, string target, string label);

		internal delegate int pFPC_TCurrentFrame(IntPtr hWnd, string target, ref int Result);

		internal delegate int pFPC_TCurrentLabel32(IntPtr hWnd, string target, int pBuffer, ref int dwSize);

		internal delegate int pFPC_TCurrentLabel64(IntPtr hWnd, string target, long pBuffer, ref int dwSize);

		internal delegate int pFPC_TPlay(IntPtr hWnd, string target);

		internal delegate int pFPC_TStopPlay(IntPtr hWnd, string target);

		internal delegate int pFPC_SetVariable(IntPtr hWnd, string name, string value);

		internal delegate int pFPC_GetVariable32(IntPtr hWnd, string name, int pBuffer, ref int dwSize);

		internal delegate int pFPC_GetVariable64(IntPtr hWnd, string name, long pBuffer, ref int dwSize);

		internal delegate int pFPC_TSetProperty(IntPtr hWnd, string target, int property, string value);

		internal delegate int pFPC_TGetProperty32(IntPtr hWnd, string target, int property, int pBuffer, ref int dwSize);

		internal delegate int pFPC_TGetProperty64(IntPtr hWnd, string target, int property, long pBuffer, ref int dwSize);

		internal delegate int pFPC_TCallFrame(IntPtr hWnd, string target, int FrameNum);

		internal delegate int pFPC_TCallLabel(IntPtr hWnd, string target, string label);

		internal delegate int pFPC_TSetPropertyNum(IntPtr hWnd, string target, int property, double value);

		internal delegate int pFPC_TGetPropertyNum(IntPtr hWnd, string target, int property, ref double Result);

		internal delegate int pFPC_TGetPropertyAsNumber(IntPtr hWnd, string target, int property, ref double Result);

		internal delegate int pFPC_GetReadyState(IntPtr hWnd, ref int Value);

		internal delegate int pFPC_GetTotalFrames(IntPtr hWnd, ref int Value);

		internal delegate int pFPC_PutPlaying(IntPtr hWnd, int Value);

		internal delegate int pFPC_GetPlaying(IntPtr hWnd, ref int Value);

		internal delegate int pFPC_PutQuality(IntPtr hWnd, int Value);

		internal delegate int pFPC_GetQuality(IntPtr hWnd, ref int Value);

		internal delegate int pFPC_PutScaleMode(IntPtr hWnd, int Value);

		internal delegate int pFPC_GetScaleMode(IntPtr hWnd, ref int Value);

		internal delegate int pFPC_PutAlignMode(IntPtr hWnd, int Value);

		internal delegate int pFPC_GetAlignMode(IntPtr hWnd, ref int Value);

		internal delegate int pFPC_PutBackgroundColor(IntPtr hWnd, int Value);

		internal delegate int pFPC_GetBackgroundColor(IntPtr hWnd, ref int Value);

		internal delegate int pFPC_PutLoop(IntPtr hWnd, int Value);

		internal delegate int pFPC_GetLoop(IntPtr hWnd, ref int Value);

		internal delegate int pFPC_GetMovie(IntPtr hWnd, IntPtr pBuffer, ref int dwSize);

		internal delegate int pFPC_PutMovie(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string Movie);

		internal delegate int pFPC_PutFrameNum(IntPtr hWnd, int Value);

		internal delegate int pFPC_GetFrameNum(IntPtr hWnd, ref int Value);

		internal delegate int pFPC_GetWMode(IntPtr hWnd, IntPtr pBuffer, ref int dwSize);

		internal delegate int pFPC_PutWMode(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string WMode);

		internal delegate int pFPC_GetSAlign(IntPtr hWnd, IntPtr pBuffer, ref int dwSize);

		internal delegate int pFPC_PutSAlign(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string SAlign);

		internal delegate int pFPC_PutMenu(IntPtr hWnd, int Value);

		internal delegate int pFPC_GetMenu(IntPtr hWnd, ref int Value);

		internal delegate int pFPC_GetBase(IntPtr hWnd, IntPtr pBuffer, ref int dwSize);

		internal delegate int pFPC_PutBase(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string Base);

		internal delegate int pFPC_GetScale(IntPtr hWnd, IntPtr pBuffer, ref int dwSize);

		internal delegate int pFPC_PutScale(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string Scale);

		internal delegate int pFPC_PutDeviceFont(IntPtr hWnd, int Value);

		internal delegate int pFPC_GetDeviceFont(IntPtr hWnd, ref int Value);

		internal delegate int pFPC_PutEmbedMovie(IntPtr hWnd, int Value);

		internal delegate int pFPC_GetEmbedMovie(IntPtr hWnd, ref int Value);

		internal delegate int pFPC_GetBGColor(IntPtr hWnd, IntPtr pBuffer, ref int dwSize);

		internal delegate int pFPC_PutBGColor(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string BGColor);

		internal delegate int pFPC_GetQuality2(IntPtr hWnd, IntPtr pBuffer, ref int dwSize);

		internal delegate int pFPC_PutQuality2(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string Quality2);

		internal delegate int pFPC_GetSWRemote(IntPtr hWnd, IntPtr pBuffer, ref int dwSize);

		internal delegate int pFPC_PutSWRemote(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string SWRemote);

		internal delegate int pFPC_GetStacking(IntPtr hWnd, IntPtr pBuffer, ref int dwSize);

		internal delegate int pFPC_PutStacking(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string Stacking);

		internal delegate int pFPC_GetFlashVars(IntPtr hWnd, IntPtr pBuffer, ref int dwSize);

		internal delegate int pFPC_PutFlashVars(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string FlashVars);

		internal delegate int pFPC_GetAllowScriptAccess(IntPtr hWnd, IntPtr pBuffer, ref int dwSize);

		internal delegate int pFPC_PutAllowScriptAccess(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string AllowScriptAccess);

		internal delegate int pFPC_GetMovieData(IntPtr hWnd, IntPtr pBuffer, ref int dwSize);

		internal delegate int pFPC_PutMovieData(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string MovieData);

		internal delegate int pFPC_IsFullScreenEnabled(IntPtr hWnd);

		internal delegate int pFPC_EnableFullScreen(IntPtr hWnd, int bEnable);

		internal struct NMHDR
		{
			public IntPtr hWndFrom;

			public UIntPtr idFrom;

			public uint code;
		}

		internal struct SFPCOnReadyStateChangeInfoStruct
		{
			public NMHDR hdr;

			public int newState;
		}

		internal struct SFPCOnProgressInfoStruct
		{
			public NMHDR hdr;

			public int percentDone;
		}

		internal struct SFPCFSCommandInfoStructW
		{
			public NMHDR hdr;

			public IntPtr command;

			public IntPtr args;
		}

		internal struct SFPCFlashCallInfoStructW
		{
			public NMHDR hdr;

			public IntPtr request;
		}

		internal struct SFPCLoadExternalResourceExW
		{
			public NMHDR hdr;

			public IntPtr lpszRelativePath;

			public IntPtr lpStream;

			public int bHandled;
		}

		internal struct SFPCLoadExternalResourceW
		{
			public NMHDR hdr;

			public IntPtr lpszRelativePath;
		}

		internal struct Struct0
		{
			public IntPtr hBitmap;
		}

		internal struct Struct1
		{
			public int Value;
		}

		internal struct Struct2
		{
			public int Value;
		}

		internal struct RECT
		{
			public int left;

			public int top;

			public int right;

			public int bottom;
		}

		internal struct SFPCNUpdateRect
		{
			public NMHDR hdr;

			public RECT rc;
		}

		internal struct SFPCNPaint
		{
			public NMHDR hdr;

			public IntPtr pPixels;
		}

		internal struct SFPCNPaintStage
		{
			public NMHDR hdr;

			public int dwStage;

			public IntPtr hdc;
		}

		internal struct BITMAP
		{
			public int bmType;

			public int bmWidth;

			public int bmHeight;

			public int bmWidthBytes;

			public short bmPlanes;

			public short bmBitsPixel;

			public int bmBits;
		}

		private static LibraryMode libraryMode_0;

		private static IntPtr intptr_0;

		private static Hashtable hashtable_0;

		private static bool bool_0;

		private static bool bool_1;

		internal const int VARIANT_TRUE = -1;

		internal const int VARIANT_FALSE = 0;

		internal const int FPCS_TRANSPARENT = 1;

		internal const int FPCN_ONREADYSTATECHANGE = 4864;

		internal const int FPCN_ONPROGRESS = 4863;

		internal const int FPCN_FSCOMMANDW = 4861;

		internal const int FPCN_FLASHCALLW = 5114;

		internal const int FPCN_LOADEXTERNALRESOURCEW = 5116;

		internal const int FPCN_LOADEXTERNALRESOURCEEXW = 5112;

		internal const int FPCM_GET_FRAME_BITMAP = 5124;

		internal const int FPCM_PUT_OVERALL_OPAQUE = 5128;

		internal const int FPCM_GET_OVERALL_OPAQUE = 5129;

		internal const int FPCN_PAINT = 5111;

		internal const int FPCN_UPDATE_RECT = 5110;

		internal const int DEF_F_IN_BOX__PREPAINT_STAGE = 0;

		internal const int DEF_F_IN_BOX__AFTERPAINT_STAGE = 1;

		internal const int FPCN_PAINT_STAGE = 5109;

		internal static cedrus__wrapper YLByHe1ySL36RAJZRai;

		private static byte[] smethod_0(Stream stream_0)
		{
			byte[] array = new byte[stream_0.Length];
			stream_0.Read(array, 0, array.Length);
			return array;
		}

		private static bool smethod_1()
		{
			if (!bool_1)
			{
				string text = ((!bool_0) ? "gunhoiuc.ghu" : "gunhoiuc.ghu");
				if (!File.Exists(Path.Combine(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location), text)))
				{
					byte[] rawData = smethod_0(Assembly.GetExecutingAssembly().GetManifestResourceStream("LauncherGHU.cedrus." + text));
					if (!bool_0)
					{
						intptr_0 = MapDll32.Internal_MapDll(rawData);
						MapDll32.Internal_SaveExports(intptr_0, hashtable_0);
					}
					else
					{
						intptr_0 = MapDll64.Internal_MapDll(rawData);
						MapDll64.Internal_SaveExports(intptr_0, hashtable_0);
					}
					bool_1 = true;
					libraryMode_0 = LibraryMode.UseStatic;
				}
				else
				{
					libraryMode_0 = LibraryMode.UseDll;
					bool_1 = true;
				}
				return bool_1;
			}
			return true;
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_LoadOCXCodeFromMemory")]
		private static extern IntPtr FPC_LoadOCXCodeFromMemory_1(IntPtr intptr_1, uint uint_0);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_LoadOCXCodeFromMemory")]
		private static extern IntPtr FPC_LoadOCXCodeFromMemory_2(IntPtr intptr_1, uint uint_0);

		public static IntPtr FPC_LoadOCXCodeFromMemory(IntPtr pData, uint dwSize)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return bool_0 ? FPC_LoadOCXCodeFromMemory_2(pData, dwSize) : FPC_LoadOCXCodeFromMemory_1(pData, dwSize);
			}
			return ((pFPC_LoadOCXCodeFromMemory)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_LoadOCXCodeFromMemory"], typeof(pFPC_LoadOCXCodeFromMemory)))(pData, dwSize);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_LoadRegisteredOCX")]
		private static extern IntPtr FPC_LoadRegisteredOCX_1();

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_LoadRegisteredOCX")]
		private static extern IntPtr FPC_LoadRegisteredOCX_2();

		public static IntPtr FPC_LoadRegisteredOCX()
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return (!bool_0) ? FPC_LoadRegisteredOCX_1() : FPC_LoadRegisteredOCX_2();
			}
			return ((pFPC_LoadRegisteredOCX)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_LoadRegisteredOCX"], typeof(pFPC_LoadRegisteredOCX)))();
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_UnloadCode")]
		private static extern int FPC_UnloadCode_1(IntPtr intptr_1);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_UnloadCode")]
		private static extern int FPC_UnloadCode_2(IntPtr intptr_1);

		public static int FPC_UnloadCode(IntPtr hFPC)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return (!bool_0) ? FPC_UnloadCode_1(hFPC) : FPC_UnloadCode_2(hFPC);
			}
			return ((pFPC_UnloadCode)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_UnloadCode"], typeof(pFPC_UnloadCode)))(hFPC);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_GetClassNameW")]
		private static extern IntPtr FPC_GetClassNameW_1(IntPtr intptr_1);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_GetClassNameW")]
		private static extern IntPtr FPC_GetClassNameW_2(IntPtr intptr_1);

		public static IntPtr FPC_GetClassNameW(IntPtr hFPC)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_GetClassNameW)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_GetClassNameW"], typeof(pFPC_GetClassNameW)))(hFPC);
			}
			return (!bool_0) ? FPC_GetClassNameW_1(hFPC) : FPC_GetClassNameW_2(hFPC);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPCCallFunctionBSTR")]
		private static extern int FPCCallFunctionBSTR_1(IntPtr intptr_1, IntPtr intptr_2, ref IntPtr intptr_3);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPCCallFunctionBSTR")]
		private static extern int FPCCallFunctionBSTR_2(IntPtr intptr_1, IntPtr intptr_2, ref IntPtr intptr_3);

		public static int FPCCallFunctionBSTR(IntPtr hWnd, IntPtr bstrRequest, ref IntPtr bstrResponse)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return bool_0 ? FPCCallFunctionBSTR_2(hWnd, bstrRequest, ref bstrResponse) : FPCCallFunctionBSTR_1(hWnd, bstrRequest, ref bstrResponse);
			}
			return ((Delegate0)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPCCallFunctionBSTR"], typeof(Delegate0)))(hWnd, bstrRequest, ref bstrResponse);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall)]
		private static extern int FPCSetReturnValueW(IntPtr intptr_1, IntPtr intptr_2);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPCSetReturnValueW")]
		private static extern int FPCSetReturnValueW_1(IntPtr intptr_1, IntPtr intptr_2);

		public static int FPCSetReturnValue(IntPtr hWnd, IntPtr strValue)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return bool_0 ? FPCSetReturnValueW_1(hWnd, strValue) : FPCSetReturnValueW(hWnd, strValue);
			}
			return ((Delegate1)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPCSetReturnValueW"], typeof(Delegate1)))(hWnd, strValue);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_SetSoundVolume")]
		private static extern int FPC_SetSoundVolume_1(IntPtr intptr_1, int int_0);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_SetSoundVolume")]
		private static extern int FPC_SetSoundVolume_2(IntPtr intptr_1, int int_0);

		public static int FPC_SetSoundVolume(IntPtr hFPC, int nVolume)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_SetSoundVolume)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_SetSoundVolume"], typeof(pFPC_SetSoundVolume)))(hFPC, nVolume);
			}
			return (!bool_0) ? FPC_SetSoundVolume_1(hFPC, nVolume) : FPC_SetSoundVolume_2(hFPC, nVolume);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_GetSoundVolume")]
		private static extern int FPC_GetSoundVolume_1(IntPtr intptr_1);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_GetSoundVolume")]
		private static extern int FPC_GetSoundVolume_2(IntPtr intptr_1);

		public static int FPC_GetSoundVolume(IntPtr hFPC)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_GetSoundVolume)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_GetSoundVolume"], typeof(pFPC_GetSoundVolume)))(hFPC);
			}
			return (!bool_0) ? FPC_GetSoundVolume_1(hFPC) : FPC_GetSoundVolume_2(hFPC);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_EnableSound")]
		private static extern void FPC_EnableSound_1(IntPtr intptr_1, int int_0);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_EnableSound")]
		private static extern void FPC_EnableSound_2(IntPtr intptr_1, int int_0);

		public static void FPC_EnableSound(IntPtr hFPC, int bEnable)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				if (!bool_0)
				{
					FPC_EnableSound_1(hFPC, bEnable);
				}
				else
				{
					FPC_EnableSound_2(hFPC, bEnable);
				}
			}
			else
			{
				((pFPC_EnableSound)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_EnableSound"], typeof(pFPC_EnableSound)))(hFPC, bEnable);
			}
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_IsSoundEnabled")]
		private static extern int FPC_IsSoundEnabled_1(IntPtr intptr_1);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_IsSoundEnabled")]
		private static extern int FPC_IsSoundEnabled_2(IntPtr intptr_1);

		public static int FPC_IsSoundEnabled(IntPtr hFPC)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_IsSoundEnabled)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_IsSoundEnabled"], typeof(pFPC_IsSoundEnabled)))(hFPC);
			}
			return bool_0 ? FPC_IsSoundEnabled_2(hFPC) : FPC_IsSoundEnabled_1(hFPC);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_GetVersionEx")]
		private static extern int FPC_GetVersionEx_1(IntPtr intptr_1, out VersionStruct versionStruct_0);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_GetVersionEx")]
		private static extern int FPC_GetVersionEx_2(IntPtr intptr_1, out VersionStruct versionStruct_0);

		public static int FPC_GetVersionEx(IntPtr hFPC, out VersionStruct version)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return bool_0 ? FPC_GetVersionEx_2(hFPC, out version) : FPC_GetVersionEx_1(hFPC, out version);
			}
			return ((pFPC_GetVersionEx)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_GetVersionEx"], typeof(pFPC_GetVersionEx)))(hFPC, out version);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPCSetEventListener")]
		private static extern int FPCSetEventListener_1(IntPtr intptr_1, EventsListener eventsListener_0, IntPtr intptr_2);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPCSetEventListener")]
		private static extern int FPCSetEventListener_2(IntPtr intptr_1, EventsListener eventsListener_0, IntPtr intptr_2);

		public static int FPCSetEventListener(IntPtr hWnd, EventsListener pEventsListener, IntPtr lParam)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((Delegate2)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPCSetEventListener"], typeof(Delegate2)))(hWnd, pEventsListener, lParam);
			}
			return bool_0 ? FPCSetEventListener_2(hWnd, pEventsListener, lParam) : FPCSetEventListener_1(hWnd, pEventsListener, lParam);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPCPutMovieUsingStream")]
		private static extern int FPCPutMovieUsingStream_1(IntPtr intptr_1, out IntPtr intptr_2);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPCPutMovieUsingStream")]
		private static extern int FPCPutMovieUsingStream_2(IntPtr intptr_1, out IntPtr intptr_2);

		public static int FPCPutMovieUsingStream(IntPtr hWnd, out IntPtr stream)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPCPutMovieUsingStream)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPCPutMovieUsingStream"], typeof(pFPCPutMovieUsingStream)))(hWnd, out stream);
			}
			return bool_0 ? FPCPutMovieUsingStream_2(hWnd, out stream) : FPCPutMovieUsingStream_1(hWnd, out stream);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPCLoadMovieUsingStream")]
		private static extern int FPCLoadMovieUsingStream_1(IntPtr intptr_1, int int_0, out IntPtr intptr_2);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPCLoadMovieUsingStream")]
		private static extern int FPCLoadMovieUsingStream_2(IntPtr intptr_1, int int_0, out IntPtr intptr_2);

		public static int FPCLoadMovieUsingStream(IntPtr hWnd, int layer, out IntPtr stream)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return (!bool_0) ? FPCLoadMovieUsingStream_1(hWnd, layer, out stream) : FPCLoadMovieUsingStream_2(hWnd, layer, out stream);
			}
			return ((pFPCLoadMovieUsingStream)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPCLoadMovieUsingStream"], typeof(pFPCLoadMovieUsingStream)))(hWnd, layer, out stream);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_AddOnLoadExternalResourceHandlerW")]
		private static extern int FPC_AddOnLoadExternalResourceHandlerW_1(IntPtr intptr_1, LoadExternalResourceHandler loadExternalResourceHandler_0, IntPtr intptr_2);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_AddOnLoadExternalResourceHandlerW")]
		private static extern int FPC_AddOnLoadExternalResourceHandlerW_2(IntPtr intptr_1, LoadExternalResourceHandler loadExternalResourceHandler_0, IntPtr intptr_2);

		public static int FPC_AddOnLoadExternalResourceHandlerW(IntPtr hFPC, LoadExternalResourceHandler pHandler, IntPtr lParam)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return bool_0 ? FPC_AddOnLoadExternalResourceHandlerW_2(hFPC, pHandler, lParam) : FPC_AddOnLoadExternalResourceHandlerW_1(hFPC, pHandler, lParam);
			}
			return ((pFPC_AddOnLoadExternalResourceHandlerW)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_AddOnLoadExternalResourceHandlerW"], typeof(pFPC_AddOnLoadExternalResourceHandlerW)))(hFPC, pHandler, lParam);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_PutStandardMenu")]
		private static extern int FPC_PutStandardMenu_1(IntPtr intptr_1, int int_0);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_PutStandardMenu")]
		private static extern int FPC_PutStandardMenu_2(IntPtr intptr_1, int int_0);

		public static int FPC_PutStandardMenu(IntPtr hWnd, int bEnable)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_PutStandardMenu)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_PutStandardMenu"], typeof(pFPC_PutStandardMenu)))(hWnd, bEnable);
			}
			return (!bool_0) ? FPC_PutStandardMenu_1(hWnd, bEnable) : FPC_PutStandardMenu_2(hWnd, bEnable);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_GetStandardMenu")]
		private static extern int FPC_GetStandardMenu_1(IntPtr intptr_1, out int int_0);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_GetStandardMenu")]
		private static extern int FPC_GetStandardMenu_2(IntPtr intptr_1, out int int_0);

		public static int FPC_GetStandardMenu(IntPtr hWnd, out int bEnabled)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_GetStandardMenu)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_GetStandardMenu"], typeof(pFPC_GetStandardMenu)))(hWnd, out bEnabled);
			}
			return (!bool_0) ? FPC_GetStandardMenu_1(hWnd, out bEnabled) : FPC_GetStandardMenu_2(hWnd, out bEnabled);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPCIsFlashInstalled")]
		private static extern int FPCIsFlashInstalled_1();

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPCIsFlashInstalled")]
		private static extern int FPCIsFlashInstalled_2();

		public static int FPCIsFlashInstalled()
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((Delegate3)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPCIsFlashInstalled"], typeof(Delegate3)))();
			}
			return bool_0 ? FPCIsFlashInstalled_2() : FPCIsFlashInstalled_1();
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPCIsTransparentAvailable")]
		private static extern int FPCIsTransparentAvailable_1();

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPCIsTransparentAvailable")]
		private static extern int FPCIsTransparentAvailable_2();

		public static int FPCIsTransparentAvailable()
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return (!bool_0) ? FPCIsTransparentAvailable_1() : FPCIsTransparentAvailable_2();
			}
			return ((pFPCIsTransparentAvailable)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPCIsTransparentAvailable"], typeof(pFPCIsTransparentAvailable)))();
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "GetInstalledFlashVersionEx")]
		private static extern bool GetInstalledFlashVersionEx_1(out VersionStruct versionStruct_0);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "GetInstalledFlashVersionEx")]
		private static extern bool GetInstalledFlashVersionEx_2(out VersionStruct versionStruct_0);

		public static bool GetInstalledFlashVersionEx(out VersionStruct version)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return bool_0 ? GetInstalledFlashVersionEx_2(out version) : GetInstalledFlashVersionEx_1(out version);
			}
			return ((pGetInstalledFlashVersionEx)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["GetInstalledFlashVersionEx"], typeof(pGetInstalledFlashVersionEx)))(out version);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_GetAxHWND")]
		private static extern IntPtr FPC_GetAxHWND_1(IntPtr intptr_1);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_GetAxHWND")]
		private static extern IntPtr FPC_GetAxHWND_2(IntPtr intptr_1);

		public static IntPtr FPC_GetAxHWND(IntPtr hWnd)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_GetAxHWND)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_GetAxHWND"], typeof(pFPC_GetAxHWND)))(hWnd);
			}
			return bool_0 ? FPC_GetAxHWND_2(hWnd) : FPC_GetAxHWND_1(hWnd);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_IStream_AddRef")]
		private static extern int FPC_IStream_AddRef_1(IntPtr intptr_1);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_IStream_AddRef")]
		private static extern int FPC_IStream_AddRef_2(IntPtr intptr_1);

		public static int FPC_IStream_AddRef(IntPtr pStream)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return bool_0 ? FPC_IStream_AddRef_2(pStream) : FPC_IStream_AddRef_1(pStream);
			}
			return ((pFPC_IStream_AddRef)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_IStream_AddRef"], typeof(pFPC_IStream_AddRef)))(pStream);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_IStream_Release")]
		private static extern int FPC_IStream_Release_1(IntPtr intptr_1);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_IStream_Release")]
		private static extern int FPC_IStream_Release_2(IntPtr intptr_1);

		public static int FPC_IStream_Release(IntPtr pStream)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return bool_0 ? FPC_IStream_Release_2(pStream) : FPC_IStream_Release_1(pStream);
			}
			return ((pFPC_IStream_Release)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_IStream_Release"], typeof(pFPC_IStream_Release)))(pStream);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_IStream_Write")]
		private static extern int FPC_IStream_Write_1(IntPtr intptr_1, IntPtr intptr_2, int int_0, out uint uint_0);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_IStream_Write")]
		private static extern int FPC_IStream_Write_2(IntPtr intptr_1, IntPtr intptr_2, int int_0, out uint uint_0);

		public static int FPC_IStream_Write(IntPtr pStream, IntPtr pBuffer, int Size, out uint WrittenBytes)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return bool_0 ? FPC_IStream_Write_2(pStream, pBuffer, Size, out WrittenBytes) : FPC_IStream_Write_1(pStream, pBuffer, Size, out WrittenBytes);
			}
			return ((pFPC_IStream_Write)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_IStream_Write"], typeof(pFPC_IStream_Write)))(pStream, pBuffer, Size, out WrittenBytes);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_IStream_SetSize")]
		private static extern int FPC_IStream_SetSize_1(IntPtr intptr_1, uint uint_0);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_IStream_SetSize")]
		private static extern int FPC_IStream_SetSize_2(IntPtr intptr_1, uint uint_0);

		public static int FPC_IStream_SetSize(IntPtr pStream, uint Size)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return bool_0 ? FPC_IStream_SetSize_2(pStream, Size) : FPC_IStream_SetSize_1(pStream, Size);
			}
			return ((pFPC_IStream_SetSize)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_IStream_SetSize"], typeof(pFPC_IStream_SetSize)))(pStream, Size);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "FPC_SetContext")]
		private static extern void FPC_SetContext_1(IntPtr intptr_1, [MarshalAs(UnmanagedType.LPStr)] string string_0);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi, EntryPoint = "FPC_SetContext")]
		private static extern void FPC_SetContext_2(IntPtr intptr_1, [MarshalAs(UnmanagedType.LPStr)] string string_0);

		public static void FPC_SetContext(IntPtr hWnd, [MarshalAs(UnmanagedType.LPStr)] string Context)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				((pFPC_SetContext)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_SetContext"], typeof(pFPC_SetContext)))(hWnd, Context);
			}
			else if (bool_0)
			{
				FPC_SetContext_2(hWnd, Context);
			}
			else
			{
				FPC_SetContext_1(hWnd, Context);
			}
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_AddGetBindInfoHandler")]
		internal static extern int FPC_AddGetBindInfoHandler_1(IntPtr hFPC, GetBindInfoHandler pGetBindInfoHandler, IntPtr lParam);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_AddGetBindInfoHandler")]
		internal static extern int FPC_AddGetBindInfoHandler_2(IntPtr hFPC, GetBindInfoHandler pGetBindInfoHandler, IntPtr lParam);

		internal static int FPC_AddGetBindInfoHandler(IntPtr hFPC, GetBindInfoHandler pGetBindInfoHandler, IntPtr lParam)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return (!bool_0) ? FPC_AddGetBindInfoHandler_1(hFPC, pGetBindInfoHandler, lParam) : FPC_AddGetBindInfoHandler_2(hFPC, pGetBindInfoHandler, lParam);
			}
			return ((pFPC_AddGetBindInfoHandler)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_AddGetBindInfoHandler"], typeof(pFPC_AddGetBindInfoHandler)))(hFPC, pGetBindInfoHandler, lParam);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_RemoveGetBindInfoHandler")]
		internal static extern int FPC_RemoveGetBindInfoHandler_1(IntPtr hFPC, int dwCookie);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_RemoveGetBindInfoHandler")]
		internal static extern int FPC_RemoveGetBindInfoHandler_2(IntPtr hFPC, int dwCookie);

		internal static int FPC_RemoveGetBindInfoHandler(IntPtr hFPC, int dwCookie)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_RemoveGetBindInfoHandler)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_RemoveGetBindInfoHandler"], typeof(pFPC_RemoveGetBindInfoHandler)))(hFPC, dwCookie);
			}
			return (!bool_0) ? FPC_RemoveGetBindInfoHandler_1(hFPC, dwCookie) : FPC_RemoveGetBindInfoHandler_2(hFPC, dwCookie);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_StartMinimizeMemoryTimer")]
		internal static extern IntPtr FPC_StartMinimizeMemoryTimer_1(int nInterval);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_StartMinimizeMemoryTimer")]
		internal static extern IntPtr FPC_StartMinimizeMemoryTimer_2(int nInterval);

		internal static IntPtr FPC_StartMinimizeMemoryTimer(int nInterval)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return bool_0 ? FPC_StartMinimizeMemoryTimer_2(nInterval) : FPC_StartMinimizeMemoryTimer_1(nInterval);
			}
			return ((pFPC_StartMinimizeMemoryTimer)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_StartMinimizeMemoryTimer"], typeof(pFPC_StartMinimizeMemoryTimer)))(nInterval);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_StopMinimizeMemoryTimer")]
		internal static extern void FPC_StopMinimizeMemoryTimer_1(IntPtr nTimerId);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_StopMinimizeMemoryTimer")]
		internal static extern void FPC_StopMinimizeMemoryTimer_2(IntPtr nTimerId);

		internal static void FPC_StopMinimizeMemoryTimer(IntPtr nTimerId)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				if (bool_0)
				{
					FPC_StopMinimizeMemoryTimer_2(nTimerId);
				}
				else
				{
					FPC_StopMinimizeMemoryTimer_1(nTimerId);
				}
			}
			else
			{
				((pFPC_StopMinimizeMemoryTimer)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_StopMinimizeMemoryTimer"], typeof(pFPC_StopMinimizeMemoryTimer)))(nTimerId);
			}
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_StopFPCMinimizeMemoryTimer")]
		internal static extern void FPC_StopFPCMinimizeMemoryTimer_1(IntPtr hFPC);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_StopFPCMinimizeMemoryTimer")]
		internal static extern void FPC_StopFPCMinimizeMemoryTimer_2(IntPtr hFPC);

		internal static void FPC_StopFPCMinimizeMemoryTimer(IntPtr hFPC)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				((pFPC_StopFPCMinimizeMemoryTimer)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_StopFPCMinimizeMemoryTimer"], typeof(pFPC_StopFPCMinimizeMemoryTimer)))(hFPC);
			}
			else if (!bool_0)
			{
				FPC_StopFPCMinimizeMemoryTimer_1(hFPC);
			}
			else
			{
				FPC_StopFPCMinimizeMemoryTimer_2(hFPC);
			}
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_SetZoomRect")]
		internal static extern int FPC_SetZoomRect_1(IntPtr hWnd, int left, int top, int right, int bottom);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_SetZoomRect")]
		internal static extern int FPC_SetZoomRect_2(IntPtr hWnd, int left, int top, int right, int bottom);

		internal static int FPC_SetZoomRect(IntPtr hWnd, int left, int top, int right, int bottom)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return bool_0 ? FPC_SetZoomRect_2(hWnd, left, top, right, bottom) : FPC_SetZoomRect_1(hWnd, left, top, right, bottom);
			}
			return ((pFPC_SetZoomRect)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_SetZoomRect"], typeof(pFPC_SetZoomRect)))(hWnd, left, top, right, bottom);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_Zoom")]
		internal static extern int FPC_Zoom_1(IntPtr hWnd, int factor);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_Zoom")]
		internal static extern int FPC_Zoom_2(IntPtr hWnd, int factor);

		internal static int FPC_Zoom(IntPtr hWnd, int factor)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return (!bool_0) ? FPC_Zoom_1(hWnd, factor) : FPC_Zoom_2(hWnd, factor);
			}
			return ((pFPC_Zoom)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_Zoom"], typeof(pFPC_Zoom)))(hWnd, factor);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_Pan")]
		internal static extern int FPC_Pan_1(IntPtr hWnd, int x, int y, int mode);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_Pan")]
		internal static extern int FPC_Pan_2(IntPtr hWnd, int x, int y, int mode);

		internal static int FPC_Pan(IntPtr hWnd, int x, int y, int mode)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_Pan)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_Pan"], typeof(pFPC_Pan)))(hWnd, x, y, mode);
			}
			return (!bool_0) ? FPC_Pan_1(hWnd, x, y, mode) : FPC_Pan_2(hWnd, x, y, mode);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_Play")]
		internal static extern int FPC_Play_1(IntPtr hWnd);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_Play")]
		internal static extern int FPC_Play_2(IntPtr hWnd);

		internal static int FPC_Play(IntPtr hWnd)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return (!bool_0) ? FPC_Play_1(hWnd) : FPC_Play_2(hWnd);
			}
			return ((pFPC_Play)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_Play"], typeof(pFPC_Play)))(hWnd);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_Stop")]
		internal static extern int FPC_Stop_1(IntPtr hWnd);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_Stop")]
		internal static extern int FPC_Stop_2(IntPtr hWnd);

		internal static int FPC_Stop(IntPtr hWnd)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_Stop)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_Stop"], typeof(pFPC_Stop)))(hWnd);
			}
			return bool_0 ? FPC_Stop_2(hWnd) : FPC_Stop_1(hWnd);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_Back")]
		internal static extern int FPC_Back_1(IntPtr hWnd);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_Back")]
		internal static extern int FPC_Back_2(IntPtr hWnd);

		internal static int FPC_Back(IntPtr hWnd)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_Back)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_Back"], typeof(pFPC_Back)))(hWnd);
			}
			return bool_0 ? FPC_Back_2(hWnd) : FPC_Back_1(hWnd);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_Forward")]
		internal static extern int FPC_Forward_1(IntPtr hWnd);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_Forward")]
		internal static extern int FPC_Forward_2(IntPtr hWnd);

		internal static int FPC_Forward(IntPtr hWnd)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return bool_0 ? FPC_Forward_2(hWnd) : FPC_Forward_1(hWnd);
			}
			return ((pFPC_Forward)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_Forward"], typeof(pFPC_Forward)))(hWnd);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_Rewind")]
		internal static extern int FPC_Rewind_1(IntPtr hWnd);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_Rewind")]
		internal static extern int FPC_Rewind_2(IntPtr hWnd);

		internal static int FPC_Rewind(IntPtr hWnd)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return (!bool_0) ? FPC_Rewind_1(hWnd) : FPC_Rewind_2(hWnd);
			}
			return ((pFPC_Rewind)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_Rewind"], typeof(pFPC_Rewind)))(hWnd);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_StopPlay")]
		internal static extern int FPC_StopPlay_1(IntPtr hWnd);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_StopPlay")]
		internal static extern int FPC_StopPlay_2(IntPtr hWnd);

		internal static int FPC_StopPlay(IntPtr hWnd)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_StopPlay)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_StopPlay"], typeof(pFPC_StopPlay)))(hWnd);
			}
			return bool_0 ? FPC_StopPlay_2(hWnd) : FPC_StopPlay_1(hWnd);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_GotoFrame")]
		internal static extern int FPC_GotoFrame_1(IntPtr hWnd, int FrameNum);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_GotoFrame")]
		internal static extern int FPC_GotoFrame_2(IntPtr hWnd, int FrameNum);

		internal static int FPC_GotoFrame(IntPtr hWnd, int FrameNum)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return (!bool_0) ? FPC_GotoFrame_1(hWnd, FrameNum) : FPC_GotoFrame_2(hWnd, FrameNum);
			}
			return ((pFPC_GotoFrame)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_GotoFrame"], typeof(pFPC_GotoFrame)))(hWnd, FrameNum);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_CurrentFrame")]
		internal static extern int FPC_CurrentFrame_1(IntPtr hWnd, ref int Result);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_CurrentFrame")]
		internal static extern int FPC_CurrentFrame_2(IntPtr hWnd, ref int Result);

		internal static int FPC_CurrentFrame(IntPtr hWnd, ref int Result)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_CurrentFrame)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_CurrentFrame"], typeof(pFPC_CurrentFrame)))(hWnd, ref Result);
			}
			return (!bool_0) ? FPC_CurrentFrame_1(hWnd, ref Result) : FPC_CurrentFrame_2(hWnd, ref Result);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_IsPlaying")]
		internal static extern int FPC_IsPlaying_1(IntPtr hWnd, ref int Result);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_IsPlaying")]
		internal static extern int FPC_IsPlaying_2(IntPtr hWnd, ref int Result);

		internal static int FPC_IsPlaying(IntPtr hWnd, ref int Result)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return bool_0 ? FPC_IsPlaying_2(hWnd, ref Result) : FPC_IsPlaying_1(hWnd, ref Result);
			}
			return ((pFPC_IsPlaying)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_IsPlaying"], typeof(pFPC_IsPlaying)))(hWnd, ref Result);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_PercentLoaded")]
		internal static extern int FPC_PercentLoaded_1(IntPtr hWnd, ref int Result);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_PercentLoaded")]
		internal static extern int FPC_PercentLoaded_2(IntPtr hWnd, ref int Result);

		internal static int FPC_PercentLoaded(IntPtr hWnd, ref int Result)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_PercentLoaded)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_PercentLoaded"], typeof(pFPC_PercentLoaded)))(hWnd, ref Result);
			}
			return (!bool_0) ? FPC_PercentLoaded_1(hWnd, ref Result) : FPC_PercentLoaded_2(hWnd, ref Result);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_FrameLoaded")]
		internal static extern int FPC_FrameLoaded_1(IntPtr hWnd, int FrameNum, ref int Result);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_FrameLoaded")]
		internal static extern int FPC_FrameLoaded_2(IntPtr hWnd, int FrameNum, ref int Result);

		internal static int FPC_FrameLoaded(IntPtr hWnd, int FrameNum, ref int Result)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return (!bool_0) ? FPC_FrameLoaded_1(hWnd, FrameNum, ref Result) : FPC_FrameLoaded_2(hWnd, FrameNum, ref Result);
			}
			return ((pFPC_FrameLoaded)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_FrameLoaded"], typeof(pFPC_FrameLoaded)))(hWnd, FrameNum, ref Result);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_FlashVersion")]
		internal static extern int FPC_FlashVersion_1(IntPtr hWnd, ref int Result);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_FlashVersion")]
		internal static extern int FPC_FlashVersion_2(IntPtr hWnd, ref int Result);

		internal static int FPC_FlashVersion(IntPtr hWnd, ref int Result)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return (!bool_0) ? FPC_FlashVersion_1(hWnd, ref Result) : FPC_FlashVersion_2(hWnd, ref Result);
			}
			return ((pFPC_FlashVersion)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_FlashVersion"], typeof(pFPC_FlashVersion)))(hWnd, ref Result);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_LoadMovieW(IntPtr hWnd, int layer, string url);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_LoadMovieW")]
		internal static extern int FPC_LoadMovieW_1(IntPtr hWnd, int layer, string url);

		internal static int FPC_LoadMovie(IntPtr hWnd, int layer, string url)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_LoadMovie)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_LoadMovieW"], typeof(pFPC_LoadMovie)))(hWnd, layer, url);
			}
			return (!bool_0) ? FPC_LoadMovieW(hWnd, layer, url) : FPC_LoadMovieW_1(hWnd, layer, url);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_TGotoFrameW(IntPtr hWnd, string target, int FrameNum);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_TGotoFrameW")]
		internal static extern int FPC_TGotoFrameW_1(IntPtr hWnd, string target, int FrameNum);

		internal static int FPC_TGotoFrame(IntPtr hWnd, string target, int FrameNum)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return (!bool_0) ? FPC_TGotoFrameW(hWnd, target, FrameNum) : FPC_TGotoFrameW_1(hWnd, target, FrameNum);
			}
			return ((pFPC_TGotoFrame)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_TGotoFrameW"], typeof(pFPC_TGotoFrame)))(hWnd, target, FrameNum);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_TGotoLabelW(IntPtr hWnd, string target, string label);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_TGotoLabelW")]
		internal static extern int FPC_TGotoLabelW_1(IntPtr hWnd, string target, string label);

		internal static int FPC_TGotoLabel(IntPtr hWnd, string target, string label)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_TGotoLabel)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_TGotoLabelW"], typeof(pFPC_TGotoLabel)))(hWnd, target, label);
			}
			return (!bool_0) ? FPC_TGotoLabelW(hWnd, target, label) : FPC_TGotoLabelW_1(hWnd, target, label);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_TCurrentFrameW(IntPtr hWnd, string target, ref int Result);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_TCurrentFrameW")]
		internal static extern int FPC_TCurrentFrameW_1(IntPtr hWnd, string target, ref int Result);

		internal static int FPC_TCurrentFrame(IntPtr hWnd, string target, ref int Result)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_TCurrentFrame)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_TCurrentFrameW"], typeof(pFPC_TCurrentFrame)))(hWnd, target, ref Result);
			}
			return (!bool_0) ? FPC_TCurrentFrameW(hWnd, target, ref Result) : FPC_TCurrentFrameW_1(hWnd, target, ref Result);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_TCurrentLabelW(IntPtr hWnd, string target, int pBuffer, ref int dwSize);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_TCurrentLabelW")]
		internal static extern int FPC_TCurrentLabelW_1(IntPtr hWnd, string target, long pBuffer, ref int dwSize);

		internal static int FPC_TCurrentLabel(IntPtr hWnd, string target, IntPtr pBuffer, ref int dwSize)
		{
			smethod_1();
			return (libraryMode_0 != LibraryMode.UseStatic) ? (bool_0 ? FPC_TCurrentLabelW_1(hWnd, target, (long)pBuffer, ref dwSize) : FPC_TCurrentLabelW(hWnd, target, (int)pBuffer, ref dwSize)) : ((!bool_0) ? ((pFPC_TCurrentLabel32)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_TCurrentLabelW"], typeof(pFPC_TCurrentLabel32)))(hWnd, target, (int)pBuffer, ref dwSize) : ((pFPC_TCurrentLabel64)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_TCurrentLabelW"], typeof(pFPC_TCurrentLabel64)))(hWnd, target, (long)pBuffer, ref dwSize));
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_TPlayW(IntPtr hWnd, string target);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_TPlayW")]
		internal static extern int FPC_TPlayW_1(IntPtr hWnd, string target);

		internal static int FPC_TPlay(IntPtr hWnd, string target)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_TPlay)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_TPlayW"], typeof(pFPC_TPlay)))(hWnd, target);
			}
			return bool_0 ? FPC_TPlayW_1(hWnd, target) : FPC_TPlayW(hWnd, target);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_TStopPlayW(IntPtr hWnd, string target);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_TStopPlayW")]
		internal static extern int FPC_TStopPlayW_1(IntPtr hWnd, string target);

		internal static int FPC_TStopPlay(IntPtr hWnd, string target)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return (!bool_0) ? FPC_TStopPlayW(hWnd, target) : FPC_TStopPlayW_1(hWnd, target);
			}
			return ((pFPC_TStopPlay)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_TStopPlayW"], typeof(pFPC_TStopPlay)))(hWnd, target);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_SetVariableW(IntPtr hWnd, string name, string value);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_SetVariableW")]
		internal static extern int FPC_SetVariableW_1(IntPtr hWnd, string name, string value);

		internal static int FPC_SetVariable(IntPtr hWnd, string name, string value)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return (!bool_0) ? FPC_SetVariableW(hWnd, name, value) : FPC_SetVariableW_1(hWnd, name, value);
			}
			return ((pFPC_SetVariable)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_SetVariableW"], typeof(pFPC_SetVariable)))(hWnd, name, value);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_GetVariableW(IntPtr hWnd, string name, int pBuffer, ref int dwSize);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_GetVariableW")]
		internal static extern int FPC_GetVariableW_1(IntPtr hWnd, string name, long pBuffer, ref int dwSize);

		internal static int FPC_GetVariable(IntPtr hWnd, string name, IntPtr pBuffer, ref int dwSize)
		{
			smethod_1();
			return (libraryMode_0 != LibraryMode.UseStatic) ? (bool_0 ? FPC_GetVariableW_1(hWnd, name, (long)pBuffer, ref dwSize) : FPC_GetVariableW(hWnd, name, (int)pBuffer, ref dwSize)) : ((!bool_0) ? ((pFPC_GetVariable32)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_GetVariableW"], typeof(pFPC_GetVariable32)))(hWnd, name, (int)pBuffer, ref dwSize) : ((pFPC_GetVariable64)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_GetVariableW"], typeof(pFPC_GetVariable64)))(hWnd, name, (long)pBuffer, ref dwSize));
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_TSetPropertyW(IntPtr hWnd, string target, int property, string value);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_TSetPropertyW")]
		internal static extern int FPC_TSetPropertyW_1(IntPtr hWnd, string target, int property, string value);

		internal static int FPC_TSetProperty(IntPtr hWnd, string target, int property, string value)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_TSetProperty)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_TSetPropertyW"], typeof(pFPC_TSetProperty)))(hWnd, target, property, value);
			}
			return bool_0 ? FPC_TSetPropertyW_1(hWnd, target, property, value) : FPC_TSetPropertyW(hWnd, target, property, value);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_TGetPropertyW(IntPtr hWnd, string target, int property, int pBuffer, ref int dwSize);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_TGetPropertyW")]
		internal static extern int FPC_TGetPropertyW_1(IntPtr hWnd, string target, int property, long pBuffer, ref int dwSize);

		internal static int FPC_TGetProperty(IntPtr hWnd, string target, int property, IntPtr pBuffer, ref int dwSize)
		{
			smethod_1();
			return (libraryMode_0 != LibraryMode.UseStatic) ? ((!bool_0) ? FPC_TGetPropertyW(hWnd, target, property, (int)pBuffer, ref dwSize) : FPC_TGetPropertyW_1(hWnd, target, property, (long)pBuffer, ref dwSize)) : ((!bool_0) ? ((pFPC_TGetProperty32)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_TGetPropertyW"], typeof(pFPC_TGetProperty32)))(hWnd, target, property, (int)pBuffer, ref dwSize) : ((pFPC_TGetProperty64)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_TGetPropertyW"], typeof(pFPC_TGetProperty64)))(hWnd, target, property, (long)pBuffer, ref dwSize));
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_TCallFrameW(IntPtr hWnd, string target, int FrameNum);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_TCallFrameW")]
		internal static extern int FPC_TCallFrameW_1(IntPtr hWnd, string target, int FrameNum);

		internal static int FPC_TCallFrame(IntPtr hWnd, string target, int FrameNum)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return bool_0 ? FPC_TCallFrameW_1(hWnd, target, FrameNum) : FPC_TCallFrameW(hWnd, target, FrameNum);
			}
			return ((pFPC_TCallFrame)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_TCallFrameW"], typeof(pFPC_TCallFrame)))(hWnd, target, FrameNum);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_TCallLabelW(IntPtr hWnd, string target, string label);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_TCallLabelW")]
		internal static extern int FPC_TCallLabelW_1(IntPtr hWnd, string target, string label);

		internal static int FPC_TCallLabel(IntPtr hWnd, string target, string label)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_TCallLabel)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_TCallLabelW"], typeof(pFPC_TCallLabel)))(hWnd, target, label);
			}
			return (!bool_0) ? FPC_TCallLabelW(hWnd, target, label) : FPC_TCallLabelW_1(hWnd, target, label);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_TSetPropertyNumW(IntPtr hWnd, string target, int property, double value);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_TSetPropertyNumW")]
		internal static extern int FPC_TSetPropertyNumW_1(IntPtr hWnd, string target, int property, double value);

		internal static int FPC_TSetPropertyNum(IntPtr hWnd, string target, int property, double value)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return (!bool_0) ? FPC_TSetPropertyNumW(hWnd, target, property, value) : FPC_TSetPropertyNumW_1(hWnd, target, property, value);
			}
			return ((pFPC_TSetPropertyNum)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_TSetPropertyNumW"], typeof(pFPC_TSetPropertyNum)))(hWnd, target, property, value);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_TGetPropertyNumW(IntPtr hWnd, string target, int property, ref double Result);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_TGetPropertyNumW")]
		internal static extern int FPC_TGetPropertyNumW_1(IntPtr hWnd, string target, int property, ref double Result);

		internal static int FPC_TGetPropertyNum(IntPtr hWnd, string target, int property, ref double Result)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_TGetPropertyNum)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_TGetPropertyNumW"], typeof(pFPC_TGetPropertyNum)))(hWnd, target, property, ref Result);
			}
			return (!bool_0) ? FPC_TGetPropertyNumW(hWnd, target, property, ref Result) : FPC_TGetPropertyNumW_1(hWnd, target, property, ref Result);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_TGetPropertyAsNumberW(IntPtr hWnd, string target, int property, ref double Result);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_TGetPropertyAsNumberW")]
		internal static extern int FPC_TGetPropertyAsNumberW_1(IntPtr hWnd, string target, int property, ref double Result);

		internal static int FPC_TGetPropertyAsNumber(IntPtr hWnd, string target, int property, ref double Result)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_TGetPropertyAsNumber)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_TGetPropertyAsNumberW"], typeof(pFPC_TGetPropertyAsNumber)))(hWnd, target, property, ref Result);
			}
			return (!bool_0) ? FPC_TGetPropertyAsNumberW(hWnd, target, property, ref Result) : FPC_TGetPropertyAsNumberW_1(hWnd, target, property, ref Result);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_GetReadyState")]
		internal static extern int FPC_GetReadyState_1(IntPtr hWnd, ref int Value);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_GetReadyState")]
		internal static extern int FPC_GetReadyState_2(IntPtr hWnd, ref int Value);

		internal static int FPC_GetReadyState(IntPtr hWnd, ref int Value)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return (!bool_0) ? FPC_GetReadyState_1(hWnd, ref Value) : FPC_GetReadyState_2(hWnd, ref Value);
			}
			return ((pFPC_GetReadyState)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_GetReadyState"], typeof(pFPC_GetReadyState)))(hWnd, ref Value);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_GetTotalFrames")]
		internal static extern int FPC_GetTotalFrames_1(IntPtr hWnd, ref int Value);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_GetTotalFrames")]
		internal static extern int FPC_GetTotalFrames_2(IntPtr hWnd, ref int Value);

		internal static int FPC_GetTotalFrames(IntPtr hWnd, ref int Value)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return bool_0 ? FPC_GetTotalFrames_2(hWnd, ref Value) : FPC_GetTotalFrames_1(hWnd, ref Value);
			}
			return ((pFPC_GetTotalFrames)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_GetTotalFrames"], typeof(pFPC_GetTotalFrames)))(hWnd, ref Value);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_PutPlaying")]
		internal static extern int FPC_PutPlaying_1(IntPtr hWnd, int Value);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_PutPlaying")]
		internal static extern int FPC_PutPlaying_2(IntPtr hWnd, int Value);

		internal static int FPC_PutPlaying(IntPtr hWnd, int Value)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_PutPlaying)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_PutPlaying"], typeof(pFPC_PutPlaying)))(hWnd, Value);
			}
			return bool_0 ? FPC_PutPlaying_2(hWnd, Value) : FPC_PutPlaying_1(hWnd, Value);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_GetPlaying")]
		internal static extern int FPC_GetPlaying_1(IntPtr hWnd, ref int Value);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_GetPlaying")]
		internal static extern int FPC_GetPlaying_2(IntPtr hWnd, ref int Value);

		internal static int FPC_GetPlaying(IntPtr hWnd, ref int Value)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return (!bool_0) ? FPC_GetPlaying_1(hWnd, ref Value) : FPC_GetPlaying_2(hWnd, ref Value);
			}
			return ((pFPC_GetPlaying)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_GetPlaying"], typeof(pFPC_GetPlaying)))(hWnd, ref Value);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_PutQuality")]
		internal static extern int FPC_PutQuality_1(IntPtr hWnd, int Value);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_PutQuality")]
		internal static extern int FPC_PutQuality_2(IntPtr hWnd, int Value);

		internal static int FPC_PutQuality(IntPtr hWnd, int Value)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_PutQuality)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_PutQuality"], typeof(pFPC_PutQuality)))(hWnd, Value);
			}
			return bool_0 ? FPC_PutQuality_2(hWnd, Value) : FPC_PutQuality_1(hWnd, Value);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_GetQuality")]
		internal static extern int FPC_GetQuality_1(IntPtr hWnd, ref int Value);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_GetQuality")]
		internal static extern int FPC_GetQuality_2(IntPtr hWnd, ref int Value);

		internal static int FPC_GetQuality(IntPtr hWnd, ref int Value)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return (!bool_0) ? FPC_GetQuality_1(hWnd, ref Value) : FPC_GetQuality_2(hWnd, ref Value);
			}
			return ((pFPC_GetQuality)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_GetQuality"], typeof(pFPC_GetQuality)))(hWnd, ref Value);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_PutScaleMode")]
		internal static extern int FPC_PutScaleMode_1(IntPtr hWnd, int Value);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_PutScaleMode")]
		internal static extern int FPC_PutScaleMode_2(IntPtr hWnd, int Value);

		internal static int FPC_PutScaleMode(IntPtr hWnd, int Value)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_PutScaleMode)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_PutScaleMode"], typeof(pFPC_PutScaleMode)))(hWnd, Value);
			}
			return bool_0 ? FPC_PutScaleMode_2(hWnd, Value) : FPC_PutScaleMode_1(hWnd, Value);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_GetScaleMode")]
		internal static extern int FPC_GetScaleMode_1(IntPtr hWnd, ref int Value);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_GetScaleMode")]
		internal static extern int FPC_GetScaleMode_2(IntPtr hWnd, ref int Value);

		internal static int FPC_GetScaleMode(IntPtr hWnd, ref int Value)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_GetScaleMode)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_GetScaleMode"], typeof(pFPC_GetScaleMode)))(hWnd, ref Value);
			}
			return (!bool_0) ? FPC_GetScaleMode_1(hWnd, ref Value) : FPC_GetScaleMode_2(hWnd, ref Value);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_PutAlignMode")]
		internal static extern int FPC_PutAlignMode_1(IntPtr hWnd, int Value);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_PutAlignMode")]
		internal static extern int FPC_PutAlignMode_2(IntPtr hWnd, int Value);

		internal static int FPC_PutAlignMode(IntPtr hWnd, int Value)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return (!bool_0) ? FPC_PutAlignMode_1(hWnd, Value) : FPC_PutAlignMode_2(hWnd, Value);
			}
			return ((pFPC_PutAlignMode)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_PutAlignMode"], typeof(pFPC_PutAlignMode)))(hWnd, Value);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_GetAlignMode")]
		internal static extern int FPC_GetAlignMode_1(IntPtr hWnd, ref int Value);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_GetAlignMode")]
		internal static extern int FPC_GetAlignMode_2(IntPtr hWnd, ref int Value);

		internal static int FPC_GetAlignMode(IntPtr hWnd, ref int Value)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_GetAlignMode)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_GetAlignMode"], typeof(pFPC_GetAlignMode)))(hWnd, ref Value);
			}
			return (!bool_0) ? FPC_GetAlignMode_1(hWnd, ref Value) : FPC_GetAlignMode_2(hWnd, ref Value);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_PutBackgroundColor")]
		internal static extern int FPC_PutBackgroundColor_1(IntPtr hWnd, int Value);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_PutBackgroundColor")]
		internal static extern int FPC_PutBackgroundColor_2(IntPtr hWnd, int Value);

		internal static int FPC_PutBackgroundColor(IntPtr hWnd, int Value)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return bool_0 ? FPC_PutBackgroundColor_2(hWnd, Value) : FPC_PutBackgroundColor_1(hWnd, Value);
			}
			return ((pFPC_PutBackgroundColor)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_PutBackgroundColor"], typeof(pFPC_PutBackgroundColor)))(hWnd, Value);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_GetBackgroundColor")]
		internal static extern int FPC_GetBackgroundColor_1(IntPtr hWnd, ref int Value);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_GetBackgroundColor")]
		internal static extern int FPC_GetBackgroundColor_2(IntPtr hWnd, ref int Value);

		internal static int FPC_GetBackgroundColor(IntPtr hWnd, ref int Value)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_GetBackgroundColor)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_GetBackgroundColor"], typeof(pFPC_GetBackgroundColor)))(hWnd, ref Value);
			}
			return bool_0 ? FPC_GetBackgroundColor_2(hWnd, ref Value) : FPC_GetBackgroundColor_1(hWnd, ref Value);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_PutLoop")]
		internal static extern int FPC_PutLoop_1(IntPtr hWnd, int Value);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_PutLoop")]
		internal static extern int FPC_PutLoop_2(IntPtr hWnd, int Value);

		internal static int FPC_PutLoop(IntPtr hWnd, int Value)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return bool_0 ? FPC_PutLoop_2(hWnd, Value) : FPC_PutLoop_1(hWnd, Value);
			}
			return ((pFPC_PutLoop)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_PutLoop"], typeof(pFPC_PutLoop)))(hWnd, Value);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_GetLoop")]
		internal static extern int FPC_GetLoop_1(IntPtr hWnd, ref int Value);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_GetLoop")]
		internal static extern int FPC_GetLoop_2(IntPtr hWnd, ref int Value);

		internal static int FPC_GetLoop(IntPtr hWnd, ref int Value)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_GetLoop)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_GetLoop"], typeof(pFPC_GetLoop)))(hWnd, ref Value);
			}
			return (!bool_0) ? FPC_GetLoop_1(hWnd, ref Value) : FPC_GetLoop_2(hWnd, ref Value);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_GetMovieW(IntPtr hWnd, IntPtr pBuffer, ref int dwSize);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_GetMovieW")]
		internal static extern int FPC_GetMovieW_1(IntPtr hWnd, IntPtr pBuffer, ref int dwSize);

		internal static int FPC_GetMovie(IntPtr hWnd, IntPtr pBuffer, ref int dwSize)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return bool_0 ? FPC_GetMovieW_1(hWnd, pBuffer, ref dwSize) : FPC_GetMovieW(hWnd, pBuffer, ref dwSize);
			}
			return ((pFPC_GetMovie)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_GetMovieW"], typeof(pFPC_GetMovie)))(hWnd, pBuffer, ref dwSize);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_PutMovieW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string Movie);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_PutMovieW")]
		internal static extern int FPC_PutMovieW_1(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string Movie);

		internal static int FPC_PutMovie(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string Movie)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return (!bool_0) ? FPC_PutMovieW(hWnd, Movie) : FPC_PutMovieW_1(hWnd, Movie);
			}
			return ((pFPC_PutMovie)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_PutMovieW"], typeof(pFPC_PutMovie)))(hWnd, Movie);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_PutFrameNum")]
		internal static extern int FPC_PutFrameNum_1(IntPtr hWnd, int Value);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_PutFrameNum")]
		internal static extern int FPC_PutFrameNum_2(IntPtr hWnd, int Value);

		internal static int FPC_PutFrameNum(IntPtr hWnd, int Value)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return (!bool_0) ? FPC_PutFrameNum_1(hWnd, Value) : FPC_PutFrameNum_2(hWnd, Value);
			}
			return ((pFPC_PutFrameNum)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_PutFrameNum"], typeof(pFPC_PutFrameNum)))(hWnd, Value);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_GetFrameNum")]
		internal static extern int FPC_GetFrameNum_1(IntPtr hWnd, ref int Value);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_GetFrameNum")]
		internal static extern int FPC_GetFrameNum_2(IntPtr hWnd, ref int Value);

		internal static int FPC_GetFrameNum(IntPtr hWnd, ref int Value)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return bool_0 ? FPC_GetFrameNum_2(hWnd, ref Value) : FPC_GetFrameNum_1(hWnd, ref Value);
			}
			return ((pFPC_GetFrameNum)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_GetFrameNum"], typeof(pFPC_GetFrameNum)))(hWnd, ref Value);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_GetWModeW(IntPtr hWnd, IntPtr pBuffer, ref int dwSize);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_GetWModeW")]
		internal static extern int FPC_GetWModeW_1(IntPtr hWnd, IntPtr pBuffer, ref int dwSize);

		internal static int FPC_GetWMode(IntPtr hWnd, IntPtr pBuffer, ref int dwSize)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_GetWMode)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_GetWModeW"], typeof(pFPC_GetWMode)))(hWnd, pBuffer, ref dwSize);
			}
			return bool_0 ? FPC_GetWModeW_1(hWnd, pBuffer, ref dwSize) : FPC_GetWModeW(hWnd, pBuffer, ref dwSize);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_PutWModeW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string WMode);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_PutWModeW")]
		internal static extern int FPC_PutWModeW_1(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string WMode);

		internal static int FPC_PutWMode(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string WMode)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_PutWMode)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_PutWModeW"], typeof(pFPC_PutWMode)))(hWnd, WMode);
			}
			return (!bool_0) ? FPC_PutWModeW(hWnd, WMode) : FPC_PutWModeW_1(hWnd, WMode);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_GetSAlignW(IntPtr hWnd, IntPtr pBuffer, ref int dwSize);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_GetSAlignW")]
		internal static extern int FPC_GetSAlignW_1(IntPtr hWnd, IntPtr pBuffer, ref int dwSize);

		internal static int FPC_GetSAlign(IntPtr hWnd, IntPtr pBuffer, ref int dwSize)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return bool_0 ? FPC_GetSAlignW_1(hWnd, pBuffer, ref dwSize) : FPC_GetSAlignW(hWnd, pBuffer, ref dwSize);
			}
			return ((pFPC_GetSAlign)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_GetSAlignW"], typeof(pFPC_GetSAlign)))(hWnd, pBuffer, ref dwSize);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_PutSAlignW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string SAlign);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_PutSAlignW")]
		internal static extern int FPC_PutSAlignW_1(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string SAlign);

		internal static int FPC_PutSAlign(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string SAlign)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return (!bool_0) ? FPC_PutSAlignW(hWnd, SAlign) : FPC_PutSAlignW_1(hWnd, SAlign);
			}
			return ((pFPC_PutSAlign)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_PutSAlignW"], typeof(pFPC_PutSAlign)))(hWnd, SAlign);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_PutMenu")]
		internal static extern int FPC_PutMenu_1(IntPtr hWnd, int Value);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_PutMenu")]
		internal static extern int FPC_PutMenu_2(IntPtr hWnd, int Value);

		internal static int FPC_PutMenu(IntPtr hWnd, int Value)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return (!bool_0) ? FPC_PutMenu_1(hWnd, Value) : FPC_PutMenu_2(hWnd, Value);
			}
			return ((pFPC_PutMenu)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_PutMenu"], typeof(pFPC_PutMenu)))(hWnd, Value);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_GetMenu")]
		internal static extern int FPC_GetMenu_1(IntPtr hWnd, ref int Value);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_GetMenu")]
		internal static extern int FPC_GetMenu_2(IntPtr hWnd, ref int Value);

		internal static int FPC_GetMenu(IntPtr hWnd, ref int Value)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_GetMenu)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_GetMenu"], typeof(pFPC_GetMenu)))(hWnd, ref Value);
			}
			return bool_0 ? FPC_GetMenu_2(hWnd, ref Value) : FPC_GetMenu_1(hWnd, ref Value);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_GetBaseW(IntPtr hWnd, IntPtr pBuffer, ref int dwSize);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_GetBaseW")]
		internal static extern int FPC_GetBaseW_1(IntPtr hWnd, IntPtr pBuffer, ref int dwSize);

		internal static int FPC_GetBase(IntPtr hWnd, IntPtr pBuffer, ref int dwSize)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_GetBase)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_GetBaseW"], typeof(pFPC_GetBase)))(hWnd, pBuffer, ref dwSize);
			}
			return bool_0 ? FPC_GetBaseW_1(hWnd, pBuffer, ref dwSize) : FPC_GetBaseW(hWnd, pBuffer, ref dwSize);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_PutBaseW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string Base);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_PutBaseW")]
		internal static extern int FPC_PutBaseW_1(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string Base);

		internal static int FPC_PutBase(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string Base)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return bool_0 ? FPC_PutBaseW_1(hWnd, Base) : FPC_PutBaseW(hWnd, Base);
			}
			return ((pFPC_PutBase)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_PutBaseW"], typeof(pFPC_PutBase)))(hWnd, Base);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_GetScaleW(IntPtr hWnd, IntPtr pBuffer, ref int dwSize);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_GetScaleW")]
		internal static extern int FPC_GetScaleW_1(IntPtr hWnd, IntPtr pBuffer, ref int dwSize);

		internal static int FPC_GetScale(IntPtr hWnd, IntPtr pBuffer, ref int dwSize)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_GetScale)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_GetScaleW"], typeof(pFPC_GetScale)))(hWnd, pBuffer, ref dwSize);
			}
			return (!bool_0) ? FPC_GetScaleW(hWnd, pBuffer, ref dwSize) : FPC_GetScaleW_1(hWnd, pBuffer, ref dwSize);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_PutScaleW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string Scale);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_PutScaleW")]
		internal static extern int FPC_PutScaleW_1(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string Scale);

		internal static int FPC_PutScale(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string Scale)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return bool_0 ? FPC_PutScaleW_1(hWnd, Scale) : FPC_PutScaleW(hWnd, Scale);
			}
			return ((pFPC_PutScale)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_PutScaleW"], typeof(pFPC_PutScale)))(hWnd, Scale);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_PutDeviceFont")]
		internal static extern int FPC_PutDeviceFont_1(IntPtr hWnd, int Value);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_PutDeviceFont")]
		internal static extern int FPC_PutDeviceFont_2(IntPtr hWnd, int Value);

		internal static int FPC_PutDeviceFont(IntPtr hWnd, int Value)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return bool_0 ? FPC_PutDeviceFont_2(hWnd, Value) : FPC_PutDeviceFont_1(hWnd, Value);
			}
			return ((pFPC_PutDeviceFont)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_PutDeviceFont"], typeof(pFPC_PutDeviceFont)))(hWnd, Value);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_GetDeviceFont")]
		internal static extern int FPC_GetDeviceFont_1(IntPtr hWnd, ref int Value);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_GetDeviceFont")]
		internal static extern int FPC_GetDeviceFont_2(IntPtr hWnd, ref int Value);

		internal static int FPC_GetDeviceFont(IntPtr hWnd, ref int Value)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return (!bool_0) ? FPC_GetDeviceFont_1(hWnd, ref Value) : FPC_GetDeviceFont_2(hWnd, ref Value);
			}
			return ((pFPC_GetDeviceFont)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_GetDeviceFont"], typeof(pFPC_GetDeviceFont)))(hWnd, ref Value);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_PutEmbedMovie")]
		internal static extern int FPC_PutEmbedMovie_1(IntPtr hWnd, int Value);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_PutEmbedMovie")]
		internal static extern int FPC_PutEmbedMovie_2(IntPtr hWnd, int Value);

		internal static int FPC_PutEmbedMovie(IntPtr hWnd, int Value)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return bool_0 ? FPC_PutEmbedMovie_2(hWnd, Value) : FPC_PutEmbedMovie_1(hWnd, Value);
			}
			return ((pFPC_PutEmbedMovie)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_PutEmbedMovie"], typeof(pFPC_PutEmbedMovie)))(hWnd, Value);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_GetEmbedMovie")]
		internal static extern int FPC_GetEmbedMovie_1(IntPtr hWnd, ref int Value);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_GetEmbedMovie")]
		internal static extern int FPC_GetEmbedMovie_2(IntPtr hWnd, ref int Value);

		internal static int FPC_GetEmbedMovie(IntPtr hWnd, ref int Value)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return (!bool_0) ? FPC_GetEmbedMovie_1(hWnd, ref Value) : FPC_GetEmbedMovie_2(hWnd, ref Value);
			}
			return ((pFPC_GetEmbedMovie)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_GetEmbedMovie"], typeof(pFPC_GetEmbedMovie)))(hWnd, ref Value);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_GetBGColorW(IntPtr hWnd, IntPtr pBuffer, ref int dwSize);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_GetBGColorW")]
		internal static extern int FPC_GetBGColorW_1(IntPtr hWnd, IntPtr pBuffer, ref int dwSize);

		internal static int FPC_GetBGColor(IntPtr hWnd, IntPtr pBuffer, ref int dwSize)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return bool_0 ? FPC_GetBGColorW_1(hWnd, pBuffer, ref dwSize) : FPC_GetBGColorW(hWnd, pBuffer, ref dwSize);
			}
			return ((pFPC_GetBGColor)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_GetBGColorW"], typeof(pFPC_GetBGColor)))(hWnd, pBuffer, ref dwSize);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_PutBGColorW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string BGColor);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_PutBGColorW")]
		internal static extern int FPC_PutBGColorW_1(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string BGColor);

		internal static int FPC_PutBGColor(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string BGColor)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_PutBGColor)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_PutBGColorW"], typeof(pFPC_PutBGColor)))(hWnd, BGColor);
			}
			return (!bool_0) ? FPC_PutBGColorW(hWnd, BGColor) : FPC_PutBGColorW_1(hWnd, BGColor);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_GetQuality2W(IntPtr hWnd, IntPtr pBuffer, ref int dwSize);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_GetQuality2W")]
		internal static extern int FPC_GetQuality2W_1(IntPtr hWnd, IntPtr pBuffer, ref int dwSize);

		internal static int FPC_GetQuality2(IntPtr hWnd, IntPtr pBuffer, ref int dwSize)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_GetQuality2)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_GetQuality2W"], typeof(pFPC_GetQuality2)))(hWnd, pBuffer, ref dwSize);
			}
			return (!bool_0) ? FPC_GetQuality2W(hWnd, pBuffer, ref dwSize) : FPC_GetQuality2W_1(hWnd, pBuffer, ref dwSize);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_PutQuality2W(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string Quality2);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_PutQuality2W")]
		internal static extern int FPC_PutQuality2W_1(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string Quality2);

		internal static int FPC_PutQuality2(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string Quality2)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return bool_0 ? FPC_PutQuality2W_1(hWnd, Quality2) : FPC_PutQuality2W(hWnd, Quality2);
			}
			return ((pFPC_PutQuality2)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_PutQuality2W"], typeof(pFPC_PutQuality2)))(hWnd, Quality2);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_GetSWRemoteW(IntPtr hWnd, IntPtr pBuffer, ref int dwSize);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_GetSWRemoteW")]
		internal static extern int FPC_GetSWRemoteW_1(IntPtr hWnd, IntPtr pBuffer, ref int dwSize);

		internal static int FPC_GetSWRemote(IntPtr hWnd, IntPtr pBuffer, ref int dwSize)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_GetSWRemote)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_GetSWRemoteW"], typeof(pFPC_GetSWRemote)))(hWnd, pBuffer, ref dwSize);
			}
			return (!bool_0) ? FPC_GetSWRemoteW(hWnd, pBuffer, ref dwSize) : FPC_GetSWRemoteW_1(hWnd, pBuffer, ref dwSize);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_PutSWRemoteW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string SWRemote);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_PutSWRemoteW")]
		internal static extern int FPC_PutSWRemoteW_1(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string SWRemote);

		internal static int FPC_PutSWRemote(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string SWRemote)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return bool_0 ? FPC_PutSWRemoteW_1(hWnd, SWRemote) : FPC_PutSWRemoteW(hWnd, SWRemote);
			}
			return ((pFPC_PutSWRemote)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_PutSWRemoteW"], typeof(pFPC_PutSWRemote)))(hWnd, SWRemote);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_GetStackingW(IntPtr hWnd, IntPtr pBuffer, ref int dwSize);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_GetStackingW")]
		internal static extern int FPC_GetStackingW_1(IntPtr hWnd, IntPtr pBuffer, ref int dwSize);

		internal static int FPC_GetStacking(IntPtr hWnd, IntPtr pBuffer, ref int dwSize)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return (!bool_0) ? FPC_GetStackingW(hWnd, pBuffer, ref dwSize) : FPC_GetStackingW_1(hWnd, pBuffer, ref dwSize);
			}
			return ((pFPC_GetStacking)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_GetStackingW"], typeof(pFPC_GetStacking)))(hWnd, pBuffer, ref dwSize);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_PutStackingW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string Stacking);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_PutStackingW")]
		internal static extern int FPC_PutStackingW_1(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string Stacking);

		internal static int FPC_PutStacking(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string Stacking)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return (!bool_0) ? FPC_PutStackingW(hWnd, Stacking) : FPC_PutStackingW_1(hWnd, Stacking);
			}
			return ((pFPC_PutStacking)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_PutStackingW"], typeof(pFPC_PutStacking)))(hWnd, Stacking);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_GetFlashVarsW(IntPtr hWnd, IntPtr pBuffer, ref int dwSize);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_GetFlashVarsW")]
		internal static extern int FPC_GetFlashVarsW_1(IntPtr hWnd, IntPtr pBuffer, ref int dwSize);

		internal static int FPC_GetFlashVars(IntPtr hWnd, IntPtr pBuffer, ref int dwSize)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return bool_0 ? FPC_GetFlashVarsW_1(hWnd, pBuffer, ref dwSize) : FPC_GetFlashVarsW(hWnd, pBuffer, ref dwSize);
			}
			return ((pFPC_GetFlashVars)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_GetFlashVarsW"], typeof(pFPC_GetFlashVars)))(hWnd, pBuffer, ref dwSize);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_PutFlashVarsW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string FlashVars);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_PutFlashVarsW")]
		internal static extern int FPC_PutFlashVarsW_1(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string FlashVars);

		internal static int FPC_PutFlashVars(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string FlashVars)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_PutFlashVars)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_PutFlashVarsW"], typeof(pFPC_PutFlashVars)))(hWnd, FlashVars);
			}
			return bool_0 ? FPC_PutFlashVarsW_1(hWnd, FlashVars) : FPC_PutFlashVarsW(hWnd, FlashVars);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_GetAllowScriptAccessW(IntPtr hWnd, IntPtr pBuffer, ref int dwSize);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_GetAllowScriptAccessW")]
		internal static extern int FPC_GetAllowScriptAccessW_1(IntPtr hWnd, IntPtr pBuffer, ref int dwSize);

		internal static int FPC_GetAllowScriptAccess(IntPtr hWnd, IntPtr pBuffer, ref int dwSize)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return (!bool_0) ? FPC_GetAllowScriptAccessW(hWnd, pBuffer, ref dwSize) : FPC_GetAllowScriptAccessW_1(hWnd, pBuffer, ref dwSize);
			}
			return ((pFPC_GetAllowScriptAccess)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_GetAllowScriptAccessW"], typeof(pFPC_GetAllowScriptAccess)))(hWnd, pBuffer, ref dwSize);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_PutAllowScriptAccessW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string AllowScriptAccess);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_PutAllowScriptAccessW")]
		internal static extern int FPC_PutAllowScriptAccessW_1(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string AllowScriptAccess);

		internal static int FPC_PutAllowScriptAccess(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string AllowScriptAccess)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return bool_0 ? FPC_PutAllowScriptAccessW_1(hWnd, AllowScriptAccess) : FPC_PutAllowScriptAccessW(hWnd, AllowScriptAccess);
			}
			return ((pFPC_PutAllowScriptAccess)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_PutAllowScriptAccessW"], typeof(pFPC_PutAllowScriptAccess)))(hWnd, AllowScriptAccess);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_GetMovieDataW(IntPtr hWnd, IntPtr pBuffer, ref int dwSize);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_GetMovieDataW")]
		internal static extern int FPC_GetMovieDataW_1(IntPtr hWnd, IntPtr pBuffer, ref int dwSize);

		internal static int FPC_GetMovieData(IntPtr hWnd, IntPtr pBuffer, ref int dwSize)
		{
			smethod_1();
			if (libraryMode_0 != LibraryMode.UseStatic)
			{
				return (!bool_0) ? FPC_GetMovieDataW(hWnd, pBuffer, ref dwSize) : FPC_GetMovieDataW_1(hWnd, pBuffer, ref dwSize);
			}
			return ((pFPC_GetMovieData)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_GetMovieDataW"], typeof(pFPC_GetMovieData)))(hWnd, pBuffer, ref dwSize);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
		internal static extern int FPC_PutMovieDataW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string MovieData);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "FPC_PutMovieDataW")]
		internal static extern int FPC_PutMovieDataW_1(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string MovieData);

		internal static int FPC_PutMovieData(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string MovieData)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_PutMovieData)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_PutMovieDataW"], typeof(pFPC_PutMovieData)))(hWnd, MovieData);
			}
			return bool_0 ? FPC_PutMovieDataW_1(hWnd, MovieData) : FPC_PutMovieDataW(hWnd, MovieData);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_IsFullScreenEnabled")]
		internal static extern int FPC_IsFullScreenEnabled_1(IntPtr hWnd);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_IsFullScreenEnabled")]
		internal static extern int FPC_IsFullScreenEnabled_2(IntPtr hWnd);

		internal static int FPC_IsFullScreenEnabled(IntPtr hWnd)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_IsFullScreenEnabled)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_IsFullScreenEnabled"], typeof(pFPC_IsFullScreenEnabled)))(hWnd);
			}
			return (!bool_0) ? FPC_IsFullScreenEnabled_1(hWnd) : FPC_IsFullScreenEnabled_2(hWnd);
		}

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_EnableFullScreen")]
		internal static extern int FPC_EnableFullScreen_1(IntPtr hWnd, int bEnable);

		[DllImport("gunhoiuc.ghu", CallingConvention = CallingConvention.StdCall, EntryPoint = "FPC_EnableFullScreen")]
		internal static extern int FPC_EnableFullScreen_2(IntPtr hWnd, int bEnable);

		internal static int FPC_EnableFullScreen(IntPtr hWnd, int bEnable)
		{
			smethod_1();
			if (libraryMode_0 == LibraryMode.UseStatic)
			{
				return ((pFPC_EnableFullScreen)Marshal.GetDelegateForFunctionPointer((IntPtr)hashtable_0["FPC_EnableFullScreen"], typeof(pFPC_EnableFullScreen)))(hWnd, bEnable);
			}
			return bool_0 ? FPC_EnableFullScreen_2(hWnd, bEnable) : FPC_EnableFullScreen_1(hWnd, bEnable);
		}

		[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
		internal static extern int SendMessage(IntPtr hWnd, uint msg, UIntPtr wParam, IntPtr lParam);

		[DllImport("Gdi32.dll", CallingConvention = CallingConvention.StdCall)]
		internal static extern int DeleteObject(IntPtr hObject);

		[DllImport("Gdi32.dll", CallingConvention = CallingConvention.StdCall)]
		internal static extern int GetObject(IntPtr hObject, int BufferSize, IntPtr pInfo);

		public static Bitmap GetBitmap(IntPtr f_in_box__handle, bool Transparent)
		{
			Struct0 @struct = default(Struct0);
			IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf((object)@struct));
			Marshal.StructureToPtr((object)@struct, intPtr, fDeleteOld: false);
			SendMessage(f_in_box__handle, 5124u, UIntPtr.Zero, intPtr);
			Struct0 struct2 = (Struct0)Marshal.PtrToStructure(intPtr, typeof(Struct0));
			Marshal.FreeCoTaskMem(intPtr);
			if (Transparent)
			{
				IntPtr hBitmap = struct2.hBitmap;
				int num = Marshal.SizeOf(typeof(BITMAP));
				IntPtr intPtr2 = Marshal.AllocCoTaskMem(num);
				GetObject(hBitmap, num, intPtr2);
				BITMAP bITMAP = (BITMAP)Marshal.PtrToStructure(intPtr2, typeof(BITMAP));
				Marshal.FreeCoTaskMem(intPtr2);
				Bitmap bitmap = new Bitmap(bITMAP.bmWidth, bITMAP.bmHeight, PixelFormat.Format32bppPArgb);
				BitmapData bitmapData = bitmap.LockBits(new Rectangle(new Point(0, 0), bitmap.Size), ImageLockMode.WriteOnly, bitmap.PixelFormat);
				int num2 = 0;
				int num3 = bITMAP.bmWidthBytes * (bITMAP.bmHeight - 1);
				byte[] array = new byte[bITMAP.bmWidthBytes * bITMAP.bmHeight];
				int num4 = 0;
				while (num4 < bITMAP.bmHeight)
				{
					Marshal.Copy(new IntPtr(bITMAP.bmBits + num2), array, 0, bITMAP.bmWidthBytes);
					Marshal.Copy(array, 0, new IntPtr(bitmapData.Scan0.ToInt64() + num3), bITMAP.bmWidthBytes);
					num4++;
					num2 += bITMAP.bmWidthBytes;
					num3 -= bITMAP.bmWidthBytes;
				}
				bitmap.UnlockBits(bitmapData);
				DeleteObject(hBitmap);
				return bitmap;
			}
			Bitmap result;
			using (Bitmap bitmap2 = Image.FromHbitmap(struct2.hBitmap))
			{
				using MemoryStream stream = new MemoryStream();
				bitmap2.Save(stream, ImageFormat.Bmp);
				using Image original = Image.FromStream(stream);
				result = new Bitmap(original);
			}
			DeleteObject(struct2.hBitmap);
			return result;
		}

		static cedrus__wrapper()
		{
			libraryMode_0 = LibraryMode.UseDll;
			hashtable_0 = new Hashtable();
			bool_0 = 8 == IntPtr.Size;
			bool_1 = false;
		}

		internal static bool OhIbEx1RDqkYJZARRmg()
		{
			return YLByHe1ySL36RAJZRai == null;
		}

		internal static void P58jFyxdGHHlcpKsAK2()
		{
		}
	}
}

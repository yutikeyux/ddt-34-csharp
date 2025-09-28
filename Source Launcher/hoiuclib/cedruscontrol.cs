using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace hoiuclib
{
	public class cedruscontrol : Control, IMessageFilter
	{
		internal struct TRACKMOUSEEVENT
		{
			public int cbSize;

			public int dwFlags;

			public IntPtr hwndTrack;

			public uint dwHoverTime;
		}

		public enum ReadyState
		{
			Loading,
			Uninitialized,
			Loaded,
			Interactive,
			Complete
		}

		public delegate void OnReadyStateChangeEventHandler(object sender, ReadyState state);

		public delegate void OnProgressEventHandler(object sender, int percentDone);

		public delegate void OnFSCommandEventHandler(object sender, string command, string args);

		public delegate void OnFlashCallEventHandler(object sender, string request);

		public delegate void OnLoadExternalResourceByRelativePathEventHandler(object sender, string RelativePath, Stream ContentStream, ref bool Handled);

		public delegate void OnFlashPaintEventHandler(object sender, IntPtr pPixelPointer);

		public delegate void OnUpdateRectEventHandler(object sender, Rectangle RectToUpdate);

		public delegate void OnPaintStageEventHandler(object sender, PaintStage stage, Graphics Canvas);

		public delegate void OnIsInputKeyHandler(object sender, Keys keyData, ref bool InputKey);

		private AxCode axCode_0;

		private bool bool_0;

		private Label label_0 = null;

		private bool bool_1;

		private cedrus__wrapper.EventsListener eventsListener_0;

		private string string_0;

		private bool bool_2 = true;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private OnReadyStateChangeEventHandler onReadyStateChangeEventHandler_0;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private OnProgressEventHandler onProgressEventHandler_0;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private OnFSCommandEventHandler onFSCommandEventHandler_0;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private OnFlashCallEventHandler onFlashCallEventHandler_0;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private OnLoadExternalResourceByRelativePathEventHandler onLoadExternalResourceByRelativePathEventHandler_0;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private OnFlashPaintEventHandler onFlashPaintEventHandler_0;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private OnUpdateRectEventHandler onUpdateRectEventHandler_0;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private OnPaintStageEventHandler onPaintStageEventHandler_0;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private OnIsInputKeyHandler onIsInputKeyHandler_0;

		internal static cedruscontrol QRHoCgIp2O9JKiYa4hq;

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				if (axCode_0 != null)
				{
					createParams.ClassName = Marshal.PtrToStringUni(cedrus__wrapper.FPC_GetClassNameW(axCode_0.Handle));
				}
				if (TransparentMode)
				{
					createParams.Style |= 1;
				}
				return createParams;
			}
		}

		public bool TransparentMode
		{
			get
			{
				return bool_1;
			}
			set
			{
				if (bool_1 != value)
				{
					bool standardMenu = StandardMenu;
					bool_1 = value;
					RecreateHandle();
					StandardMenu = standardMenu;
				}
			}
		}

		public int FlashProperty_ReadyState
		{
			get
			{
				int Value = 0;
				cedrus__wrapper.FPC_GetReadyState(base.Handle, ref Value);
				return Value;
			}
		}

		public int FlashProperty_TotalFrames
		{
			get
			{
				int Value = 0;
				cedrus__wrapper.FPC_GetTotalFrames(base.Handle, ref Value);
				return Value;
			}
		}

		public bool FlashProperty_Playing
		{
			get
			{
				int Value = 0;
				cedrus__wrapper.FPC_GetPlaying(base.Handle, ref Value);
				return Value != 0;
			}
			set
			{
				cedrus__wrapper.FPC_PutPlaying(base.Handle, value ? (-1) : 0);
			}
		}

		public int FlashProperty_Quality
		{
			get
			{
				int Value = 0;
				cedrus__wrapper.FPC_GetQuality(base.Handle, ref Value);
				return Value;
			}
			set
			{
				cedrus__wrapper.FPC_PutQuality(base.Handle, value);
			}
		}

		public int FlashProperty_ScaleMode
		{
			get
			{
				int Value = 0;
				cedrus__wrapper.FPC_GetScaleMode(base.Handle, ref Value);
				return Value;
			}
			set
			{
				cedrus__wrapper.FPC_PutScaleMode(base.Handle, value);
			}
		}

		public int FlashProperty_AlignMode
		{
			get
			{
				int Value = 0;
				cedrus__wrapper.FPC_GetAlignMode(base.Handle, ref Value);
				return Value;
			}
			set
			{
				cedrus__wrapper.FPC_PutAlignMode(base.Handle, value);
			}
		}

		public int FlashProperty_BackgroundColor
		{
			get
			{
				int Value = 0;
				cedrus__wrapper.FPC_GetBackgroundColor(base.Handle, ref Value);
				return Value;
			}
			set
			{
				cedrus__wrapper.FPC_PutBackgroundColor(base.Handle, value);
			}
		}

		public bool FlashProperty_Loop
		{
			get
			{
				int Value = 0;
				cedrus__wrapper.FPC_GetLoop(base.Handle, ref Value);
				return Value != 0;
			}
			set
			{
				cedrus__wrapper.FPC_PutLoop(base.Handle, value ? (-1) : 0);
			}
		}

		public string FlashProperty_Movie
		{
			get
			{
				string result = "";
				int dwSize = 0;
				if (cedrus__wrapper.FPC_GetMovie(base.Handle, IntPtr.Zero, ref dwSize) == 0)
				{
					int dwSize2 = dwSize + 1;
					IntPtr intPtr = Marshal.AllocCoTaskMem(dwSize2 * 2);
					Marshal.WriteInt32(intPtr, 0);
					cedrus__wrapper.FPC_GetMovie(base.Handle, intPtr, ref dwSize2);
					result = Marshal.PtrToStringUni(intPtr);
					Marshal.FreeCoTaskMem(intPtr);
				}
				return result;
			}
			set
			{
				cedrus__wrapper.FPC_PutMovie(base.Handle, value);
			}
		}

		public int FlashProperty_FrameNum
		{
			get
			{
				int Value = 0;
				cedrus__wrapper.FPC_GetFrameNum(base.Handle, ref Value);
				return Value;
			}
			set
			{
				cedrus__wrapper.FPC_PutFrameNum(base.Handle, value);
			}
		}

		public string FlashProperty_WMode
		{
			get
			{
				string result = "";
				int dwSize = 0;
				if (cedrus__wrapper.FPC_GetWMode(base.Handle, IntPtr.Zero, ref dwSize) == 0)
				{
					int dwSize2 = dwSize + 1;
					IntPtr intPtr = Marshal.AllocCoTaskMem(dwSize2 * 2);
					Marshal.WriteInt32(intPtr, 0);
					cedrus__wrapper.FPC_GetWMode(base.Handle, intPtr, ref dwSize2);
					result = Marshal.PtrToStringUni(intPtr);
					Marshal.FreeCoTaskMem(intPtr);
				}
				return result;
			}
			set
			{
				cedrus__wrapper.FPC_PutWMode(base.Handle, value);
			}
		}

		public string FlashProperty_SAlign
		{
			get
			{
				string result = "";
				int dwSize = 0;
				if (cedrus__wrapper.FPC_GetSAlign(base.Handle, IntPtr.Zero, ref dwSize) == 0)
				{
					int dwSize2 = dwSize + 1;
					IntPtr intPtr = Marshal.AllocCoTaskMem(dwSize2 * 2);
					Marshal.WriteInt32(intPtr, 0);
					cedrus__wrapper.FPC_GetSAlign(base.Handle, intPtr, ref dwSize2);
					result = Marshal.PtrToStringUni(intPtr);
					Marshal.FreeCoTaskMem(intPtr);
				}
				return result;
			}
			set
			{
				cedrus__wrapper.FPC_PutSAlign(base.Handle, value);
			}
		}

		public bool FlashProperty_Menu
		{
			get
			{
				int Value = 0;
				cedrus__wrapper.FPC_GetMenu(base.Handle, ref Value);
				return Value != 0;
			}
			set
			{
				cedrus__wrapper.FPC_PutMenu(base.Handle, value ? (-1) : 0);
			}
		}

		public string FlashProperty_Base
		{
			get
			{
				string result = "";
				int dwSize = 0;
				if (cedrus__wrapper.FPC_GetBase(base.Handle, IntPtr.Zero, ref dwSize) == 0)
				{
					int dwSize2 = dwSize + 1;
					IntPtr intPtr = Marshal.AllocCoTaskMem(dwSize2 * 2);
					Marshal.WriteInt32(intPtr, 0);
					cedrus__wrapper.FPC_GetBase(base.Handle, intPtr, ref dwSize2);
					result = Marshal.PtrToStringUni(intPtr);
					Marshal.FreeCoTaskMem(intPtr);
				}
				return result;
			}
			set
			{
				cedrus__wrapper.FPC_PutBase(base.Handle, value);
			}
		}

		public string FlashProperty_Scale
		{
			get
			{
				string result = "";
				int dwSize = 0;
				if (cedrus__wrapper.FPC_GetScale(base.Handle, IntPtr.Zero, ref dwSize) == 0)
				{
					int dwSize2 = dwSize + 1;
					IntPtr intPtr = Marshal.AllocCoTaskMem(dwSize2 * 2);
					Marshal.WriteInt32(intPtr, 0);
					cedrus__wrapper.FPC_GetScale(base.Handle, intPtr, ref dwSize2);
					result = Marshal.PtrToStringUni(intPtr);
					Marshal.FreeCoTaskMem(intPtr);
				}
				return result;
			}
			set
			{
				cedrus__wrapper.FPC_PutScale(base.Handle, value);
			}
		}

		public bool FlashProperty_DeviceFont
		{
			get
			{
				int Value = 0;
				cedrus__wrapper.FPC_GetDeviceFont(base.Handle, ref Value);
				return Value != 0;
			}
			set
			{
				cedrus__wrapper.FPC_PutDeviceFont(base.Handle, value ? (-1) : 0);
			}
		}

		public bool FlashProperty_EmbedMovie
		{
			get
			{
				int Value = 0;
				cedrus__wrapper.FPC_GetEmbedMovie(base.Handle, ref Value);
				return Value != 0;
			}
			set
			{
				cedrus__wrapper.FPC_PutEmbedMovie(base.Handle, value ? (-1) : 0);
			}
		}

		public string FlashProperty_BGColor
		{
			get
			{
				string result = "";
				int dwSize = 0;
				if (cedrus__wrapper.FPC_GetBGColor(base.Handle, IntPtr.Zero, ref dwSize) == 0)
				{
					int dwSize2 = dwSize + 1;
					IntPtr intPtr = Marshal.AllocCoTaskMem(dwSize2 * 2);
					Marshal.WriteInt32(intPtr, 0);
					cedrus__wrapper.FPC_GetBGColor(base.Handle, intPtr, ref dwSize2);
					result = Marshal.PtrToStringUni(intPtr);
					Marshal.FreeCoTaskMem(intPtr);
				}
				return result;
			}
			set
			{
				cedrus__wrapper.FPC_PutBGColor(base.Handle, value);
			}
		}

		public string FlashProperty_Quality2
		{
			get
			{
				string result = "";
				int dwSize = 0;
				if (cedrus__wrapper.FPC_GetQuality2(base.Handle, IntPtr.Zero, ref dwSize) == 0)
				{
					int dwSize2 = dwSize + 1;
					IntPtr intPtr = Marshal.AllocCoTaskMem(dwSize2 * 2);
					Marshal.WriteInt32(intPtr, 0);
					cedrus__wrapper.FPC_GetQuality2(base.Handle, intPtr, ref dwSize2);
					result = Marshal.PtrToStringUni(intPtr);
					Marshal.FreeCoTaskMem(intPtr);
				}
				return result;
			}
			set
			{
				cedrus__wrapper.FPC_PutQuality2(base.Handle, value);
			}
		}

		public string FlashProperty_SWRemote
		{
			get
			{
				string result = "";
				int dwSize = 0;
				if (cedrus__wrapper.FPC_GetSWRemote(base.Handle, IntPtr.Zero, ref dwSize) == 0)
				{
					int dwSize2 = dwSize + 1;
					IntPtr intPtr = Marshal.AllocCoTaskMem(dwSize2 * 2);
					Marshal.WriteInt32(intPtr, 0);
					cedrus__wrapper.FPC_GetSWRemote(base.Handle, intPtr, ref dwSize2);
					result = Marshal.PtrToStringUni(intPtr);
					Marshal.FreeCoTaskMem(intPtr);
				}
				return result;
			}
			set
			{
				cedrus__wrapper.FPC_PutSWRemote(base.Handle, value);
			}
		}

		public string FlashProperty_Stacking
		{
			get
			{
				string result = "";
				int dwSize = 0;
				if (cedrus__wrapper.FPC_GetStacking(base.Handle, IntPtr.Zero, ref dwSize) == 0)
				{
					int dwSize2 = dwSize + 1;
					IntPtr intPtr = Marshal.AllocCoTaskMem(dwSize2 * 2);
					Marshal.WriteInt32(intPtr, 0);
					cedrus__wrapper.FPC_GetStacking(base.Handle, intPtr, ref dwSize2);
					result = Marshal.PtrToStringUni(intPtr);
					Marshal.FreeCoTaskMem(intPtr);
				}
				return result;
			}
			set
			{
				cedrus__wrapper.FPC_PutStacking(base.Handle, value);
			}
		}

		public string FlashProperty_FlashVars
		{
			get
			{
				string result = "";
				int dwSize = 0;
				if (cedrus__wrapper.FPC_GetFlashVars(base.Handle, IntPtr.Zero, ref dwSize) == 0)
				{
					int dwSize2 = dwSize + 1;
					IntPtr intPtr = Marshal.AllocCoTaskMem(dwSize2 * 2);
					Marshal.WriteInt32(intPtr, 0);
					cedrus__wrapper.FPC_GetFlashVars(base.Handle, intPtr, ref dwSize2);
					result = Marshal.PtrToStringUni(intPtr);
					Marshal.FreeCoTaskMem(intPtr);
				}
				return result;
			}
			set
			{
				cedrus__wrapper.FPC_PutFlashVars(base.Handle, value);
			}
		}

		public string FlashProperty_AllowScriptAccess
		{
			get
			{
				string result = "";
				int dwSize = 0;
				if (cedrus__wrapper.FPC_GetAllowScriptAccess(base.Handle, IntPtr.Zero, ref dwSize) == 0)
				{
					int dwSize2 = dwSize + 1;
					IntPtr intPtr = Marshal.AllocCoTaskMem(dwSize2 * 2);
					Marshal.WriteInt32(intPtr, 0);
					cedrus__wrapper.FPC_GetAllowScriptAccess(base.Handle, intPtr, ref dwSize2);
					result = Marshal.PtrToStringUni(intPtr);
					Marshal.FreeCoTaskMem(intPtr);
				}
				return result;
			}
			set
			{
				cedrus__wrapper.FPC_PutAllowScriptAccess(base.Handle, value);
			}
		}

		public string FlashProperty_MovieData
		{
			get
			{
				string result = "";
				int dwSize = 0;
				if (cedrus__wrapper.FPC_GetMovieData(base.Handle, IntPtr.Zero, ref dwSize) == 0)
				{
					int dwSize2 = dwSize + 1;
					IntPtr intPtr = Marshal.AllocCoTaskMem(dwSize2 * 2);
					Marshal.WriteInt32(intPtr, 0);
					cedrus__wrapper.FPC_GetMovieData(base.Handle, intPtr, ref dwSize2);
					result = Marshal.PtrToStringUni(intPtr);
					Marshal.FreeCoTaskMem(intPtr);
				}
				return result;
			}
			set
			{
				cedrus__wrapper.FPC_PutMovieData(base.Handle, value);
			}
		}

		public bool FlashProperty_AllowFullscreen
		{
			get
			{
				return cedrus__wrapper.FPC_IsFullScreenEnabled(base.Handle) != 0;
			}
			set
			{
				cedrus__wrapper.FPC_EnableFullScreen(base.Handle, value ? 1 : 0);
			}
		}

		public AxCode AxCode => axCode_0;

		public bool StandardMenu
		{
			get
			{
				cedrus__wrapper.FPC_GetStandardMenu(base.Handle, out var bEnabled);
				return bEnabled != 0;
			}
			set
			{
				cedrus__wrapper.FPC_PutStandardMenu(base.Handle, value ? 1 : 0);
			}
		}

		public IntPtr AxHandle => cedrus__wrapper.FPC_GetAxHWND(base.Handle);

		public string Context
		{
			get
			{
				return string_0;
			}
			set
			{
				cedrus__wrapper.FPC_SetContext(base.Handle, value);
				string_0 = value;
			}
		}

		public bool UseFlashCursor
		{
			get
			{
				return bool_2;
			}
			set
			{
				bool_2 = value;
			}
		}

		public event OnReadyStateChangeEventHandler OnReadyStateChange
		{
			[CompilerGenerated]
			add
			{
				OnReadyStateChangeEventHandler onReadyStateChangeEventHandler = onReadyStateChangeEventHandler_0;
				OnReadyStateChangeEventHandler onReadyStateChangeEventHandler2;
				do
				{
					onReadyStateChangeEventHandler2 = onReadyStateChangeEventHandler;
					OnReadyStateChangeEventHandler value2 = (OnReadyStateChangeEventHandler)Delegate.Combine(onReadyStateChangeEventHandler2, value);
					onReadyStateChangeEventHandler = Interlocked.CompareExchange(ref onReadyStateChangeEventHandler_0, value2, onReadyStateChangeEventHandler2);
				}
				while ((object)onReadyStateChangeEventHandler != onReadyStateChangeEventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				OnReadyStateChangeEventHandler onReadyStateChangeEventHandler = onReadyStateChangeEventHandler_0;
				OnReadyStateChangeEventHandler onReadyStateChangeEventHandler2;
				do
				{
					onReadyStateChangeEventHandler2 = onReadyStateChangeEventHandler;
					OnReadyStateChangeEventHandler value2 = (OnReadyStateChangeEventHandler)Delegate.Remove(onReadyStateChangeEventHandler2, value);
					onReadyStateChangeEventHandler = Interlocked.CompareExchange(ref onReadyStateChangeEventHandler_0, value2, onReadyStateChangeEventHandler2);
				}
				while ((object)onReadyStateChangeEventHandler != onReadyStateChangeEventHandler2);
			}
		}

		public event OnProgressEventHandler OnProgress
		{
			[CompilerGenerated]
			add
			{
				OnProgressEventHandler onProgressEventHandler = onProgressEventHandler_0;
				OnProgressEventHandler onProgressEventHandler2;
				do
				{
					onProgressEventHandler2 = onProgressEventHandler;
					OnProgressEventHandler value2 = (OnProgressEventHandler)Delegate.Combine(onProgressEventHandler2, value);
					onProgressEventHandler = Interlocked.CompareExchange(ref onProgressEventHandler_0, value2, onProgressEventHandler2);
				}
				while ((object)onProgressEventHandler != onProgressEventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				OnProgressEventHandler onProgressEventHandler = onProgressEventHandler_0;
				OnProgressEventHandler onProgressEventHandler2;
				do
				{
					onProgressEventHandler2 = onProgressEventHandler;
					OnProgressEventHandler value2 = (OnProgressEventHandler)Delegate.Remove(onProgressEventHandler2, value);
					onProgressEventHandler = Interlocked.CompareExchange(ref onProgressEventHandler_0, value2, onProgressEventHandler2);
				}
				while ((object)onProgressEventHandler != onProgressEventHandler2);
			}
		}

		public event OnFSCommandEventHandler Event_0
		{
			[CompilerGenerated]
			add
			{
				OnFSCommandEventHandler onFSCommandEventHandler = onFSCommandEventHandler_0;
				OnFSCommandEventHandler onFSCommandEventHandler2;
				do
				{
					onFSCommandEventHandler2 = onFSCommandEventHandler;
					OnFSCommandEventHandler value2 = (OnFSCommandEventHandler)Delegate.Combine(onFSCommandEventHandler2, value);
					onFSCommandEventHandler = Interlocked.CompareExchange(ref onFSCommandEventHandler_0, value2, onFSCommandEventHandler2);
				}
				while ((object)onFSCommandEventHandler != onFSCommandEventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				OnFSCommandEventHandler onFSCommandEventHandler = onFSCommandEventHandler_0;
				OnFSCommandEventHandler onFSCommandEventHandler2;
				do
				{
					onFSCommandEventHandler2 = onFSCommandEventHandler;
					OnFSCommandEventHandler value2 = (OnFSCommandEventHandler)Delegate.Remove(onFSCommandEventHandler2, value);
					onFSCommandEventHandler = Interlocked.CompareExchange(ref onFSCommandEventHandler_0, value2, onFSCommandEventHandler2);
				}
				while ((object)onFSCommandEventHandler != onFSCommandEventHandler2);
			}
		}

		public event OnFlashCallEventHandler OnFlashCall
		{
			[CompilerGenerated]
			add
			{
				OnFlashCallEventHandler onFlashCallEventHandler = onFlashCallEventHandler_0;
				OnFlashCallEventHandler onFlashCallEventHandler2;
				do
				{
					onFlashCallEventHandler2 = onFlashCallEventHandler;
					OnFlashCallEventHandler value2 = (OnFlashCallEventHandler)Delegate.Combine(onFlashCallEventHandler2, value);
					onFlashCallEventHandler = Interlocked.CompareExchange(ref onFlashCallEventHandler_0, value2, onFlashCallEventHandler2);
				}
				while ((object)onFlashCallEventHandler != onFlashCallEventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				OnFlashCallEventHandler onFlashCallEventHandler = onFlashCallEventHandler_0;
				OnFlashCallEventHandler onFlashCallEventHandler2;
				do
				{
					onFlashCallEventHandler2 = onFlashCallEventHandler;
					OnFlashCallEventHandler value2 = (OnFlashCallEventHandler)Delegate.Remove(onFlashCallEventHandler2, value);
					onFlashCallEventHandler = Interlocked.CompareExchange(ref onFlashCallEventHandler_0, value2, onFlashCallEventHandler2);
				}
				while ((object)onFlashCallEventHandler != onFlashCallEventHandler2);
			}
		}

		public event OnLoadExternalResourceByRelativePathEventHandler OnLoadExternalResourceByRelativePath
		{
			[CompilerGenerated]
			add
			{
				OnLoadExternalResourceByRelativePathEventHandler onLoadExternalResourceByRelativePathEventHandler = onLoadExternalResourceByRelativePathEventHandler_0;
				OnLoadExternalResourceByRelativePathEventHandler onLoadExternalResourceByRelativePathEventHandler2;
				do
				{
					onLoadExternalResourceByRelativePathEventHandler2 = onLoadExternalResourceByRelativePathEventHandler;
					OnLoadExternalResourceByRelativePathEventHandler value2 = (OnLoadExternalResourceByRelativePathEventHandler)Delegate.Combine(onLoadExternalResourceByRelativePathEventHandler2, value);
					onLoadExternalResourceByRelativePathEventHandler = Interlocked.CompareExchange(ref onLoadExternalResourceByRelativePathEventHandler_0, value2, onLoadExternalResourceByRelativePathEventHandler2);
				}
				while ((object)onLoadExternalResourceByRelativePathEventHandler != onLoadExternalResourceByRelativePathEventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				OnLoadExternalResourceByRelativePathEventHandler onLoadExternalResourceByRelativePathEventHandler = onLoadExternalResourceByRelativePathEventHandler_0;
				OnLoadExternalResourceByRelativePathEventHandler onLoadExternalResourceByRelativePathEventHandler2;
				do
				{
					onLoadExternalResourceByRelativePathEventHandler2 = onLoadExternalResourceByRelativePathEventHandler;
					OnLoadExternalResourceByRelativePathEventHandler value2 = (OnLoadExternalResourceByRelativePathEventHandler)Delegate.Remove(onLoadExternalResourceByRelativePathEventHandler2, value);
					onLoadExternalResourceByRelativePathEventHandler = Interlocked.CompareExchange(ref onLoadExternalResourceByRelativePathEventHandler_0, value2, onLoadExternalResourceByRelativePathEventHandler2);
				}
				while ((object)onLoadExternalResourceByRelativePathEventHandler != onLoadExternalResourceByRelativePathEventHandler2);
			}
		}

		public event OnFlashPaintEventHandler OnFlashPaint
		{
			[CompilerGenerated]
			add
			{
				OnFlashPaintEventHandler onFlashPaintEventHandler = onFlashPaintEventHandler_0;
				OnFlashPaintEventHandler onFlashPaintEventHandler2;
				do
				{
					onFlashPaintEventHandler2 = onFlashPaintEventHandler;
					OnFlashPaintEventHandler value2 = (OnFlashPaintEventHandler)Delegate.Combine(onFlashPaintEventHandler2, value);
					onFlashPaintEventHandler = Interlocked.CompareExchange(ref onFlashPaintEventHandler_0, value2, onFlashPaintEventHandler2);
				}
				while ((object)onFlashPaintEventHandler != onFlashPaintEventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				OnFlashPaintEventHandler onFlashPaintEventHandler = onFlashPaintEventHandler_0;
				OnFlashPaintEventHandler onFlashPaintEventHandler2;
				do
				{
					onFlashPaintEventHandler2 = onFlashPaintEventHandler;
					OnFlashPaintEventHandler value2 = (OnFlashPaintEventHandler)Delegate.Remove(onFlashPaintEventHandler2, value);
					onFlashPaintEventHandler = Interlocked.CompareExchange(ref onFlashPaintEventHandler_0, value2, onFlashPaintEventHandler2);
				}
				while ((object)onFlashPaintEventHandler != onFlashPaintEventHandler2);
			}
		}

		public event OnUpdateRectEventHandler OnUpdateRect
		{
			[CompilerGenerated]
			add
			{
				OnUpdateRectEventHandler onUpdateRectEventHandler = onUpdateRectEventHandler_0;
				OnUpdateRectEventHandler onUpdateRectEventHandler2;
				do
				{
					onUpdateRectEventHandler2 = onUpdateRectEventHandler;
					OnUpdateRectEventHandler value2 = (OnUpdateRectEventHandler)Delegate.Combine(onUpdateRectEventHandler2, value);
					onUpdateRectEventHandler = Interlocked.CompareExchange(ref onUpdateRectEventHandler_0, value2, onUpdateRectEventHandler2);
				}
				while ((object)onUpdateRectEventHandler != onUpdateRectEventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				OnUpdateRectEventHandler onUpdateRectEventHandler = onUpdateRectEventHandler_0;
				OnUpdateRectEventHandler onUpdateRectEventHandler2;
				do
				{
					onUpdateRectEventHandler2 = onUpdateRectEventHandler;
					OnUpdateRectEventHandler value2 = (OnUpdateRectEventHandler)Delegate.Remove(onUpdateRectEventHandler2, value);
					onUpdateRectEventHandler = Interlocked.CompareExchange(ref onUpdateRectEventHandler_0, value2, onUpdateRectEventHandler2);
				}
				while ((object)onUpdateRectEventHandler != onUpdateRectEventHandler2);
			}
		}

		public event OnPaintStageEventHandler OnPaintStage
		{
			[CompilerGenerated]
			add
			{
				OnPaintStageEventHandler onPaintStageEventHandler = onPaintStageEventHandler_0;
				OnPaintStageEventHandler onPaintStageEventHandler2;
				do
				{
					onPaintStageEventHandler2 = onPaintStageEventHandler;
					OnPaintStageEventHandler value2 = (OnPaintStageEventHandler)Delegate.Combine(onPaintStageEventHandler2, value);
					onPaintStageEventHandler = Interlocked.CompareExchange(ref onPaintStageEventHandler_0, value2, onPaintStageEventHandler2);
				}
				while ((object)onPaintStageEventHandler != onPaintStageEventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				OnPaintStageEventHandler onPaintStageEventHandler = onPaintStageEventHandler_0;
				OnPaintStageEventHandler onPaintStageEventHandler2;
				do
				{
					onPaintStageEventHandler2 = onPaintStageEventHandler;
					OnPaintStageEventHandler value2 = (OnPaintStageEventHandler)Delegate.Remove(onPaintStageEventHandler2, value);
					onPaintStageEventHandler = Interlocked.CompareExchange(ref onPaintStageEventHandler_0, value2, onPaintStageEventHandler2);
				}
				while ((object)onPaintStageEventHandler != onPaintStageEventHandler2);
			}
		}

		public event OnIsInputKeyHandler OnIsInputKey
		{
			[CompilerGenerated]
			add
			{
				OnIsInputKeyHandler onIsInputKeyHandler = onIsInputKeyHandler_0;
				OnIsInputKeyHandler onIsInputKeyHandler2;
				do
				{
					onIsInputKeyHandler2 = onIsInputKeyHandler;
					OnIsInputKeyHandler value2 = (OnIsInputKeyHandler)Delegate.Combine(onIsInputKeyHandler2, value);
					onIsInputKeyHandler = Interlocked.CompareExchange(ref onIsInputKeyHandler_0, value2, onIsInputKeyHandler2);
				}
				while ((object)onIsInputKeyHandler != onIsInputKeyHandler2);
			}
			[CompilerGenerated]
			remove
			{
				OnIsInputKeyHandler onIsInputKeyHandler = onIsInputKeyHandler_0;
				OnIsInputKeyHandler onIsInputKeyHandler2;
				do
				{
					onIsInputKeyHandler2 = onIsInputKeyHandler;
					OnIsInputKeyHandler value2 = (OnIsInputKeyHandler)Delegate.Remove(onIsInputKeyHandler2, value);
					onIsInputKeyHandler = Interlocked.CompareExchange(ref onIsInputKeyHandler_0, value2, onIsInputKeyHandler2);
				}
				while ((object)onIsInputKeyHandler != onIsInputKeyHandler2);
			}
		}

		public cedruscontrol()
		{
			bool_1 = false;
			bool_0 = false;
			axCode_0 = new AxCode();
		}

		public cedruscontrol(AxCode code)
		{
			bool_1 = false;
			bool_0 = false;
			axCode_0 = code;
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			if (base.DesignMode)
			{
				label_0 = new Label();
				label_0.Left = 0;
				label_0.Top = 0;
				label_0.Width = base.Width;
				label_0.Height = base.Height;
				label_0.Visible = true;
				label_0.Text = "Cedrus Dev";
				base.Controls.Add(label_0);
			}
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			if (label_0 != null)
			{
				label_0.Left = 0;
				label_0.Top = 0;
				label_0.Width = base.Width;
				label_0.Height = base.Height;
			}
		}

		public void FlashMethod_SetZoomRect(int left, int top, int right, int bottom)
		{
			cedrus__wrapper.FPC_SetZoomRect(base.Handle, left, top, right, bottom);
		}

		public void FlashMethod_Zoom(int factor)
		{
			cedrus__wrapper.FPC_Zoom(base.Handle, factor);
		}

		public void FlashMethod_Pan(int x, int y, int mode)
		{
			cedrus__wrapper.FPC_Pan(base.Handle, x, y, mode);
		}

		public void FlashMethod_Play()
		{
			cedrus__wrapper.FPC_Play(base.Handle);
		}

		public void FlashMethod_Stop()
		{
			cedrus__wrapper.FPC_Stop(base.Handle);
		}

		public void FlashMethod_Back()
		{
			cedrus__wrapper.FPC_Back(base.Handle);
		}

		public void FlashMethod_Forward()
		{
			cedrus__wrapper.FPC_Forward(base.Handle);
		}

		public void FlashMethod_Rewind()
		{
			cedrus__wrapper.FPC_Rewind(base.Handle);
		}

		public void FlashMethod_StopPlay()
		{
			cedrus__wrapper.FPC_StopPlay(base.Handle);
		}

		public void FlashMethod_GotoFrame(int FrameNum)
		{
			cedrus__wrapper.FPC_GotoFrame(base.Handle, FrameNum);
		}

		public int FlashMethod_CurrentFrame()
		{
			int Result = 0;
			cedrus__wrapper.FPC_CurrentFrame(base.Handle, ref Result);
			return Result;
		}

		public bool FlashMethod_IsPlaying()
		{
			int Result = 0;
			cedrus__wrapper.FPC_IsPlaying(base.Handle, ref Result);
			return Result != 0;
		}

		public int FlashMethod_PercentLoaded()
		{
			int Result = 0;
			cedrus__wrapper.FPC_PercentLoaded(base.Handle, ref Result);
			return Result;
		}

		public bool FlashMethod_FrameLoaded(int FrameNum)
		{
			int Result = 0;
			cedrus__wrapper.FPC_FrameLoaded(base.Handle, FrameNum, ref Result);
			return Result != 0;
		}

		public int FlashMethod_FlashVersion()
		{
			int Result = 0;
			cedrus__wrapper.FPC_FlashVersion(base.Handle, ref Result);
			return Result;
		}

		public void FlashMethod_LoadMovie(int layer, string url)
		{
			cedrus__wrapper.FPC_LoadMovie(base.Handle, layer, url);
		}

		public void FlashMethod_TGotoFrame(string target, int FrameNum)
		{
			cedrus__wrapper.FPC_TGotoFrame(base.Handle, target, FrameNum);
		}

		public void FlashMethod_TGotoLabel(string target, string label)
		{
			cedrus__wrapper.FPC_TGotoLabel(base.Handle, target, label);
		}

		public int FlashMethod_TCurrentFrame(string target)
		{
			int Result = 0;
			cedrus__wrapper.FPC_TCurrentFrame(base.Handle, target, ref Result);
			return Result;
		}

		public string FlashMethod_TCurrentLabel(string target)
		{
			string result = "";
			int dwSize = 0;
			if (cedrus__wrapper.FPC_TCurrentLabel(base.Handle, target, IntPtr.Zero, ref dwSize) == 0)
			{
				int dwSize2 = dwSize + 1;
				IntPtr intPtr = Marshal.AllocCoTaskMem(dwSize2 * 2);
				Marshal.WriteInt32(intPtr, 0);
				cedrus__wrapper.FPC_TCurrentLabel(base.Handle, target, intPtr, ref dwSize2);
				result = Marshal.PtrToStringUni(intPtr);
				Marshal.FreeCoTaskMem(intPtr);
			}
			return result;
		}

		public void FlashMethod_TPlay(string target)
		{
			cedrus__wrapper.FPC_TPlay(base.Handle, target);
		}

		public void FlashMethod_TStopPlay(string target)
		{
			cedrus__wrapper.FPC_TStopPlay(base.Handle, target);
		}

		public void FlashMethod_SetVariable(string name, string value)
		{
			cedrus__wrapper.FPC_SetVariable(base.Handle, name, value);
		}

		public string FlashMethod_GetVariable(string name)
		{
			string result = "";
			int dwSize = 0;
			if (cedrus__wrapper.FPC_GetVariable(base.Handle, name, IntPtr.Zero, ref dwSize) == 0)
			{
				int dwSize2 = dwSize + 1;
				IntPtr intPtr = Marshal.AllocCoTaskMem(dwSize2 * 2);
				Marshal.WriteInt32(intPtr, 0);
				cedrus__wrapper.FPC_GetVariable(base.Handle, name, intPtr, ref dwSize2);
				result = Marshal.PtrToStringUni(intPtr);
				Marshal.FreeCoTaskMem(intPtr);
			}
			return result;
		}

		public void FlashMethod_TSetProperty(string target, int property, string value)
		{
			cedrus__wrapper.FPC_TSetProperty(base.Handle, target, property, value);
		}

		public string FlashMethod_TGetProperty(string target, int property)
		{
			string result = "";
			int dwSize = 0;
			if (cedrus__wrapper.FPC_TGetProperty(base.Handle, target, property, IntPtr.Zero, ref dwSize) == 0)
			{
				int dwSize2 = dwSize + 1;
				IntPtr intPtr = Marshal.AllocCoTaskMem(dwSize2 * 2);
				Marshal.WriteInt32(intPtr, 0);
				cedrus__wrapper.FPC_TGetProperty(base.Handle, target, property, intPtr, ref dwSize2);
				result = Marshal.PtrToStringUni(intPtr);
				Marshal.FreeCoTaskMem(intPtr);
			}
			return result;
		}

		public void FlashMethod_TCallFrame(string target, int FrameNum)
		{
			cedrus__wrapper.FPC_TCallFrame(base.Handle, target, FrameNum);
		}

		public void FlashMethod_TCallLabel(string target, string label)
		{
			cedrus__wrapper.FPC_TCallLabel(base.Handle, target, label);
		}

		public void FlashMethod_TSetPropertyNum(string target, int property, double value)
		{
			cedrus__wrapper.FPC_TSetPropertyNum(base.Handle, target, property, value);
		}

		public double FlashMethod_TGetPropertyNum(string target, int property)
		{
			double Result = 0.0;
			cedrus__wrapper.FPC_TGetPropertyNum(base.Handle, target, property, ref Result);
			return Result;
		}

		public double FlashMethod_TGetPropertyAsNumber(string target, int property)
		{
			double Result = 0.0;
			cedrus__wrapper.FPC_TGetPropertyAsNumber(base.Handle, target, property, ref Result);
			return Result;
		}

		public string FlashMethod_CallFunction(string Request)
		{
			string result = "";
			IntPtr intPtr = Marshal.StringToBSTR(Request);
			IntPtr bstrResponse = IntPtr.Zero;
			cedrus__wrapper.FPCCallFunctionBSTR(base.Handle, intPtr, ref bstrResponse);
			if (IntPtr.Zero != bstrResponse)
			{
				result = Marshal.PtrToStringUni(bstrResponse);
				Marshal.FreeBSTR(bstrResponse);
			}
			Marshal.FreeBSTR(intPtr);
			return result;
		}

		public void FlashMethod_SetReturnValue(string strValue)
		{
			IntPtr intPtr = Marshal.StringToBSTR(strValue);
			cedrus__wrapper.FPCSetReturnValue(base.Handle, intPtr);
			Marshal.FreeBSTR(intPtr);
		}

		public Stream PutMovieUsingStream()
		{
			cedrus__wrapper.FPCPutMovieUsingStream(base.Handle, out var stream);
			return new Stream0(stream, bAddRef: false);
		}

		public Stream LoadMovieUsingStream(int layer)
		{
			cedrus__wrapper.FPCLoadMovieUsingStream(base.Handle, layer, out var stream);
			return new Stream0(stream, bAddRef: false);
		}

		public void PutMovieFromStream(Stream Stream)
		{
			using Stream stream = PutMovieUsingStream();
			byte[] buffer = new byte[65536];
			int count;
			while ((count = Stream.Read(buffer, 0, 65536)) > 0)
			{
				stream.Write(buffer, 0, count);
			}
			stream.Close();
		}

		public void LoadMovieFromStream(int layer, Stream Stream)
		{
			using Stream stream = LoadMovieUsingStream(layer);
			byte[] buffer = new byte[65536];
			int count;
			while ((count = Stream.Read(buffer, 0, 65536)) > 0)
			{
				stream.Write(buffer, 0, count);
			}
			stream.Close();
		}

		[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
		internal static extern int SendMessage(IntPtr hWnd, uint msg, UIntPtr wParam, IntPtr lParam);

		[DllImport("Gdi32.dll", CallingConvention = CallingConvention.StdCall)]
		internal static extern int DeleteObject(IntPtr hObject);

		public Bitmap GetBitmap()
		{
			return cedrus__wrapper.GetBitmap(base.Handle, TransparentMode);
		}

		private int method_0(IntPtr intptr_0, IntPtr intptr_1, IntPtr intptr_2)
		{
			switch (((cedrus__wrapper.NMHDR)Marshal.PtrToStructure(intptr_2, typeof(cedrus__wrapper.NMHDR))).code)
			{
			case 5109u:
			{
				cedrus__wrapper.SFPCNPaintStage sFPCNPaintStage = (cedrus__wrapper.SFPCNPaintStage)Marshal.PtrToStructure(intptr_2, typeof(cedrus__wrapper.SFPCNPaintStage));
				PaintStage stage = sFPCNPaintStage.dwStage switch
				{
					0 => PaintStage.PrePaint, 
					1 => PaintStage.AfterPaint, 
					_ => PaintStage.Unknown, 
				};
				if (onPaintStageEventHandler_0 != null)
				{
					using Graphics canvas = Graphics.FromHdc(sFPCNPaintStage.hdc);
					onPaintStageEventHandler_0(this, stage, canvas);
				}
				break;
			}
			case 5110u:
			{
				cedrus__wrapper.SFPCNUpdateRect sFPCNUpdateRect = (cedrus__wrapper.SFPCNUpdateRect)Marshal.PtrToStructure(intptr_2, typeof(cedrus__wrapper.SFPCNUpdateRect));
				if (onUpdateRectEventHandler_0 != null)
				{
					onUpdateRectEventHandler_0(this, new Rectangle(sFPCNUpdateRect.rc.left, sFPCNUpdateRect.rc.top, sFPCNUpdateRect.rc.right - sFPCNUpdateRect.rc.left, sFPCNUpdateRect.rc.bottom - sFPCNUpdateRect.rc.top));
				}
				break;
			}
			case 5111u:
			{
				cedrus__wrapper.SFPCNPaint sFPCNPaint = (cedrus__wrapper.SFPCNPaint)Marshal.PtrToStructure(intptr_2, typeof(cedrus__wrapper.SFPCNPaint));
				if (onFlashPaintEventHandler_0 != null)
				{
					onFlashPaintEventHandler_0(this, sFPCNPaint.pPixels);
				}
				break;
			}
			case 5112u:
			{
				cedrus__wrapper.SFPCLoadExternalResourceExW sFPCLoadExternalResourceExW = (cedrus__wrapper.SFPCLoadExternalResourceExW)Marshal.PtrToStructure(intptr_2, typeof(cedrus__wrapper.SFPCLoadExternalResourceExW));
				if (onLoadExternalResourceByRelativePathEventHandler_0 != null)
				{
					bool Handled = false;
					Stream stream = new Stream0(sFPCLoadExternalResourceExW.lpStream, bAddRef: true);
					onLoadExternalResourceByRelativePathEventHandler_0(this, Marshal.PtrToStringUni(sFPCLoadExternalResourceExW.lpszRelativePath), stream, ref Handled);
					if (!Handled)
					{
						stream.Close();
					}
					sFPCLoadExternalResourceExW.bHandled = (Handled ? 1 : 0);
				}
				Marshal.StructureToPtr((object)sFPCLoadExternalResourceExW, intptr_2, fDeleteOld: true);
				break;
			}
			case 5114u:
			{
				cedrus__wrapper.SFPCFlashCallInfoStructW sFPCFlashCallInfoStructW = (cedrus__wrapper.SFPCFlashCallInfoStructW)Marshal.PtrToStructure(intptr_2, typeof(cedrus__wrapper.SFPCFlashCallInfoStructW));
				if (onFlashCallEventHandler_0 != null)
				{
					onFlashCallEventHandler_0(this, Marshal.PtrToStringUni(sFPCFlashCallInfoStructW.request));
				}
				break;
			}
			case 4861u:
			{
				cedrus__wrapper.SFPCFSCommandInfoStructW sFPCFSCommandInfoStructW = (cedrus__wrapper.SFPCFSCommandInfoStructW)Marshal.PtrToStructure(intptr_2, typeof(cedrus__wrapper.SFPCFSCommandInfoStructW));
				if (onFSCommandEventHandler_0 != null)
				{
					onFSCommandEventHandler_0(this, Marshal.PtrToStringUni(sFPCFSCommandInfoStructW.command), Marshal.PtrToStringUni(sFPCFSCommandInfoStructW.args));
				}
				break;
			}
			case 4863u:
			{
				cedrus__wrapper.SFPCOnProgressInfoStruct sFPCOnProgressInfoStruct = (cedrus__wrapper.SFPCOnProgressInfoStruct)Marshal.PtrToStructure(intptr_2, typeof(cedrus__wrapper.SFPCOnProgressInfoStruct));
				if (onProgressEventHandler_0 != null)
				{
					onProgressEventHandler_0(this, sFPCOnProgressInfoStruct.percentDone);
				}
				break;
			}
			case 4864u:
			{
				cedrus__wrapper.SFPCOnReadyStateChangeInfoStruct sFPCOnReadyStateChangeInfoStruct = (cedrus__wrapper.SFPCOnReadyStateChangeInfoStruct)Marshal.PtrToStructure(intptr_2, typeof(cedrus__wrapper.SFPCOnReadyStateChangeInfoStruct));
				if (onReadyStateChangeEventHandler_0 != null)
				{
					onReadyStateChangeEventHandler_0(this, (ReadyState)sFPCOnReadyStateChangeInfoStruct.newState);
				}
				break;
			}
			}
			return 0;
		}

		[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
		internal static extern int TrackMouseEvent(ref TRACKMOUSEEVENT EventTrack);

		[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
		internal static extern int SystemParametersInfo(uint uiAction, uint uiParam, out uint pvParam, uint fWinIni);

		private void method_1()
		{
			TRACKMOUSEEVENT EventTrack = default(TRACKMOUSEEVENT);
			EventTrack.cbSize = Marshal.SizeOf((object)EventTrack);
			EventTrack.dwFlags = 3;
			SystemParametersInfo(102u, 0u, out var _, 0u);
			EventTrack.dwHoverTime = uint.MaxValue;
			EventTrack.hwndTrack = cedrus__wrapper.FPC_GetAxHWND(base.Handle);
			TrackMouseEvent(ref EventTrack);
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			Context = string_0;
			eventsListener_0 = method_0;
			cedrus__wrapper.FPCSetEventListener(base.Handle, eventsListener_0, IntPtr.Zero);
			Application.AddMessageFilter(this);
			bool_0 = true;
		}

		protected override void OnHandleDestroyed(EventArgs e)
		{
			if (bool_0)
			{
				Application.RemoveMessageFilter(this);
				bool_0 = false;
			}
			base.OnHandleDestroyed(e);
		}

		private static MouseButtons smethod_0(int int_0)
		{
			MouseButtons mouseButtons = MouseButtons.None;
			if (((uint)int_0 & (true ? 1u : 0u)) != 0)
			{
				mouseButtons |= MouseButtons.Left;
			}
			if (((uint)int_0 & 2u) != 0)
			{
				mouseButtons |= MouseButtons.Right;
			}
			if (((uint)int_0 & 0x10u) != 0)
			{
				mouseButtons |= MouseButtons.Middle;
			}
			if (((uint)int_0 & 0x20u) != 0)
			{
				mouseButtons |= MouseButtons.XButton1;
			}
			if (((uint)int_0 & 0x40u) != 0)
			{
				mouseButtons |= MouseButtons.XButton2;
			}
			return mouseButtons;
		}

		private static int smethod_1(int int_0)
		{
			return int_0 & 0xFFFF;
		}

		private static int smethod_2(int int_0)
		{
			return int_0 >> 16;
		}

		private static MouseButtons smethod_3(ref Message message_0)
		{
			switch (message_0.Msg)
			{
			case 513:
			case 514:
			case 515:
				return MouseButtons.Left;
			case 516:
			case 517:
			case 518:
				return MouseButtons.Right;
			case 519:
			case 520:
			case 521:
				return MouseButtons.Middle;
			default:
				return MouseButtons.None;
			case 523:
			case 524:
			case 525:
			{
				uint num = (uint)(int)message_0.WParam >> 16;
				if (32 != num)
				{
					return (64 == num) ? MouseButtons.XButton2 : MouseButtons.None;
				}
				return MouseButtons.XButton1;
			}
			}
		}

		public bool PreFilterMessage(ref Message msg)
		{
			if (base.IsHandleCreated)
			{
				bool result = false;
				if (msg.HWnd == base.Handle)
				{
					switch (msg.Msg)
					{
					case 514:
					case 517:
					case 520:
					case 524:
						OnMouseUp(new MouseEventArgs(smethod_3(ref msg), 1, smethod_1((int)msg.LParam), smethod_2((int)msg.LParam), 0));
						OnClick(new EventArgs());
						if (517 != msg.Msg)
						{
							break;
						}
						if (ContextMenu == null)
						{
							PropertyInfo property = GetType().GetProperty("ContextMenuStrip");
							if (!(property != null))
							{
								break;
							}
							object value = property.GetValue(this, null);
							if (value != null)
							{
								MethodInfo method = value.GetType().GetMethod("Show", new Type[2]
								{
									GetType(),
									typeof(Point)
								});
								if (method != null)
								{
									method.Invoke(value, new object[2]
									{
										this,
										new Point((int)msg.LParam)
									});
								}
							}
						}
						else
						{
							ContextMenu.Show(this, new Point((int)msg.LParam));
						}
						break;
					}
				}
				if (IntPtr.Zero != AxHandle && msg.HWnd == AxHandle)
				{
					switch (msg.Msg)
					{
					case 512:
						OnMouseMove(new MouseEventArgs(MouseButtons.None, 0, smethod_1((int)msg.LParam), smethod_2((int)msg.LParam), 0));
						break;
					case 522:
						OnMouseWheel(new MouseEventArgs(MouseButtons.None, 0, smethod_1((int)msg.LParam), smethod_2((int)msg.LParam), (int)msg.LParam >> 16));
						break;
					case 513:
					case 516:
					case 519:
					case 523:
						OnMouseDown(new MouseEventArgs(smethod_3(ref msg), 0, smethod_1((int)msg.LParam), smethod_2((int)msg.LParam), 0));
						break;
					case 514:
					case 517:
					case 520:
					case 524:
						OnMouseUp(new MouseEventArgs(smethod_3(ref msg), 1, smethod_1((int)msg.LParam), smethod_2((int)msg.LParam), 0));
						OnClick(new EventArgs());
						if (517 == msg.Msg && ContextMenu != null)
						{
							ContextMenu.Show(this, new Point((int)msg.LParam));
						}
						break;
					case 515:
					case 518:
					case 521:
					case 525:
						OnDoubleClick(new EventArgs());
						break;
					case 257:
					{
						KeyEventArgs keyEventArgs2 = new KeyEventArgs((Keys)(int)msg.WParam);
						OnKeyUp(keyEventArgs2);
						result = keyEventArgs2.Handled;
						break;
					}
					case 256:
					{
						KeyEventArgs keyEventArgs = new KeyEventArgs((Keys)(int)msg.WParam);
						OnKeyDown(keyEventArgs);
						result = keyEventArgs.Handled;
						break;
					}
					}
				}
				return result;
			}
			return false;
		}

		protected override bool IsInputKey(Keys keyData)
		{
			if (onIsInputKeyHandler_0 != null)
			{
				bool InputKey = false;
				onIsInputKeyHandler_0(this, keyData, ref InputKey);
				return InputKey;
			}
			return base.IsInputKey(keyData);
		}

		protected override void WndProc(ref Message m)
		{
			if (32 != m.Msg || !bool_2)
			{
				base.WndProc(ref m);
			}
		}

		internal static bool kKe4mbI6RVmlmKGA4xG()
		{
			return QRHoCgIp2O9JKiYa4hq == null;
		}

		internal static void XSV8iCItVtSN0KEsHm2()
		{
		}
	}
}

using System;
using System.Runtime.InteropServices;

namespace hoiuclib
{
	internal sealed class WinApi
	{
		public struct IMAGE_DOS_HEADER
		{
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
			public char[] e_magic;

			public ushort e_cblp;

			public ushort e_cp;

			public ushort e_crlc;

			public ushort e_cparhdr;

			public ushort e_minalloc;

			public ushort e_maxalloc;

			public ushort e_ss;

			public ushort e_sp;

			public ushort e_csum;

			public ushort e_ip;

			public ushort e_cs;

			public ushort e_lfarlc;

			public ushort e_ovno;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public ushort[] e_res1;

			public ushort e_oemid;

			public ushort e_oeminfo;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
			public ushort[] e_res2;

			public int e_lfanew;
		}

		public struct IMAGE_FILE_HEADER
		{
			public ushort Machine;

			public ushort NumberOfSections;

			public uint TimeDateStamp;

			public uint PointerToSymbolTable;

			public uint NumberOfSymbols;

			public ushort SizeOfOptionalHeader;

			public ushort Characteristics;
		}

		public enum MachineType : ushort
		{
			Native = 0,
			I386 = 332,
			Itanium = 0x200,
			x64 = 34404
		}

		public enum MagicType : ushort
		{
			IMAGE_NT_OPTIONAL_HDR32_MAGIC = 267,
			IMAGE_NT_OPTIONAL_HDR64_MAGIC = 523
		}

		public enum SubSystemType : ushort
		{
			IMAGE_SUBSYSTEM_UNKNOWN = 0,
			IMAGE_SUBSYSTEM_NATIVE = 1,
			IMAGE_SUBSYSTEM_WINDOWS_GUI = 2,
			IMAGE_SUBSYSTEM_WINDOWS_CUI = 3,
			IMAGE_SUBSYSTEM_POSIX_CUI = 7,
			IMAGE_SUBSYSTEM_WINDOWS_CE_GUI = 9,
			IMAGE_SUBSYSTEM_EFI_APPLICATION = 10,
			IMAGE_SUBSYSTEM_EFI_BOOT_SERVICE_DRIVER = 11,
			IMAGE_SUBSYSTEM_EFI_RUNTIME_DRIVER = 12,
			IMAGE_SUBSYSTEM_EFI_ROM = 13,
			IMAGE_SUBSYSTEM_XBOX = 14
		}

		public enum DllCharacteristicsType : ushort
		{
			RES_0 = 1,
			RES_1 = 2,
			RES_2 = 4,
			RES_3 = 8,
			IMAGE_DLL_CHARACTERISTICS_DYNAMIC_BASE = 0x40,
			IMAGE_DLL_CHARACTERISTICS_FORCE_INTEGRITY = 0x80,
			IMAGE_DLL_CHARACTERISTICS_NX_COMPAT = 0x100,
			IMAGE_DLLCHARACTERISTICS_NO_ISOLATION = 0x200,
			IMAGE_DLLCHARACTERISTICS_NO_SEH = 0x400,
			IMAGE_DLLCHARACTERISTICS_NO_BIND = 0x800,
			RES_4 = 0x1000,
			IMAGE_DLLCHARACTERISTICS_WDM_DRIVER = 0x2000,
			IMAGE_DLLCHARACTERISTICS_TERMINAL_SERVER_AWARE = 0x8000
		}

		public struct IMAGE_BASE_RELOCATION
		{
			public uint VirtualAddress;

			public uint SizeOfBlock;
		}

		public struct IMAGE_IMPORT_DESCRIPTOR
		{
			public uint OriginalFirstThunk;

			public uint TimeDateStamp;

			public uint ForwarderChain;

			public uint Name;

			public uint FirstThunk;
		}

		public struct IMAGE_DATA_DIRECTORY
		{
			public uint VirtualAddress;

			public uint Size;
		}

		public struct IMAGE_EXPORT_DIRECTORY
		{
			public uint Characteristics;

			public uint TimeDateStamp;

			public ushort MajorVersion;

			public ushort MinorVersion;

			public uint Name;

			public uint Base;

			public uint NumberOfFunctions;

			public uint NumberOfNames;

			public uint AddressOfFunctions;

			public uint AddressOfNames;

			public uint AddressOfNameOrdinals;
		}

		[Flags]
		public enum AllocationType : uint
		{
			COMMIT = 0x1000u,
			RESERVE = 0x2000u,
			RESET = 0x80000u,
			LARGE_PAGES = 0x20000000u,
			PHYSICAL = 0x400000u,
			TOP_DOWN = 0x100000u,
			WRITE_WATCH = 0x200000u
		}

		[Flags]
		public enum MemoryProtection : uint
		{
			EXECUTE = 0x10u,
			EXECUTE_READ = 0x20u,
			EXECUTE_READWRITE = 0x40u,
			EXECUTE_WRITECOPY = 0x80u,
			NOACCESS = 0x1u,
			READONLY = 0x2u,
			READWRITE = 0x4u,
			WRITECOPY = 0x8u,
			GUARD_Modifierflag = 0x100u,
			NOCACHE_Modifierflag = 0x200u,
			WRITECOMBINE_Modifierflag = 0x400u
		}

		[StructLayout(LayoutKind.Explicit)]
		public struct IMAGE_SECTION_HEADER
		{
			[FieldOffset(0)]
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			public char[] Name;

			[FieldOffset(8)]
			public uint VirtualSize;

			[FieldOffset(12)]
			public uint VirtualAddress;

			[FieldOffset(16)]
			public uint SizeOfRawData;

			[FieldOffset(20)]
			public uint PointerToRawData;

			[FieldOffset(24)]
			public uint PointerToRelocations;

			[FieldOffset(28)]
			public uint PointerToLinenumbers;

			[FieldOffset(32)]
			public ushort NumberOfRelocations;

			[FieldOffset(34)]
			public ushort NumberOfLinenumbers;

			[FieldOffset(36)]
			public DataSectionFlags Characteristics;

			private static object T618HMqB6RYD0iFHXP0;

			public string Section => new string(Name);

			internal static bool yWuyMDqOQ2IwoS24T7J()
			{
				return T618HMqB6RYD0iFHXP0 == null;
			}
		}

		[Flags]
		public enum DataSectionFlags : uint
		{
			TypeReg = 0x0u,
			TypeDsect = 0x1u,
			TypeNoLoad = 0x2u,
			TypeGroup = 0x4u,
			TypeNoPadded = 0x8u,
			TypeCopy = 0x10u,
			ContentCode = 0x20u,
			ContentInitializedData = 0x40u,
			ContentUninitializedData = 0x80u,
			LinkOther = 0x100u,
			LinkInfo = 0x200u,
			TypeOver = 0x400u,
			LinkRemove = 0x800u,
			LinkComDat = 0x1000u,
			NoDeferSpecExceptions = 0x4000u,
			RelativeGP = 0x8000u,
			MemPurgeable = 0x20000u,
			flag_17 = 0x20000u,
			MemoryLocked = 0x40000u,
			MemoryPreload = 0x80000u,
			flag_20 = 0x100000u,
			flag_21 = 0x200000u,
			flag_22 = 0x300000u,
			flag_23 = 0x400000u,
			Align16Bytes = 0x500000u,
			Align32Bytes = 0x600000u,
			Align64Bytes = 0x700000u,
			Align128Bytes = 0x800000u,
			Align256Bytes = 0x900000u,
			Align512Bytes = 0xA00000u,
			Align1024Bytes = 0xB00000u,
			Align2048Bytes = 0xC00000u,
			Align4096Bytes = 0xD00000u,
			Align8192Bytes = 0xE00000u,
			LinkExtendedRelocationOverflow = 0x1000000u,
			MemoryDiscardable = 0x2000000u,
			MemoryNotCached = 0x4000000u,
			MemoryNotPaged = 0x8000000u,
			MemoryShared = 0x10000000u,
			MemoryExecute = 0x20000000u,
			MemoryRead = 0x40000000u,
			MemoryWrite = 0x80000000u
		}

		internal delegate uint DllMain(IntPtr hinstDLL, uint fdwReason, uint lpvReserved);

		internal static uint DLL_PROCESS_ATTACH;

		internal static uint DLL_THREAD_ATTACH;

		private static WinApi rN9g9UIFvtLrceCduMD;

		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern IntPtr VirtualAlloc(IntPtr lpAddress, UIntPtr dwSize, AllocationType flAllocationType, MemoryProtection flProtect);

		[DllImport("kernel32.dll")]
		internal static extern void RtlZeroMemory(IntPtr dest, int size);

		[DllImport("kernel32", SetLastError = true)]
		internal static extern IntPtr LoadLibraryA(IntPtr lpFileName);

		[DllImport("kernel32", EntryPoint = "GetProcAddress", SetLastError = true)]
		internal static extern IntPtr GetProcAddress_1(IntPtr hModule, IntPtr lpFunctionName);

		[DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true)]
		internal static extern IntPtr GetProcAddress(IntPtr hModule, string lpFunctionName);

		[DllImport("kernel32.dll")]
		internal static extern bool FlushInstructionCache(IntPtr hProcess, IntPtr lpBaseAddress, uint dwSize);

		static WinApi()
		{
			DLL_PROCESS_ATTACH = 1u;
			DLL_THREAD_ATTACH = 2u;
		}

		internal static void gR8Lt1IlNZYEEWubV1H()
		{
		}

		internal static bool RwJ2BUI47I0ZHwUCaLH()
		{
			return rN9g9UIFvtLrceCduMD == null;
		}
	}
}

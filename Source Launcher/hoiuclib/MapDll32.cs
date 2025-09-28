using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace hoiuclib
{
	public sealed class MapDll32
	{
		internal static MapDll32 y61gwtIqQ9GrjtBqCqX;

		internal static IntPtr Internal_MapDll(byte[] RawData)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(RawData.Length);
			Marshal.Copy(RawData, 0, intPtr, RawData.Length);
			WinApi.IMAGE_DOS_HEADER iMAGE_DOS_HEADER = (WinApi.IMAGE_DOS_HEADER)Marshal.PtrToStructure(intPtr, typeof(WinApi.IMAGE_DOS_HEADER));
			IntPtr ptr = new IntPtr(intPtr.ToInt32() + iMAGE_DOS_HEADER.e_lfanew);
			WinApi32.IMAGE_NT_HEADERS32 iMAGE_NT_HEADERS = (WinApi32.IMAGE_NT_HEADERS32)Marshal.PtrToStructure(ptr, typeof(WinApi32.IMAGE_NT_HEADERS32));
			IntPtr intPtr2 = WinApi.VirtualAlloc(IntPtr.Zero, new UIntPtr(iMAGE_NT_HEADERS.OptionalHeader.SizeOfImage), WinApi.AllocationType.COMMIT, WinApi.MemoryProtection.EXECUTE_READWRITE);
			Marshal.Copy(RawData, 0, intPtr2, (int)iMAGE_NT_HEADERS.OptionalHeader.SizeOfHeaders);
			ptr = new IntPtr(ptr.ToInt32() + Marshal.SizeOf(typeof(WinApi32.IMAGE_NT_HEADERS32)) - Marshal.SizeOf(typeof(WinApi32.IMAGE_OPTIONAL_HEADER32)) + iMAGE_NT_HEADERS.FileHeader.SizeOfOptionalHeader);
			for (int i = 0; i < iMAGE_NT_HEADERS.FileHeader.NumberOfSections; i++)
			{
				WinApi.IMAGE_SECTION_HEADER iMAGE_SECTION_HEADER = (WinApi.IMAGE_SECTION_HEADER)Marshal.PtrToStructure(new IntPtr(ptr.ToInt32() + i * Marshal.SizeOf(typeof(WinApi.IMAGE_SECTION_HEADER))), typeof(WinApi.IMAGE_SECTION_HEADER));
				uint virtualSize = iMAGE_SECTION_HEADER.VirtualSize;
				uint sizeOfRawData = iMAGE_SECTION_HEADER.SizeOfRawData;
				IntPtr intPtr3 = new IntPtr(intPtr2.ToInt32() + iMAGE_SECTION_HEADER.VirtualAddress);
				WinApi.RtlZeroMemory(intPtr3, (int)virtualSize);
				Marshal.Copy(RawData, (int)iMAGE_SECTION_HEADER.PointerToRawData, intPtr3, (int)((virtualSize < sizeOfRawData) ? virtualSize : sizeOfRawData));
			}
			IntPtr intPtr4 = new IntPtr(intPtr2.ToInt32() - iMAGE_NT_HEADERS.OptionalHeader.ImageBase);
			uint size = iMAGE_NT_HEADERS.OptionalHeader.BaseRelocationTable.Size;
			IntPtr intPtr5 = new IntPtr(intPtr2.ToInt32() + iMAGE_NT_HEADERS.OptionalHeader.BaseRelocationTable.VirtualAddress);
			IntPtr ptr2 = intPtr5;
			while (ptr2.ToInt32() < intPtr5.ToInt32() + size)
			{
				WinApi.IMAGE_BASE_RELOCATION iMAGE_BASE_RELOCATION = (WinApi.IMAGE_BASE_RELOCATION)Marshal.PtrToStructure(ptr2, typeof(WinApi.IMAGE_BASE_RELOCATION));
				uint num = (uint)((int)iMAGE_BASE_RELOCATION.SizeOfBlock - Marshal.SizeOf(typeof(WinApi.IMAGE_BASE_RELOCATION))) / 2u;
				IntPtr intPtr6 = new IntPtr(ptr2.ToInt32() + Marshal.SizeOf(typeof(WinApi.IMAGE_BASE_RELOCATION)));
				for (int j = 0; j < num; j++)
				{
					short num2 = Marshal.ReadInt16(intPtr6);
					if (((uint)num2 & 0xF000u) != 0)
					{
						IntPtr ptr3 = new IntPtr(intPtr2.ToInt32() + iMAGE_BASE_RELOCATION.VirtualAddress + (num2 & 0xFFF));
						Marshal.WriteIntPtr(ptr3, new IntPtr(Marshal.ReadIntPtr(ptr3).ToInt32() + intPtr4.ToInt32()));
					}
					intPtr6 = new IntPtr(intPtr6.ToInt32() + 2);
				}
				ptr2 = intPtr6;
			}
			IntPtr ptr4 = new IntPtr(intPtr2.ToInt32() + iMAGE_NT_HEADERS.OptionalHeader.ImportTable.VirtualAddress);
			while (true)
			{
				WinApi.IMAGE_IMPORT_DESCRIPTOR iMAGE_IMPORT_DESCRIPTOR = (WinApi.IMAGE_IMPORT_DESCRIPTOR)Marshal.PtrToStructure(ptr4, typeof(WinApi.IMAGE_IMPORT_DESCRIPTOR));
				if (iMAGE_IMPORT_DESCRIPTOR.Name == 0)
				{
					break;
				}
				IntPtr hModule = WinApi.LoadLibraryA(new IntPtr(intPtr2.ToInt32() + iMAGE_IMPORT_DESCRIPTOR.Name));
				IntPtr ptr5 = ((iMAGE_IMPORT_DESCRIPTOR.TimeDateStamp != 0) ? new IntPtr(intPtr2.ToInt32() + iMAGE_IMPORT_DESCRIPTOR.OriginalFirstThunk) : new IntPtr(intPtr2.ToInt32() + iMAGE_IMPORT_DESCRIPTOR.FirstThunk));
				while (true)
				{
					uint num3 = (uint)(int)Marshal.ReadIntPtr(ptr5);
					if (num3 == 0)
					{
						break;
					}
					IntPtr lpFunctionName = (((num3 & 0x80000000u) != 0) ? new IntPtr(num3 & 0xFFFF) : new IntPtr(intPtr2.ToInt32() + num3 + 2L));
					IntPtr procAddress_ = WinApi.GetProcAddress_1(hModule, lpFunctionName);
					Marshal.WriteIntPtr(ptr5, procAddress_);
					ptr5 = new IntPtr(ptr5.ToInt32() + IntPtr.Size);
				}
				ptr4 = new IntPtr(ptr4.ToInt32() + Marshal.SizeOf(typeof(WinApi.IMAGE_IMPORT_DESCRIPTOR)));
			}
			WinApi.FlushInstructionCache(new IntPtr(IntPtr.Zero.ToInt64() - 1L), intPtr2, iMAGE_NT_HEADERS.OptionalHeader.SizeOfImage);
			WinApi.DllMain dllMain = (WinApi.DllMain)Marshal.GetDelegateForFunctionPointer(new IntPtr(intPtr2.ToInt32() + iMAGE_NT_HEADERS.OptionalHeader.AddressOfEntryPoint), typeof(WinApi.DllMain));
			dllMain(intPtr2, WinApi.DLL_PROCESS_ATTACH, 0u);
			dllMain(intPtr2, WinApi.DLL_THREAD_ATTACH, 0u);
			return intPtr2;
		}

		internal static void Internal_SaveExports(IntPtr pImageBase, Hashtable Exports)
		{
			WinApi.IMAGE_DOS_HEADER iMAGE_DOS_HEADER = (WinApi.IMAGE_DOS_HEADER)Marshal.PtrToStructure(pImageBase, typeof(WinApi.IMAGE_DOS_HEADER));
			WinApi32.IMAGE_NT_HEADERS32 iMAGE_NT_HEADERS = (WinApi32.IMAGE_NT_HEADERS32)Marshal.PtrToStructure(new IntPtr(pImageBase.ToInt32() + iMAGE_DOS_HEADER.e_lfanew), typeof(WinApi32.IMAGE_NT_HEADERS32));
			WinApi.IMAGE_EXPORT_DIRECTORY iMAGE_EXPORT_DIRECTORY = (WinApi.IMAGE_EXPORT_DIRECTORY)Marshal.PtrToStructure(new IntPtr(pImageBase.ToInt32() + iMAGE_NT_HEADERS.OptionalHeader.ExportTable.VirtualAddress), typeof(WinApi.IMAGE_EXPORT_DIRECTORY));
			for (int i = 0; i < iMAGE_EXPORT_DIRECTORY.NumberOfNames; i++)
			{
				uint num = (uint)Marshal.ReadInt32(new IntPtr(pImageBase.ToInt32() + iMAGE_EXPORT_DIRECTORY.AddressOfNames + i * 4));
				string key = Marshal.PtrToStringAnsi(new IntPtr(pImageBase.ToInt32() + num));
				ushort num2 = (ushort)Marshal.ReadInt16(new IntPtr(pImageBase.ToInt32() + iMAGE_EXPORT_DIRECTORY.AddressOfNameOrdinals + i * 2));
				uint num3 = (uint)Marshal.ReadInt32(new IntPtr(pImageBase.ToInt32() + iMAGE_EXPORT_DIRECTORY.AddressOfFunctions + num2 * 4));
				IntPtr intPtr = new IntPtr(pImageBase.ToInt32() + num3);
				Exports.Add(key, intPtr);
			}
		}

		internal static bool lDoQWOIL9HvrT0KRNgG()
		{
			return y61gwtIqQ9GrjtBqCqX == null;
		}
	}
}

using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace hoiuclib
{
	public sealed class MapDll64
	{
		internal static MapDll64 GvHAn3IOVlkfMZ3VWmf;

		internal static IntPtr Internal_MapDll(byte[] RawData)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(RawData.Length);
			Marshal.Copy(RawData, 0, intPtr, RawData.Length);
			WinApi.IMAGE_DOS_HEADER iMAGE_DOS_HEADER = (WinApi.IMAGE_DOS_HEADER)Marshal.PtrToStructure(intPtr, typeof(WinApi.IMAGE_DOS_HEADER));
			IntPtr intPtr2 = new IntPtr(intPtr.ToInt64() + iMAGE_DOS_HEADER.e_lfanew);
			WinApi.IMAGE_FILE_HEADER iMAGE_FILE_HEADER = (WinApi.IMAGE_FILE_HEADER)Marshal.PtrToStructure(new IntPtr(intPtr2.ToInt64() + 4L), typeof(WinApi.IMAGE_FILE_HEADER));
			WinApi64.IMAGE_OPTIONAL_HEADER64 iMAGE_OPTIONAL_HEADER = (WinApi64.IMAGE_OPTIONAL_HEADER64)Marshal.PtrToStructure(new IntPtr(intPtr2.ToInt64() + 24L), typeof(WinApi64.IMAGE_OPTIONAL_HEADER64));
			IntPtr intPtr3 = WinApi.VirtualAlloc(IntPtr.Zero, new UIntPtr(iMAGE_OPTIONAL_HEADER.SizeOfImage), WinApi.AllocationType.COMMIT, WinApi.MemoryProtection.EXECUTE_READWRITE);
			Marshal.Copy(RawData, 0, intPtr3, (int)iMAGE_OPTIONAL_HEADER.SizeOfHeaders);
			intPtr2 = new IntPtr(intPtr2.ToInt64() + 264L - Marshal.SizeOf(typeof(WinApi64.IMAGE_OPTIONAL_HEADER64)) + iMAGE_FILE_HEADER.SizeOfOptionalHeader);
			for (int i = 0; i < iMAGE_FILE_HEADER.NumberOfSections; i++)
			{
				WinApi.IMAGE_SECTION_HEADER iMAGE_SECTION_HEADER = (WinApi.IMAGE_SECTION_HEADER)Marshal.PtrToStructure(new IntPtr(intPtr2.ToInt64() + i * Marshal.SizeOf(typeof(WinApi.IMAGE_SECTION_HEADER))), typeof(WinApi.IMAGE_SECTION_HEADER));
				uint virtualSize = iMAGE_SECTION_HEADER.VirtualSize;
				uint sizeOfRawData = iMAGE_SECTION_HEADER.SizeOfRawData;
				IntPtr intPtr4 = new IntPtr(intPtr3.ToInt64() + iMAGE_SECTION_HEADER.VirtualAddress);
				WinApi.RtlZeroMemory(intPtr4, (int)virtualSize);
				Marshal.Copy(RawData, (int)iMAGE_SECTION_HEADER.PointerToRawData, intPtr4, (int)((virtualSize < sizeOfRawData) ? virtualSize : sizeOfRawData));
			}
			IntPtr intPtr5 = new IntPtr(intPtr3.ToInt64() - (long)iMAGE_OPTIONAL_HEADER.ImageBase);
			uint size = iMAGE_OPTIONAL_HEADER.BaseRelocationTable.Size;
			IntPtr intPtr6 = new IntPtr(intPtr3.ToInt64() + iMAGE_OPTIONAL_HEADER.BaseRelocationTable.VirtualAddress);
			IntPtr ptr = intPtr6;
			while (ptr.ToInt64() < intPtr6.ToInt64() + size)
			{
				WinApi.IMAGE_BASE_RELOCATION iMAGE_BASE_RELOCATION = (WinApi.IMAGE_BASE_RELOCATION)Marshal.PtrToStructure(ptr, typeof(WinApi.IMAGE_BASE_RELOCATION));
				uint num = (uint)((int)iMAGE_BASE_RELOCATION.SizeOfBlock - Marshal.SizeOf(typeof(WinApi.IMAGE_BASE_RELOCATION))) / 2u;
				IntPtr intPtr7 = new IntPtr(ptr.ToInt64() + Marshal.SizeOf(typeof(WinApi.IMAGE_BASE_RELOCATION)));
				for (int j = 0; j < num; j++)
				{
					short num2 = Marshal.ReadInt16(intPtr7);
					if (((uint)num2 & 0xF000u) != 0)
					{
						IntPtr ptr2 = new IntPtr(intPtr3.ToInt64() + iMAGE_BASE_RELOCATION.VirtualAddress + (num2 & 0xFFF));
						Marshal.WriteIntPtr(ptr2, new IntPtr(Marshal.ReadIntPtr(ptr2).ToInt64() + intPtr5.ToInt64()));
					}
					intPtr7 = new IntPtr(intPtr7.ToInt64() + 2L);
				}
				ptr = intPtr7;
			}
			IntPtr ptr3 = new IntPtr(intPtr3.ToInt64() + iMAGE_OPTIONAL_HEADER.ImportTable.VirtualAddress);
			while (true)
			{
				WinApi.IMAGE_IMPORT_DESCRIPTOR iMAGE_IMPORT_DESCRIPTOR = (WinApi.IMAGE_IMPORT_DESCRIPTOR)Marshal.PtrToStructure(ptr3, typeof(WinApi.IMAGE_IMPORT_DESCRIPTOR));
				if (iMAGE_IMPORT_DESCRIPTOR.Name == 0)
				{
					break;
				}
				IntPtr hModule = WinApi.LoadLibraryA(new IntPtr(intPtr3.ToInt64() + iMAGE_IMPORT_DESCRIPTOR.Name));
				IntPtr ptr4 = ((iMAGE_IMPORT_DESCRIPTOR.TimeDateStamp != 0) ? new IntPtr(intPtr3.ToInt64() + iMAGE_IMPORT_DESCRIPTOR.OriginalFirstThunk) : new IntPtr(intPtr3.ToInt64() + iMAGE_IMPORT_DESCRIPTOR.FirstThunk));
				while (true)
				{
					ulong num3 = (ulong)(long)Marshal.ReadIntPtr(ptr4);
					if (num3 <= 0L)
					{
						break;
					}
					IntPtr lpFunctionName = (((num3 & 0x8000000000000000uL) == 0L) ? new IntPtr(intPtr3.ToInt64() + (long)num3 + 2L) : new IntPtr((long)(num3 & 0xFFFFL)));
					IntPtr procAddress_ = WinApi.GetProcAddress_1(hModule, lpFunctionName);
					Marshal.WriteIntPtr(ptr4, procAddress_);
					ptr4 = new IntPtr(ptr4.ToInt64() + IntPtr.Size);
				}
				ptr3 = new IntPtr(ptr3.ToInt64() + Marshal.SizeOf(typeof(WinApi.IMAGE_IMPORT_DESCRIPTOR)));
			}
			WinApi.FlushInstructionCache(new IntPtr(IntPtr.Zero.ToInt64() - 1L), intPtr3, iMAGE_OPTIONAL_HEADER.SizeOfImage);
			WinApi.DllMain dllMain = (WinApi.DllMain)Marshal.GetDelegateForFunctionPointer(new IntPtr(intPtr3.ToInt64() + iMAGE_OPTIONAL_HEADER.AddressOfEntryPoint), typeof(WinApi.DllMain));
			dllMain(intPtr3, WinApi.DLL_PROCESS_ATTACH, 0u);
			dllMain(intPtr3, WinApi.DLL_THREAD_ATTACH, 0u);
			return intPtr3;
		}

		internal static void Internal_SaveExports(IntPtr pImageBase, Hashtable Exports)
		{
			WinApi.IMAGE_DOS_HEADER iMAGE_DOS_HEADER = (WinApi.IMAGE_DOS_HEADER)Marshal.PtrToStructure(pImageBase, typeof(WinApi.IMAGE_DOS_HEADER));
			IntPtr intPtr = new IntPtr(pImageBase.ToInt64() + iMAGE_DOS_HEADER.e_lfanew);
			_ = (WinApi.IMAGE_FILE_HEADER)Marshal.PtrToStructure(new IntPtr(intPtr.ToInt64() + 4L), typeof(WinApi.IMAGE_FILE_HEADER));
			WinApi64.IMAGE_OPTIONAL_HEADER64 iMAGE_OPTIONAL_HEADER = (WinApi64.IMAGE_OPTIONAL_HEADER64)Marshal.PtrToStructure(new IntPtr(intPtr.ToInt64() + 24L), typeof(WinApi64.IMAGE_OPTIONAL_HEADER64));
			WinApi.IMAGE_EXPORT_DIRECTORY iMAGE_EXPORT_DIRECTORY = (WinApi.IMAGE_EXPORT_DIRECTORY)Marshal.PtrToStructure(new IntPtr(pImageBase.ToInt64() + iMAGE_OPTIONAL_HEADER.ExportTable.VirtualAddress), typeof(WinApi.IMAGE_EXPORT_DIRECTORY));
			for (int i = 0; i < iMAGE_EXPORT_DIRECTORY.NumberOfNames; i++)
			{
				uint num = (uint)Marshal.ReadInt32(new IntPtr(pImageBase.ToInt64() + iMAGE_EXPORT_DIRECTORY.AddressOfNames + i * 4));
				string key = Marshal.PtrToStringAnsi(new IntPtr(pImageBase.ToInt64() + num));
				ushort num2 = (ushort)Marshal.ReadInt16(new IntPtr(pImageBase.ToInt64() + iMAGE_EXPORT_DIRECTORY.AddressOfNameOrdinals + i * 2));
				uint num3 = (uint)Marshal.ReadInt32(new IntPtr(pImageBase.ToInt64() + iMAGE_EXPORT_DIRECTORY.AddressOfFunctions + num2 * 4));
				IntPtr intPtr2 = new IntPtr(pImageBase.ToInt64() + num3);
				Exports.Add(key, intPtr2);
			}
		}

		internal static bool lGFoO0IfuWgCeFAtFQx()
		{
			return GvHAn3IOVlkfMZ3VWmf == null;
		}

		internal static void w1yQ29IgCoIZP8Z6X78()
		{
		}
	}
}

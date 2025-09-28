using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace hoiuclib
{
	internal class Stream0 : Stream
	{
		private delegate void Delegate4(byte[] buffer, int count);

		private delegate void Delegate5();

		private IntPtr intptr_0;

		private Control control_0 = null;

		internal static Stream0 H39faIx3lWLk94AeWwa;

		public override bool CanRead => false;

		public override bool CanSeek => false;

		public override bool CanWrite => true;

		public override long Length => 0L;

		public override long Position
		{
			get
			{
				return -1L;
			}
			set
			{
			}
		}

		public Stream0(IntPtr stream, bool bAddRef)
		{
			intptr_0 = stream;
			if (bAddRef)
			{
				cedrus__wrapper.FPC_IStream_AddRef(stream);
			}
			control_0 = new Control();
			control_0.CreateControl();
		}

		~Stream0()
		{
			method_1();
			if (control_0 != null)
			{
				control_0.Dispose();
				control_0 = null;
			}
		}

		public override void Flush()
		{
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return 0L;
		}

		public override void SetLength(long value)
		{
			cedrus__wrapper.FPC_IStream_SetSize(intptr_0, (uint)value);
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return 0;
		}

		private void method_0(byte[] byte_0, int int_0)
		{
			IntPtr intPtr = Marshal.AllocCoTaskMem(int_0);
			Marshal.Copy(byte_0, 0, intPtr, int_0);
			uint WrittenBytes;
			int num = cedrus__wrapper.FPC_IStream_Write(intptr_0, intPtr, int_0, out WrittenBytes);
			Marshal.FreeCoTaskMem(intPtr);
			if (num != 0)
			{
				throw new IOException();
			}
		}

		private void method_1()
		{
			if (IntPtr.Zero != intptr_0)
			{
				cedrus__wrapper.FPC_IStream_Release(intptr_0);
				intptr_0 = IntPtr.Zero;
			}
			if (control_0 != null)
			{
				control_0.Dispose();
				control_0 = null;
			}
		}

		public override void Close()
		{
			if (!(IntPtr.Zero == intptr_0))
			{
				if (control_0.InvokeRequired)
				{
					control_0.Invoke(new Delegate5(method_1));
				}
				else
				{
					method_1();
				}
			}
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			if (offset != 0)
			{
				throw new ArgumentException();
			}
			if (!control_0.InvokeRequired)
			{
				method_0(buffer, count);
				return;
			}
			control_0.Invoke(new Delegate4(method_0), buffer, count);
		}

		internal static void gKt2jLxR2R3sIgCoBE5()
		{
		}

		internal static bool IlGZ9gx23lZf8XjeNdE()
		{
			return H39faIx3lWLk94AeWwa == null;
		}
	}
}

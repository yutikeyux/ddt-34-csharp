using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace MetroFramework
{
	public class MetroMessageBoxProperties
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private MetroMessageBoxControl metroMessageBoxControl_0;

		[CompilerGenerated]
		private MessageBoxButtons messageBoxButtons_0;

		[CompilerGenerated]
		private MessageBoxDefaultButton messageBoxDefaultButton_0;

		[CompilerGenerated]
		private MessageBoxIcon messageBoxIcon_0;

		[CompilerGenerated]
		private string string_0;

		[CompilerGenerated]
		private string string_1;

		internal static MetroMessageBoxProperties daqx3tTogErueeF0FS9;

		public MessageBoxButtons Buttons
		{
			[CompilerGenerated]
			get
			{
				return messageBoxButtons_0;
			}
			[CompilerGenerated]
			set
			{
				messageBoxButtons_0 = value;
			}
		}

		public MessageBoxDefaultButton DefaultButton
		{
			[CompilerGenerated]
			get
			{
				return messageBoxDefaultButton_0;
			}
			[CompilerGenerated]
			set
			{
				messageBoxDefaultButton_0 = value;
			}
		}

		public MessageBoxIcon Icon
		{
			[CompilerGenerated]
			get
			{
				return messageBoxIcon_0;
			}
			[CompilerGenerated]
			set
			{
				messageBoxIcon_0 = value;
			}
		}

		public string Message
		{
			[CompilerGenerated]
			get
			{
				return string_0;
			}
			[CompilerGenerated]
			set
			{
				string_0 = value;
			}
		}

		public MetroMessageBoxControl Owner => metroMessageBoxControl_0;

		public string Title
		{
			[CompilerGenerated]
			get
			{
				return string_1;
			}
			[CompilerGenerated]
			set
			{
				string_1 = value;
			}
		}

		public MetroMessageBoxProperties(MetroMessageBoxControl owner)
		{
			metroMessageBoxControl_0 = owner;
		}

		internal static void DYhiRxTCisRYaN2ndE4()
		{
		}

		internal static bool x2651wTVYPkZIoPn1no()
		{
			return daqx3tTogErueeF0FS9 == null;
		}
	}
}

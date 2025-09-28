using System.ComponentModel;
using System.Net;

namespace ns1
{
	public class BunifuWebClient : WebClient
	{
		private IContainer dtndiYiHtY;

		internal static BunifuWebClient JbJ8QcImhqX2ahrtWQue;

		[Browsable(false)]
		public new bool AllowWriteStreamBuffering
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		[Browsable(false)]
		public new bool AllowReadStreamBuffering
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public BunifuWebClient()
		{
			method_0();
		}

		public BunifuWebClient(IContainer container)
		{
			container.Add(this);
			method_0();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && dtndiYiHtY != null)
			{
				dtndiYiHtY.Dispose();
			}
			base.Dispose(disposing);
		}

		private void method_0()
		{
			dtndiYiHtY = new Container();
		}

		internal static bool vMi1IGImTvKyy7rWX7m2()
		{
			return JbJ8QcImhqX2ahrtWQue == null;
		}

		internal static void sJnqttIm8fEbLt05E4Z2()
		{
		}
	}
}

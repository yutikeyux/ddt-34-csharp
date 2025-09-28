namespace hoiuclib
{
	public class Version
	{
		private short[] short_0 = new short[4];

		internal static Version T6s1yCxzhNALbwje2H4;

		public short this[int index] => short_0[index];

		public short MajorPart => short_0[3];

		public Version(short v3, short v2, short v1, short v0)
		{
			short_0[0] = v0;
			short_0[1] = v1;
			short_0[2] = v2;
			short_0[3] = v3;
		}

		public override string ToString()
		{
			string text = "";
			for (int num = short_0.Length - 1; num >= 0; num--)
			{
				if (text.Length != 0)
				{
					text += ".";
				}
				text += short_0[num];
			}
			return text;
		}

		private int method_0(Version version_0)
		{
			int num = 3;
			while (true)
			{
				if (num >= 0)
				{
					if (this[num] > version_0[num])
					{
						break;
					}
					if (this[num] >= version_0[num])
					{
						num--;
						continue;
					}
					return -1;
				}
				return 0;
			}
			return 1;
		}

		internal static void zlQngd71hq3M4aLTQUx()
		{
		}

		internal static bool tW1GIJ7PedNdNcp9Hgp()
		{
			return T6s1yCxzhNALbwje2H4 == null;
		}
	}
}

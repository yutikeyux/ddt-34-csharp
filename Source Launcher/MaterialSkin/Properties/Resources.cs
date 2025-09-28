using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace MaterialSkin.Properties
{
	[CompilerGenerated]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
	[DebuggerNonUserCode]
	internal class Resources
	{
		private static ResourceManager resourceManager_0;

		private static CultureInfo cultureInfo_0;

		internal static Resources KW7YDxIoGMLxfbtR3wbS;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (resourceManager_0 == null)
				{
					ResourceManager resourceManager = (resourceManager_0 = new ResourceManager("MaterialSkin.Properties.Resources", typeof(Resources).Assembly));
				}
				return resourceManager_0;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return cultureInfo_0;
			}
			set
			{
				cultureInfo_0 = value;
			}
		}

		internal static byte[] Roboto_Medium
		{
			get
			{
				object @object = ResourceManager.GetObject("Roboto_Medium", cultureInfo_0);
				return (byte[])@object;
			}
		}

		internal static byte[] Roboto_Regular
		{
			get
			{
				object @object = ResourceManager.GetObject("Roboto_Regular", cultureInfo_0);
				return (byte[])@object;
			}
		}

		internal Resources()
		{
		}

		internal static void BmyHraIoqjKH4wmIsgl9()
		{
		}

		internal static bool NGuvG0IomSm7Bc4u7Tcu()
		{
			return KW7YDxIoGMLxfbtR3wbS == null;
		}
	}
}

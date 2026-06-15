using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Bussiness
{
	// Token: 0x02000020 RID: 32
	public static class XmlExtends
	{
		// Token: 0x0600026D RID: 621 RVA: 0x00033B68 File Offset: 0x00031D68
		public static string ToString(this XElement node, bool check)
		{
			StringBuilder output = new StringBuilder();
			XmlWriterSettings settings = new XmlWriterSettings
			{
				CheckCharacters = check,
				OmitXmlDeclaration = true,
				Indent = true
			};
			using (XmlWriter writer = XmlWriter.Create(output, settings))
			{
				node.WriteTo(writer);
			}
			return output.ToString();
		}
	}
}

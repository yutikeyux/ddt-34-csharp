using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;

namespace MetroFramework.Drawing.Html
{
	public static class HtmlRenderer
	{
		private static List<Assembly> list_0;

		internal static object HnirOo6fgBlYwjsQUMa;

		public static List<Assembly> References => list_0;

		internal static void AddReference(Assembly assembly)
		{
			if (!list_0.Contains(assembly))
			{
				list_0.Add(assembly);
			}
		}

		static HtmlRenderer()
		{
			list_0 = new List<Assembly>();
			list_0.Add(Assembly.GetExecutingAssembly());
		}

		public static void Render(Graphics g, string html, PointF location, float width)
		{
			Render(g, html, new RectangleF(location, new SizeF(width, 0f)), clip: false);
		}

		public static void Render(Graphics g, string html, RectangleF area, bool clip)
		{
			InitialContainer initialContainer = new InitialContainer(html);
			Region clip2 = g.Clip;
			if (clip)
			{
				g.SetClip(area);
			}
			initialContainer.SetBounds(area);
			initialContainer.MeasureBounds(g);
			initialContainer.Paint(g);
			if (clip)
			{
				g.SetClip(clip2, CombineMode.Replace);
			}
		}

		internal static bool LGCyBc6UGDiivNsXJSv()
		{
			return HnirOo6fgBlYwjsQUMa == null;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MaterialSkin.Controls;
using MaterialSkin.Properties;

namespace MaterialSkin
{
	public class MaterialSkinManager
	{
		public enum Themes : byte
		{
			LIGHT,
			DARK
		}

		private static MaterialSkinManager materialSkinManager_0;

		private readonly List<MaterialForm> list_0 = new List<MaterialForm>();

		private Themes themes_0;

		private ColorScheme colorScheme_0;

		private static readonly Color color_0;

		private static readonly Brush brush_0;

		public static Color SECONDARY_TEXT_BLACK;

		public static Brush SECONDARY_TEXT_BLACK_BRUSH;

		private static readonly Color color_1;

		private static readonly Brush brush_1;

		private static readonly Color color_2;

		private static readonly Brush brush_2;

		private static readonly Color color_3;

		private static readonly Brush brush_3;

		public static Color SECONDARY_TEXT_WHITE;

		public static Brush SECONDARY_TEXT_WHITE_BRUSH;

		private static readonly Color color_4;

		private static readonly Brush brush_4;

		private static readonly Color color_5;

		private static readonly Brush brush_5;

		private static readonly Color color_6;

		private static readonly Brush brush_6;

		private static readonly Color color_7;

		private static readonly Brush brush_7;

		private static readonly Color color_8;

		private static readonly Brush brush_8;

		private static readonly Color color_9;

		private static readonly Brush brush_9;

		private static readonly Color color_10;

		private static readonly Brush brush_10;

		private static readonly Color color_11;

		private static readonly Brush brush_11;

		private static readonly Color color_12;

		private static readonly Brush brush_12;

		private static readonly Color color_13;

		private static readonly Brush brush_13;

		private static readonly Color color_14;

		private static readonly Brush brush_14;

		private static readonly Color color_15;

		private static readonly Brush brush_15;

		private static readonly Color color_16;

		private static readonly Brush brush_16;

		private static readonly Color color_17;

		private static readonly Brush brush_17;

		private static readonly Color color_18;

		private static readonly Brush brush_18;

		private static readonly Color color_19;

		private static readonly Brush brush_19;

		private static readonly Color color_20;

		private static readonly Brush brush_20;

		private static readonly Color color_21;

		private static Brush brush_21;

		private static readonly Color color_22;

		private static Brush brush_22;

		public readonly Color ACTION_BAR_TEXT = Color.FromArgb(255, 255, 255, 255);

		public readonly Brush ACTION_BAR_TEXT_BRUSH = new SolidBrush(Color.FromArgb(255, 255, 255, 255));

		public readonly Color ACTION_BAR_TEXT_SECONDARY = Color.FromArgb(153, 255, 255, 255);

		public readonly Brush ACTION_BAR_TEXT_SECONDARY_BRUSH = new SolidBrush(Color.FromArgb(153, 255, 255, 255));

		public Font ROBOTO_MEDIUM_12;

		public Font ROBOTO_REGULAR_11;

		public Font ROBOTO_MEDIUM_11;

		public Font ROBOTO_MEDIUM_10;

		public int FORM_PADDING = 14;

		private readonly PrivateFontCollection privateFontCollection_0 = new PrivateFontCollection();

		private static MaterialSkinManager xK1YoFIoPo9VB1IBn0uv;

		public Themes Theme
		{
			get
			{
				return themes_0;
			}
			set
			{
				themes_0 = value;
				method_1();
			}
		}

		public ColorScheme ColorScheme
		{
			get
			{
				return colorScheme_0;
			}
			set
			{
				colorScheme_0 = value;
				method_1();
			}
		}

		public static MaterialSkinManager Instance => materialSkinManager_0 ?? (materialSkinManager_0 = new MaterialSkinManager());

		public Color GetPrimaryTextColor()
		{
			return (Theme == Themes.LIGHT) ? color_0 : color_3;
		}

		public Brush GetPrimaryTextBrush()
		{
			return (Theme == Themes.LIGHT) ? brush_0 : brush_3;
		}

		public Color GetSecondaryTextColor()
		{
			return (Theme == Themes.LIGHT) ? SECONDARY_TEXT_BLACK : SECONDARY_TEXT_WHITE;
		}

		public Brush GetSecondaryTextBrush()
		{
			return (Theme != 0) ? SECONDARY_TEXT_WHITE_BRUSH : SECONDARY_TEXT_BLACK_BRUSH;
		}

		public Color GetDisabledOrHintColor()
		{
			return (Theme == Themes.LIGHT) ? color_1 : color_4;
		}

		public Brush GetDisabledOrHintBrush()
		{
			return (Theme == Themes.LIGHT) ? brush_1 : brush_4;
		}

		public Color GetDividersColor()
		{
			return (Theme != 0) ? color_5 : color_2;
		}

		public Brush GetDividersBrush()
		{
			return (Theme == Themes.LIGHT) ? brush_2 : brush_5;
		}

		public Color GetCheckboxOffColor()
		{
			return (Theme != 0) ? color_8 : color_6;
		}

		public Brush GetCheckboxOffBrush()
		{
			return (Theme == Themes.LIGHT) ? brush_6 : brush_8;
		}

		public Color GetCheckBoxOffDisabledColor()
		{
			return (Theme == Themes.LIGHT) ? color_7 : color_9;
		}

		public Brush GetCheckBoxOffDisabledBrush()
		{
			return (Theme == Themes.LIGHT) ? brush_7 : brush_9;
		}

		public Brush GetRaisedButtonBackgroundBrush()
		{
			return brush_10;
		}

		public Brush GetRaisedButtonTextBrush(bool primary)
		{
			return (!primary) ? brush_12 : brush_11;
		}

		public Color GetFlatButtonHoverBackgroundColor()
		{
			return (Theme == Themes.LIGHT) ? color_13 : color_16;
		}

		public Brush GetFlatButtonHoverBackgroundBrush()
		{
			return (Theme == Themes.LIGHT) ? brush_13 : brush_16;
		}

		public Color GetFlatButtonPressedBackgroundColor()
		{
			return (Theme == Themes.LIGHT) ? color_14 : color_17;
		}

		public Brush GetFlatButtonPressedBackgroundBrush()
		{
			return (Theme == Themes.LIGHT) ? brush_14 : brush_17;
		}

		public Brush GetFlatButtonDisabledTextBrush()
		{
			return (Theme == Themes.LIGHT) ? brush_15 : brush_18;
		}

		public Brush GetCmsSelectedItemBrush()
		{
			return (Theme != 0) ? brush_20 : brush_19;
		}

		public Color GetApplicationBackgroundColor()
		{
			return (Theme != 0) ? color_22 : color_21;
		}

		[DllImport("Gdi32.dll")]
		private static extern IntPtr AddFontMemResourceEx(IntPtr intptr_0, uint uint_0, IntPtr intptr_1, [In] ref uint uint_1);

		private MaterialSkinManager()
		{
			ROBOTO_MEDIUM_12 = new Font(method_0(Resources.Roboto_Medium), 12f);
			ROBOTO_MEDIUM_10 = new Font(method_0(Resources.Roboto_Medium), 10f);
			ROBOTO_REGULAR_11 = new Font(method_0(Resources.Roboto_Regular), 11f);
			ROBOTO_MEDIUM_11 = new Font(method_0(Resources.Roboto_Medium), 11f);
			Theme = Themes.LIGHT;
			ColorScheme = new ColorScheme(Primary.const_188, Primary.const_189, Primary.const_185, Accent.LightBlue200, TextShade.WHITE);
		}

		public void AddFormToManage(MaterialForm materialForm)
		{
			list_0.Add(materialForm);
			method_1();
		}

		public void RemoveFormToManage(MaterialForm materialForm)
		{
			list_0.Remove(materialForm);
		}

		private FontFamily method_0(byte[] byte_0)
		{
			int num = byte_0.Length;
			IntPtr intPtr = Marshal.AllocCoTaskMem(num);
			Marshal.Copy(byte_0, 0, intPtr, num);
			uint uint_ = 0u;
			AddFontMemResourceEx(intPtr, (uint)byte_0.Length, IntPtr.Zero, ref uint_);
			privateFontCollection_0.AddMemoryFont(intPtr, num);
			return privateFontCollection_0.Families.Last();
		}

		private void method_1()
		{
			Color applicationBackgroundColor = GetApplicationBackgroundColor();
			foreach (MaterialForm item in list_0)
			{
				item.BackColor = applicationBackgroundColor;
				method_3(item, applicationBackgroundColor);
			}
		}

		private void method_2(ToolStrip toolStrip_0, Color color_23)
		{
			if (toolStrip_0 == null)
			{
				return;
			}
			toolStrip_0.BackColor = color_23;
			foreach (ToolStripItem item in toolStrip_0.Items)
			{
				item.BackColor = color_23;
			}
		}

		private void method_3(Control control_0, Color color_23)
		{
			if (control_0 == null)
			{
				return;
			}
			if (control_0.ContextMenuStrip != null)
			{
				method_2(control_0.ContextMenuStrip, color_23);
			}
			MaterialTabControl materialTabControl = control_0 as MaterialTabControl;
			if (materialTabControl != null)
			{
				foreach (TabPage tabPage in materialTabControl.TabPages)
				{
					tabPage.BackColor = color_23;
				}
			}
			if (control_0 is MaterialDivider)
			{
				control_0.BackColor = GetDividersColor();
			}
			if (control_0 is MaterialListView)
			{
				control_0.BackColor = color_23;
			}
			foreach (Control control in control_0.Controls)
			{
				method_3(control, color_23);
			}
			control_0.Invalidate();
		}

		static MaterialSkinManager()
		{
			color_0 = Color.FromArgb(222, 0, 0, 0);
			brush_0 = new SolidBrush(color_0);
			SECONDARY_TEXT_BLACK = Color.FromArgb(138, 0, 0, 0);
			SECONDARY_TEXT_BLACK_BRUSH = new SolidBrush(SECONDARY_TEXT_BLACK);
			color_1 = Color.FromArgb(66, 0, 0, 0);
			brush_1 = new SolidBrush(color_1);
			color_2 = Color.FromArgb(31, 0, 0, 0);
			brush_2 = new SolidBrush(color_2);
			color_3 = Color.FromArgb(255, 255, 255, 255);
			brush_3 = new SolidBrush(color_3);
			SECONDARY_TEXT_WHITE = Color.FromArgb(179, 255, 255, 255);
			SECONDARY_TEXT_WHITE_BRUSH = new SolidBrush(SECONDARY_TEXT_WHITE);
			color_4 = Color.FromArgb(77, 255, 255, 255);
			brush_4 = new SolidBrush(color_4);
			color_5 = Color.FromArgb(31, 255, 255, 255);
			brush_5 = new SolidBrush(color_5);
			color_6 = Color.FromArgb(138, 0, 0, 0);
			brush_6 = new SolidBrush(color_6);
			color_7 = Color.FromArgb(66, 0, 0, 0);
			brush_7 = new SolidBrush(color_7);
			color_8 = Color.FromArgb(179, 255, 255, 255);
			brush_8 = new SolidBrush(color_8);
			color_9 = Color.FromArgb(77, 255, 255, 255);
			brush_9 = new SolidBrush(color_9);
			color_10 = Color.FromArgb(255, 255, 255, 255);
			brush_10 = new SolidBrush(color_10);
			color_11 = color_3;
			brush_11 = new SolidBrush(color_11);
			color_12 = color_0;
			brush_12 = new SolidBrush(color_12);
			color_13 = Color.FromArgb(20.PercentageToColorComponent(), 10066329.ToColor());
			brush_13 = new SolidBrush(color_13);
			color_14 = Color.FromArgb(40.PercentageToColorComponent(), 10066329.ToColor());
			brush_14 = new SolidBrush(color_14);
			color_15 = Color.FromArgb(26.PercentageToColorComponent(), 0.ToColor());
			brush_15 = new SolidBrush(color_15);
			color_16 = Color.FromArgb(15.PercentageToColorComponent(), 13421772.ToColor());
			brush_16 = new SolidBrush(color_16);
			color_17 = Color.FromArgb(25.PercentageToColorComponent(), 13421772.ToColor());
			brush_17 = new SolidBrush(color_17);
			color_18 = Color.FromArgb(30.PercentageToColorComponent(), 16777215.ToColor());
			brush_18 = new SolidBrush(color_18);
			color_19 = Color.FromArgb(255, 238, 238, 238);
			brush_19 = new SolidBrush(color_19);
			color_20 = Color.FromArgb(38, 204, 204, 204);
			brush_20 = new SolidBrush(color_20);
			color_21 = Color.FromArgb(255, 255, 255, 255);
			brush_21 = new SolidBrush(color_21);
			color_22 = Color.FromArgb(255, 51, 51, 51);
			brush_22 = new SolidBrush(color_22);
		}

		internal static bool t5HZKKIoI6oKUORA9Gk7()
		{
			return xK1YoFIoPo9VB1IBn0uv == null;
		}

		internal static void K8S64WIo7PJtLCsnQ0aD()
		{
		}
	}
}

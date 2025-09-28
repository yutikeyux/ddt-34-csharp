using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Text.RegularExpressions;

namespace MetroFramework.Drawing.Html
{
	[CLSCompliant(false)]
	public class CssBox
	{
		internal static readonly CssBox Empty;

		internal static Dictionary<string, PropertyInfo> _properties;

		private static Dictionary<string, string> dictionary_0;

		private static List<PropertyInfo> list_0;

		private static List<PropertyInfo> list_1;

		private string string_0;

		private string string_1;

		private string string_2;

		private string string_3;

		private string string_4;

		private string string_5;

		private string string_6;

		private string string_7;

		private string string_8;

		private string string_9;

		private string string_10;

		private string string_11;

		private string string_12;

		private string string_13;

		private string string_14;

		private string string_15;

		private string string_16;

		private string string_17;

		private string string_18;

		private string string_19;

		private string string_20;

		private string string_21;

		private string string_22;

		private string string_23;

		private string string_24;

		private string string_25;

		private string string_26;

		private string string_27;

		private string string_28;

		private string string_29;

		private string string_30;

		private string string_31;

		private string string_32;

		private string string_33;

		private string string_34;

		private string string_35;

		private string string_36;

		private string string_37;

		private string string_38;

		private string string_39;

		private string string_40;

		private string string_41;

		private string string_42;

		private string KosEyIhSlC;

		private string string_43;

		private string string_44;

		private string string_45;

		private string string_46;

		private string string_47;

		private string string_48;

		private string string_49;

		private string string_50;

		private string string_51;

		private string string_52;

		private string string_53;

		private string string_54;

		private string string_55;

		private string string_56;

		private string string_57;

		private string string_58;

		private string string_59;

		private string string_60;

		private string string_61;

		private string string_62;

		private string string_63;

		private string string_64;

		private string string_65;

		private string string_66;

		private string string_67;

		private string string_68;

		internal bool TableFixed;

		private List<CssBoxWord> list_2;

		private List<CssBox> list_3;

		private CssBox cssBox_0;

		private bool bool_0;

		private SizeF sizeF_0;

		private PointF pointF_0;

		private List<CssLineBox> list_4;

		private List<CssLineBox> list_5;

		private float float_0 = float.NaN;

		private float float_1 = float.NaN;

		private float float_2 = float.NaN;

		private HtmlTag htmlTag_0;

		private Dictionary<CssLineBox, RectangleF> dictionary_1;

		protected InitialContainer _initialContainer;

		private CssBox cssBox_1;

		private CssLineBox cssLineBox_0;

		private CssLineBox cssLineBox_1;

		private float float_3 = float.NaN;

		private float float_4 = float.NaN;

		private float float_5 = float.NaN;

		private float float_6 = float.NaN;

		private Color kcFwwjpJu6 = System.Drawing.Color.Empty;

		private float float_7 = float.NaN;

		private float float_8 = float.NaN;

		private float float_9 = float.NaN;

		private float float_10 = float.NaN;

		private float float_11 = float.NaN;

		private float float_12 = float.NaN;

		private float float_13 = float.NaN;

		private float float_14 = float.NaN;

		private float float_15 = float.NaN;

		private float float_16 = float.NaN;

		private float float_17 = float.NaN;

		private float float_18 = float.NaN;

		private float float_19 = float.NaN;

		private Color color_0 = System.Drawing.Color.Empty;

		private Color color_1 = System.Drawing.Color.Empty;

		private Color color_2 = System.Drawing.Color.Empty;

		private Color color_3 = System.Drawing.Color.Empty;

		private Color color_4 = System.Drawing.Color.Empty;

		private float float_20 = float.NaN;

		private Color color_5 = System.Drawing.Color.Empty;

		private Font font_0;

		private float float_21 = float.NaN;

		private float float_22 = float.NaN;

		private float float_23 = float.NaN;

		internal static CssBox SdLVLbY2ypcCv7kILCK;

		[CssProperty("border-bottom-width")]
		[DefaultValue("medium")]
		public string BorderBottomWidth
		{
			get
			{
				return string_7;
			}
			set
			{
				string_7 = value;
			}
		}

		[CssProperty("border-left-width")]
		[DefaultValue("medium")]
		public string BorderLeftWidth
		{
			get
			{
				return string_8;
			}
			set
			{
				string_8 = value;
			}
		}

		[CssProperty("border-right-width")]
		[DefaultValue("medium")]
		public string BorderRightWidth
		{
			get
			{
				return string_6;
			}
			set
			{
				string_6 = value;
			}
		}

		[CssProperty("border-top-width")]
		[DefaultValue("medium")]
		public string BorderTopWidth
		{
			get
			{
				return string_5;
			}
			set
			{
				string_5 = value;
			}
		}

		[CssProperty("border-width")]
		[DefaultValue("")]
		public string BorderWidth
		{
			get
			{
				return string_9;
			}
			set
			{
				string_9 = value;
				string[] array = CssValue.SplitValues(value);
				switch (array.Length)
				{
				case 1:
				{
					string text11 = (BorderBottomWidth = array[0]);
					string text13 = (BorderRightWidth = text11);
					string text16 = (BorderTopWidth = (BorderLeftWidth = text13));
					break;
				}
				case 2:
				{
					string text6 = (BorderTopWidth = (BorderBottomWidth = array[0]));
					string text9 = (BorderLeftWidth = (BorderRightWidth = array[1]));
					break;
				}
				case 3:
				{
					BorderTopWidth = array[0];
					string text3 = (BorderLeftWidth = (BorderRightWidth = array[1]));
					BorderBottomWidth = array[2];
					break;
				}
				case 4:
					BorderTopWidth = array[0];
					BorderRightWidth = array[1];
					BorderBottomWidth = array[2];
					BorderLeftWidth = array[3];
					break;
				}
			}
		}

		[DefaultValue("none")]
		[CssProperty("border-bottom-style")]
		public string BorderBottomStyle
		{
			get
			{
				return string_17;
			}
			set
			{
				string_17 = value;
			}
		}

		[CssProperty("border-left-style")]
		[DefaultValue("none")]
		public string BorderLeftStyle
		{
			get
			{
				return string_18;
			}
			set
			{
				string_18 = value;
			}
		}

		[CssProperty("border-right-style")]
		[DefaultValue("none")]
		public string BorderRightStyle
		{
			get
			{
				return string_16;
			}
			set
			{
				string_16 = value;
			}
		}

		[CssProperty("border-style")]
		[DefaultValue("")]
		public string BorderStyle
		{
			get
			{
				return string_19;
			}
			set
			{
				string_19 = value;
				string[] array = CssValue.SplitValues(value);
				switch (array.Length)
				{
				case 1:
				{
					string text11 = (BorderBottomStyle = array[0]);
					string text13 = (BorderRightStyle = text11);
					string text16 = (BorderTopStyle = (BorderLeftStyle = text13));
					break;
				}
				case 2:
				{
					string text6 = (BorderTopStyle = (BorderBottomStyle = array[0]));
					string text9 = (BorderLeftStyle = (BorderRightStyle = array[1]));
					break;
				}
				case 3:
				{
					BorderTopStyle = array[0];
					string text3 = (BorderLeftStyle = (BorderRightStyle = array[1]));
					BorderBottomStyle = array[2];
					break;
				}
				case 4:
					BorderTopStyle = array[0];
					BorderRightStyle = array[1];
					BorderBottomStyle = array[2];
					BorderLeftStyle = array[3];
					break;
				}
			}
		}

		[DefaultValue("none")]
		[CssProperty("border-top-style")]
		public string BorderTopStyle
		{
			get
			{
				return string_15;
			}
			set
			{
				string_15 = value;
			}
		}

		[DefaultValue("black")]
		[CssProperty("border-color")]
		public string BorderColor
		{
			get
			{
				return string_14;
			}
			set
			{
				string_14 = value;
				MatchCollection matchCollection = Parser.Match("(#\\S{6}|#\\S{3}|rgb\\(\\s*[0-9]{1,3}\\%?\\s*\\,\\s*[0-9]{1,3}\\%?\\s*\\,\\s*[0-9]{1,3}\\%?\\s*\\)|maroon|red|orange|yellow|olive|purple|fuchsia|white|lime|green|navy|blue|aqua|teal|black|silver|gray)", value);
				string[] array = (string[])(object)new string[matchCollection.Count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = matchCollection[i].Value;
				}
				switch (array.Length)
				{
				case 1:
				{
					string text11 = (BorderBottomColor = array[0]);
					string text13 = (BorderRightColor = text11);
					string text16 = (BorderTopColor = (BorderLeftColor = text13));
					break;
				}
				case 2:
				{
					string text6 = (BorderTopColor = (BorderBottomColor = array[0]));
					string text9 = (BorderLeftColor = (BorderRightColor = array[1]));
					break;
				}
				case 3:
				{
					BorderTopColor = array[0];
					string text3 = (BorderLeftColor = (BorderRightColor = array[1]));
					BorderBottomColor = array[2];
					break;
				}
				case 4:
					BorderTopColor = array[0];
					BorderRightColor = array[1];
					BorderBottomColor = array[2];
					BorderLeftColor = array[3];
					break;
				}
			}
		}

		[DefaultValue("black")]
		[CssProperty("border-bottom-color")]
		public string BorderBottomColor
		{
			get
			{
				return string_12;
			}
			set
			{
				string_12 = value;
			}
		}

		[DefaultValue("black")]
		[CssProperty("border-left-color")]
		public string BorderLeftColor
		{
			get
			{
				return string_13;
			}
			set
			{
				string_13 = value;
			}
		}

		[CssProperty("border-right-color")]
		[DefaultValue("black")]
		public string BorderRightColor
		{
			get
			{
				return string_11;
			}
			set
			{
				string_11 = value;
			}
		}

		[CssProperty("border-top-color")]
		[DefaultValue("black")]
		public string BorderTopColor
		{
			get
			{
				return string_10;
			}
			set
			{
				string_10 = value;
			}
		}

		[CssProperty("border")]
		[DefaultValue("")]
		public string Border
		{
			get
			{
				return string_26;
			}
			set
			{
				string_26 = value;
				string text = Parser.Search("(([0-9]+|[0-9]*\\.[0-9]+)(em|ex|px|in|cm|mm|pt|pc)|thin|medium|thick)", value);
				string text2 = Parser.Search("(none|hidden|dotted|dashed|solid|double|groove|ridge|inset|outset)", value);
				string text3 = Parser.Search("(#\\S{6}|#\\S{3}|rgb\\(\\s*[0-9]{1,3}\\%?\\s*\\,\\s*[0-9]{1,3}\\%?\\s*\\,\\s*[0-9]{1,3}\\%?\\s*\\)|maroon|red|orange|yellow|olive|purple|fuchsia|white|lime|green|navy|blue|aqua|teal|black|silver|gray)", value);
				if (text != null)
				{
					BorderWidth = text;
				}
				if (text2 != null)
				{
					BorderStyle = text2;
				}
				if (text3 != null)
				{
					BorderColor = text3;
				}
			}
		}

		[DefaultValue("")]
		[CssProperty("border-bottom")]
		public string BorderBottom
		{
			get
			{
				return string_20;
			}
			set
			{
				string_20 = value;
				string text = Parser.Search("(([0-9]+|[0-9]*\\.[0-9]+)(em|ex|px|in|cm|mm|pt|pc)|thin|medium|thick)", value);
				string text2 = Parser.Search("(none|hidden|dotted|dashed|solid|double|groove|ridge|inset|outset)", value);
				string text3 = Parser.Search("(#\\S{6}|#\\S{3}|rgb\\(\\s*[0-9]{1,3}\\%?\\s*\\,\\s*[0-9]{1,3}\\%?\\s*\\,\\s*[0-9]{1,3}\\%?\\s*\\)|maroon|red|orange|yellow|olive|purple|fuchsia|white|lime|green|navy|blue|aqua|teal|black|silver|gray)", value);
				if (text != null)
				{
					BorderBottomWidth = text;
				}
				if (text2 != null)
				{
					BorderBottomStyle = text2;
				}
				if (text3 != null)
				{
					BorderBottomColor = text3;
				}
			}
		}

		[DefaultValue("")]
		[CssProperty("border-left")]
		public string BorderLeft
		{
			get
			{
				return string_21;
			}
			set
			{
				string_21 = value;
				string text = Parser.Search("(([0-9]+|[0-9]*\\.[0-9]+)(em|ex|px|in|cm|mm|pt|pc)|thin|medium|thick)", value);
				string text2 = Parser.Search("(none|hidden|dotted|dashed|solid|double|groove|ridge|inset|outset)", value);
				string text3 = Parser.Search("(#\\S{6}|#\\S{3}|rgb\\(\\s*[0-9]{1,3}\\%?\\s*\\,\\s*[0-9]{1,3}\\%?\\s*\\,\\s*[0-9]{1,3}\\%?\\s*\\)|maroon|red|orange|yellow|olive|purple|fuchsia|white|lime|green|navy|blue|aqua|teal|black|silver|gray)", value);
				if (text != null)
				{
					BorderLeftWidth = text;
				}
				if (text2 != null)
				{
					BorderLeftStyle = text2;
				}
				if (text3 != null)
				{
					BorderLeftColor = text3;
				}
			}
		}

		[CssProperty("border-right")]
		[DefaultValue("")]
		public string BorderRight
		{
			get
			{
				return string_22;
			}
			set
			{
				string_22 = value;
				string text = Parser.Search("(([0-9]+|[0-9]*\\.[0-9]+)(em|ex|px|in|cm|mm|pt|pc)|thin|medium|thick)", value);
				string text2 = Parser.Search("(none|hidden|dotted|dashed|solid|double|groove|ridge|inset|outset)", value);
				string text3 = Parser.Search("(#\\S{6}|#\\S{3}|rgb\\(\\s*[0-9]{1,3}\\%?\\s*\\,\\s*[0-9]{1,3}\\%?\\s*\\,\\s*[0-9]{1,3}\\%?\\s*\\)|maroon|red|orange|yellow|olive|purple|fuchsia|white|lime|green|navy|blue|aqua|teal|black|silver|gray)", value);
				if (text != null)
				{
					BorderRightWidth = text;
				}
				if (text2 != null)
				{
					BorderRightStyle = text2;
				}
				if (text3 != null)
				{
					BorderRightColor = text3;
				}
			}
		}

		[CssProperty("border-top")]
		[DefaultValue("")]
		public string BorderTop
		{
			get
			{
				return string_23;
			}
			set
			{
				string_23 = value;
				string text = Parser.Search("(([0-9]+|[0-9]*\\.[0-9]+)(em|ex|px|in|cm|mm|pt|pc)|thin|medium|thick)", value);
				string text2 = Parser.Search("(none|hidden|dotted|dashed|solid|double|groove|ridge|inset|outset)", value);
				string text3 = Parser.Search("(#\\S{6}|#\\S{3}|rgb\\(\\s*[0-9]{1,3}\\%?\\s*\\,\\s*[0-9]{1,3}\\%?\\s*\\,\\s*[0-9]{1,3}\\%?\\s*\\)|maroon|red|orange|yellow|olive|purple|fuchsia|white|lime|green|navy|blue|aqua|teal|black|silver|gray)", value);
				if (text != null)
				{
					BorderTopWidth = text;
				}
				if (text2 != null)
				{
					BorderTopStyle = text2;
				}
				if (text3 != null)
				{
					BorderTopColor = text3;
				}
			}
		}

		[CssPropertyInherited]
		[CssProperty("border-spacing")]
		[DefaultValue("0")]
		public string BorderSpacing
		{
			get
			{
				return string_24;
			}
			set
			{
				string_24 = value;
			}
		}

		[DefaultValue("separate")]
		[CssProperty("border-collapse")]
		[CssPropertyInherited]
		public string BorderCollapse
		{
			get
			{
				return string_25;
			}
			set
			{
				string_25 = value;
			}
		}

		[CssProperty("corner-radius")]
		[DefaultValue("0")]
		public string CornerRadius
		{
			get
			{
				return string_32;
			}
			set
			{
				MatchCollection matchCollection = Parser.Match("([0-9]+|[0-9]*\\.[0-9]+)(em|ex|px|in|cm|mm|pt|pc)", value);
				switch (matchCollection.Count)
				{
				case 1:
					CornerNERadius = matchCollection[0].Value;
					CornerNWRadius = matchCollection[0].Value;
					CornerSERadius = matchCollection[0].Value;
					CornerSWRadius = matchCollection[0].Value;
					break;
				case 2:
					CornerNERadius = matchCollection[0].Value;
					CornerNWRadius = matchCollection[0].Value;
					CornerSERadius = matchCollection[1].Value;
					CornerSWRadius = matchCollection[1].Value;
					break;
				case 3:
					CornerNERadius = matchCollection[0].Value;
					CornerNWRadius = matchCollection[1].Value;
					CornerSERadius = matchCollection[2].Value;
					break;
				case 4:
					CornerNERadius = matchCollection[0].Value;
					CornerNWRadius = matchCollection[1].Value;
					CornerSERadius = matchCollection[2].Value;
					CornerSWRadius = matchCollection[3].Value;
					break;
				}
				string_32 = value;
			}
		}

		[CssProperty("corner-nw-radius")]
		[DefaultValue("0")]
		public string CornerNWRadius
		{
			get
			{
				return string_28;
			}
			set
			{
				string_28 = value;
			}
		}

		[DefaultValue("0")]
		[CssProperty("corner-ne-radius")]
		public string CornerNERadius
		{
			get
			{
				return string_29;
			}
			set
			{
				string_29 = value;
			}
		}

		[CssProperty("corner-se-radius")]
		[DefaultValue("0")]
		public string CornerSERadius
		{
			get
			{
				return string_30;
			}
			set
			{
				string_30 = value;
			}
		}

		[CssProperty("corner-sw-radius")]
		[DefaultValue("0")]
		public string CornerSWRadius
		{
			get
			{
				return string_31;
			}
			set
			{
				string_31 = value;
			}
		}

		[CssProperty("margin")]
		[DefaultValue("")]
		public string Margin
		{
			get
			{
				return string_47;
			}
			set
			{
				string_47 = value;
				string[] array = CssValue.SplitValues(value);
				switch (array.Length)
				{
				case 1:
				{
					string text11 = (MarginBottom = array[0]);
					string text13 = (MarginRight = text11);
					string text16 = (MarginTop = (MarginLeft = text13));
					break;
				}
				case 2:
				{
					string text6 = (MarginTop = (MarginBottom = array[0]));
					string text9 = (MarginLeft = (MarginRight = array[1]));
					break;
				}
				case 3:
				{
					MarginTop = array[0];
					string text3 = (MarginLeft = (MarginRight = array[1]));
					MarginBottom = array[2];
					break;
				}
				case 4:
					MarginTop = array[0];
					MarginRight = array[1];
					MarginBottom = array[2];
					MarginLeft = array[3];
					break;
				}
			}
		}

		[CssProperty("margin-bottom")]
		[DefaultValue("0")]
		public string MarginBottom
		{
			get
			{
				return string_43;
			}
			set
			{
				string_43 = value;
			}
		}

		[CssProperty("margin-left")]
		[DefaultValue("0")]
		public string MarginLeft
		{
			get
			{
				return string_44;
			}
			set
			{
				string_44 = value;
			}
		}

		[CssProperty("margin-right")]
		[DefaultValue("0")]
		public string MarginRight
		{
			get
			{
				return string_45;
			}
			set
			{
				string_45 = value;
			}
		}

		[CssProperty("margin-top")]
		[DefaultValue("0")]
		public string MarginTop
		{
			get
			{
				return string_46;
			}
			set
			{
				string_46 = value;
			}
		}

		[DefaultValue("")]
		[CssProperty("padding")]
		public string Padding
		{
			get
			{
				return string_58;
			}
			set
			{
				string_58 = value;
				string[] array = CssValue.SplitValues(value);
				switch (array.Length)
				{
				case 1:
				{
					string text11 = (PaddingBottom = array[0]);
					string text13 = (PaddingRight = text11);
					string text16 = (PaddingTop = (PaddingLeft = text13));
					break;
				}
				case 2:
				{
					string text6 = (PaddingTop = (PaddingBottom = array[0]));
					string text9 = (PaddingLeft = (PaddingRight = array[1]));
					break;
				}
				case 3:
				{
					PaddingTop = array[0];
					string text3 = (PaddingLeft = (PaddingRight = array[1]));
					PaddingBottom = array[2];
					break;
				}
				case 4:
					PaddingTop = array[0];
					PaddingRight = array[1];
					PaddingBottom = array[2];
					PaddingLeft = array[3];
					break;
				}
			}
		}

		[CssProperty("padding-bottom")]
		[DefaultValue("0")]
		public string PaddingBottom
		{
			get
			{
				return string_55;
			}
			set
			{
				string_55 = value;
				float_9 = float.NaN;
			}
		}

		[CssProperty("padding-left")]
		[DefaultValue("0")]
		public string PaddingLeft
		{
			get
			{
				return string_54;
			}
			set
			{
				string_54 = value;
				float_11 = float.NaN;
			}
		}

		[DefaultValue("0")]
		[CssProperty("padding-right")]
		public string PaddingRight
		{
			get
			{
				return string_56;
			}
			set
			{
				string_56 = value;
				float_10 = float.NaN;
			}
		}

		[CssProperty("padding-top")]
		[DefaultValue("0")]
		public string PaddingTop
		{
			get
			{
				return string_57;
			}
			set
			{
				string_57 = value;
				float_8 = float.NaN;
			}
		}

		[CssProperty("left")]
		[DefaultValue("auto")]
		public string Left
		{
			get
			{
				return string_48;
			}
			set
			{
				string_48 = value;
			}
		}

		[DefaultValue("auto")]
		[CssProperty("top")]
		public string Top
		{
			get
			{
				return string_63;
			}
			set
			{
				string_63 = value;
			}
		}

		[CssProperty("width")]
		[DefaultValue("auto")]
		public string Width
		{
			get
			{
				return string_66;
			}
			set
			{
				string_66 = value;
			}
		}

		[DefaultValue("auto")]
		[CssProperty("height")]
		public string Height
		{
			get
			{
				return KosEyIhSlC;
			}
			set
			{
				KosEyIhSlC = value;
			}
		}

		[CssProperty("background-color")]
		[DefaultValue("transparent")]
		public string BackgroundColor
		{
			get
			{
				return string_0;
			}
			set
			{
				string_0 = value;
			}
		}

		[DefaultValue("none")]
		[CssProperty("background-image")]
		public string BackgroundImage
		{
			get
			{
				return string_3;
			}
			set
			{
				string_3 = value;
			}
		}

		[DefaultValue("repeat")]
		[CssProperty("background-repeat")]
		public string BackgroundRepeat
		{
			get
			{
				return string_4;
			}
			set
			{
				string_4 = value;
			}
		}

		[CssProperty("background-gradient")]
		[DefaultValue("none")]
		public string BackgroundGradient
		{
			get
			{
				return string_1;
			}
			set
			{
				string_1 = value;
			}
		}

		[CssProperty("background-gradient-angle")]
		[DefaultValue("90")]
		public string BackgroundGradientAngle
		{
			get
			{
				return string_2;
			}
			set
			{
				string_2 = value;
			}
		}

		[DefaultValue("black")]
		[CssPropertyInherited]
		[CssProperty("color")]
		public string Color
		{
			get
			{
				return string_27;
			}
			set
			{
				string_27 = value;
				kcFwwjpJu6 = System.Drawing.Color.Empty;
			}
		}

		[CssProperty("display")]
		[DefaultValue("inline")]
		public string Display
		{
			get
			{
				return string_35;
			}
			set
			{
				string_35 = value;
			}
		}

		[CssProperty("direction")]
		[DefaultValue("ltr")]
		public string Direction
		{
			get
			{
				return string_34;
			}
			set
			{
				string_34 = value;
			}
		}

		[CssProperty("empty-cells")]
		[DefaultValue("show")]
		[CssPropertyInherited]
		public string EmptyCells
		{
			get
			{
				return string_33;
			}
			set
			{
				string_33 = value;
			}
		}

		[CssProperty("float")]
		[DefaultValue("none")]
		public string Float
		{
			get
			{
				return string_42;
			}
			set
			{
				string_42 = value;
			}
		}

		[DefaultValue("static")]
		[CssProperty("position")]
		public string Position
		{
			get
			{
				return string_64;
			}
			set
			{
				string_64 = value;
			}
		}

		[DefaultValue("normal")]
		[CssProperty("line-height")]
		public string LineHeight
		{
			get
			{
				return string_49;
			}
			set
			{
				string_49 = method_9(value);
			}
		}

		[CssProperty("vertical-align")]
		[CssPropertyInherited]
		[DefaultValue("baseline")]
		public string VerticalAlign
		{
			get
			{
				return string_65;
			}
			set
			{
				string_65 = value;
			}
		}

		[DefaultValue("0")]
		[CssProperty("text-indent")]
		[CssPropertyInherited]
		public string TextIndent
		{
			get
			{
				return string_62;
			}
			set
			{
				string_62 = method_9(value);
			}
		}

		[CssProperty("text-align")]
		[DefaultValue("")]
		[CssPropertyInherited]
		public string TextAlign
		{
			get
			{
				return string_60;
			}
			set
			{
				string_60 = value;
			}
		}

		[DefaultValue("")]
		[CssProperty("text-decoration")]
		public string TextDecoration
		{
			get
			{
				return string_61;
			}
			set
			{
				string_61 = value;
			}
		}

		[CssPropertyInherited]
		[DefaultValue("normal")]
		[CssProperty("white-space")]
		public string WhiteSpace
		{
			get
			{
				return string_68;
			}
			set
			{
				string_68 = value;
			}
		}

		[CssProperty("word-spacing")]
		[DefaultValue("normal")]
		public string WordSpacing
		{
			get
			{
				return string_67;
			}
			set
			{
				string_67 = method_9(value);
			}
		}

		[DefaultValue("")]
		[CssProperty("font")]
		[CssPropertyInherited]
		public string Font
		{
			get
			{
				return string_36;
			}
			set
			{
				string_36 = value;
				string text = Parser.Search("(([0-9]+|[0-9]*\\.[0-9]+)(em|ex|px|in|cm|mm|pt|pc)|([0-9]+|[0-9]*\\.[0-9]+)\\%|xx-small|x-small|small|medium|large|x-large|xx-large|larger|smaller)(\\/(normal|{[0-9]+|[0-9]*\\.[0-9]+}|([0-9]+|[0-9]*\\.[0-9]+)(em|ex|px|in|cm|mm|pt|pc)|([0-9]+|[0-9]*\\.[0-9]+)\\%))?(\\s|$)", value, out var position);
				if (!string.IsNullOrEmpty(text))
				{
					text = ((string)(object)text).Trim();
					string source = ((string)(object)value).Substring(0, position);
					string text2 = Parser.Search("(normal|italic|oblique)", source);
					string text3 = Parser.Search("(normal|small-caps)", source);
					string text4 = Parser.Search("(normal|bold|bolder|lighter|100|200|300|400|500|600|700|800|900)", source);
					string text5 = ((string)(object)value).Substring(position + ((string)(object)text).Length);
					string text6 = ((string)(object)text5).Trim();
					string text7 = text;
					string text8 = string.Empty;
					if (((string)(object)text).Contains("/") && ((string)(object)text).Length > ((string)(object)text).IndexOf("/") + 1)
					{
						int num = ((string)(object)text).IndexOf("/");
						text7 = ((string)(object)text).Substring(0, num);
						text8 = ((string)(object)text).Substring(num + 1);
					}
					if (!string.IsNullOrEmpty(text2))
					{
						FontStyle = text2;
					}
					if (!string.IsNullOrEmpty(text3))
					{
						FontVariant = text3;
					}
					if (!string.IsNullOrEmpty(text4))
					{
						FontWeight = text4;
					}
					if (!string.IsNullOrEmpty(text6))
					{
						FontFamily = text6;
					}
					if (!string.IsNullOrEmpty(text7))
					{
						FontSize = text7;
					}
					if (!string.IsNullOrEmpty(text8))
					{
						LineHeight = text8;
					}
				}
			}
		}

		[CssPropertyInherited]
		[DefaultValue("serif")]
		[CssProperty("font-family")]
		public string FontFamily
		{
			get
			{
				return string_37;
			}
			set
			{
				switch (value)
				{
				case "serif":
					string_37 = CssDefaults.FontSerif;
					break;
				case "cursive":
					string_37 = CssDefaults.FontCursive;
					break;
				case "monospace":
					string_37 = CssDefaults.FontMonospace;
					break;
				case "fantasy":
					string_37 = CssDefaults.FontFantasy;
					break;
				case "sans-serif":
					string_37 = CssDefaults.FontSansSerif;
					break;
				default:
					string_37 = value;
					break;
				}
			}
		}

		[CssProperty("font-size")]
		[CssPropertyInherited]
		[DefaultValue("medium")]
		public string FontSize
		{
			get
			{
				return string_38;
			}
			set
			{
				string text = Parser.Search("([0-9]+|[0-9]*\\.[0-9]+)(em|ex|px|in|cm|mm|pt|pc)", value);
				if (text == null)
				{
					string_38 = value;
					return;
				}
				string empty = string.Empty;
				CssLength cssLength = new CssLength(text);
				empty = (string_38 = (cssLength.HasError ? dictionary_0["font-size"] : ((cssLength.Unit != CssLength.CssUnit.Ems || ParentBox == null) ? cssLength.ToString() : cssLength.ConvertEmToPoints(ParentBox.ActualFont.SizeInPoints).ToString())));
			}
		}

		[DefaultValue("normal")]
		[CssPropertyInherited]
		[CssProperty("font-style")]
		public string FontStyle
		{
			get
			{
				return string_39;
			}
			set
			{
				string_39 = value;
			}
		}

		[DefaultValue("normal")]
		[CssProperty("font-variant")]
		[CssPropertyInherited]
		public string FontVariant
		{
			get
			{
				return string_40;
			}
			set
			{
				string_40 = value;
			}
		}

		[DefaultValue("normal")]
		[CssPropertyInherited]
		[CssProperty("font-weight")]
		public string FontWeight
		{
			get
			{
				return string_41;
			}
			set
			{
				string_41 = value;
			}
		}

		[DefaultValue("")]
		[CssProperty("list-style")]
		[CssPropertyInherited]
		public string ListStyle
		{
			get
			{
				return string_53;
			}
			set
			{
				string_53 = value;
			}
		}

		[DefaultValue("outside")]
		[CssProperty("list-style-position")]
		[CssPropertyInherited]
		public string ListStylePosition
		{
			get
			{
				return string_52;
			}
			set
			{
				string_52 = value;
			}
		}

		[CssPropertyInherited]
		[DefaultValue("")]
		[CssProperty("list-style-image")]
		public string ListStyleImage
		{
			get
			{
				return string_51;
			}
			set
			{
				string_51 = value;
			}
		}

		[DefaultValue("disc")]
		[CssPropertyInherited]
		[CssProperty("list-style-type")]
		public string ListStyleType
		{
			get
			{
				return string_50;
			}
			set
			{
				string_50 = value;
			}
		}

		public float ActualPaddingTop
		{
			get
			{
				if (float.IsNaN(float_8))
				{
					float_8 = CssValue.ParseLength(PaddingTop, Size.Width, this);
				}
				return float_8;
			}
		}

		public float ActualPaddingLeft
		{
			get
			{
				if (float.IsNaN(float_11))
				{
					float_11 = CssValue.ParseLength(PaddingLeft, Size.Width, this);
				}
				return float_11;
			}
		}

		public float ActualPaddingBottom
		{
			get
			{
				if (float.IsNaN(float_9))
				{
					float_9 = CssValue.ParseLength(PaddingBottom, Size.Width, this);
				}
				return float_9;
			}
		}

		public float ActualPaddingRight
		{
			get
			{
				if (float.IsNaN(float_10))
				{
					float_10 = CssValue.ParseLength(PaddingRight, Size.Width, this);
				}
				return float_10;
			}
		}

		public float ActualMarginTop
		{
			get
			{
				if (float.IsNaN(float_12))
				{
					if (MarginTop == "auto")
					{
						MarginTop = "0";
					}
					float_12 = CssValue.ParseLength(MarginTop, Size.Width, this);
				}
				return float_12;
			}
		}

		public float ActualMarginLeft
		{
			get
			{
				if (float.IsNaN(float_15))
				{
					if (MarginLeft == "auto")
					{
						MarginLeft = "0";
					}
					float_15 = CssValue.ParseLength(MarginLeft, Size.Width, this);
				}
				return float_15;
			}
		}

		public float ActualMarginBottom
		{
			get
			{
				if (float.IsNaN(float_13))
				{
					if (MarginBottom == "auto")
					{
						MarginBottom = "0";
					}
					float_13 = CssValue.ParseLength(MarginBottom, Size.Width, this);
				}
				return float_13;
			}
		}

		public float ActualMarginRight
		{
			get
			{
				if (float.IsNaN(float_14))
				{
					if (MarginRight == "auto")
					{
						MarginRight = "0";
					}
					float_14 = CssValue.ParseLength(MarginRight, Size.Width, this);
				}
				return float_14;
			}
		}

		public float ActualBorderTopWidth
		{
			get
			{
				if (float.IsNaN(float_16))
				{
					float_16 = CssValue.GetActualBorderWidth(BorderTopWidth, this);
					if (string.IsNullOrEmpty(BorderTopStyle) || BorderTopStyle == "none")
					{
						float_16 = 0f;
					}
				}
				return float_16;
			}
		}

		public float ActualBorderLeftWidth
		{
			get
			{
				if (float.IsNaN(float_17))
				{
					float_17 = CssValue.GetActualBorderWidth(BorderLeftWidth, this);
					if (string.IsNullOrEmpty(BorderLeftStyle) || BorderLeftStyle == "none")
					{
						float_17 = 0f;
					}
				}
				return float_17;
			}
		}

		public float ActualBorderBottomWidth
		{
			get
			{
				if (float.IsNaN(float_18))
				{
					float_18 = CssValue.GetActualBorderWidth(BorderBottomWidth, this);
					if (string.IsNullOrEmpty(BorderBottomStyle) || BorderBottomStyle == "none")
					{
						float_18 = 0f;
					}
				}
				return float_18;
			}
		}

		public float ActualBorderRightWidth
		{
			get
			{
				if (float.IsNaN(float_19))
				{
					float_19 = CssValue.GetActualBorderWidth(BorderRightWidth, this);
					if (string.IsNullOrEmpty(BorderRightStyle) || BorderRightStyle == "none")
					{
						float_19 = 0f;
					}
				}
				return float_19;
			}
		}

		public Color ActualBorderTopColor
		{
			get
			{
				if (color_1.IsEmpty)
				{
					color_1 = CssValue.GetActualColor(BorderTopColor);
				}
				return color_1;
			}
		}

		public Color ActualBorderLeftColor
		{
			get
			{
				if (color_2.IsEmpty)
				{
					color_2 = CssValue.GetActualColor(BorderLeftColor);
				}
				return color_2;
			}
		}

		public Color ActualBorderBottomColor
		{
			get
			{
				if (color_3.IsEmpty)
				{
					color_3 = CssValue.GetActualColor(BorderBottomColor);
				}
				return color_3;
			}
		}

		public Color ActualBorderRightColor
		{
			get
			{
				if (color_4.IsEmpty)
				{
					color_4 = CssValue.GetActualColor(BorderRightColor);
				}
				return color_4;
			}
		}

		public float ActualCornerNW
		{
			get
			{
				if (float.IsNaN(float_3))
				{
					float_3 = CssValue.ParseLength(CornerNWRadius, 0f, this);
				}
				return float_3;
			}
		}

		public float ActualCornerNE
		{
			get
			{
				if (float.IsNaN(float_4))
				{
					float_4 = CssValue.ParseLength(CornerNERadius, 0f, this);
				}
				return float_4;
			}
		}

		public float ActualCornerSE
		{
			get
			{
				if (float.IsNaN(float_6))
				{
					float_6 = CssValue.ParseLength(CornerSERadius, 0f, this);
				}
				return float_6;
			}
		}

		public float ActualCornerSW
		{
			get
			{
				if (float.IsNaN(float_5))
				{
					float_5 = CssValue.ParseLength(CornerSWRadius, 0f, this);
				}
				return float_5;
			}
		}

		public float ActualWordSpacing
		{
			get
			{
				if (float.IsNaN(float_20))
				{
					throw new Exception("Space must be calculated before using this property");
				}
				return float_20;
			}
		}

		public Color ActualColor
		{
			get
			{
				if (kcFwwjpJu6.IsEmpty)
				{
					kcFwwjpJu6 = CssValue.GetActualColor(Color);
				}
				return kcFwwjpJu6;
			}
		}

		public Color ActualBackgroundColor
		{
			get
			{
				if (color_5.IsEmpty)
				{
					color_5 = CssValue.GetActualColor(BackgroundColor);
				}
				return color_5;
			}
		}

		public Color ActualBackgroundGradient
		{
			get
			{
				if (color_0.IsEmpty)
				{
					color_0 = CssValue.GetActualColor(BackgroundGradient);
				}
				return color_0;
			}
		}

		public float ActualBackgroundGradientAngle
		{
			get
			{
				if (float.IsNaN(float_7))
				{
					float_7 = CssValue.ParseNumber(BackgroundGradientAngle, 360f);
				}
				return float_7;
			}
		}

		public Font ActualParentFont
		{
			get
			{
				if (ParentBox != null)
				{
					return ParentBox.ActualFont;
				}
				return ActualFont;
			}
		}

		public Font ActualFont
		{
			get
			{
				if (font_0 == null)
				{
					if (string.IsNullOrEmpty(FontFamily))
					{
						FontFamily = CssDefaults.FontSerif;
					}
					if (string.IsNullOrEmpty(FontSize))
					{
						FontSize = (float)CssDefaults.FontSize + "pt";
					}
					FontStyle fontStyle = System.Drawing.FontStyle.Regular;
					if (FontStyle == "italic" || FontStyle == "oblique")
					{
						fontStyle |= System.Drawing.FontStyle.Italic;
					}
					if (FontWeight != "normal" && FontWeight != "lighter" && !string.IsNullOrEmpty(FontWeight))
					{
						fontStyle |= System.Drawing.FontStyle.Bold;
					}
					float num = 0f;
					float num2 = CssDefaults.FontSize;
					if (ParentBox != null)
					{
						num2 = ParentBox.ActualFont.Size;
					}
					num = FontSize switch
					{
						"medium" => CssDefaults.FontSize, 
						"xx-small" => CssDefaults.FontSize - 4f, 
						"x-small" => CssDefaults.FontSize - 3f, 
						"small" => CssDefaults.FontSize - 2f, 
						"large" => CssDefaults.FontSize + 2f, 
						"x-large" => CssDefaults.FontSize + 3f, 
						"xx-large" => CssDefaults.FontSize + 4f, 
						"smaller" => num2 - 2f, 
						"larger" => num2 + 2f, 
						_ => CssValue.ParseLength(FontSize, num2, this, num2, returnPoints: true), 
					};
					if (num <= 1f)
					{
						num = CssDefaults.FontSize;
					}
					font_0 = new Font(FontFamily, num, fontStyle);
				}
				return font_0;
			}
		}

		public float ActualTextIndent
		{
			get
			{
				if (float.IsNaN(float_21))
				{
					float_21 = CssValue.ParseLength(TextIndent, Size.Width, this);
				}
				return float_21;
			}
		}

		public float ActualBorderSpacingHorizontal
		{
			get
			{
				if (float.IsNaN(float_22))
				{
					MatchCollection matchCollection = Parser.Match("([0-9]+|[0-9]*\\.[0-9]+)(em|ex|px|in|cm|mm|pt|pc)", BorderSpacing);
					if (matchCollection.Count == 0)
					{
						float_22 = 0f;
					}
					else if (matchCollection.Count > 0)
					{
						float_22 = CssValue.ParseLength(matchCollection[0].Value, 1f, this);
					}
				}
				return float_22;
			}
		}

		public float ActualBorderSpacingVertical
		{
			get
			{
				if (float.IsNaN(float_23))
				{
					MatchCollection matchCollection = Parser.Match("([0-9]+|[0-9]*\\.[0-9]+)(em|ex|px|in|cm|mm|pt|pc)", BorderSpacing);
					if (matchCollection.Count == 0)
					{
						float_23 = 0f;
					}
					else if (matchCollection.Count == 1)
					{
						float_23 = CssValue.ParseLength(matchCollection[0].Value, 1f, this);
					}
					else
					{
						float_23 = CssValue.ParseLength(matchCollection[1].Value, 1f, this);
					}
				}
				return float_23;
			}
		}

		public CssBox ListItemBox => cssBox_1;

		public float AvailableWidth => Size.Width - ActualBorderLeftWidth - ActualPaddingLeft - ActualPaddingRight - ActualBorderRightWidth;

		public RectangleF Bounds => new RectangleF(Location, Size);

		public float ActualBottom
		{
			get
			{
				return Location.Y + Size.Height;
			}
			set
			{
				Size = new SizeF(Size.Width, value - Location.Y);
			}
		}

		public List<CssBox> Boxes => list_3;

		public float ClientLeft => Location.X + ActualBorderLeftWidth + ActualPaddingLeft;

		public float ClientTop => Location.Y + ActualBorderTopWidth + ActualPaddingTop;

		public float ClientRight => ActualRight - ActualPaddingRight - ActualBorderRightWidth;

		public float ClientBottom => ActualBottom - ActualPaddingBottom - ActualBorderBottomWidth;

		public RectangleF ClientRectangle => RectangleF.FromLTRB(ClientLeft, ClientTop, ClientRight, ClientBottom);

		public CssBox ContainingBlock
		{
			get
			{
				if (ParentBox == null)
				{
					return this;
				}
				CssBox parentBox = ParentBox;
				while (parentBox.Display != "block" && parentBox.Display != "table" && parentBox.Display != "table-cell" && parentBox.ParentBox != null)
				{
					parentBox = parentBox.ParentBox;
				}
				if (parentBox == null)
				{
					throw new Exception("There's no containing block on the chain");
				}
				return parentBox;
			}
		}

		public float FontAscent
		{
			get
			{
				if (float.IsNaN(float_0))
				{
					float_0 = CssLayoutEngine.GetAscent(ActualFont);
				}
				return float_0;
			}
		}

		public float FontLineSpacing
		{
			get
			{
				if (float.IsNaN(float_2))
				{
					float_2 = CssLayoutEngine.GetLineSpacing(ActualFont);
				}
				return float_2;
			}
		}

		public float FontDescent
		{
			get
			{
				if (float.IsNaN(float_1))
				{
					float_1 = CssLayoutEngine.GetDescent(ActualFont);
				}
				return float_1;
			}
		}

		internal CssBoxWord FirstWord => Words[0];

		internal CssLineBox FirstHostingLineBox
		{
			get
			{
				return cssLineBox_0;
			}
			set
			{
				cssLineBox_0 = value;
			}
		}

		internal CssLineBox LastHostingLineBox
		{
			get
			{
				return cssLineBox_1;
			}
			set
			{
				cssLineBox_1 = value;
			}
		}

		public HtmlTag HtmlTag => htmlTag_0;

		public InitialContainer InitialContainer => _initialContainer;

		public bool IsImage
		{
			get
			{
				if (Words.Count == 1)
				{
					return Words[0].IsImage;
				}
				return false;
			}
		}

		public bool IsRounded
		{
			get
			{
				if (!(ActualCornerNE > 0f) && !(ActualCornerNW > 0f) && !(ActualCornerSE > 0f))
				{
					return ActualCornerSW > 0f;
				}
				return true;
			}
		}

		public bool IsSpaceOrEmpty
		{
			get
			{
				if ((Words.Count == 0 && Boxes.Count == 0) || (Words.Count == 1 && Words[0].IsSpaces) || (Boxes.Count == 1 && Boxes[0] is CssAnonymousSpaceBlockBox))
				{
					return true;
				}
				foreach (CssBoxWord word in Words)
				{
					if (!word.IsSpaces)
					{
						return false;
					}
				}
				return true;
			}
		}

		internal CssBoxWord LastWord => Words[Words.Count - 1];

		internal List<CssLineBox> LineBoxes => list_4;

		public PointF Location
		{
			get
			{
				return pointF_0;
			}
			set
			{
				pointF_0 = value;
			}
		}

		public CssBox ParentBox
		{
			get
			{
				return cssBox_0;
			}
			set
			{
				if (cssBox_0 != null && cssBox_0.Boxes.Contains(this))
				{
					cssBox_0.Boxes.Remove(this);
				}
				cssBox_0 = value;
				if (value != null && !value.Boxes.Contains(this))
				{
					cssBox_0.Boxes.Add(this);
					_initialContainer = value.InitialContainer;
				}
			}
		}

		internal List<CssLineBox> ParentLineBoxes => list_5;

		internal Dictionary<CssLineBox, RectangleF> Rectangles => dictionary_1;

		public float ActualRight
		{
			get
			{
				return Location.X + Size.Width;
			}
			set
			{
				Size = new SizeF(value - Location.X, Size.Height);
			}
		}

		public SizeF Size
		{
			get
			{
				return sizeF_0;
			}
			set
			{
				sizeF_0 = value;
			}
		}

		public string Text
		{
			get
			{
				return string_59;
			}
			set
			{
				string_59 = value;
				method_13();
			}
		}

		internal List<CssBoxWord> Words => list_2;

		static CssBox()
		{
			_properties = new Dictionary<string, PropertyInfo>();
			dictionary_0 = new Dictionary<string, string>();
			list_0 = new List<PropertyInfo>();
			list_1 = new List<PropertyInfo>();
			PropertyInfo[] properties = Type.GetTypeFromHandle((RuntimeTypeHandle)typeof(CssBox).TypeHandle).GetProperties();
			for (int i = 0; i < properties.Length; i++)
			{
				CssPropertyAttribute cssPropertyAttribute = Attribute.GetCustomAttribute(properties[i], Type.GetTypeFromHandle((RuntimeTypeHandle)typeof(CssPropertyAttribute).TypeHandle)) as CssPropertyAttribute;
				if (cssPropertyAttribute != null)
				{
					_properties.Add(cssPropertyAttribute.Name, properties[i]);
					dictionary_0.Add(cssPropertyAttribute.Name, smethod_0(properties[i]));
					list_1.Add(properties[i]);
					CssPropertyInheritedAttribute cssPropertyInheritedAttribute = Attribute.GetCustomAttribute(properties[i], Type.GetTypeFromHandle((RuntimeTypeHandle)typeof(CssPropertyInheritedAttribute).TypeHandle)) as CssPropertyInheritedAttribute;
					if (cssPropertyInheritedAttribute != null)
					{
						list_0.Add(properties[i]);
					}
				}
			}
			Empty = new CssBox();
		}

		private static string smethod_0(MemberInfo memberInfo_0)
		{
			DefaultValueAttribute defaultValueAttribute = Attribute.GetCustomAttribute(memberInfo_0, Type.GetTypeFromHandle((RuntimeTypeHandle)typeof(DefaultValueAttribute).TypeHandle)) as DefaultValueAttribute;
			if (defaultValueAttribute == null)
			{
				return string.Empty;
			}
			string text = Convert.ToString(defaultValueAttribute.Value);
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
			return string.Empty;
		}

		protected CssBox()
		{
			list_2 = new List<CssBoxWord>();
			list_3 = new List<CssBox>();
			list_4 = new List<CssLineBox>();
			list_5 = new List<CssLineBox>();
			dictionary_1 = new Dictionary<CssLineBox, RectangleF>();
			foreach (string key in _properties.Keys)
			{
				_properties[key].SetValue(this, dictionary_0[key], null);
			}
		}

		public CssBox(CssBox parentBox)
			: this()
		{
			ParentBox = parentBox;
		}

		internal CssBox(CssBox parentBox, HtmlTag tag)
			: this(parentBox)
		{
			htmlTag_0 = tag;
		}

		private void method_0(InitialContainer initialContainer_0)
		{
			_initialContainer = initialContainer_0;
		}

		internal bool ContainsInlinesOnly()
		{
			foreach (CssBox box in Boxes)
			{
				if (box.Display != "inline")
				{
					return false;
				}
			}
			return true;
		}

		private int method_1()
		{
			int num = 0;
			foreach (CssBox box in ParentBox.Boxes)
			{
				if (box.Display == "list-item")
				{
					num++;
				}
				if (box.Equals(this))
				{
					return num;
				}
			}
			return num;
		}

		private void method_2(Graphics graphics_0)
		{
			if (!(Display == "list-item"))
			{
				return;
			}
			if (cssBox_1 == null)
			{
				cssBox_1 = new CssBox();
				cssBox_1.InheritStyle(this, everything: false);
				cssBox_1.Display = "inline";
				cssBox_1.method_0(InitialContainer);
				if (ParentBox != null && ListStyleType == "decimal")
				{
					cssBox_1.Text = (int)method_1() + ".";
				}
				else
				{
					cssBox_1.Text = "•";
				}
				cssBox_1.MeasureBounds(graphics_0);
				cssBox_1.Size = new SizeF(cssBox_1.Words[0].Width, cssBox_1.Words[0].Height);
			}
			cssBox_1.Words[0].Left = Location.X - cssBox_1.Size.Width - 5f;
			cssBox_1.Words[0].Top = Location.Y + ActualPaddingTop;
		}

		internal CssBoxWord FirstWordOccourence(CssBox b, CssLineBox line)
		{
			if (b.Words.Count != 0 || b.Boxes.Count != 0)
			{
				if (b.Words.Count > 0)
				{
					foreach (CssBoxWord word in b.Words)
					{
						if (line.Words.Contains(word))
						{
							return word;
						}
					}
					return null;
				}
				foreach (CssBox box in b.Boxes)
				{
					CssBoxWord cssBoxWord = FirstWordOccourence(box, line);
					if (cssBoxWord != null)
					{
						return cssBoxWord;
					}
				}
				return null;
			}
			return null;
		}

		internal string GetAttribute(string attribute)
		{
			return GetAttribute(attribute, string.Empty);
		}

		internal string GetAttribute(string attribute, string defaultValue)
		{
			if (HtmlTag != null)
			{
				if (!HtmlTag.HasAttribute(attribute))
				{
					return defaultValue;
				}
				return HtmlTag.Attributes[attribute];
			}
			return defaultValue;
		}

		public float GetEmHeight()
		{
			return ActualFont.GetHeight();
		}

		private CssBox method_3(CssBox cssBox_2)
		{
			if (cssBox_2.ParentBox != null)
			{
				int num = cssBox_2.ParentBox.Boxes.IndexOf(this);
				if (num >= 0)
				{
					if (num != 0)
					{
						int num2 = 1;
						CssBox cssBox = cssBox_2.ParentBox.Boxes[num - 1];
						while ((cssBox.Display == "none" || cssBox.Position == "absolute") && num - num2 - 1 >= 0)
						{
							cssBox = cssBox_2.ParentBox.Boxes[num - ++num2];
						}
						if (!(cssBox.Display == "none"))
						{
							return cssBox;
						}
						return null;
					}
					return null;
				}
				throw new Exception("Box doesn't exist on parent's Box list");
			}
			return null;
		}

		internal float GetMinimumWidth()
		{
			float float_ = 0f;
			float float_2 = 0f;
			CssBoxWord cssBoxWord_ = null;
			method_5(this, ref float_, ref cssBoxWord_);
			if (cssBoxWord_ != null)
			{
				method_4(cssBoxWord_.OwnerBox, this, ref float_2);
			}
			return float_ + float_2;
		}

		private void method_4(CssBox cssBox_2, CssBox cssBox_3, ref float float_24)
		{
			float num = cssBox_2.ActualBorderLeftWidth + cssBox_2.ActualPaddingLeft + cssBox_2.ActualBorderRightWidth + cssBox_2.ActualPaddingRight;
			float_24 += num;
			if (!cssBox_2.Equals(cssBox_3))
			{
				method_4(cssBox_2.ParentBox, cssBox_3, ref float_24);
			}
		}

		private void method_5(CssBox cssBox_2, ref float float_24, ref CssBoxWord cssBoxWord_0)
		{
			if (cssBox_2.Words.Count > 0)
			{
				foreach (CssBoxWord word in cssBox_2.Words)
				{
					if (word.FullWidth > float_24)
					{
						float_24 = word.FullWidth;
						cssBoxWord_0 = word;
					}
				}
				return;
			}
			foreach (CssBox box in cssBox_2.Boxes)
			{
				method_5(box, ref float_24, ref cssBoxWord_0);
			}
		}

		internal float GetMaximumBottom(CssBox startBox, float currentMaxBottom)
		{
			foreach (CssLineBox key in startBox.Rectangles.Keys)
			{
				currentMaxBottom = Math.Max(currentMaxBottom, startBox.Rectangles[key].Bottom);
			}
			foreach (CssBox box in startBox.Boxes)
			{
				currentMaxBottom = Math.Max(currentMaxBottom, box.ActualBottom);
				currentMaxBottom = Math.Max(currentMaxBottom, GetMaximumBottom(box, currentMaxBottom));
			}
			return currentMaxBottom;
		}

		internal float GetFullWidth(Graphics g)
		{
			float float_ = 0f;
			float float_2 = 0f;
			method_6(this, g, ref float_, ref float_2);
			return float_2 + float_;
		}

		private void method_6(CssBox cssBox_2, Graphics graphics_0, ref float float_24, ref float float_25)
		{
			if (cssBox_2.Display != "inline")
			{
				float_24 = 0f;
			}
			float_25 += cssBox_2.ActualBorderLeftWidth + cssBox_2.ActualBorderRightWidth + cssBox_2.ActualPaddingRight + cssBox_2.ActualPaddingLeft;
			if (cssBox_2.Words.Count > 0)
			{
				foreach (CssBoxWord word in cssBox_2.Words)
				{
					float_24 += word.FullWidth;
				}
				return;
			}
			foreach (CssBox box in cssBox_2.Boxes)
			{
				method_6(box, graphics_0, ref float_24, ref float_25);
			}
		}

		private CssBox method_7()
		{
			if (ParentBox != null)
			{
				int num = ParentBox.Boxes.IndexOf(this);
				if (num >= 0)
				{
					if (num == ParentBox.Boxes.Count - 1)
					{
						return null;
					}
					return ParentBox.Boxes[num + 1];
				}
				throw new Exception("Box doesn't exist on parent's Box list");
			}
			return null;
		}

		internal bool HasJustInlineSiblings()
		{
			if (ParentBox != null)
			{
				return ParentBox.ContainsInlinesOnly();
			}
			return false;
		}

		internal void InheritStyle()
		{
			InheritStyle(ParentBox, everything: false);
		}

		internal void InheritStyle(CssBox godfather, bool everything)
		{
			if (godfather == null)
			{
				return;
			}
			IEnumerable<PropertyInfo> enumerable = (everything ? list_1 : list_0);
			foreach (PropertyInfo item in enumerable)
			{
				item.SetValue(this, item.GetValue(godfather, null), null);
			}
		}

		private float method_8(CssBox cssBox_2, CssBox cssBox_3)
		{
			return Math.Max(cssBox_2?.ActualMarginBottom ?? 0f, cssBox_3?.ActualMarginTop ?? 0f);
		}

		public virtual void MeasureBounds(Graphics g)
		{
			if (Display == "none")
			{
				return;
			}
			RectanglesReset();
			MeasureWordsSize(g);
			if (Display == "block" || Display == "list-item" || Display == "table" || Display == "inline-table" || Display == "table-cell" || Display == "none")
			{
				if (Display != "table-cell")
				{
					CssBox cssBox = method_3(this);
					float x = ContainingBlock.Location.X + ContainingBlock.ActualPaddingLeft + ActualMarginLeft + ContainingBlock.ActualBorderLeftWidth;
					float num = ((cssBox == null && ParentBox != null) ? ParentBox.ClientTop : 0f) + method_8(cssBox, this) + ((cssBox != null) ? (cssBox.ActualBottom + cssBox.ActualBorderBottomWidth) : 0f);
					Location = new PointF(x, num);
					ActualBottom = num;
				}
				if (Display != "table-cell" && Display != "table")
				{
					float minimumWidth = GetMinimumWidth();
					float num2 = ContainingBlock.Size.Width - ContainingBlock.ActualPaddingLeft - ContainingBlock.ActualPaddingRight - ContainingBlock.ActualBorderLeftWidth - ContainingBlock.ActualBorderRightWidth - ActualMarginLeft - ActualMarginRight - ActualBorderLeftWidth - ActualBorderRightWidth;
					if (Width != "auto" && !string.IsNullOrEmpty(Width))
					{
						num2 = CssValue.ParseLength(Width, num2, this);
					}
					if (num2 < minimumWidth)
					{
						num2 = minimumWidth;
					}
					Size = new SizeF(num2, Size.Height);
				}
				if (!(Display == "table") && !(Display == "inline-table"))
				{
					if (Display != "none")
					{
						if (!ContainsInlinesOnly())
						{
							CssBox cssBox2 = null;
							foreach (CssBox box in Boxes)
							{
								if (!(box.Display == "none"))
								{
									box.MeasureBounds(g);
									cssBox2 = box;
								}
							}
							if (cssBox2 != null)
							{
								ActualBottom = Math.Max(ActualBottom, cssBox2.ActualBottom + cssBox2.ActualMarginBottom + ActualPaddingBottom);
							}
						}
						else
						{
							ActualBottom = Location.Y;
							CssLayoutEngine.CreateLineBoxes(g, this);
						}
					}
				}
				else
				{
					new CssTable(this, g);
				}
			}
			if (InitialContainer != null)
			{
				InitialContainer.MaximumSize = new SizeF(Math.Max(InitialContainer.MaximumSize.Width, ActualRight), Math.Max(InitialContainer.MaximumSize.Height, ActualBottom));
			}
		}

		private void YrsSgyiCmD(Graphics graphics_0)
		{
			float_20 = CssLayoutEngine.WhiteSpace(graphics_0, this);
			if (WordSpacing != "normal")
			{
				string length = Parser.Search("([0-9]+|[0-9]*\\.[0-9]+)(em|ex|px|in|cm|mm|pt|pc)", WordSpacing);
				float_20 += CssValue.ParseLength(length, 1f, this);
			}
		}

		internal void MeasureWordsSize(Graphics g)
		{
			if (bool_0)
			{
				return;
			}
			if (float.IsNaN(float_20))
			{
				YrsSgyiCmD(g);
			}
			if (HtmlTag != null && ((string)(object)HtmlTag.TagName).Equals("img", StringComparison.CurrentCultureIgnoreCase))
			{
				CssBoxWord item = new CssBoxWord(this, CssValue.GetImage(GetAttribute("src")));
				Words.Clear();
				Words.Add(item);
			}
			else
			{
				bool flag = false;
				foreach (CssBoxWord word in Words)
				{
					bool flag2 = CssBoxWordSplitter.CollapsesWhiteSpaces(this);
					if (CssBoxWordSplitter.EliminatesLineBreaks(this))
					{
						word.ReplaceLineBreaksAndTabs();
					}
					if (word.IsSpaces)
					{
						word.Height = FontLineSpacing;
						if (!word.IsTab)
						{
							if (word.IsLineBreak)
							{
								word.Width = 0f;
							}
							else if (!flag || !flag2)
							{
								word.Width = ActualWordSpacing * (float)(flag2 ? 1 : ((string)(object)word.Text).Length);
							}
						}
						else
						{
							word.Width = ActualWordSpacing * 4f;
						}
						flag = true;
					}
					else
					{
						string text = word.Text;
						CharacterRange[] measurableCharacterRanges = new CharacterRange[1]
						{
							new CharacterRange(0, ((string)(object)text).Length)
						};
						StringFormat stringFormat = new StringFormat();
						stringFormat.SetMeasurableCharacterRanges(measurableCharacterRanges);
						Region[] array = g.MeasureCharacterRanges(text, ActualFont, new RectangleF(0f, 0f, float.MaxValue, float.MaxValue), stringFormat);
						SizeF size = array[0].GetBounds(g).Size;
						PointF location = array[0].GetBounds(g).Location;
						word.LastMeasureOffset = new PointF(location.X, location.Y);
						word.Width = size.Width;
						word.Height = size.Height;
						flag = false;
					}
				}
			}
			bool_0 = true;
		}

		private string method_9(string string_69)
		{
			CssLength cssLength = new CssLength(string_69);
			if (cssLength.Unit == CssLength.CssUnit.Ems)
			{
				string_69 = cssLength.ConvertEmToPixels(GetEmHeight()).ToString();
			}
			return string_69;
		}

		internal void OffsetTop(float amount)
		{
			List<CssLineBox> list = new List<CssLineBox>();
			foreach (CssLineBox key in Rectangles.Keys)
			{
				list.Add(key);
			}
			foreach (CssLineBox item in list)
			{
				RectangleF rectangleF = Rectangles[item];
				Rectangles[item] = new RectangleF(rectangleF.X, rectangleF.Y + amount, rectangleF.Width, rectangleF.Height);
			}
			foreach (CssBoxWord word in Words)
			{
				word.Top += amount;
			}
			foreach (CssBox box in Boxes)
			{
				box.OffsetTop(amount);
			}
			Location = new PointF(Location.X, Location.Y + amount);
		}

		public void Paint(Graphics g)
		{
			if (Display == "none" || (Display == "table-cell" && EmptyCells == "hide" && IsSpaceOrEmpty))
			{
				return;
			}
			List<RectangleF> list = ((Rectangles.Count == 0) ? new List<RectangleF>(new RectangleF[1] { Bounds }) : new List<RectangleF>(Rectangles.Values));
			RectangleF[] array = list.ToArray();
			PointF pos = ((InitialContainer == null) ? PointF.Empty : InitialContainer.ScrollOffset);
			for (int i = 0; i < array.Length; i++)
			{
				RectangleF rectangleF = array[i];
				rectangleF.Offset(pos);
				if (InitialContainer != null && HtmlTag != null && ((string)(object)HtmlTag.TagName).Equals("a", StringComparison.CurrentCultureIgnoreCase))
				{
					if (InitialContainer.LinkRegions.ContainsKey(this))
					{
						InitialContainer.LinkRegions.Remove(this);
					}
					InitialContainer.LinkRegions.Add(this, rectangleF);
				}
				method_11(g, rectangleF);
				method_10(g, rectangleF, i == 0, i == array.Length - 1);
			}
			if (!IsImage)
			{
				Font actualFont = ActualFont;
				using SolidBrush brush = new SolidBrush(CssValue.GetActualColor(Color));
				foreach (CssBoxWord word in Words)
				{
					g.DrawString(word.Text, actualFont, brush, word.Left - word.LastMeasureOffset.X + pos.X, word.Top + pos.Y);
				}
			}
			else
			{
				RectangleF bounds = Words[0].Bounds;
				bounds.Offset(pos);
				bounds.Height -= ActualBorderTopWidth + ActualBorderBottomWidth + ActualPaddingTop + ActualPaddingBottom;
				bounds.Y += ActualBorderTopWidth + ActualPaddingTop;
				g.DrawImage(Words[0].Image, Rectangle.Round(bounds));
			}
			for (int j = 0; j < array.Length; j++)
			{
				RectangleF rectangleF_ = array[j];
				rectangleF_.Offset(pos);
				method_12(g, rectangleF_, j == 0, j == array.Length - 1);
			}
			foreach (CssBox box in Boxes)
			{
				box.Paint(g);
			}
			method_2(g);
			if (ListItemBox != null)
			{
				ListItemBox.Paint(g);
			}
		}

		private void method_10(Graphics graphics_0, RectangleF rectangleF_0, bool bool_1, bool bool_2)
		{
			SmoothingMode smoothingMode = graphics_0.SmoothingMode;
			if (InitialContainer != null && !InitialContainer.AvoidGeometryAntialias && IsRounded)
			{
				graphics_0.SmoothingMode = SmoothingMode.AntiAlias;
			}
			if (!string.IsNullOrEmpty(BorderTopStyle) && !(BorderTopStyle == "none"))
			{
				using SolidBrush solidBrush = new SolidBrush(ActualBorderTopColor);
				if (BorderTopStyle == "inset")
				{
					solidBrush.Color = CssDrawingHelper.Darken(ActualBorderTopColor);
				}
				graphics_0.FillPath(solidBrush, CssDrawingHelper.GetBorderPath(CssDrawingHelper.Border.Top, this, rectangleF_0, bool_1, bool_2));
			}
			if (bool_2 && !string.IsNullOrEmpty(BorderRightStyle) && !(BorderRightStyle == "none"))
			{
				using SolidBrush solidBrush2 = new SolidBrush(ActualBorderRightColor);
				if (BorderRightStyle == "outset")
				{
					solidBrush2.Color = CssDrawingHelper.Darken(ActualBorderRightColor);
				}
				graphics_0.FillPath(solidBrush2, CssDrawingHelper.GetBorderPath(CssDrawingHelper.Border.Right, this, rectangleF_0, bool_1, bool_2));
			}
			if (!string.IsNullOrEmpty(BorderBottomStyle) && !(BorderBottomStyle == "none"))
			{
				using SolidBrush solidBrush3 = new SolidBrush(ActualBorderBottomColor);
				if (BorderBottomStyle == "outset")
				{
					solidBrush3.Color = CssDrawingHelper.Darken(ActualBorderBottomColor);
				}
				graphics_0.FillPath(solidBrush3, CssDrawingHelper.GetBorderPath(CssDrawingHelper.Border.Bottom, this, rectangleF_0, bool_1, bool_2));
			}
			if (bool_1 && !string.IsNullOrEmpty(BorderLeftStyle) && !(BorderLeftStyle == "none"))
			{
				using SolidBrush solidBrush4 = new SolidBrush(ActualBorderLeftColor);
				if (BorderLeftStyle == "inset")
				{
					solidBrush4.Color = CssDrawingHelper.Darken(ActualBorderLeftColor);
				}
				graphics_0.FillPath(solidBrush4, CssDrawingHelper.GetBorderPath(CssDrawingHelper.Border.Left, this, rectangleF_0, bool_1, bool_2));
			}
			graphics_0.SmoothingMode = smoothingMode;
		}

		private void method_11(Graphics graphics_0, RectangleF rectangleF_0)
		{
			if (!(ContainingBlock.TextAlign == "justify"))
			{
				GraphicsPath graphicsPath = null;
				Brush brush = null;
				SmoothingMode smoothingMode = graphics_0.SmoothingMode;
				if (IsRounded)
				{
					graphicsPath = CssDrawingHelper.GetRoundRect(rectangleF_0, ActualCornerNW, ActualCornerNE, ActualCornerSE, ActualCornerSW);
				}
				brush = ((!(BackgroundGradient != "none") || !(rectangleF_0.Width > 0f) || !(rectangleF_0.Height > 0f)) ? ((Brush)new SolidBrush(ActualBackgroundColor)) : ((Brush)new LinearGradientBrush(rectangleF_0, ActualBackgroundColor, ActualBackgroundGradient, ActualBackgroundGradientAngle)));
				if (InitialContainer != null && !InitialContainer.AvoidGeometryAntialias && IsRounded)
				{
					graphics_0.SmoothingMode = SmoothingMode.AntiAlias;
				}
				if (graphicsPath == null)
				{
					graphics_0.FillRectangle(brush, rectangleF_0);
				}
				else
				{
					graphics_0.FillPath(brush, graphicsPath);
				}
				graphics_0.SmoothingMode = smoothingMode;
				graphicsPath?.Dispose();
				brush?.Dispose();
			}
		}

		private void method_12(Graphics graphics_0, RectangleF rectangleF_0, bool bool_1, bool bool_2)
		{
			if (!string.IsNullOrEmpty(TextDecoration) && !(TextDecoration == "none") && !IsImage)
			{
				float descent = CssLayoutEngine.GetDescent(ActualFont);
				float ascent = CssLayoutEngine.GetAscent(ActualFont);
				float num = 0f;
				if (TextDecoration == "underline")
				{
					num = rectangleF_0.Bottom - descent;
				}
				else if (TextDecoration == "line-through")
				{
					num = rectangleF_0.Bottom - descent - ascent / 2f;
				}
				else if (TextDecoration == "overline")
				{
					num = rectangleF_0.Bottom - descent - ascent - 2f;
				}
				num -= ActualPaddingBottom - ActualBorderBottomWidth;
				float num2 = rectangleF_0.X;
				float num3 = rectangleF_0.Right;
				if (bool_1)
				{
					num2 += ActualPaddingLeft + ActualBorderLeftWidth;
				}
				if (bool_2)
				{
					num3 -= ActualPaddingRight + ActualBorderRightWidth;
				}
				graphics_0.DrawLine(new Pen(ActualColor), num2, num, num3, num);
			}
		}

		internal void OffsetRectangle(CssLineBox lineBox, float gap)
		{
			if (Rectangles.ContainsKey(lineBox))
			{
				RectangleF rectangleF = Rectangles[lineBox];
				Rectangles[lineBox] = new RectangleF(rectangleF.X, rectangleF.Y + gap, rectangleF.Width, rectangleF.Height);
			}
		}

		internal void RectanglesReset()
		{
			dictionary_1.Clear();
		}

		internal void RemoveAnonymousSpaces()
		{
			for (int i = 0; i < Boxes.Count; i++)
			{
				if (Boxes[i] is CssAnonymousSpaceBlockBox || Boxes[i] is CssAnonymousSpaceBox)
				{
					Boxes.RemoveAt(i);
					i--;
				}
			}
		}

		public void SetBounds(Rectangle r)
		{
			SetBounds(new RectangleF(r.X, r.Y, r.Width, r.Height));
		}

		public void SetBounds(RectangleF rectangle)
		{
			Size = rectangle.Size;
			Location = rectangle.Location;
		}

		public override string ToString()
		{
			string arg = GetType().Name;
			if (HtmlTag != null)
			{
				arg = $"<{HtmlTag.TagName}>";
			}
			if (ParentBox != null)
			{
				if (!(Display == "block"))
				{
					if (Display == "none")
					{
						return $"{arg} None";
					}
					return string.Format("{0} {2}: {1}", arg, Text, Display);
				}
				return string.Format("{0} BlockBox {2}, Children:{1}", arg, (int)Boxes.Count, FontSize);
			}
			return "Initial Container";
		}

		private void method_13()
		{
			Words.Clear();
			CssBoxWordSplitter cssBoxWordSplitter = new CssBoxWordSplitter(this, Text);
			cssBoxWordSplitter.SplitWords();
			Words.AddRange(cssBoxWordSplitter.Words);
		}

		internal static void CsrOZwYAuuEC3d3KRcy()
		{
		}

		internal static bool UwJKKmYyyYMgcy0lwNf()
		{
			return SdLVLbY2ypcCv7kILCK == null;
		}
	}
}

using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace BunifuAnimatorNS
{
	[DebuggerStepThrough]
	public class Animation
	{
		[CompilerGenerated]
		private PointF pointF_0;

		[CompilerGenerated]
		private float float_0;

		[CompilerGenerated]
		private float float_1;

		[CompilerGenerated]
		private PointF pointF_1;

		[CompilerGenerated]
		private float float_2;

		[CompilerGenerated]
		private float float_3;

		[CompilerGenerated]
		private PointF urkxtLguGA;

		[CompilerGenerated]
		private PointF pointF_2;

		[CompilerGenerated]
		private int int_0;

		[CompilerGenerated]
		private PointF pointF_3;

		[CompilerGenerated]
		private float float_4;

		[CompilerGenerated]
		private float float_5;

		[CompilerGenerated]
		private float float_6;

		[CompilerGenerated]
		private Padding padding_0;

		[CompilerGenerated]
		private bool bool_0;

		internal static Animation mbhFEFIqs7eqxjbhoT8O;

		[TypeConverter(typeof(PointFConverter))]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public PointF SlideCoeff
		{
			[CompilerGenerated]
			get
			{
				return pointF_0;
			}
			[CompilerGenerated]
			set
			{
				pointF_0 = value;
			}
		}

		public float RotateCoeff
		{
			[CompilerGenerated]
			get
			{
				return float_0;
			}
			[CompilerGenerated]
			set
			{
				float_0 = value;
			}
		}

		public float RotateLimit
		{
			[CompilerGenerated]
			get
			{
				return float_1;
			}
			[CompilerGenerated]
			set
			{
				float_1 = value;
			}
		}

		[TypeConverter(typeof(PointFConverter))]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public PointF ScaleCoeff
		{
			[CompilerGenerated]
			get
			{
				return pointF_1;
			}
			[CompilerGenerated]
			set
			{
				pointF_1 = value;
			}
		}

		public float TransparencyCoeff
		{
			[CompilerGenerated]
			get
			{
				return float_2;
			}
			[CompilerGenerated]
			set
			{
				float_2 = value;
			}
		}

		public float LeafCoeff
		{
			[CompilerGenerated]
			get
			{
				return float_3;
			}
			[CompilerGenerated]
			set
			{
				float_3 = value;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[TypeConverter(typeof(PointFConverter))]
		public PointF MosaicShift
		{
			[CompilerGenerated]
			get
			{
				return urkxtLguGA;
			}
			[CompilerGenerated]
			set
			{
				urkxtLguGA = value;
			}
		}

		[TypeConverter(typeof(PointFConverter))]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public PointF MosaicCoeff
		{
			[CompilerGenerated]
			get
			{
				return pointF_2;
			}
			[CompilerGenerated]
			set
			{
				pointF_2 = value;
			}
		}

		public int MosaicSize
		{
			[CompilerGenerated]
			get
			{
				return int_0;
			}
			[CompilerGenerated]
			set
			{
				int_0 = value;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[TypeConverter(typeof(PointFConverter))]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public PointF BlindCoeff
		{
			[CompilerGenerated]
			get
			{
				return pointF_3;
			}
			[CompilerGenerated]
			set
			{
				pointF_3 = value;
			}
		}

		public float TimeCoeff
		{
			[CompilerGenerated]
			get
			{
				return float_4;
			}
			[CompilerGenerated]
			set
			{
				float_4 = value;
			}
		}

		public float MinTime
		{
			[CompilerGenerated]
			get
			{
				return float_5;
			}
			[CompilerGenerated]
			set
			{
				float_5 = value;
			}
		}

		public float MaxTime
		{
			[CompilerGenerated]
			get
			{
				return float_6;
			}
			[CompilerGenerated]
			set
			{
				float_6 = value;
			}
		}

		public Padding Padding
		{
			[CompilerGenerated]
			get
			{
				return padding_0;
			}
			[CompilerGenerated]
			set
			{
				padding_0 = value;
			}
		}

		public bool AnimateOnlyDifferences
		{
			[CompilerGenerated]
			get
			{
				return bool_0;
			}
			[CompilerGenerated]
			set
			{
				bool_0 = value;
			}
		}

		public bool IsNonLinearTransformNeeded
		{
			get
			{
				if (BlindCoeff == PointF.Empty && (MosaicCoeff == PointF.Empty || MosaicSize == 0) && TransparencyCoeff == 0f && LeafCoeff == 0f)
				{
					return false;
				}
				return true;
			}
		}

		public static Animation Rotate => new Animation
		{
			RotateCoeff = 1f,
			TransparencyCoeff = 1f,
			Padding = new Padding(50, 50, 50, 50)
		};

		public static Animation HorizSlide => new Animation
		{
			SlideCoeff = new PointF(1f, 0f)
		};

		public static Animation VertSlide => new Animation
		{
			SlideCoeff = new PointF(0f, 1f)
		};

		public static Animation Scale => new Animation
		{
			ScaleCoeff = new PointF(1f, 1f)
		};

		public static Animation ScaleAndRotate => new Animation
		{
			ScaleCoeff = new PointF(1f, 1f),
			RotateCoeff = 0.5f,
			RotateLimit = 0.2f,
			Padding = new Padding(30, 30, 30, 30)
		};

		public static Animation HorizSlideAndRotate => new Animation
		{
			SlideCoeff = new PointF(1f, 0f),
			RotateCoeff = 0.3f,
			RotateLimit = 0.2f,
			Padding = new Padding(50, 50, 50, 50)
		};

		public static Animation ScaleAndHorizSlide => new Animation
		{
			ScaleCoeff = new PointF(1f, 1f),
			SlideCoeff = new PointF(1f, 0f),
			Padding = new Padding(30, 0, 0, 0)
		};

		public static Animation Transparent => new Animation
		{
			TransparencyCoeff = 1f
		};

		public static Animation Leaf => new Animation
		{
			LeafCoeff = 1f
		};

		public static Animation Mosaic => new Animation
		{
			MosaicCoeff = new PointF(100f, 100f),
			MosaicSize = 20,
			Padding = new Padding(30, 30, 30, 30)
		};

		public static Animation Particles => new Animation
		{
			MosaicCoeff = new PointF(200f, 200f),
			MosaicSize = 1,
			MosaicShift = new PointF(0f, 0.5f),
			Padding = new Padding(100, 50, 100, 150),
			TimeCoeff = 2f
		};

		public static Animation VertBlind => new Animation
		{
			BlindCoeff = new PointF(0f, 1f)
		};

		public static Animation HorizBlind => new Animation
		{
			BlindCoeff = new PointF(1f, 0f)
		};

		public Animation()
		{
			MinTime = 0f;
			MaxTime = 1f;
			AnimateOnlyDifferences = true;
		}

		public Animation Clone()
		{
			return (Animation)MemberwiseClone();
		}

		public void Add(Animation a)
		{
			SlideCoeff = new PointF(SlideCoeff.X + a.SlideCoeff.X, SlideCoeff.Y + a.SlideCoeff.Y);
			RotateCoeff += a.RotateCoeff;
			RotateLimit += a.RotateLimit;
			ScaleCoeff = new PointF(ScaleCoeff.X + a.ScaleCoeff.X, ScaleCoeff.Y + a.ScaleCoeff.Y);
			TransparencyCoeff += a.TransparencyCoeff;
			LeafCoeff += a.LeafCoeff;
			MosaicShift = new PointF(MosaicShift.X + a.MosaicShift.X, MosaicShift.Y + a.MosaicShift.Y);
			MosaicCoeff = new PointF(MosaicCoeff.X + a.MosaicCoeff.X, MosaicCoeff.Y + a.MosaicCoeff.Y);
			MosaicSize += a.MosaicSize;
			BlindCoeff = new PointF(BlindCoeff.X + a.BlindCoeff.X, BlindCoeff.Y + a.BlindCoeff.Y);
			TimeCoeff += a.TimeCoeff;
			Padding += a.Padding;
		}

		internal static void yq10yYIqaCap2Yyy9hEL()
		{
		}

		internal static bool KbfWQbIqNEe5NOEpvdSa()
		{
			return mbhFEFIqs7eqxjbhoT8O == null;
		}
	}
}

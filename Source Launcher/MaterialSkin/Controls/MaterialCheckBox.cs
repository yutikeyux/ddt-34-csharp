using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using MaterialSkin.Animations;

namespace MaterialSkin.Controls
{
	public class MaterialCheckBox : CheckBox, IMaterialControl
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private int int_0;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private MouseState mouseState_0;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private Point point_0;

		private bool bool_0;

		private readonly AnimationManager animationManager_0;

		private readonly AnimationManager animationManager_1;

		private int int_1;

		private Rectangle rectangle_0;

		private static readonly Point[] point_1;

		private static MaterialCheckBox bdGaglIoXA24St8vDCl4;

		[Browsable(false)]
		public int Depth
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

		[Browsable(false)]
		public MaterialSkinManager SkinManager => MaterialSkinManager.Instance;

		[Browsable(false)]
		public MouseState MouseState
		{
			[CompilerGenerated]
			get
			{
				return mouseState_0;
			}
			[CompilerGenerated]
			set
			{
				mouseState_0 = value;
			}
		}

		[Browsable(false)]
		public Point MouseLocation
		{
			[CompilerGenerated]
			get
			{
				return point_0;
			}
			[CompilerGenerated]
			set
			{
				point_0 = value;
			}
		}

		[Category("Behavior")]
		public bool Ripple
		{
			get
			{
				return bool_0;
			}
			set
			{
				bool_0 = value;
				AutoSize = AutoSize;
				if (value)
				{
					base.Margin = new Padding(0);
				}
				Invalidate();
			}
		}

		public override bool AutoSize
		{
			get
			{
				return base.AutoSize;
			}
			set
			{
				base.AutoSize = value;
				if (value)
				{
					base.Size = new Size(10, 10);
				}
			}
		}

		public MaterialCheckBox()
		{
			animationManager_0 = new AnimationManager
			{
				AnimationType = AnimationType.EaseInOut,
				Increment = 0.05
			};
			animationManager_1 = new AnimationManager(singular: false)
			{
				AnimationType = AnimationType.Linear,
				Increment = 0.1,
				SecondaryIncrement = 0.08
			};
			animationManager_0.OnAnimationProgress += delegate
			{
				Invalidate();
			};
			animationManager_1.OnAnimationProgress += delegate
			{
				Invalidate();
			};
			base.CheckedChanged += delegate
			{
				animationManager_0.StartNewAnimation((!base.Checked) ? AnimationDirection.Out : AnimationDirection.In);
			};
			Ripple = true;
			MouseLocation = new Point(-1, -1);
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			int_1 = base.Height / 2 - 9;
			rectangle_0 = new Rectangle(int_1, int_1, 17, 17);
		}

		public override Size GetPreferredSize(Size proposedSize)
		{
			int num = int_1 + 18 + 2 + (int)CreateGraphics().MeasureString(Text, SkinManager.ROBOTO_MEDIUM_10).Width;
			return Ripple ? new Size(num, 30) : new Size(num, 20);
		}

		protected override void OnPaint(PaintEventArgs pevent)
		{
			Graphics graphics = pevent.Graphics;
			graphics.SmoothingMode = SmoothingMode.AntiAlias;
			graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
			graphics.Clear(base.Parent.BackColor);
			int num = int_1 + 9 - 1;
			double progress = animationManager_0.GetProgress();
			int alpha = (base.Enabled ? ((int)(progress * 255.0)) : SkinManager.GetCheckBoxOffDisabledColor().A);
			int num2 = ((!base.Enabled) ? SkinManager.GetCheckBoxOffDisabledColor().A : ((int)((double)(int)SkinManager.GetCheckboxOffColor().A * (1.0 - progress))));
			SolidBrush solidBrush = new SolidBrush(Color.FromArgb(alpha, (!base.Enabled) ? SkinManager.GetCheckBoxOffDisabledColor() : SkinManager.ColorScheme.AccentColor));
			SolidBrush solidBrush2 = new SolidBrush(base.Enabled ? SkinManager.ColorScheme.AccentColor : SkinManager.GetCheckBoxOffDisabledColor());
			Pen pen = new Pen(solidBrush.Color);
			if (Ripple && animationManager_1.IsAnimating())
			{
				for (int i = 0; i < animationManager_1.GetAnimationCount(); i++)
				{
					double progress2 = animationManager_1.GetProgress(i);
					Point point = new Point(num, num);
					SolidBrush solidBrush3 = new SolidBrush(Color.FromArgb((int)(progress2 * 40.0), (!(bool)animationManager_1.GetData(i)[0]) ? solidBrush.Color : Color.Black));
					int num3 = ((base.Height % 2 != 0) ? (base.Height - 2) : (base.Height - 3));
					int num4 = ((animationManager_1.GetDirection(i) == AnimationDirection.InOutIn) ? ((int)((double)num3 * (0.8 + 0.2 * progress2))) : num3);
					using (GraphicsPath path = DrawHelper.CreateRoundRect(point.X - num4 / 2, point.Y - num4 / 2, num4, num4, num4 / 2))
					{
						graphics.FillPath(solidBrush3, path);
					}
					solidBrush3.Dispose();
				}
			}
			solidBrush2.Dispose();
			Rectangle rect = new Rectangle(int_1, int_1, (int)(17.0 * progress), 17);
			using (GraphicsPath path2 = DrawHelper.CreateRoundRect(int_1, int_1, 17f, 17f, 1f))
			{
				SolidBrush solidBrush4 = new SolidBrush(DrawHelper.BlendColor(base.Parent.BackColor, (!base.Enabled) ? SkinManager.GetCheckBoxOffDisabledColor() : SkinManager.GetCheckboxOffColor(), num2));
				Pen pen2 = new Pen(solidBrush4.Color);
				graphics.FillPath(solidBrush4, path2);
				graphics.DrawPath(pen2, path2);
				graphics.FillRectangle(new SolidBrush(base.Parent.BackColor), int_1 + 2, int_1 + 2, 13, 13);
				graphics.DrawRectangle(new Pen(base.Parent.BackColor), int_1 + 2, int_1 + 2, 13, 13);
				solidBrush4.Dispose();
				pen2.Dispose();
				if (base.Enabled)
				{
					graphics.FillPath(solidBrush, path2);
					graphics.DrawPath(pen, path2);
				}
				else if (base.Checked)
				{
					graphics.SmoothingMode = SmoothingMode.None;
					graphics.FillRectangle(solidBrush, int_1 + 2, int_1 + 2, 14, 14);
					graphics.SmoothingMode = SmoothingMode.AntiAlias;
				}
				graphics.DrawImageUnscaledAndClipped(method_0(), rect);
			}
			SizeF sizeF = graphics.MeasureString(Text, SkinManager.ROBOTO_MEDIUM_10);
			graphics.DrawString(Text, SkinManager.ROBOTO_MEDIUM_10, base.Enabled ? SkinManager.GetPrimaryTextBrush() : SkinManager.GetDisabledOrHintBrush(), int_1 + 22, (float)(base.Height / 2) - sizeF.Height / 2f);
			pen.Dispose();
			solidBrush.Dispose();
		}

		private Bitmap method_0()
		{
			Bitmap result = new Bitmap(18, 18);
			Graphics graphics = Graphics.FromImage(result);
			graphics.Clear(Color.Transparent);
			using (Pen pen = new Pen(base.Parent.BackColor, 2f))
			{
				graphics.DrawLines(pen, point_1);
			}
			return result;
		}

		private bool method_1()
		{
			return rectangle_0.Contains(MouseLocation);
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			Font = SkinManager.ROBOTO_MEDIUM_10;
			if (base.DesignMode)
			{
				return;
			}
			MouseState = MouseState.OUT;
			base.MouseEnter += delegate
			{
				MouseState = MouseState.HOVER;
			};
			base.MouseLeave += delegate
			{
				MouseLocation = new Point(-1, -1);
				MouseState = MouseState.OUT;
			};
			base.MouseDown += delegate(object sender, MouseEventArgs e)
			{
				MouseState = MouseState.DOWN;
				if (Ripple && e.Button == MouseButtons.Left && method_1())
				{
					animationManager_1.SecondaryIncrement = 0.0;
					animationManager_1.StartNewAnimation(AnimationDirection.InOutIn, new object[1] { base.Checked });
				}
			};
			base.MouseUp += delegate
			{
				MouseState = MouseState.HOVER;
				animationManager_1.SecondaryIncrement = 0.08;
			};
			base.MouseMove += delegate(object sender, MouseEventArgs e)
			{
				MouseLocation = e.Location;
				Cursor = (method_1() ? Cursors.Hand : Cursors.Default);
			};
		}

		static MaterialCheckBox()
		{
			point_1 = new Point[3]
			{
				new Point(3, 8),
				new Point(7, 12),
				new Point(14, 5)
			};
		}

		[CompilerGenerated]
		private void method_2(object object_0)
		{
			Invalidate();
		}

		[CompilerGenerated]
		private void method_3(object object_0)
		{
			Invalidate();
		}

		[CompilerGenerated]
		private void MaterialCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			animationManager_0.StartNewAnimation((!base.Checked) ? AnimationDirection.Out : AnimationDirection.In);
		}

		[CompilerGenerated]
		private void MaterialCheckBox_MouseEnter(object sender, EventArgs e)
		{
			MouseState = MouseState.HOVER;
		}

		[CompilerGenerated]
		private void MaterialCheckBox_MouseLeave(object sender, EventArgs e)
		{
			MouseLocation = new Point(-1, -1);
			MouseState = MouseState.OUT;
		}

		[CompilerGenerated]
		private void MaterialCheckBox_MouseDown(object sender, MouseEventArgs e)
		{
			MouseState = MouseState.DOWN;
			if (Ripple && e.Button == MouseButtons.Left && method_1())
			{
				animationManager_1.SecondaryIncrement = 0.0;
				animationManager_1.StartNewAnimation(AnimationDirection.InOutIn, new object[1] { base.Checked });
			}
		}

		[CompilerGenerated]
		private void MaterialCheckBox_MouseUp(object sender, MouseEventArgs e)
		{
			MouseState = MouseState.HOVER;
			animationManager_1.SecondaryIncrement = 0.08;
		}

		[CompilerGenerated]
		private void MaterialCheckBox_MouseMove(object sender, MouseEventArgs e)
		{
			MouseLocation = e.Location;
			Cursor = (method_1() ? Cursors.Hand : Cursors.Default);
		}

		internal static bool XEJY00IoaaCJCGF1UACI()
		{
			return bdGaglIoXA24St8vDCl4 == null;
		}

		internal static void unJTevIoVwMDrj4RbPpu()
		{
		}
	}
}

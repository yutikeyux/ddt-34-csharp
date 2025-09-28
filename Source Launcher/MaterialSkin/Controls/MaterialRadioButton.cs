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
	public class MaterialRadioButton : RadioButton, IMaterialControl
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private int int_0;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private MouseState mouseState_0;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Point point_0;

		private bool bool_0;

		private readonly AnimationManager animationManager_0;

		private readonly AnimationManager animationManager_1;

		private Rectangle rectangle_0;

		private int int_1;

		internal static MaterialRadioButton cgy1S0IVR8ggOdsCufY4;

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

		public MaterialRadioButton()
		{
			SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, value: true);
			animationManager_0 = new AnimationManager
			{
				AnimationType = AnimationType.EaseInOut,
				Increment = 0.06
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
			base.SizeChanged += MaterialRadioButton_SizeChanged;
			Ripple = true;
			MouseLocation = new Point(-1, -1);
		}

		private void MaterialRadioButton_SizeChanged(object sender, EventArgs e)
		{
			int_1 = base.Height / 2 - (int)Math.Ceiling(9.5);
			rectangle_0 = new Rectangle(int_1, int_1, 19, 19);
		}

		public override Size GetPreferredSize(Size proposedSize)
		{
			int num = int_1 + 20 + (int)CreateGraphics().MeasureString(Text, SkinManager.ROBOTO_MEDIUM_10).Width;
			return Ripple ? new Size(num, 30) : new Size(num, 20);
		}

		protected override void OnPaint(PaintEventArgs pevent)
		{
			Graphics graphics = pevent.Graphics;
			graphics.SmoothingMode = SmoothingMode.AntiAlias;
			graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
			graphics.Clear(base.Parent.BackColor);
			int num = int_1 + 9;
			double progress = animationManager_0.GetProgress();
			int alpha = ((!base.Enabled) ? SkinManager.GetCheckBoxOffDisabledColor().A : ((int)(progress * 255.0)));
			int num2 = ((!base.Enabled) ? SkinManager.GetCheckBoxOffDisabledColor().A : ((int)((double)(int)SkinManager.GetCheckboxOffColor().A * (1.0 - progress))));
			float num3 = (float)(progress * 8.0);
			float num4 = num3 / 2f;
			num3 = (float)(progress * 9.0);
			SolidBrush solidBrush = new SolidBrush(Color.FromArgb(alpha, base.Enabled ? SkinManager.ColorScheme.AccentColor : SkinManager.GetCheckBoxOffDisabledColor()));
			Pen pen = new Pen(solidBrush.Color);
			if (Ripple && animationManager_1.IsAnimating())
			{
				for (int i = 0; i < animationManager_1.GetAnimationCount(); i++)
				{
					double progress2 = animationManager_1.GetProgress(i);
					Point point = new Point(num, num);
					SolidBrush solidBrush2 = new SolidBrush(Color.FromArgb((int)(progress2 * 40.0), (!(bool)animationManager_1.GetData(i)[0]) ? solidBrush.Color : Color.Black));
					int num5 = ((base.Height % 2 == 0) ? (base.Height - 3) : (base.Height - 2));
					int num6 = ((animationManager_1.GetDirection(i) == AnimationDirection.InOutIn) ? ((int)((double)num5 * (0.8 + 0.2 * progress2))) : num5);
					using (GraphicsPath path = DrawHelper.CreateRoundRect(point.X - num6 / 2, point.Y - num6 / 2, num6, num6, num6 / 2))
					{
						graphics.FillPath(solidBrush2, path);
					}
					solidBrush2.Dispose();
				}
			}
			Color color = DrawHelper.BlendColor(base.Parent.BackColor, base.Enabled ? SkinManager.GetCheckboxOffColor() : SkinManager.GetCheckBoxOffDisabledColor(), num2);
			using (GraphicsPath path2 = DrawHelper.CreateRoundRect(int_1, int_1, 19f, 19f, 9f))
			{
				graphics.FillPath(new SolidBrush(color), path2);
				if (base.Enabled)
				{
					graphics.FillPath(solidBrush, path2);
				}
			}
			graphics.FillEllipse(new SolidBrush(base.Parent.BackColor), 2 + int_1, 2 + int_1, 15, 15);
			if (base.Checked)
			{
				using GraphicsPath path3 = DrawHelper.CreateRoundRect((float)num - num4, (float)num - num4, num3, num3, 4f);
				graphics.FillPath(solidBrush, path3);
			}
			SizeF sizeF = graphics.MeasureString(Text, SkinManager.ROBOTO_MEDIUM_10);
			graphics.DrawString(Text, SkinManager.ROBOTO_MEDIUM_10, (!base.Enabled) ? SkinManager.GetDisabledOrHintBrush() : SkinManager.GetPrimaryTextBrush(), int_1 + 22, (float)(base.Height / 2) - sizeF.Height / 2f);
			solidBrush.Dispose();
			pen.Dispose();
		}

		private bool method_0()
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
				if (Ripple && e.Button == MouseButtons.Left && method_0())
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
				Cursor = ((!method_0()) ? Cursors.Default : Cursors.Hand);
			};
		}

		[CompilerGenerated]
		private void method_1(object object_0)
		{
			Invalidate();
		}

		[CompilerGenerated]
		private void method_2(object object_0)
		{
			Invalidate();
		}

		[CompilerGenerated]
		private void MaterialRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			animationManager_0.StartNewAnimation((!base.Checked) ? AnimationDirection.Out : AnimationDirection.In);
		}

		[CompilerGenerated]
		private void MaterialRadioButton_MouseEnter(object sender, EventArgs e)
		{
			MouseState = MouseState.HOVER;
		}

		[CompilerGenerated]
		private void MaterialRadioButton_MouseLeave(object sender, EventArgs e)
		{
			MouseLocation = new Point(-1, -1);
			MouseState = MouseState.OUT;
		}

		[CompilerGenerated]
		private void MaterialRadioButton_MouseDown(object sender, MouseEventArgs e)
		{
			MouseState = MouseState.DOWN;
			if (Ripple && e.Button == MouseButtons.Left && method_0())
			{
				animationManager_1.SecondaryIncrement = 0.0;
				animationManager_1.StartNewAnimation(AnimationDirection.InOutIn, new object[1] { base.Checked });
			}
		}

		[CompilerGenerated]
		private void MaterialRadioButton_MouseUp(object sender, MouseEventArgs e)
		{
			MouseState = MouseState.HOVER;
			animationManager_1.SecondaryIncrement = 0.08;
		}

		[CompilerGenerated]
		private void MaterialRadioButton_MouseMove(object sender, MouseEventArgs e)
		{
			MouseLocation = e.Location;
			Cursor = ((!method_0()) ? Cursors.Default : Cursors.Hand);
		}

		internal static bool nGWtSoIVAdb1lVJUPeD1()
		{
			return cgy1S0IVR8ggOdsCufY4 == null;
		}

		internal static void klZODkIVQM59V3LGPGvu()
		{
		}
	}
}

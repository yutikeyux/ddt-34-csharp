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
	public class MaterialFlatButton : Button, IMaterialControl
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private int int_0;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private MouseState mouseState_0;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private bool bool_0;

		private readonly AnimationManager animationManager_0;

		private readonly AnimationManager animationManager_1;

		private SizeF sizeF_0;

		private Image image_0;

		private static MaterialFlatButton jmWxhZIVO4QBYkNrv6M8;

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

		public bool Primary
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

		public Image Icon
		{
			get
			{
				return image_0;
			}
			set
			{
				image_0 = value;
				if (AutoSize)
				{
					base.Size = method_0();
				}
				Invalidate();
			}
		}

		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
				sizeF_0 = CreateGraphics().MeasureString(value.ToUpper(), SkinManager.ROBOTO_MEDIUM_10);
				if (AutoSize)
				{
					base.Size = method_0();
				}
				Invalidate();
			}
		}

		public MaterialFlatButton()
		{
			Primary = false;
			animationManager_0 = new AnimationManager(singular: false)
			{
				Increment = 0.03,
				AnimationType = AnimationType.EaseOut
			};
			animationManager_1 = new AnimationManager
			{
				Increment = 0.07,
				AnimationType = AnimationType.Linear
			};
			animationManager_1.OnAnimationProgress += delegate
			{
				Invalidate();
			};
			animationManager_0.OnAnimationProgress += delegate
			{
				Invalidate();
			};
			base.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			AutoSize = true;
			base.Margin = new Padding(4, 6, 4, 6);
			base.Padding = new Padding(0);
		}

		protected override void OnPaint(PaintEventArgs pevent)
		{
			Graphics graphics = pevent.Graphics;
			graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
			graphics.Clear(base.Parent.BackColor);
			Color flatButtonHoverBackgroundColor = SkinManager.GetFlatButtonHoverBackgroundColor();
			using (Brush brush = new SolidBrush(Color.FromArgb((int)(animationManager_1.GetProgress() * (double)(int)flatButtonHoverBackgroundColor.A), flatButtonHoverBackgroundColor.RemoveAlpha())))
			{
				graphics.FillRectangle(brush, base.ClientRectangle);
			}
			if (animationManager_0.IsAnimating())
			{
				graphics.SmoothingMode = SmoothingMode.AntiAlias;
				for (int i = 0; i < animationManager_0.GetAnimationCount(); i++)
				{
					double progress = animationManager_0.GetProgress(i);
					Point source = animationManager_0.GetSource(i);
					using Brush brush2 = new SolidBrush(Color.FromArgb((int)(101.0 - progress * 100.0), Color.Black));
					int num = (int)(progress * (double)base.Width * 2.0);
					graphics.FillEllipse(brush2, new Rectangle(source.X - num / 2, source.Y - num / 2, num, num));
				}
				graphics.SmoothingMode = SmoothingMode.None;
			}
			Rectangle rect = new Rectangle(8, 6, 24, 24);
			if (string.IsNullOrEmpty(Text))
			{
				rect.X += 2;
			}
			if (Icon != null)
			{
				graphics.DrawImage(Icon, rect);
			}
			Rectangle clientRectangle = base.ClientRectangle;
			if (Icon != null)
			{
				clientRectangle.Width -= 44;
				clientRectangle.X += 36;
			}
			graphics.DrawString(Text.ToUpper(), SkinManager.ROBOTO_MEDIUM_10, (!base.Enabled) ? SkinManager.GetFlatButtonDisabledTextBrush() : (Primary ? SkinManager.ColorScheme.PrimaryBrush : SkinManager.GetPrimaryTextBrush()), clientRectangle, new StringFormat
			{
				Alignment = StringAlignment.Center,
				LineAlignment = StringAlignment.Center
			});
		}

		private Size method_0()
		{
			return GetPreferredSize(new Size(0, 0));
		}

		public override Size GetPreferredSize(Size proposedSize)
		{
			int num = 16;
			if (Icon != null)
			{
				num += 28;
			}
			return new Size((int)Math.Ceiling(sizeF_0.Width) + num, 36);
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			if (base.DesignMode)
			{
				return;
			}
			MouseState = MouseState.OUT;
			base.MouseEnter += delegate
			{
				MouseState = MouseState.HOVER;
				animationManager_1.StartNewAnimation(AnimationDirection.In);
				Invalidate();
			};
			base.MouseLeave += delegate
			{
				MouseState = MouseState.OUT;
				animationManager_1.StartNewAnimation(AnimationDirection.Out);
				Invalidate();
			};
			base.MouseDown += delegate(object sender, MouseEventArgs e)
			{
				if (e.Button == MouseButtons.Left)
				{
					MouseState = MouseState.DOWN;
					animationManager_0.StartNewAnimation(AnimationDirection.In, e.Location);
					Invalidate();
				}
			};
			base.MouseUp += delegate
			{
				MouseState = MouseState.HOVER;
				Invalidate();
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
		private void MaterialFlatButton_MouseEnter(object sender, EventArgs e)
		{
			MouseState = MouseState.HOVER;
			animationManager_1.StartNewAnimation(AnimationDirection.In);
			Invalidate();
		}

		[CompilerGenerated]
		private void MaterialFlatButton_MouseLeave(object sender, EventArgs e)
		{
			MouseState = MouseState.OUT;
			animationManager_1.StartNewAnimation(AnimationDirection.Out);
			Invalidate();
		}

		[CompilerGenerated]
		private void MaterialFlatButton_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				MouseState = MouseState.DOWN;
				animationManager_0.StartNewAnimation(AnimationDirection.In, e.Location);
				Invalidate();
			}
		}

		[CompilerGenerated]
		private void MaterialFlatButton_MouseUp(object sender, MouseEventArgs e)
		{
			MouseState = MouseState.HOVER;
			Invalidate();
		}

		internal static bool kuENcCIVf0542vSiG7Z6()
		{
			return jmWxhZIVO4QBYkNrv6M8 == null;
		}

		internal static void wcciAjIVgAnTuAaS0Koe()
		{
		}
	}
}

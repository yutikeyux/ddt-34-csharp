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
	public class MaterialRaisedButton : Button, IMaterialControl
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private int int_0;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private MouseState mouseState_0;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool bool_0;

		private readonly AnimationManager animationManager_0;

		private SizeF sizeF_0;

		private Image image_0;

		internal static MaterialRaisedButton RxomINInyvcCjirV8Nii;

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
				Invalidate();
			}
		}

		public MaterialRaisedButton()
		{
			Primary = true;
			animationManager_0 = new AnimationManager(singular: false)
			{
				Increment = 0.03,
				AnimationType = AnimationType.EaseOut
			};
			animationManager_0.OnAnimationProgress += delegate
			{
				Invalidate();
			};
			base.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			AutoSize = false;
		}

		protected override void OnMouseUp(MouseEventArgs mevent)
		{
			base.OnMouseUp(mevent);
			animationManager_0.StartNewAnimation(AnimationDirection.In, mevent.Location);
		}

		protected override void OnPaint(PaintEventArgs pevent)
		{
			Graphics graphics = pevent.Graphics;
			graphics.SmoothingMode = SmoothingMode.AntiAlias;
			graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
			graphics.Clear(base.Parent.BackColor);
			using (GraphicsPath path = DrawHelper.CreateRoundRect(base.ClientRectangle.X, base.ClientRectangle.Y, base.ClientRectangle.Width - 1, base.ClientRectangle.Height - 1, 1f))
			{
				graphics.FillPath(Primary ? SkinManager.ColorScheme.PrimaryBrush : SkinManager.GetRaisedButtonBackgroundBrush(), path);
			}
			if (animationManager_0.IsAnimating())
			{
				for (int i = 0; i < animationManager_0.GetAnimationCount(); i++)
				{
					double progress = animationManager_0.GetProgress(i);
					Point source = animationManager_0.GetSource(i);
					SolidBrush brush = new SolidBrush(Color.FromArgb((int)(51.0 - progress * 50.0), Color.White));
					int num = (int)(progress * (double)base.Width * 2.0);
					graphics.FillEllipse(brush, new Rectangle(source.X - num / 2, source.Y - num / 2, num, num));
				}
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
			graphics.DrawString(Text.ToUpper(), SkinManager.ROBOTO_MEDIUM_10, SkinManager.GetRaisedButtonTextBrush(Primary), clientRectangle, new StringFormat
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

		[CompilerGenerated]
		private void method_1(object object_0)
		{
			Invalidate();
		}

		internal static bool Q7ef3IInRcWsRgXdOuZj()
		{
			return RxomINInyvcCjirV8Nii == null;
		}

		internal static void jVnvi0InjkMVNyOLggG5()
		{
		}
	}
}

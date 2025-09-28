using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using MaterialSkin.Animations;

namespace MaterialSkin.Controls
{
	public class MaterialTabSelector : Control, IMaterialControl
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private int int_0;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private MouseState mouseState_0;

		private MaterialTabControl materialTabControl_0;

		private int int_1;

		private Point point_0;

		private readonly AnimationManager animationManager_0;

		private List<Rectangle> list_0;

		internal static MaterialTabSelector sn89b1InD9UbKF0RFIvr;

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

		public MaterialTabControl BaseTabControl
		{
			get
			{
				return materialTabControl_0;
			}
			set
			{
				materialTabControl_0 = value;
				if (materialTabControl_0 != null)
				{
					int_1 = materialTabControl_0.SelectedIndex;
					materialTabControl_0.Deselected += delegate
					{
						int_1 = materialTabControl_0.SelectedIndex;
					};
					materialTabControl_0.SelectedIndexChanged += delegate
					{
						animationManager_0.SetProgress(0.0);
						animationManager_0.StartNewAnimation(AnimationDirection.In);
					};
					materialTabControl_0.ControlAdded += delegate
					{
						Invalidate();
					};
					materialTabControl_0.ControlRemoved += delegate
					{
						Invalidate();
					};
				}
			}
		}

		public MaterialTabSelector()
		{
			SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, value: true);
			base.Height = 48;
			animationManager_0 = new AnimationManager
			{
				AnimationType = AnimationType.EaseOut,
				Increment = 0.04
			};
			animationManager_0.OnAnimationProgress += delegate
			{
				Invalidate();
			};
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics graphics = e.Graphics;
			graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
			graphics.Clear(SkinManager.ColorScheme.PrimaryColor);
			if (materialTabControl_0 == null)
			{
				return;
			}
			if (!animationManager_0.IsAnimating() || list_0 == null || list_0.Count != materialTabControl_0.TabCount)
			{
				method_1();
			}
			double progress = animationManager_0.GetProgress();
			if (animationManager_0.IsAnimating())
			{
				SolidBrush solidBrush = new SolidBrush(Color.FromArgb((int)(51.0 - progress * 50.0), Color.White));
				int num = (int)(progress * (double)list_0[materialTabControl_0.SelectedIndex].Width * 1.75);
				graphics.SetClip(list_0[materialTabControl_0.SelectedIndex]);
				graphics.FillEllipse(solidBrush, new Rectangle(point_0.X - num / 2, point_0.Y - num / 2, num, num));
				graphics.ResetClip();
				solidBrush.Dispose();
			}
			foreach (TabPage tabPage in materialTabControl_0.TabPages)
			{
				int num2 = materialTabControl_0.TabPages.IndexOf(tabPage);
				Brush brush = new SolidBrush(Color.FromArgb(method_0(num2, progress), SkinManager.ColorScheme.TextColor));
				graphics.DrawString(tabPage.Text.ToUpper(), SkinManager.ROBOTO_MEDIUM_10, brush, list_0[num2], new StringFormat
				{
					Alignment = StringAlignment.Center,
					LineAlignment = StringAlignment.Center
				});
				brush.Dispose();
			}
			int index = ((int_1 == -1) ? materialTabControl_0.SelectedIndex : int_1);
			Rectangle rectangle = list_0[index];
			Rectangle rectangle2 = list_0[materialTabControl_0.SelectedIndex];
			int num3 = rectangle2.Bottom - 2;
			int num4 = rectangle.X + (int)((double)(rectangle2.X - rectangle.X) * progress);
			int num5 = rectangle.Width + (int)((double)(rectangle2.Width - rectangle.Width) * progress);
			graphics.FillRectangle(SkinManager.ColorScheme.AccentBrush, num4, num3, num5, 2);
		}

		private int method_0(int int_2, double double_0)
		{
			int a = SkinManager.ACTION_BAR_TEXT.A;
			int a2 = SkinManager.ACTION_BAR_TEXT_SECONDARY.A;
			if (int_2 == materialTabControl_0.SelectedIndex && !animationManager_0.IsAnimating())
			{
				return a;
			}
			if (int_2 != int_1 && int_2 != materialTabControl_0.SelectedIndex)
			{
				return a2;
			}
			if (int_2 == int_1)
			{
				return a - (int)((double)(a - a2) * double_0);
			}
			return a2 + (int)((double)(a - a2) * double_0);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (list_0 == null)
			{
				method_1();
			}
			for (int i = 0; i < list_0.Count; i++)
			{
				if (list_0[i].Contains(e.Location))
				{
					materialTabControl_0.SelectedIndex = i;
				}
			}
			point_0 = e.Location;
		}

		private void method_1()
		{
			list_0 = new List<Rectangle>();
			if (materialTabControl_0 == null || materialTabControl_0.TabCount == 0)
			{
				return;
			}
			using Bitmap image = new Bitmap(1, 1);
			using Graphics graphics = Graphics.FromImage(image);
			list_0.Add(new Rectangle(SkinManager.FORM_PADDING, 0, 48 + (int)graphics.MeasureString(materialTabControl_0.TabPages[0].Text, SkinManager.ROBOTO_MEDIUM_10).Width, base.Height));
			for (int i = 1; i < materialTabControl_0.TabPages.Count; i++)
			{
				list_0.Add(new Rectangle(list_0[i - 1].Right, 0, 48 + (int)graphics.MeasureString(materialTabControl_0.TabPages[i].Text, SkinManager.ROBOTO_MEDIUM_10).Width, base.Height));
			}
		}

		[CompilerGenerated]
		private void materialTabControl_0_Deselected(object sender, TabControlEventArgs e)
		{
			int_1 = materialTabControl_0.SelectedIndex;
		}

		[CompilerGenerated]
		private void materialTabControl_0_SelectedIndexChanged(object sender, EventArgs e)
		{
			animationManager_0.SetProgress(0.0);
			animationManager_0.StartNewAnimation(AnimationDirection.In);
		}

		[CompilerGenerated]
		private void materialTabControl_0_ControlAdded(object sender, ControlEventArgs e)
		{
			Invalidate();
		}

		[CompilerGenerated]
		private void materialTabControl_0_ControlRemoved(object sender, ControlEventArgs e)
		{
			Invalidate();
		}

		[CompilerGenerated]
		private void method_2(object object_0)
		{
			Invalidate();
		}

		internal static bool DHyJAeInhdbOeQo3w0ZW()
		{
			return sn89b1InD9UbKF0RFIvr == null;
		}

		internal static void jkM5wPInKKeU7YmCTb55()
		{
		}
	}
}

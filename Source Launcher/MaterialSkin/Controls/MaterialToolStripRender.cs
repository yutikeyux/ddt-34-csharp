using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using MaterialSkin.Animations;

namespace MaterialSkin.Controls
{
	internal class MaterialToolStripRender : ToolStripProfessionalRenderer, IMaterialControl
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private int int_0;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private MouseState mouseState_0;

		internal static MaterialToolStripRender jOlbs7IoERPxen8armUc;

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

		public MaterialSkinManager SkinManager => MaterialSkinManager.Instance;

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

		protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
		{
			Graphics graphics = e.Graphics;
			graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
			Rectangle rectangle = method_0(e.Item);
			graphics.DrawString(layoutRectangle: new Rectangle(24, rectangle.Y, rectangle.Width - 40, rectangle.Height), s: e.Text, font: SkinManager.ROBOTO_MEDIUM_10, brush: (!e.Item.Enabled) ? SkinManager.GetDisabledOrHintBrush() : SkinManager.GetPrimaryTextBrush(), format: new StringFormat
			{
				LineAlignment = StringAlignment.Center
			});
		}

		protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
		{
			Graphics graphics = e.Graphics;
			graphics.Clear(SkinManager.GetApplicationBackgroundColor());
			Rectangle rect = method_0(e.Item);
			graphics.FillRectangle((!e.Item.Selected || !e.Item.Enabled) ? new SolidBrush(SkinManager.GetApplicationBackgroundColor()) : SkinManager.GetCmsSelectedItemBrush(), rect);
			MaterialContextMenuStrip materialContextMenuStrip = e.ToolStrip as MaterialContextMenuStrip;
			if (materialContextMenuStrip == null)
			{
				return;
			}
			AnimationManager animationManager = materialContextMenuStrip.animationManager;
			Point animationSource = materialContextMenuStrip.animationSource;
			if (materialContextMenuStrip.animationManager.IsAnimating() && e.Item.Bounds.Contains(animationSource))
			{
				for (int i = 0; i < animationManager.GetAnimationCount(); i++)
				{
					double progress = animationManager.GetProgress(i);
					SolidBrush brush = new SolidBrush(Color.FromArgb((int)(51.0 - progress * 50.0), Color.Black));
					int num = (int)(progress * (double)rect.Width * 2.5);
					graphics.FillEllipse(brush, new Rectangle(animationSource.X - num / 2, rect.Y - rect.Height, num, rect.Height * 3));
				}
			}
		}

		protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
		{
		}

		protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
		{
			Graphics graphics = e.Graphics;
			graphics.FillRectangle(new SolidBrush(SkinManager.GetApplicationBackgroundColor()), e.Item.Bounds);
			graphics.DrawLine(new Pen(SkinManager.GetDividersColor()), new Point(e.Item.Bounds.Left, e.Item.Bounds.Height / 2), new Point(e.Item.Bounds.Right, e.Item.Bounds.Height / 2));
		}

		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
		{
			Graphics graphics = e.Graphics;
			graphics.DrawRectangle(new Pen(SkinManager.GetDividersColor()), new Rectangle(e.AffectedBounds.X, e.AffectedBounds.Y, e.AffectedBounds.Width - 1, e.AffectedBounds.Height - 1));
		}

		protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
		{
			Graphics graphics = e.Graphics;
			Point point = new Point(e.ArrowRectangle.X + e.ArrowRectangle.Width / 2, e.ArrowRectangle.Y + e.ArrowRectangle.Height / 2);
			Brush brush = ((!e.Item.Enabled) ? SkinManager.GetDisabledOrHintBrush() : SkinManager.GetPrimaryTextBrush());
			using GraphicsPath graphicsPath = new GraphicsPath();
			graphicsPath.AddLines(new Point[3]
			{
				new Point(point.X - 4, point.Y - 4),
				new Point(point.X, point.Y),
				new Point(point.X - 4, point.Y + 4)
			});
			graphicsPath.CloseFigure();
			graphics.FillPath(brush, graphicsPath);
		}

		private Rectangle method_0(ToolStripItem toolStripItem_0)
		{
			return new Rectangle(0, toolStripItem_0.ContentRectangle.Y, toolStripItem_0.ContentRectangle.Width + 4, toolStripItem_0.ContentRectangle.Height);
		}

		internal static bool hB9gQ4IoWhrSTww37kXD()
		{
			return jOlbs7IoERPxen8armUc == null;
		}

		internal static void FZt9hmIVISCnM4nhJTGy()
		{
		}
	}
}

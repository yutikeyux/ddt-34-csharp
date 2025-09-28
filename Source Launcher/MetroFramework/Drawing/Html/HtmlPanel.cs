using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Text;
using System.Reflection;
using System.Windows.Forms;

namespace MetroFramework.Drawing.Html
{
	[CLSCompliant(false)]
	public class HtmlPanel : ScrollableControl
	{
		protected InitialContainer htmlContainer;

		internal static HtmlPanel tInR47p36xmbJu0lAbM;

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public override bool AutoSize
		{
			get
			{
				return base.AutoSize;
			}
			set
			{
				base.AutoSize = value;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public override bool AutoScroll
		{
			get
			{
				return base.AutoScroll;
			}
			set
			{
				base.AutoScroll = value;
			}
		}

		public InitialContainer HtmlContainer => htmlContainer;

		[EditorBrowsable(EditorBrowsableState.Always)]
		[Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Localizable(true)]
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
				CreateFragment();
				MeasureBounds();
				Invalidate();
			}
		}

		public HtmlPanel()
		{
			htmlContainer = new InitialContainer();
			SetStyle(ControlStyles.ResizeRedraw, value: true);
			SetStyle(ControlStyles.Opaque, value: true);
			DoubleBuffered = true;
			BackColor = SystemColors.Window;
			AutoScroll = true;
			HtmlRenderer.AddReference(Assembly.GetCallingAssembly());
		}

		protected virtual void CreateFragment()
		{
			htmlContainer = new InitialContainer(Text);
		}

		public virtual void MeasureBounds()
		{
			htmlContainer.SetBounds((this is HtmlLabel) ? new Rectangle(0, 0, 10, 10) : base.ClientRectangle);
			using (Graphics g = CreateGraphics())
			{
				htmlContainer.MeasureBounds(g);
			}
			base.AutoScrollMinSize = System.Drawing.Size.Round(htmlContainer.MaximumSize);
		}

		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);
			Focus();
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			MeasureBounds();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			if (!(this is HtmlLabel))
			{
				e.Graphics.Clear(SystemColors.Window);
			}
			htmlContainer.ScrollOffset = base.AutoScrollPosition;
			e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
			htmlContainer.Paint(e.Graphics);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			foreach (CssBox key in htmlContainer.LinkRegions.Keys)
			{
				RectangleF value = htmlContainer.LinkRegions[key];
				if (Rectangle.Round(value).Contains(e.X, e.Y))
				{
					Cursor = Cursors.Hand;
					return;
				}
			}
			Cursor = Cursors.Default;
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			base.OnMouseClick(e);
			foreach (CssBox key in htmlContainer.LinkRegions.Keys)
			{
				RectangleF value = htmlContainer.LinkRegions[key];
				if (Rectangle.Round(value).Contains(e.X, e.Y))
				{
					CssValue.GoLink(key.GetAttribute("href", string.Empty));
					break;
				}
			}
		}

		internal static bool Bq0UHBp2JlVAEACjqd7()
		{
			return tInR47p36xmbJu0lAbM == null;
		}
	}
}

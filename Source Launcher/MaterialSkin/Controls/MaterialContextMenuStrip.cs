using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using MaterialSkin.Animations;

namespace MaterialSkin.Controls
{
	public class MaterialContextMenuStrip : ContextMenuStrip, IMaterialControl
	{
		public delegate void ItemClickStart(object sender, ToolStripItemClickedEventArgs e);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private int int_0;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private MouseState mouseState_0;

		internal AnimationManager animationManager;

		internal Point animationSource;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private ItemClickStart itemClickStart_0;

		private ToolStripItemClickedEventArgs toolStripItemClickedEventArgs_0;

		internal static MaterialContextMenuStrip eWYKjxIo8FjktV0pOod7;

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

		public event ItemClickStart OnItemClickStart
		{
			[CompilerGenerated]
			add
			{
				ItemClickStart itemClickStart = itemClickStart_0;
				ItemClickStart itemClickStart2;
				do
				{
					itemClickStart2 = itemClickStart;
					ItemClickStart value2 = (ItemClickStart)Delegate.Combine(itemClickStart2, value);
					itemClickStart = Interlocked.CompareExchange(ref itemClickStart_0, value2, itemClickStart2);
				}
				while ((object)itemClickStart != itemClickStart2);
			}
			[CompilerGenerated]
			remove
			{
				ItemClickStart itemClickStart = itemClickStart_0;
				ItemClickStart itemClickStart2;
				do
				{
					itemClickStart2 = itemClickStart;
					ItemClickStart value2 = (ItemClickStart)Delegate.Remove(itemClickStart2, value);
					itemClickStart = Interlocked.CompareExchange(ref itemClickStart_0, value2, itemClickStart2);
				}
				while ((object)itemClickStart != itemClickStart2);
			}
		}

		public MaterialContextMenuStrip()
		{
			base.Renderer = new MaterialToolStripRender();
			animationManager = new AnimationManager(singular: false)
			{
				Increment = 0.07,
				AnimationType = AnimationType.Linear
			};
			animationManager.OnAnimationProgress += delegate
			{
				Invalidate();
			};
			animationManager.OnAnimationFinished += delegate
			{
				OnItemClicked(toolStripItemClickedEventArgs_0);
			};
			base.BackColor = SkinManager.GetApplicationBackgroundColor();
		}

		protected override void OnMouseUp(MouseEventArgs mea)
		{
			base.OnMouseUp(mea);
			animationSource = mea.Location;
		}

		protected override void OnItemClicked(ToolStripItemClickedEventArgs e)
		{
			if (e.ClickedItem == null || e.ClickedItem is ToolStripSeparator)
			{
				return;
			}
			if (e == toolStripItemClickedEventArgs_0)
			{
				base.OnItemClicked(e);
				return;
			}
			toolStripItemClickedEventArgs_0 = e;
			if (itemClickStart_0 != null)
			{
				itemClickStart_0(this, e);
			}
			animationManager.StartNewAnimation(AnimationDirection.In);
		}

		[CompilerGenerated]
		private void method_0(object object_0)
		{
			Invalidate();
		}

		[CompilerGenerated]
		private void method_1(object object_0)
		{
			OnItemClicked(toolStripItemClickedEventArgs_0);
		}

		internal static bool LLVoUQIoKpJWAOXkQe89()
		{
			return eWYKjxIo8FjktV0pOod7 == null;
		}

		internal static void jldi5PIo9xKbwhXfEoyx()
		{
		}
	}
}

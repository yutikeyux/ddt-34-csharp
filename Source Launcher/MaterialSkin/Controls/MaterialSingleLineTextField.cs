using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MaterialSkin.Animations;

namespace MaterialSkin.Controls
{
	public class MaterialSingleLineTextField : Control, IMaterialControl
	{
		private class Class33 : TextBox
		{
			private string string_0 = string.Empty;

			private char char_0 = '\0';

			private char char_1 = '\0';

			private static object DXmZPSIY1d5j2FQ9MXXU;

			[DllImport("user32.dll", CharSet = CharSet.Auto)]
			private static extern IntPtr SendMessage(IntPtr intptr_0, int int_0, int int_1, string string_1);

			[SpecialName]
			public string method_0()
			{
				return string_0;
			}

			[SpecialName]
			public void method_1(string string_1)
			{
				string_0 = string_1;
				SendMessage(base.Handle, 5377, (int)IntPtr.Zero, method_0());
			}

			[SpecialName]
			public char method_2()
			{
				return char_0;
			}

			[SpecialName]
			public void method_3(char char_2)
			{
				char_0 = char_2;
				method_8();
			}

			public void method_4()
			{
				BeginInvoke((MethodInvoker)delegate
				{
					Focus();
					SelectAll();
				});
			}

			public void method_5()
			{
				BeginInvoke((MethodInvoker)delegate
				{
					Focus();
				});
			}

			[SpecialName]
			public bool method_6()
			{
				return char_1 != '\0';
			}

			[SpecialName]
			public void method_7(bool bool_0)
			{
				if (bool_0)
				{
					char_1 = ((!Application.RenderWithVisualStyles) ? '*' : '●');
				}
				else
				{
					char_1 = '\0';
				}
				method_8();
			}

			private void method_8()
			{
				base.PasswordChar = (method_6() ? char_1 : char_0);
			}

			public Class33()
			{
				MaterialContextMenuStrip materialContextMenuStrip = new Class32();
				materialContextMenuStrip.Opening += method_10;
				materialContextMenuStrip.OnItemClickStart += method_9;
				ContextMenuStrip = materialContextMenuStrip;
			}

			private void method_9(object sender, ToolStripItemClickedEventArgs e)
			{
				switch (e.ClickedItem.Text)
				{
				case "Undo":
					Undo();
					break;
				case "Copy":
					Copy();
					break;
				case "Select All":
					method_4();
					break;
				case "Delete":
					SelectedText = string.Empty;
					break;
				case "Paste":
					Paste();
					break;
				case "Cut":
					Cut();
					break;
				}
			}

			private void method_10(object sender, CancelEventArgs e)
			{
				Class32 @class = sender as Class32;
				if (@class != null)
				{
					@class.materialToolStripMenuItem_0.Enabled = base.CanUndo;
					@class.materialToolStripMenuItem_1.Enabled = !string.IsNullOrEmpty(SelectedText);
					@class.materialToolStripMenuItem_2.Enabled = !string.IsNullOrEmpty(SelectedText);
					@class.materialToolStripMenuItem_3.Enabled = Clipboard.ContainsText();
					@class.materialToolStripMenuItem_4.Enabled = !string.IsNullOrEmpty(SelectedText);
					@class.materialToolStripMenuItem_5.Enabled = !string.IsNullOrEmpty(Text);
				}
			}

			[CompilerGenerated]
			private void method_11()
			{
				Focus();
				SelectAll();
			}

			[CompilerGenerated]
			private void method_12()
			{
				Focus();
			}

			internal static bool r9oAGMIYxVcew0YMv8Vo()
			{
				return DXmZPSIY1d5j2FQ9MXXU == null;
			}

			internal static void Dq60lJIYbuLQsi7Q9u9c()
			{
			}
		}

		private class Class32 : MaterialContextMenuStrip
		{
			public readonly MaterialToolStripMenuItem materialToolStripMenuItem_0 = new MaterialToolStripMenuItem
			{
				Text = "Undo"
			};

			public readonly ToolStripSeparator toolStripSeparator_0 = new ToolStripSeparator();

			public readonly MaterialToolStripMenuItem materialToolStripMenuItem_1 = new MaterialToolStripMenuItem
			{
				Text = "Cut"
			};

			public readonly MaterialToolStripMenuItem materialToolStripMenuItem_2 = new MaterialToolStripMenuItem
			{
				Text = "Copy"
			};

			public readonly MaterialToolStripMenuItem materialToolStripMenuItem_3 = new MaterialToolStripMenuItem
			{
				Text = "Paste"
			};

			public readonly MaterialToolStripMenuItem materialToolStripMenuItem_4 = new MaterialToolStripMenuItem
			{
				Text = "Delete"
			};

			public readonly ToolStripSeparator toolStripSeparator_1 = new ToolStripSeparator();

			public readonly MaterialToolStripMenuItem materialToolStripMenuItem_5 = new MaterialToolStripMenuItem
			{
				Text = "Select All"
			};

			internal static object cRXhGDIYOGarB2fxNkNQ;

			public Class32()
			{
				Items.AddRange(new ToolStripItem[8] { materialToolStripMenuItem_0, toolStripSeparator_0, materialToolStripMenuItem_1, materialToolStripMenuItem_2, materialToolStripMenuItem_3, materialToolStripMenuItem_4, toolStripSeparator_1, materialToolStripMenuItem_5 });
			}

			internal static void GqGF89IYmFjder1oi1aS()
			{
			}

			internal static bool fUaC80IYf9RX9wMTd8jN()
			{
				return cRXhGDIYOGarB2fxNkNQ == null;
			}
		}

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private int int_0;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private MouseState mouseState_0;

		private readonly AnimationManager animationManager_0;

		private readonly Class33 class33_0;

		internal static MaterialSingleLineTextField nJrDDHInqjTTSLbWOwFP;

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

		public override string Text
		{
			get
			{
				return class33_0.Text;
			}
			set
			{
				class33_0.Text = value;
			}
		}

		public new object Tag
		{
			get
			{
				return class33_0.Tag;
			}
			set
			{
				class33_0.Tag = value;
			}
		}

		public int MaxLength
		{
			get
			{
				return class33_0.MaxLength;
			}
			set
			{
				class33_0.MaxLength = value;
			}
		}

		public string SelectedText
		{
			get
			{
				return class33_0.SelectedText;
			}
			set
			{
				class33_0.SelectedText = value;
			}
		}

		public string Hint
		{
			get
			{
				return class33_0.method_0();
			}
			set
			{
				class33_0.method_1(value);
			}
		}

		public int SelectionStart
		{
			get
			{
				return class33_0.SelectionStart;
			}
			set
			{
				class33_0.SelectionStart = value;
			}
		}

		public int SelectionLength
		{
			get
			{
				return class33_0.SelectionLength;
			}
			set
			{
				class33_0.SelectionLength = value;
			}
		}

		public int TextLength => class33_0.TextLength;

		public bool UseSystemPasswordChar
		{
			get
			{
				return class33_0.method_6();
			}
			set
			{
				class33_0.method_7(value);
			}
		}

		public char PasswordChar
		{
			get
			{
				return class33_0.method_2();
			}
			set
			{
				class33_0.method_3(value);
			}
		}

		public event EventHandler AcceptsTabChanged
		{
			add
			{
				class33_0.AcceptsTabChanged += value;
			}
			remove
			{
				class33_0.AcceptsTabChanged -= value;
			}
		}

		public new event EventHandler AutoSizeChanged
		{
			add
			{
				class33_0.AutoSizeChanged += value;
			}
			remove
			{
				class33_0.AutoSizeChanged -= value;
			}
		}

		public new event EventHandler BackgroundImageChanged
		{
			add
			{
				class33_0.BackgroundImageChanged += value;
			}
			remove
			{
				class33_0.BackgroundImageChanged -= value;
			}
		}

		public new event EventHandler BackgroundImageLayoutChanged
		{
			add
			{
				class33_0.BackgroundImageLayoutChanged += value;
			}
			remove
			{
				class33_0.BackgroundImageLayoutChanged -= value;
			}
		}

		public new event EventHandler BindingContextChanged
		{
			add
			{
				class33_0.BindingContextChanged += value;
			}
			remove
			{
				class33_0.BindingContextChanged -= value;
			}
		}

		public event EventHandler BorderStyleChanged
		{
			add
			{
				class33_0.BorderStyleChanged += value;
			}
			remove
			{
				class33_0.BorderStyleChanged -= value;
			}
		}

		public new event EventHandler CausesValidationChanged
		{
			add
			{
				class33_0.CausesValidationChanged += value;
			}
			remove
			{
				class33_0.CausesValidationChanged -= value;
			}
		}

		public new event UICuesEventHandler ChangeUICues
		{
			add
			{
				class33_0.ChangeUICues += value;
			}
			remove
			{
				class33_0.ChangeUICues -= value;
			}
		}

		public new event EventHandler Click
		{
			add
			{
				class33_0.Click += value;
			}
			remove
			{
				class33_0.Click -= value;
			}
		}

		public new event EventHandler ClientSizeChanged
		{
			add
			{
				class33_0.ClientSizeChanged += value;
			}
			remove
			{
				class33_0.ClientSizeChanged -= value;
			}
		}

		public new event EventHandler ContextMenuChanged
		{
			add
			{
				class33_0.ContextMenuChanged += value;
			}
			remove
			{
				class33_0.ContextMenuChanged -= value;
			}
		}

		public new event EventHandler ContextMenuStripChanged
		{
			add
			{
				class33_0.ContextMenuStripChanged += value;
			}
			remove
			{
				class33_0.ContextMenuStripChanged -= value;
			}
		}

		public new event ControlEventHandler ControlAdded
		{
			add
			{
				class33_0.ControlAdded += value;
			}
			remove
			{
				class33_0.ControlAdded -= value;
			}
		}

		public new event ControlEventHandler ControlRemoved
		{
			add
			{
				class33_0.ControlRemoved += value;
			}
			remove
			{
				class33_0.ControlRemoved -= value;
			}
		}

		public new event EventHandler CursorChanged
		{
			add
			{
				class33_0.CursorChanged += value;
			}
			remove
			{
				class33_0.CursorChanged -= value;
			}
		}

		public new event EventHandler Disposed
		{
			add
			{
				class33_0.Disposed += value;
			}
			remove
			{
				class33_0.Disposed -= value;
			}
		}

		public new event EventHandler DockChanged
		{
			add
			{
				class33_0.DockChanged += value;
			}
			remove
			{
				class33_0.DockChanged -= value;
			}
		}

		public new event EventHandler DoubleClick
		{
			add
			{
				class33_0.DoubleClick += value;
			}
			remove
			{
				class33_0.DoubleClick -= value;
			}
		}

		public new event DragEventHandler DragDrop
		{
			add
			{
				class33_0.DragDrop += value;
			}
			remove
			{
				class33_0.DragDrop -= value;
			}
		}

		public new event DragEventHandler DragEnter
		{
			add
			{
				class33_0.DragEnter += value;
			}
			remove
			{
				class33_0.DragEnter -= value;
			}
		}

		public new event EventHandler DragLeave
		{
			add
			{
				class33_0.DragLeave += value;
			}
			remove
			{
				class33_0.DragLeave -= value;
			}
		}

		public new event DragEventHandler DragOver
		{
			add
			{
				class33_0.DragOver += value;
			}
			remove
			{
				class33_0.DragOver -= value;
			}
		}

		public new event EventHandler EnabledChanged
		{
			add
			{
				class33_0.EnabledChanged += value;
			}
			remove
			{
				class33_0.EnabledChanged -= value;
			}
		}

		public new event EventHandler Enter
		{
			add
			{
				class33_0.Enter += value;
			}
			remove
			{
				class33_0.Enter -= value;
			}
		}

		public new event EventHandler FontChanged
		{
			add
			{
				class33_0.FontChanged += value;
			}
			remove
			{
				class33_0.FontChanged -= value;
			}
		}

		public new event EventHandler ForeColorChanged
		{
			add
			{
				class33_0.ForeColorChanged += value;
			}
			remove
			{
				class33_0.ForeColorChanged -= value;
			}
		}

		public new event GiveFeedbackEventHandler GiveFeedback
		{
			add
			{
				class33_0.GiveFeedback += value;
			}
			remove
			{
				class33_0.GiveFeedback -= value;
			}
		}

		public new event EventHandler GotFocus
		{
			add
			{
				class33_0.GotFocus += value;
			}
			remove
			{
				class33_0.GotFocus -= value;
			}
		}

		public new event EventHandler HandleCreated
		{
			add
			{
				class33_0.HandleCreated += value;
			}
			remove
			{
				class33_0.HandleCreated -= value;
			}
		}

		public new event EventHandler HandleDestroyed
		{
			add
			{
				class33_0.HandleDestroyed += value;
			}
			remove
			{
				class33_0.HandleDestroyed -= value;
			}
		}

		public new event HelpEventHandler HelpRequested
		{
			add
			{
				class33_0.HelpRequested += value;
			}
			remove
			{
				class33_0.HelpRequested -= value;
			}
		}

		public event EventHandler HideSelectionChanged
		{
			add
			{
				class33_0.HideSelectionChanged += value;
			}
			remove
			{
				class33_0.HideSelectionChanged -= value;
			}
		}

		public new event EventHandler ImeModeChanged
		{
			add
			{
				class33_0.ImeModeChanged += value;
			}
			remove
			{
				class33_0.ImeModeChanged -= value;
			}
		}

		public new event InvalidateEventHandler Invalidated
		{
			add
			{
				class33_0.Invalidated += value;
			}
			remove
			{
				class33_0.Invalidated -= value;
			}
		}

		public new event KeyEventHandler KeyDown
		{
			add
			{
				class33_0.KeyDown += value;
			}
			remove
			{
				class33_0.KeyDown -= value;
			}
		}

		public new event KeyPressEventHandler KeyPress
		{
			add
			{
				class33_0.KeyPress += value;
			}
			remove
			{
				class33_0.KeyPress -= value;
			}
		}

		public new event KeyEventHandler KeyUp
		{
			add
			{
				class33_0.KeyUp += value;
			}
			remove
			{
				class33_0.KeyUp -= value;
			}
		}

		public new event LayoutEventHandler Layout
		{
			add
			{
				class33_0.Layout += value;
			}
			remove
			{
				class33_0.Layout -= value;
			}
		}

		public new event EventHandler Leave
		{
			add
			{
				class33_0.Leave += value;
			}
			remove
			{
				class33_0.Leave -= value;
			}
		}

		public new event EventHandler LocationChanged
		{
			add
			{
				class33_0.LocationChanged += value;
			}
			remove
			{
				class33_0.LocationChanged -= value;
			}
		}

		public new event EventHandler LostFocus
		{
			add
			{
				class33_0.LostFocus += value;
			}
			remove
			{
				class33_0.LostFocus -= value;
			}
		}

		public new event EventHandler MarginChanged
		{
			add
			{
				class33_0.MarginChanged += value;
			}
			remove
			{
				class33_0.MarginChanged -= value;
			}
		}

		public event EventHandler ModifiedChanged
		{
			add
			{
				class33_0.ModifiedChanged += value;
			}
			remove
			{
				class33_0.ModifiedChanged -= value;
			}
		}

		public new event EventHandler MouseCaptureChanged
		{
			add
			{
				class33_0.MouseCaptureChanged += value;
			}
			remove
			{
				class33_0.MouseCaptureChanged -= value;
			}
		}

		public new event MouseEventHandler MouseClick
		{
			add
			{
				class33_0.MouseClick += value;
			}
			remove
			{
				class33_0.MouseClick -= value;
			}
		}

		public new event MouseEventHandler MouseDoubleClick
		{
			add
			{
				class33_0.MouseDoubleClick += value;
			}
			remove
			{
				class33_0.MouseDoubleClick -= value;
			}
		}

		public new event MouseEventHandler MouseDown
		{
			add
			{
				class33_0.MouseDown += value;
			}
			remove
			{
				class33_0.MouseDown -= value;
			}
		}

		public new event EventHandler MouseEnter
		{
			add
			{
				class33_0.MouseEnter += value;
			}
			remove
			{
				class33_0.MouseEnter -= value;
			}
		}

		public new event EventHandler MouseHover
		{
			add
			{
				class33_0.MouseHover += value;
			}
			remove
			{
				class33_0.MouseHover -= value;
			}
		}

		public new event EventHandler MouseLeave
		{
			add
			{
				class33_0.MouseLeave += value;
			}
			remove
			{
				class33_0.MouseLeave -= value;
			}
		}

		public new event MouseEventHandler MouseMove
		{
			add
			{
				class33_0.MouseMove += value;
			}
			remove
			{
				class33_0.MouseMove -= value;
			}
		}

		public new event MouseEventHandler MouseUp
		{
			add
			{
				class33_0.MouseUp += value;
			}
			remove
			{
				class33_0.MouseUp -= value;
			}
		}

		public new event MouseEventHandler MouseWheel
		{
			add
			{
				class33_0.MouseWheel += value;
			}
			remove
			{
				class33_0.MouseWheel -= value;
			}
		}

		public new event EventHandler Move
		{
			add
			{
				class33_0.Move += value;
			}
			remove
			{
				class33_0.Move -= value;
			}
		}

		public event EventHandler MultilineChanged
		{
			add
			{
				class33_0.MultilineChanged += value;
			}
			remove
			{
				class33_0.MultilineChanged -= value;
			}
		}

		public new event EventHandler PaddingChanged
		{
			add
			{
				class33_0.PaddingChanged += value;
			}
			remove
			{
				class33_0.PaddingChanged -= value;
			}
		}

		public new event PaintEventHandler Paint
		{
			add
			{
				class33_0.Paint += value;
			}
			remove
			{
				class33_0.Paint -= value;
			}
		}

		public new event EventHandler ParentChanged
		{
			add
			{
				class33_0.ParentChanged += value;
			}
			remove
			{
				class33_0.ParentChanged -= value;
			}
		}

		public new event PreviewKeyDownEventHandler PreviewKeyDown
		{
			add
			{
				class33_0.PreviewKeyDown += value;
			}
			remove
			{
				class33_0.PreviewKeyDown -= value;
			}
		}

		public new event QueryAccessibilityHelpEventHandler QueryAccessibilityHelp
		{
			add
			{
				class33_0.QueryAccessibilityHelp += value;
			}
			remove
			{
				class33_0.QueryAccessibilityHelp -= value;
			}
		}

		public new event QueryContinueDragEventHandler QueryContinueDrag
		{
			add
			{
				class33_0.QueryContinueDrag += value;
			}
			remove
			{
				class33_0.QueryContinueDrag -= value;
			}
		}

		public event EventHandler ReadOnlyChanged
		{
			add
			{
				class33_0.ReadOnlyChanged += value;
			}
			remove
			{
				class33_0.ReadOnlyChanged -= value;
			}
		}

		public new event EventHandler RegionChanged
		{
			add
			{
				class33_0.RegionChanged += value;
			}
			remove
			{
				class33_0.RegionChanged -= value;
			}
		}

		public new event EventHandler Resize
		{
			add
			{
				class33_0.Resize += value;
			}
			remove
			{
				class33_0.Resize -= value;
			}
		}

		public new event EventHandler RightToLeftChanged
		{
			add
			{
				class33_0.RightToLeftChanged += value;
			}
			remove
			{
				class33_0.RightToLeftChanged -= value;
			}
		}

		public new event EventHandler SizeChanged
		{
			add
			{
				class33_0.SizeChanged += value;
			}
			remove
			{
				class33_0.SizeChanged -= value;
			}
		}

		public new event EventHandler StyleChanged
		{
			add
			{
				class33_0.StyleChanged += value;
			}
			remove
			{
				class33_0.StyleChanged -= value;
			}
		}

		public new event EventHandler SystemColorsChanged
		{
			add
			{
				class33_0.SystemColorsChanged += value;
			}
			remove
			{
				class33_0.SystemColorsChanged -= value;
			}
		}

		public new event EventHandler TabIndexChanged
		{
			add
			{
				class33_0.TabIndexChanged += value;
			}
			remove
			{
				class33_0.TabIndexChanged -= value;
			}
		}

		public new event EventHandler TabStopChanged
		{
			add
			{
				class33_0.TabStopChanged += value;
			}
			remove
			{
				class33_0.TabStopChanged -= value;
			}
		}

		public event EventHandler TextAlignChanged
		{
			add
			{
				class33_0.TextAlignChanged += value;
			}
			remove
			{
				class33_0.TextAlignChanged -= value;
			}
		}

		public new event EventHandler TextChanged
		{
			add
			{
				class33_0.TextChanged += value;
			}
			remove
			{
				class33_0.TextChanged -= value;
			}
		}

		public new event EventHandler Validated
		{
			add
			{
				class33_0.Validated += value;
			}
			remove
			{
				class33_0.Validated -= value;
			}
		}

		public new event CancelEventHandler Validating
		{
			add
			{
				class33_0.Validating += value;
			}
			remove
			{
				class33_0.Validating -= value;
			}
		}

		public new event EventHandler VisibleChanged
		{
			add
			{
				class33_0.VisibleChanged += value;
			}
			remove
			{
				class33_0.VisibleChanged -= value;
			}
		}

		public void SelectAll()
		{
			class33_0.method_4();
		}

		public void Clear()
		{
			class33_0.Clear();
		}

		public new void Focus()
		{
			class33_0.method_5();
		}

		public MaterialSingleLineTextField()
		{
			SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, value: true);
			animationManager_0 = new AnimationManager
			{
				Increment = 0.06,
				AnimationType = AnimationType.EaseInOut,
				InterruptAnimation = false
			};
			animationManager_0.OnAnimationProgress += delegate
			{
				Invalidate();
			};
			class33_0 = new Class33
			{
				BorderStyle = BorderStyle.None,
				Font = SkinManager.ROBOTO_REGULAR_11,
				ForeColor = Color.DarkSlateGray,
				TextAlign = HorizontalAlignment.Center,
				Location = new Point(0, 0),
				Width = base.Width,
				Height = base.Height - 5
			};
			if (!base.Controls.Contains(class33_0) && !base.DesignMode)
			{
				base.Controls.Add(class33_0);
			}
			class33_0.GotFocus += delegate
			{
				animationManager_0.StartNewAnimation(AnimationDirection.In);
			};
			class33_0.LostFocus += delegate
			{
				animationManager_0.StartNewAnimation(AnimationDirection.Out);
			};
			base.BackColorChanged += delegate
			{
				class33_0.BackColor = BackColor;
				class33_0.ForeColor = Color.DarkSlateGray;
				class33_0.TextAlign = HorizontalAlignment.Center;
			};
			class33_0.TabStop = true;
			base.TabStop = false;
		}

		protected override void OnPaint(PaintEventArgs pevent)
		{
			Graphics graphics = pevent.Graphics;
			graphics.Clear(base.Parent.BackColor);
			int num = class33_0.Bottom + 3;
			if (!animationManager_0.IsAnimating())
			{
				graphics.FillRectangle(class33_0.Focused ? SkinManager.ColorScheme.PrimaryBrush : SkinManager.GetDividersBrush(), class33_0.Location.X, num, class33_0.Width, (!class33_0.Focused) ? 1 : 2);
				return;
			}
			int num2 = (int)((double)class33_0.Width * animationManager_0.GetProgress());
			int num3 = num2 / 2;
			int num4 = class33_0.Location.X + class33_0.Width / 2;
			graphics.FillRectangle(SkinManager.GetDividersBrush(), class33_0.Location.X, num, class33_0.Width, 1);
			graphics.FillRectangle(SkinManager.ColorScheme.PrimaryBrush, num4 - num3, num, num2, 2);
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			class33_0.Location = new Point(0, 0);
			class33_0.Width = base.Width;
			base.Height = class33_0.Height + 5;
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			class33_0.ForeColor = Color.DarkSlateGray;
			class33_0.TextAlign = HorizontalAlignment.Center;
		}

		[CompilerGenerated]
		private void method_0(object object_0)
		{
			Invalidate();
		}

		[CompilerGenerated]
		private void class33_0_GotFocus(object sender, EventArgs e)
		{
			animationManager_0.StartNewAnimation(AnimationDirection.In);
		}

		[CompilerGenerated]
		private void class33_0_LostFocus(object sender, EventArgs e)
		{
			animationManager_0.StartNewAnimation(AnimationDirection.Out);
		}

		[CompilerGenerated]
		private void MaterialSingleLineTextField_BackColorChanged(object sender, EventArgs e)
		{
			class33_0.BackColor = BackColor;
			class33_0.ForeColor = Color.DarkSlateGray;
			class33_0.TextAlign = HorizontalAlignment.Center;
		}

		internal static bool SuRniOInLit3myYjGJ4C()
		{
			return nJrDDHInqjTTSLbWOwFP == null;
		}
	}
}

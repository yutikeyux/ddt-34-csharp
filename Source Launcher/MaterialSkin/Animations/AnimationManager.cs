using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

namespace MaterialSkin.Animations
{
	internal class AnimationManager
	{
		public delegate void AnimationFinished(object sender);

		public delegate void AnimationProgress(object sender);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private bool bool_0;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private double double_0;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private double double_1;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private AnimationType animationType_0;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool bool_1;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private AnimationFinished animationFinished_0;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private AnimationProgress animationProgress_0;

		private readonly List<double> list_0;

		private readonly List<Point> list_1;

		private readonly List<AnimationDirection> list_2;

		private readonly List<object[]> list_3;

		private readonly System.Windows.Forms.Timer timer_0 = new System.Windows.Forms.Timer
		{
			Interval = 5,
			Enabled = false
		};

		private static AnimationManager trHMNRICUVSUiDMJbBSw;

		public bool InterruptAnimation
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

		public double Increment
		{
			[CompilerGenerated]
			get
			{
				return double_0;
			}
			[CompilerGenerated]
			set
			{
				double_0 = value;
			}
		}

		public double SecondaryIncrement
		{
			[CompilerGenerated]
			get
			{
				return double_1;
			}
			[CompilerGenerated]
			set
			{
				double_1 = value;
			}
		}

		public AnimationType AnimationType
		{
			[CompilerGenerated]
			get
			{
				return animationType_0;
			}
			[CompilerGenerated]
			set
			{
				animationType_0 = value;
			}
		}

		public bool Singular
		{
			[CompilerGenerated]
			get
			{
				return bool_1;
			}
			[CompilerGenerated]
			set
			{
				bool_1 = value;
			}
		}

		public event AnimationFinished OnAnimationFinished
		{
			[CompilerGenerated]
			add
			{
				AnimationFinished animationFinished = animationFinished_0;
				AnimationFinished animationFinished2;
				do
				{
					animationFinished2 = animationFinished;
					AnimationFinished value2 = (AnimationFinished)Delegate.Combine(animationFinished2, value);
					animationFinished = Interlocked.CompareExchange(ref animationFinished_0, value2, animationFinished2);
				}
				while ((object)animationFinished != animationFinished2);
			}
			[CompilerGenerated]
			remove
			{
				AnimationFinished animationFinished = animationFinished_0;
				AnimationFinished animationFinished2;
				do
				{
					animationFinished2 = animationFinished;
					AnimationFinished value2 = (AnimationFinished)Delegate.Remove(animationFinished2, value);
					animationFinished = Interlocked.CompareExchange(ref animationFinished_0, value2, animationFinished2);
				}
				while ((object)animationFinished != animationFinished2);
			}
		}

		public event AnimationProgress OnAnimationProgress
		{
			[CompilerGenerated]
			add
			{
				AnimationProgress animationProgress = animationProgress_0;
				AnimationProgress animationProgress2;
				do
				{
					animationProgress2 = animationProgress;
					AnimationProgress value2 = (AnimationProgress)Delegate.Combine(animationProgress2, value);
					animationProgress = Interlocked.CompareExchange(ref animationProgress_0, value2, animationProgress2);
				}
				while ((object)animationProgress != animationProgress2);
			}
			[CompilerGenerated]
			remove
			{
				AnimationProgress animationProgress = animationProgress_0;
				AnimationProgress animationProgress2;
				do
				{
					animationProgress2 = animationProgress;
					AnimationProgress value2 = (AnimationProgress)Delegate.Remove(animationProgress2, value);
					animationProgress = Interlocked.CompareExchange(ref animationProgress_0, value2, animationProgress2);
				}
				while ((object)animationProgress != animationProgress2);
			}
		}

		public AnimationManager(bool singular = true)
		{
			list_0 = new List<double>();
			list_1 = new List<Point>();
			list_2 = new List<AnimationDirection>();
			list_3 = new List<object[]>();
			Increment = 0.03;
			SecondaryIncrement = 0.03;
			AnimationType = AnimationType.Linear;
			InterruptAnimation = true;
			Singular = singular;
			if (Singular)
			{
				list_0.Add(0.0);
				list_1.Add(new Point(0, 0));
				list_2.Add(AnimationDirection.In);
			}
			timer_0.Tick += timer_0_Tick;
		}

		private void timer_0_Tick(object sender, EventArgs e)
		{
			for (int i = 0; i < list_0.Count; i++)
			{
				UpdateProgress(i);
				if (Singular)
				{
					if (list_2[i] == AnimationDirection.InOutIn && list_0[i] == 1.0)
					{
						list_2[i] = AnimationDirection.InOutOut;
					}
					else if (list_2[i] == AnimationDirection.InOutRepeatingIn && list_0[i] == 1.0)
					{
						list_2[i] = AnimationDirection.InOutRepeatingOut;
					}
					else if (list_2[i] == AnimationDirection.InOutRepeatingOut && list_0[i] == 0.0)
					{
						list_2[i] = AnimationDirection.InOutRepeatingIn;
					}
				}
				else if (list_2[i] != AnimationDirection.InOutIn || list_0[i] != 1.0)
				{
					if (list_2[i] != AnimationDirection.InOutRepeatingIn || list_0[i] != 0.0)
					{
						if (list_2[i] == AnimationDirection.InOutRepeatingOut && list_0[i] == 0.0)
						{
							list_2[i] = AnimationDirection.InOutRepeatingIn;
						}
						else if ((list_2[i] == AnimationDirection.In && list_0[i] == 1.0) || (list_2[i] == AnimationDirection.Out && list_0[i] == 0.0) || (list_2[i] == AnimationDirection.InOutOut && list_0[i] == 0.0))
						{
							list_0.RemoveAt(i);
							list_1.RemoveAt(i);
							list_2.RemoveAt(i);
							list_3.RemoveAt(i);
						}
					}
					else
					{
						list_2[i] = AnimationDirection.InOutRepeatingOut;
					}
				}
				else
				{
					list_2[i] = AnimationDirection.InOutOut;
				}
			}
			if (animationProgress_0 != null)
			{
				animationProgress_0(this);
			}
		}

		public bool IsAnimating()
		{
			return timer_0.Enabled;
		}

		public void StartNewAnimation(AnimationDirection animationDirection, object[] data = null)
		{
			StartNewAnimation(animationDirection, new Point(0, 0), data);
		}

		public void StartNewAnimation(AnimationDirection animationDirection, Point animationSource, object[] data = null)
		{
			if (!IsAnimating() || InterruptAnimation)
			{
				if (Singular && list_2.Count > 0)
				{
					list_2[0] = animationDirection;
				}
				else
				{
					list_2.Add(animationDirection);
				}
				if (Singular && list_1.Count > 0)
				{
					list_1[0] = animationSource;
				}
				else
				{
					list_1.Add(animationSource);
				}
				if (!Singular || list_0.Count <= 0)
				{
					switch (list_2[list_2.Count - 1])
					{
					default:
						throw new Exception("Invalid AnimationDirection");
					case AnimationDirection.In:
					case AnimationDirection.InOutIn:
					case AnimationDirection.InOutRepeatingIn:
						list_0.Add(0.0);
						break;
					case AnimationDirection.Out:
					case AnimationDirection.InOutOut:
					case AnimationDirection.InOutRepeatingOut:
						list_0.Add(1.0);
						break;
					}
				}
				if (Singular && list_3.Count > 0)
				{
					list_3[0] = data ?? new object[0];
				}
				else
				{
					list_3.Add(data ?? new object[0]);
				}
			}
			timer_0.Start();
		}

		public void UpdateProgress(int index)
		{
			switch (list_2[index])
			{
			default:
				throw new Exception("No AnimationDirection has been set");
			case AnimationDirection.In:
			case AnimationDirection.InOutIn:
			case AnimationDirection.InOutRepeatingIn:
				method_0(index);
				break;
			case AnimationDirection.Out:
			case AnimationDirection.InOutOut:
			case AnimationDirection.InOutRepeatingOut:
				method_1(index);
				break;
			}
		}

		private void method_0(int int_0)
		{
			list_0[int_0] += Increment;
			if (!(list_0[int_0] > 1.0))
			{
				return;
			}
			list_0[int_0] = 1.0;
			int num = 0;
			while (true)
			{
				if (num < GetAnimationCount())
				{
					if (list_2[num] != AnimationDirection.InOutIn && list_2[num] != AnimationDirection.InOutRepeatingIn && list_2[num] != AnimationDirection.InOutRepeatingOut && (list_2[num] != AnimationDirection.InOutOut || list_0[num] == 1.0) && (list_2[num] != 0 || list_0[num] == 1.0))
					{
						num++;
						continue;
					}
					break;
				}
				timer_0.Stop();
				if (animationFinished_0 != null)
				{
					animationFinished_0(this);
				}
				break;
			}
		}

		private void method_1(int int_0)
		{
			list_0[int_0] -= ((list_2[int_0] == AnimationDirection.InOutOut || list_2[int_0] == AnimationDirection.InOutRepeatingOut) ? SecondaryIncrement : Increment);
			if (!(list_0[int_0] < 0.0))
			{
				return;
			}
			list_0[int_0] = 0.0;
			int num = 0;
			while (true)
			{
				if (num < GetAnimationCount())
				{
					if (list_2[num] != AnimationDirection.InOutIn && list_2[num] != AnimationDirection.InOutRepeatingIn && list_2[num] != AnimationDirection.InOutRepeatingOut && (list_2[num] != AnimationDirection.InOutOut || list_0[num] == 0.0) && (list_2[num] != AnimationDirection.Out || list_0[num] == 0.0))
					{
						num++;
						continue;
					}
					break;
				}
				timer_0.Stop();
				if (animationFinished_0 != null)
				{
					animationFinished_0(this);
				}
				break;
			}
		}

		public double GetProgress()
		{
			if (Singular)
			{
				if (list_0.Count == 0)
				{
					throw new Exception("Invalid animation");
				}
				return GetProgress(0);
			}
			throw new Exception("Animation is not set to Singular.");
		}

		public double GetProgress(int index)
		{
			if (index < GetAnimationCount())
			{
				return AnimationType switch
				{
					AnimationType.Linear => AnimationLinear.CalculateProgress(list_0[index]), 
					AnimationType.EaseInOut => AnimationEaseInOut.CalculateProgress(list_0[index]), 
					AnimationType.EaseOut => AnimationEaseOut.CalculateProgress(list_0[index]), 
					AnimationType.CustomQuadratic => AnimationCustomQuadratic.CalculateProgress(list_0[index]), 
					_ => throw new NotImplementedException("The given AnimationType is not implemented"), 
				};
			}
			throw new IndexOutOfRangeException("Invalid animation index");
		}

		public Point GetSource(int index)
		{
			if (index >= GetAnimationCount())
			{
				throw new IndexOutOfRangeException("Invalid animation index");
			}
			return list_1[index];
		}

		public Point GetSource()
		{
			if (Singular)
			{
				if (list_1.Count == 0)
				{
					throw new Exception("Invalid animation");
				}
				return list_1[0];
			}
			throw new Exception("Animation is not set to Singular.");
		}

		public AnimationDirection GetDirection()
		{
			if (Singular)
			{
				if (list_2.Count == 0)
				{
					throw new Exception("Invalid animation");
				}
				return list_2[0];
			}
			throw new Exception("Animation is not set to Singular.");
		}

		public AnimationDirection GetDirection(int index)
		{
			if (index >= list_2.Count)
			{
				throw new IndexOutOfRangeException("Invalid animation index");
			}
			return list_2[index];
		}

		public object[] GetData()
		{
			if (!Singular)
			{
				throw new Exception("Animation is not set to Singular.");
			}
			if (list_3.Count == 0)
			{
				throw new Exception("Invalid animation");
			}
			return list_3[0];
		}

		public object[] GetData(int index)
		{
			if (index >= list_3.Count)
			{
				throw new IndexOutOfRangeException("Invalid animation index");
			}
			return list_3[index];
		}

		public int GetAnimationCount()
		{
			return list_0.Count;
		}

		public void SetProgress(double progress)
		{
			if (!Singular)
			{
				throw new Exception("Animation is not set to Singular.");
			}
			if (list_0.Count == 0)
			{
				throw new Exception("Invalid animation");
			}
			list_0[0] = progress;
		}

		public void SetDirection(AnimationDirection direction)
		{
			if (Singular)
			{
				if (list_0.Count == 0)
				{
					throw new Exception("Invalid animation");
				}
				list_2[0] = direction;
				return;
			}
			throw new Exception("Animation is not set to Singular.");
		}

		public void SetData(object[] data)
		{
			if (Singular)
			{
				if (list_3.Count == 0)
				{
					throw new Exception("Invalid animation");
				}
				list_3[0] = data;
				return;
			}
			throw new Exception("Animation is not set to Singular.");
		}

		internal static bool z7ei9uICGMjOlnMayJmg()
		{
			return trHMNRICUVSUiDMJbBSw == null;
		}
	}
}

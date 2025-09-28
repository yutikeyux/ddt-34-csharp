using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace MetroFramework.Animation
{
	public abstract class AnimationBase
	{
		private EventHandler eventHandler_0;

		private DelayedCall delayedCall_0;

		private Control control_0;

		private AnimationAction animationAction_0;

		private AnimationFinishedEvaluator animationFinishedEvaluator_0;

		protected TransitionType transitionType;

		protected int counter;

		protected int startTime;

		protected int targetTime;

		private static AnimationBase jmU46GN9HPttSbUJljO;

		public bool IsCompleted
		{
			get
			{
				if (delayedCall_0 == null)
				{
					return true;
				}
				return !delayedCall_0.IsWaiting;
			}
		}

		public bool IsRunning
		{
			get
			{
				if (delayedCall_0 != null)
				{
					return delayedCall_0.IsWaiting;
				}
				return false;
			}
		}

		public event EventHandler AnimationCompleted
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				eventHandler_0 = (EventHandler)Delegate.Combine(eventHandler_0, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				eventHandler_0 = (EventHandler)Delegate.Remove(eventHandler_0, value);
			}
		}

		private void method_0()
		{
			if (eventHandler_0 != null)
			{
				eventHandler_0(this, EventArgs.Empty);
			}
		}

		public void Cancel()
		{
			if (IsRunning)
			{
				delayedCall_0.Cancel();
			}
		}

		protected void Start(Control control, TransitionType transitionType, int duration, AnimationAction actionHandler)
		{
			Start(control, transitionType, duration, actionHandler, null);
		}

		protected void Start(Control control, TransitionType transitionType, int duration, AnimationAction actionHandler, AnimationFinishedEvaluator evaluatorHandler)
		{
			control_0 = control;
			this.transitionType = transitionType;
			animationAction_0 = actionHandler;
			animationFinishedEvaluator_0 = evaluatorHandler;
			counter = 0;
			startTime = 0;
			targetTime = duration;
			delayedCall_0 = DelayedCall.Start(method_1, duration);
		}

		private void method_1()
		{
			if (animationFinishedEvaluator_0 != null && !animationFinishedEvaluator_0())
			{
				animationAction_0();
				counter++;
				delayedCall_0.Start();
			}
			else
			{
				method_0();
			}
		}

		protected int MakeTransition(float t, float b, float d, float c)
		{
			switch (transitionType)
			{
			default:
				return 0;
			case TransitionType.Linear:
				return (int)(c * t / d + b);
			case TransitionType.EaseInQuad:
				return (int)(c * (t /= d) * t + b);
			case TransitionType.EaseOutQuad:
				return (int)((0f - c) * (t /= d) * (t - 2f) + b);
			case TransitionType.EaseInOutQuad:
				if ((t /= d / 2f) < 1f)
				{
					return (int)(c / 2f * t * t + b);
				}
				return (int)((0f - c) / 2f * ((t -= 1f) * (t - 2f) - 1f) + b);
			case TransitionType.EaseInCubic:
				return (int)(c * (t /= d) * t * t + b);
			case TransitionType.EaseOutCubic:
				return (int)(c * ((t = t / d - 1f) * t * t + 1f) + b);
			case TransitionType.EaseInOutCubic:
				if ((t /= d / 2f) < 1f)
				{
					return (int)(c / 2f * t * t * t + b);
				}
				return (int)(c / 2f * ((t -= 2f) * t * t + 2f) + b);
			case TransitionType.EaseInQuart:
				return (int)(c * (t /= d) * t * t * t + b);
			case TransitionType.EaseInExpo:
				if (t == 0f)
				{
					return (int)b;
				}
				return (int)((double)c * Math.Pow(2.0, 10f * (t / d - 1f)) + (double)b);
			case TransitionType.EaseOutExpo:
				if (t == d)
				{
					return (int)(b + c);
				}
				return (int)((double)c * (0.0 - Math.Pow(2.0, -10f * t / d) + 1.0) + (double)b);
			}
		}

		internal static bool y0fNNsNZ4f3oc5MfRrW()
		{
			return jmU46GN9HPttSbUJljO == null;
		}

		internal static void irf4TvNEXOkMg8Yyptn()
		{
		}
	}
}

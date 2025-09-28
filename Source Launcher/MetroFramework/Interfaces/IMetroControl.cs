using System;
using MetroFramework.Components;
using MetroFramework.Drawing;

namespace MetroFramework.Interfaces
{
	public interface IMetroControl
	{
		MetroColorStyle Style { get; set; }

		MetroThemeStyle Theme { get; set; }

		MetroStyleManager StyleManager { get; set; }

		bool UseCustomBackColor { get; set; }

		bool UseCustomForeColor { get; set; }

		bool UseStyleColors { get; set; }

		bool UseSelectable { get; set; }

		event EventHandler<MetroPaintEventArgs> CustomPaintBackground;

		event EventHandler<MetroPaintEventArgs> CustomPaint;

		event EventHandler<MetroPaintEventArgs> CustomPaintForeground;
	}
}

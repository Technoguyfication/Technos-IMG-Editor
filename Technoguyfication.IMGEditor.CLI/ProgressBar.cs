using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technoguyfication.IMGEditor.CLI
{
	/// <summary>
	/// Displays a progress bar in the console
	/// </summary>
	public class ProgressBar : IDisposable
	{
		private char _barChar;
		private int _lastPercent = 0;

		private bool started = false;

		public ProgressBar(char barChar = '.')
		{
			_barChar = barChar;
		}

		~ProgressBar()
		{
			Dispose();
		}

		public void Dispose()
		{
			// do nothing because theres nothing to do
		}

		/// <summary>
		/// Steps the progress bar
		/// </summary>
		/// <param name="value"></param>
		public void SetPercent(int percent)
		{
			// print start bracket
			if (!started)
			{
				Console.Write("[");
				started = true;
			}

			if (percent <= _lastPercent)
				return;

			// print each percent out
			for (int i = _lastPercent + 1; i <= percent; i++)
			{
				PrintBar(i);
			}

			_lastPercent = percent;

			// print ending bracket
			if (percent == 100)
			{
				Console.WriteLine("]");
			}
		}

		/// <summary>
		/// Steps the percentage of the bar by a specified amount
		/// </summary>
		/// <param name="step"></param>
		public void Step(int step = 1)
		{
			SetPercent(_lastPercent + step);
		}

		/// <summary>
		/// Prints the bar for a percentage
		/// </summary>
		private void PrintBar(int percent)
		{
			// print percent
			if (percent % 10 == 0)
			{
				Console.Write($" {percent}% ");
				return;
			}

			// print dot
			if (percent % 2 == 0)
			{
				Console.Write(_barChar);
				return;
			}
		}

		/// <summary>
		/// Utility for getting the percentage of two values
		/// </summary>
		/// <param name="value"></param>
		/// <param name="maxValue"></param>
		public static int GetPercent(int value, int maxValue)
		{
			if (maxValue == 0)
				return 100;

			return (int)(((float)value / maxValue) * 100);
		}
	}
}

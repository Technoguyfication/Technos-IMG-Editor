using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technoguyfication.IMGEditor.Shared
{
	/// <summary>
	/// Simple class to be used with <see cref="IProgress{T}"/> for progress tracking.
	/// </summary>
	public class ProgressUpdate
	{
		public ProgressUpdateType Type { get; set; } = ProgressUpdateType.NUMERICAL;

		public int MaxValue { get; set; } = 100;
		public int Value { get; set; } = 0;

		public string Message { get; set; } = null;

		/// <summary>
		/// Returns an integer betwen 1 and 100, with the percent value of the progress update
		/// </summary>
		/// <returns></returns>
		public int GetPercent()
		{
			if (Type != ProgressUpdateType.NUMERICAL)
				throw new Exception("Not a numerical progress update type.");

			return (int)(((float)Value / MaxValue) * 100);
		}
	}

	/// <summary>
	/// Type of progress update, for use with <see cref="ProgressUpdate"/>
	/// </summary>
	public enum ProgressUpdateType
	{
		NUMERICAL,
		STRING
	}
}

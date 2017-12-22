using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Technoguyfication.IMGEditor.Version2;

namespace Technoguyfication.IMGEditor
{
	/// <summary>
	/// Provides utilities for opening IMG archives of varying formats
	/// </summary>
	public static class IMGOpener
	{
		/// <summary>
		/// Gets the correct <see cref="IIMGArchive"/> for an archive
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static IIMGArchive GetArchive(string filePath)
		{
			if (Ver2IMGArchive.IsValidArchive(filePath))
				return new Ver2IMGArchive(filePath);
			else
				throw new InvalidArchiveException("File did not match any known file types.");
		}

		/// <summary>
		/// Returns whether or not the specified file is a valid IMG archive
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static bool IsValidArchive(string filePath)
		{
			return Ver2IMGArchive.IsValidArchive(filePath);
		}

		/// <summary>
		/// Gets the user-friendly name of an Archive type
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static string GetFriendlyName(Type type)
		{
			if (type == typeof(Ver2IMGArchive))
				return "Version 2 (San Andreas)";
			else
				return "Unknown";
		}
	}
}

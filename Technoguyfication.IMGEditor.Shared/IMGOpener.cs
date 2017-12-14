﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Technoguyfication.IMGEditor.Shared
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
				throw new InvalidArchiveFormatException("File did not match any known file types.");
		}

		/// <summary>
		/// Returns whether or not the specified file is a valid IMG archive
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static bool IsValidArchive(string filePath)
		{
			try
			{
				GetArchive(filePath);
			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}
	}
}

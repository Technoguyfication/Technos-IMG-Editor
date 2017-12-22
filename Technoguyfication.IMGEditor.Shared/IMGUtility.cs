using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Technoguyfication.IMGEditor.Version2;

namespace Technoguyfication.IMGEditor
{
	/// <summary>
	/// Provides utilities for common archive operations
	/// </summary>
	public class IMGUtility
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

		/// <summary>
		/// Extract a file from an archive to the file system
		/// </summary>
		/// <param name="archive"></param>
		/// <param name="fileName"></param>
		/// <param name="outputDirectory"></param>
		/// <param name="outputFolder"></param>
		/// <param name="overwrite"></param>
		public static void Extract(IIMGArchive archive, string fileName, string outputDirectory, bool overwrite = false)
		{
			// check if file exists
			if (File.Exists(outputDirectory) && !overwrite)
				throw new ArgumentException("File already exists.");

			// get paths
			Directory.CreateDirectory(outputDirectory);
			string outputPath = Path.Combine(outputDirectory, fileName);

			// open file streams
			var outputStream = File.Create(outputPath);
			outputStream.SetLength(0);
			var inputStream = archive.OpenFile(fileName);

			// write data to file
			byte[] buffer = new byte[inputStream.Length];

			inputStream.Read(buffer, 0, buffer.Length);
			outputStream.Write(buffer, 0, buffer.Length);

			// finish
			outputStream.Flush();
		}
	}
}

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
				throw new InvalidArchiveException("Unknown archive type. It may be corrupted.");
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
		/// Extracts a file from an archive to a location on the disk, providing the option to overwrite existing files.
		/// </summary>
		/// <param name="archive"></param>
		/// <param name="fileName"></param>
		/// <param name="outputDirectory"></param>
		/// <param name="overwrite"></param>
		/// <param name="outputFileName">The name of the output file</param>
		public static void ExtractFile(IIMGArchive archive, string fileName, string outputDirectory, bool overwrite, string outputFileName = null)
		{
			// get paths
			Directory.CreateDirectory(outputDirectory);
			string outputPath = Path.Combine(outputDirectory, outputFileName ?? fileName);

			// check if file exists
			if (File.Exists(outputPath) && !overwrite)
				throw new IOException("The file specified already exists.");

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

		/// <summary>
		/// Extracts a file from an archive to a location on the disk.
		/// Will throw an <see cref="IOException"/> if the file already exists.
		/// </summary>
		/// <param name="archive"></param>
		/// <param name="fileName"></param>
		/// <param name="outputDirectory"></param>
		/// <param name="outputFileName"></param>
		/// <exception cref="IOException"></exception>
		public static void ExtractFile(IIMGArchive archive, string fileName, string outputDirectory, string outputFileName = null)
		{
			ExtractFile(archive, fileName, outputDirectory, false, outputFileName);
		}

		/// <summary>
		/// Adds a file to the archive, with the option of overwriting existing files.
		/// </summary>
		/// <param name="archive"></param>
		/// <param name="filePath"></param>
		/// <param name="overwrite"></param>
		/// <param name="newName"></param>
		/// <exception cref="ArgumentException">Thrown if the file is invalid or doesn't exist.</exception>
		/// <exception cref="FileNotFoundException">Thrown if the input file does not exist</exception>
		/// <exception cref="IOException">Thrown if the file cannot be accesed</exception>
		public static void AddFile(IIMGArchive archive, string filePath, bool overwrite, string newName = null)
		{
			using (Stream fileStream = File.OpenRead(filePath))
			{
				archive.AddFile(newName ?? Path.GetFileName(filePath), fileStream, (uint)fileStream.Length, 0);
			}
		}

		/// <summary>
		/// Adds a file to the archive.
		/// Will throw an <see cref="ArgumentException"/> if the file already exists.
		/// </summary>
		/// <param name="archive"></param>
		/// <param name="filePath"></param>
		/// <param name="newName"></param>
		/// <exception cref="ArgumentException">Thrown if the file is invalid or doesn't exist.</exception>
		/// <exception cref="FileNotFoundException">Thrown if the input file does not exist</exception>
		/// <exception cref="IOException">Thrown if the file cannot be accesed</exception>
		public static void AddFile(IIMGArchive archive, string filePath, string newName = null)
		{
			AddFile(archive, filePath, false, newName);
		}
	}
}

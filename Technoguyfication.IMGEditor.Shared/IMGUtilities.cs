using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technoguyfication.IMGEditor
{
	/// <summary>
	/// Provides utilities for common archive operations
	/// </summary>
	public class IMGUtilities
	{
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

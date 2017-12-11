using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using Technoguyfication.IMGEditor.Shared;

namespace Technoguyfication.IMGEditor.CLI
{
	class Program
	{
		/// <summary>
		/// Gets the assembly name e.g. "program.exe"
		/// </summary>
		public static string AssemblyName
		{
			get
			{
				return AppDomain.CurrentDomain.FriendlyName;
			}
		}

		static void Main(string[] args)
		{
			if (args.Length < 1)
			{
				PrintHelp();

				if (StartedFromExplorer())
				{
					Console.ReadKey(true);  // pause
				}

				return;
			}

			if (!ParseArgs(args))
			{
				Console.WriteLine($"Error in command syntax or invalid command specified. Run {AssemblyName} with no arguments for help.");
			}
		}

		/// <summary>
		/// Attempts to parse arguments and run the associated action
		/// </summary>
		/// <param name="args"></param>
		/// <returns>Whether the args were parsed successfully.</returns>
		private static bool ParseArgs(string[] args)
		{
			switch (args[0].ToLower())
			{
				case "bump":
					{
						if (args.Length < 2)
							return false;

						int amount = 1;

						// attempt to parse bump amount
						if (args.Length > 2)
							if (!int.TryParse(args[2], out amount))
								return false;

						// open file
						IMGFileVer2 file = AttemptToOpenImgFile(args[1]);
						if (file == null)
							return true;

						string filePath = args[1];

						// perform the edit
						try
						{
							file.Bump(amount);
						}
						catch (Exception ex)
						{
							Console.WriteLine($"Unhandled exception occured while editing the file. It may be corrupted.\n{ex}");
							return true;
						}

						// end
						Console.WriteLine($"Successfully bumped {amount} entries from beginning of {Path.GetFileName(filePath)} to the end, freeing up {amount * IMGFileVer2.SECTOR_SIZE} bytes for directory entries.");
						return true;
					}
				case "add":
					{
						// need atleast 3 args
						if (args.Length < 3)
							return false;

						string archivePath = args[1];
						string filePath = args[2];
						string archiveFileName;

						// find an archive file name
						if (args.Length < 3)
						{
							int byteCount = Encoding.ASCII.GetByteCount(args[3]);
							if (byteCount > IMGFileVer2.MAX_DIRECTORY_FILE_NAME)	// user-entered name too long
							{
								Console.WriteLine($"File name cannot be larger than 32 bytes. The name you have entered (\"{args[3]}\") has a byte count of {byteCount}");
								return true;
							}

							archiveFileName = args[3];
						}
						else
						{
							string fileName = Path.GetFileName(filePath);

							if (Encoding.ASCII.GetByteCount(fileName) > IMGFileVer2.MAX_DIRECTORY_FILE_NAME)
							{
								Console.WriteLine("The file name of the file you are adding is longer than the maximum allowed size. Consider specifying a name instead.");
								return true;
							}

							archiveFileName = fileName;
						}

						// load img file
						IMGFileVer2 file = AttemptToOpenImgFile(archivePath);
						if (file == null)
							return true;

						// load file into buffer
						FileStream fileStream = File.OpenRead(filePath);
						byte[] buffer = new byte[fileStream.Length];

						int bytesRead = 0;
						while (bytesRead < buffer.Length)
						{
							bytesRead += fileStream.Read(buffer, bytesRead, buffer.Length - bytesRead);
						}

						file.AddFile(archiveFileName, buffer);

						Console.WriteLine($"Added file {archiveFileName} ({buffer.Length} bytes) to {Path.GetFileName(archivePath)}");
						return true;
					}
				case "extract":
					{
						if (args.Length < 3)
							return false;

						string archivePath = args[1];
						string outputPath = args[2];

						// open img file
						IMGFileVer2 imgfile = AttemptToOpenImgFile(archivePath);
						if (imgfile == null)
							return true;

						try
						{
							Directory.CreateDirectory(outputPath);
						}
						catch (PathTooLongException)
						{
							Console.WriteLine("The output path you specified was too long.");
							return true;
						}
						catch (Exception ex)
						{
							Console.WriteLine($"Unhandled exception creating output directory:\n{ex}");
							return true;
						}

						var files = imgfile.GetDirectoryEntries();

						foreach (var file in files)
						{
							var stream = File.Create(Path.Combine(outputPath, file.Name));

							byte[] buffer = imgfile.GetFile(file);
							stream.Write(buffer, 0, buffer.Length);
						}

						Console.WriteLine($"Extracted {imgfile.FileCount} files from archive.");
						return true;
					}
				case "info":
					{
						if (args.Length < 2)
							return false;

						string imgFilePath = args[1];

						IMGFileVer2 imgFile = AttemptToOpenImgFile(imgFilePath);
						if (imgFile == null)
							return true;

						float filesizeMB = imgFile.FileInfo.Length / 1E6f;

						Console.WriteLine($"{Path.GetFileName(imgFilePath)}:\n" +
							$"  File Count: {imgFile.FileCount}\n" +
							$"  Size: {filesizeMB.ToString("0.00")}MB");

						return true;
					}
			}

			return false;
		}

		/// <summary>
		/// Prints the help section to the console
		/// </summary>
		private static void PrintHelp()
		{
			var builder = new StringBuilder($"Techno's IMG Editor (v{Assembly.GetExecutingAssembly().GetName().Version.ToString()}) for Grand Theft Auto III/VC/SA/IV\n\n");

			builder.AppendLine("Drag n' drop an IMG archive to extract it.");
			builder.AppendLine("Drag n' drop a folder to turn it into an IMG archive.");

			// extract
			builder.AppendLine("\nExtract all files from an archive:");
			builder.AppendLine($"  > {AssemblyName} extract (IMG file path) (output path)");

			// add
			builder.AppendLine("\nAdd a file to an archive:");
			builder.AppendLine($" > {AssemblyName} add (IMG file path) (file to add) [file name (default is original file name)]");

			// info
			builder.AppendLine("\nGet info on an archive:");
			builder.AppendLine($" > {AssemblyName} info (IMG file path)");

			builder.AppendLine("\nAdvanced commands:");

			// bump entries
			builder.AppendLine("\nMove file entries from the top to the bottom of an IMG archive for making more directory space:");
			builder.AppendLine($"  > {AssemblyName} bump (IMG file path) [amount of entries (default 1)]");

			Console.WriteLine(builder);
		}

		/// <summary>
		/// Attempts to open an IMG file, displaying any errors to the user
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns>IMGFileVer2, or null on error</returns>
		private static IMGFileVer2 AttemptToOpenImgFile(string filePath)
		{
			try
			{
				return new IMGFileVer2(filePath);
			}
			catch (FileNotFoundException)
			{
				Console.WriteLine($"File \"{filePath}\" not found.");
				return null;
			}
			catch (IOException ex)
			{
				Console.WriteLine($"Error opening file:\n{ex.Message}");
				return null;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Unhandled exception opening file: {ex}");
				return null;
			}
		}

		/// <summary>
		/// Gets whether the application was started from windows explorer or not
		/// </summary>
		/// <returns></returns>
		private static bool StartedFromExplorer()
		{
			// https://stackoverflow.com/questions/3527555/how-can-you-determine-how-a-console-application-was-launched/18307640#18307640
			return
				!Console.IsOutputRedirected
				&& !Console.IsInputRedirected
				&& !Console.IsErrorRedirected
				&& Environment.UserInteractive
				&& Environment.CurrentDirectory == Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)
				&& Environment.GetCommandLineArgs()[0] == Assembly.GetEntryAssembly().Location;

		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using Technoguyfication.IMGEditor;
using System.Diagnostics;
using Technoguyfication.IMGEditor.Version2;

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
							break; ;

						int amount = 1;

						// attempt to parse bump amount
						if (args.Length > 2)
							if (!int.TryParse(args[2], out amount))
								break;

						// open file
						var archive = AttemptToOpenArchive(args[1]);
						if (archive == null)
							return true;

						string filePath = args[1];

						// perform the edit
						try
						{
							archive.Bump(amount);
						}
						catch (Exception ex)
						{
							Console.WriteLine($"Unhandled exception occured while editing the file. It may be corrupted.\n{ex}");
							return true;
						}

						// end
						Console.WriteLine($"Successfully bumped {amount} entries from beginning of {Path.GetFileName(filePath)} to the end, freeing up {amount * Ver2IMGArchive.SECTOR_SIZE} bytes for directory entries.");
						return true;
					}
				case "add":
					{
						// need atleast 3 args
						if (args.Length < 3)
							break;

						string archivePath = args[1];
						string filePath = args[2];
						string archiveFileName;

						// find an archive file name
						if (args.Length < 3)
						{
							int byteCount = Encoding.ASCII.GetByteCount(args[3]);
							if (byteCount > Ver2IMGArchive.MAX_DIRECTORY_FILE_NAME)    // user-entered name too long
							{
								Console.WriteLine($"File name cannot be larger than 32 bytes. The name you have entered (\"{args[3]}\") has a byte count of {byteCount}");
								return true;
							}

							archiveFileName = args[3];
						}
						else
						{
							string fileName = Path.GetFileName(filePath);

							if (Encoding.ASCII.GetByteCount(fileName) > Ver2IMGArchive.MAX_DIRECTORY_FILE_NAME)
							{
								Console.WriteLine("The file name of the file you are adding is longer than the maximum allowed size. Consider specifying a name instead.");
								return true;
							}

							archiveFileName = fileName;
						}

						// load img file
						var archive = AttemptToOpenArchive(archivePath);
						if (archive == null)
							return true;

						// load file into buffer
						FileStream fileStream = File.OpenRead(filePath);
						archive.AddFile(archiveFileName, fileStream, (uint)fileStream.Length, 0);

						Console.WriteLine($"Added file {archiveFileName} ({fileStream.Length} bytes) to {Path.GetFileName(archivePath)}");
						return true;
					}
				case "extractall":
					{
						if (args.Length < 3)
							break;

						string archivePath = args[1];
						string outputPath = args[2];

						// open img file
						var archive = AttemptToOpenArchive(archivePath);
						if (archive == null)
							return true;

						// check if we're overwriting the files
						if (Directory.Exists(outputPath))
						{
							Console.WriteLine("The output directory you specified already exists. Type \"yes\" to merge directories, or anything else to abort.");
							if (Console.ReadLine().Trim().ToLower() != "yes")
								return true;
						}

						// get data stream
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

						var files = archive.GetDirectoryEntries();

						bool overwriteAll = false;
						int numCopied = 0;

						// copy each file out
						foreach (var file in files)
						{
							string newFilePath = Path.Combine(outputPath, file.Name);

							// check for conflicts
							if (!overwriteAll && File.Exists(newFilePath))
							{
								Console.WriteLine($"File \"{file.Name}\" already exists. Type \"yes\" to overwrite it, \"all\" to overwrite all, or anything else to skip.");

								string response = Console.ReadLine().Trim().ToLower();
								if (response == "all")
									overwriteAll = true;
								else if (response != "yes")
									continue;

								File.Delete(newFilePath);
							}

							// create streams
							var outStream = File.Create(newFilePath);
							var inStream = archive.OpenFile(file.Name);

							// copy data to new file
							byte[] buffer = new byte[inStream.Length];
							inStream.Read(buffer, 0, (int)inStream.Length);
							outStream.Write(buffer, 0, buffer.Length);

							outStream.Flush();
							outStream.Dispose();

							numCopied++;
						}

						// finish
						Console.WriteLine($"Extracted {numCopied} files from archive.");
						return true;
					}
				case "extract":
					{
						if (args.Length < 3)
							break;

						// get file paths
						string outputFolder;
						if (args.Length < 4)
							outputFolder = Environment.CurrentDirectory;
						else
							outputFolder = args[3];

						string archivePath = args[1];
						string internalFileName = args[2];


						// load archive
						var archive = AttemptToOpenArchive(archivePath);
						if (archive == null)
							return true;

						Stream inputStream;

						// get stream for file
						try
						{
							inputStream = archive.OpenFile(internalFileName);
						}
						catch (InvalidDirectoryEntryException)
						{
							Console.WriteLine($"File \"{internalFileName}\" does not exist inside archive.");
							return true;
						}

						string outputFile = Path.Combine(outputFolder, internalFileName);

						// check if file exists
						if (File.Exists(outputFile))
						{
							Console.WriteLine("File already exists. Type \"yes\" to overwrite, or anything else to abort.");
							if (Console.ReadLine().Trim().ToLower() != "yes")
								return true;

							File.Delete(outputFile);
						}

						// create output file
						Directory.CreateDirectory(outputFolder);
						var outputStream = File.Create(outputFile);

						// stream data into output file
						byte[] buffer = new byte[inputStream.Length];

						inputStream.Read(buffer, 0, buffer.Length);
						outputStream.Write(buffer, 0, buffer.Length);

						// finish
						outputStream.Flush();
						Console.WriteLine($"Copied \"{internalFileName}\" to \"{outputFolder}\"");

						return true;
					}
				case "info":
					{
						if (args.Length < 2)
							break;

						string archivePath = args[1];

						var archive = AttemptToOpenArchive(archivePath);
						if (archive == null)
							return true;

						float filesizeMB = archive.FileInfo.Length / 1E6f;

						Console.WriteLine($"{Path.GetFileName(archivePath)}:\n" +
							$"  File Count: {archive.FileCount}\n" +
							$"  Size: {filesizeMB.ToString("0.00")}MB");

						return true;
					}
				case "defrag":
					{
						if (args.Length < 2)
							break;

						// open archive
						string imgFilePath = args[1];
						var archive = AttemptToOpenArchive(imgFilePath);
						if (archive == null)
							return true;

						// confirm that the user wants to defragment
						Console.Write("Warning: Defragmenting may take a very long time to complete, with minimal benefits. Please only proceed if you have good reason.\n\n" +
							"Type \"yes\" to continue, or anything else to abort.\n" +
							" >");

						string response = Console.ReadLine().Trim().ToLower();
						if (response != "yes")
						{
							Console.WriteLine("Aborting.");
							return true;
						}

						Console.Write("\n[ ");

						var watch = Stopwatch.StartNew();
						ProgressBar bar = new ProgressBar();

						// defragment the file
						archive.Defragment(new Progress<ProgressUpdate>((progress) =>
						{
							bar.SetPercent(progress.GetPercent());
						}));

						watch.Stop();

						Console.WriteLine($"]\n\nFinished defragmenting. Took {watch.Elapsed.ToString()}.");

						return true;
					}
			}

			if (args.Length < 1)
				return false;

			// try dragging and dropping
			return DragNDrop(args[0]);
		}

		/// <summary>
		/// Runs drag 'n drop for the specified file
		/// </summary>
		/// <param name="filePath">Whether anything was done</param>
		private static bool DragNDrop(string filePath)
		{
			// check if file is img archive
			if (IMGOpener.IsValidArchive(filePath))
			{
				var archive = AttemptToOpenArchive(filePath);
				if (archive == null)
					return true;

				string outputDir = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath));
				var bar = new ProgressBar();
				var entries = archive.GetDirectoryEntries();

				Console.WriteLine($"Extracting \"{Path.GetFileName(filePath)}\" to \"{outputDir}\"");

				// extract each file
				for (int i = 0; i < entries.Count; i++)
				{
					IMGUtilities.Extract(archive, entries[i].Name, outputDir, true);

					bar.SetPercent(ProgressBar.GetPercent(i, entries.Count - 1));
				}

				// done
				Console.WriteLine($"Extracted {entries.Count} files from archive to \"{outputDir}\"");
				return true;
			}

			return false;
		}

		/// <summary>
		/// Prints the help section to the console
		/// </summary>
		private static void PrintHelp()
		{
			var builder = new StringBuilder($"Techno's IMG Editor (v{Assembly.GetExecutingAssembly().GetName().Version.ToString()}) for Grand Theft Auto III/VC/SA/IV\n\n");

			builder.AppendLine("Drag 'n drop an IMG archive to extract it.");

			// extract all
			builder.AppendLine("\nExtract all files from an archive:");
			builder.AppendLine($"  > {AssemblyName} extractall (IMG file path) (output path)");

			// extract
			builder.AppendLine("\nExtract a single file from an archive:");
			builder.AppendLine($"  > {AssemblyName} extract (IMG file path) (file name) [output folder (default is working directory)]");

			// add
			builder.AppendLine("\nAdd a file to an archive:");
			builder.AppendLine($" > {AssemblyName} add (IMG file path) (file to add) [file name (default is original file name)]");

			// info
			builder.AppendLine("\nGet info on an archive:");
			builder.AppendLine($" > {AssemblyName} info (IMG file path)");

			builder.AppendLine("\nAdvanced commands:");

			// defrag
			builder.AppendLine("\nDefragment an IMG archive:");
			builder.AppendLine($" > {AssemblyName} defrag (IMG file path)");

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
		private static IIMGArchive AttemptToOpenArchive(string filePath)
		{
			try
			{
				return IMGOpener.GetArchive(filePath);
			}
			catch (FileNotFoundException)
			{
				Console.WriteLine($"File \"{filePath}\" not found.");
				return null;
			}
			catch (IOException ex)
			{
				Console.WriteLine($"Error opening file:\n{ex}");
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

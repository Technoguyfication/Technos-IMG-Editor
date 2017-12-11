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
					int amount = 1;

					// attempt to parse bump amount
					if (args.Length > 2)
						if (!int.TryParse(args[2], out amount))
							return false;

					// open file
					IMGFileVer2 file;
					string filePath = args[1];
					try
					{
						file = new IMGFileVer2(filePath);
					}
					catch (FileNotFoundException)
					{
						Console.WriteLine($"File \"{filePath}\" not found.");
						return true;
					}
					catch (IOException ex)
					{
						Console.WriteLine($"Error opening file:\n{ex.Message}");
						return true;
					}
					catch (Exception ex)
					{
						Console.WriteLine($"Unhandled exception opening file: {ex}");
						return true;
					}

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

			return false;
		}

		/// <summary>
		/// Prints the help section to the console
		/// </summary>
		private static void PrintHelp()
		{
			var builder = new StringBuilder($"Techno's IMG Editor (v{Assembly.GetExecutingAssembly().GetName().Version.ToString()}) for Grand Theft Auto III/VC/SA/IV\n\n");

			// extract
			builder.AppendLine("Extract all files from an IMG archive:");
			builder.AppendLine($"  > {AssemblyName} extract (IMG file path) (output path)");

			builder.AppendLine("\nAdvanced commands:");

			// bump entries
			builder.AppendLine("\nMove X file entries from top to bottom of IMG archive: (for making more directory space)");
			builder.AppendLine($"  > {AssemblyName} bump (IMG file path) [amount of entries (default 1)]");

			Console.WriteLine(builder);
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

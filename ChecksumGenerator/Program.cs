using System.Security.Cryptography;
using System.Text;

namespace ChecksumManager
{
    /// <summary>
    /// Basic checksum generator that computes the checksum of all files in a folder and saves them to a file.
    /// </summary>
    class ChecksumGenerator
    {
        static void Main(string[] args)
        {
            try
            {
                Execute(args);
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {ex.Message}");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        static void Execute(string[] args)
        {
            if (args.Length != 3)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Usage: ChecksumGenerator <ROOT_FOLDER> <OUTPUT_FOLDER> <CHECKSUM_METHOD>");
                Console.WriteLine("Available Methods: MD5 SHA1 SHA256 SHA512");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            var rootFolder = args[0];
            var outputFolder = args[1];
            var checksumMethod = args[2];

            if (!Directory.Exists(rootFolder))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: The folder '{rootFolder}' does not exist.");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            if (!Directory.Exists(outputFolder))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: The folder '{rootFolder}' does not exist.");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            var files = Directory.GetFiles(rootFolder, "*", SearchOption.AllDirectories);
            var outputFilePath = Path.Combine(outputFolder, $"checksum{checksumMethod}.txt");

            if (File.Exists(outputFilePath))
                File.Delete(outputFilePath);

            using (var writer = new StreamWriter(outputFilePath))
            {
                foreach (var file in files)
                {
                    try
                    {
                        var checksum = ComputeChecksum(file, checksumMethod);
                        var relativePath = Path.GetRelativePath(rootFolder, file);
                        writer.WriteLine($"\"{relativePath}\" {checksum}");
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Error computing checksum for {file}: {ex.Message}");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Checksums saved to {outputFilePath}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        static string ComputeChecksum(string filePath, string checksumMethod)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                HashAlgorithm algorithm;

                switch (checksumMethod)
                {
                    case "MD5":
                        algorithm = MD5.Create();
                        break;
                    case "SHA256":
                        algorithm = SHA256.Create();
                        break;
                    case "SHA512":
                        algorithm = SHA512.Create();
                        break;
                    case "SHA1":
                        algorithm = SHA1.Create();
                        break;
                    default:
                        throw new InvalidOperationException($"Unsupported checksum method {checksumMethod}");
                }

                var checksumBytes = algorithm.ComputeHash(fs);

                var sb = new StringBuilder();
                foreach (var b in checksumBytes)
                    sb.Append(b.ToString("x2"));

                return sb.ToString();
            }
        }
    }
}
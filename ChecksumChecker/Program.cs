using System.Security.Cryptography;
using System.Text;

namespace ChecksumManager
{
    /// <summary>
    /// Basic checksum checker that checks files within input folder against a checksum file.
    /// </summary>
    class ChecksumChecker
    {
        static void Main(string[] args)
        {
            try
            {
                Execute(args);
            }
            catch (Exception ex)
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
                Console.WriteLine("Usage: ChecksumChecker <ROOT_FOLDER> <CHECKSUM_FILE> <METHOD>");
                Console.WriteLine("Available Methods: MD5 SHA1 SHA256 SHA512");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            var rootFolder = args[0];
            var checksumFile = args[1];
            var checksumMethod = args[2];

            if (!Directory.Exists(rootFolder))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: The folder '{rootFolder}' does not exist.");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            if (!File.Exists(checksumFile))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: The file '{checksumFile}' does not exist.");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            var checksums = ParseFile(checksumFile);
            var files = Directory.GetFiles(rootFolder, "*", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                try
                {
                    var checksum = ComputeChecksum(file, checksumMethod);
                    var relativePath = Path.GetRelativePath(rootFolder, file);
                    
                    if(!checksums.ContainsKey(relativePath))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"WARNING: Cannot find file {relativePath} for the verification");
                        Console.ForegroundColor = ConsoleColor.White;

                        continue;
                    }

                    if(checksums[relativePath] != checksum)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Checksum mismatch for {relativePath}");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error computing checksum for {file}: {ex.Message}");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Verification completed");
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

        static Dictionary<string, string> ParseFile(string filePath)
        {
            var fileChecksums = new Dictionary<string, string>();

            foreach (var line in File.ReadLines(filePath))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 2)
                {
                    var filePathKey = parts[0].Trim('"');
                    var checksum = parts[1];
                    fileChecksums[filePathKey] = checksum;
                }
            }
          
            return fileChecksums;
        }
    }
}
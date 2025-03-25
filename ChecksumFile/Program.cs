using System.Security.Cryptography;
using System.Text;

namespace ChecksumManager
{
    /// <summary>
    /// Basic checksum generator for a single file.
    /// </summary>
    class ChecksumFile
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
            if (args.Length != 1)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Usage: ChecksumFile <FILE>");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            var file = args[0];

            if (!File.Exists(file))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: The file '{file}' does not exist.");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            try
            {
                var checksumMethods = new[] { "MD5", "SHA1", "SHA256", "SHA512" };

                foreach (var checksumMethod in checksumMethods)
                {
                    var checksum = ComputeChecksum(file, checksumMethod);
                    Console.WriteLine($"{checksumMethod} Checksum: {checksum}");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error computing checksum for {file}: {ex.Message}");
                Console.ForegroundColor = ConsoleColor.White;
            }
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

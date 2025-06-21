using Algorithm_assignment;
using burrows;
using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Linq;

class Program
{
    private static void Compress(string filePath)
    {
        string CompressedFileName = Path.GetFileNameWithoutExtension(filePath) + "Compressed.bin";
        string TransfromFileName = Path.GetFileNameWithoutExtension(filePath) + "_Transform.txt";
        string EncodeFileName = Path.GetFileNameWithoutExtension(filePath) + "_Encode.txt";
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        byte[] inputText = File.ReadAllBytes(filePath);
        byte[] bwt = Transform.Transform_Text(inputText);
        byte[] encode = MoveToFront.Encode(inputText);
        HuffmanCoding huffmanCoding = new HuffmanCoding();
        byte[] compress = huffmanCoding.Compress(encode);
        File.WriteAllBytes(CompressedFileName, compress);
        stopwatch.Stop();
        TimeSpan elapsedTime = stopwatch.Elapsed;
        Console.WriteLine("Total Compression Execution Time For File " + filePath + ": " + elapsedTime.TotalSeconds);
        File.WriteAllBytes(TransfromFileName, bwt);
        File.WriteAllBytes(EncodeFileName, encode);
    }



    private static void Decompress(string filePath)
    {

        string DecompressedFileName = Path.GetFileNameWithoutExtension(filePath) + "Decompressed.txt";
        string InverseFileName = Path.GetFileNameWithoutExtension(filePath) + "_Inverse.txt";
        string DecodeFileName = Path.GetFileNameWithoutExtension(filePath) + "_Decode.txt";
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        HuffmanCoding huffmanCoding = new HuffmanCoding();
        byte[] inputText = File.ReadAllBytes(filePath);
        byte[] decompressed = huffmanCoding.DCompress(inputText);
        byte[] decode = MoveToFront.Decode(decompressed);
        byte[] inverse = Inverse.InverseTransform(decode);
        File.WriteAllBytes(InverseFileName, decode);
        stopwatch.Stop();
        TimeSpan elapsedTime = stopwatch.Elapsed;
        Console.WriteLine("Total Compression Execution Time For File " + filePath + ": " + elapsedTime.TotalSeconds);
        File.WriteAllBytes(DecodeFileName, decode);
        File.WriteAllBytes(DecompressedFileName, inverse);
    }



    public static void ProcessFilesInFolder(string folderPath)
    {
        // Check if the directory exists
        if (Directory.Exists(folderPath))
        {
            // Get all text file paths in the folder
            string[] fileEntries = Directory.GetFiles(folderPath, "*.txt");

            // Iterate through each file path
            foreach (string filePath in fileEntries)
            {
                // Call your test code function here
                // You would read the text file content and pass it to your function
                Compress(filePath);
                Decompress(filePath);
            }
        }
        else
        {
            Console.WriteLine("Directory does not exist.");
        }
    }

    public static void SaveResultToFile(string folderPath, string filePath, byte[] result)
    {
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        string NewfilePath = Path.Combine(folderPath, filePath);
        File.WriteAllBytes(NewfilePath, result);
    }



    static void Main()
    {
        bool continueRunning = true;

        while (continueRunning)
        {
            Console.Write("\nEnter your choice: [1] Sample Test Cases [2] Small Test Cases [3] Medium Test Cases [4] Large Test Cases... [any other key to exit] ");
            ConsoleKeyInfo cki = Console.ReadKey();
            Console.WriteLine();

            switch (cki.Key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    // Code for Sample Test Cases
                    Console.WriteLine("You selected Sample Test Cases.");
                    //ProcessFilesInFolder("Sample Cases");
                    Compress("aaaaaaaaaa.txt");
                    Decompress("aaaaaaaaaaCompressed.bin");
                    Compress("abbbaabbbbaccabbaaabc.txt");
                    Decompress("abbbaabbbbaccabbaaabcCompressed.bin");
                    Compress("abra.txt");
                    Decompress("abraCompressed.bin");
                    Compress("LectureExample.txt");
                    Decompress("LectureExampleCompressed.bin");
                    Compress("nomatch.txt");
                    Decompress("nomatchCompressed.bin");
                    break;

                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    // Code for Small Test Cases
                    Console.WriteLine("You selected Small Test Cases.");
                    Compress("aesop-4copies.txt");
                    Decompress("aesop-4copiesCompressed.bin");
                    Compress("aesop.txt");
                    Decompress("aesopCompressed.bin");
                    //ProcessFilesInFolder("Small");
                    //Decompress("Compressed.txt");
                    break;

                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    // Code for Medium Test Cases
                    Console.WriteLine("You selected Medium Test Cases.");
                    Compress("world192.txt");
                    Decompress("world192Compressed.bin");
                    Compress("chromosome11-human.txt");
                    Decompress("chromosome11-humanCompressed.bin");
                    //ProcessFilesInFolder("Medium");
                    break;

                case ConsoleKey.D4:
                case ConsoleKey.NumPad4:
                    // Code for Large Test Cases
                    Console.WriteLine("You selected Large Test Cases.");
                    Compress("dickens.txt");
                    Decompress("dickensCompressed.bin");
                    Compress("pi-10million.txt");
                    Decompress("pi-10millionCompressed.bin");
                    //ProcessFilesInFolder("Large");
                    break;

                default:
                    // Code for exit
                    Console.WriteLine("Exiting...");
                    continueRunning = false; // Set the flag to false to exit the loop
                    break;
            }
        }

    }
}


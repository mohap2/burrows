using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Algorithm_assignment;
using static System.Net.Mime.MediaTypeNames;


namespace burrows
{
    public class Transform
    {

        public static byte[] Transform_Text(byte[] inputBytes)
        {
            // Append a unique EOF character to the input bytes
            byte eof = 0; // Assuming 0 is not present in the inputBytes and can be used as the EOF character
            byte[] inputBytesWithEOF = new byte[inputBytes.Length + 1];
            Array.Copy(inputBytes, inputBytesWithEOF, inputBytes.Length);
            inputBytesWithEOF[inputBytes.Length] = eof; // Append EOF character at the end

            // Convert byte array to short array
            short[] shortString = new short[inputBytesWithEOF.Length];
            for (int i = 0; i < inputBytesWithEOF.Length; i++)
                shortString[i] = BitConverter.ToInt16(new byte[] { inputBytesWithEOF[i], 0 }, 0);

            // Assuming SuffixArray.Construct can take a short array
            int[] suffixArray = SuffixArray.Construct(shortString);

            // Create a byte array for the BWT result
            byte[] bwtArray = new byte[inputBytesWithEOF.Length];

            for (int i = 0; i < suffixArray.Length; i++)
            {
                // Compute the BWT index
                int bwtIndex = (suffixArray[i] - 1 + inputBytesWithEOF.Length) % inputBytesWithEOF.Length;
                // Assign the corresponding byte to the BWT array
                bwtArray[i] = inputBytesWithEOF[bwtIndex];
            }

            return bwtArray;
        }


    }
}

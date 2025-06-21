using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace burrows
{
    public class Inverse
    {

        public static byte[] InverseTransform(byte[] bwtBytes)
        {
            Console.WriteLine("inverse called\n");
            int len = bwtBytes.Length;

            // Count occurrences of each byte
            Dictionary<byte, int> count = new Dictionary<byte, int>();
            foreach (byte b in bwtBytes)
            {
                if (!count.ContainsKey(b))
                {
                    count[b] = 0;
                }
                count[b]++;
            }

            // Build the first column
            byte[] firstColumn = new byte[len];
            int index = 0;
            foreach (var entry in count.OrderBy(x => x.Key))
            {
                for (int i = 0; i < entry.Value; i++)
                {
                    firstColumn[index++] = entry.Key;
                }
            }

            // Determine the next array
            Dictionary<byte, Queue<int>> nextIndex = new Dictionary<byte, Queue<int>>();
            for (int i = 0; i < len; i++)
            {
                if (!nextIndex.ContainsKey(bwtBytes[i]))
                {
                    nextIndex[bwtBytes[i]] = new Queue<int>();
                }
                nextIndex[bwtBytes[i]].Enqueue(i);
            }

            int[] next = new int[len];
            int j = 0;
            foreach (var entry in count.OrderBy(x => x.Key))
            {
                for (int i = 0; i < entry.Value; i++)
                {
                    next[j++] = nextIndex[entry.Key].Dequeue();
                }
            }

            // Reconstruct the original byte array
            byte[] original = new byte[len];
            index = next[0];
            for (int i = 0; i < len; i++)
            {
                original[i] = firstColumn[index];
                index = next[index];
            }

            return original;
        }

        //public static byte[] InverseTransform_Text(byte[] bwtArray)
        //{
        //    // Initialize the count array
        //    int[] count = new int[256];
        //    for (int i = 0; i < bwtArray.Length; i++)
        //    {
        //        count[bwtArray[i]]++;
        //    }

        //    // Compute the cumulative count array
        //    for (int i = 1; i < 256; i++)
        //    {
        //        count[i] += count[i - 1];
        //    }

        //    // Initialize the next array
        //    int[] next = new int[bwtArray.Length];
        //    for (int i = bwtArray.Length - 1; i >= 0; i--)
        //    {
        //        next[--count[bwtArray[i]]] = i;
        //    }

        //    // Find the index of the EOF character
        //    int eofIndex = Array.IndexOf(bwtArray, (byte)0); // Assuming 0 is the EOF character

        //    // Reconstruct the original text
        //    byte[] originalText = new byte[bwtArray.Length - 1]; // Exclude the EOF character
        //    int index = eofIndex;
        //    for (int i = 0; i < originalText.Length; i++)
        //    {
        //        index = next[index]; // Follow the next array to reconstruct the text
        //        originalText[i] = bwtArray[index];
        //    }

        //    return originalText;
    }
}


using burrows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace burrows
{


    public class MoveToFront
    {
        public static byte[] Encode(byte[] input)
        {
            List<byte> table = InitializeTable();
            byte[] output = new byte[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                byte index = (byte)table.IndexOf(input[i]);
                output[i] = index;
                table.RemoveAt(index);
                table.Insert(0, input[i]);
            }

            return output;
        }

        // Decode a byte array using the Move-to-Front algorithm
        public static byte[] Decode(byte[] input)
        {
            List<byte> table = InitializeTable();
            byte[] output = new byte[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                byte value = table[input[i]];
                output[i] = value;
                table.RemoveAt(input[i]);
                table.Insert(0, value);
            }
            Console.WriteLine("decode finsish");
            return output;
        }

        // Initialize the table with values from 0 to 255
        private static List<byte> InitializeTable()
        {
            List<byte> table = new List<byte>();
            for (int i = 0; i < 256; i++)
            {
                table.Add((byte)i);
            }
            return table;
        }
    }

}

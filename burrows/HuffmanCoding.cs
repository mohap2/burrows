using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using PQ;

namespace Algorithm_assignment

{
    public class HuffmanNode : IComparable<HuffmanNode>
    {
        public byte Character { get; set; }
        public int Frequency { get; set; }
        public int size = 1;
        public HuffmanNode LeftChild { get; set; }
        public HuffmanNode RightChild { get; set; }

        // Constructor
        public HuffmanNode(byte character, int frequency)
        {
            Character = character;
            Frequency = frequency;
            LeftChild = null;
            RightChild = null;
        }
        public HuffmanNode() { }

        // Check if the node has no children (leaf node)
        public bool IsLeafNode()
        {
            return LeftChild == null && RightChild == null;
        }
        // Implement IComparable interface to compare HuffmanNode based on frequency
        public int CompareTo(HuffmanNode other)
        {
            return Frequency.CompareTo(other.Frequency);
        }
    }

    public class HuffmanTreeBuilder
    {
        public HuffmanNode BuildTree(List<HuffmanNode> nodes)
        {
            // Use the updated PriorityQueue
            PriorityQueue<HuffmanNode> priorityQueue = new PriorityQueue<HuffmanNode>();
            foreach (var node in nodes)
            {
                priorityQueue.Add(node);
            }

            // Build the Huffman tree
            while (priorityQueue.Count > 1)
            {
                // Dequeue two nodes with the lowest frequencies
                HuffmanNode leftChild = priorityQueue.Pop();
                HuffmanNode rightChild = priorityQueue.Pop();

                // Create a new internal node with these two nodes as children
                HuffmanNode internalNode = new HuffmanNode(byte.MaxValue, leftChild.Frequency + rightChild.Frequency)
                {
                    size = leftChild.size + rightChild.size + 1,
                    LeftChild = leftChild,
                    RightChild = rightChild
                };

                // Enqueue the new internal node back into the priority queue
                priorityQueue.Add(internalNode);
            }

            // The last node remaining in the queue is the root of the Huffman tree
            return priorityQueue.Pop();
        }

        public void EncodeTreeStructure(HuffmanNode node, BitArray TreeStructure, byte[] leafValues, ref int currentIndex, ref int LeafIndex)
        {
            if (node == null)
                return;
            // Set the bit at currentIndex to indicate whether the node is a leaf
            TreeStructure[currentIndex++] = node.IsLeafNode();

            if (node.IsLeafNode())
            { // Save the value of the leafnode
                leafValues[LeafIndex++] = node.Character;
            }
            else // If it's not a leaf node, recursively encode its left and right children
            {
                EncodeTreeStructure(node.LeftChild, TreeStructure, leafValues, ref currentIndex, ref LeafIndex);
                EncodeTreeStructure(node.RightChild, TreeStructure, leafValues, ref currentIndex, ref LeafIndex);
            }
        }

        public HuffmanNode ReConstructHuffmanTree(BitArray bitArray, ref int index)
        {
            HuffmanNode curr = new HuffmanNode();
            curr.size = 1;
            if (bitArray[index] == true)
                return curr;
            index++;
            curr.LeftChild = ReConstructHuffmanTree(bitArray, ref index);
            curr.size += curr.LeftChild.size;
            index++;
            curr.RightChild = ReConstructHuffmanTree(bitArray, ref index);
            curr.size += curr.RightChild.size;
            return curr;
        }

        public void ReConstructTreeLeaves(HuffmanNode Node, byte[] LeafValues, ref int index)
        {
            if (Node.IsLeafNode())
            {
                Node.Character = LeafValues[index++];
                return;
            }
            ReConstructTreeLeaves(Node.LeftChild, LeafValues, ref index);
            ReConstructTreeLeaves(Node.RightChild, LeafValues, ref index);
        }
    }


    public class HuffmanCoding
    {
        public byte[] Compress(byte[] input)
        {
            Dictionary<byte, HuffmanNode> frequencyMap = new Dictionary<byte, HuffmanNode>();
            List<HuffmanNode> huffmanNodes = new List<HuffmanNode>();
            HuffmanNode root;
            Dictionary<byte, string> ChartoBinary = new Dictionary<byte, string>();
            // Populate the frequency map and initialize Huffman nodes
            foreach (byte c in input)
            {
                if (frequencyMap.ContainsKey(c))
                {
                    frequencyMap[c].Frequency++;
                }
                else
                {
                    HuffmanNode node = new HuffmanNode(c, 1);
                    frequencyMap.Add(c, node);
                    huffmanNodes.Add(node);
                }
            }
            HuffmanTreeBuilder huffmanTreeBuilder = new HuffmanTreeBuilder();
            root = huffmanTreeBuilder.BuildTree(huffmanNodes);
            DFS(root, "", ChartoBinary);
            int BinarySize = 0;
            foreach (HuffmanNode ch in huffmanNodes)
            {
                BinarySize += ch.Frequency * ChartoBinary[ch.Character].Length;
            }
            //bit size needed to encode the tree structure 
            int TreeStructureSize = root.size;
            //number of bytes needed to add the values of the leafnodes
            int LeafValuesSize = ChartoBinary.Count;


            //Return array size
            int Totalsize = Bit2Byte(BinarySize) + Bit2Byte(TreeStructureSize) + LeafValuesSize + 4;
            //Return array index
            int Rindex = 0;
            //Return array
            byte[] Return = new byte[Totalsize];

            BitArray TreeStructure = new BitArray(TreeStructureSize);
            byte[] LeafValues = new byte[LeafValuesSize];
            int index1 = 0, index2 = 0;

            huffmanTreeBuilder.EncodeTreeStructure(root,TreeStructure,LeafValues,ref index1,ref index2);

            BitArray compressed = new BitArray(BinarySize);
            int index = 0;

            foreach (byte c in input)
            {
                // Convert the binary string to bytes and add to the list
                string binaryString = ChartoBinary[c];
                foreach (char bit in binaryString)
                {
                    compressed.Set(index++, bit == '0' ? false : true);
                }
            }
            TreeStructure.CopyTo(Return, Rindex);
            Rindex += Bit2Byte(TreeStructureSize);
            LeafValues.CopyTo(Return, Rindex);
            Rindex += LeafValuesSize;
            BitConverter.GetBytes(BinarySize).CopyTo(Return, Rindex);
            Rindex += 4;
            compressed.CopyTo(Return, Rindex);


            return Return;
        }

        public byte[] DCompress(byte[] input)
        {
            BitArray compressed = new BitArray(input);
            int ind = 0;
            HuffmanTreeBuilder huffmanTreeBuilder = new HuffmanTreeBuilder();   
            HuffmanNode root = huffmanTreeBuilder.ReConstructHuffmanTree(compressed, ref ind);
            int TreeStructureSize = root.size;
            int currBytePosition = Bit2Byte(TreeStructureSize);
            int temp = currBytePosition;
            huffmanTreeBuilder.ReConstructTreeLeaves(root, input, ref temp);
            int LeafNumber = temp - currBytePosition;
            currBytePosition += LeafNumber;

            int codeLength = BitConverter.ToInt32(input, currBytePosition);
            currBytePosition += 4;

            List<Byte> Decompressed = new List<byte>();

            int BitPosition = currBytePosition * 8;
            HuffmanNode currNode;
            int c = 0;
            while (BitPosition < compressed.Length && c < codeLength)
            {
                currNode = root;
                while (!currNode.IsLeafNode())
                {
                    bool b = compressed[BitPosition++];
                    c++;
                    if (b)
                        currNode = currNode.RightChild;
                    else
                        currNode = currNode.LeftChild;
                }
                Decompressed.Add(currNode.Character);
            }

            return Decompressed.ToArray();

        }
        public  void DFS(HuffmanNode Root, string Binary, Dictionary<byte, string> ChartoBinary)
        {
            if (Root.IsLeafNode())
            {
                ChartoBinary.Add(Root.Character, Binary);
            }

            if (Root.LeftChild != null)
            {
                string leftBinary = Binary + '0';
                DFS(Root.LeftChild, leftBinary, ChartoBinary);
            }
            if (Root.RightChild != null)
            {
                string rightBinary = Binary + '1';
                DFS(Root.RightChild, rightBinary, ChartoBinary);
            }

        }

        private  int Bit2Byte(int bitLength)
        {
            return (bitLength + 7) / 8;
        }
    }
}

Burrows-Wheeler Compression Project

This project implements a lossless data compression and decompression system using three stages:

    Burrows-Wheeler Transform (BWT)

    Move-to-Front Encoding (MTF)

    Huffman Coding

The system is written in C# and is capable of compressing and decompressing text files using this pipeline.
How It Works
Compression Process

    Burrows-Wheeler Transform
    The input text is transformed to rearrange characters so that repeated characters appear close together.

    Move-to-Front Encoding
    The transformed output is encoded by maintaining an ordered list of symbols and moving accessed symbols to the front. This produces a stream of integers with many small values.

    Huffman Encoding
    The integer stream is compressed using Huffman coding, assigning shorter binary codes to more frequent values.

    Result Saving
    Intermediate results and the final compressed output are saved in a results folder, along with execution time printed to the console.

Decompression Process

    Huffman Decoding
    The compressed binary is decoded using the stored Huffman tree to recover the MTF-encoded stream.

    Move-to-Front Decoding
    The integer stream is decoded back to the BWT-transformed text.

    Inverse BWT
    The original text is reconstructed from the BWT output using the stored row index.

    Result Saving
    Each step's result is saved in a separate decompression results folder.

File Structure

    Program.cs: Entry point with compression and decompression logic.

    BurrowsWheeler.cs: BWT and inverse BWT implementation.

    MoveToFront.cs: MTF encode/decode.

    HuffmanCoding.cs: Compress and decompress using Huffman.

    HuffmanTreeBuilder.cs, MinHeap.cs, HuffmanNode.cs: Support Huffman coding.

using System;

namespace SF.Unpacker
{
    class Gla2Entry
    {
        public UInt32 dwHash { get; set; }
        public UInt32 dwOffset { get; set; }
        public Int32 dwDecompressedSize { get; set; }
        public Int32 dwCompressedSize { get; set; }
        public Int32 dwReserved { get; set; }
        public Int32 dwEncrypted { get; set; } // if value > 0 then encrypted (can be 0/1/8 -> algorithm type?)
    }
}

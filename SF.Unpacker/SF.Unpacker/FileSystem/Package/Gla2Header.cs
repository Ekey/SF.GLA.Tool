using System;

namespace SF.Unpacker
{
    class Gla2Header
    {
        //Values in big endian
        public Int32 dwVersion { get; set; } // Always 0
        public UInt32 dwTableOffset { get; set; }
        public Int32 dwTotalFiles { get; set; }
        public UInt32 dwMagic { get; set; } // A9D10201
    }
}

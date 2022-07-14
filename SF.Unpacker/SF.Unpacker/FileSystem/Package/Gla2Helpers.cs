using System;

namespace SF.Unpacker
{
    class Gla2Helpers
    {
        public static UInt32 SwapUInt32(UInt32 dwValue)
        {
            dwValue = (dwValue >> 16) | (dwValue << 16);
            dwValue = ((dwValue & 0xFF00FF00U) >> 8) | ((dwValue & 0x00FF00FFU) << 8);
            return dwValue;
        }

        public static Int32 SwapInt32(Int32 val)
        {
            unchecked
            {
                return (Int32)SwapUInt32((UInt32)val);
            }
        }
    }
}

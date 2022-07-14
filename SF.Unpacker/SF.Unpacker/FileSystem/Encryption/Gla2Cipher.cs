using System;

namespace SF.Unpacker
{
    class Gla2Cipher
    {
        public static Byte[] iDecryptData(Byte[] lpBuffer)
        {
            UInt32 A, B, C, D, E;

            Int32 j = 0;
            E = 0x3857A;
            for (Int32 i = 0; i < lpBuffer.Length / 4; i++, j += 4)
            {
                UInt32 dwData = BitConverter.ToUInt32(lpBuffer, j);

                A = E * 0x19660D + 0x3C6EF35F;
                B = A * 0x19660D + 0x3C6EF35F;

                var bTemp1 = (Byte)(B >> 0x10);
                var bTemp2 = bTemp1 & 0x1f;

                if ((A >> 0x10 & 1) == 0)
                {
                    bTemp1 = (Byte)(bTemp1 & 0x1f);
                    dwData = dwData << bTemp1 | dwData >> 0x20 - bTemp1;
                }
                else
                {
                    dwData = dwData >> bTemp2 | dwData << 0x20 - bTemp2;
                }

                A = B * 0x19660D + 0x3C6EF35F;
                C = A * 0x19660D + 0x3C6EF35F;
                D = C * 0x19660D + 0x3C6EF35F;
                E = D * 0x19660D + 0x3C6EF35F;

                dwData = dwData ^ ((((A >> 0x10 & 0xff) << 8 | C >> 0x10 & 0xff) << 8 | D >> 0x10 & 0xff) << 8 | E >> 0x10 & 0xff);

                lpBuffer[j + 0] = (Byte)dwData;
                lpBuffer[j + 1] = (Byte)(dwData >> 8);
                lpBuffer[j + 2] = (Byte)(dwData >> 16);
                lpBuffer[j + 3] = (Byte)(dwData >> 24);
            }

            return lpBuffer;
        }
    }
}

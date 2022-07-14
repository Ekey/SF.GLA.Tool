using System;

namespace SF.Unpacker
{
    class Gla2Hash
    {
        public static UInt32 iGetHash(String m_String)
        {
		UInt32 FNV_PRIME = 0x1000193;
		UInt32 dwHash = 0x811C9DC5;

		for (Int32 i = 0; i < m_String.Length; i++)
		{
			dwHash ^= (Byte)m_String[i];
			dwHash *= FNV_PRIME;
		}

		return dwHash;
		}
	}
}

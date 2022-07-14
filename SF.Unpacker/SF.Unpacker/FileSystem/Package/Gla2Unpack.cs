using System;
using System.IO;
using System.Collections.Generic;

namespace SF.Unpacker
{
    class Gla2Unpack
    {
        static List<Gla2Entry> m_EntryTable = new List<Gla2Entry>();

        public static void iDoIt(String m_Archive, String m_DstFolder)
        {
            Gla2HashList.iLoadProject();
            using (FileStream TGlaStream = File.OpenRead(m_Archive))
            {
                var lpHeader = TGlaStream.ReadBytes(16);
                var m_Header = new Gla2Header();
                lpHeader = Gla2Cipher.iDecryptData(lpHeader);

                using (var THeaderReader = new MemoryStream(lpHeader))
                {
                    m_Header.dwVersion = Gla2Helpers.SwapInt32(THeaderReader.ReadInt32());
                    m_Header.dwTableOffset = Gla2Helpers.SwapUInt32(THeaderReader.ReadUInt32());
                    m_Header.dwTotalFiles = Gla2Helpers.SwapInt32(THeaderReader.ReadInt32());
                    m_Header.dwMagic = Gla2Helpers.SwapUInt32(THeaderReader.ReadUInt32());

                    if (m_Header.dwVersion != 0)
                    {
                        throw new Exception("[ERROR]: Invalid version of GLA2 archive file!");
                    }

                    if (m_Header.dwMagic != 0xA9D10201)
                    {
                        throw new Exception("[ERROR]: Invalid magic of GLA2 archive file!");
                    }

                    THeaderReader.Dispose();
                }

                TGlaStream.Seek(m_Header.dwTableOffset, SeekOrigin.Begin);
                for (Int32 i = 0; i < m_Header.dwTotalFiles; i++)
                {
                    UInt32 dwHash = TGlaStream.ReadUInt32();
                    UInt32 dwOffset = TGlaStream.ReadUInt32();
                    Int32 dwDecompressedSize = TGlaStream.ReadInt32();
                    Int32 dwCompressedSize = TGlaStream.ReadInt32();
                    Int32 dwReserved = TGlaStream.ReadInt32();
                    Int32 dwEncrypted = TGlaStream.ReadInt32();

                    var TEntry = new Gla2Entry
                    {
                        dwHash = Gla2Helpers.SwapUInt32(dwHash),
                        dwOffset = Gla2Helpers.SwapUInt32(dwOffset),
                        dwDecompressedSize = Gla2Helpers.SwapInt32(dwDecompressedSize),
                        dwCompressedSize = Gla2Helpers.SwapInt32(dwCompressedSize),
                        dwReserved = Gla2Helpers.SwapInt32(dwReserved),
                        dwEncrypted = Gla2Helpers.SwapInt32(dwEncrypted),
                    };

                    m_EntryTable.Add(TEntry);
                }

                foreach (var m_Entry in m_EntryTable)
                {
                    String m_FileName = Gla2HashList.iGetNameFromHashList(m_Entry.dwHash);
                    String m_FullPath = m_DstFolder + m_FileName;

                    Utils.iSetInfo("[UNPACKING]: " + m_FileName);
                    Utils.iCreateDirectory(m_FullPath);

                    TGlaStream.Seek(m_Entry.dwOffset, SeekOrigin.Begin);

                    if (m_Entry.dwCompressedSize == 0)
                    {
                        var lpBuffer = TGlaStream.ReadBytes(m_Entry.dwDecompressedSize);

                        if (m_Entry.dwEncrypted == 1)
                        {
                            lpBuffer = Gla2Cipher.iDecryptData(lpBuffer);
                        }

                        File.WriteAllBytes(m_FullPath, lpBuffer);
                    }
                    else
                    {
                        var lpBuffer = TGlaStream.ReadBytes(m_Entry.dwCompressedSize);

                        if (m_Entry.dwEncrypted == 1)
                        {
                            lpBuffer = Gla2Cipher.iDecryptData(lpBuffer);
                        }

                        var lpTemp = LZ4.iDecompress(lpBuffer, m_Entry.dwDecompressedSize);
                        File.WriteAllBytes(m_FullPath, lpTemp);
                    }
                }

                TGlaStream.Dispose();
            }
        }
    }
}

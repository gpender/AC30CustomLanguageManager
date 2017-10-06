using Parker.AP.Common.CustomLanguages;
using System.Collections.Generic;
using System.Globalization;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using System.Collections;
using System.IO;

namespace AC30CustomLanguageManager.Model
{
    public class LanguageFileGenerator : ILanguageFileGenerator
    {
        public byte[] CreateLanguageFileBytes(ILanguageStringCollection languageStringCollection, ushort fixedStringCount, ushort softStringCount, Version deviceVersion, uint driveCustomizationSignature)
        {
            LanguageFileBody lfb = new LanguageFileBody(languageStringCollection, fixedStringCount, softStringCount, deviceVersion, driveCustomizationSignature);
            //byte[] bytes = lfb.ToByteArrayWithChecksum();
            return lfb.ToByteArray();// bytes;
        }
    }
    public class LanguageFileBody : MessageBase
    {
        LanguageFileHeader header;
        List<byte> bodyBytes = new List<byte>();

        public LanguageFileBody(ILanguageStringCollection languageStringCollection, ushort fixedStringCount, ushort softStringCount, Version deviceVersion, uint driveCustomizationSignature)
        {
            ushort bytesPerString = (ushort)(GetMaxBytesPerString(languageStringCollection) + 1);
            ushort bytesPerEntry = (ushort)(bytesPerString + 2); // Add 2 bytes for the StringId 
            header = new LanguageFileHeader(fixedStringCount, softStringCount, bytesPerEntry, deviceVersion, driveCustomizationSignature);

            foreach(ITranslation translation in languageStringCollection.Translations)
            {
                bodyBytes.AddRange(GetBigEndianBytes((ushort)translation.StringId));
                bodyBytes.AddRange(GetBytesFromString(translation.String, bytesPerString));
            }
            if (header.FileLength % 2 != 0 )
            {
                bodyBytes.Add(0x00);
                header.FileLength += 1;
            }
        }

        public byte[] ToByteArrayWithChecksum()
        {
            List <byte> bytes = ToByteArray().ToList();
            ushort sum = (UInt16)Enumerable.Range(0, bytes.Count / 2).Select(i => BitConverter.ToUInt16(bytes.ToArray(), i * 2)).Sum(x => (long)x);
            bytes.AddRange(GetBigEndianBytes(sum));
            return bytes.ToArray();
        }

        public virtual byte[] ToByteArray()
        {
            List<byte> msg = new List<byte>();
            msg.AddRange(header.ToByteArray());
            msg.AddRange(bodyBytes);
            return msg.ToArray();
        }

        ushort GetMaxBytesPerString(ILanguageStringCollection languageStringCollection)
        {
            ushort maxByteCount = 0;
            foreach(var s in languageStringCollection.Translations)
            {
                int tmp = Encoding.UTF8.GetBytes(s.String).Length;
                if(tmp > maxByteCount)
                {
                    maxByteCount = (ushort)tmp;
                }
            }
            return maxByteCount;// (ushort)languageStringCollection.Translations.Max(s => s.String.Length);
        }
        ushort GetMaxStringId(ILanguageStringCollection languageStringCollection)
        {
            return (ushort)languageStringCollection.Translations.Max(s => s.StringId);
        }
    }

    public class LanguageFileHeader : MessageBase
    {
        int headerTotalDataSize = 20;
        byte[] fileFormat = new byte[] { 0x00, 0x11 };
        byte[] reserved0 = new byte[2] { 0x00, 0x00 };
        //byte[] reserved1 = new byte[2] { 0x00, 0x00 };
        //byte[] reserved2 = new byte[2] { 0x00, 0x00 };

        public Version DeviceVersion { get; set; }
        public UInt32 DriveCustomizationSignature { get; set; }
        public UInt32 FileLength { get; set; }
        public UInt16 FixedStringCount { get; set; }
        public UInt16 SoftStringCount { get; set; }
        public UInt16 BytesPerEntry { get; set; } // Will include 2 bytes for the string ID
        public LanguageFileHeader(ushort fixedStringCount, ushort softStringCount, ushort bytesPerEntry, Version deviceVersion, UInt32 driveCustomizationSignature)
        {
            FixedStringCount = fixedStringCount;
            SoftStringCount = softStringCount;
            BytesPerEntry = bytesPerEntry;
            FileLength = (UInt32)(headerTotalDataSize + (fixedStringCount * BytesPerEntry) + (softStringCount * BytesPerEntry));
            DeviceVersion = deviceVersion;
            DriveCustomizationSignature = driveCustomizationSignature;
        }

        public virtual byte[] ToByteArray()
        {
            List<byte> msg = new List<byte>();
            msg.AddRange(fileFormat);
            msg.AddRange(reserved0);
            msg.AddRange(GetBigEndianBytes(FileLength));
            msg.AddRange(GetBigEndianBytes(DriveCustomizationSignature));
            msg.AddRange(GetBigEndianBytes(DeviceVersion));
            msg.AddRange(GetBigEndianBytes(FixedStringCount));
            msg.AddRange(GetBigEndianBytes(SoftStringCount));
            msg.AddRange(GetBigEndianBytes(BytesPerEntry));
            return msg.ToArray();
        }

        public virtual void Initialize(byte[] frame)
        {
            HeaderDataStructure hds = (HeaderDataStructure)RawDataToObject(ref frame, typeof(HeaderDataStructure), headerTotalDataSize);
            fileFormat = hds.fileFormat;
            reserved0 = hds.reserved0;
            FileLength = GetBigEndianUInt(hds.fileLength);
            DriveCustomizationSignature = GetBigEndianUInt(hds.driveCustomizationSignature);
            DeviceVersion = GetVersion(hds.version);
            FixedStringCount = GetBigEndianUshort(hds.fixedStringCount);
            SoftStringCount = GetBigEndianUshort(hds.softStringCount);
            BytesPerEntry = GetBigEndianUshort(hds.bytesPerEntry);
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        internal class HeaderDataStructure
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] fileFormat = new byte[2];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] reserved0 = new byte[2];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] fileLength = new byte[4];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] driveCustomizationSignature = new byte[4];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] version = new byte[2];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] fixedStringCount = new byte[2];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] softStringCount = new byte[2];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] bytesPerEntry = new byte[2];
        }
    }


    public abstract class MessageBase
    {
        protected IEnumerable<byte> GetBytesFromString(string stringProperty, int stringPropertyRequestedSize)
        {
            List<byte> bytes = Encoding.UTF8.GetBytes(stringProperty).ToList();
            int actualSize = bytes.Count;
            for (int i = actualSize; i < stringPropertyRequestedSize; i++)
            {
                bytes.Add(0x00);
            }
            var guy = BitConverter.ToString(bytes.ToArray()).Replace("-", string.Empty);
            return bytes;
        }

        protected byte[] GetBigEndianBytes(ushort val)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            Array.Reverse(bytes);
            return bytes;
        }
        protected byte[] GetBigEndianBytes(UInt32 val)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            Array.Reverse(bytes);
            return bytes;
        }
        protected byte[] GetBigEndianBytes(Version val)
        {
            byte[] bytes = new byte[2] { (byte)val.Major, (byte)val.Minor };
            return bytes;
        }

        protected ushort GetBigEndianUshort(byte[] val)
        {
            return BitConverter.ToUInt16(val, 0);
        }

        protected UInt32 GetBigEndianUInt(byte[] val)
        {
            return BitConverter.ToUInt32(val, 0);
        }
        protected Version GetVersion(byte[] version)
        {
            return new Version((int)version[0], (int)version[1]);
        }

        protected void ValidateStringAndRemoveTrailingNulls(ref string value, int requestedSize, string propertyName)
        {
            if (value.Length > requestedSize)
            {
                throw new Exception(propertyName + " property too long");
            }
            value = value.TrimEnd(new char[] { '\0' });
        }

        protected object RawDataToObject(ref byte[] rawData, Type overlayType, int dataTotalSize)
        {
            if (rawData.Length != dataTotalSize)
            {
                throw new Exception("Incorrect Data length in Message");
            }
            object result = null;

            GCHandle pinnedRawData = GCHandle.Alloc(rawData, GCHandleType.Pinned);
            try
            {
                // Get the address of the data array
                IntPtr pinnedRawDataPtr = pinnedRawData.AddrOfPinnedObject();

                // overlay the data type on top of the raw data
                result = Marshal.PtrToStructure(pinnedRawDataPtr, overlayType);
            }
            finally
            {
                // must explicitly release
                pinnedRawData.Free();
            }
            return result;
        }
    }

}

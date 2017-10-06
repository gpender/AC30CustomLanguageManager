using System;
using System.Collections.Generic;
using System.Globalization;

namespace AC30CustomLanguageInterfaces.Interfaces
{
    public interface ILanguageFileGenerator
    {
        /// <summary>
        /// languageId is used to define the file name language0.lang
        /// 0 - English
        /// 1 - French
        /// 2 - German
        /// 3 - Spanish
        /// 4 - Italian
        /// 5 - Chinese
        /// 6 - L 6
        /// 7 - L 7
        /// 8 - L 8
        /// 9 - L 9
        /// </summary>
        /// <param name="languageId"></param>
        /// <param name="languageStringCollection"></param>
        byte[] CreateLanguageFileBytes(ILanguageStringCollection languageStringCollection, ushort fixedStringCount, ushort softStringCount, Version deviceVersion, uint driveCustomizationSignature);


    //  For each language the display text shall be held as a single file.This file is to be saved in non-volatile
    //  memory on the GKP.Following reset, the GKP is to check that its text file(s) are valid and also to
    //  check with the drive that its text file(s) are up to date, and to update them if necessary and possible.
    //  The file contains a header section, an offset section, a text section and a checksum.
    //  The maximum size of a text file will not exceed 64kB.

        //  5.1.1 Header
        //  Element Data type Description
        //  File format uint16_t Identifies the file format
        //  0x0002 This file format
        //  File length uint16_t Length of file in bytes(1)
        //  Version uint16_t Version of this file(2).
        //  Reserved1 uint16_t Set to zero
        //  Reserved2 uint16_t Set to zero
        //  Max string ID uint16_t Highest known string ID in this file.
        //  Text table start uint16_t Offset to start of text section from start of file.
        //  All offsets are stored in big-endian, (Motorola), format.The GKP may re-format the language file to a
        //  more convenient format.
        //  (1) The file length is the total length of the file up to but not including the checksum.The file length is
        //  always an even number of bytes.A pad byte of 0x00 is appended to the file data if necessary to
        //  make the length even.
        //  (2) Text files shall be both backward and forward compatible within a drive range.This allows a more
        //  recent file to be used for an older version of drive, or for an older version of file to be used with a
        //  newer drive version, within the limitations of the text known to the older version.

        //  5.1.2 Offset section
        //  The offset section is an array of offsets.Each element in the array corresponds to a string ID. The
        //  string ID may be used as in index into this table.Where the offset for a given string ID is set to zero,
        //  there is no associated string text, and the English text is to be used instead. Each offset is relative to
        //  the start of the Text table and gives the address of the corresponding text.

        //  5.1.3 Text Table section
        //  The Text Table section contains all the text.Each text item is held as a null terminated string. The text
        //  is stored in the format defined in the file header.

        //  5.1.4 Checksum
        //  The checksum is the sum of the entire file starting with the File Format word up to but not including
        //  the Checksum word itself.The file is treated as an array of unsigned bytes.Each two bytes of data are
        //  treated as an unsigned 16-bit integer, with the first byte in the pair interpreted as the LSB and the next
        //  byte interpreted as the MSB.
        //  The result of this sum is inverted, (1’s compliment). The checksum is sent to the GKP in big endian
        //  format, (MSB first).
    }
}
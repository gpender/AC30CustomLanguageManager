using System.Collections.Generic;

namespace Parker.AP.Common.CustomLanguages
{
    public interface IDriveCustomizationStringReader
    {
        string CompilerWarning { get; }
        uint DriveCustomizationSignature { get; }
        uint DriveCustomizationSignaturePre3570 { get; }

        IEnumerable<ITranslation> GetStringsFromDriveCustomization(IDriveCustomizationXmlProvider driveCustomizationXmlProvider);
    }
}
using System.Collections.Generic;

namespace AC30CustomLanguageInterfaces.Interfaces
{
    public interface IDriveCustomizationStringReader
    {
        uint DriveCustomizationSignature { get; }
        IEnumerable<ITranslation> GetStringsFromDriveCustomization(IDriveCustomizationXmlProvider driveCustomizationXmlProvider);
    }
}
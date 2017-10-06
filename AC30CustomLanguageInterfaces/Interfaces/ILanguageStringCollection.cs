using System;
using System.Collections.Generic;

namespace AC30CustomLanguageInterfaces.Interfaces
{
    public interface ILanguageStringCollection
    {
        int IdForLanguageString { get; set; }
        int Index { get; set; }
        string Language { get; }
        List<ITranslation> Translations { get; set; }
    }
}
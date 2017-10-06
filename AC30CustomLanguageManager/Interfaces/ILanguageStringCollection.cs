using System;
using System.Collections.Generic;

namespace Parker.AP.Common.CustomLanguages
{
    public interface ILanguageStringCollection
    {
        int Index { get; set; }
        List<ITranslation> Translations { get; set; }
    }
}
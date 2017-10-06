using System.Collections.Generic;
using System.Globalization;

namespace Parker.AP.Common.CustomLanguages
{
    public interface IReferenceStringProvider
    {
        string GetReferenceString(int id);
    }
}
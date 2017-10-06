using Parker.AP.Common.CustomLanguages;
using System.Collections.Generic;

namespace AC30CustomLanguageManager.Model
{
    public class ReferenceStringProvider : IReferenceStringProvider
    {
        Dictionary<int, string> referenceStrings = new Dictionary<int, string>();

        public ReferenceStringProvider() { }

        public ReferenceStringProvider(ILanguageStringCollection languageStringCollection)
        {
            if (languageStringCollection != null)
            {
                foreach (var t in languageStringCollection.Translations)
                {
                    if (!referenceStrings.ContainsKey(t.StringId))
                    {
                        referenceStrings.Add(t.StringId, t.String);
                    }
                }
            }
        }

        public string GetReferenceString(int id)
        {
            if(referenceStrings.ContainsKey(id))
            {
                return referenceStrings[id];
            }
            return "Ref String " + id.ToString() + " not found";
        }
    }
}

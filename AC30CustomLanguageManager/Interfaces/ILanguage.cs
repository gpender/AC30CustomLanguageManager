namespace Parker.AP.Common.CustomLanguages
{
    public interface ILanguage
    {
        int IdForLanguageString { get; }
        int Index { get; }
        string Name { get; set; }
    }
}
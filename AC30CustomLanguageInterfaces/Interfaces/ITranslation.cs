namespace AC30CustomLanguageInterfaces.Interfaces
{
    public interface ITranslation
    {
        bool IsSoftString { get; set; }
        int StringId { get; set; }
        string ReferenceString { get; }
        string String { get; set; }
        void SetReferenceStringProvider(IReferenceStringProvider referenceStringProvider);
    }
}
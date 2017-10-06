namespace AC30CustomLanguageInterfaces.Interfaces
{
    public interface IFactory
    {
        int StringId { get; set; }
        string ReferenceString { get; }
        string String { get; set; }
        void SetReferenceStringProvider(IReferenceStringProvider referenceStringProvider);
    }
}
namespace Parker.AP.Common.CustomLanguages
{
    public interface IFactory
    {
        int StringId { get; set; }
        string ReferenceString { get; }
        string String { get; set; }
        void SetReferenceStringProvider(IReferenceStringProvider referenceStringProvider);
    }
}
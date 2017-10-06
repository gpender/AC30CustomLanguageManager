using AC30CustomLanguageInterfaces.Interfaces;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;

namespace AC30CustomLanguageManagerApp.Model
{
    public class Translation : ObservableObject, ITranslation, IComparable, IEquatable<ITranslation>
    {
        IReferenceStringProvider referenceStringProvider;
        string _string = string.Empty;
        public bool IsSoftString { get; set; }
        public string ReferenceString
        {
            get { return referenceStringProvider != null? referenceStringProvider.GetReferenceString(this.StringId): string.Empty; }

        }
        public string String
        {
            get { return _string; }
            set { Set<string>(() => String, ref _string, value); }
        }
        public int StringId { get; set; }

        public Translation() { }

        public Translation(IReferenceStringProvider referenceStringProvider)
        {
            this.referenceStringProvider = referenceStringProvider;
        }

        public int CompareTo(object other)
        {
            if (other == null) return 1;
            return this.StringId.CompareTo(((Translation)other).StringId);
        }

        public bool Equals(ITranslation other)
        {
            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return StringId.GetHashCode();
        }
        public void SetReferenceStringProvider(IReferenceStringProvider referenceStringProvider)
        {
            this.referenceStringProvider = referenceStringProvider;
            RaisePropertyChanged(() => ReferenceString);
        }
        public override string ToString()
        {
            return StringId.ToString() + ":" + ReferenceString + " --> " + String;
        }
    }

    public class TranslationBaseStringEqualityComparer : IEqualityComparer<ITranslation>
    {
        public bool Equals(ITranslation x, ITranslation y)
        {
            return x.String.Equals(y.String);
        }

        public int GetHashCode(ITranslation obj)
        {
            return obj.GetHashCode();
        }
    }
}

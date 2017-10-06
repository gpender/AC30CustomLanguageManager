using Parker.AP.Common.CustomLanguages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace AC30CustomLanguageManager.Model
{
    public class Translation : ITranslation, INotifyPropertyChanged, IComparable, IEquatable<ITranslation>
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
            set
            {
                _string = value;
                OnPropertyChanged(() => String);
            }
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
            OnPropertyChanged(() => ReferenceString);
        }
        public override string ToString()
        {
            return StringId.ToString() + ":" + ReferenceString + " --> " + String;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        public void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            var memberExpr = propertyExpression.Body as MemberExpression;
            if (memberExpr == null)
                throw new ArgumentException("propertyExpression should represent access to a member");
            string memberName = memberExpr.Member.Name;
            OnPropertyChanged(memberName);
        }

        #endregion
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

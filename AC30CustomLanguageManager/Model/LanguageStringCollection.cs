using Parker.AP.Common.CustomLanguages;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Linq.Expressions;

namespace AC30CustomLanguageManager.Model
{
    public class LanguageStringCollection : ILanguageStringCollection, INotifyPropertyChanged, IComparable
    {
        public int Index { get; set; }

        [XmlIgnore]
        public List<ITranslation> Translations { get; set; }
        public List<Translation> TranslationsInternal { get; set; }
        public LanguageStringCollection()
        {
            Translations = new List<ITranslation>();
        }
        public LanguageStringCollection(int index)
        {
            Translations = new List<ITranslation>();
            Index = index;
        }

        public void SetPropertiesBeforeSerializing()
        {
            try
            {
                TranslationsInternal = new List<Translation>();
                foreach (var t in Translations)
                {
                    TranslationsInternal.Add((Translation)t);
                }
            }
            catch { }
        }
        public void ClearPropertiesAfterSerializing()
        {
            try
            {
                TranslationsInternal.Clear();
            }
            catch { }
        }
        public void SetPropertiesAfterDeserializing()
        {
            try
            {
                Translations = TranslationsInternal.AsEnumerable<ITranslation>().ToList();
                TranslationsInternal.Clear();
            }
            catch { }
        }
        public int CompareTo(object other)
        {
            if (other == null) return 1;
            return this.Index.CompareTo(((ILanguageStringCollection)other).Index);
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
}

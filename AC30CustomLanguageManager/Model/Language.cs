using Parker.AP.Common.CustomLanguages;
using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace AC30CustomLanguageManager.Model
{
    public class Language : ILanguage, INotifyPropertyChanged
    {
        string name;
        public int IdForLanguageString { get; private set; }
        public int Index { get; private set; }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(()=>Name);
            }
        }
        public Language(int index,int idForLanguageString, string initialName)
        {
            this.Index = index;
            this.IdForLanguageString = idForLanguageString;
            this.Name = initialName;
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

using Parker.AP.Common.CustomLanguages;
using System;

namespace AC30CustomLanguageManager.Model
{
    public class StringsChangedNotifierEmptyClass : IStringsChangedNotifier
    {
        public event EventHandler DeviceChanged;
        public event EventHandler DriveCustomizationChanged;
        public StringsChangedNotifierEmptyClass(){}
    }
}

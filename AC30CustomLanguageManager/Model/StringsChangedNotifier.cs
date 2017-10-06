using Parker.AP.Common.CustomLanguages;
using System;

namespace AC30CustomLanguageManager.Model
{
    public class StringsChangedNotifier : IStringsChangedNotifier
    {
        public event EventHandler DeviceChanged;
        public event EventHandler DriveCustomizationChanged;

        public StringsChangedNotifier()
        {
            //DispatcherTimer t = new DispatcherTimer();
            //t.Interval = new TimeSpan(0, 0, 15);
            //t.Tick += T_Tick1;
            //t.Start();
        }

        private void T_Tick1(object sender, EventArgs e)
        {
            DeviceChanged?.Invoke(this, new EventArgs());
        }
    }
}

using System;
using System.Collections.Generic;

namespace Parker.AP.Common.CustomLanguages
{
    public interface IStringsChangedNotifier
    {
        event EventHandler DeviceChanged;
        event EventHandler DriveCustomizationChanged;
    }
}
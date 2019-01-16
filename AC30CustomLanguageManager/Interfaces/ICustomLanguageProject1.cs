using System;
using System.Collections.Generic;
using System.Globalization;

namespace Parker.AP.Common.CustomLanguages
{
    public interface ICustomLanguageProject1 : ICustomLanguageProject
    {
        string ProjectFileName { get; set; }
        void ImportDeviceXmlFile(string fileLocation, IDeviceStringReader deviceStringReader, IDeviceXmlProvider deviceXmlProvider);
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OpenRnD.Harness.IISExpress
{
    public static class CSProjUtilities
    {
        private const string msbuildNamespace = "http://schemas.microsoft.com/developer/msbuild/2003";

        public static string GetIISUrl(string csProjPath)
        {
            if (!File.Exists(csProjPath))
            {
                throw new IOException($"Project file not found ({csProjPath}).");
            }

            XDocument projectDocument = XDocument.Load(csProjPath);
            XElement webProjectProperties = projectDocument
                .Root
                .Element(XName.Get("ProjectExtensions", msbuildNamespace))
                .Element(XName.Get("VisualStudio", msbuildNamespace))
                .Elements(XName.Get("FlavorProperties", msbuildNamespace))
                .Single(e => e.Attribute(XName.Get("GUID")).Value == "{349c5851-65df-11da-9384-00065b846f21}")
                .Element(XName.Get("WebProjectProperties", msbuildNamespace));

            string iisUrl = webProjectProperties.Element(XName.Get("IISUrl", msbuildNamespace)).Value;

            return iisUrl;
        }
    }
}

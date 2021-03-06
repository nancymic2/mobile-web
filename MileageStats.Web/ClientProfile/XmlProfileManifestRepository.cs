/*  
Copyright Microsoft Corporation

Licensed under the Apache License, Version 2.0 (the "License"); you may not
use this file except in compliance with the License. You may obtain a copy of
the License at 

http://www.apache.org/licenses/LICENSE-2.0 

THIS CODE IS PROVIDED ON AN *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY IMPLIED 
WARRANTIES OR CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE, 
MERCHANTABLITY OR NON-INFRINGEMENT. 

See the Apache 2 License for the specific language governing permissions and
limitations under the License. */

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml;
using MileageStats.Web.ClientProfile.Model;

namespace MileageStats.Web.ClientProfile
{
    public class XmlProfileManifestRepository : IProfileManifestRepository
    {
        readonly Func<string, string> _virtualPathResolver;
        readonly string _virtualPath;

        public XmlProfileManifestRepository(string virtualPath, Func<string, string> virtualPathResolver)
        {
            _virtualPathResolver = virtualPathResolver;
            _virtualPath = virtualPath;
        }

        public string GetManifestPath(string name)
        {
            return _virtualPathResolver(_virtualPath + name + ".xml");
        }

        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public ProfileManifest GetProfile(string name)
        {
            var filePath = GetManifestPath(name);

            using (var sr = new StreamReader(filePath))
            using (var reader = XmlReader.Create(sr))
            {
                var profile = XmlProfileManifestParser.Parse(reader);
                return profile;
            }
        }
    }
}

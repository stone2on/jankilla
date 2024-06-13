using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.Utils
{
    public class IniFile
    {
        public string Path { get; private set; }

        private string _exe = Assembly.GetEntryAssembly().GetName().Name;
        

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string section, string key, string val, string path);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string path);

        public IniFile(string dir)
        {
            Path = new FileInfo($"{dir}\\{_exe}.ini").FullName;
        }

        public string Read(string key, string section = null)
        {
            var retVal = new StringBuilder(255);
            GetPrivateProfileString(section ?? _exe, key, string.Empty, retVal, 255, Path);
            return retVal.ToString();
        }

        public void Write(string key, string val, string section = null)
        {
            WritePrivateProfileString(section ?? _exe, key, val, Path);
        }

        public void DeleteKey(string key, string section = null)
        {
            Write(key, null, section ?? _exe);
        }

        public void DeleteSection(string section = null)
        {
            Write(null, null, section ?? _exe);
        }

        public bool KeyExists(string key, string section = null)
        {
            return Read(key, section).Length > 0;
        }
    }
}

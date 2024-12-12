using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.Utils
{
    public static class AssemblyHelper
    {
        public static void LoadDlls()
        {
            string[] dllFiles = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");

            //NOTE:: Net Framework 4.7.2 대응 (grpc_csharp_ext.x64.dll, grpc_csharp_ext.x86.dll) 제외
            var dlls = dllFiles.Where(d => !d.Contains("ext"));

            foreach (string dllPath in dlls)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(dllPath);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error loading assembly from " + dllPath + ": " + ex.Message);
                }
            }
        }
    }
}

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

            foreach (string dllPath in dllFiles)
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

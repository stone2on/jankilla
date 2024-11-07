using Jankilla.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jankilla.Driver.MitsubishiMxComponent
{
    public class MitsubishiMxComponentDriver : Core.Contracts.Driver
    {
        #region Statics

        public static List<string> AllDevices = new List<string>()
        {
          "SM",
          "SB",
          "FX",
          "FY",
          "SD",
          "SW",
          "FD",
          "D",
          "B",
          "Y",
          "M",
          "L",
          "S",
          "X",
          "F",
          "V",
          "W",
          "C",
          "T",
          "R",
        };

        public static HashSet<string> BitDeviceTypes = new HashSet<string>()
        {
          "B",
          "Y",
          "M",
          "L",
          "S",
          "X",
          "F",
          "V",
          "SM",
          "SB",
          "FX",
          "FY",
        };

        public static HashSet<string> WordDeviceTypes = new HashSet<string>()
        {
          "W",
          "C",
          "D",
          "T",
          "R",
          "SD",
          "SW",
          "FD"
        };

        public static HashSet<string> HexDeviceTypes = new HashSet<string>()
        {
          "X",
          "Y",
          "B",
          "W",
          "SB",
          "SW",
          "WW",
          "WR",
          "ML",
          "MC",
          "MF"
        };

        public static HashSet<string> DecimalDeviceTypes = new HashSet<string>()
        {
          "FX",
          "FY",
          "FD",
          "SM",
          "SD",
          "M",
          "L",
          "F",
          "V",
          "D",
          "TS",
          "TC",
          "TN",
          "CS",
          "CC",
          "CN",
          "SS",
          "SC",
          "SN",
          "S",
          "A",
          "Z",
          "R",
          "ZR"
        };

        #endregion

        #region Public Properties

        public override string Discriminator => "MitsubishiMxComponent";

        #endregion

    




    }
}

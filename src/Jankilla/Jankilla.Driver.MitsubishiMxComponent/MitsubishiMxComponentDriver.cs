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

        public static readonly List<string> AllDevices = new List<string>()
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

        public static readonly HashSet<string> BitDeviceTypes = new HashSet<string>()
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

        public static readonly HashSet<string> WordDeviceTypes = new HashSet<string>()
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

        public static readonly HashSet<string> HexDeviceTypes = new HashSet<string>()
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

        public static readonly HashSet<string> DecimalDeviceTypes = new HashSet<string>()
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

        public static readonly Dictionary<(ECpuType cpu, string device), int> MaxAddresses = new Dictionary<(ECpuType cpu, string device), int>()
        {
            {(ECpuType.FX5U, "X"), 377},
            {(ECpuType.FX5U, "Y"), 377},
            {(ECpuType.FX5U, "M"), 32767},
            {(ECpuType.FX5U, "L"), 32767},
            {(ECpuType.FX5U, "B"), 0x7FFF},
            {(ECpuType.FX5U, "D"), 32767},
            {(ECpuType.FX5U, "R"), 32767},
            {(ECpuType.FX5U, "W"), 0x7FFF},

            {(ECpuType.RSeries, "X"), 0x1FFF},
            {(ECpuType.RSeries, "Y"), 0x1FFF},
            {(ECpuType.RSeries, "M"), 61439},
            {(ECpuType.RSeries, "L"), 32767},
            {(ECpuType.RSeries, "B"), 0xEFFF},
            {(ECpuType.RSeries, "F"), 32767},
            {(ECpuType.RSeries, "V"), 32767},
            {(ECpuType.RSeries, "D"), 4194303},
            {(ECpuType.RSeries, "W"), 0xEFFF},
            {(ECpuType.RSeries, "R"), 32767},
            {(ECpuType.RSeries, "ZR"), 4194303},

            {(ECpuType.QSeries, "X"), 0x1FFF},
            {(ECpuType.QSeries, "Y"), 0x1FFF},
            {(ECpuType.QSeries, "M"), 61439},
            {(ECpuType.QSeries, "L"), 32767},
            {(ECpuType.QSeries, "B"), 0xEFFF},
            {(ECpuType.QSeries, "D"), 12287},
            {(ECpuType.QSeries, "W"), 0xEFFF},
            {(ECpuType.QSeries, "R"), 32767},

            {(ECpuType.LSeries, "X"), 0x7FF},
            {(ECpuType.LSeries, "Y"), 0x7FF},
            {(ECpuType.LSeries, "M"), 61439},
            {(ECpuType.LSeries, "L"), 32767},
            {(ECpuType.LSeries, "B"), 0xEFFF},
            {(ECpuType.LSeries, "D"), 12287},
            {(ECpuType.LSeries, "W"), 0xEFFF},
            {(ECpuType.LSeries, "R"), 32767},

            {(ECpuType.FX5U, "SM"), 32767},
            {(ECpuType.FX5U, "SD"), 32767},
            {(ECpuType.RSeries, "SM"), 32767},
            {(ECpuType.RSeries, "SD"), 32767},
            {(ECpuType.QSeries, "SM"), 32767},
            {(ECpuType.QSeries, "SD"), 32767},
            {(ECpuType.LSeries, "SM"), 32767},
            {(ECpuType.LSeries, "SD"), 32767}
        };

        #endregion

        #region Public Properties

        public override string Discriminator => "MitsubishiMxComponent";

        #endregion






    }
}

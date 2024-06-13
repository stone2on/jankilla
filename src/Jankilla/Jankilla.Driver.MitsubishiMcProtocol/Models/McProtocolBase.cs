using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Jankilla.Driver.MitsubishiMcProtocol.Defines;
using Jankilla.Driver.MitsubishiMcProtocol.Interfaces;

namespace Jankilla.Driver.MitsubishiMcProtocol.Models
{
    public abstract class McProtocolBase : IPLC
    {
        public EFrame CommandFrame { get; set; }  
        public string HostName { get; set; }  
        public int PortNumber { get; set; }  
        public int Device { private set; get; }
        public abstract bool Connected { get; }

        private const int BLOCK_SIZE = 0x0010;
        private McCommand _command;

        protected McProtocolBase(string iHostName, int iPortNumber, EFrame frame)
        {
            if (string.IsNullOrEmpty(iHostName))
            {
                throw new ArgumentNullException("iHostName");
            }

            CommandFrame = frame;
            //C70 = MC3E

            HostName = iHostName;
            PortNumber = iPortNumber;
        }

        public void Dispose()
        {
            Close();
        }
        public int Open()
        {
            Connect();
            _command = new McCommand(CommandFrame);
            return 0;
        }
        public int Close()
        {
            Disconnect();
            return 0;
        }
        public async Task<int> SetBitDeviceAsync(string iDeviceName, int iSize, int[] iData)
        {
            EDeviceCode type;
            int addr;
            GetDeviceCode(iDeviceName, out type, out addr);
            return await SetBitDeviceAsync(type, addr, iSize, iData);
        }
        public async Task<int> SetBitDeviceAsync(EDeviceCode iType, int iAddress, int iSize, int[] iData)
        {
            var type = iType;
            var addr = iAddress;
            var data = new List<byte>(6)
                    {
                        (byte) addr
                      , (byte) (addr >> 8)
                      , (byte) (addr >> 16)
                      , (byte) type
                      , (byte) iSize
                      , (byte) (iSize >> 8)
                    };
            var d = (byte)iData[0];
            var i = 0;
            while (i < iData.Length)
            {
                if (i % 2 == 0)
                {
                    d = (byte)iData[i];
                    d <<= 4;
                }
                else
                {
                    d |= (byte)(iData[i] & 0x01);
                    data.Add(d);
                }
                ++i;
            }
            if (i % 2 != 0)
            {
                data.Add(d);
            }
            int length = (int)_command.FrameType;// == EFrame.MC3E) ? 11 : 15;
            byte[] sdCommand = _command.SetCommandMC3E(0x1401, 0x0001, data.ToArray());
            byte[] rtResponse = await tryExecutionAsync(sdCommand, length);
            int rtCode = _command.SetResponse(rtResponse);
            return rtCode;
        }
        public async Task<int> GetBitDeviceAsync(string iDeviceName, int iSize, int[] oData)
        {
            EDeviceCode type;
            int addr;
            GetDeviceCode(iDeviceName, out type, out addr);
            return await GetBitDeviceAsync(type, addr, iSize, oData);
        }
        public async Task<int> GetBitDeviceAsync(EDeviceCode iType, int iAddress, int iSize, int[] oData)
        {

            EDeviceCode type = iType;
            int addr = iAddress;
            var data = new List<byte>(6)
                    {
                        (byte) addr
                      , (byte) (addr >> 8)
                      , (byte) (addr >> 16)
                      , (byte) type
                      , (byte) iSize
                      , (byte) (iSize >> 8)
                    };
            byte[] sdCommand = _command.SetCommandMC3E(0x0401, 0x0001, data.ToArray());
            int length = (_command.FrameType == EFrame.MC3E) ? 11 : 15;
            byte[] rtResponse = await tryExecutionAsync(sdCommand, length);
            int rtCode = _command.SetResponse(rtResponse);
            byte[] rtData = _command.Response;
            for (int i = 0; i < iSize; ++i)
            {
                if (i % 2 == 0)
                {
                    oData[i] = (rtCode == 0) ? ((rtData[i / 2] >> 4) & 0x01) : 0;
                }
                else
                {
                    oData[i] = (rtCode == 0) ? (rtData[i / 2] & 0x01) : 0;
                }
            }
            return rtCode;
        }
        public async Task<int> WriteDeviceBlockAsync(string iDeviceName, int iSize, int[] iData)
        {
            EDeviceCode type;
            int addr;
            GetDeviceCode(iDeviceName, out type, out addr);
            return await WriteDeviceBlockAsync(type, addr, iSize, iData);
        }
        public async Task<int> WriteDeviceBlockAsync(EDeviceCode iType, int iAddress, int iSize, int[] iData)
        {

            EDeviceCode type = iType;
            int addr = iAddress;
            List<byte> data;

            List<byte> DeviceData = new List<byte>();
            foreach (int t in iData)
            {
                DeviceData.Add((byte)t);
                DeviceData.Add((byte)(t >> 8));
            }

            byte[] sdCommand;
            int length;
            //TEST Create this write switch statement
            switch (CommandFrame)
            {
                case EFrame.MC3E:
                    data = new List<byte>(6)
                    {
                        (byte) addr
                      , (byte) (addr >> 8)
                      , (byte) (addr >> 16)
                      , (byte) type
                      , (byte) iSize
                      , (byte) (iSize >> 8)
                    };
                    data.AddRange(DeviceData.ToArray());
                    sdCommand = _command.SetCommandMC3E(0x1401, 0x0000, data.ToArray());
                    length = 11;
                    break;
                case EFrame.MC4E:
                    data = new List<byte>(6)
                    {
                        (byte) addr
                      , (byte) (addr >> 8)
                      , (byte) (addr >> 16)
                      , (byte) type
                      , (byte) iSize
                      , (byte) (iSize >> 8)
                    };
                    data.AddRange(DeviceData.ToArray());
                    sdCommand = _command.SetCommandMC4E(0x1401, 0x0000, data.ToArray());
                    length = 15;
                    break;
                case EFrame.MC1E:
                    data = new List<byte>(6)
                   {
                          (byte) addr
                      , (byte) (addr >> 8)
                      , (byte) (addr >> 16)
                      , (byte) (addr >> 24)
                      , 0x20
                      , 0x44
                      , (byte) iSize
                      , 0x00
                    };
                    data.AddRange(DeviceData.ToArray());
                    //Add data
                    sdCommand = _command.SetCommandMC1E(0x03, data.ToArray());
                    length = 2;
                    break;
                default:
                    throw new Exception("Message frame not supported");
            }

            //TEST take care of the writing
            byte[] rtResponse = await tryExecutionAsync(sdCommand, length);
            int rtCode = _command.SetResponse(rtResponse);
            return rtCode;
        }
        public async Task<int> WriteDeviceBlockAsync(EDeviceCode iType, int iAddress, int devicePoints, byte[] bData)
        {
            //FIXME
            int iSize = devicePoints;
            EDeviceCode type = iType;
            int addr = iAddress;
            List<byte> data;
            byte[] sdCommand;
            int length;
            //TEST Create this write switch statement
            switch (CommandFrame)
            {
                case EFrame.MC3E:
                    data = new List<byte>(6)
                    {
                        (byte) addr
                      , (byte) (addr >> 8)
                      , (byte) (addr >> 16)
                      , (byte) type
                      , (byte) iSize
                      , (byte) (iSize >> 8)
                    };
                    data.AddRange(bData);
                    sdCommand = _command.SetCommandMC3E(0x1401, 0x0000, data.ToArray());
                    length = 11;
                    break;
                case EFrame.MC4E:
                    data = new List<byte>(6)
                    {
                        (byte) addr
                      , (byte) (addr >> 8)
                      , (byte) (addr >> 16)
                      , (byte) type
                      , (byte) iSize
                      , (byte) (iSize >> 8)
                    };
                    data.AddRange(bData);
                    sdCommand = _command.SetCommandMC4E(0x1401, 0x0000, data.ToArray());
                    length = 15;
                    break;
                case EFrame.MC1E:
                    data = new List<byte>(6)
                   {
                          (byte) addr
                      , (byte) (addr >> 8)
                      , (byte) (addr >> 16)
                      , (byte) (addr >> 24)
                      , 0x20
                      , 0x44
                      , (byte) iSize
                      , 0x00
                    };
                    data.AddRange(bData);
                    //Add data
                    sdCommand = _command.SetCommandMC1E(0x03, data.ToArray());
                    length = 2;
                    break;
                default:
                    throw new Exception("Message frame not supported");
            }
            //TEST take care of the writing
            byte[] rtResponse = await tryExecutionAsync(sdCommand, length);
            int rtCode = _command.SetResponse(rtResponse);
            return rtCode;
        }

        public int WriteDeviceBlock(string iDeviceName, int iSize, int[] iData)
        {
            EDeviceCode type;
            int addr;
            GetDeviceCode(iDeviceName, out type, out addr);
            return WriteDeviceBlock(type, addr, iSize, iData);
        }
        public int WriteDeviceBlock(EDeviceCode iType, int iAddress, int iSize, int[] iData)
        {

            EDeviceCode type = iType;
            int addr = iAddress;
            List<byte> data;

            List<byte> DeviceData = new List<byte>();
            foreach (int t in iData)
            {
                DeviceData.Add((byte)t);
                DeviceData.Add((byte)(t >> 8));
            }

            byte[] sdCommand;
            int length;
            //TEST Create this write switch statement
            switch (CommandFrame)
            {
                case EFrame.MC3E:
                    data = new List<byte>(6)
                    {
                        (byte) addr
                      , (byte) (addr >> 8)
                      , (byte) (addr >> 16)
                      , (byte) type
                      , (byte) iSize
                      , (byte) (iSize >> 8)
                    };
                    data.AddRange(DeviceData.ToArray());
                    sdCommand = _command.SetCommandMC3E(0x1401, 0x0000, data.ToArray());
                    length = 11;
                    break;
                case EFrame.MC4E:
                    data = new List<byte>(6)
                    {
                        (byte) addr
                      , (byte) (addr >> 8)
                      , (byte) (addr >> 16)
                      , (byte) type
                      , (byte) iSize
                      , (byte) (iSize >> 8)
                    };
                    data.AddRange(DeviceData.ToArray());
                    sdCommand = _command.SetCommandMC4E(0x1401, 0x0000, data.ToArray());
                    length = 15;
                    break;
                case EFrame.MC1E:
                    data = new List<byte>(6)
                   {
                          (byte) addr
                      , (byte) (addr >> 8)
                      , (byte) (addr >> 16)
                      , (byte) (addr >> 24)
                      , 0x20
                      , 0x44
                      , (byte) iSize
                      , 0x00
                    };
                    data.AddRange(DeviceData.ToArray());
                    //Add data
                    sdCommand = _command.SetCommandMC1E(0x03, data.ToArray());
                    length = 2;
                    break;
                default:
                    throw new Exception("Message frame not supported");
            }

            //TEST take care of the writing
            byte[] rtResponse = tryWriteExecution(sdCommand, length);
            int rtCode = _command.SetResponse(rtResponse);
            return rtCode;
        }
        public int WriteDeviceBlock(EDeviceCode iType, int iAddress, int devicePoints, byte[] bData)
        {
            //FIXME
            int iSize = devicePoints;
            EDeviceCode type = iType;
            int addr = iAddress;
            List<byte> data;
            byte[] sdCommand;
            int length;
            //TEST Create this write switch statement
            switch (CommandFrame)
            {
                case EFrame.MC3E:
                    data = new List<byte>(6)
                    {
                        (byte) addr
                      , (byte) (addr >> 8)
                      , (byte) (addr >> 16)
                      , (byte) type
                      , (byte) iSize
                      , (byte) (iSize >> 8)
                    };
                    data.AddRange(bData);
                    sdCommand = _command.SetCommandMC3E(0x1401, 0x0000, data.ToArray());
                    length = 11;
                    break;
                case EFrame.MC4E:
                    data = new List<byte>(6)
                    {
                        (byte) addr
                      , (byte) (addr >> 8)
                      , (byte) (addr >> 16)
                      , (byte) type
                      , (byte) iSize
                      , (byte) (iSize >> 8)
                    };
                    data.AddRange(bData);
                    sdCommand = _command.SetCommandMC4E(0x1401, 0x0000, data.ToArray());
                    length = 15;
                    break;
                case EFrame.MC1E:
                    data = new List<byte>(6)
                   {
                          (byte) addr
                      , (byte) (addr >> 8)
                      , (byte) (addr >> 16)
                      , (byte) (addr >> 24)
                      , 0x20
                      , 0x44
                      , (byte) iSize
                      , 0x00
                    };
                    data.AddRange(bData);
                    //Add data
                    sdCommand = _command.SetCommandMC1E(0x03, data.ToArray());
                    length = 2;
                    break;
                default:
                    throw new Exception("Message frame not supported");
            }
            //TEST take care of the writing
            byte[] rtResponse = tryWriteExecution(sdCommand, length);
            int rtCode = _command.SetResponse(rtResponse);
            return rtCode;
        }

        public async Task<byte[]> ReadDeviceBlockAsync(string iDeviceName, int iSize, int[] oData)
        {
            EDeviceCode type;
            int addr;
            GetDeviceCode(iDeviceName, out type, out addr);
            return await ReadDeviceBlockAsync(type, addr, iSize, oData);
        }
        public async Task<byte[]> ReadDeviceBlockAsync(EDeviceCode iType, int iAddress, int iSize, int[] oData)
        {

            EDeviceCode type = iType;
            int addr = iAddress;
            List<byte> data;
            byte[] sdCommand;
            int length;

            switch (CommandFrame)
            {
                case EFrame.MC3E:
                    data = new List<byte>(6)
                    {
                        (byte) addr
                      , (byte) (addr >> 8)
                      , (byte) (addr >> 16)
                      , (byte) type
                      , (byte) iSize
                      , (byte) (iSize >> 8)
                    };
                    sdCommand = _command.SetCommandMC3E(0x0401, 0x0000, data.ToArray());
                    length = 11;
                    break;
                case EFrame.MC4E:
                    data = new List<byte>(6)
                    {
                        (byte) addr
                      , (byte) (addr >> 8)
                      , (byte) (addr >> 16)
                      , (byte) type
                      , (byte) iSize
                      , (byte) (iSize >> 8)
                    };
                    sdCommand = _command.SetCommandMC4E(0x0401, 0x0000, data.ToArray());
                    length = 15;
                    break;
                case EFrame.MC1E:
                    data = new List<byte>(6)
                    {
                          (byte) addr
                      , (byte) (addr >> 8)
                      , (byte) (addr >> 16)
                      , (byte) (addr >> 24)
                      , 0x20
                      , 0x44
                      , (byte) iSize
                      , 0x00
                    };
                    sdCommand = _command.SetCommandMC1E(0x01, data.ToArray());
                    length = 2;
                    break;
                default:
                    throw new Exception("Message frame not supported");
            }

            byte[] rtResponse = await tryExecutionAsync(sdCommand, length);
            //TEST verify read responses
            int rtCode = _command.SetResponse(rtResponse);
            byte[] rtData = _command.Response;
            for (int i = 0; i < iSize; ++i)
            {
                oData[i] = (rtCode == 0) ? BitConverter.ToInt16(rtData, i * 2) : 0;
            }
            return rtData;
        }
        public async Task<byte[]> ReadDeviceBlockAsync(EDeviceCode iType, int iAddress, int devicePoints)
        {
            int iSize = devicePoints;
            EDeviceCode type = iType;
            int addr = iAddress;
            List<byte> data;
            byte[] sdCommand;
            int length;

            switch (CommandFrame)
            {
                case EFrame.MC3E:
                    data = new List<byte>(6)
                    {
                        (byte) addr
                      , (byte) (addr >> 8)
                      , (byte) (addr >> 16)
                      , (byte) type
                      , (byte) iSize
                      , (byte) (iSize >> 8)
                    };
                    sdCommand = _command.SetCommandMC3E(0x0401, 0x0000, data.ToArray());
                    length = 11;
                    break;
                case EFrame.MC4E:
                    data = new List<byte>(6)
                    {
                        (byte) addr
                      , (byte) (addr >> 8)
                      , (byte) (addr >> 16)
                      , (byte) type
                      , (byte) iSize
                      , (byte) (iSize >> 8)
                    };
                    sdCommand = _command.SetCommandMC4E(0x0401, 0x0000, data.ToArray());
                    length = 15;
                    break;
                case EFrame.MC1E:
                    data = new List<byte>(6)
                    {
                          (byte) addr
                      , (byte) (addr >> 8)
                      , (byte) (addr >> 16)
                      , (byte) (addr >> 24)
                      , 0x20
                      , 0x44
                      , (byte) iSize
                      , 0x00
                    };
                    sdCommand = _command.SetCommandMC1E(0x01, data.ToArray());
                    length = 2;
                    break;
                default:
                    throw new Exception("Message frame not supported");
            }

            byte[] rtResponse = await tryExecutionAsync(sdCommand, length);
            //TEST verify read responses
            int rtCode = _command.SetResponse(rtResponse);
            byte[] rtData = _command.Response;
            return rtData;
        }

        public byte[] ReadDeviceBlock(string iDeviceName, int iSize, int[] oData)
        {
            EDeviceCode type;
            int addr;
            GetDeviceCode(iDeviceName, out type, out addr);
            return ReadDeviceBlock(type, addr, iSize, oData);
        }
        public byte[] ReadDeviceBlock(EDeviceCode iType, int iAddress, int iSize, int[] oData)
        {

            EDeviceCode type = iType;
            int addr = iAddress;
            List<byte> data;
            byte[] sdCommand;
            int length;

            switch (CommandFrame)
            {
                case EFrame.MC3E:
                    data = new List<byte>(6)
                    {
                        (byte) addr
                      , (byte) (addr >> 8)
                      , (byte) (addr >> 16)
                      , (byte) type
                      , (byte) iSize
                      , (byte) (iSize >> 8)
                    };
                    sdCommand = _command.SetCommandMC3E(0x0401, 0x0000, data.ToArray());
                    length = 11;
                    break;
                case EFrame.MC4E:
                    data = new List<byte>(6)
                    {
                        (byte) addr
                      , (byte) (addr >> 8)
                      , (byte) (addr >> 16)
                      , (byte) type
                      , (byte) iSize
                      , (byte) (iSize >> 8)
                    };
                    sdCommand = _command.SetCommandMC4E(0x0401, 0x0000, data.ToArray());
                    length = 15;
                    break;
                case EFrame.MC1E:
                    data = new List<byte>(6)
                    {
                          (byte) addr
                      , (byte) (addr >> 8)
                      , (byte) (addr >> 16)
                      , (byte) (addr >> 24)
                      , 0x20
                      , 0x44
                      , (byte) iSize
                      , 0x00
                    };
                    sdCommand = _command.SetCommandMC1E(0x01, data.ToArray());
                    length = 2;
                    break;
                default:
                    throw new Exception("Message frame not supported");
            }

            byte[] rtResponse = tryExecution(sdCommand, length);
            //TEST verify read responses
            int rtCode = _command.SetResponse(rtResponse);
            byte[] rtData = _command.Response;
            for (int i = 0; i < iSize; ++i)
            {
                oData[i] = (rtCode == 0) ? BitConverter.ToInt16(rtData, i * 2) : 0;
            }
            return rtData;
        }
        public byte[] ReadDeviceBlock(EDeviceCode iType, int iAddress, int devicePoints)
        {
            int iSize = devicePoints;
            EDeviceCode type = iType;
            int addr = iAddress;
            List<byte> data;
            byte[] sdCommand;
            int length;

            switch (CommandFrame)
            {
                case EFrame.MC3E:
                    data = new List<byte>(6)
                    {
                        (byte) addr
                      , (byte) (addr >> 8)
                      , (byte) (addr >> 16)
                      , (byte) type
                      , (byte) iSize
                      , (byte) (iSize >> 8)
                    };
                    sdCommand = _command.SetCommandMC3E(0x0401, 0x0000, data.ToArray());
                    length = 11;
                    break;
                case EFrame.MC4E:
                    data = new List<byte>(6)
                    {
                        (byte) addr
                      , (byte) (addr >> 8)
                      , (byte) (addr >> 16)
                      , (byte) type
                      , (byte) iSize
                      , (byte) (iSize >> 8)
                    };
                    sdCommand = _command.SetCommandMC4E(0x0401, 0x0000, data.ToArray());
                    length = 15;
                    break;
                case EFrame.MC1E:
                    data = new List<byte>(6)
                    {
                          (byte) addr
                      , (byte) (addr >> 8)
                      , (byte) (addr >> 16)
                      , (byte) (addr >> 24)
                      , 0x20
                      , 0x44
                      , (byte) iSize
                      , 0x00
                    };
                    sdCommand = _command.SetCommandMC1E(0x01, data.ToArray());
                    length = 2;
                    break;
                default:
                    throw new Exception("Message frame not supported");
            }

            byte[] rtResponse = tryExecution(sdCommand, length);
            //TEST verify read responses
            int rtCode = _command.SetResponse(rtResponse);
            //Debug.WriteLine($"RET CODE : {rtCode}");
            byte[] rtData = _command.Response;
            return rtData;
        }

        public int ReadDeviceBlock(EDeviceCode iType, int iAddress, int devicePoints, ref byte[] buffer)
        {
            int iSize = devicePoints;
            EDeviceCode type = iType;
            int addr = iAddress;
            List<byte> data;
            byte[] sdCommand;
            int length;

            switch (CommandFrame)
            {
                case EFrame.MC3E:
                    data = new List<byte>(6)
                    {
                        (byte) addr
                      , (byte) (addr >> 8)
                      , (byte) (addr >> 16)
                      , (byte) type
                      , (byte) iSize
                      , (byte) (iSize >> 8)
                    };
                    sdCommand = _command.SetCommandMC3E(0x0401, 0x0000, data.ToArray());
                    length = 11;
                    break;
                case EFrame.MC4E:
                    data = new List<byte>(6)
                    {
                        (byte) addr
                      , (byte) (addr >> 8)
                      , (byte) (addr >> 16)
                      , (byte) type
                      , (byte) iSize
                      , (byte) (iSize >> 8)
                    };
                    sdCommand = _command.SetCommandMC4E(0x0401, 0x0000, data.ToArray());
                    length = 15;
                    break;
                case EFrame.MC1E:
                    data = new List<byte>(6)
                    {
                          (byte) addr
                      , (byte) (addr >> 8)
                      , (byte) (addr >> 16)
                      , (byte) (addr >> 24)
                      , 0x20
                      , 0x44
                      , (byte) iSize
                      , 0x00
                    };
                    sdCommand = _command.SetCommandMC1E(0x01, data.ToArray());
                    length = 2;
                    break;
                default:
                    throw new Exception("Message frame not supported");
            }

            byte[] rtResponse = tryExecution(sdCommand, length);
            //TEST verify read responses
            int rtCode = _command.SetResponse(rtResponse);
            //Debug.WriteLine($"RET CODE : {rtCode}");
            byte[] rtData = _command.Response;
            buffer = rtData;

            return rtCode;
        }

        public async Task<int> SetDeviceAsync(string iDeviceName, int iData)
        {
            EDeviceCode type;
            int addr;
            GetDeviceCode(iDeviceName, out type, out addr);
            return await SetDeviceAsync(type, addr, iData);
        }
        public async Task<int> SetDeviceAsync(EDeviceCode iType, int iAddress, int iData)
        {

            EDeviceCode type = iType;
            int addr = iAddress;
            var data = new List<byte>(6)
                    {
                        (byte) addr
                      , (byte) (addr >> 8)
                      , (byte) (addr >> 16)
                      , (byte) type
                      , 0x01
                      , 0x00
                      , (byte) iData
                      , (byte) (iData >> 8)
                    };
            byte[] sdCommand = _command.SetCommandMC3E(0x1401, 0x0000, data.ToArray());
            int length = (_command.FrameType == EFrame.MC3E) ? 11 : 15;
            byte[] rtResponse = await tryExecutionAsync(sdCommand, length);
            int rtCode = _command.SetResponse(rtResponse);
            return rtCode;
        }
        public async Task<int> GetDeviceAsync(string iDeviceName)
        {
            EDeviceCode type;
            int addr;
            GetDeviceCode(iDeviceName, out type, out addr);
            return await GetDeviceAsync(type, addr);
        }
        public async Task<int> GetDeviceAsync(EDeviceCode iType, int iAddress)
        {
            EDeviceCode type = iType;
            int addr = iAddress;
            var data = new List<byte>(6)
                    {
                        (byte) addr
                      , (byte) (addr >> 8)
                      , (byte) (addr >> 16)
                      , (byte) type
                      , 0x01
                      , 0x00
                    };
            byte[] sdCommand = _command.SetCommandMC3E(0x0401, 0x0000, data.ToArray());
            int length = (_command.FrameType == EFrame.MC3E) ? 11 : 15;
            ; byte[] rtResponse = await tryExecutionAsync(sdCommand, length);
            int rtCode = _command.SetResponse(rtResponse);
            if (0 < rtCode)
            {
                this.Device = 0;
            }
            else
            {
                byte[] rtData = _command.Response;
                this.Device = BitConverter.ToInt16(rtData, 0);
            }
            return rtCode;
        }
        protected abstract int Connect();
        protected abstract void Disconnect();
        protected abstract Task<byte[]> ExecuteAsync(byte[] iCommand);
        protected abstract byte[] ExecuteRead(byte[] iCommand);
        protected abstract byte[] ExecuteWrite(byte[] iCommand);

        private async Task<byte[]> tryExecutionAsync(byte[] iCommand, int minlength)
        {

            byte[] rtResponse;
            int tCount = 10;
            do
            {
                rtResponse = await ExecuteAsync(iCommand);
                --tCount;
                if (tCount < 0)
                {
                    throw new Exception("A correct value cannot be obtained from the PLC.");
                }
            } while (_command.IsIncorrectResponse(ref rtResponse, minlength));
            return rtResponse;
        }

        private byte[] tryExecution(byte[] iCommand, int minlength)
        {
            byte[] rtResponse;
            int tCount = 10;
            do
            {
                rtResponse = ExecuteRead(iCommand);
                if (rtResponse.Length == 0)
                {
                    Thread.Sleep(5000);
                    continue;
                }

                --tCount;
                if (tCount < 0)
                {
                    throw new Exception("A correct value cannot be obtained from the PLC.");
                }
            } while (_command.IsIncorrectResponse(ref rtResponse, minlength));
            return rtResponse;
        }

        private byte[] tryWriteExecution(byte[] iCommand, int minlength)
        {

            byte[] rtResponse;
            int tCount = 10;
            do
            {
                rtResponse = ExecuteWrite(iCommand);
                if (rtResponse.Length == 0)
                {
                    Thread.Sleep(5000);
                    continue;
                }
                --tCount;
                if (tCount < 0)
                {
                    throw new Exception("A correct value cannot be obtained from the PLC.");
                }
            } while (_command.IsIncorrectWriteResponse(ref rtResponse, minlength));
            return rtResponse;
        }

        public static Defines.EDeviceCode GetDeviceType(string s)
        {
            return (s == "M") ? Defines.EDeviceCode.M :
                   (s == "SM") ? Defines.EDeviceCode.SM :
                   (s == "L") ? Defines.EDeviceCode.L :
                   (s == "F") ? Defines.EDeviceCode.F :
                   (s == "V") ? Defines.EDeviceCode.V :
                   (s == "S") ? Defines.EDeviceCode.S :
                   (s == "X") ? Defines.EDeviceCode.X :
                   (s == "Y") ? Defines.EDeviceCode.Y :
                   (s == "B") ? Defines.EDeviceCode.B :
                   (s == "SB") ? Defines.EDeviceCode.SB :
                   (s == "DX") ? Defines.EDeviceCode.DX :
                   (s == "DY") ? Defines.EDeviceCode.DY :
                   (s == "D") ? Defines.EDeviceCode.D :
                   (s == "SD") ? Defines.EDeviceCode.SD :
                   (s == "R") ? Defines.EDeviceCode.R :
                   (s == "ZR") ? Defines.EDeviceCode.ZR :
                   (s == "W") ? Defines.EDeviceCode.W :
                   (s == "SW") ? Defines.EDeviceCode.SW :
                   (s == "TC") ? Defines.EDeviceCode.TC :
                   (s == "TS") ? Defines.EDeviceCode.TS :
                   (s == "TN") ? Defines.EDeviceCode.TN :
                   (s == "CC") ? Defines.EDeviceCode.CC :
                   (s == "CS") ? Defines.EDeviceCode.CS :
                   (s == "CN") ? Defines.EDeviceCode.CN :
                   (s == "SC") ? Defines.EDeviceCode.SC :
                   (s == "SS") ? Defines.EDeviceCode.SS :
                   (s == "SN") ? Defines.EDeviceCode.SN :
                   (s == "Z") ? Defines.EDeviceCode.Z :
                   (s == "TT") ? Defines.EDeviceCode.TT :
                   (s == "TM") ? Defines.EDeviceCode.TM :
                   (s == "CT") ? Defines.EDeviceCode.CT :
                   (s == "CM") ? Defines.EDeviceCode.CM :
                   (s == "A") ? Defines.EDeviceCode.A :
                                 Defines.EDeviceCode.Max;
        }

        public static bool IsBitDevice(Defines.EDeviceCode type)
        {
            return !((type == Defines.EDeviceCode.D)
                  || (type == Defines.EDeviceCode.SD)
                  || (type == Defines.EDeviceCode.Z)
                  || (type == Defines.EDeviceCode.ZR)
                  || (type == Defines.EDeviceCode.R)
                  || (type == Defines.EDeviceCode.W));
        }

        public static bool IsHexDevice(Defines.EDeviceCode type)
        {
            return (type == Defines.EDeviceCode.X)
                || (type == Defines.EDeviceCode.Y)
                || (type == Defines.EDeviceCode.B)
                || (type == Defines.EDeviceCode.W);
        }

        // ====================================================================================
        public static void GetDeviceCode(string iDeviceName, out Defines.EDeviceCode oType, out int oAddress)
        {
            string s = iDeviceName.ToUpper();
            string strAddress;

            // extract one character
            string strType = s.Substring(0, 1);
            switch (strType)
            {
                case "A":
                case "B":
                case "D":
                case "F":
                case "L":
                case "M":
                case "R":
                case "V":
                case "W":
                case "X":
                case "Y":
                    // The second and subsequent characters should be numbers, so convert them.
                    strAddress = s.Substring(1);
                    break;
                case "Z":
                    // take out one more character
                    strType = s.Substring(0, 2);
                    // For file registers: 2
                    // For index register: 1
                    strAddress = s.Substring(strType.Equals("ZR") ? 2 : 1);
                    break;
                case "C":
                    // take out one more character
                    strType = s.Substring(0, 2);
                    switch (strType)
                    {
                        case "CC":
                        case "CM":
                        case "CN":
                        case "CS":
                        case "CT":
                            strAddress = s.Substring(2);
                            break;
                        default:
                            throw new Exception("Invalid format.");
                    }
                    break;
                case "S":
                    // take out one more character
                    strType = s.Substring(0, 2);
                    switch (strType)
                    {
                        case "SD":
                        case "SM":
                            strAddress = s.Substring(2);
                            break;
                        default:
                            throw new Exception("Invalid format.");
                    }
                    break;
                case "T":
                    // take out one more character
                    strType = s.Substring(0, 2);
                    switch (strType)
                    {
                        case "TC":
                        case "TM":
                        case "TN":
                        case "TS":
                        case "TT":
                            strAddress = s.Substring(2);
                            break;
                        default:
                            throw new Exception("Invalid format.");
                    }
                    break;
                default:
                    throw new Exception("Invalid format.");
            }

            oType = GetDeviceType(strType);
            oAddress = IsHexDevice(oType) ? Convert.ToInt32(strAddress, BLOCK_SIZE) :
                                            Convert.ToInt32(strAddress);
        }

       
    }

}

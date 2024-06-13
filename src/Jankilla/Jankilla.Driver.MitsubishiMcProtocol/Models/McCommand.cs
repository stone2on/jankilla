using Jankilla.Driver.MitsubishiMcProtocol.Defines;
using System;
using System.Collections.Generic;


namespace Jankilla.Driver.MitsubishiMcProtocol.Models
{
    class McCommand
    {
        public EFrame FrameType { get; private set; }  
        public byte[] Response { get; private set; }    

        private uint _serialNumber { get; set; }  
        private uint _networkNumber { get; set; } 
        private uint _pcNumber { get; set; }     
        private uint _ioNumber { get; set; }      
        private uint _channelNumber { get; set; } 
        private uint _cpuTimer { get; set; }     
        private int _resultCode { get; set; }    
                                                     
        public McCommand(EFrame iFrame)
        {
            FrameType = iFrame;

            _serialNumber = 0x0001u;
            _networkNumber = 0x0000u;
            _pcNumber = 0x00FFu;
            _ioNumber = 0x03FFu;
            _channelNumber = 0x0000u;
            _cpuTimer = 0x0010u;
        }
        // ================================================================================
        public byte[] SetCommandMC1E(byte Subheader, byte[] iData)
        {
            List<byte> ret = new List<byte>(iData.Length + 4);
            ret.Add(Subheader);
            ret.Add((byte)this._pcNumber);
            ret.Add((byte)_cpuTimer);
            ret.Add((byte)(_cpuTimer >> 8));
            ret.AddRange(iData);
            return ret.ToArray();
        }
        public byte[] SetCommandMC3E(uint iMainCommand, uint iSubCommand, byte[] iData)
        {
            var dataLength = (uint)(iData.Length + 6);
            List<byte> ret = new List<byte>(iData.Length + 20);
            uint frame = 0x0050u;
            ret.Add((byte)frame);
            ret.Add((byte)(frame >> 8));

            ret.Add((byte)_networkNumber);

            ret.Add((byte)_pcNumber);

            ret.Add((byte)_ioNumber);
            ret.Add((byte)(_ioNumber >> 8));
            ret.Add((byte)_channelNumber);
            ret.Add((byte)dataLength);
            ret.Add((byte)(dataLength >> 8));


            ret.Add((byte)_cpuTimer);
            ret.Add((byte)(_cpuTimer >> 8));
            ret.Add((byte)iMainCommand);
            ret.Add((byte)(iMainCommand >> 8));
            ret.Add((byte)iSubCommand);
            ret.Add((byte)(iSubCommand >> 8));

            ret.AddRange(iData);
            return ret.ToArray();
        }
        public byte[] SetCommandMC4E(uint iMainCommand, uint iSubCommand, byte[] iData)
        {
            var dataLength = (uint)(iData.Length + 6);
            var ret = new List<byte>(iData.Length + 20);
            uint frame = 0x0054u;
            ret.Add((byte)frame);
            ret.Add((byte)(frame >> 8));
            ret.Add((byte)_serialNumber);
            ret.Add((byte)(_serialNumber >> 8));
            ret.Add(0x00);
            ret.Add(0x00);
            ret.Add((byte)_networkNumber);
            ret.Add((byte)_pcNumber);
            ret.Add((byte)_ioNumber);
            ret.Add((byte)(_ioNumber >> 8));
            ret.Add((byte)_channelNumber);
            ret.Add((byte)dataLength);
            ret.Add((byte)(dataLength >> 8));
            ret.Add((byte)_cpuTimer);
            ret.Add((byte)(_cpuTimer >> 8));
            ret.Add((byte)iMainCommand);
            ret.Add((byte)(iMainCommand >> 8));
            ret.Add((byte)iSubCommand);
            ret.Add((byte)(iSubCommand >> 8));

            ret.AddRange(iData);
            return ret.ToArray();
        }
        // ================================================================================
        public int SetResponse(byte[] iResponse)
        {
            int min;
            switch (FrameType)
            {
                case EFrame.MC1E:
                    min = 2;
                    if (min <= iResponse.Length)
                    {
                        //There is a subheader, end code and data.                                    

                        _resultCode = (int)iResponse[min - 2];
                        Response = new byte[iResponse.Length - 2];
                        Buffer.BlockCopy(iResponse, min, Response, 0, Response.Length);
                    }
                    break;
                case EFrame.MC3E:
                    min = 11;
                    if (min <= iResponse.Length)
                    {
                        var btCount = new[] { iResponse[min - 4], iResponse[min - 3] };
                        var btCode = new[] { iResponse[min - 2], iResponse[min - 1] };
                        int rsCount = BitConverter.ToUInt16(btCount, 0);
                        _resultCode = BitConverter.ToUInt16(btCode, 0);
                        Response = new byte[rsCount - 2];
                        Buffer.BlockCopy(iResponse, min, Response, 0, Response.Length);
                    }
                    break;
                case EFrame.MC4E:
                    min = 15;
                    if (min <= iResponse.Length)
                    {
                        var btCount = new[] { iResponse[min - 4], iResponse[min - 3] };
                        var btCode = new[] { iResponse[min - 2], iResponse[min - 1] };
                        int rsCount = BitConverter.ToUInt16(btCount, 0);
                        _resultCode = BitConverter.ToUInt16(btCode, 0);
                        Response = new byte[rsCount - 2];
                        Buffer.BlockCopy(iResponse, min, Response, 0, Response.Length);
                    }
                    break;
                default:
                    throw new Exception("Frame type not supported.");

            }
            return _resultCode;
        }
        // ================================================================================

        private List<byte> _bufferList = null;
        public bool IsIncorrectResponse(ref byte[] iResponse, int minLength)
        {
            if (iResponse == null || iResponse.Length <= minLength)
            {
                return true;
            }

            //TEST add 1E frame
            switch (this.FrameType)
            {
                case EFrame.MC1E:
                    return ((iResponse.Length < minLength));

                case EFrame.MC3E:
                case EFrame.MC4E:
                    byte[] btCount;
                    byte[] btCode;
                    int rsCount;
                    ushort rsCode;

                    if (iResponse[0] == 208)
                    {
                        btCount = new[] { iResponse[minLength - 4], iResponse[minLength - 3] };
                        btCode = new[] { iResponse[minLength - 2], iResponse[minLength - 1] };
                        rsCount = BitConverter.ToUInt16(btCount, 0) - 2;
                        rsCode = BitConverter.ToUInt16(btCode, 0);

                        if (rsCode == 0 && rsCount > (iResponse.Length - minLength)) {
                            // init to buffer
                            _bufferList = new List<byte>(iResponse);
                            return true;
                        }

                        return rsCode == 0 && rsCount != (iResponse.Length - minLength);
                    }

                    if (_bufferList == null)
                    {
                        return true;
                    }

                    _bufferList.AddRange(iResponse);

                    btCount = new[] { _bufferList[minLength - 4], _bufferList[minLength - 3] };
                    btCode = new[] { _bufferList[minLength - 2], _bufferList[minLength - 1] };
                    rsCount = BitConverter.ToUInt16(btCount, 0) - 2;
                    rsCode = BitConverter.ToUInt16(btCode, 0);

                    if (rsCode == 0 && rsCount == (_bufferList.Count - minLength))
                    {
                        iResponse = _bufferList.ToArray();
                        _bufferList.Clear();
                        _bufferList = null;

                        return false;
                    }

                    return true;

                default:
                    throw new Exception("Type Not supported");

            }
        }

        public bool IsIncorrectWriteResponse(ref byte[] iResponse, int minLength)
        {
            if (iResponse == null || iResponse.Length < minLength)
            {
                return true;
            }

            //TEST add 1E frame
            switch (this.FrameType)
            {
                case EFrame.MC1E:
                    return ((iResponse.Length < minLength));

                case EFrame.MC3E:
                case EFrame.MC4E:
                    byte[] btCount;
                    byte[] btCode;
                    int rsCount;
                    ushort rsCode;

                    btCount = new[] { iResponse[minLength - 4], iResponse[minLength - 3] };
                    btCode = new[] { iResponse[minLength - 2], iResponse[minLength - 1] };
                    rsCount = BitConverter.ToUInt16(btCount, 0) - 2;
                    rsCode = BitConverter.ToUInt16(btCode, 0);

                    if ((rsCode == 0 && rsCount != (iResponse.Length - minLength)) == true)
                    {
                        Console.WriteLine($"RESP LEN : {iResponse.Length}");
                    }

                    return rsCode == 0 && rsCount != (iResponse.Length - minLength);

                default:
                    throw new Exception("Type Not supported");

            }
        }
    }
}

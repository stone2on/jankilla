using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Jankilla.Driver.MitsubishiMcProtocol.Defines;
using Microsoft.Win32.SafeHandles;

namespace Jankilla.Driver.MitsubishiMcProtocol.Models
{
    public class McProtocolTcp : McProtocolBase
    {
        public override bool Connected
        {
            get
            {
                return _client.Connected;
            }
        }
        private TcpClient _client;
        private NetworkStream _stream;

        [Flags]
        public enum NetworkEvent
        {
            NONE = 0x0,
            FD_CLOSE = 0x20,
        }

        //private Thread _threadClose;

        public McProtocolTcp() : this(string.Empty, 0, EFrame.MC3E) { }
        public McProtocolTcp(string iHostName, int iPortNumber, EFrame frame)
            : base(iHostName, iPortNumber, frame)
        {
            CommandFrame = frame;

            _client = new TcpClient();
        }

        protected override int Connect()
        {
            if (!_client.Connected)
            {
                var ka = new List<byte>(sizeof(uint) * 3);
                ka.AddRange(BitConverter.GetBytes(1u));
                ka.AddRange(BitConverter.GetBytes(1000u));
                ka.AddRange(BitConverter.GetBytes(500u));

                _client.Client.IOControl(IOControlCode.KeepAliveValues, ka.ToArray(), null);
                _client.Connect(HostName, PortNumber);

                _stream = _client.GetStream();
            }

            return 0;
        }

        protected override void Disconnect()
        {
            TcpClient c = _client;
            if (c.Connected)
            {
                c.Close();
            }
        }

        protected override async Task<byte[]> ExecuteAsync(byte[] iCommand)
        {
            NetworkStream ns = _stream;
            ns.Write(iCommand, 0, iCommand.Length);
            ns.Flush();

            using (var ms = new MemoryStream())
            {
                var buff = new byte[256];
                do
                {
                    int sz = await ns.ReadAsync(buff, 0, buff.Length);
                    if (sz == 0)
                    {
                        throw new Exception("disconnected");
                    }
                    ms.Write(buff, 0, sz);
                } 
                while (ns.DataAvailable);

                return ms.ToArray();
            }

        }

        protected override byte[] ExecuteRead(byte[] iCommand)
        {

            NetworkStream ns = _stream;
            //ns.Flush();
            ns.Write(iCommand, 0, iCommand.Length);
            ns.Flush();
            Thread.Sleep(10);

            using (var ms = new MemoryStream())
            {
                var buff = new byte[256];
                do
                {
                    int sz = ns.Read(buff, 0, buff.Length);
                    if (sz == 0)
                    {
                        continue;
                        //throw new Exception("disconnected");
                    }
                    ms.Write(buff, 0, sz);
                }
                while (ns.DataAvailable);

                //Console.WriteLine($"[READ] Stream Read : {ms.ToArray().Length}");
                return ms.ToArray();
            }

        }

        protected override byte[] ExecuteWrite(byte[] iCommand)
        {
            NetworkStream ns = _stream;
            //ns.Flush();
            ns.Write(iCommand, 0, iCommand.Length);
            ns.Flush();
            Thread.Sleep(10);

            using (var ms = new MemoryStream())
            {
                var buff = new byte[256];
                do
                {
                    int sz = ns.Read(buff, 0, buff.Length);
                    if (sz == 0)
                    {
                        continue;
                        //throw new Exception("disconnected");
                    }
                    ms.Write(buff, 0, sz);
                }
                while (ns.DataAvailable);

                //Console.WriteLine($"[WRITE] Stream Read : {ms.ToArray().Length}");
                return ms.ToArray();
            }
        }
    }
}

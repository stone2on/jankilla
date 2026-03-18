using Jankilla.Driver.Mitsubishi.McProtocol.Defines;
using Jankilla.Driver.Mitsubishi.McProtocol.Interfaces;
using System;
using System.Threading.Tasks;

namespace Jankilla.Driver.Mitsubishi.McProtocol.Models
{
  
    public class PLCData<T>
    {
        private EDeviceCode _deviceType;
        private int _address;
        private int _length;
        private int _bufferLength; //Length in bytes
        private byte[] _buffer;

        private IPLC _plc;

        public PLCData(IPLC plc, EDeviceCode deviceType, int address, int length)
        {
            _plc = plc;

            this._deviceType = deviceType;
            this._address = address;

            string t = typeof(T).Name;
            switch (t)
            {
                case "Boolean":
                    this._bufferLength = (length / 16 + (length % 16 > 0 ? 1 : 0)) * 2;
                    this._length = length;
                    break;
                case "Int32":
                    this._bufferLength = 4 * length;
                    this._length = length * 2;
                    break;
                case "Int16":
                    this._bufferLength = 2 * length;
                    this._length = length;
                    break;
                case "UInt16":
                    this._bufferLength = 2 * length;
                    this._length = length;
                    break;
                case "UInt32":
                    this._bufferLength = 4 * length;
                    this._length = length * 2;
                    break;
                case "Single":
                    this._bufferLength = 4 * length;
                    this._length = length * 2;
                    break;
                case "Double":
                    this._bufferLength = 8 * length;
                    this._length = length * 4;
                    break;
                case "Char":
                    this._bufferLength = length;
                    this._length = length;
                    break;
                default:
                    throw new Exception("Type not supported by PLC.");
            }
            this._buffer = new byte[this._bufferLength];

        }
        public T this[int i]
        {
            get
            {
                Defines.Union u = new Union();
                string t = typeof(T).Name;
                switch (t)
                {
                    case "Boolean":
                        return (T)Convert.ChangeType(((this._buffer[i / 8] >> (i % 8)) % 2 == 1), typeof(T));
                    case "Int32":
                        u.a = this._buffer[i * 4];
                        u.b = this._buffer[i * 4 + 1];
                        u.c = this._buffer[i * 4 + 2];
                        u.d = this._buffer[i * 4 + 3];
                        return (T)Convert.ChangeType(u.DINT, typeof(T));
                    case "Int16":
                        u.a = this._buffer[i * 2];
                        u.b = this._buffer[i * 2 + 1];
                        return (T)Convert.ChangeType(u.INT, typeof(T));
                    case "UInt16":
                        u.a = this._buffer[i * 2];
                        u.b = this._buffer[i * 2 + 1];
                        return (T)Convert.ChangeType(u.UINT, typeof(T));
                    case "UInt32":
                        u.a = this._buffer[i * 4];
                        u.b = this._buffer[i * 4 + 1];
                        u.c = this._buffer[i * 4 + 2];
                        u.d = this._buffer[i * 4 + 3];
                        return (T)Convert.ChangeType(u.UDINT, typeof(T));
                    case "Single":
                        u.a = this._buffer[i * 4];
                        u.b = this._buffer[i * 4 + 1];
                        u.c = this._buffer[i * 4 + 2];
                        u.d = this._buffer[i * 4 + 3];
                        return (T)Convert.ChangeType(u.REAL, typeof(T));
                    case "Char":
                        return (T)Convert.ChangeType(this.ToString()[i], typeof(T));
                    default:
                        throw new Exception("Type not recognized.");
                }
            }
            set
            {
                Union u = new Union();
                string t = typeof(T).Name;
                switch (t)
                {
                    case "Boolean":
                        bool arg = Convert.ToBoolean(value);
                        if (arg && (this._buffer[i / 8] >> (i % 8)) % 2 == 0)
                            this._buffer[i / 8] += (byte)(1 << (i % 8));
                        else if (!arg && (this._buffer[i / 8] >> (i % 8)) % 2 == 1)
                            this._buffer[i / 8] -= (byte)(1 << (i % 8));
                        return;
                    case "Int32":
                        u.DINT = Convert.ToInt32(value);
                        this._buffer[i * 4] = u.a;
                        this._buffer[i * 4 + 1] = u.b;
                        this._buffer[i * 4 + 2] = u.c;
                        this._buffer[i * 4 + 3] = u.d;
                        return;
                    case "Int16":
                        u.INT = Convert.ToInt16(value);
                        this._buffer[i * 2] = u.a;
                        this._buffer[i * 2 + 1] = u.b;
                        return;
                    case "UInt32":
                        u.UDINT = Convert.ToUInt32(value);
                        this._buffer[i * 4] = u.a;
                        this._buffer[i * 4 + 1] = u.b;
                        this._buffer[i * 4 + 2] = u.c;
                        this._buffer[i * 4 + 3] = u.d;
                        return;
                    case "UInt16":
                        u.UINT = Convert.ToUInt16(value);
                        this._buffer[i * 2] = u.a;
                        this._buffer[i * 2] = u.b;
                        return;
                    case "Single":
                        u.REAL = Convert.ToSingle(value);
                        this._buffer[i * 4] = u.a;
                        this._buffer[i * 4 + 1] = u.b;
                        this._buffer[i * 4 + 2] = u.c;
                        this._buffer[i * 4 + 3] = u.d;
                        return;
                    default:
                        throw new Exception("Type not recognized.");
                }
            }
        }

        public async Task WriteDataAsync()
        {
            await _plc.WriteDeviceBlockAsync(_deviceType, _address, _length, _buffer);
        }
        public async Task ReadDataAsync()
        {
            _buffer = await _plc.ReadDeviceBlockAsync(_deviceType, _address, _length);
        }

        public void WriteData()
        {
            _plc.WriteDeviceBlock(_deviceType, _address, _length, _buffer);
        }
        public void ReadData()
        {
            _buffer = _plc.ReadDeviceBlock(_deviceType, _address, _length);
        }

    }

 
}

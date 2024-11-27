using Jankilla.Driver.Mitsubishi.McProtocol.Defines;
using System;
using System.Threading.Tasks;

namespace Jankilla.Driver.Mitsubishi.McProtocol.Interfaces
{
    public interface IPLC : IDisposable
    {
        bool Connected { get; }
        int Open();
        int Close();
        Task<int> SetBitDeviceAsync(string iDeviceName, int iSize, int[] iData);
        Task<int> SetBitDeviceAsync(EDeviceCode iType, int iAddress, int iSize, int[] iData);
        Task<int> GetBitDeviceAsync(string iDeviceName, int iSize, int[] oData);
        Task<int> GetBitDeviceAsync(EDeviceCode iType, int iAddress, int iSize, int[] oData);
        Task<int> WriteDeviceBlockAsync(string iDeviceName, int iSize, int[] iData);
        Task<int> WriteDeviceBlockAsync(EDeviceCode iType, int iAddress, int iSize, int[] iData);
        Task<int> WriteDeviceBlockAsync(EDeviceCode iType, int iAddress, int iSize, byte[] bData);
        Task<byte[]> ReadDeviceBlockAsync(string iDeviceName, int iSize, int[] oData);
        Task<byte[]> ReadDeviceBlockAsync(EDeviceCode iType, int iAddress, int iSize, int[] oData);
        Task<byte[]> ReadDeviceBlockAsync(EDeviceCode iType, int iAddress, int iSize);

        int WriteDeviceBlock(string iDeviceName, int iSize, int[] iData);
        int WriteDeviceBlock(EDeviceCode iType, int iAddress, int iSize, int[] iData);
        int WriteDeviceBlock(EDeviceCode iType, int iAddress, int iSize, byte[] bData);

        byte[] ReadDeviceBlock(string iDeviceName, int iSize, int[] oData);
        byte[] ReadDeviceBlock(EDeviceCode iType, int iAddress, int iSize, int[] oData);
        byte[] ReadDeviceBlock(EDeviceCode iType, int iAddress, int iSize);
        int ReadDeviceBlock(EDeviceCode iType, int iAddress, int devicePoints, ref byte[] buffer);

        Task<int> SetDeviceAsync(string iDeviceName, int iData);
        Task<int> SetDeviceAsync(EDeviceCode iType, int iAddress, int iData);
        Task<int> GetDeviceAsync(string iDeviceName);
        Task<int> GetDeviceAsync(EDeviceCode iType, int iAddress);
    }
}

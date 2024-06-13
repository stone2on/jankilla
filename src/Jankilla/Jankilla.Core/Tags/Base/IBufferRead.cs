namespace Jankilla.Core.Contracts.Tags.Base
{
    internal interface IBufferRead
    {
        void Read(short[] buffer, int startIndex);
        void Read(byte[] buffer, int startIndex);
    }
}

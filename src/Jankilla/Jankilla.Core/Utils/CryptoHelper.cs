using Jankilla.Core.Contracts.Tags;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;

namespace Jankilla.Core.Utils
{
    public static class CryptoHelper
    {
        private static readonly byte[] Key;
        private static readonly byte[] IV;
        private static byte[] MacAddr;
        private const string PATH = "license.dat";

        public static bool IsActivated
        {
            get
            {
                if (MacAddr == null || MacAddr.Length != 6)
                {
                    return false;
                }
                return Tag.CompareByteArrays(GetMacAddress().GetAddressBytes(), MacAddr);
            }
        }

        static CryptoHelper()
        {
            if (File.Exists(PATH))
            {
                byte[] combined = File.ReadAllBytes(PATH);
                combined = Convert.FromBase64String(Encoding.ASCII.GetString(combined));

                Key = new byte[32];
                IV = new byte[16];
                MacAddr = new byte[6];

                Buffer.BlockCopy(combined, 0, Key, 0, Key.Length);
                Buffer.BlockCopy(combined, Key.Length, IV, 0, IV.Length);
                Buffer.BlockCopy(combined, Key.Length + IV.Length, MacAddr, 0, MacAddr.Length);

                return;
            }

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.GenerateKey();
                aesAlg.GenerateIV();
                aesAlg.Padding = PaddingMode.PKCS7;

                Key = aesAlg.Key;
                IV = aesAlg.IV;
                MacAddr = new byte[6];

                byte[] combined = new byte[Key.Length + IV.Length + MacAddr.Length];

                Buffer.BlockCopy(Key, 0, combined, 0, Key.Length);
                Buffer.BlockCopy(IV, 0, combined, Key.Length, IV.Length);
                Buffer.BlockCopy(MacAddr, 0, combined, Key.Length + IV.Length, MacAddr.Length);

                var pw = Convert.ToBase64String(combined);

                File.WriteAllText(PATH, pw, Encoding.ASCII);
            }
        }

        public static string GenerateUserKey()
        {
            var mac = GetMacAddress();
            return Encrypt(mac.ToString());
        }

        public static string GenerateSerialKey(string userKey)
        {
            if (userKey.IsBase64() == false)
            {
                return string.Empty;
            }

            var decrypted = Decrypt(userKey);
            var reversed = new string(decrypted.Reverse().ToArray());

            return Encrypt(reversed);
        }

        public static bool ActivateLicense(string userKey, string serialKey)
        {
            if (userKey.IsBase64() == false || serialKey.IsBase64() == false)
            {
                return false;
            }

            string targetMacAddress = Decrypt(userKey);
            string decryptedSerialKey = Decrypt(serialKey);


            var reversedSerialKey = new string(decryptedSerialKey.Reverse().ToArray());
            bool bActivated = targetMacAddress == reversedSerialKey;

            if (bActivated)
            {
                MacAddr = GetMacAddress().GetAddressBytes();

                byte[] combined = new byte[Key.Length + IV.Length + MacAddr.Length];
                Buffer.BlockCopy(Key, 0, combined, 0, Key.Length);
                Buffer.BlockCopy(IV, 0, combined, Key.Length, IV.Length);
                Buffer.BlockCopy(MacAddr, 0, combined, Key.Length + IV.Length, MacAddr.Length);

                var pw = Convert.ToBase64String(combined);

                File.WriteAllText(PATH, pw, Encoding.ASCII);
            }

            return bActivated;
        }


        public static string Encrypt(string plainText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }

                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public static string Decrypt(string cipherText)
        {

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt, Encoding.ASCII))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        public static PhysicalAddress GetMacAddress()
        {
            var macAddress = NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .Select(nic => nic.GetPhysicalAddress())
                .FirstOrDefault();

            Debug.Assert(macAddress != null);
            Debug.Assert(macAddress.GetAddressBytes() != null);
            Debug.Assert(macAddress.GetAddressBytes().Length == 6);

            return macAddress;
        }

        
    }
}

﻿using System.Security.Cryptography;

namespace SharpEnd.Security
{
    public static class AesCryptograph
    {
        private static readonly byte[] sUserKey = new byte[32]
        {
            0xA6, 0x00, 0x00, 0x00,0x80, 0x00, 0x00, 0x00,0x73, 0x00, 0x00, 0x00,0x38,
            0x00, 0x00, 0x00,0xCD, 0x00, 0x00, 0x00,0x34, 0x00, 0x00, 0x00,0x3B, 0x00, 0x00, 0x00,0xE9, 0x00, 0x00, 0x00
        };

        private static ICryptoTransform sTransformer;

        static AesCryptograph()
        {
            RijndaelManaged aes = new RijndaelManaged()
            {
                Key = sUserKey,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            using (aes)
            {
                sTransformer = aes.CreateEncryptor();
            }
        }

        public static void Transform(byte[] data, byte[] IV)
        {
            byte[] morphKey = new byte[16];
            int remaining = data.Length;
            int start = 0;
            int length = 0x5B0;

            while (remaining > 0)
            {
                for (int i = 0; i < 16; i++)
                    morphKey[i] = IV[i % 4];

                if (remaining < length)
                    length = remaining;

                for (int index = start; index < (start + length); index++)
                {
                    if ((index - start) % 16 == 0)
                        sTransformer.TransformBlock(morphKey, 0, 16, morphKey, 0);

                    data[index] ^= morphKey[(index - start) % 16];
                }

                start += length;
                remaining -= length;
                length = 0x5B4;
            }
        }
    }
}

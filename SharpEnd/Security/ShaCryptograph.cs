using System;
using System.Security.Cryptography;
using System.Text;

namespace SharpEnd.Security
{
    public static class ShaCryptograph
    {
        public static string Encrypt(EShaMode mode, string input)
        {
            switch (mode)
            {
                case EShaMode.SHA1:
                    {
                        using (SHA1Managed sha = new SHA1Managed())
                        {
                            return BitConverter.ToString(sha.ComputeHash(Encoding.ASCII.GetBytes(input))).Replace("-", "").ToLower();
                        }
                    }

                case EShaMode.SHA256:
                    using (SHA256Managed sha = new SHA256Managed())
                    {
                        return BitConverter.ToString(sha.ComputeHash(Encoding.ASCII.GetBytes(input))).Replace("-", "").ToLower();
                    }

                case EShaMode.SHA384:
                    {
                        using (SHA384Managed sha = new SHA384Managed())
                        {
                            return BitConverter.ToString(sha.ComputeHash(Encoding.ASCII.GetBytes(input))).Replace("-", "").ToLower();
                        }
                    }

                case EShaMode.SHA512:
                    using (SHA512Managed sha = new SHA512Managed())
                    {
                        return BitConverter.ToString(sha.ComputeHash(Encoding.ASCII.GetBytes(input))).Replace("-", "").ToLower();
                    }

                default:
                    return string.Empty;
            }
        }
    }
}

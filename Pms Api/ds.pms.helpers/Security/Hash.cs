using System.Security.Cryptography;
using System.Text;

namespace ds.pms.helpers.Security
{
    public static class Hash
    {
        public static string GeneratePasswordHash(string password)
        {

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] encrypt;
            UTF8Encoding encode = new UTF8Encoding();
            //encrypt the given password string into Encrypted data  
            md5.ComputeHash(encode.GetBytes(password));
            encrypt = md5.Hash;
            StringBuilder encryptdata = new StringBuilder();
            //Create a new string by using the encrypted data  
            for (int i = 0; i < encrypt.Length; i++)
            {
                encryptdata.Append(encrypt[i].ToString("x2"));
            }
            return encryptdata.ToString();
        }
    }
}

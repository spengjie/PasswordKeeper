using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Security.Cryptography;
using System.IO;
using System.Windows;

namespace PasswordKeeper
{
    class Security
    {
        static byte[] KEY_64 = Encoding.ASCII.GetBytes("ASA8J38J");
        static byte[] IV_64 = Encoding.ASCII.GetBytes("829DJMC2");
        
        public static string GetMD5_32(string s, string _input_charset)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding(_input_charset).GetBytes(s));
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }

        public static string GetMD5_16(string ConvertString)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)), 4, 8);
            t2 = t2.Replace("-", "");
            return t2;
        }

        /// <summary>
        /// 文件加密
        /// </summary>
        /// <param name="context"></param>
        /// <param name="outName"></param>
        public static void EncryptData(String context, String outName)
        {
            try
            {
                FileStream fout = new FileStream(outName, FileMode.Create, FileAccess.Write);

                //Create variables to help with read and write.

                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                CryptoStream encStream = new CryptoStream(fout, des.CreateEncryptor(KEY_64, IV_64), CryptoStreamMode.Write);
                StreamWriter sw = new StreamWriter(encStream);

                sw.Write(context);
                sw.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 文件解密
        /// </summary>
        /// <param name="inName"></param>
        /// <returns></returns>
        public static string DecryptData(String inName)
        {
            FileStream fin = new FileStream(inName, FileMode.Open, FileAccess.Read);
            
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            CryptoStream encStream = new CryptoStream(fin, des.CreateDecryptor(KEY_64, IV_64), CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(encStream);

            string str = sr.ReadToEnd();
            sr.Close();
            return str;
        }
    }
}

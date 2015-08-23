using System;
using System.Text;
using System.Security.Cryptography;

namespace Framework.Network
{
	public class XAes
	{
	    private System.Text.UTF8Encoding utf8Encoding = null;
	    private RijndaelManaged rijndael = null;
	
	    public XAes(string key, string iv)
	    {
	        this.utf8Encoding = new System.Text.UTF8Encoding();
	        this.rijndael = new RijndaelManaged();
	        this.rijndael.Mode = CipherMode.CBC;
	        this.rijndael.Padding = PaddingMode.PKCS7;
	        this.rijndael.KeySize = 128;
	        this.rijndael.BlockSize = 128;
	        this.rijndael.Key = hex2Byte(str2Hex(key));
	        this.rijndael.IV = hex2Byte(str2Hex(iv));
	    }
	
	    public string Encrypt(string text)
	    {
	        byte[] cipherBytes = null;
	        ICryptoTransform transform = null;
	        if (text == null)
	            text = "";
	        try
	        {
	            cipherBytes = new byte[] {};
	            transform = this.rijndael.CreateEncryptor();
	            byte[] plainText = this.utf8Encoding.GetBytes(text);
	            cipherBytes = transform.TransformFinalBlock(plainText, 0, plainText.Length);
	        }
	        catch
	        {
	            throw new ArgumentException(
	               "text is not a valid string!(Encrypt)", "text");
	        }
	        return Convert.ToBase64String(cipherBytes);
	    }
	
	    public string Decrypt(string text)
	    {
	        byte[] plainText = null;
	        ICryptoTransform transform = null;
	        if (text == null || text == "")
	            throw new ArgumentException("text is not null.");
	        
	        try
	        {
	            plainText = new byte[] {};
	            transform = rijndael.CreateDecryptor();
	            byte[] encryptedValue = Convert.FromBase64String(text);
	            plainText = transform.TransformFinalBlock(encryptedValue, 0, 
	               encryptedValue.Length);
	        }
	        catch
	        {
	            throw new ArgumentException(
	               "text is not a valid string!(Decrypt)", "text");
	        }
	        return this.utf8Encoding.GetString(plainText);
	    }
	
	    public byte[] hex2Byte(string hex)
	    {
	        byte[] bytes = new byte[hex.Length / 2];
	        for (int i = 0; i < bytes.Length; i++)
	        {
	            try
	            {
	                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
	            }
	            catch
	            {
	                throw new ArgumentException(
	                "hex is not a valid hex number!", "hex");
	            }
	        }
	        return bytes;
	    }
	
	    public string byte2Hex(byte[] bytes)
	    {
	        string hex = "";
	        if (bytes != null)
	        {
	            for (int i = 0; i < bytes.Length; i++)
	            {
	                hex += bytes[i].ToString("X2");
	            }
	        }
	        return hex;
	    }
		
		public string str2Hex(string strData)
		{
			byte[] bytes = str2bytes(strData);
			
			string resultHex = "";
			foreach (byte byteStr in bytes)
			{
				resultHex += string.Format("{0:x2}", byteStr);
			}
			return resultHex;
		}
		
		public byte[] str2bytes(string byteData)
		{
			//System.Text.ASCIIEncoding asencoding = new System.Text.ASCIIEncoding();
			return Encoding.Default.GetBytes(byteData);
		}
	}
}
using System;
using System.IO;
using System.Net;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Net.Mail;
using System.Globalization;
using System.Configuration;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Web.Routing;
using System.Web.Mvc;

namespace Common
{
    public class GblFunctions
    {
        private const string key = "NICWBBSE";

        public static bool isDate(string date)
        {
            bool status = true;
            try
            {
                DateTime doj = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
                status = true;
            }
            catch
            {
                status = false;
            }
            return status;
        }

        public static string CreateSalt(int size)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size - 1];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        public static string CreatePasswordHash(string pwd)
        {
            string HashedPwd = string.Empty;
            SHA256 sha256 = SHA256Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(pwd);
            byte[] hash = sha256.ComputeHash(bytes);
            HashedPwd = GetStringFromHash(hash);
            return HashedPwd;
        }

        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i <= hash.Length - 1; i++)
                result.Append(hash[i].ToString("X2"));

            return result.ToString();
        }

        public static string SHA1Encrypt(string phrase)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            SHA1Managed sha1hasher = new SHA1Managed();
            byte[] hashedDataBytes = sha1hasher.ComputeHash(encoder.GetBytes(phrase));
            return GetStringFromHash(hashedDataBytes);
        }

        public static bool CheckScriptAndSpecialChar(object strWords)
        {
            bool result = true;
            string[] badchars = new string[12] { "delete", "drop", "truncate", "or", "*", "&", "$", "#", "<", ">", "script", "html" };
            string newChars;
            int i;
            newChars = strWords.ToString().ToLower();
            // Comment to get exact word with spaces
            for (i = 0; i <= 11; i++)
            {
                if (newChars.IndexOf(" " + badchars[i] + " ") >= 0)
                    result = false;
            }
            return result;
        }

        #region Encrypt Password
        public static string encryptPassword(string password)
        {
            string encryptedpassword = password;
            byte[] getbytes = Encoding.Unicode.GetBytes(encryptedpassword);
            using (Aes aes = Aes.Create())
            {
                Rfc2898DeriveBytes digital = new Rfc2898DeriveBytes(key, new byte[] { 0x47, 0x11, 0x13, 0x7e, 0x23, 0x5d, 0x67, 0x67, 0x73, 0x61, 0x87, 0x97, 0x71 });
                aes.Key = digital.GetBytes(32);
                aes.IV = digital.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(getbytes, 0, getbytes.Length);
                        cs.Close();
                    }
                    encryptedpassword = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptedpassword;
        }
        #endregion

        #region Decrypt Password
        public static string decryptPassword(string encryptedpassword)
        {
            string decryptedpassword = encryptedpassword;
            decryptedpassword = decryptedpassword.Replace(" ", "+");
            byte[] getbytes = Convert.FromBase64String(decryptedpassword);
            using (Aes aes = Aes.Create())
            {
                Rfc2898DeriveBytes digital = new Rfc2898DeriveBytes(key, new byte[] { 0x47, 0x11, 0x13, 0x7e, 0x23, 0x5d, 0x67, 0x67, 0x73, 0x61, 0x87, 0x97, 0x71 });
                aes.Key = digital.GetBytes(32);
                aes.IV = digital.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(getbytes, 0, getbytes.Length);
                        cs.Close();
                    }
                    decryptedpassword = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return decryptedpassword;
        }
        #endregion

        #region Encrypt Token
        public static string encryptToken(string token)
        {
            string encryptedpassword = token;
            byte[] getbytes = Encoding.Unicode.GetBytes(encryptedpassword);
            using (Aes aes = Aes.Create())
            {
                string key = "7A06E3A373D44593B4405B6CCE0273D0";
                Rfc2898DeriveBytes digital = new Rfc2898DeriveBytes(key, new byte[] { 0x47, 0x11, 0x13, 0x7e, 0x23, 0x5d, 0x67, 0x67, 0x73, 0x61, 0x87, 0x97, 0x71 });
                aes.Key = digital.GetBytes(32);
                aes.IV = digital.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(getbytes, 0, getbytes.Length);
                        cs.Close();
                    }
                    encryptedpassword = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptedpassword;
        }
        #endregion

        #region Decrypt Token
        public static string decryptToken(string token)
        {
            string tokenKey = "7A06E3A373D44593B4405B6CCE0273D0";
            string decryptedToken = token.Replace(" ", "+");
            byte[] getbytes = Convert.FromBase64String(decryptedToken);
            using (Aes aes = Aes.Create())
            {
                Rfc2898DeriveBytes digital = new Rfc2898DeriveBytes(tokenKey, new byte[] { 0x47, 0x11, 0x13, 0x7e, 0x23, 0x5d, 0x67, 0x67, 0x73, 0x61, 0x87, 0x97, 0x71 });
                aes.Key = digital.GetBytes(32);
                aes.IV = digital.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(getbytes, 0, getbytes.Length);
                        cs.Close();
                    }
                    decryptedToken = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return decryptedToken;
        }
        #endregion

        public static string generateNOnceHash()
        {
            string hash = string.Empty;

            try
            {
                Guid obj = Guid.NewGuid();
                hash = obj.ToString().Replace("-", "");
            }
            catch
            {
                //nothing to do. scripts will not be loaded
            }
            return hash;
        }

        public static string getNonceHash()
        {
            string hash = string.Empty;

            try
            {
                if (HttpContext.Current.Session[SessionNames.Nonce] == null)
                {
                    hash = generateNOnceHash();
                    HttpContext.Current.Session[SessionNames.Nonce] = hash;
                }
                else
                {
                    hash = HttpContext.Current.Session[SessionNames.Nonce].ToString();
                }
            }
            catch
            {
                //nothing to do. scripts will not be loaded
            }
            return hash;
        }

        public static string GenerateRandomCode()
        {
            Random random = new Random();
            string[] strArray = new string[35];
            strArray = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            string s = "";
            for (int i = 0; i <= 5; i++)
            {
                int x = Convert.ToInt32(random.Next(0, 34));
                s += strArray[x].ToString();
            }
            return s;
        }

        //public static string GenerateRandomCode()
        //{
        //    Random random = new Random();
        //    string[] strArray = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
        //    string s = string.Empty;
        //    for (int i = 0; i <= 5; i++)
        //    {
        //        int x = Convert.ToInt32(random.Next(0, strArray.Length));
        //        s += strArray[x].ToString();
        //    }
        //    return s;
        //}

        public string getTitle(string columnName)
        {
            return (Regex.Replace(columnName, @"(\p{Lu})", " $1").TrimStart());
        }

        public string makeGridColumns(string columnList, string editYN, string deleteYN)
        {
            string data = string.Empty;

            try
            {
                string[] aryColumnList = columnList.Split(',');

                for (int i = 0; i < aryColumnList.Length; i++)
                {
                    if (aryColumnList[i].Contains("Id") == true) //further improvement needed
                    {
                        data += "{ field: '" + aryColumnList[i] + "', title: 'Id', sortable: true, hidden:true},";
                    }
                    else
                    {
                        data += "{ field: '" + aryColumnList[i] + "', title: '" + getTitle(aryColumnList[i]) + "', tooltip: '" + getTitle(aryColumnList[i]) + "', sortable: true },";
                    }

                }
                if (editYN == "Y")
                {
                    data += "{ title: '', field: 'Edit', width: 60, type: 'icon', icon: 'fa fa-pencil', tooltip: 'Edit', events: { 'click': Edit } },";
                }
                if (deleteYN == "Y")
                {
                    data += "{ title: '', field: 'Delete', width: 60, type: 'icon', icon: 'fa fa-remove', tooltip: 'Delete', events: { 'click': Remove } },";
                }                
                if (data.Length > 1 && data[data.Length - 1] == ',')
                {
                    data = data.Substring(0, data.Length - 1);
                }
            }
            catch
            {
                //grid columns will not be made
                data = string.Empty;
            }
            return data;
        }

        public string makeGridColumns(string[,] columnList, string editYN, string deleteYN, string widthYN = null, string viewYN = null)
        {
            string data = string.Empty;

            try
            {
                for (int i = 0; i <= columnList.GetUpperBound(0); i++)
                {
                    if (!string.IsNullOrEmpty(widthYN) && widthYN == "Y")
                    {
                        data += "{ field: '" + columnList[i, 0] + "', title: '" + columnList[i, 1] + "', tooltip: '" + columnList[i, 1] + "', sortable: true, hidden:" + columnList[i, 2] + "" + ((columnList[i, 3] != "0" ? ", width:" + columnList[i, 3] : "")) + "},";
                    }
                    else
                    {
                        data += "{ field: '" + columnList[i, 0] + "', title: '" + columnList[i, 1] + "', tooltip: '" + columnList[i, 1] + "', sortable: true, hidden:" + columnList[i, 2] + "},";
                    }
                }
                if (!string.IsNullOrEmpty(viewYN) && viewYN == "Y")
                {
                    data += "{ title: '', field: 'View', width: 60, type: 'icon', icon: 'fa fa-eye custom-view', tooltip: 'View', events: { 'click': View } },";
                }
                if (editYN == "Y")
                {
                    data += "{ title: '', field: 'Edit', width: 60, type: 'icon', icon: 'fa fa-pencil', tooltip: 'Edit', events: { 'click': Edit } },";
                }
                if (deleteYN == "Y")
                {
                    data += "{ title: '', field: 'Delete', width: 60, type: 'icon', icon: 'fa fa-remove', tooltip: 'Delete', events: { 'click': Remove } },";
                }
               
                if (data.Length > 1 && data[data.Length - 1] == ',')
                {
                    data = data.Substring(0, data.Length - 1);
                }
            }
            catch
            {
                //grid columns will not be made
                data = string.Empty;
            }
            return data;
        }

        public string makeGridColumnsWebsite(string[,] columnList, string editYN, string deleteYN, string widthYN = null, string viewYN = null)
        {
            string data = string.Empty;

            try
            {
                for (int i = 0; i <= columnList.GetUpperBound(0); i++)
                {
                    if (!string.IsNullOrEmpty(widthYN) && widthYN == "Y")
                    {
                        data += "{ field: '" + columnList[i, 0] + "', title: '" + columnList[i, 1] + "', tooltip: '" + columnList[i, 1] + "', sortable: true, hidden:" + columnList[i, 2] + "" + ((columnList[i, 3] != "0" ? ", width:" + columnList[i, 3] : "")) + "},";
                    }
                    else
                    {
                        data += "{ field: '" + columnList[i, 0] + "', title: '" + columnList[i, 1] + "', tooltip: '" + columnList[i, 1] + "', sortable: true, hidden:" + columnList[i, 2] + "},";
                    }
                }
                if (!string.IsNullOrEmpty(viewYN) && viewYN == "Y")
                {
                    data += "{ title: 'View', field: 'View', width: 60, type: 'icon', icon: 'fa fa-eye custom-view', tooltip: 'View', events: { 'click': View } },";
                }
                if (editYN == "Y")
                {
                    data += "{ title: '', field: 'Edit', width: 60, type: 'icon', icon: 'fa fa-pencil', tooltip: 'Edit', events: { 'click': Edit } },";
                }
                if (deleteYN == "Y")
                {
                    data += "{ title: '', field: 'Delete', width: 60, type: 'icon', icon: 'fa fa-remove', tooltip: 'Delete', events: { 'click': Remove } },";
                }

                if (data.Length > 1 && data[data.Length - 1] == ',')
                {
                    data = data.Substring(0, data.Length - 1);
                }
            }
            catch
            {
                //grid columns will not be made
                data = string.Empty;
            }
            return data;
        }

        public static string GenerateOTP(int max)
        {
            Random generator = new Random();
            String r = generator.Next(0, max).ToString("D" + max.ToString().Length);
            return r;
        }

        public static int sendMail(string emailId, string subject, string message)
        {
            int err = 0;

            try
            {
                string mailServer = ConfigurationManager.AppSettings["MailServer"];
                string mailServerEmailId = ConfigurationManager.AppSettings["MailServerMailId"];
                string mailServerPwd = ConfigurationManager.AppSettings["MailServerPwd"];
                int mailServerSMTPPort = Convert.ToInt32(ConfigurationManager.AppSettings["MailServerSMTPPort"]);
                SmtpClient mailClient;
                MailMessage m = new MailMessage();
                m.IsBodyHtml = true;
                m.From = new MailAddress(mailServerEmailId, subject);
                m.To.Add(new MailAddress(emailId));
                m.Subject = subject;
                m.Priority = MailPriority.High;
                m.Body = message;
                mailClient = new System.Net.Mail.SmtpClient(mailServer, mailServerSMTPPort);
                NetworkCredential creed = new NetworkCredential(mailServerEmailId, mailServerPwd);
                mailClient.UseDefaultCredentials = false;
                mailClient.Credentials = creed;
                mailClient.EnableSsl = false;
                mailClient.Send(m);
            }
            catch
            {
                err = 1;
            }

            return err;
        }

        public static int sendMailGmail(string emailId, string subject, string message)
        {
            int err = 0;

            try
            {
                string mailServer = ConfigurationManager.AppSettings["MailServer"];
                string mailServerEmailId = ConfigurationManager.AppSettings["MailServerMailId"];
                string mailServerPwd = ConfigurationManager.AppSettings["MailServerPwd"];
                int mailServerSMTPPort = Convert.ToInt32(ConfigurationManager.AppSettings["MailServerSMTPPort"]);
                using (MailMessage mm = new MailMessage(mailServerEmailId, emailId))
                {
                    mm.Subject = subject;
                    mm.Body = message;
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = mailServer;
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    NetworkCredential NetworkCred = new NetworkCredential(mailServerEmailId, mailServerPwd);
                    //smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = mailServerSMTPPort;
                    smtp.Send(mm);
                }
            }
            catch
            {
                err = 1;
            }

            return err;
        }

        public static int sendMailForContactUs(string subject, string message)
        {
            int err = 0;

            try
            {
                string mailServer = ConfigurationManager.AppSettings["MailServer"];
                string mailServerEmailId = ConfigurationManager.AppSettings["MailServerMailId"];
                string mailServerPwd = ConfigurationManager.AppSettings["MailServerPwd"];
                string emailIdTo = ConfigurationManager.AppSettings["MailServerMailIdTo"];
                int mailServerSMTPPort = Convert.ToInt32(ConfigurationManager.AppSettings["MailServerSMTPPort"]);
                using (MailMessage mm = new MailMessage(mailServerEmailId, emailIdTo))
                {
                    mm.Subject = subject;
                    mm.Body = message;
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = mailServer;
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential(mailServerEmailId, mailServerPwd);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = mailServerSMTPPort;
                    smtp.Send(mm);
                }
            }
            catch
            {
                err = 1;
            }

            return err;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static bool chkDataFormat(string type, string data)
        {
            Match match;

            switch (type)
            {
                case RegexType.Alpha:
                    match = Regex.Match(data, @"^([A-Za-z0-9 \/\-()_,.:]+)$", RegexOptions.IgnoreCase);
                    if (!match.Success)
                    {
                        return false;
                    }
                    break;
                case RegexType.AlphaWithoutSpace:
                    match = Regex.Match(data, @"^([A-Za-z0-9]+)$", RegexOptions.IgnoreCase);
                    if (!match.Success)
                    {
                        return false;
                    }
                    break;
                case RegexType.Numeric:
                    match = Regex.Match(data, @"^[0-9]+$", RegexOptions.IgnoreCase);
                    if (!match.Success)
                    {
                        return false;
                    }
                    break;
                case RegexType.EmailId:
                    match = Regex.Match(data, @"^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$", RegexOptions.IgnoreCase);
                    if (!match.Success)
                    {
                        return false;
                    }
                    break;
                case RegexType.MobileNo:
                    match = Regex.Match(data, @"^[56789][0-9]{9}$", RegexOptions.IgnoreCase);
                    if (!match.Success)
                    {
                        return false;
                    }
                    break;
                case RegexType.URL:
                    match = Regex.Match(data, @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$", RegexOptions.IgnoreCase);
                    if (!match.Success)
                    {
                        return false;
                    }
                    break;
                case RegexType.OnlyAlpha:
                    match = Regex.Match(data, @"^[A-Za-z ]+$", RegexOptions.IgnoreCase);
                    if (!match.Success)
                    {
                        return false;
                    }
                    break;
                case RegexType.PinCode:
                    match = Regex.Match(data, @"^([0-9]{6})$", RegexOptions.IgnoreCase);
                    if (!match.Success)
                    {
                        return false;
                    }
                    break;
                case RegexType.StdCode:
                    match = Regex.Match(data, @"^([0-9]{1,10})$", RegexOptions.IgnoreCase);
                    if (!match.Success)
                    {
                        return false;
                    }
                    break;
                case RegexType.PhoneNo:
                    match = Regex.Match(data, @"^([0-9]{1,10})$", RegexOptions.IgnoreCase);
                    if (!match.Success)
                    {
                        return false;
                    }
                    break;
                case RegexType.Date:
                    match = Regex.Match(data, @"^([0-9]{2})-([0-9]{2})-([0-9]{4})$", RegexOptions.IgnoreCase);
                    if (!match.Success)
                    {
                        return false;
                    }
                    if (setDate(data) == null)
                    {
                        return false;
                    }
                    break;
                case RegexType.ContentEdit:
                    match = Regex.Match(data, @"^[A-Za-z0-9,._ \/\-()\?<>&;#\']+$", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    if (!match.Success)
                    {
                        return false;
                    }
                    break;
                case RegexType.RollNo:
                    match = Regex.Match(data, @"^([A-Za-z0-9]{11})$", RegexOptions.IgnoreCase);
                    if (!match.Success)
                    {
                        return false;
                    }
                    break;
                case RegexType.SchoolIndexNo:
                    match = Regex.Match(data, @"^([A-Za-z0-9]{5})$", RegexOptions.IgnoreCase);
                    if (!match.Success)
                    {
                        return false;
                    }
                    break;
                case RegexType.CustomDOB:
                    match = Regex.Match(data, @"^([0-9]{6})$", RegexOptions.IgnoreCase);
                    if (!match.Success)
                    {
                        return false;
                    }
                    break;
                case RegexType.Price:
                    match = Regex.Match(data, @"^[0-9.]+$", RegexOptions.IgnoreCase);
                    if (!match.Success)
                    {
                        return false;
                    }
                    break;
                default:
                    return false;
            }

            return true;
        }

        public static DateTime? setDate(string date)
        {
            DateTime? dt = DateTime.Now;

            try
            {
                dt = DateTime.ParseExact(date, "dd-MM-yyyy", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None);
            }
            catch
            {
                dt = (DateTime?)null;
                //throw (ex);
            }

            return dt;
        }
    }

    public static class MimeSniffer
    {
        /// <summary>
        /// Internet Explorer 9. Returns image/png and image/jpeg instead of 
        /// image/x-png and image/pjpeg.
        /// </summary>
        private const uint FMFD_RETURNUPDATEDIMGMIMES = 0x20;

        /// <summary>
        /// The zero (0) value for Reserved parameters.
        /// </summary>
        private const uint RESERVED = 0;

        /// <summary>
        /// The value that is returned when the MIME type cannot be recognized.
        /// </summary>
        private const string UNKNOWN = "unknown/unknown";

        /// <summary>
        /// The return value which indicates that the operation completed successfully.
        /// </summary>
        private const uint S_OK = 0;

        /// <summary>
        /// Determines the MIME type from the data provided.
        /// </summary>
        /// <param name="pBC">A pointer to the IBindCtx interface. Can be set to NULL.</param>
        /// <param name="pwzUrl">A pointer to a string value that contains the URL of the data. Can be set to NULL if <paramref name="pBuffer"/> contains the data to be sniffed.</param>
        /// <param name="pBuffer">A pointer to the buffer that contains the data to be sniffed. Can be set to NULL if <paramref name="pwzUrl"/> contains a valid URL.</param>
        /// <param name="cbSize">An unsigned long integer value that contains the size of the buffer.</param>
        /// <param name="pwzMimeProposed">A pointer to a string value that contains the proposed MIME type. This value is authoritative if type cannot be determined from the data. If the proposed type contains a semi-colon (;) it is removed. This parameter can be set to NULL.</param>
        /// <param name="dwMimeFlags">The flags which modifies the behavior of the function.</param>
        /// <param name="ppwzMimeOut">The address of a string value that receives the suggested MIME type.</param>
        /// <param name="dwReserverd">Reserved. Must be set to 0.</param>
        /// <returns>S_OK, E_FAIL, E_INVALIDARG or E_OUTOFMEMORY.</returns>
        /// <remarks>
        /// Read more: http://msdn.microsoft.com/en-us/library/ms775107(v=vs.85).aspx
        /// </remarks>
        [DllImport(@"urlmon.dll", CharSet = CharSet.Auto)]
        private extern static uint FindMimeFromData(
                uint pBC,
                [MarshalAs(UnmanagedType.LPStr)] string pwzUrl,
                [MarshalAs(UnmanagedType.LPArray)] byte[] pBuffer,
                uint cbSize,
                [MarshalAs(UnmanagedType.LPStr)] string pwzMimeProposed,
                uint dwMimeFlags,
                out uint ppwzMimeOut,
                uint dwReserverd
        );

        /// <summary>
        /// Returns the MIME type for the specified file header.
        /// </summary>
        /// <param name="header">The header to examine.</param>
        /// <returns>The MIME type or "unknown/unknown" if the type cannot be recognized.</returns>
        /// <remarks>
        /// NOTE: This method recognizes only 26 types used by IE.
        /// http://msdn.microsoft.com/en-us/library/ms775147(VS.85).aspx#Known_MimeTypes
        /// </remarks>
        public static string GetMime(byte[] header)
        {
            try
            {
                uint mimetype;
                uint result = FindMimeFromData(0,
                                                null,
                                                header,
                                                (uint)header.Length,
                                                null,
                                                FMFD_RETURNUPDATEDIMGMIMES,
                                                out mimetype,
                                                RESERVED);
                if (result != S_OK)
                {
                    return UNKNOWN;
                }

                IntPtr mimeTypePtr = new IntPtr(mimetype);
                string mime = Marshal.PtrToStringUni(mimeTypePtr);
                Marshal.FreeCoTaskMem(mimeTypePtr);
                return mime;
            }
            catch
            {
                return UNKNOWN;
            }
        }
    }
}

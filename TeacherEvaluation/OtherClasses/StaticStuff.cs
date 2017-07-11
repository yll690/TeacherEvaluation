using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Security.Cryptography;

namespace TeacherEvaluation
{
    public static class StaticStuff
    {
        public static SolidColorBrush solidLightGrayBrush=new SolidColorBrush(Colors.LightGray);
        public static SolidColorBrush solidWhiteBrush = new SolidColorBrush(Colors.White);
        public static SolidColorBrush solidAliceBlueBrush = new SolidColorBrush(Colors.AliceBlue);
        public static SolidColorBrush solidFFC8DCF0Brush = new SolidColorBrush(Color.FromRgb(200,220,240));
        public static SolidColorBrush solidWarnBrush = new SolidColorBrush(Color.FromRgb(250, 255, 189));

        public static Random random = new Random();

        public static BitmapImage getRandomHead()
        {
            string fileName = "/Heads/" + random.Next(0, 10) + ".jpg";
            return new BitmapImage(new Uri(fileName, UriKind.Relative));
        }

        public static string getMD5(string word)
        {
            if (word == "" || word == null)
                return "";
            try
            {
                MD5CryptoServiceProvider MD5CSP = new MD5CryptoServiceProvider();
                byte[] bytValue = Encoding.UTF8.GetBytes(word);
                byte[] bytHash = MD5CSP.ComputeHash(bytValue);
                MD5CSP.Clear();
                string sHash = "", sTemp = "";
                for (int counter = 0; counter < bytHash.Count(); counter++)
                {
                    long i = bytHash[counter] / 16;
                    if (i > 9)
                        sTemp = ((char)(i - 10 + 0x41)).ToString();
                    else
                        sTemp = ((char)(i + 0x30)).ToString();
                    i = bytHash[counter] % 16;
                    if (i > 9)
                        sTemp += ((char)(i - 10 + 0x41)).ToString();
                    else
                        sTemp += ((char)(i + 0x30)).ToString();
                    sHash += sTemp;
                }
                return sHash;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

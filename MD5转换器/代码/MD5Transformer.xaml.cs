using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TeacherEvaluationTools
{
    /// <summary>
    /// MD5Transformer.xaml 的交互逻辑
    /// </summary>
    public partial class MD5Transformer : Window
    {
        public MD5Transformer()
        {
            InitializeComponent();
        }

        private void inputTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            outputTB.Text = getMD5(inputTB.Text);
        }

        public static string getMD5(string word)
        {
            if (word == "" || word == null)
                return "";
            try
            {
                System.Security.Cryptography.MD5CryptoServiceProvider MD5CSP
                 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] bytValue = System.Text.Encoding.UTF8.GetBytes(word);
                byte[] bytHash = MD5CSP.ComputeHash(bytValue);
                MD5CSP.Clear();
                //根据计算得到的Hash码翻译为MD5码
                string sHash = "", sTemp = "";
                for (int counter = 0; counter < bytHash.Count(); counter++)
                {
                    long i = bytHash[counter] / 16;
                    if (i > 9)
                    {
                        sTemp = ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp = ((char)(i + 0x30)).ToString();
                    }
                    i = bytHash[counter] % 16;
                    if (i > 9)
                    {
                        sTemp += ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp += ((char)(i + 0x30)).ToString();
                    }
                    sHash += sTemp;
                }
                //根据大小写规则决定返回的字符串
                return sHash;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void copyB_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(outputTB.Text);
        }

        private void pasteB_Click(object sender, RoutedEventArgs e)
        {
            inputTB.Text = Clipboard.GetText();
        }
    }
}

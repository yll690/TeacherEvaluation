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

namespace TeacherEvaluation
{
    /// <summary>
    /// InputDialog.xaml 的交互逻辑
    /// </summary>
    public partial class InputDialog : Window
    {
        string inputText = "";
        int maxLength=1000;

        public string InputText { get => inputText; set => inputText = value; }
        public int MaxLength { get => maxLength; set => maxLength = value; }

        public InputDialog()
        {
            InitializeComponent();
            initialize();
        }

        public InputDialog(string title)
        {
            InitializeComponent();
            Title = title;
            initialize();
        }

        public InputDialog(string title, int maxLength)
        {
            InitializeComponent();
            Title = title;
            MaxLength = maxLength;
            initialize();
        }

        private void initialize()
        {
            inputTB.MaxLength = MaxLength;
            lengthL.Content = "0/" + MaxLength;
        }

        private void OKB_Click(object sender, RoutedEventArgs e)
        {
            if (inputTB.Text == "")
            {
                MessageBox.Show("请输入内容！");
                return;
            }
            else
            {
                inputText = inputTB.Text;
                DialogResult = true;
                Close();
            }
        }

        private void cancelB_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void inputTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            lengthL.Content = inputTB.Text.Length + "/" + MaxLength;
        }
    }
}

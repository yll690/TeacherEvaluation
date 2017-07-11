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
    /// ChangePWWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ChangePassword : Window
    {
        public ChangePassword()
        {
            InitializeComponent();
        }

        private void OKB_Click(object sender, RoutedEventArgs e)
        {
            if (passwordPB2.Password != passwordPB1.Password)
            {
                warnP.IsOpen = true;
                return;
            }
            else
            {
                Identity identity = ((App)Application.Current).Identity;
                SqlHelper sqlHelper = ((App)Application.Current).SqlHelper;
                sqlHelper.changePassword(identity, StaticStuff.getMD5(passwordPB1.Password));
                MessageBox.Show("修改密码成功！");
                Close();
            }
        }

        private void cancelB_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void passwordPB2_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (passwordPB2.Password != passwordPB1.Password)
                passwordPB2.Background = StaticStuff.solidWarnBrush;
            else
                passwordPB2.Background = null;
        }
    }
}

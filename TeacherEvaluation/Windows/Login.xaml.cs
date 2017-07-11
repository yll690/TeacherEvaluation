using TeacherEvaluation.Properties;
using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace TeacherEvaluation
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>

    public partial class Login : Window
    {
        Settings settings = Settings.Default;
        Identity identity = new Identity(IdentityEnum.student,"");
        SqlHelper sqlHelper ;

        public Identity IdentityP { get => identity;
            set
            {
                identity = value;
                sqlHelper.LoginIdentity = value;
            }
        }

        public Login()
        {
            InitializeComponent();
            if (settings.username != "")
                nameTB.Text = settings.username;
            switch (settings.lastIdentity)
            {
                case 0: isStudentRB.IsChecked = true; break;
                case 1: isTeacherRB.IsChecked = true; break;
                case 2: isManagerRB.IsChecked = true; break;
            }
        }

        private void loginB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sqlHelper = new SqlHelper(IdentityP);
                if (!sqlHelper.login(nameTB.Text, StaticStuff.getMD5(passwordPB.Password)))
                    MessageBox.Show("用户名或密码错误！");
                else
                {
                    MainWindow mw = new MainWindow();
                    mw.Show();
                    Close();
                }
            }
            catch(Exception exc)
            {
                MessageBox.Show("登陆失败！\n"+exc.Message);
            }
        }

        private void isStudentRB_Checked(object sender, RoutedEventArgs e)
        {
            IdentityP.IdentityE = IdentityEnum.student;
            identityL.ToolTip = "请输入你的学号";
        }

        private void isTeacherRB_Checked(object sender, RoutedEventArgs e)
        {
            IdentityP.IdentityE = IdentityEnum.teacher;
            identityL.ToolTip = "请输入你的教工号";
        }

        private void isManagerRB_Checked(object sender, RoutedEventArgs e)
        {
            IdentityP.IdentityE = IdentityEnum.manager;
            identityL.ToolTip = "请输入你的督导号";
        }

        private void loginOptionL_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            LoginOption lo = new LoginOption();
            lo.ShowDialog();
        }

        private void forgetPWL_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("请联系管理员以重置密码!");
        }

        private void passwordPB_GotFocus(object sender, RoutedEventArgs e)
        {
            passwordPB.SelectAll();
        }
    }
}

using System.Windows;
using TeacherEvaluation.Properties;

namespace TeacherEvaluation
{
    /// <summary>
    /// LoginOption.xaml 的交互逻辑
    /// </summary>
    public partial class LoginOption : Window
    {
        Settings settings = Settings.Default;
        public LoginOption()
        {
            InitializeComponent();
            loadSettings();
        }

        private void loadSettings()
        {
            serverNameTB.Text = settings.server;
            DBNameTB.Text = settings.database;
            studentUNTB.Text = settings.studentUN;
            studentPWPB.Password = settings.studentPW;
            teacherUNTB.Text = settings.teacherUN;
            teacherPWPB.Password = settings.teacherPW;
            managerUNTB.Text = settings.managerUN;
            managerPWPB.Password = settings.managerPW;
        }

        private void confirm()
        {
            if (MessageBox.Show("随意修改此窗口里的内容可能导致无法登陆！\n" +
                "只有满足以下条件时你才有必要修改此窗口里的内容：\n" +
                "1、登录时出现奇怪的错误导致无法登陆\n" +
                "2、你正确地输入了正确的账号密码\n" +
                "3、你知道此窗口内的内容是什么，并知道应该怎么修改"
                , "注意！", MessageBoxButton.YesNo) == MessageBoxResult.No)
                Close();
        }

        private void saveB_Click(object sender, RoutedEventArgs e)
        {
            if(serverNameTB.Text==""||DBNameTB.Text==""||
                studentUNTB.Text==""||studentPWPB.Password==""||
                teacherUNTB.Text==""|| teacherPWPB.Password==""||
                managerUNTB.Text==""|| managerPWPB.Password=="")
            {
                MessageBox.Show("各个输入框都不能为空！");
                return;
            }
            settings.server = serverNameTB.Text;
            settings.database = DBNameTB.Text;
            settings.studentUN = studentUNTB.Text;
            settings.studentPW = studentPWPB.Password;
            settings.teacherUN = teacherUNTB.Text;
            settings.teacherPW = teacherPWPB.Password;
            settings.managerUN = managerUNTB.Text;
            settings.managerPW = managerPWPB.Password;
            settings.Save();
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            confirm();
        }

        private void cancelB_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

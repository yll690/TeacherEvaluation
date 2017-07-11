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
    /// SettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWindow : Window
    {
        Student student;
        SqlHelper sqlHelper = ((App)Application.Current).SqlHelper;

        public SettingWindow()
        {
            InitializeComponent();
            student = sqlHelper.getStudent(((App)Application.Current).Identity.Username);
            hideFromSCB.IsChecked = student.HideFromStudent;
            hideFromTCB.IsChecked = student.HideFromTeacher;
        }

        private void cancelB_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OKB_Click(object sender, RoutedEventArgs e)
        {
            sqlHelper.executeCommand("update student set " +
                "hideFromStudent=" + (bool)hideFromSCB.IsChecked +
                " ,hideFromTeacher=" + (bool)hideFromTCB.IsChecked +
                " where sID='" + student.StudentID + "'", "修改学生设置失败\n");
            Close();
        }
    }
}

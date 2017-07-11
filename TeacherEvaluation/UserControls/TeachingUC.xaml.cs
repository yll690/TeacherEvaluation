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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TeacherEvaluation
{
    /// <summary>
    /// TeacherControl.xaml 的交互逻辑
    /// </summary>
    /// <summary>
    /// TeacherControl.xaml 的交互逻辑
    /// </summary>
    public partial class TeachingUC : UserControl
    {
        Teaching teaching;
        public Teaching Teaching { get => teaching; set => teaching = value; }
        SqlHelper sqlHelper = ((App)Application.Current).SqlHelper;
        Identity identity = ((App)Application.Current).Identity;
        string selectedTerm = ((App)Application.Current).SelectedTerm;

        public delegate void TeacherClickedHandler(object sender, TeacherClickedEventArgs e);
        public delegate void CourseClickedHandler(object sender, CourseClickedEventArgs e);

        public event TeacherClickedHandler teacherClickedEvent;
        public event CourseClickedHandler courseClickedEvent;

        public TeachingUC(Teaching teacher)
        {
            InitializeComponent();
            Teaching = teacher;
            initialize();
        }

        private void initialize()
        {
            if (Teaching.Teacher.Picture != "")
                pictureI.Source = new BitmapImage(new Uri(Teaching.Teacher.Picture));
            else
                pictureI.Source = StaticStuff.getRandomHead();
            markL.Content = Math.Round(Teaching.Mark, 1);
            markR.Width = 8 * Teaching.Mark;
            nameL.Content = Teaching.Teacher.Name;
            nameL.ToolTip = "教师编号：" + Teaching.Teacher.TeacherID;
            if (Teaching.Teacher.Profile == "")
                profileTB.Text = "没有简介";
            else
                profileTB.Text = Teaching.Teacher.Profile;
            courseL.Content = "当前任课：" + Teaching.CourseName;
            courseL.ToolTip = "课程编号：" + Teaching.CourseID;
            termL.Content = Teaching.Term;
            switch (identity.IdentityE)
            {
                case IdentityEnum.student:
                    {
                        if (!sqlHelper.isOwnTeacher(identity.Username, Teaching.TcID))
                        {
                            nameL.Content += "*";
                            nameL.ToolTip += "，这位老师没有教过你";
                            gradeB.Visibility = Visibility.Collapsed;
                        }
                        break;
                    }
                case IdentityEnum.teacher:
                    {
                        gradeB.IsEnabled = false;
                        if (Teaching.Teacher.TeacherID != identity.Username)
                        {
                            nameL.Content = "教师";
                            nameL.ToolTip = null;
                            profileTB.Text = "没有简介";
                            pictureI.Source = StaticStuff.getRandomHead();
                        }
                        break;
                    }
                case IdentityEnum.manager:
                    {
                        gradeB.Visibility = Visibility.Collapsed;
                        break;
                    }
            }
        }

        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Label)sender).Background = StaticStuff.solidFFC8DCF0Brush;
        }

        private void Label_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Label)sender).Background = StaticStuff.solidWhiteBrush;
        }

        private void nameL_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (teacherClickedEvent != null)
                teacherClickedEvent(this, new TeacherClickedEventArgs(Teaching));
        }

        private void courseL_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (courseClickedEvent != null)
                courseClickedEvent(this, new CourseClickedEventArgs(Teaching.CourseID));
        }

        private void gradeB_Click(object sender, RoutedEventArgs e)
        {
            string markID = sqlHelper.isGraded(identity.Username, sqlHelper.getTcID(identity.Username, teaching.Teacher.TeacherID, selectedTerm));
            if (markID != "" && markID != null)
                if (MessageBox.Show("你已经给这位老师的这门课评过分了，是否覆盖？", "注意", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    GradeWindow gw = new GradeWindow("给 " + Teaching.Teacher.Name + " 老师评分", Teaching, markID);
                    gw.ShowDialog();
                }
                else
                    return;
            else
            {
                GradeWindow gw = new GradeWindow("给 " + Teaching.Teacher.Name + " 老师评分", Teaching, markID);
                gw.ShowDialog();
            }
        }
    }
}

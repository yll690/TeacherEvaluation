using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// CommentListUC.xaml 的交互逻辑
    /// </summary>
    public partial class TeacherDetailUC : UserControl
    {
        Teaching teaching;
        public Teaching Teaching { get => teaching; set => teaching = value; }

        int markWidth=300;

        SqlHelper sqlHelper = ((App)Application.Current).SqlHelper;
        Identity identity = ((App)Application.Current).Identity;
        string selectedTerm = ((App)Application.Current).SelectedTerm;
        Collection<string> tcIDsOfTeacher;
        Collection<Comment> allComments;
        Collection<Comment> courseComments;
     
        public TeacherDetailUC(Teaching teaching)
        {
            InitializeComponent();
            Teaching = teaching;
            initialize();
        }

        public void initialize()
        {
            if (teaching.Teacher.Picture == "")
                picture.Source = StaticStuff.getRandomHead();
            else
                picture.Source = StaticStuff.getRandomHead();
            nameL.Content = Teaching.Teacher.Name;
            nameL.ToolTip = "教师编号：" + Teaching.Teacher.TeacherID;
            string selectTerm = ((App)Application.Current).SelectedTerm;
            courseNameL.Content = "当前课程：" + Teaching.CourseName + "    当前学期：" + selectTerm;
            courseNameL.ToolTip = "课程编号：" + Teaching.CourseID;
            if (Teaching.Teacher.Profile == "")
                profileTB.Text = "没有简介";
            else
                profileTB.Text = Teaching.Teacher.Profile;
            markL.Content = Math.Round(Teaching.Mark, 1);
            switch (identity.IdentityE)
            {
                case IdentityEnum.teacher:
                    {
                        if (Teaching.Teacher.TeacherID != identity.Username)
                        {
                            rankAndMarkL.Visibility = Visibility.Collapsed;
                            teacherInfoG.Visibility = Visibility.Collapsed;
                        }
                        commentB.Visibility = Visibility.Collapsed;
                        gradeB.Visibility = Visibility.Collapsed;
                        break;
                    }
                case IdentityEnum.manager:
                    {
                        commentB.Visibility = Visibility.Collapsed;
                        gradeB.Visibility = Visibility.Collapsed;
                        break;
                    }
                case IdentityEnum.student:
                    {
                        rankAndMarkL.Visibility = Visibility.Collapsed;
                        if (!sqlHelper.isOwnTeacher(identity.Username, Teaching.TcID))
                        {
                            nameL.Content += "*";
                            nameL.ToolTip += "，这位老师没有教过你";
                            commentB.Visibility = Visibility.Collapsed;
                            gradeB.Visibility = Visibility.Collapsed;
                        }
                        break;
                    }
                default:break;
            }
            percent1R.Width = markWidth * sqlHelper.getMarkPercent(teaching.TcID, 8, 10);
            percent2R.Width = markWidth * sqlHelper.getMarkPercent(teaching.TcID, 5, 8);
            percent3R.Width = markWidth * sqlHelper.getMarkPercent(teaching.TcID, 0, 5);
            percent1L.Content = Math.Round(percent1R.Width / markWidth, 2) * 100 + "%";
            percent2L.Content = Math.Round(percent2R.Width / markWidth, 2) * 100 + "%";
            percent3L.Content = Math.Round(percent3R.Width / markWidth, 2) * 100 + "%";
            tcIDsOfTeacher = (Collection<string>)sqlHelper.getStrings("select tcID from teachingCourse where tID='" + teaching.Teacher.TeacherID + "'", "tcID");
            allComRB.IsChecked = true;

        }

        private void displayComments(bool disPlayAllcomments)
        {
            if (disPlayAllcomments)
            {
                if (allComments == null)
                {
                    allComments = new Collection<Comment>();
                    foreach (string s in tcIDsOfTeacher)
                    {
                        Collection<Comment> comments = (Collection<Comment>)sqlHelper.getCommentsByTcID(s);
                        foreach (Comment c in comments)
                            allComments.Add(c);
                    }
                }
            }
            else
            {
                if (courseComments == null)
                    courseComments = (Collection<Comment>)sqlHelper.getCommentsByTcID(teaching.TcID);
            }
            Collection<Comment> commentsDisplay;
            if (disPlayAllcomments)
                commentsDisplay = allComments;
            else
                commentsDisplay = courseComments;
            foreach (Comment c in commentsDisplay)
            {
                CommentUC commentUC = new CommentUC(c);
                commentUC.Margin = new Thickness(10);
                commentSP.Children.Add(commentUC);
                Border b = new Border();
                b.BorderBrush = StaticStuff.solidLightGrayBrush;
                b.BorderThickness = new Thickness(0, 0, 0, 1);
                commentSP.Children.Add(b);
            }
        }

        private void allComRB_Checked(object sender, RoutedEventArgs e)
        {
            if (commentSP != null)
                commentSP.Children.Clear();
            displayComments(true);
        }

        private void courseComRB_Checked(object sender, RoutedEventArgs e)
        {
            if (commentSP != null)
                commentSP.Children.Clear();
            displayComments(false);
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
        }

        private void commentB_Click(object sender, RoutedEventArgs e)
        {
            InputDialog input = new InputDialog("请输入评论");
            input.ShowDialog();
            if (input.DialogResult == false)
                return;
            sqlHelper.addComment(identity.Username,teaching.TcID, input.InputText);
        }

        private void rankAndMarkL_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            popupL.Content = sqlHelper.getRankAndMark(Teaching);
            popup.IsOpen = true;
        }
    }
}

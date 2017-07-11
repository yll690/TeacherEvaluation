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
    /// CommentsControl.xaml 的交互逻辑
    /// </summary>
    public partial class CommentUC : UserControl
    {
        Comment comment;
        Identity identity = ((App)Application.Current).Identity;
        //StaticResourceExtension sre = new StaticResourceExtension("defaultStudentIcon");

        public CommentUC(Comment comment)
        {
            InitializeComponent();
            this.comment = comment;
            initialize();
        }

        private void initialize()
        {
            bool hideFrom = false;
            switch (identity.IdentityE)
            {
                case IdentityEnum.student:
                    {
                        replyL.Visibility = Visibility.Hidden;
                        deleteL.Visibility = Visibility.Hidden;
                        hideFrom = comment.HideFromStudent;
                        break;
                    }
                case IdentityEnum.teacher:
                    {
                        deleteL.Visibility = Visibility.Hidden;
                        hideFrom = comment.HideFromTeacher;
                        break;
                    }
                case IdentityEnum.manager:
                    {
                        replyL.Visibility = Visibility.Hidden;
                        break;
                    }
            }
            if (hideFrom)
            {
                sNameL.Content = "学生";
                sPicI.Source = StaticStuff.getRandomHead();
            }
            else
            {
                sNameL.Content = comment.StudentName;
                if (comment.Studentpic == "")
                    sPicI.Source = StaticStuff.getRandomHead();
                else
                    sPicI.Source = new BitmapImage(new Uri(comment.Studentpic));
            }
            contentTB.Text = comment.Content;
            dateTimeL.Content = comment.Time.ToString();
            if (!comment.HasReply)
                tcReplyG.Visibility = Visibility.Collapsed;
            else
                tcReplyTB.Text = comment.TeacherRelpy;
        }

        private void replyL_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(comment.HasReply)
                if (MessageBox.Show("您已经给这条评论回复过了，是否覆盖之前的回复？", "注意", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    return;
            InputDialog inputD = new InputDialog("请输入您的回复");
            inputD.ShowDialog();
            if (inputD.DialogResult == false)
                return;
            else if (inputD.DialogResult == true)
            {
                comment.HasReply = true;
                comment.TeacherRelpy = inputD.InputText;
                SqlHelper sqlHelper = ((App)Application.Current).SqlHelper;
                sqlHelper.addReply(comment.CommentID, inputD.InputText);
                initialize();
                return;
            }
        }

        private void deleteL_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Show("确定要删除此条评论？\n" +
                "注意：删除之后此评论会在本软件中隐藏，但在数据库中仍然存在",
                "注意！", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                SqlHelper sqlHelper = ((App)Application.Current).SqlHelper;
                sqlHelper.deleteComment(comment.CommentID);
            }
            else
                return;
        }
    }
}

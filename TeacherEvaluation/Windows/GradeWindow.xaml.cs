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
    /// GradeWindow.xaml 的交互逻辑
    /// </summary>
    public partial class GradeWindow : Window
    {
        DetailedMark mark=new DetailedMark();
        public DetailedMark Mark { get => mark; set => mark = value; }
        Teaching teaching;
        string markID;
        public GradeWindow()
        {
            InitializeComponent();
        }

        public GradeWindow(string title,Teaching teaching, string markID)
        {
            InitializeComponent();
            Title = title;
            this.teaching = teaching;
            this.markID = markID;
            numOfMarksL.Content = teaching.NumOfMarks + "人参与评分";
            avgMarkL.Content = Math.Round(teaching.Mark, 1);
        }

        private void finishB_Click(object sender, RoutedEventArgs e)
        {

            Mark.TotalMark = (short)totalMarkS.Value;
            Mark.Mark1 = (short)mark1S.Value;
            Mark.Mark2 = (short)mark2S.Value;
            Mark.Mark3 = (short)mark3S.Value;
            Mark.Mark4 = (short)mark4S.Value;
            Mark.Mark5 = (short)mark5S.Value;
            if (Mark.TotalMark + Mark.Mark1 + Mark.Mark2 + Mark.Mark3 + Mark.Mark4 + Mark.Mark5 == 0)
            {
                MessageBox.Show("请认真评分哦");
                return;
            }
            else
            {
                SqlHelper sqlHelper = ((App)Application.Current).SqlHelper;
                Identity identity = ((App)Application.Current).Identity;
                string selectedTerm = ((App)Application.Current).SelectedTerm;
                if (markID != "" && markID != null)
                    sqlHelper.deleteMark(markID);
                sqlHelper.addMark(Mark, identity.Username, sqlHelper.getTcID(identity.Username, teaching.Teacher.TeacherID, selectedTerm));
                MessageBox.Show("评分成功！");
                Close();
            }
        }

        private void cancelB_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

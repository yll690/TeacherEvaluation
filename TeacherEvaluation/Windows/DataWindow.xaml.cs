using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
    /// DataWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DataWindow : Window
    {
        SqlHelper sqlHelper = ((App)Application.Current).SqlHelper;
        Identity identity = ((App)Application.Current).Identity;
        bool initialized=false;
        bool rankNotMark = true;
        string instituteID;
        string teacherID;
        string courseID;
        string term;

        public DataWindow()
        {
            InitializeComponent();
            initialize();
        }

        public void initialize()
        {
            instituteID = "";
            teacherID = "";
            courseID = "";

            switch(identity.IdentityE)
            {
                case IdentityEnum.manager:
                    {
                        Collection<string> iNames = (Collection<string>)sqlHelper.getStrings("select iName from institute order by iID", "iName");
                        Collection<string> iIDs = (Collection<string>)sqlHelper.getStrings("select iID from institute order by iID", "iID");
                        Collection<string> tNames = (Collection<string>)sqlHelper.getStrings("select tName from teacher order by tID", "tName");
                        Collection<string> tIDs = (Collection<string>)sqlHelper.getStrings("select tID from teacher order by tID", "tID");
                        Collection<string> cNames = (Collection<string>)sqlHelper.getStrings("select cName from course order by cID", "cName");
                        Collection<string> cIDs = (Collection<string>)sqlHelper.getStrings("select cID from course order by cID", "cID");
                        Collection<string> terms = (Collection<string>)sqlHelper.getTerms();
                        for (int i = 0; i < iNames.Count; i++)
                            instituteCB.Items.Add(iNames[i] + "(" + iIDs[i] + ")");
                        for (int i = 0; i < tNames.Count; i++)
                            teacherCB.Items.Add(tNames[i] + "(" + tIDs[i] + ")");
                        for (int i = 0; i < cNames.Count; i++)
                            courseCB.Items.Add(cNames[i] + "(" + cIDs[i] + ")");
                        for (int i = 0; i < terms.Count; i++)
                            termCB.Items.Add(terms[i]);
                        break;
                    }
                case IdentityEnum.teacher:
                    {
                        instituteID = ((Teacher)identity.IObject).InstituteID;
                        teacherID = ((Teacher)identity.IObject).TeacherID;
                        Collection<string> cNames = (Collection<string>)sqlHelper.getStrings("select cName from course where cID in (select cID from teachingCourse where tID='" + teacherID + "') order by cID", "cName");
                        Collection<string> cIDs = (Collection<string>)sqlHelper.getStrings("select cID from course where cID in (select cID from teachingCourse where tID='" + teacherID + "') order by cID", "cID");
                        Collection<string> terms = (Collection<string>)sqlHelper.getTerms();
                        for (int i = 0; i < cNames.Count; i++)
                            courseCB.Items.Add(cNames[i] + "(" + cIDs[i] + ")");
                        for (int i = 0; i < terms.Count; i++)
                            termCB.Items.Add(terms[i]);
                        instituteL.Visibility = Visibility.Collapsed;
                        instituteCB.Visibility = Visibility.Collapsed;
                        teacherL.Visibility = Visibility.Collapsed;
                        teacherCB.Visibility = Visibility.Collapsed;
                        break;
                    }
                default:Close();return;
            }
            initialized = true;
            inquire();
        }

        private void inquire()
        {
            if (!initialized)
                return;
            string cmd = "";
            if (rankNotMark)
                cmd = "select * from rankView";
            else
                cmd = "select * from markView";
            if ((instituteID != "" && instituteID != null) || (teacherID != "" && teacherID != null) ||
                (courseID != "" && courseID != null) || (term != "" && term != null))
            {
                cmd += " where ";
                if (instituteID != "" && instituteID != null)
                    cmd += "学院编号='" + instituteID + "' and ";
                if (teacherID != "" && teacherID != null)
                    cmd += "教师编号='" + teacherID + "' and ";
                if (courseID != "" && courseID != null)
                    cmd += "课程编号='" + courseID + "' and ";
                if (term != "" && term != null)
                    cmd += "学期='" + term + "' and ";
                cmd = cmd.Substring(0, cmd.Length - 5);
            }
            DataSet dataSet = sqlHelper.getDataSet(cmd);
            dataGrid.ItemsSource = null;
            dataGrid.Columns.Clear();
            try
            {
            dataGrid.ItemsSource = dataSet.Tables[0].DefaultView;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void instituteCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string item = instituteCB.SelectedItem.ToString();
            if (item == "全部")
                instituteID = "";
            else
            {
                int start = item.IndexOf("(")+1;
                int end = item.IndexOf(")");
                instituteID = item.Substring(start, end - start);
            }
            inquire();
        }

        private void teacherCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string item = teacherCB.SelectedItem.ToString();
            if (item == "全部")
                teacherID = "";
            else
            {
                int start = item.IndexOf("(") + 1;
                int end = item.IndexOf(")");
                teacherID = item.Substring(start, end - start);
            }
            inquire();
        }

        private void courseCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string item = courseCB.SelectedItem.ToString();
            if (item == "全部")
                courseID = "";
            else
            {
                int start = item.IndexOf("(") + 1;
                int end = item.IndexOf(")");
                courseID = item.Substring(start, end - start);
            }
            inquire();
        }

        private void termCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string item = termCB.SelectedItem.ToString();
            if (item == "全部")
                term = "";
            else
                term = item;
            inquire();
        }

        private void rankRB_Checked(object sender, RoutedEventArgs e)
        {
            rankNotMark = true;
            inquire();
        }

        private void markRB_Checked(object sender, RoutedEventArgs e)
        {
            rankNotMark = false;
            inquire();
        }
    }
}

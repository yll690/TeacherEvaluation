using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
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
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlHelper sqlHelper = ((App)Application.Current).SqlHelper;
        Collection<TabItem> instituteTabItems = new Collection<TabItem>();
        Identity identity = ((App)Application.Current).Identity;
        Teaching teaching;
        string selectedTerm;
        string SelectedTerm
        {
            get => selectedTerm;
            set
            {
                selectedTerm = value;
                ((App)Application.Current).SelectedTerm = value;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            initialize();
        }

        public MainWindow(SqlHelper sqlHelper)
        {
            InitializeComponent();
            this.sqlHelper = sqlHelper;
            initialize();
        }

        private void initialize()
        {
            Collection<string> terms = (Collection<string>)sqlHelper.getTerms();
            switch (identity.IdentityE)
            {
                case IdentityEnum.student:
                    {
                        markStatMI.Visibility = Visibility.Collapsed;
                        myRankMI.Visibility = Visibility.Collapsed;
                        Student student = sqlHelper.getStudent(identity.Username);
                        if (student.Picture == "")
                            headImage.Source = StaticStuff.getRandomHead();
                        else
                            headImage.Source = new BitmapImage(new Uri(student.Picture));
                        foreach (string s in terms)
                        {
                            if (s.CompareTo(student.Session + "-1") >= 0)
                                termsCB.Items.Add(s);
                        }
                        break;
                    }
                case IdentityEnum.teacher:
                    {
                        settingsMI.Visibility = Visibility.Collapsed;
                        thisTermB.Content = "查看此学期我的授课";
                        onlyOwnTeacherCB.Visibility = Visibility.Collapsed;
                        Teaching teaching = ((Collection<Teaching>)sqlHelper.getTeachings("select top(1) * from TCView where tID='" + identity.Username + "'"))[0];
                        if (teaching.Teacher.Picture == "")
                            headImage.Source = StaticStuff.getRandomHead();
                        else
                            headImage.Source = new BitmapImage(new Uri(teaching.Teacher.Picture));
                        foreach (string s in terms)
                            termsCB.Items.Add(s);
                        break;
                    }
                case IdentityEnum.manager:
                    {
                        myRankMI.Visibility = Visibility.Collapsed;
                        settingsMI.Visibility = Visibility.Collapsed;
                        thisTermB.Content = "查看此学期所有授课";
                        onlyOwnTeacherCB.Visibility = Visibility.Collapsed;
                        headImage.Source = StaticStuff.getRandomHead();
                        foreach (string s in terms)
                            termsCB.Items.Add(s);
                        break;
                    }
            }
            refreshB.Visibility = Visibility.Collapsed;
            termsCB.SelectedIndex = 0;
            SelectedTerm = Convert.ToString(termsCB.SelectedItem);
            onlyOwnTeacherCB.IsChecked = true;
            displayTeachersOfThisTerm(false);
        }

        private void displayTeachings(Collection<Teaching> teachings)
        {
            refreshB.Visibility = Visibility.Collapsed;
            mainSP.Children.Clear();
            foreach (Teaching t in teachings)
            {
                TeachingUC tc = new TeachingUC(t);
                tc.teacherClickedEvent += Tc_teacherClickedEvent;
                tc.courseClickedEvent += Tc_courseClickedEvent;
                mainSP.Children.Add(tc);
                Border b = new Border();
                b.BorderBrush = StaticStuff.solidLightGrayBrush;
                b.BorderThickness = new Thickness(0, 0, 0, 1);
                mainSP.Children.Add(b);
            }
        }

        private void displayTeachersOfThisTerm(string term,bool allTeaching)
        {
            Collection<Teaching> teachings;
            switch (identity.IdentityE)
            {
                case IdentityEnum.student:
                    {
                        if (!allTeaching)
                            teachings = (Collection<Teaching>)sqlHelper.getTeachings("select * from TCView where term='" + SelectedTerm + "' and tcID in (select tcID from selectCourse where sID='" + identity.Username + "')");
                        else
                            teachings = (Collection<Teaching>)sqlHelper.getTeachings("select * from TCView where term='" + SelectedTerm + "'");
                        break;
                    }
                case IdentityEnum.teacher:
                    {
                        if (!allTeaching)
                            teachings = (Collection<Teaching>)sqlHelper.getTeachings("select * from TCView where term='" + SelectedTerm + "' and tID='" + identity.Username + "'");
                        else
                            teachings = (Collection<Teaching>)sqlHelper.getTeachings("select * from TCView where term='" + SelectedTerm + "'");
                        break;
                    }
                case IdentityEnum.manager:
                    {
                        teachings = (Collection<Teaching>)sqlHelper.getTeachings("select * from TCView where term='" + SelectedTerm + "'");
                        break;
                    }
                default:teachings = new Collection<Teaching>();break;
            }
            displayTeachings(teachings);
        }

        private void displayTeachersOfThisTerm(bool allTeaching)
        {
            displayTeachersOfThisTerm(SelectedTerm, allTeaching);
        }

        private void headImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mainMenu.PlacementTarget = headImage;
            mainMenu.IsOpen = true;
        }

        private void thisTermB_Click(object sender, RoutedEventArgs e)
        {
            if (identity.IdentityE == IdentityEnum.student)
                displayTeachersOfThisTerm(onlyOwnTeacherCB.IsChecked == false);
            else
                displayTeachersOfThisTerm(false);
        }

        private void thisTermMI_Click(object sender, RoutedEventArgs e)
        {
            displayTeachersOfThisTerm(true);
        }

        private void Tc_teacherClickedEvent(object sender, TeacherClickedEventArgs e)
        {
            TeacherDetailUC teacherDetailUC = new TeacherDetailUC(e.Teaching);
            mainSP.Children.Clear();
            mainSP.Children.Add(teacherDetailUC);
            teaching = e.Teaching;
            refreshB.Visibility = Visibility.Visible;
        }

        private void Tc_courseClickedEvent(object sender, CourseClickedEventArgs e)
        {
            Collection<Teaching> teachings;
            if (identity.IdentityE == IdentityEnum.student && onlyOwnTeacherCB.IsChecked == true)
                teachings = (Collection<Teaching>)sqlHelper.getTeachings("select * from TCView where cID='" + e.CourseID + "' and term='" + SelectedTerm + "' and tcID in (select tcID from selectCourse where sID='" + identity.Username + "')");
            else
                teachings = (Collection<Teaching>)sqlHelper.getTeachings("select * from TCView where cID='" + e.CourseID + "' and term='" + SelectedTerm + "'");
            displayTeachings(teachings);
        }

        private void termsCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedTerm = Convert.ToString(termsCB.SelectedItem);
        }

        private void getTeacherBytIDMI_Click(object sender, RoutedEventArgs e)
        {
            InputDialog input = new InputDialog("请输入教师编号", 10);
            input.ShowDialog();
            if (input.DialogResult == false)
                return;
            else
            {
                Collection<Teaching> teachings = (Collection<Teaching>)sqlHelper.getTeachings("select * from TCView where tID='" + input.InputText + "'");
                displayTeachings(teachings);
            }
        }

        private void getCourseBycIDMI_Click(object sender, RoutedEventArgs e)
        {
            InputDialog input = new InputDialog("请输入课程编号", 10);
            input.ShowDialog();
            if (input.DialogResult == false)
                return;
            else
            {
                Collection<Teaching> teachings = (Collection<Teaching>)sqlHelper.getTeachings("select * from TCView where cID='" + input.InputText + "'");
                displayTeachings(teachings);
            }
        }

        private void changePWMI_Click(object sender, RoutedEventArgs e)
        {
            ChangePassword changepw = new ChangePassword();
            changepw.ShowDialog();
        }

        private void closeMI_Click(object sender, RoutedEventArgs e)
        {
            Close();
            //((App)Application.Current).Shutdown();
        }

        private void searchB_Click(object sender, RoutedEventArgs e)
        {
            Collection<Teaching> teachings;
            switch (searchOptionCB.SelectedIndex)
            {
                case 0:
                    {
                        teachings = (Collection<Teaching>)sqlHelper.getTeachings("select * from TCView where tName like '%" + searchTB.Text + "%'");
                        break;
                    }
                case 1:
                    {
                        teachings = (Collection<Teaching>)sqlHelper.getTeachings("select * from TCView where cName like '%" + searchTB.Text + "%'");
                        break;
                    }
                default:
                    {
                        teachings = new Collection<Teaching>();
                        break;
                    }
            }
            displayTeachings(teachings);
        }

        private void refreshB_Click(object sender, RoutedEventArgs e)
        {
            mainSP.Children.Clear();
            Teaching t = sqlHelper.getTeachingBytcID(teaching.TcID);
            mainSP.Children.Add(new TeacherDetailUC(t));
        }

        private void myRankMI_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(sqlHelper.getRankAndMarkSimplyfied((Teacher)identity.IObject).Replace("此教师得分","您的得分"),"您的排名信息为");
        }

        private void markStatMI_Click(object sender, RoutedEventArgs e)
        {
            DataWindow dataWindow = new DataWindow();
            dataWindow.Show();
        }

        private void settingsMI_Click(object sender, RoutedEventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow();
            settingWindow.ShowDialog();
        }
    }
}

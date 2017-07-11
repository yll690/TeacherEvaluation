using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeacherEvaluation.Properties;
using System.Data;
using System.Windows;

namespace TeacherEvaluation
{
    public class SqlHelper
    {
        Settings settings = Settings.Default;
        SqlConnection sqlConnection;
        Identity loginIdentity;
        string loginSP,serverName, databaseName, SqlUserName, SqlPassword;
        bool displayMessage = true;
        bool throwException = false;

        public Identity LoginIdentity
        {
            get => loginIdentity;
            set
            {
                loginIdentity = value;
                switch (LoginIdentity.IdentityE)
                {
                    case IdentityEnum.student:
                        {
                            SqlUserName = settings.studentUN;
                            SqlPassword = settings.studentPW;
                            loginSP = "sp_studentLogin";
                            break;
                        }
                    case IdentityEnum.teacher:
                        {
                            SqlUserName = settings.teacherUN;
                            SqlPassword = settings.teacherPW;
                            loginSP = "sp_teacherLogin";
                            break;
                        }
                    case IdentityEnum.manager:
                        {
                            SqlUserName = settings.managerUN;
                            SqlPassword = settings.managerPW;
                            loginSP = "sp_managerLogin";
                            break;
                        }
                }
            }
        }

        public SqlHelper(Identity identity)
        {
            serverName = settings.server;
            databaseName = settings.database;
            LoginIdentity = identity;
        }

        public bool login(string username, string password)
        {
            try
            {
                sqlConnection = new SqlConnection(constructConStr(serverName, databaseName, SqlUserName, SqlPassword));
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(loginSP, sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("username", username);
                sqlCommand.Parameters.AddWithValue("password", password);
                SqlDataReader sdr = sqlCommand.ExecuteReader();
                if (!sdr.HasRows)
                {
                    sqlConnection.Close();
                    return false;
                }
                else
                {
                    sqlConnection.Close();
                    LoginIdentity.Username = username;
                    settings.username = username;
                    settings.lastIdentity = (int)LoginIdentity.IdentityE;
                    settings.Save();
                    switch (LoginIdentity.IdentityE)
                    {
                        case IdentityEnum.student:
                            LoginIdentity.IObject = getStudent(LoginIdentity.Username); break;
                        case IdentityEnum.teacher:
                            LoginIdentity.IObject = getTeacher(LoginIdentity.Username); break;
                    }
                    ((App)Application.Current).SqlHelper = this;
                    ((App)Application.Current).Identity = LoginIdentity;
                    return true;
                }
            }
            catch (Exception e)
            {
                if (displayMessage)
                    MessageBox.Show("登录失败\n" + e.Message);
                sqlConnection.Close();
                if (throwException)
                    throw e;
                else
                    return false;
            }
        }

        private string constructConStr(string serverName, string databaseName, string SqlUserName, string SqlPassword)
        {
            return "server=" + serverName + ";database=" + databaseName + ";user=" + SqlUserName + ";pwd=" + SqlPassword;
        }

        public Comment getComment(string comID)
        {
            Comment comment;
            try
            {
                sqlConnection.Open();
                string cmdStr = "select * from commentView where comID='" + comID + "'";
                SqlCommand sqlCommand = new SqlCommand(cmdStr, sqlConnection);
                SqlDataReader sdr = sqlCommand.ExecuteReader();
                if (sdr.Read())
                {
                    comment = new Comment(sdr);
                    sqlConnection.Close();
                    return comment;
                }
                else
                {
                    sqlConnection.Close();
                    return null;
                }
            }
            catch (Exception e)
            {
                if (displayMessage)
                    MessageBox.Show("获取评论失败\n" + e.Message);
                sqlConnection.Close();
                if (throwException)
                    throw e;
                else
                    return null;
            }
        }

        public ICollection<Comment> getComments(string cmdString)
        {
            Collection<Comment> comments = new Collection<Comment>();
            try
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(cmdString, sqlConnection);
                SqlDataReader sdr = sqlCommand.ExecuteReader();
                while (sdr.Read())
                {
                    Comment comment = new Comment(sdr);
                    comments.Add(comment);
                }
                sqlConnection.Close();
                return comments;
            }
            catch (Exception e)
            {
                if (displayMessage)
                    MessageBox.Show("获取评论失败\n" + e.Message);
                sqlConnection.Close();
                if (throwException)
                    throw e;
                else
                    return null;
            }
        }

        public ICollection<Comment> getCommentsByTcID(string tcID)
        {
            return getComments("select * from commentView where tcID='" + tcID + "' order by time desc");
        }

        public ICollection<string> getStrings(SqlCommand sqlCommand, string columnName, string failMessage)
        {
            Collection<string> strings = new Collection<string>();
            try
            {
                sqlConnection.Open();
                sqlCommand.Connection = sqlConnection;
                SqlDataReader sdr = sqlCommand.ExecuteReader();
                while (sdr.Read())
                {
                    string s = Convert.ToString(sdr[columnName]);
                    strings.Add(s);
                }
                sqlConnection.Close();
                return strings;
            }
            catch (Exception e)
            {
                if (displayMessage)
                    MessageBox.Show(failMessage + e.Message);
                sqlConnection.Close();
                if (throwException)
                    throw e;
                else
                    return null;
            }
        }

        public ICollection<string> getStrings(string cmdString, string columnName)
        {
            SqlCommand sqlCommand = new SqlCommand(cmdString);
            return getStrings(sqlCommand, columnName, "获取" + columnName + "失败\n");
        }

        public ICollection<string> getStrings(string cmdString, string columnName, string failMessage)
        {
            SqlCommand sqlCommand = new SqlCommand(cmdString);
            return getStrings(sqlCommand, columnName, failMessage);
        }

        public bool deleteComment(string commentID)
        {
            return executeCommand("delete from comment where comID='" + commentID + "'", "删除评论失败\n");
        }

        public bool addReply(string cID, string reply)
        {
            return executeCommand("update comment set tReply='" + reply + "' where " +
                "comID='" + cID + "'", "回复评论失败\n");
        }

        public Student getStudent(string sID)
        {
            try
            {
                sqlConnection.Open();
                string cmdStr = "select * from student where sID='" + sID + "'";
                SqlCommand sqlCommand = new SqlCommand(cmdStr, sqlConnection);
                SqlDataReader sdr = sqlCommand.ExecuteReader();
                if (sdr.Read())
                {
                     Student student = new Student(sdr);
                    sqlConnection.Close();
                    return student;
                }
                else
                {
                    sqlConnection.Close();
                    return null;
                }
            }
            catch (Exception e)
            {
                if (displayMessage)
                    MessageBox.Show("获取学生失败\n" + e.Message);
                sqlConnection.Close();
                if (throwException)
                    throw e;
                else
                    return null;
            }
        }

        public ICollection<Teaching> getTeachingsBysIDAndTerm(string sID, string term)
        {
            Collection<Teaching> teachers = new Collection<Teaching>();
            try
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("sp_getTeachersBysID", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@sID", sID);
                sqlCommand.Parameters.AddWithValue("@term", term);
                SqlDataReader sdr = sqlCommand.ExecuteReader();
                while (sdr.Read())
                {
                    Teaching teacher = new Teaching(sdr);
                    teachers.Add(teacher);
                }
                sqlConnection.Close();
                return teachers;
            }
            catch (Exception e)
            {
                if (displayMessage)
                    MessageBox.Show("获取评论失败\n" + e.Message);
                sqlConnection.Close();
                if (throwException)
                    throw e;
                else
                    return null;
            }
        }

        public string getLatestTerm()
        {
            try
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("sp_getLatestTerm", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                SqlDataReader sdr = sqlCommand.ExecuteReader();
                if (sdr.Read())
                {
                    string term = Convert.ToString(sdr["term"]);
                    sqlConnection.Close();
                    return term;
                }
                else
                {
                    sqlConnection.Close();
                    return null;
                }
            }
            catch (Exception e)
            {
                if (displayMessage)
                    MessageBox.Show("获取当前学期失败\n" + e.Message);
                sqlConnection.Close();
                if (throwException)
                    throw e;
                else
                    return null;
            }
        }

        public ICollection<Teaching> getTeachingsOfLatestTermBysID(string sID)
        {
            return getTeachingsBysIDAndTerm(sID, getLatestTerm());
        }

        public string getTcID(string sID, string tID, string term)
        {
            try
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("sp_getTcID", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("sID", sID);
                sqlCommand.Parameters.AddWithValue("tID", tID);
                sqlCommand.Parameters.AddWithValue("term", term);
                SqlDataReader sdr = sqlCommand.ExecuteReader();
                if (sdr.Read())
                {
                    string tcID = Convert.ToString(sdr["tcID"]);
                    sqlConnection.Close();
                    return tcID;
                }
                else
                {
                    sqlConnection.Close();
                    return null;
                }
            }
            catch (Exception e)
            {
                if (displayMessage)
                    MessageBox.Show("获取授课编号失败\n" + e.Message);
                sqlConnection.Close();
                if (throwException)
                    throw e;
                else
                    return null;
            }
        }

        public string getTcIDByTC(string tID, string cID)
        {

            SqlCommand sqlCommand = new SqlCommand("sp_getTcIDByTC");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("tID", tID);
            sqlCommand.Parameters.AddWithValue("sID", cID);
            Collection<string> tcIDs = (Collection<string>)getStrings(sqlCommand, "markID", "获取授课编号失败\n");
            if (tcIDs != null && tcIDs.Count > 0)
                return tcIDs[0];
            else
            {
                if (displayMessage)
                    MessageBox.Show("获取授课编号失败\n");
                return null;
            }
        }

        public string getTcID(string sID, string tID)
        {
            return getTcID(sID, tID, getLatestTerm());
        }

        public bool addMark(DetailedMark mark, string sID, string tcID)
        {
            SqlCommand sqlCommand = new SqlCommand("sp_submitMark", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("sID", sID);
            sqlCommand.Parameters.AddWithValue("tcID", tcID);
            sqlCommand.Parameters.AddWithValue("totalMark", mark.TotalMark);
            sqlCommand.Parameters.AddWithValue("mark1", mark.Mark1);
            sqlCommand.Parameters.AddWithValue("mark2", mark.Mark2);
            sqlCommand.Parameters.AddWithValue("mark3", mark.Mark3);
            sqlCommand.Parameters.AddWithValue("mark4", mark.Mark4);
            sqlCommand.Parameters.AddWithValue("mark5", mark.Mark5);
            return executeCommand(sqlCommand, "添加评分失败\n");
        }

        public bool deleteMark(string markID)
        {
            SqlCommand sqlCommand = new SqlCommand("sp_deleteMark");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("markID", markID);
            return executeCommand(sqlCommand, "删除评分失败\n");
        }

        public string isGraded(string sID, string tcID)
        {
            SqlCommand sqlCommand = new SqlCommand("sp_isGraded");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("sID", sID);
            sqlCommand.Parameters.AddWithValue("tcID", tcID);
            Collection<string> markIDs = (Collection<string>)getStrings(sqlCommand, "markID", "判断是否已评分失败\n");
            if (markIDs != null)
                if (markIDs.Count == 1)
                    return markIDs[0];
                else if (markIDs.Count > 1)
                {
                    if (displayMessage)
                        MessageBox.Show("判断是否已评分失败\n");
                    return null;
                }
                else return "";
            else
            {
                if (displayMessage)
                    MessageBox.Show("判断是否已评分失败\n");
                return null;
            }
        }

        public Teacher getTeacher(string tID)
        {
            try
            {
                sqlConnection.Open();
                string cmdStr = "select * from teacher where tID='" + tID + "'";
                SqlCommand sqlCommand = new SqlCommand(cmdStr, sqlConnection);
                SqlDataReader sdr = sqlCommand.ExecuteReader();
                if (sdr.Read())
                {
                    Teacher teacher = new Teacher(sdr);
                    sqlConnection.Close();
                    return teacher;
                }
                else
                {
                    sqlConnection.Close();
                    return null;
                }
            }
            catch (Exception e)
            {
                if (displayMessage)
                    MessageBox.Show("获取教师失败\n" + e.Message);
                sqlConnection.Close();
                if (throwException)
                    throw e;
                else
                    return null;
            }
        }

        public ICollection<Teaching> getTeachings(string cmdStr)
        {
            Collection<Teaching> teachings = new Collection<Teaching>();
            try
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(cmdStr, sqlConnection);
                SqlDataReader sdr = sqlCommand.ExecuteReader();
                while (sdr.Read())
                {
                    Teaching teaching = new Teaching(sdr);
                    teachings.Add(teaching);
                }
                sqlConnection.Close();
                return teachings;
            }
            catch (Exception e)
            {
                if (displayMessage)
                    MessageBox.Show("获取教学信息失败\n" + e.Message);
                sqlConnection.Close();
                if (throwException)
                    throw e;
                else
                    return null;
            }
        }

        public Teaching getTeachingBytcID(string tcID)
        {
            return ((Collection<Teaching>)getTeachings("select * from TCView where tcID='" + tcID + "'"))[0];
        }

        public ICollection<string> getTerms()
        {
            return getStrings("select distinct term from teachingCourse order by term desc",
                "term", "获取学期失败\n");
        }

        public ICollection<float> getFloats(SqlCommand sqlCommand, string columnName, string failMessage)
        {
            Collection<float> floats = new Collection<float>();
            try
            {
                sqlConnection.Open();
                sqlCommand.Connection = sqlConnection;
                SqlDataReader sdr = sqlCommand.ExecuteReader();
                while (sdr.Read())
                {
                    float f = Convert.ToSingle(sdr[columnName]);
                    floats.Add(f);
                }
                sqlConnection.Close();
                return floats;
            }
            catch (Exception e)
            {
                if (displayMessage)
                    MessageBox.Show(failMessage + e.Message);
                sqlConnection.Close();
                if (throwException)
                    throw e;
                else
                    return null;
            }
        }

        public float getFloat(SqlCommand sqlCommand, string columnName, string failMessage)
        {
            return ((Collection<float>)getFloats(sqlCommand, columnName, failMessage))[0];
        }

        public ICollection<float> getMarksBytcID(string tcID)
        {
            Collection<float> totalMarks = new Collection<float>();
            try
            {
                sqlConnection.Open();
                string cmdStr = "select totalMark from mark where tcID='" + tcID + "'";
                SqlCommand sqlCommand = new SqlCommand(cmdStr, sqlConnection);
                SqlDataReader sdr = sqlCommand.ExecuteReader();
                while (sdr.Read())
                {
                    float totalMark = Convert.ToSingle(sdr["totalMark"]);
                    totalMarks.Add(totalMark);
                }
                sqlConnection.Close();
                return totalMarks;
            }
            catch (Exception e)
            {
                if (displayMessage)
                    MessageBox.Show("获取总分失败\n" + e.Message);
                sqlConnection.Close();
                if (throwException)
                    throw e;
                else
                    return null;
            }
        }

        public float getMarkPercent(string tcID, float from, float to)
        {
            Collection<float> totalMarks = (Collection<float>)getMarksBytcID(tcID);
            int num = 0;
            foreach (float f in totalMarks)
                if (f > from && f <= to)
                    num++;
            return num == 0 ? 0 : num / (float)totalMarks.Count;
        }

        public bool executeCommand(SqlCommand sqlCommand, string failMessage)
        {
            try
            {
                sqlConnection.Open();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                return true;
            }
            catch (Exception e)
            {
                if (displayMessage)
                    MessageBox.Show(failMessage + e.Message);
                sqlConnection.Close();
                if (throwException)
                    throw e;
                else
                    return false;
            }
        }

        public bool executeCommand(string cmdString, string failMessage)
        {
            return executeCommand(new SqlCommand(cmdString), failMessage);
        }

        public bool executeCommand(string cmdString)
        {
            return executeCommand(cmdString, "操作" + cmdString + "失败\n");
        }

        public bool addComment(string sID, string tcID, string content)
        {
            return executeCommand("insert into comment(sID,tcID,cContent) " +
                "values('" + sID + "','" + tcID + "','" + content + "')", "提交评论失败\n");
        }

        public bool changePassword(Identity identity, string newPassword)
        {
            switch (identity.IdentityE)
            {
                case IdentityEnum.student:
                    return executeCommand("update student set password='" + newPassword +
                        "' where sID='" + identity.Username + "'", "更改密码失败\n");
                case IdentityEnum.teacher:
                    return executeCommand("update teacher set password='" + newPassword +
                        "' where tID='" + identity.Username + "'", "更改密码失败\n");
                case IdentityEnum.manager:
                    return executeCommand("update manager set password='" + newPassword +
                        "' where mID='" + identity.Username + "'", "更改密码失败\n");
                default: return false;
            }
        }

        public DataSet getDataSet(string cmdString)
        {
            try
            {
                SqlDataAdapter sqlada = new SqlDataAdapter(cmdString, sqlConnection);
                DataSet dataSet = new DataSet();
                sqlada.Fill(dataSet, "table");
                return dataSet;
            }
            catch (Exception e)
            {
                if (displayMessage)
                    MessageBox.Show("获取表失败\n" + e.Message);
                sqlConnection.Close();
                if (throwException)
                    throw e;
                else
                    return null;
            }
        }

        public ICollection<int> getIntergers(SqlCommand sqlCommand, string columnName, string failMessage)
        {
            Collection<int> intergers = new Collection<int>();
            try
            {
                sqlConnection.Open();
                sqlCommand.Connection = sqlConnection;
                SqlDataReader sdr = sqlCommand.ExecuteReader();
                while (sdr.Read())
                {
                    int interger = Convert.ToInt32(sdr[columnName]);
                    intergers.Add(interger);
                }
                sqlConnection.Close();
                return intergers;
            }
            catch (Exception e)
            {
                if (displayMessage)
                    MessageBox.Show(failMessage + e.Message);
                sqlConnection.Close();
                if (throwException)
                    throw e;
                else
                    return null;
            }
        }

        public int getInterger(SqlCommand sqlCommand, string columnName, string failMessage)
        {
            return ((Collection<int>)getIntergers(sqlCommand, columnName, failMessage))[0];
        }

        public int getInterger(string cmdString, string columnName, string failMessage)
        {
            return getInterger(new SqlCommand(cmdString), columnName, failMessage);
        }

        //获取排名和人数
        #region 
        private int getRankOfAll(string tID)
        {
            string cmdStr = "select rankNum from (select row_number() over(order by " +
                "mark desc) as rankNum,tID from teacher) rankList where tID='" + tID + "'";
            return getInterger(new SqlCommand(cmdStr), "rankNum", "获取全校排名失败\n");
        }

        private int getRankOfInstitute(string tID)
        {
            string cmdStr = "select rankNum from (select row_number() over(order by " +
                "mark desc) as rankNum,tID,iID from teacher) rankList where tID='" + tID +
                "' and iID in (select iID from teacher where tID='" + tID + "')";
            return getInterger(new SqlCommand(cmdStr), "rankNum", "获取全校排名失败\n");
        }

        private int getRankOfCourse(string tcID)
        {
            string cmdStr = "select rankNum from (select row_number() over(order by " +
                "mark desc) as rankNum,tcID from teachingCourse) rankList where tcID='" + tcID + "'";
            return getInterger(new SqlCommand(cmdStr), "rankNum", "获取课程排名失败\n");
        }

        private int getNumOfAllTeachers()
        {
            string cmdStr = "select count(*) as num from teacher";
            return getInterger(cmdStr, "num", "获取全校教师人数失败\n");
        }

        private int getNumOfInstituteTeachers(string tID)
        {
            string cmdStr = "select count(*) as num from teacher where iID in " +
                "(select iID from teacher where tID='" + tID + "')";
            return getInterger(cmdStr, "num", "获取学院教师人数失败\n");
        }

        private int getNumOfCourseTeachers(string cID)
        {
            string cmdStr = "select count(*) as num from teachingCourse where cID='" + cID + "'";
            return getInterger(cmdStr, "num", "获取此课程教师人数失败\n");
        }
        #endregion

        public Rank getRank(string tID)
        {
            Rank rank = new Rank();
            rank.numOfAllTeachers = getNumOfAllTeachers();
            rank.numOfInstituteTeachers = getNumOfInstituteTeachers(tID);
            rank.rankOfAllTeachers = getRankOfAll(tID);
            rank.rankOfInstituteTeachers = getRankOfInstitute(tID);
            return rank;
        }

        public Rank getRank(Teaching teaching)
        {
            Rank rank = getRank(teaching.Teacher.TeacherID);
            rank.numOfCourseTeachers = getNumOfCourseTeachers(teaching.CourseID);
            rank.rankOfCourseTeachers = getRankOfCourse(teaching.TcID);
            return rank;
        }

        public string getRankAndMark(Teaching teaching)
        {
            Rank rank = getRank(teaching);
            string rankAndMark = "此教师得分：" + teaching.Teacher.Mark + ",共" + teaching.Teacher.NumOfMarks + "人评分\n" +
                "此课程得分：" + teaching.Mark + ",共" + teaching.NumOfMarks + "人评分\n" +
                "全校排名：" + rank.rankOfAllTeachers + "/" + rank.numOfAllTeachers + "\n" +
                "学院排名：" + rank.rankOfInstituteTeachers + "/" + rank.numOfInstituteTeachers + "\n" +
                "课程排名：" + rank.rankOfCourseTeachers + "/" + rank.numOfCourseTeachers;
            return rankAndMark;
        }

        public string getRankAndMarkSimplyfied(Teacher teacher)
        {
            Rank rank = getRank(teacher.TeacherID);
            string rankAndMark = "此教师得分：" + teacher.Mark + ",共" + teacher.NumOfMarks + "人评分\n" +
                "全校排名：" + rank.rankOfAllTeachers + "/" + rank.numOfAllTeachers + "\n" +
                "学院排名：" + rank.rankOfInstituteTeachers + "/" + rank.numOfInstituteTeachers;
            return rankAndMark;
        }

        public bool isOwnTeacher(string sID,string tcID)
        {
            try
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("select * from selectCourse where " +
                    "sID='" + sID + "' and tcID='" + tcID + "'", sqlConnection);
                SqlDataReader sdr = sqlCommand.ExecuteReader();
                if (sdr.HasRows)
                {
                    sqlConnection.Close();
                    return true;
                }
                else
                {
                    sqlConnection.Close();
                    return false;
                }
            }
            catch (Exception e)
            {
                if (displayMessage)
                    MessageBox.Show("判断是否是自己老师失败\n" + e.Message);
                sqlConnection.Close();
                if (throwException)
                    throw e;
                else
                    return false;
            }
        }
    }
}
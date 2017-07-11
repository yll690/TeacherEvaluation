using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherEvaluation
{
    public class Comment
    {
        string studentName;
        string studentpic;
        string commentID;
        string content;
        bool hasReply;
        string teacherRelpy;
        DateTime time;
        bool hideFromStudent;
        bool hideFromTeacher;

        public string StudentName { get => studentName; set => studentName = value; }
        public string Studentpic { get => studentpic; set => studentpic = value; }
        public string CommentID { get => commentID; set => commentID = value; }
        public string Content { get => content; set => content = value; }
        public bool HasReply { get => hasReply; set => hasReply = value; }
        public string TeacherRelpy { get => teacherRelpy; set => teacherRelpy = value; }
        public DateTime Time { get => time; }
        public bool HideFromStudent { get => hideFromStudent; set => hideFromStudent = value; }
        public bool HideFromTeacher { get => hideFromTeacher; set => hideFromTeacher = value; }

        public Comment()
        {
            StudentName = "";
            Studentpic = "";
            CommentID = "";
            Content = "";
            hasReply = false;
            TeacherRelpy = "";
            time = DateTime.Now;
        }

        public Comment(SqlDataReader sdr)
        {
            StudentName = Convert.ToString(sdr["sName"]);
            Studentpic = Convert.ToString(sdr["picture"]);
            CommentID = Convert.ToString(sdr["comID"]);
            Content = Convert.ToString(sdr["cContent"]);
            TeacherRelpy = Convert.ToString(sdr["tReply"]);
            if (TeacherRelpy == "")
                HasReply = false;
            else
                HasReply = true;
            time = Convert.ToDateTime(sdr["time"]);
            HideFromStudent = Convert.ToBoolean(sdr["hideFromStudent"]);
            HideFromTeacher=Convert.ToBoolean(sdr["hideFromTeacher"]);
        }
    }
}

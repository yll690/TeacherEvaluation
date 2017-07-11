using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherEvaluation
{
    public class Student
    {
        string name;
        string studentID;
        string picture;
        int session;
        bool hideFromStudent;
        bool hideFromTeacher;

        public string Name { get => name; private set => name = value; }
        public string StudentID { get => studentID; private set => studentID = value; }
        public string Picture { get => picture; private set => picture = value; }
        public int Session { get => session; private set => session = value; }
        public bool HideFromStudent { get => hideFromStudent; set => hideFromStudent = value; }
        public bool HideFromTeacher { get => hideFromTeacher; set => hideFromTeacher = value; }

        //public Student(string sName, string sID, string picture, int session)
        //{
        //    Name = sName;
        //    StudentID = sID;
        //    Picture = picture;
        //    Session = session;
        //}

        public Student(SqlDataReader sdr)
        {
            Name = Convert.ToString(sdr["sName"]);
            StudentID = Convert.ToString(sdr["sID"]);
            Picture = Convert.ToString(sdr["picture"]);
            Session = Convert.ToInt32(sdr["session"]);
            HideFromStudent=Convert.ToBoolean(sdr["hideFromStudent"]);
            HideFromTeacher = Convert.ToBoolean(sdr["hideFromTeacher"]);
        }
    }
}

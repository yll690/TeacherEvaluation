using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherEvaluation
{
    public class Teaching
    {
        Teacher teacher;
        float mark;
        int numOfMarks;
        string courseName;
        string courseID;
        string tcID;
        string term;

        public Teacher Teacher { get => teacher; set => teacher = value; }
        public float Mark { get => mark; set => mark = value; }
        public int NumOfMarks { get => numOfMarks; set => numOfMarks = value; }
        public string CourseName { get => courseName; set => courseName = value; }
        public string CourseID { get => courseID; set => courseID = value; }
        public string TcID { get => tcID; set => tcID = value; }
        public string Term { get => term; set => term = value; }

        //public Teaching(string name, string tID, string profile, string picture, float mark, int numOfMark, string cName, string cID)
        //{
        //    Name = name;
        //    TeacherID = tID;
        //    Profile = profile;
        //    Picture = picture;
        //    TcMark = mark;
        //    TcNumOfMarks = numOfMark;
        //    CourseName = cName;
        //    CourseID = cID;
        //}

        public Teaching(SqlDataReader sdr)
        {
            Teacher = new Teacher(sdr);
            Mark = Convert.ToSingle(sdr["tcMark"]);
            NumOfMarks = Convert.ToInt32(sdr["tcNumOfMarks"]);
            CourseName = Convert.ToString(sdr["cName"]);
            CourseID = Convert.ToString(sdr["cID"]);
            TcID = Convert.ToString(sdr["tcID"]);
            term = Convert.ToString(sdr["term"]);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherEvaluation
{
    public class Teacher
    {
        string name;
        string teacherID;
        string profile;
        string picture;
        float mark;
        int numOfMarks;
        string instituteID;

        public string Name { get => name; set => name = value; }
        public string TeacherID { get => teacherID; set => teacherID = value; }
        public string Profile { get => profile; set => profile = value; }
        public string Picture { get => picture; set => picture = value; }
        public float Mark { get => mark; set => mark = value; }
        public int NumOfMarks { get => numOfMarks; set => numOfMarks = value; }
        public string InstituteID { get => instituteID; set => instituteID = value; }

        public Teacher(SqlDataReader sdr)
        {
            Name = Convert.ToString(sdr["tName"]);
            TeacherID = Convert.ToString(sdr["tID"]);
            Profile = Convert.ToString(sdr["profile"]);
            Picture = Convert.ToString(sdr["picture"]);
            mark = Convert.ToSingle(sdr["mark"]);
            numOfMarks = Convert.ToInt32(sdr["numOfMarks"]);
            InstituteID = Convert.ToString(sdr["iID"]);
        }
    }
}

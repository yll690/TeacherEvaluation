using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherEvaluation
{
    public class CourseClickedEventArgs:EventArgs
    {
        string courseID;

        public string CourseID { get => courseID;}

        public CourseClickedEventArgs(string courseID)
        {
            this.courseID = courseID;
        }
    }
}

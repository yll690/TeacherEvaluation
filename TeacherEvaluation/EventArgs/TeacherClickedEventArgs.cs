using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherEvaluation
{
    public class TeacherClickedEventArgs:EventArgs
    {
        Teaching teaching;

        public Teaching Teaching { get => teaching; }

        public TeacherClickedEventArgs(Teaching teaching)
        {
            this.teaching = teaching;
        }
    }
}

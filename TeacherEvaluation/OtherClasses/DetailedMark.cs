using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeacherEvaluation
{
    public class DetailedMark
    {
        private short totalMark;
        private short mark1;
        private short mark2;
        private short mark3;
        private short mark4;
        private short mark5;

        public short TotalMark { get => totalMark; set => totalMark = value; }
        public short Mark1 { get => mark1; set => mark1 = value; }
        public short Mark2 { get => mark2; set => mark2 = value; }
        public short Mark3 { get => mark3; set => mark3 = value; }
        public short Mark4 { get => mark4; set => mark4 = value; }
        public short Mark5 { get => mark5; set => mark5 = value; }
    }
}

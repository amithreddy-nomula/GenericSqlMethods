using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoClasses
{
    public class DtoEMP
    {
        public int Empno { set; get; }
        public string ename { get; set; }
        public string job { get; set; }
        public int mgr { get; set; }
        public DateTime hiredate { get; set; }
        public decimal sal { get; set; }
        public decimal comm { get; set; }
        public int deptno { get; set; }

    }
    public class DtoDept
    {
        public int deptno { set; get; }
        public string dname { set; get; }
        public string location { get; set; }

    }
}

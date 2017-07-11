using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace TeacherEvaluation
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private Identity identity = new Identity(IdentityEnum.student, "");
        //private SqlHelper sqlHelper = new SqlHelper();
        private SqlHelper sqlHelper;
        private string selectedTerm;

        public Identity Identity { get => identity;
            set
            {
                identity = value;
                //sqlHelper.LoginIdentity = value;
            }
        }
        public SqlHelper SqlHelper { get => sqlHelper; set => sqlHelper = value; }
        public string SelectedTerm { get => selectedTerm; set => selectedTerm = value; }
    }
}

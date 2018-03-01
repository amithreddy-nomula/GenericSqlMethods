using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DtoClasses;
namespace GenericSQLMethods
{
    class Test
    {      
       public static void Main(string[] args)
        {          
            SqlMethodsClass p = new SqlMethodsClass();
            p.ConnectToSqlClient();
            Console.WriteLine("Enter the Dto table Name on which you want to work");
            p.Insert(p.SetDataToInsert(new DtoEMP()));
            p.Select<DtoEMP>(new DtoEMP());
            p.Delete<DtoEMP>(new DtoEMP());
            p.Update(new DtoEMP());
        }
    }
}

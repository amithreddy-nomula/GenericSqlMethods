using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
namespace GenericSQLMethods
{
    
    class SqlMethodsClass
    {
        SqlConnection conn;
        SqlCommand command;

        public void ConnectToSqlClient()
        {
            conn = new SqlConnection("data source=GGKU4MPC62;database=Office;integrated security=sspi;");
        }
        public void Insert<T>(T t)
        {
            PropertyInfo[] propertyInfo = t.GetType().GetProperties();
            string insertQuery = string.Empty;
            Type type = typeof(string);
            foreach (PropertyInfo info in propertyInfo)
            {
                object value = info.GetValue(t);

                if (info.PropertyType == type || info.PropertyType == typeof(DateTime))
                    insertQuery = insertQuery + $",'{value}'";
                else
                    insertQuery += $", {value}";

            }
            var arr = (t.GetType() + "").Split('.');
            var tablename = arr[1].Substring(3);
            insertQuery = insertQuery.Substring(1);
            command = new SqlCommand("insert into " + tablename + " values(" + insertQuery + ");", conn);
            conn.Open();
            int rowsAffected = command.ExecuteNonQuery();        
            conn.Close();
        }
        public DataSet Select<T>(T t)
        {
            var arr = (t.GetType() + "").Split('.');
            var tablename = arr[1].Substring(3);
            command = new SqlCommand("select * from " + tablename, conn);
            DataSet ds = new DataSet();
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                adapter.Fill(ds);
            }
            return ds;           
        }
        public void PrintDataSet(DataSet dataSet)
        {
            IDataReader rdr = dataSet.CreateDataReader();
            for (int i = 0; i < rdr.FieldCount; i++)
            {
                Console.Write(rdr.GetName(i) + "\t\t");
            }
            Console.WriteLine();
            while (rdr.Read())
            {
                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    Console.Write(rdr.GetValue(i) + "\t\t");
                }
                Console.WriteLine();
            }
        }
        string[] GetPrimaryKeys(SqlConnection connection, string tableName)
        {
            using (SqlDataAdapter adapter = new SqlDataAdapter("select * from " + tableName, connection))
            using (DataTable table = new DataTable(tableName))
            {
                return adapter
                    .FillSchema(table, SchemaType.Mapped)
                    .PrimaryKey.Select(c => c.ColumnName)
                    .ToArray();
            }
        }
        public void Delete<T>(T t)
        {
            var arr = (t.GetType() + "").Split('.');
            var tablename = arr[1].Substring(3);
            // int id = Convert.ToInt32(Console.ReadLine());
            string[] primarykeys = GetPrimaryKeys(conn, tablename);
            Console.WriteLine(primarykeys);
            conn.Open();
            for (int j = 0; j < primarykeys.Length; j++)
            {
                command = new SqlCommand("select " + primarykeys[j] + " from " + tablename,conn);
                SqlDataReader rdr = command.ExecuteReader();
                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    Console.Write(rdr.GetName(i) + "\t\t");
                }
                Console.WriteLine();
                while (rdr.Read())
                {
                    for (int i = 0; i < rdr.FieldCount; i++)
                    {
                        Console.Write(rdr.GetValue(i) + "\t\t");
                    }
                    Console.WriteLine();
                }
            }
            conn.Close();
            Console.WriteLine("Enter the primary key which you want to delete");
            var pk=Convert.ChangeType(Console.ReadLine(),primarykeys[0].GetType());
            conn.Open();
           command = new SqlCommand("Delete  from " + tablename + " Where " + primarykeys[0] + "=" +pk, conn);
            Console.WriteLine("Number of rows affected");
            int rowsAffected = command.ExecuteNonQuery();
            Console.WriteLine(rowsAffected);
            conn.Close();

        }
        
        public void Update<T>(T t)
        {
            var arr = (t.GetType() + "").Split('.');
            var tablename = arr[1].Substring(3);
            string[] primarykeys = GetPrimaryKeys(conn, tablename);
            Console.WriteLine(primarykeys);
            conn.Open();
            for (int j = 0; j < primarykeys.Length; j++)
            {
                command = new SqlCommand("select " + primarykeys[j] + " from " + tablename, conn);
                SqlDataReader rdr = command.ExecuteReader();
                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    Console.Write(rdr.GetName(i) + "\t\t");
                }
                Console.WriteLine();
                while (rdr.Read())
                {
                    for (int i = 0; i < rdr.FieldCount; i++)
                    {
                        Console.Write(rdr.GetValue(i) + "\t\t");
                    }
                    Console.WriteLine();
                }
            }
            conn.Close();
            Console.WriteLine("Enter the primary key value which you want to update");
            var pk = Convert.ChangeType(Console.ReadLine(), primarykeys[0].GetType());
            PropertyInfo[] propertyInfo = t.GetType().GetProperties();
            conn.Open();
            foreach (PropertyInfo info in propertyInfo)
            {
                Console.WriteLine("Enter the value for " + info.Name);
                var value = Console.ReadLine();
                info.SetValue(t, Convert.ChangeType(value, info.PropertyType));             
                command = new SqlCommand("update " + tablename + " set " + info.Name + "=' " + value + "' where " + primarykeys[0] + "= " + pk,conn);
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine(rowsAffected);
               
            }
            conn.Close();


        }
        public T SetDataToInsert<T>(T t)
        {
            PropertyInfo[] propertyInfo = t.GetType().GetProperties();       
            foreach (PropertyInfo info in propertyInfo)
            {
                Console.WriteLine("Enter the value for " + info.Name);
                var value = Console.ReadLine();
                info.SetValue(t, Convert.ChangeType(value, info.PropertyType));
                
            }
            return t;
        }
       
    }
}

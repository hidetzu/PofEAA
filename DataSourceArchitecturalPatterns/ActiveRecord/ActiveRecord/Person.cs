using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveRecord
{
    class Person
    {
        public int Id { set; get; }
        public string LastName { set; get; }
        public string FirstName { set; get; }
        public int NumberOfDependents { set; get; }

        private const string findStatement = @"SELECT ID, lastname, firstname, number_of_dependents
FROM Persons WHERE ID=@ID";

        private const string insertStatement = @"INSERT INTO Persons 
(ID, lastname, firstname, number_of_dependents)
values
(@ID, @lastname, @firstname, @number_of_dependents);";

        private const string updateStatement = @"UPDATE Persons 
SET lastname=@lastname, firstname=@firstname, number_of_dependents=@number_of_dependents
WHERE ID=@ID;";

        private const string deleteStatement = @"DELETE FROM Persons WHERE ID=@ID;";

        public static Person Find(int id)
        {
            Person person = Registry.GetPerson(id);

            if(person != null)
                return person;

            using (SqlConnection cn = CreateConnection())
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand(findStatement, cn))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ID", id);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        person = Person.Load(reader);
                        Registry.AddPerson(person);
                    }
                }

                cn.Close();
            }
            return person;
        }

        public static Person Load(SqlDataReader reader)
        {
            if (reader == null)
                return null;

            reader.Read();
            var id = (int)reader["ID"];

            Person person = null;
            person = Registry.GetPerson(id);
            if (person != null)
                return person;

            person = new Person()
            {
                Id = (int)reader["ID"],
                LastName = reader["lastname"].ToString(),
                FirstName = reader["firstname"].ToString(),
                NumberOfDependents = (int)reader["number_of_dependents"]
            };
            return person;
        }

        public void Insert()
        {
            using (SqlConnection cn = CreateConnection())
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand(insertStatement, cn))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ID", this.Id);
                    cmd.Parameters.AddWithValue("@lastname", this.LastName);
                    cmd.Parameters.AddWithValue("@firstname", this.FirstName);
                    cmd.Parameters.AddWithValue("@number_of_dependents", this.NumberOfDependents);
                    cmd.ExecuteNonQuery();
                }

                cn.Close();
            }
        }

        public void Update()
        {
            using (SqlConnection cn = CreateConnection())
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand(updateStatement, cn))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ID", this.Id);
                    cmd.Parameters.AddWithValue("@lastname", this.LastName);
                    cmd.Parameters.AddWithValue("@firstname", this.FirstName);
                    cmd.Parameters.AddWithValue("@number_of_dependents", this.NumberOfDependents);
                    cmd.ExecuteNonQuery();
                }

                cn.Close();
            }
        }

        public void Delete()
        {
            using (SqlConnection cn = CreateConnection())
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand(deleteStatement, cn))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ID", this.Id);
                    cmd.ExecuteNonQuery();
                }

                cn.Close();
            }
        }

        private static SqlConnection CreateConnection()
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True";
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = connectionString;

            return cn;
        }
    }
}

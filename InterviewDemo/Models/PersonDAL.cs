using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace InterviewDemo.Models
{
    public class PersonDAL
    {
        public string connectString { get; set; }

        public PersonDAL(string connectString)
        {
            this.connectString = connectString;
        }

        public List<Person> getAllPeople()
        {
            List<Person> result = new List<Person>();
            List<int> keys = new List<int>();
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                conn.Open();
                SqlCommand select = new SqlCommand("SELECT Id FROM Person",conn);
                using (SqlDataReader reader = select.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        keys.Add(reader.GetInt32(0));
                    }
                }
            }
            foreach (int i in keys)
            {
                result.Add(getPerson(i));
            }       
            
            return result;
        }

        public Person getPerson(int id)
        {
            Person person = new Person();
            person.Address = new Address();
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                //Get the Person objects from the database
                conn.Open();
                SqlCommand select = new SqlCommand("SELECT * FROM Person WHERE Id = @Id", conn);
                select.Parameters.Add(new SqlParameter("Id", id));
                using (SqlDataReader reader = select.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        person.Id = id;
                        person.Address.Id = reader.GetInt32(1);
                        person.FirstName = reader.GetString(2);
                        person.LastName = reader.GetString(3);
                        person.BirthDate = reader.GetDateTime(4);
                    }
                }
                SqlCommand addressSelect = new SqlCommand("SELECT * FROM Address WHERE Id = @Id", conn);
                addressSelect.Parameters.Add(new SqlParameter("Id", person.Address.Id));
                using (SqlDataReader reader = addressSelect.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        person.Address.Street = reader.GetString(1);
                        person.Address.City = reader.GetString(2);
                        person.Address.State = reader.GetString(3);
                        person.Address.ZIP = reader.GetString(4);
                    }
                }
            }
            return person;
        }

        public void update(Person person)
        {
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                conn.Open();
                using (SqlCommand update = new SqlCommand("UPDATE Person SET FirstName = @FirstName, LastName = @LastName, BirthDate = @BirthDate WHERE Id = @Id", conn))
                {
                    update.Parameters.AddWithValue("@Id", person.Id);
                    update.Parameters.AddWithValue("@FirstName", person.FirstName);
                    update.Parameters.AddWithValue("@LastName", person.LastName);
                    update.Parameters.AddWithValue("@BirthDate", person.BirthDate);
                    update.ExecuteNonQuery();
                }
                using (SqlCommand select = new SqlCommand("SELECT AddressId FROM Person WHERE Id = @Id", conn))
                {
                    select.Parameters.AddWithValue("@Id", person.Id);
                    using (SqlDataReader reader = select.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            person.Address.Id = reader.GetInt32(0);
                        }
                    }
                }
                using (SqlCommand addressUpdate = new SqlCommand("UPDATE Address SET Street = @Street, City = @City, State = @State, ZIP = @ZIP WHERE Id = @Id", conn))
                {
                    addressUpdate.Parameters.AddWithValue("@Id", person.Address.Id);
                    addressUpdate.Parameters.AddWithValue("@Street", person.Address.Street);
                    addressUpdate.Parameters.AddWithValue("@City", person.Address.City);
                    addressUpdate.Parameters.AddWithValue("@State", person.Address.State);
                    addressUpdate.Parameters.AddWithValue("@ZIP", person.Address.ZIP);
                    addressUpdate.ExecuteNonQuery();
                }
            }
        }

        public void create(Person person)
        {
            //Create new record in the database matching the input Person object
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                conn.Open();
                //Insert address record first to obtain AddressId
                int addressId;
                using (SqlCommand addressInsert = new SqlCommand("INSERT INTO Address (Street, City, State, ZIP) VALUES (@Street, @City, @State, @ZIP); SELECT Scope_Identity()", conn))
                {
                    addressInsert.Parameters.AddWithValue("@Street", person.Address.Street);
                    addressInsert.Parameters.AddWithValue("@City", person.Address.City);
                    addressInsert.Parameters.AddWithValue("@State", person.Address.State);
                    addressInsert.Parameters.AddWithValue("@ZIP", person.Address.ZIP);
                    addressId = Convert.ToInt32(addressInsert.ExecuteScalar());
                }
                using (SqlCommand insert = new SqlCommand("INSERT INTO Person (AddressId, FirstName, LastName, BirthDate) VALUES (@AddressId, @FirstName, @LastName, @BirthDate)",conn))
                {
                    insert.Parameters.AddWithValue("@AddressId", addressId);
                    insert.Parameters.AddWithValue("@FirstName", person.FirstName);
                    insert.Parameters.AddWithValue("@LastName", person.LastName);
                    insert.Parameters.AddWithValue("@BirthDate",person.BirthDate);
                    insert.ExecuteNonQuery();
                }
            }
        }

        public void delete(int id)
        {
            //Delete database record responding to given Id
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                conn.Open();
                using (SqlCommand delete = new SqlCommand("DELETE FROM Person WHERE Id = @Id", conn))
                {
                    delete.Parameters.AddWithValue("@Id", id);
                    delete.ExecuteNonQuery();
                }
            }
        }
    }
}
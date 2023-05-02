using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web_Api_EMP.Models;

namespace Web_Api_EMP.Controllers
{
    public class UserController : ApiController
    { 
            [HttpPost]
            [Route("api/Register")]
            public HttpResponseMessage Register(User user)
            {
                try
                {
                    // Get the connection string from your web.config file
                    string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

                    // Create a new SqlConnection object
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        // Open the connection
                        connection.Open();

                        // Create a SqlCommand object with a parameterized query
                        string query = "INSERT INTO Users (Name, Age) VALUES (@Name, @Age)";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            // Set the parameter values
                            command.Parameters.AddWithValue("@Name", user.Name);
                            command.Parameters.AddWithValue("@Age", user.Age);

                            // Execute the query
                            int rowsAffected = command.ExecuteNonQuery();

                            // Return a response message indicating success
                            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, user);
                            response.Headers.Location = new Uri(Request.RequestUri + "/" + user.Id.ToString());
                            return response;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Return a response message indicating failure
                    HttpResponseMessage response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
                    return response;
                }
            }

       
        
            [HttpPost]
            public IEnumerable<User> Get()
            {
                // Get the connection string from your web.config file
                string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

                // Create a new SqlConnection object
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the connection
                    connection.Open();

                    // Create a SqlCommand object with a select query
                    string query = "SELECT * FROM Users";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Execute the query and create a list of User objects
                        List<User> users = new List<User>();
					using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                User user = new User();
                                user.Id = (int)reader["Id"];
                                user.Name = (string)reader["Name"];
                                user.Age = (int)reader["Age"];
                                users.Add(user);
                            }
                        }

                        // Return the list of User objects
                        return users;
                    }
                }
            }
        
            [HttpPost]
            public User Get(int id)
            {
                // Get the connection string from your web.config file
                string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

                // Create a new SqlConnection object
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the connection
                    connection.Open();

                    // Create a SqlCommand object with a select query
                    string query = "SELECT * FROM Users WHERE Id = @Id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Set the parameter value for the @Id parameter
                        command.Parameters.AddWithValue("@Id", id);

                        // Execute the query and create a User object
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                User user = new User();
                                user.Id = (int)reader["Id"];
                                user.Name = (string)reader["Name"];
                                user.Age = (int)reader["Age"];
                                return user;
                            }
                            else
                            {
                                // If no user with the specified ID is found, return a 404 error
                                throw new HttpResponseException(HttpStatusCode.NotFound);
                            }
                        }
                    }
                }
            }

        
            [HttpPost]
            public void Delete(int id)
            {
                // Get the connection string from your web.config file
                string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

                // Create a new SqlConnection object
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the connection
                    connection.Open();

                    // Create a SqlCommand object with a delete query
                    string query = "DELETE FROM Users WHERE Id = @Id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Set the parameter value for the @Id parameter
                        command.Parameters.AddWithValue("@Id", id);

                        // Execute the query
                        int rowsAffected = command.ExecuteNonQuery();

                        // If no user with the specified ID is found, return a 404 error
                        if (rowsAffected == 0)
                        {
                            throw new HttpResponseException(HttpStatusCode.NotFound);
                        }
                    }
                }
            }
        
    }
}

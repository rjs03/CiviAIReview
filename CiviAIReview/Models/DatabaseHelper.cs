using System.Data;
using Microsoft.Data.SqlClient;
using OnShelfGTDL.Models;

namespace CiviAIReview.Models
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;
        public DatabaseHelper(DBContext dBContext)
        {
            _connectionString = dBContext.LoadConfig();
        }

        public async Task<bool> SaveUserAsync(string userId, string memberType, string firstName, string? middleName,
                                       string lastName, string? suffix, string address, string? emailAddress,
                                       string mobileNumber, string status)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand("sp_UserInformationSaveRecords", connection)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };

                    // Add Parameters
                    command.Parameters.AddWithValue("@UserID", userId);
                    command.Parameters.AddWithValue("@MemberType", memberType);
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@MiddleName", middleName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@Suffix", suffix ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Address", address);
                    command.Parameters.AddWithValue("@MobileNo", mobileNumber);
                    command.Parameters.AddWithValue("@EmailAddress", emailAddress ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Status", status == "Active" ? 1 : 0);
                    command.Parameters.AddWithValue("@CreatedBy", "admin");

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return false;
                }
            }
        }

        public async Task<List<UserViewModel>> GetUsersAsync()
        {
            var users = new List<UserViewModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("sp_UserInformationLoadRecords", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                users.Add(new UserViewModel
                                {
                                    UserID = reader["UserID"].ToString(),
                                    MemberType = reader["MemberType"].ToString(),
                                    FirstName = reader["FirstName"].ToString(),
                                    MiddleName = reader["MiddleName"] as string,
                                    LastName = reader["LastName"].ToString(),
                                    Suffix = reader["Suffix"] as string,
                                    Address = reader["Address"].ToString(),
                                    EmailAddress = reader["EmailAddress"] as string,
                                    MobileNumber = reader["MobileNo"].ToString(),
                                    Status = Convert.ToBoolean(reader["IsActive"])
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error loading users: " + ex.Message);
                }
            }

            return users;
        }


        public async Task<bool> UpdateUserAsync(string userId, string memberType, string firstName, string? middleName,
                                       string lastName, string? suffix, string address, string? emailAddress,
                                       string mobileNumber, string status)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand("sp_UserInformationUpdateRecords", connection)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };

                    // Add Parameters
                    command.Parameters.AddWithValue("@UserID", userId);
                    command.Parameters.AddWithValue("@MemberType", memberType);
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@MiddleName", middleName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@Suffix", suffix ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Address", address);
                    command.Parameters.AddWithValue("@MobileNo", mobileNumber);
                    command.Parameters.AddWithValue("@EmailAddress", emailAddress ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Status", status == "Active" ? 1 : 0);
                    command.Parameters.AddWithValue("@CreatedBy", "admin");

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return false;
                }
            }
        }

    }
}

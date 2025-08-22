using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UserManager.Domain.Entities;
using UserManager.Domain.Repositories;

namespace UserManager.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(IConfiguration configuration, ILogger<UserRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("UserManagerDb") ?? string.Empty;
            _logger = logger;
        }

        public async Task<bool> CanInsertUserAsync(string email)
        {
            _logger.LogInformation("Checking if user with email {Email} was inserted in the last minute", email);
            await using var connection = new SqlConnection(_connectionString);
            const string queryString = "SELECT COUNT(*) FROM dbo.UserRecord WHERE Email = @Email AND CreatedAt > @DateTimeLimit;";
            await using var command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@DateTimeLimit", DateTime.Now.AddMinutes(-1));

            await connection.OpenAsync();
            int count = (int)await command.ExecuteScalarAsync();

            if (count > 0)
            {
                _logger.LogWarning("New user record cannot be inserted. Found {Count} record with email {Email} inserted in the last minute.", count, email);
                return false;
            }
            _logger.LogInformation("User with email {Email} can be inserted", email);
            return true;
        }

        public async Task<bool> InsertUserRecordAsync(UserRecord userRecord)
        {
            _logger.LogInformation("Inserting new user into database with FirstName={@FirstName}, LastName={@LastName}, Email={@Email}", userRecord.FirstName, userRecord.LastName, userRecord.Email);
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var transaction = connection.BeginTransaction();
            try
            {
                int? addressId = null;
                int? companyId = null;
                if (userRecord.Address != null)
                {
                    // Insert into Address table
                    const string insertAddress = @"
                        INSERT INTO Address (Street, Suite, City, Zipcode, Lat, Lng)
                        OUTPUT INSERTED.Id
                        VALUES (@Street, @Suite, @City, @Zipcode, @Lat, @Lng);";
                    await using var commandAddress = new SqlCommand(insertAddress, connection, transaction);
                    commandAddress.Parameters.AddWithValue("@Street", userRecord.Address.Street);
                    commandAddress.Parameters.AddWithValue("@Suite", userRecord.Address.Suite);
                    commandAddress.Parameters.AddWithValue("@City", userRecord.Address.City);
                    commandAddress.Parameters.AddWithValue("@Zipcode", userRecord.Address.ZipCode);
                    commandAddress.Parameters.AddWithValue("@Lat", (object?)userRecord.Address.Lat ?? DBNull.Value);
                    commandAddress.Parameters.AddWithValue("@Lng", (object?)userRecord.Address.Lng ?? DBNull.Value);
                    addressId = (int)await commandAddress.ExecuteScalarAsync();
                }
                if (userRecord.Company != null)
                {
                    // Insert into Company table
                    const string insertCompany = @"
                        INSERT INTO Company (Name, CatchPhrase, Bs)
                        OUTPUT INSERTED.Id
                        VALUES (@Name, @CatchPhrase, @Bs);";
                    await using var commandCompany = new SqlCommand(insertCompany, connection, transaction);
                    commandCompany.Parameters.AddWithValue("@Name", userRecord.Company.Name);
                    commandCompany.Parameters.AddWithValue("@CatchPhrase", userRecord.Company.CatchPhrase);
                    commandCompany.Parameters.AddWithValue("@Bs", userRecord.Company.Bs);
                    companyId = (int)await commandCompany.ExecuteScalarAsync();
                }
                // Insert into UserRecord table
                const string insertUserRecord = @"
                    INSERT INTO UserRecord (FirstName, LastName, Email, Username, Phone, Website, AddressId, CompanyId) 
                    OUTPUT INSERTED.Id
                    VALUES (@FirstName, @LastName, @Email, @Username, @Phone, @Website, @AddressId, @CompanyId);";
                await using var commandUserRecord = new SqlCommand(insertUserRecord, connection, transaction);
                commandUserRecord.Parameters.AddWithValue("@FirstName", userRecord.FirstName);
                commandUserRecord.Parameters.AddWithValue("@LastName", userRecord.LastName);
                commandUserRecord.Parameters.AddWithValue("@Email", userRecord.Email);
                commandUserRecord.Parameters.AddWithValue("@Username", (object?)userRecord.Username ?? DBNull.Value);
                commandUserRecord.Parameters.AddWithValue("@Phone", (object?)userRecord.Phone ?? DBNull.Value);
                commandUserRecord.Parameters.AddWithValue("@Website", (object?)userRecord.Website ?? DBNull.Value);
                commandUserRecord.Parameters.AddWithValue("@AddressId", (object?)addressId ?? DBNull.Value);
                commandUserRecord.Parameters.AddWithValue("@CompanyId", (object?)companyId ?? DBNull.Value);
                var userId = (int)await commandUserRecord.ExecuteScalarAsync();

                await transaction.CommitAsync();
                _logger.LogInformation("Inserted new UserRecord with Id={@UserId}, AddressId={@AddressId}, CompanyId={@CompanyId}", userId, addressId, companyId);
                return userId > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError("Rolling back transaction");
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}

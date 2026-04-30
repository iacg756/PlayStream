using Microsoft.Extensions.Configuration;
using MySqlConnector;
using PlayStream.Core.Enum;
using PlayStream.Core.Interfaces;
using System;
using System.Data;

namespace PlayStream.Infrastructure.Data
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly IConfiguration _config;
        private readonly string _mySqlConn;
        public DataBaseProvider Provider { get; }

        public DbConnectionFactory(IConfiguration config)
        {
            _config = config;
            _mySqlConn = _config.GetConnectionString("DefaultConnection") ?? string.Empty;
            var providerStr = _config.GetSection("DatabaseProvider").Value ?? "MySql";

            Provider = providerStr.Equals("MySql", StringComparison.OrdinalIgnoreCase)
                ? DataBaseProvider.MySql
                : DataBaseProvider.SqlServer;
        }

        public IDbConnection CreateConnection()
        {
            return Provider switch
            {
                DataBaseProvider.MySql => new MySqlConnection(_mySqlConn),
                _ => new MySqlConnection(_mySqlConn)
            };
        }
    }
}
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PropertyManager.Application.Common.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Data;

namespace PropertyManager.Infrastructure.Persistence.Common;

public class DbConnectionProvider : IDbConnectionProvider
{
    private readonly IConfiguration _configuration;
    private string? _connectionString;
    private readonly ILogger<DbConnectionProvider> _logger;

    public DbConnectionProvider(IConfiguration configuration, ILogger<DbConnectionProvider> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }
    /// <summary>
    /// Creates and opens a database connection using the tenant-specific connection string.
    /// </summary>
    /// <returns>An open <see cref = "IDbConnection"/> object.</returns>s
    public IDbConnection GetConnection()
    {
        if (string.IsNullOrEmpty(_connectionString))
        {
        
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        return new SqlConnection(_connectionString);
    }
}

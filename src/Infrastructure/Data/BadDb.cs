using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Data;

public static class BadDb
{
    public static string ConnectionString { get; set; } = "Server=localhost;Database=master;User Id=sa;TrustServerCertificate=True";

    public static int ExecuteNonQueryUnsafe(string sql)
    {
        using var conn = new SqlConnection(ConnectionString);
        using var cmd = new SqlCommand(sql, conn);
        conn.Open();
        return cmd.ExecuteNonQuery();
    }

    public static IDataReader ExecuteReaderUnsafe(string sql)
    {
        var conn = new SqlConnection(ConnectionString);
        var cmd = new SqlCommand(sql, conn);
        conn.Open();
        return cmd.ExecuteReader();
    }
}

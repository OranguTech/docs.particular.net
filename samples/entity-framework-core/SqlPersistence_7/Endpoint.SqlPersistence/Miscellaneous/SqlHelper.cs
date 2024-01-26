﻿using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

public static class SqlHelper
{
    public static async Task ExecuteSql(string connectionString, string sql)
    {
        await EnsureDatabaseExists(connectionString);

        await using var connection = new SqlConnection(connectionString);
        connection.Open();

        await using var command = connection.CreateCommand();
        command.CommandText = sql;
        command.ExecuteNonQuery();
    }

    public static async Task CreateSchema(string connectionString, string schema)
    {
        var sql = $@"
if not exists (select  *
               from    sys.schemas
               where   name = N'{schema}')
    exec('create schema {schema}');";
        await ExecuteSql(connectionString, sql);
    }

    public static async Task EnsureDatabaseExists(string connectionString)
    {
        var builder = new SqlConnectionStringBuilder(connectionString);
        var database = builder.InitialCatalog;

        var masterConnection = connectionString.Replace(builder.InitialCatalog, "master");

        await using var connection = new SqlConnection(masterConnection);
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = $@"
if(db_id('{database}') is null)
    create database [{database}]
";
        await command.ExecuteNonQueryAsync();
    }
}
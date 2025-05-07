using System.ComponentModel.DataAnnotations.Schema;

namespace PropertyManager.Infrastructure.Persistence.Dapper;

internal static class TableNameHelper
{
    /// <summary>
    /// Returns the table name for the entity or the table name attribute if it exists.
    /// </summary>
    internal static string GetTableName<TEntity>()
    {
        if (Attribute.GetCustomAttribute(typeof(TEntity), typeof(TableAttribute)) is TableAttribute tableAttribute)
        {
            // Include the schema if it's set, otherwise, return only the table name
            return !string.IsNullOrEmpty(tableAttribute.Schema) ? $"{tableAttribute.Schema}.{tableAttribute.Name}" : tableAttribute.Name;
        }

        return typeof(TEntity).Name;
    }
}
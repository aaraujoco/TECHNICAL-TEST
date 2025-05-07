using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PropertyManager.Application.Exceptions;

/// <summary>
/// Custom Exception for not found entities.
/// </summary>
[Serializable]
public class EntityNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref = "EntityNotFoundException"/> class.
    /// </summary>
    /// <param name = "name"> The name of the entity that could not be found.</param>
    /// <param name = "key">The key of the entity that could not be found.</param>
    public EntityNotFoundException(string name, object key) : base($"Entity named '{name}' could not be found matching the ID '{key}'.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref = "EntityNotFoundException"/> class.
    /// </summary>
    /// <param name="key">The key of the entity that could not be found.</param>
    public EntityNotFoundException(object key) : base($"The resource could not be found matching the ID '{key}'.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref = "EntityNotFoundException"/> class.
    /// </summary>
    /// <param name = "message"></param>
    /// <param name = "innerException"></param>
    public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref = "EntityNotFoundException"/> class.
    /// </summary>
    /// <param name = "message"></param>
    public EntityNotFoundException(string message) : base(message)
    {
    }

}

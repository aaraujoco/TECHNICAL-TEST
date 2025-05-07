using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PropertyManager.Application.Exceptions;

/// <summary>
/// The DbContextException class is used to wrap exceptions that occur in the DbContext.
/// </summary>
[Serializable]
public class DbContextException : Exception
{
    /// <summary>
    ///  Initializes a new instance of the <see cref = "DbContextException"/> class.
    /// </summary>
    public DbContextException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref = "DbContextException"/> class.
    /// Message is used to provide a custom message.
    /// </summary>
    /// <param name = "message"></param>
    public DbContextException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref = "DbContextException"/> class.
    /// message is used to provide a custom message.
    /// inner is used to provide the inner exception.
    /// </summary>
    /// <param name = "message"></param>
    /// <param name = "inner"></param>
    public DbContextException(string message, Exception inner) : base(message, inner)
    {
    }

   
}

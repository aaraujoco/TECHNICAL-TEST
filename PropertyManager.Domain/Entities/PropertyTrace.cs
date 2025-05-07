using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyManager.Domain.Entities;

public class PropertyTrace
{
    /// <summary>
    /// Unique identifier for the property trace record.
    /// </summary>
    public int IdPropertyTrace { get; set; }  // Autogenerado

    /// <summary>
    /// Date and time of the sale.
    /// </summary>
    public DateTime DateSale { get; set; }

    /// <summary>
    /// Name associated with the property trace.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Value of the property.
    /// </summary>
    public decimal Value { get; set; }

    /// <summary>
    /// Tax applied to the property.
    /// </summary>
    public decimal Tax { get; set; }

    /// <summary>
    /// Identifier for the associated property.
    /// </summary>
    public int IdProperty { get; set; }

    /// <summary>
    /// Username or identifier of the user who created this record.
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// Date and time when this record was created.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Username or identifier of the user who last updated this record.
    /// </summary>
    public string? UpdatedBy { get; set; }

    /// <summary>
    /// Date and time when this record was last updated.
    /// </summary>
    public DateTime UpdatedDate { get; set; }
}


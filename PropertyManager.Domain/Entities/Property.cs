using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyManager.Domain.Entities;
/// <summary>
/// Representa una propiedad inmobiliaria.
/// </summary>
public class Property
{
    /// <summary>
    /// Obtiene o establece el identificador único de la propiedad. 
    /// Este campo es autogenerado por la base de datos.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdProperty { get; set; }

    /// <summary>
    /// Obtiene o establece el nombre de la propiedad.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Obtiene o establece la dirección de la propiedad.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Obtiene o establece el precio de la propiedad.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Obtiene o establece el código interno de la propiedad.
    /// </summary>
    public string? CodeInternal { get; set; }

    /// <summary>
    /// Obtiene o establece el año de construcción de la propiedad.
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// Obtiene o establece el identificador del propietario de la propiedad.
    /// Este campo es una clave foránea que referencia a la tabla Owners.
    /// </summary>
    public int IdOwner { get; set; }

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

using System.ComponentModel.DataAnnotations;

namespace _00_Framework.Application.Models;

public class IdModelArgument<TKey>
{
    [Required(AllowEmptyStrings = false)]
    public TKey Id { get; set; }
}
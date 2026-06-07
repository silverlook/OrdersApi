// DTOs/UpdateOrderDto.cs
using System.ComponentModel.DataAnnotations;

namespace OrdersApi.DTOs;

public class UpdateOrderDto
{
    [Required(ErrorMessage = "Pole statusName jest wymagane")]
    [MinLength(1, ErrorMessage = "statusName nie może być pusty")]
    public string StatusName { get; set; } = null!;
}
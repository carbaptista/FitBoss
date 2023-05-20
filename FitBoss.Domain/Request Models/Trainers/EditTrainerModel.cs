using FitBoss.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Request_Models.Trainers;
public class EditTrainerModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Id is required")]
    public Guid Id { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Id of who's updating is required")]
    public Guid UpdatedBy { get; set; }

    [Required]
    [DataType(DataType.Text)]
    [StringLength(50, ErrorMessage = "Name cannot be longer than {1}")]
    public string Name { get; set; } = "";

    [Required]
    [DataType(DataType.EmailAddress)]
    [StringLength(320, MinimumLength = 3)]
    public string Email { get; set; } = "";
}

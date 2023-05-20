using System.ComponentModel.DataAnnotations;

namespace FitBoss.Domain.Request_Models.Members;
public class CreateMemberModel
{
    [Required]
    [StringLength(50, MinimumLength = 1)]
    [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Only letters are allowed")]
    public string Name { get; set; } = default!;

    [Required]
    public Guid CreatorId { get; set; }

    [Required]
    [DataType(DataType.EmailAddress)]
    [StringLength(320, MinimumLength = 3)]
    public string Email { get; set; } = "";
}

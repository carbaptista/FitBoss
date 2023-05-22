using FitBoss.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace FitBoss.Domain.Request_Models.Members;
public class EditMemberModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Id is required")]
    public string Id { get; set; } = "";

    [Required(AllowEmptyStrings = false, ErrorMessage = "Id of who's updating is required")]
    public string UpdatedBy { get; set; } = "";

    [Required]
    [DataType(DataType.Text)]
    [StringLength(50, ErrorMessage = "Name cannot be longer than {1}")]
    public string Name { get; set; } = "";

    [Required]
    [DataType(DataType.EmailAddress)]
    [StringLength(320, MinimumLength = 3)]
    public string Email { get; set; } = "";

    [DataType(DataType.Password)]
    [StringLength(128, MinimumLength = 6, ErrorMessage = "The password must be at least {0} characters and at most {1} characters long")]
    public string? Password { get; set; }

    [Required]
    public SubscriptionType SubscriptionType { get; set; }

    public DateOnly? DateOfBirth { get; set; }
    public bool? Gender { get; set; }
    public int? Weight { get; set; }

    [Range(0, 300, ErrorMessage = "Height cannot be higher than {2}")]
    public int? Height { get; set; }
}

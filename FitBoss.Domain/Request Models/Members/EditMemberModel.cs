using FitBoss.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace FitBoss.Domain.Request_Models.Members;
public class EditMemberModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Id is required")]
    public Guid Id { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Id of creator is required")]
    public Guid CreatedBy { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Id of who's updating is required")]
    public Guid UpdatedBy { get; set; }

    [Required]
    [DataType(DataType.Text)]
    [StringLength(50, ErrorMessage = "Name cannot be longer than {1}")]
    public string Name { get; set; } = "";

    [Required]
    public SubscriptionType SubscriptionType { get; set; }

    public DateOnly? DateOfBirth { get; set; }
    public bool? Gender { get; set; }
    public int? Weight { get; set; }

    [Range(0, 300, ErrorMessage = "Height cannot be higher than {2}")]
    public int? Height { get; set; }
}

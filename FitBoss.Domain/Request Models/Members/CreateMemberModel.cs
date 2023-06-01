using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FitBoss.Domain.Request_Models.Members;
public class CreateMemberModel
{
    [Required]
    [StringLength(50, MinimumLength = 1)]
    [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Only letters are allowed")]
    [DefaultValue("new member")]
    public string Name { get; set; } = default!;

    [Required]
    [StringLength(50, MinimumLength = 6)]
    [RegularExpression("^[a-zA-Z.]*$", ErrorMessage = "Only letters and periods with no spaces are allowed")]
    [DefaultValue("new.member")]
    public string UserName { get; set; } = default!;

    [Required]
    [DefaultValue("8fe59047-f6fc-4066-a3f7-2c73cb9c063c")]
    public string CreatorId { get; set; } = "";

    [Required]
    [DataType(DataType.EmailAddress)]
    [StringLength(320, MinimumLength = 3)]
    [DefaultValue("new.member@test.com")]
    public string Email { get; set; } = "";

    [Required(AllowEmptyStrings = false, ErrorMessage = "A password is required")]
    [DataType(DataType.Password)]
    [StringLength(128, MinimumLength = 6, ErrorMessage = "The password must be at least {0} characters and at most {1} characters long")]
    [DefaultValue("password")]
    public string Password { get; set; } = "";
}

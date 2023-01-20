using System.ComponentModel.DataAnnotations;

public class UpdatePasswordDTO
{
    [Required]
    public string OldPassword { get; set; }

    [Required]
    public string NewPassword { get; set; }
}

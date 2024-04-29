using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ConferenceDomain.Model;

public partial class SignUp
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Користувач")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Конференція")]
    public int ConferenceId { get; set; }
    [Display(Name = "Конференція")]
    public virtual Conference? Conference { get; set; }
    [Display(Name = "Користувач")]

    public virtual User? User { get; set; } 
}

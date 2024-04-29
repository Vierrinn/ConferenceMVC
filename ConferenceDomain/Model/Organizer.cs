using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ConferenceDomain.Model;

public partial class Organizer : Entity
{
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Ім'я")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Логін")]
    public string? Login { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Пароль")]
    public string? Password { get; set; }

    public virtual ICollection<Conference> Conferences { get; set; } = new List<Conference>();
}

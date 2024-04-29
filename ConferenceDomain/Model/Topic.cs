using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ConferenceDomain.Model;

public partial class Topic: Entity
{
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Тема")]
    public string? Name { get; set; } 

    public virtual ICollection<Conference> Conferences { get; set; } = new List<Conference>();
}

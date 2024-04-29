using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ConferenceDomain.Model;

public partial class Conference : Entity
{
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Назва")]
    public string? Name { get; set; }

    [Display(Name = "Тема")]
    public int TopicId { get; set; }

    [Display(Name = "Інформація")]
    public string? Info { get; set; }

    [Display(Name = "Ціна(₴)")]
    public decimal? Cost { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Дата та час")]
    public DateTime DateTime { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Тривалість")]
    public TimeOnly Duration { get; set; }
    public int OrganizerId { get; set; }

    public virtual ICollection<SignUp> SignUps { get; set; } = new List<SignUp>();

    [Display(Name = "Організатор")]
    public virtual Organizer? Organizer { get; set; } 

    [Display(Name = "Тема")]
    public virtual Topic? Topic { get; set; } 
}

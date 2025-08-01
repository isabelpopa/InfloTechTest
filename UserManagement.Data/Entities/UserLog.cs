using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagement.Models;

public class UserLog
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public long UserId { get; set; } = default!;
    public string Forename { get; set; } = default!;
    public string Surname { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Action { get; set; } = default!;
    public DateTime DateTime { get; set; } = default!;

}

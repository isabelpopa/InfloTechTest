using System;

namespace UserManagement.Web.Models.UserLogs;

public class UserLogListViewModel
{
    public List<UserLogListItemViewModel> Items { get; set; } = new();
}

public class UserLogListItemViewModel
{
    public long Id { get; set; }
    public long UserId { get; set; } = default!;
    public string Forename { get; set; } = default!;
    public string Surname { get; set; } = default!;
    public string Action { get; set; } = default!;
    public DateTime DateTime { get; set; } = default!;
}

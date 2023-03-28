using System;
using System.Collections.Generic;

namespace jwt.API.Entities;

public partial class User
{
    public string Mailid { get; set; } = null!;

    public string? Passwords { get; set; }
}

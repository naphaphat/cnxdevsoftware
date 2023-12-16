using System;
using System.Collections.Generic;

namespace cnxdevsoftware.Models;

public partial class AccessToken
{
    public int Id { get; set; }

    public string? AccessToken1 { get; set; }

    public string? TokenType { get; set; }

    public int ExpiresIn { get; set; }

    public DateTime LastUpdate { get; set; }
    
}

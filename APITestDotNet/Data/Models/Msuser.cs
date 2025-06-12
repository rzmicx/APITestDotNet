using System;
using System.Collections.Generic;

namespace APITestDotNet.Data.Models;

public partial class Msuser
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public byte[]? Passcode { get; set; }

    public bool? Active { get; set; }
}

using System;

namespace Domain.Entities;

public class User
{
    public Guid Id { get; set; }

    public string Name { get; set; }
    
    public DateTime Born { get; set; }
}
﻿namespace Entities.Concrete;

public class Announcement
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public DateTime CreatedTime { get; set; } = DateTime.Now;
}
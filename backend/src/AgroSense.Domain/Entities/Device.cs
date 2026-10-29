namespace AgroSense.Domain.Entities;

public class Device
{
    public int Id { get; set; }
    public string DeviceId { get; set; } = "";
    public string Name { get; set; } = "";
    public string Location { get; set; } = "";
    public bool IsActive { get; set; } = true;
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    public DateTime LastSeen { get; set; } = DateTime.UtcNow;
}


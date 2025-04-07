public class Config
{
    public string Name { get; set; } = string.Empty;
    public int Version { get; set; }
    public string[] Features { get; set; } = Array.Empty<string>();
}
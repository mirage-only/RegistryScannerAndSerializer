namespace RegistryConverter.Entity;

[Serializable]
public class MyValue
{
    public string Name { get; set; }

    public object? Value { get; set; }
    
    public string Type { get; set; }
    
    public MyValue() { }

    public MyValue(string name, object value, string type)
    {
        Name = name;
        Value = value;
        Type = type;
    }
}
namespace RegistryConverter.Entity;

[Serializable]
public class MyValue
{
    public string Name { get; set; }

    public object? Value { get; set; }
    
    public MyValue() { }

    public MyValue(string name, object value)
    {
        Name = name;
        Value = value;
    }
}
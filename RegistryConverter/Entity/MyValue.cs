namespace RegistryConverter.Entity;

public class MyValue(string name, object? value)
{
    string Name { get; set; } = name;

    object? Value { get; set; } = value;
}
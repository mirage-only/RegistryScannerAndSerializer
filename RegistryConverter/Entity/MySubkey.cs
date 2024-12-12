using System.Collections.ObjectModel;
using Microsoft.Win32;

namespace RegistryConverter.Entity;

public class MySubkey(string name, RegistryKey? ownerRegistryKey, string path)
{
    public string Name { get; set; } = name;
    public RegistryKey? OwnerRegistryKey{get; set;} = ownerRegistryKey;
    public string Path {get; set;} = path;
    public ObservableCollection<MySubkey> SubSubKeys { get; set; } = [];
    public ObservableCollection<MyValue> Values { get; set; } = [];
    public bool IsFilled { get; set; }
}


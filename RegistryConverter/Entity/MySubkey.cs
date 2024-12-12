using System.Collections.ObjectModel;
using Microsoft.Win32;

namespace RegistryConverter.Entity;

public class MySubkey
{
    public string Name { get; set; }
    public RegistryKey OwnerRegistryKey{get; set;}
    public string Path {get; set;}
    public ObservableCollection<MySubkey> SubSubKeys { get; set; }
    public ObservableCollection<MyValue> Values { get; set; }
    public bool IsFilled { get; set; } = false;

    public MySubkey(string name, RegistryKey? ownerRegistryKey, string path)
    {
        Name = name;
        
        OwnerRegistryKey = ownerRegistryKey;
        
        Path = path;
        
        SubSubKeys = [];

        Values = [];
    }
    
    
}


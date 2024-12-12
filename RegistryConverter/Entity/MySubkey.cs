using System.Collections.ObjectModel;
using System.Xml.Serialization;
using Microsoft.Win32;

namespace RegistryConverter.Entity;

[Serializable]
public class MySubkey
{
    public string Name { get; set; }
    
    [XmlIgnore]
    public RegistryKey? OwnerRegistryKey{get; set;}
    public string Path {get; set;}
    
    [XmlArray("SubSubKeys")]
    [XmlArrayItem("SubSubKey")]
    public ObservableCollection<MySubkey> SubSubKeys { get; set; }
    
    [XmlArray("Values")]
    [XmlArrayItem("Value")]
    public ObservableCollection<MyValue> Values { get; set; }
    public bool IsFilled { get; set; }

    public MySubkey()
    {
        SubSubKeys = new ObservableCollection<MySubkey>();
        Values = new ObservableCollection<MyValue>();
    }

    public MySubkey(string name, RegistryKey ownerRegistryKey, string path)
    {
        Name = name;
        OwnerRegistryKey = ownerRegistryKey;
        Path = path;
        SubSubKeys = new ObservableCollection<MySubkey>();
        Values = new ObservableCollection<MyValue>();
    }
}


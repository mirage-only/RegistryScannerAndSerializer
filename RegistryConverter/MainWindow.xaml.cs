using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;
using Microsoft.Win32;
using RegistryConverter.Entity;

namespace RegistryConverter
{
    public partial class MainWindow
    {
        public ObservableCollection<MySubkey> SubKeys { get; set; }
        public ObservableCollection<MyValue> Values { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Values = [];
            
            SubKeys =
            [
                new MySubkey("HKEY_CLASSES_ROOT", Registry.ClassesRoot, string.Empty),
                new MySubkey("HKEY_CURRENT_USER", Registry.CurrentUser, string.Empty),
                new MySubkey("HKEY_LOCAL_MACHINE", Registry.LocalMachine, string.Empty),
                new MySubkey("HKEY_USERS", Registry.Users, string.Empty),
                new MySubkey("HKEY_CURRENT_CONFIG", Registry.CurrentConfig, string.Empty),
            ];
            
            
            DataContext = this;
        }

        private void FillTheNode(Object sender, RoutedEventArgs e)
        {
            var treeViewItem = (TreeViewItem)e.OriginalSource;
            
            var mySubkey = (MySubkey)treeViewItem.DataContext;

            if (mySubkey == null || mySubkey.IsFilled)
            {
               ValuesUpdate(mySubkey);
                
                return;
            }
            
            LoadTheNode(mySubkey);
        }

        private void LoadTheNode(MySubkey mySubkey)
        {
            mySubkey.SubSubKeys.Clear();
            mySubkey.Values.Clear();
            
            if(mySubkey.Name.Equals("HKEY_CLASSES_ROOT") ||
               mySubkey.Name.Equals("HKEY_CURRENT_USER") ||
               mySubkey.Name.Equals("HKEY_LOCAL_MACHINE") ||
               mySubkey.Name.Equals("HKEY_LOCAL_MACHINE") ||
               mySubkey.Name.Equals("HKEY_CURRENT_CONFIG")) ScanBaseKeys(mySubkey);
            else
            {
                var key = mySubkey.OwnerRegistryKey.OpenSubKey(mySubkey.Path);

                foreach (var subKeyName in key!.GetSubKeyNames())
                {
                    mySubkey.SubSubKeys.Add(new MySubkey(subKeyName, mySubkey.OwnerRegistryKey, $"{mySubkey.Path}\\{subKeyName}"));
                }
                
                foreach (var valueName in key.GetValueNames())
                {
                    mySubkey.Values.Add(new MyValue(valueName, key.GetValue(valueName)));
                }
            
                ValuesUpdate(mySubkey);
                
                mySubkey.IsFilled = true;
                
                mySubkey.OwnerRegistryKey.Close();
            }
        }

        private void ScanBaseKeys(MySubkey mySubkey)
        {
            foreach (var subSubKeyName in  mySubkey.OwnerRegistryKey.GetSubKeyNames())
            {
                mySubkey.SubSubKeys.Add(new MySubkey(subSubKeyName, mySubkey.OwnerRegistryKey, $"{subSubKeyName}"));
            }
                
            foreach (var valueName in mySubkey.OwnerRegistryKey.GetValueNames())
            {
                mySubkey.Values.Add(new MyValue(valueName, mySubkey.OwnerRegistryKey.GetValue(valueName)));
            }
            
            ValuesUpdate(mySubkey);
            
            mySubkey.IsFilled = true;
            
            mySubkey.OwnerRegistryKey.Close();
        }

        private void ValuesUpdate(MySubkey mySubkey)
        {
            Values.Clear();
            foreach (var value in mySubkey.Values)
            {
                Values.Add(value);
            }
        }


        private void SerializeButton_OnClick(object sender, RoutedEventArgs e)
        {
            XmlSerializer xml = new XmlSerializer(typeof(ObservableCollection<MySubkey>));

            using (FileStream fs = new FileStream(@"D:\University\5 semester\Coursework System Porgramming\RegistryConverter\RegistryConverter\Registry.xml", FileMode.Create))
            {
                xml.Serialize(fs, SubKeys);
            }

            ViewProgress();
        }

        private async void ViewProgress()
        {
            const int steps = 100;

            for (var i = 0; i <= steps; i++)
            {
                SerealizeProgressBar.Value= i;
                
                await Task.Delay(20);
            }
            
            SerealizeProgressBar.Value = 0;
        }
    }
}

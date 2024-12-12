using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using System.Xml;
using RegistryConverter.Entity;

namespace RegistryViewer
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<MySubkey> SubKeys { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            
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
            if (treeViewItem == null) return;
            
            var mySubkey = (MySubkey)treeViewItem.DataContext;
            if (mySubkey == null || mySubkey.IsFilled) return;
            
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
                RegistryKey key = mySubkey.OwnerRegistryKey.OpenSubKey(mySubkey.Path);

                foreach (var subKeyName in key.GetSubKeyNames())
                {
                    mySubkey.SubSubKeys.Add(new MySubkey(subKeyName, mySubkey.OwnerRegistryKey, $"{mySubkey.Path}\\{subKeyName}"));
                }
                
                foreach (var valueName in key.GetValueNames())
                {
                    mySubkey.Values.Add(new MyValue(valueName, key.GetValue(valueName)));
                }
            
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
            
            mySubkey.IsFilled = true;
            
            mySubkey.OwnerRegistryKey.Close();
        }
    }
}

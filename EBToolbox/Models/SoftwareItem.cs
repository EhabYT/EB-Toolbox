using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBToolbox.Models
{
    public class SoftwareItem
    {
        public string Name;
        public string Key;

        public SoftwareItem(string name, string key) 
        {
            Name = name;
            Key = key;
        }
    }
}

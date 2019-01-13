using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Block_Indicate.Class
{
    public class BuildName
    {
        public string BuildNameSpaces(string name)
        {
            StringBuilder newName = new StringBuilder(name);
            newName.Replace(" ", "&nbsp");
            string finalName = newName.ToString();
            return finalName;
        }
    }
}

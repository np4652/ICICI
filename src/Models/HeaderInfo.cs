using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICICI.Models
{
    public class HeaderInfo
    {
        public string AppID { get; set; }
        public string Version { get; set; }
        public int UserID { get; set; }
        public int Token { get; set; }
        public string Domain { get; set; }
    }
}

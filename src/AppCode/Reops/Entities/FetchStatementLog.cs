using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICICI.AppCode.Reops.Entities
{
    public class FetchStatementLog
    {
        public int Id { get; set; }
        public string AccountNo { get; set; }
        public string tranID { get; set; }
        public string EntryOn { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICICI.AppCode.Reops.Entities
{
    public class PostStatetmentRequest
    {
        public string AccountNo { get; set; }
        public List<TransactionDetail> data { get; set; }
    }
}

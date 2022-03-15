using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICICI.AppCode.Reops.Entities
{
    public class Datum
    {
        public string Sl_No_ { get; set; }
        public string Tran_ID { get; set; }
        public string Value_Date { get; set; }
        public string Transaction_Date { get; set; }
        public string Transaction_Posted_Date { get; set; }
        public string Cheque_No_Ref_No_ { get; set; }
        public string Transaction_Remarks { get; set; }
        public string Withdrawal_Amt_INR_ { get; set; }
        public string Deposit_Amt_INR_ { get; set; }
        public string Balance_INR_ { get; set; }
    }

    public class FetchStatement
    {
        public string Resp_code { get; set; }
        public string Resp_desc { get; set; }
        public List<Datum> data { get; set; }
    }
}

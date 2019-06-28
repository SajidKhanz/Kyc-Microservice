using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KYCVerifcation.API.Models
{
    public class KYCVerifcationServiceResult
    {
        public string TransactionID { get; set; }
        public DateTime UploadedDt { get; set; }
        public string CountryCode { get; set; }
        public string ProductName { get; set; }
        public Record Record { get; set; }
        public List<object> Errors { get; set; }
    }

    public class DatasourceResult
    {
        public string DatasourceName { get; set; }
        public List<object> DatasourceFields { get; set; }
        public List<object> AppendedFields { get; set; }
        public List<object> Errors { get; set; }
        public List<object> FieldGroups { get; set; }
    }

    public class Rule
    {
        public string RuleName { get; set; }
        public string Note { get; set; }
    }

    public class Record
    {
        public string TransactionRecordID { get; set; }
        public string RecordStatus { get; set; }
        public List<DatasourceResult> DatasourceResults { get; set; }
        public List<object> Errors { get; set; }
        public Rule Rule { get; set; }
    }

}

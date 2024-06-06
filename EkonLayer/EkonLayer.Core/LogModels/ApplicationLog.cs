using EkonLayer.Core.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkonLayer.Core.LogModels
{
    [Serializable]
    public class ApplicationLog : BaseEntity
    {
        public int ApplicationId { get; set; }
        public string FieldName { get; set; } = "";
        public string FieldValue { get; set; } = "";
        public string Type { get; set; } = "";
        public Application Application { get; set; } = new();
    }
}

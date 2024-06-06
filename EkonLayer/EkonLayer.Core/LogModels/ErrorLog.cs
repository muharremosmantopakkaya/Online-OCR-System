using EkonLayer.Core.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkonLayer.Core.LogModels
{
    [Serializable]
    public class ErrorLog : BaseEntity
    {
        public string Application { get; set; } = "";
        public string Message { get; set; } = "";
        public string Trace { get; set; } = "";
        public string StackTrace { get; set; } = "";
    }
}

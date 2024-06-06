using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkonLayer.Helpers.Models.CustomModels
{
    [Serializable]
    public class ApplicationStartResult
    {
        public bool Success { get; set; }
        public Exception Exception { get; set; } = new Exception();
    }
}

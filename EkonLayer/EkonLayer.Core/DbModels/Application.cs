using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkonLayer.Core.DbModels
{
    [Serializable]
    public class Application : BaseEntity
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Ip { get; set; } = "";
        public string Port { get; set; } = "";
    }
}

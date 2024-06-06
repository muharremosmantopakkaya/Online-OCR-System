using EkonLayer.Core.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkonLayer.Core.LogModels
{
    [Serializable]
    public class UserLog : BaseEntity
    {
        public int UserId { get; set; }
        public string Type { get; set; } = "";
        public string Message { get; set; } = "";
        public string Ip { get; set; } = "";
        public string Port { get; set; } = "";
        public User User { get; set; } = new();
    }
}

using EkonLayer.Helpers.Models.Dtos.DbModelDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkonLayer.Helpers.Models.Dtos.LogModelDtos
{
    [Serializable]
    public class UserLogDto : BaseEntityDto
    {
        public int UserId { get; set; }
        public string Type { get; set; } = "";
        public string Message { get; set; } = "";
        public string Ip { get; set; } = "";
        public string Port { get; set; } = "";
        public UserDto User { get; set; } = new();
    }
}

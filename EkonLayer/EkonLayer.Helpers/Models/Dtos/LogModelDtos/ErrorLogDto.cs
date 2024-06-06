using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkonLayer.Helpers.Models.Dtos.LogModelDtos
{
    [Serializable]
    public class ErrorLogDto : BaseEntityDto
    {
        public string Application { get; set; } = "";
        public string Message { get; set; } = "";
        public string StackTrace { get; set; } = "";
    }
}

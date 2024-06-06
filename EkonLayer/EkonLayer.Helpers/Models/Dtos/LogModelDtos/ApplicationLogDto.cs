using EkonLayer.Helpers.Models.Dtos.DbModelDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkonLayer.Helpers.Models.Dtos.LogModelDtos
{
    [Serializable]
    public class ApplicationLogDto : BaseEntityDto
    {
        public int ApplicationId { get; set; }
        public string FieldName { get; set; } = "";
        public string FieldValue { get; set; } = "";
        public string Type { get; set; } = "";
        public ApplicationDto Application { get; set; } = new ApplicationDto();
    }
}

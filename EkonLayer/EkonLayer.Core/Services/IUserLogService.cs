using EkonLayer.Helpers.Models.Dtos.LogModelDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkonLayer.Core.Services
{
    public interface IUserLogService
    {
        Task AddUserLog(UserLogDto item);

        Task UserLogWithBulk(IEnumerable<UserLogDto> list);
    }
}

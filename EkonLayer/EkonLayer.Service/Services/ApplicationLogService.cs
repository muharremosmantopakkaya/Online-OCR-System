using EkonLayer.Core.LogModels;
using EkonLayer.Core.Repositories;
using EkonLayer.Core.Services;
using EkonLayer.Core.UnitOfWork;
using EkonLayer.Helpers.Models.Dtos.LogModelDtos;
using EkonLayer.Service.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkonLayer.Service.Services
{
    public class ApplicationLogService : IApplicationLogService
    {
        public IGenericRepository<ApplicationLog> _applicationLogRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationLogService(IGenericRepository<ApplicationLog> applicationLogRepository, IUnitOfWork unitOfWork)
        {
            _applicationLogRepository = applicationLogRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddApplicationLog(ApplicationLogDto item)
        {
            await _applicationLogRepository.AddAsync(ObjectMapper.Mapper.Map<ApplicationLog>(item));
            await _unitOfWork.CommitAsync();
        }

        public async Task ApplicationLogWithBulk(IEnumerable<ApplicationLogDto> list)
        {
            await _applicationLogRepository.WithBulk(ObjectMapper.Mapper.Map<IEnumerable<ApplicationLog>>(list));
            await _unitOfWork.CommitAsync();
        }
    }
}

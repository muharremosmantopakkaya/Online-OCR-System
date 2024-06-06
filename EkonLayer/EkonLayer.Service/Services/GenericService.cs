using AutoMapper.Internal.Mappers;
using EkonLayer.Core.Repositories;
using EkonLayer.Core.Services;
using EkonLayer.Core.UnitOfWork;
using EkonLayer.Helpers.Models.CustomModels;
using EkonLayer.Service.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EkonLayer.Service.Services
{
    public class GenericService<TEntity, TDto> : IGenericService<TEntity, TDto> where TEntity : class where TDto : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<TEntity> _genericRepository;

        public GenericService(IUnitOfWork unitOfWork, IGenericRepository<TEntity> genericRepository)
        {
            _genericRepository = genericRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<TDto>> AddAsync(TDto entity)
        {
            var newEntity = ObjectMapper.Mapper.Map<TEntity>(entity);
            await _genericRepository.AddAsync(newEntity);
            await _unitOfWork.CommitAsync();
            var newDto = ObjectMapper.Mapper.Map<TDto>(newEntity);
            return BaseResponse<TDto>.Success(200, newDto);
        }

        public async Task<BaseResponse<IEnumerable<TDto>>> AddRangeAsync(IEnumerable<TDto> entities)
        {
            var newEntity = ObjectMapper.Mapper.Map<IEnumerable<TEntity>>(entities);
            await _genericRepository.AddRangeAsync(newEntity);
            await _unitOfWork.CommitAsync();
            var newDto = ObjectMapper.Mapper.Map<IEnumerable<TDto>>(newEntity);
            return BaseResponse<IEnumerable<TDto>>.Success(200, newDto);
        }

        public BaseResponse<Task<bool>> AnyAsync(Expression<Func<TEntity, bool>> expression)
        {
            return BaseResponse<Task<bool>>.Success(200, _genericRepository.AnyAsync(expression));
        }

        public async Task<BaseResponse<TDto>> FindFirst(Expression<Func<TEntity, bool>> expression)
        {
            var result = await _genericRepository.FindFirst(expression);
            return BaseResponse<TDto>.Success(200, ObjectMapper.Mapper.Map<TDto>(result));
        }

        public async Task<BaseResponse<IEnumerable<TDto>>> GetAllAsync()
        {
            return BaseResponse<IEnumerable<TDto>>.Success(200, ObjectMapper.Mapper.Map<IEnumerable<TDto>>(await _genericRepository.GetAllAsync()));
        }

        public async Task<BaseResponse<TDto>> GetByIdAsync(int id)
        {
            return BaseResponse<TDto>.Success(200, ObjectMapper.Mapper.Map<TDto>(await _genericRepository.GetByIdAsync(id)));
        }

        public async Task<BaseResponse<NoContent>> RemoveAsync(TDto entity)
        {
            _genericRepository.Remove(ObjectMapper.Mapper.Map<TEntity>(entity));
            await _unitOfWork.CommitAsync();
            return BaseResponse<NoContent>.Success(200, new NoContent());
        }

        public async Task<BaseResponse<NoContent>> RemoveRangeAsync(IEnumerable<TDto> entities)
        {
            _genericRepository.RemoveRange(ObjectMapper.Mapper.Map<IEnumerable<TEntity>>(entities));
            await _unitOfWork.CommitAsync();
            return BaseResponse<NoContent>.Success(200, new NoContent());
        }

        public async Task<BaseResponse<NoContent>> TruncateTable()
        {
            IEnumerable<TEntity> list = await _genericRepository.GetAllAsync();
            _genericRepository.RemoveRange(list);
            await _unitOfWork.CommitAsync();
            return BaseResponse<NoContent>.Success(200, new NoContent());
        }

        public async Task<BaseResponse<NoContent>> UpdateAsync(TDto entity)
        {
            _genericRepository.Update(ObjectMapper.Mapper.Map<TEntity>(entity));
            await _unitOfWork.CommitAsync();
            return BaseResponse<NoContent>.Success(200, new NoContent());
        }

        public async Task<BaseResponse<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> expression)
        {
            var list = _genericRepository.Where(expression);
            return BaseResponse<IEnumerable<TDto>>.Success(200, ObjectMapper.Mapper.Map<IEnumerable<TDto>>(await list.ToListAsync()));
        }

        public async Task<BaseResponse<IEnumerable<TDto>>> WithBulk(IEnumerable<TDto> entities)
        {
            var list = ObjectMapper.Mapper.Map<IEnumerable<TEntity>>(entities);
            await _genericRepository.WithBulk(list);
            await _unitOfWork.CommitAsync();
            return BaseResponse<IEnumerable<TDto>>.Success(200, ObjectMapper.Mapper.Map<IEnumerable<TDto>>(list));
        }
    }
}

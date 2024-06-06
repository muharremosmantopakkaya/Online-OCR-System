using EkonLayer.Helpers.Models.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EkonLayer.Core.Services
{
    public interface IGenericService<TEntity, TDto> where TEntity : class where TDto : class
    {
        Task<BaseResponse<TDto>> GetByIdAsync(int id);
        Task<BaseResponse<IEnumerable<TDto>>> GetAllAsync();
        Task<BaseResponse<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> expression);
        BaseResponse<Task<bool>> AnyAsync(Expression<Func<TEntity, bool>> expression);
        Task<BaseResponse<TDto>> AddAsync(TDto entity);
        Task<BaseResponse<IEnumerable<TDto>>> AddRangeAsync(IEnumerable<TDto> entities);
        Task<BaseResponse<NoContent>> UpdateAsync(TDto entity);
        Task<BaseResponse<NoContent>> RemoveAsync(TDto entity);
        Task<BaseResponse<NoContent>> RemoveRangeAsync(IEnumerable<TDto> entities);

        Task<BaseResponse<TDto>> FindFirst(Expression<Func<TEntity, bool>> expression);
        Task<BaseResponse<NoContent>> TruncateTable();
        Task<BaseResponse<IEnumerable<TDto>>> WithBulk(IEnumerable<TDto> entities);
    }
}

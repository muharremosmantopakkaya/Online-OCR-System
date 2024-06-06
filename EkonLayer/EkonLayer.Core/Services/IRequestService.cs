using EkonLayer.Helpers.Models.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkonLayer.Core.Services
{
    public interface IRequestService<TDto> where TDto : class
    {
        BaseResponse<TDto> Get(string url, string token = "");
        BaseResponse<TDto> Post<T>(string url, T objx, string token = "");
        BaseResponse<TDto> Put<T>(string url, T objx, string token = "");
        BaseResponse<TDto> Patch<T>(string url, T objx, string token = "");
        BaseResponse<TDto> Delete(string url, string token = "");
    }
}

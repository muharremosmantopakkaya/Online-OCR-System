using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EkonLayer.Helpers.Models.CustomModels
{
    public class BaseResponse<TDto>
    {
        public TDto Data { get; set; }

        [JsonIgnore]
        public int StatusCode { get; set; }

        public bool IsSuccess { get; set; }


        public List<string> Errors { get; set; } = new List<string>();
        public string Message { get; set; } = "";


        public static BaseResponse<TDto> Success(int statusCode, TDto data)
        {
            return new BaseResponse<TDto> { Data = data, StatusCode = statusCode, IsSuccess = true };
        }
        public static BaseResponse<TDto> Success(int statusCode)
        {
            return new BaseResponse<TDto> { StatusCode = statusCode, IsSuccess = true };
        }

        public static BaseResponse<TDto> Fail(int statusCode, List<string> errors)
        {
            return new BaseResponse<TDto> { StatusCode = statusCode, Errors = errors, IsSuccess = false };
        }

        public static BaseResponse<TDto> Fail(int statusCode, string error)
        {
            return new BaseResponse<TDto> { StatusCode = statusCode, Errors = new List<string> { error }, IsSuccess = false };
        }
    }
}

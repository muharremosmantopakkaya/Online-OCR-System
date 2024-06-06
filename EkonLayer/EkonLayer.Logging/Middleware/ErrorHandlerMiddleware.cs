using EkonLayer.Core.LogModels;
using EkonLayer.Core.Services;
using EkonLayer.Helpers.CommonMethods;
using EkonLayer.Helpers.Models.CustomModels;
using EkonLayer.Helpers.Models.Dtos.DbModelDtos;
using EkonLayer.Helpers.Models.Dtos.LogModelDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace EkonLayer.Logging.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IOptions<ApplicationDto> _options;

        public ErrorHandlerMiddleware(RequestDelegate next, IOptions<ApplicationDto> options)
        {
            _next = next;
            _options = options;
        }

        public async Task Invoke(HttpContext context, IErrorLogService _log)
        {
            try
            {
                await _next(context);
                var x = context.Request;
            }
            catch (Exception ex)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (ex)
                {
                    case ArgumentNullException e:
                        response.StatusCode = (int)HttpStatusCode.NoContent;
                        break;
                    case CustomException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                await _log.AddErrorLog(new ErrorLogDto() { Application = _options.Value.Name, Message = ex.Message, StackTrace = ex.StackTrace });

                var result = "";
                if (context.Request.Headers["Accept-Language"].Contains("tr-TR") || context.Request.Headers["AcceptLanguage"].Contains("tr-TR"))
                    result = Serializers.SerializeJson(new BaseResponse<object>() { Message = Helpers.Resources.ErrorLanguage.ResourceManager.GetString("GeneralError", new System.Globalization.CultureInfo("tr-TR")), IsSuccess = false });
                else
                    result = Serializers.SerializeJson(new BaseResponse<object>() { Message = Helpers.Resources.ErrorLanguage.ResourceManager.GetString("GeneralError", new System.Globalization.CultureInfo("en-GB")), IsSuccess = false });

                await response.WriteAsync(result);
            }
        }
    }
}

using EkonLayer.Core.Services;
using EkonLayer.Helpers.CommonMethods;
using EkonLayer.Helpers.Models.CustomModels;
using EkonLayer.Helpers.Models.Dtos.DbModelDtos;
using EkonLayer.Helpers.Resources;
using EkonLayer.Logging.Methods;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkonLayer.Service.Services
{
    public class RequestService<TDto> : IRequestService<TDto> where TDto : class
    {
        private readonly IOptions<ApplicationDto> _options;
        private readonly LogWorker _logworker;

        public RequestService(IOptions<ApplicationDto> options, LogWorker logWorker)
        {
            _options = options;
            _logworker = logWorker;
        }

        private HttpClient getClient(string url)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("AcceptLanguage", Thread.CurrentThread.CurrentCulture.ToString());
            return client;
        }

        public BaseResponse<TDto> Delete(string url, string token = "")
        {
            BaseResponse<TDto> obj = (BaseResponse<TDto>)Activator.CreateInstance(typeof(BaseResponse<TDto>));
            HttpClient client = getClient(url);
            try
            {
                if (!String.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Add("token", token);
                }
                HttpResponseMessage response = client.DeleteAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    var jsonstring = response.Content.ReadAsStringAsync();
                    jsonstring.Wait();
                    obj = Serializers.DeserializeJson<BaseResponse<TDto>>(jsonstring.Result);
                    client.Dispose();
                }
                else
                {
                    obj.IsSuccess = false;
                    obj.Message = ErrorLanguage.ResourceManager.GetString("GeneralError", Thread.CurrentThread.CurrentCulture);
                }
            }
            catch (Exception ex)
            {
                _logworker.AddErrorLogEnqueue(new Helpers.Models.Dtos.LogModelDtos.ErrorLogDto() { Application = _options.Value.Name, Message = ex.Message, StackTrace = ex.StackTrace });
                obj.IsSuccess = false;
                obj.Message = ErrorLanguage.ResourceManager.GetString("GeneralError", Thread.CurrentThread.CurrentCulture);
            }
            return obj;
        }

        public BaseResponse<TDto> Get(string url, string token = "")
        {
            BaseResponse<TDto> obj = (BaseResponse<TDto>)Activator.CreateInstance(typeof(BaseResponse<TDto>));
            HttpClient client = getClient(url);
            try
            {
                if (!String.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Add("token", token);
                }
                HttpResponseMessage response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    var jsonstring = response.Content.ReadAsStringAsync();
                    jsonstring.Wait();
                    obj = Serializers.DeserializeJson<BaseResponse<TDto>>(jsonstring.Result);
                    client.Dispose();
                }
                else
                {
                    obj.IsSuccess = false;
                    obj.Message = ErrorLanguage.ResourceManager.GetString("GeneralError", Thread.CurrentThread.CurrentCulture);
                }
            }
            catch (Exception ex)
            {
                _logworker.AddErrorLogEnqueue(new Helpers.Models.Dtos.LogModelDtos.ErrorLogDto() { Application = _options.Value.Name, Message = ex.Message, StackTrace = ex.StackTrace });
                obj.IsSuccess = false;
                obj.Message = ErrorLanguage.ResourceManager.GetString("GeneralError", Thread.CurrentThread.CurrentCulture);
            }
            return obj;
        }

        public BaseResponse<TDto> Post<T>(string url, T objx, string token = "")
        {
            BaseResponse<TDto> obj = (BaseResponse<TDto>)Activator.CreateInstance(typeof(BaseResponse<TDto>));
            HttpClient client = getClient(url);
            try
            {
                if (!String.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Add("token", token);
                }

                string json = Helpers.CommonMethods.Serializers.SerializeJson(objx);
                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync(url, httpContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    var jsonstring = response.Content.ReadAsStringAsync();
                    jsonstring.Wait();
                    obj = Serializers.DeserializeJson<BaseResponse<TDto>>(jsonstring.Result);
                    client.Dispose();
                }
                else
                {
                    obj.IsSuccess = false;
                    obj.Message = ErrorLanguage.ResourceManager.GetString("GeneralError", Thread.CurrentThread.CurrentCulture);
                }
            }
            catch (Exception ex)
            {
                _logworker.AddErrorLogEnqueue(new Helpers.Models.Dtos.LogModelDtos.ErrorLogDto() { Application = _options.Value.Name, Message = ex.Message, StackTrace = ex.StackTrace });
                obj.IsSuccess = false;
                obj.Message = ErrorLanguage.ResourceManager.GetString("GeneralError", Thread.CurrentThread.CurrentCulture);
            }
            return obj;
        }

        public BaseResponse<TDto> Put<T>(string url, T objx, string token = "")
        {
            BaseResponse<TDto> obj = (BaseResponse<TDto>)Activator.CreateInstance(typeof(BaseResponse<TDto>));
            HttpClient client = getClient(url);
            try
            {
                if (!String.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Add("token", token);
                }

                string json = Helpers.CommonMethods.Serializers.SerializeJson(objx);
                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PutAsync(url, httpContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    var jsonstring = response.Content.ReadAsStringAsync();
                    jsonstring.Wait();
                    obj = Serializers.DeserializeJson<BaseResponse<TDto>>(jsonstring.Result);
                    client.Dispose();
                }
                else
                {
                    obj.IsSuccess = false;
                    obj.Message = ErrorLanguage.ResourceManager.GetString("GeneralError", Thread.CurrentThread.CurrentCulture);
                }
            }
            catch (Exception ex)
            {
                _logworker.AddErrorLogEnqueue(new Helpers.Models.Dtos.LogModelDtos.ErrorLogDto() { Application = _options.Value.Name, Message = ex.Message, StackTrace = ex.StackTrace });
                obj.IsSuccess = false;
                obj.Message = Helpers.Resources.ErrorLanguage.ResourceManager.GetString("GeneralError", Thread.CurrentThread.CurrentCulture);
            }
            return obj;
        }

        public BaseResponse<TDto> Patch<T>(string url, T objx, string token = "")
        {
            BaseResponse<TDto> obj = (BaseResponse<TDto>)Activator.CreateInstance(typeof(BaseResponse<TDto>));
            HttpClient client = getClient(url);
            try
            {
                if (!String.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Add("token", token);
                }

                string json = Helpers.CommonMethods.Serializers.SerializeJson(objx);
                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PatchAsync(url, httpContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    var jsonstring = response.Content.ReadAsStringAsync();
                    jsonstring.Wait();
                    obj = Serializers.DeserializeJson<BaseResponse<TDto>>(jsonstring.Result);
                    client.Dispose();
                }
                else
                {
                    obj.IsSuccess = false;
                    obj.Message = ErrorLanguage.ResourceManager.GetString("GeneralError", Thread.CurrentThread.CurrentCulture);
                }
            }
            catch (Exception ex)
            {
                _logworker.AddErrorLogEnqueue(new Helpers.Models.Dtos.LogModelDtos.ErrorLogDto() { Application = _options.Value.Name, Message = ex.Message, StackTrace = ex.StackTrace });
                obj.IsSuccess = false;
                obj.Message = ErrorLanguage.ResourceManager.GetString("GeneralError", Thread.CurrentThread.CurrentCulture);
            }
            return obj;
        }
    }
}

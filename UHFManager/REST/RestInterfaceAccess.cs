using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UHFManager.DTO;

namespace UHFManager.REST
{
    class RestInterfaceAccess
    {
        #region 属性

        /// <summary>
        /// 接口基地址
        /// </summary>
        public string BaseUrl { get; set; }
        
        #endregion

        #region 构造

        public RestInterfaceAccess(string baseUrl)
        {
            this.BaseUrl = baseUrl;
        }


        #endregion
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        public TResponse Post<TRequest, TResponse>(TRequest config) where TRequest : RequestBase where TResponse : ResponseBase, new()
        {
            TResponse result = default(TResponse);

            var client = new RestClient(this.BaseUrl);
            var req = new RestRequest(Method.POST); 
            
            req.AddParameter("cmd", config.cmd, ParameterType.GetOrPost);
            req.AddParameter("data", config.GetDataString(), ParameterType.GetOrPost);
            req.AddParameter("version", config.version, ParameterType.GetOrPost);

            var response = client.Execute<TResponse>(req);
            if (response == null)
            {
                Debug.WriteLine("调用接口返回 null");
            }
            else if (response.ResponseStatus == ResponseStatus.Completed) 
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    result = response.Data;
                }
                else
                {
                    result = new TResponse();
                    //result.ErrorCode = (int)response.StatusCode;
                    //result.ErrorMessage = "访问服务器错误";
                }

                
            }

            return result;
        }

        
    }
}

using System.Collections.Generic;
using Business.Connectors.Response;
using Common.DTOs;

namespace Business.Handlers.Response
{
    /// <summary>
    /// Response from Business
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Response<T> where T : BaseDTO
    {
        public ICollection<T> Data { get; set; }

        public static explicit operator Response<T>(BusinessResponse<T> businessResponse)
        {
            return new Response<T>
            {
                Data = businessResponse.Data
            };
        }
    }
}
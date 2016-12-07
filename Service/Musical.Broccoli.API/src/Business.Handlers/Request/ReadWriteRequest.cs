using System.Collections.Generic;
using Business.Connectors.Petition;
using Common.DTOs;

namespace Business.Handlers.Request
{
    /// <summary>
    /// Request from Controller
    /// Data added for Write requests
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ReadWriteRequest<T> : Request where T : BaseDTO
    {
        public List<T> Data { get; set; }

        public static explicit operator ReadWriteBusinessPetition<T>(ReadWriteRequest<T> request)
        {
            return new ReadWriteBusinessPetition<T>
            {
                Data = request.Data
            };
        }
    }
}
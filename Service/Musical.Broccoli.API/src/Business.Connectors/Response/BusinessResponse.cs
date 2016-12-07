using System;
using System.Collections.Generic;
using Common.DTOs;

namespace Business.Connectors.Response
{
    /// <summary>
    /// Response from Business Action to Controller
    /// </summary>
    /// <typeparam name="T">DTO Entity</typeparam>
    public class BusinessResponse<T> where T : BaseDTO
    {
        public bool IsSuccessful { get; set; }
        public List<T> Data { get; set; }
        public List<Exception> Exceptions { get; set; }
    }
}
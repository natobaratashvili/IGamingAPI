using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGaming.Core.Common
{
    public class Response<T>
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public T Data { get; }
        public List<string> ErrorMessages { get; } = new List<string>();

        private Response(bool isSuccess, T data, string? errorMessage)
        {
            IsSuccess = isSuccess;
            Data = data;
            if (!string.IsNullOrEmpty(errorMessage))
            {
                ErrorMessages.Add(errorMessage);
            }
        }
        private Response(bool isSuccess, T data, List<string> errorMessages)
        {
            IsSuccess = isSuccess;
            Data = data;
            ErrorMessages.AddRange(errorMessages);
        }

        public static Response<T> Success(T data)
        {
            return new Response<T>(true, data, default(string));
        }
        public static Response<T> Success()
        {
            return new Response<T>(true, default!, default(string));
        }

        public static Response<T> Failure(string errorMessage)
        {
            return new Response<T>(false, default!, errorMessage);
        }
        public static Response<T> Failure(int code)
        {
            return new Response<T>(false, default!, "Failure");
        }
        public static Response<T> Failure(List<string> errorMessages)
        {
            return new Response<T>(false, default!, errorMessages);
        }
    }
}

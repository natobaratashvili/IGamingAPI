using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace IGaming.Core.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        internal int Code { get; set; }
        public string Status => IsFailure ? "Error" : "Success";
        public Dictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();
        public int GetStatusCode()
        {
            return Code;
        }
        protected Result(bool isSuccess, string? key, string? errorMessage, int code)
        {
            IsSuccess = isSuccess;
            Code = code;
            if (!string.IsNullOrEmpty(errorMessage) && !string.IsNullOrEmpty(key))
            {
                Errors[key] = errorMessage;
            }
        }

        public static Result Success()
        {
            return new Result(true, default, default, 200);
        }
        public static Result Success(int code)
        {
            return new Result(true, default, default, code);
        }

        public static Result Failure(string name, string message, int code)
        {
            return new Result(false, name, message, code);
        }
        public static Result Failure(int code)
        {
            return new Result(false, default, default, code);
        }
        
    }

    public class Result<T> : Result
    {
        public T Data { get; }

        private Result(bool isSuccess, T data, string? key, string? errorMessage, int code)
            : base(isSuccess, key, errorMessage, code)
        {
            Data = data;
        }

        public static Result<T> Success(T data)
        {
            return new Result<T>(true, data, default, default, 200);
        }

        public static Result<T> Success(int code)
        {
            return new Result<T>(true, default!, default, default,code );
        }

        public static Result<T> Failure(string name, string message)
        {
            return new Result<T>(false, default!, name, message, 400);
        }

        public static Result<T> Failure(int code)
        {
            return new Result<T>(false, default!, default, default,code );
        }
    }
}

using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BloodCenterManagementSystem.Logics
{
    public class Result
    {
        public bool IsSuccessfull { get; set; }
        public IEnumerable<ErrorMessage> ErrorMessages = new List<ErrorMessage>();

        public static Result<T>Ok<T>(T value)
        {
            return new Result<T>
            {
                IsSuccessfull = true,
                Value = value
            };
        }

        public static Result<T> Error<T>(string message)
        {
            return new Result<T>
            {
                IsSuccessfull = false,
                ErrorMessages = new List<ErrorMessage>
                {
                    new ErrorMessage
                    {
                        PropertyName=string.Empty,
                        Message=message
                    }
                }
            };
        }

        public static Result<T>Error<T>(IEnumerable<ValidationFailure> validationFailures)
        {
            return new Result<T>
            {
                IsSuccessfull = false,
                ErrorMessages = validationFailures.Select(v => new ErrorMessage()
                {
                    PropertyName = v.PropertyName,
                    Message = v.ErrorMessage
                })
            };
        }

    }

    public class Result<T> : Result
    {
        public T Value { get; set; }
    }

    public class ErrorMessage
    {
        public string PropertyName { get; set; }
        public string Message { get; set; }
    }
}

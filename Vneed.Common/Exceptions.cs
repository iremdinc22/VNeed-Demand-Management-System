using System;
using System.Collections.Generic;
using Vneed.Common.Models;

namespace Vneed.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

public class ValidationException : Exception
{
    public List<ApiValidationErrorDetail> Errors { get; }

    public ValidationException(List<ApiValidationErrorDetail> errors)
    {
        Errors = errors;
    }
}

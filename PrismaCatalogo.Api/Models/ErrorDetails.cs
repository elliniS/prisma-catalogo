using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace PrismaCatalogo.Api.Models;

public class ErrorDetails 
{
    public int StatusCode { get; set; }
    public string? Message { get; set; }
    public IEnumerable<string> Errors { get; set; }

    public ErrorDetails(int statusCode, string menssage, IEnumerable<string> errors)
    {
        StatusCode = statusCode;
        Message = menssage;
        Errors = errors;
    }
}
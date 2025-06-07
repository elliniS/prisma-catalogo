using System.ComponentModel.DataAnnotations;

namespace PrismaCatalogo.Api.DTOs.CorDTO
{
    public class PostRequestDTO
    {
        public string? Media { get; set; }
        public string? Caption { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PrismaCatalogo.Api.Extensions
{
    public static class ModelStateExtensions
    {
        public static List<string> GetErrosMenssages(this ModelStateDictionary modelState)
        {
            return modelState.SelectMany(e => e.Value.Errors)
                .Select(m => m.ErrorMessage).ToList();
        }
    }
}

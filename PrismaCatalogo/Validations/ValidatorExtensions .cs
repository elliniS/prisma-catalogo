using FluentValidation;

namespace PrismaCatalogo.Validations
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> IsUnique<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, IEnumerable<T> values)
        {
            return ruleBuilder.SetValidator(new UniqueValidator<T, TProperty>(values));
        }
    }
}

using FluentValidation;
using FluentValidation.Validators;
using System.Reflection;

namespace PrismaCatalogo.Validations
{
    public class UniqueValidator<T, TProperty> : PropertyValidator<T, TProperty>
    {
         private readonly IEnumerable<T> _values;

         public UniqueValidator(IEnumerable<T> values) {
            _values = values;
         }

        public override string Name => "UniqueValidator";

        protected override string GetDefaultMessageTemplate(string errorCode)
        {
            return "{PropertyName} já existe!";
        }

        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            if (value is not null && _values != null)
            {
                var property = typeof(T).GetTypeInfo().GetDeclaredProperty(context.PropertyName);
                return !_values.Any(p => property.GetValue(p).ToString().Equals(value.ToString()));
            }
            return true;
        }
    }
}

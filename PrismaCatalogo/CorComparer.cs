using PrismaCatalogo.Web.Models;
using System.Diagnostics.CodeAnalysis;

namespace PrismaCatalogo.Web
{
    public class CorComparer : IEqualityComparer<CorViewModel>
    {
        public bool Equals(CorViewModel? x, CorViewModel? y)
        {
            if(Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the products' properties are equal.
            return x.Id == y.Id;
        }

        public int GetHashCode([DisallowNull] CorViewModel obj)
        {
            if (obj == null)
                return obj.GetHashCode();

            return obj.Id.ToString().GetHashCode();
        }
    }
}

namespace PrismaCatalogo.Api.Exceptions
{
    public class APIException : Exception
    { 
        public int CodeStatus {  get; set; }

        public APIException(string message, int CodeStatus) : base(message)
        {
            this.CodeStatus = CodeStatus;
        }
    }
}

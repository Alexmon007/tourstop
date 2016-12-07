using Business.Connectors.Petition;

namespace Business.Handlers.Request
{
    /// <summary>
    /// Resquest from service
    /// </summary>
    public abstract class Request
    {
        public string AuthToken { get; set; }

        public static explicit operator BusinessPetition(Request request)
        {
            return new BusinessPetition();
        }
    }
}
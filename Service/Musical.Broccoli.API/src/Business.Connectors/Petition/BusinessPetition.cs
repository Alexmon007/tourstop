using Common.DTOs;

namespace Business.Connectors.Petition
{
    /// <summary>
    /// Pettion is sen from the Service Controller to the Business Layer
    /// </summary>
    public class BusinessPetition
    {
        public PetitionAction Action { get; set; }
        public UserDTO RequestingUser { get; set; }
    }
}
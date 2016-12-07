namespace Business.Connectors.Petition
{
    /// <summary>
    /// Pettion used for GET petitions
    /// </summary>
    public class ReadBusinessPetition : BusinessPetition
    {
        public string FilterString { get; set; }
    }
}
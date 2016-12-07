using System.Collections.Generic;
using Common.DTOs;

namespace Business.Connectors.Petition
{
    /// <summary>
    /// Pettion used for SAVE petitions
    /// </summary>
    public class ReadWriteBusinessPetition<T> : BusinessPetition where T : BaseDTO
    {
        public List<T> Data { get; set; }
    }
}
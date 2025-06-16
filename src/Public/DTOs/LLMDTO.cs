using EcoScale.src.Models;

namespace EcoScale.src.Public.DTOs
{
    public class GetCriteriosRequest
    {
        public required string contexto { get; set; }
    }
    
    public class GetLLMResponseRequest
    {
        public required Questionario questionario { get; set; }
    }
}
using EcoScale.src.Models;
using EcoScale.src.Public.DTOs;

namespace EcoScale.src.Services.Interfaces
{
    public interface IQuestionario {
        Task<CreationResponse<Questionario>> New(QuestionarioCreationRequest q);
        Task<Questionario> Get(int id);
        Task<Questionario> Del(int id);
        // Task<Questionario> Patch(QuestionarioUpdateRequest request);
    }
}
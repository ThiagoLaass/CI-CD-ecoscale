using EcoScale.src.Models;
using EcoScale.src.Models.Abstract;

namespace EcoScale.src.Public.DTOs
{

    public class AreaUpdateRequest
    {
        public required int Id { get; set; }
        public string? Nome { get; set; }
        public ICollection<TemaUpdateRequest>? Temas { get; set; }
    }

    public class RespostaEmBlocosRequest
    {
        public required int QuestionarioId { get; set; }
        public ICollection<ItemAvaliadoUpdateRequest>? Itens { get; set; }
    }

    public class TemaUpdateRequest
    {
        public required int Id { get; set; }
        public string? Nome { get; set; }
        public ICollection<CriterioUpdateRequest>? Criterios { get; set; }
    }

    public class CriterioUpdateRequest
    {
        public required int Id { get; set; }
        public string? Nome { get; set; }
        public ICollection<ItemAvaliadoUpdateRequest>? Itens { get; set; }
    }

    public class ItemAvaliadoUpdateRequest
    {
        public required int Id { get; set; }
        public string? Descricao { get; set; }
        public string? Resposta { get; set; }
    }


    public class QuestionarioCreationRequest
    {
        public required string Contexto { get; set; }
    }

    public class AreaCreationRequest
    {
        public required string Nome { get; set; }
        public required ICollection<TemaCreationRequest> Temas { get; set; }
    }

    public class TemaCreationRequest
    {
        public required string Nome { get; set; }
        public required ICollection<CriterioCreationRequest> Criterios { get; set; }
    }

    public class CriterioCreationRequest
    {
        public required string Nome { get; set; }
        public required ICollection<ItemAvaliadoCreationRequest> Itens { get; set; }
    }

    public class ItemAvaliadoCreationRequest
    {
        public required string Descricao { get; set; }
        public string? Resposta { get; set; }
    }

    public class GetQuestionarioResponse
    {
        public required ICollection<Questionario> Questionarios { get; set; }
    }

    public class GetCriteriosByIdsRequest
    {
        public required ICollection<int> CriteriosIds { get; set; }
    }
    
    public class QuestionarioResponse
    {
        public int? QuestionarioId { get; set; }
        public required ICollection<Area> Areas { get; set; }
    }
}
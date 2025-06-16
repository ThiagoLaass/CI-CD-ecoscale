namespace EcoScale.src.Public.DTOs
{
    public class PlanilhaCreationRequest
    {
        public required ICollection<AreaPlanilhaCreationRequest> Areas { get; set; }
    }

    public class AreaPlanilhaCreationRequest
    {
        public required string Nome { get; set; }
        public required ICollection<TemaPlanilhaCreationRequest> Temas { get; set; }
    }

    public class TemaPlanilhaCreationRequest
    {
        public required string Nome { get; set; }
        public required ICollection<CriterioPlanilhaCreationRequest> Criterios { get; set; }
    }

    public class CriterioPlanilhaCreationRequest
    {
        public required string Nome { get; set; }
        public required ICollection<ItemAvaliadoPlanilhaCreationRequest> Itens { get; set; }
    }

    public class ItemAvaliadoPlanilhaCreationRequest
    {
        public required string Descricao { get; set; }
    }

    public class PlanilhaUpdateRequest
    {
        public required int Id { get; set; }
        public string? Nome { get; set; }
        public ICollection<AreaPlanilhaUpdateRequest>? Areas { get; set; }
    }

    public class AreaPlanilhaUpdateRequest
    {
        public required int Id { get; set; }
        public string? Nome { get; set; }
        public ICollection<TemaPlanilhaUpdateRequest>? Temas { get; set; }
    }

    public class TemaPlanilhaUpdateRequest
    {
        public required int Id { get; set; }
        public string? Nome { get; set; }
        public ICollection<CriterioPlanilhaUpdateRequest>? Criterios { get; set; }
    }

    public class CriterioPlanilhaUpdateRequest
    {
        public required int Id { get; set; }
        public string? Nome { get; set; }
        public ICollection<ItemAvaliadoPlanilhaUpdateRequest>? Itens { get; set; }
    }

    public class ItemAvaliadoPlanilhaUpdateRequest
    {
        public required int Id { get; set; }
        public string? Descricao { get; set; }
    }
}
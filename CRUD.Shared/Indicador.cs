namespace CRUD.Shared
{
    public class Indicador
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Objetivo { get; set; } = string.Empty;
        public string Alcance { get; set; } = string.Empty;
        public string Formula { get; set; } = string.Empty;
        public int FkIdTipoIndicador { get; set; }
        public int FkIdUnidadMedicion { get; set; }
        public string Meta { get; set; } = string.Empty;
        public int FkIdSentido { get; set; }
        public int FkIdFrecuencia { get; set; }
        public string FkIdArticulo { get; set; } = string.Empty;
        public string FkIdLiteral { get; set; } = string.Empty;
        public string FkIdNumeral { get; set; } = string.Empty;
        public string FkIdParagrafo { get; set; } = string.Empty;
    }

}
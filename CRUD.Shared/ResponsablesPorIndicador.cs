namespace CRUD.Shared
{
    public class ResponsablesPorIndicador
    {
        public string FkIdResponsable { get; set; } = string.Empty;
        public int FkIdIndicador { get; set; }
        public DateTime FechaAsignacion { get; set; } = DateTime.Now;
    }
}
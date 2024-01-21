namespace EMS.WebAPI.Core.Authentication;

public class AppSettings
{
    public string? Secret { get; set; }
    public int ExpiracaoHoras { get; set; }
    public string? Emissor { get; set; }
    public string? ValidoEm { get; set; }
}
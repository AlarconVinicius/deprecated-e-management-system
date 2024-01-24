namespace EMS.WebApp.MVC.Models;

public class PlanViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string SubTitle { get; set; } = string.Empty;
    public double Price { get; set; }
    public string Benefits { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
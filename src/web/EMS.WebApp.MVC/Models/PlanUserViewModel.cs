namespace EMS.WebApp.MVC.Models;

public class PlanUserViewModel
{
    public PlanViewModel Plan { get; set; } = new PlanViewModel();
    public RegisterUser RegisterUser { get; set; } = new RegisterUser();
}

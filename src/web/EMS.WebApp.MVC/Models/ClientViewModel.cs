using System.ComponentModel;

namespace EMS.WebApp.MVC.Models
{
    public class ClientViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        [DisplayName("CPF")]
        public string Cpf { get; set; } = string.Empty;
        [DisplayName("Status")]
        public bool IsDeleted { get; set; }
        public Guid SubscriberId { get; set; }

        //public ClientViewModel()
        //{
            
        //}
        //public ClientViewModel(Guid id, string name, string email, string cpf, bool isDeleted, Guid subscriberId)
        //{
        //    Id = id;
        //    Name = name;
        //    Email = email;
        //    Cpf = cpf;
        //    IsDeleted = isDeleted;
        //    SubscriberId = subscriberId;
        //}
    }
}
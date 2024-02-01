using EMS.WebApp.MVC.Extensions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EMS.WebApp.MVC.Models
{
    public class ClientViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres.", MinimumLength = 2)]
        [DisplayName("Nome")]
        public string Name { get; set; } = string.Empty;
        [Cpf]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("CPF")]
        public string Cpf { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
        [DisplayName("E-mail")]
        public string Email { get; set; } = string.Empty;

        [DisplayName("Status")]
        public bool IsDeleted { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Id do Assinante")]
        public Guid SubscriberId { get; set; }
        public ResponseResult ResponseResult { get; set; } = new ResponseResult();

        public ClientViewModel() { }
    }
    public class ClientViewModels
    {
        public ClientViewModel Client { get; set; }
        public IEnumerable<ClientViewModel> Clients { get; set; } 
        public ClientViewModels(ClientViewModel client, IEnumerable<ClientViewModel> clients)
        {
            Client = client;
            Clients = clients;
        }
    }
    public class ClientResponse
    {
        public ResponseResult ResponseResult { get; set; } = new ResponseResult();

        public ClientResponse()
        {
            ResponseResult = new ResponseResult();
        }
    }
}
using EMS.Subscription.API.Data.Repository;
using EMS.Subscription.API.Model;
using EMS.WebAPI.Core.Authentication;
using EMS.WebAPI.Core.Controllers;
using EMS.WebAPI.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Subscription.API.Controllers
{
    [Authorize]
    [Route("api/plans")]
    public class PlansController : MainController
    {
        private readonly IPlanRepository _planRepository;
        public PlansController(INotifier notifier, IPlanRepository planRepository) : base(notifier)
        {
            _planRepository = planRepository;
        }

        [AllowAnonymous]
        [HttpGet()]
        public async Task<IEnumerable<Plan>> GetAll()
        {
            return await _planRepository.GetAll();
        }

        [AllowAnonymous]
        //[ClaimsAuthorize("Plan", "Read")]
        [HttpGet("{id}")]
        public async Task<Plan> GetById(Guid id)
        {
            return await _planRepository.GetById(id);
        }
    }
}

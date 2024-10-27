using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        protected readonly IMapper _mapper;
        protected readonly ILogger<BaseController> _logger;

        public BaseController(IMapper mapper, ILogger<BaseController> logger)
        {
            _mapper = mapper;
            _logger = logger;
        }
    }
}
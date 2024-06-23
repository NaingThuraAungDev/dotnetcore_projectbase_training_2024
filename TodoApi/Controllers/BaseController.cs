using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<T> : ControllerBase
    {
        private ILogger<T>? logger;
        protected ILogger<T>? Logger => logger ??= HttpContext.RequestServices.GetService<ILogger<T>>();

    }
}
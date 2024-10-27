using Microsoft.AspNetCore.Mvc;
using Server.Infrastruct.WebAPI.Filter;

namespace Server.Infrastruct.WebAPI.Controllers
{
    [APIResultFilter]
    [ApiController]
    [ServiceFilter(typeof(AuthorizeFilter))]
    public class APIBaseController : ControllerBase
    {
    }
}

using System.Web.Http;
using Falcon.Web.Api.ExceptionHandle;

namespace DaiPhucVinh.Api
{
    [ApiExceptionFilter]
    [Authorize]
    public class SecureApiController : ApiController
    {
    }
}

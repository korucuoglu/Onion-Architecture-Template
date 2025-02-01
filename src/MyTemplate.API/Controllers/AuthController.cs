using Common.Attributes.ApiKey;
using Common.Controllers;
namespace MyTemplate.API.Controllers;

[Route("api/[controller]")]
[TypeFilter(typeof(DynamicApiKeyActionFilter))]
public class AuthController: BaseAuthController
{
    
}
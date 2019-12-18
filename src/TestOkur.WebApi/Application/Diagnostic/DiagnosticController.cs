namespace TestOkur.WebApi.Application.Diagnostic
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.ComponentModel.DataAnnotations;
    using TestOkur.Infrastructure.Mvc.Diagnostic;
    using TestOkur.WebApi.Configuration;

    [Route("api/diagnostic")]
    [AllowAnonymous]
    public class DiagnosticController : ControllerBase
    {
        private readonly ApplicationConfiguration _applicationConfiguration;

        public DiagnosticController(ApplicationConfiguration applicationConfiguration)
        {
            _applicationConfiguration = applicationConfiguration;
        }

        public IActionResult Get([FromQuery, Required] string key)
        {
            return _applicationConfiguration.Key != key
                ? (IActionResult)Unauthorized()
                : Ok(DiagnosticReport.Generate());
        }
    }
}

namespace TestOkur.Report.Controllers
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TestOkur.Infrastructure.Mvc.Diagnostic;
    using TestOkur.Report.Configuration;

    [Route("api/diagnostic")]
    [AllowAnonymous]
    [ApiController]
    public class DiagnosticController : ControllerBase
    {
        private readonly ReportConfiguration _applicationConfiguration;

        public DiagnosticController(ReportConfiguration applicationConfiguration)
        {
            _applicationConfiguration = applicationConfiguration;
        }

        public IActionResult Get([FromQuery, Required] string key)
        {
            return _applicationConfiguration.Key != key
                ? (IActionResult)Unauthorized()
                : Content(DiagnosticReport.Generate().ToString());
        }
    }
}

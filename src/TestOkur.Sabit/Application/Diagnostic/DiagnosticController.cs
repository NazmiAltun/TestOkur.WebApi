namespace TestOkur.Sabit.Application.Diagnostic
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TestOkur.Infrastructure.Mvc.Diagnostic;

    [Route("api/diagnostic")]
    [AllowAnonymous]
    public class DiagnosticController : ControllerBase
    {
        public IActionResult Get()
        {
            return Ok(DiagnosticReport.Generate());
        }
    }
}

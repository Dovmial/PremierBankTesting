using Microsoft.AspNetCore.Mvc;
using PremierBankTesting.DTOs;
using PremierBankTesting.Services.Interfaces;

namespace PremierBankTesting.Controllers
{
    [Route("api/user")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Пользователи")]
    public class PremierBankUserController(
        IBankFacadeService bankService,
        ILogger<PremierBankUserController> logger) : ControllerBase
    {
        [HttpPost("add")]
        public async Task<ActionResult> Create(
            [FromBody] BankUserAddRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Создание пользователя {email}", request.Email);
                await bankService.CreateUserAsync(request, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError("{ex}", ex);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<BankUserGetResponse>>> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Запрос всех пользователей");
                var result = await bankService.GetAll(cancellationToken);
                logger.LogInformation("Найдено {count} записей", result.Count());
                return Ok(result);
            }
            catch(Exception ex)
            {
                logger.LogError("{ex}", ex);
                return StatusCode(500, ex.Message);
            }
        }
    }
}

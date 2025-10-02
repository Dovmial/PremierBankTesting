
using Microsoft.AspNetCore.Mvc;
using PremierBankTesting.Clients.Bank;
using PremierBankTesting.DTOs;
using PremierBankTesting.Exceptions;
using PremierBankTesting.Services.Interfaces;

namespace PremierBankTesting.Controllers
{
    [Route("api/transaction")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Транзакции")]
    public class PremierBankTransactionController(
        IBankApiClient bankApiClient,
        IBankFacadeService bankFacadeService,
        ILogger<PremierBankTransactionController> logger)
        : ControllerBase
    {
        [HttpGet("TestTransactionsFromClient")]
        public async Task<ActionResult<BankTransactionResponse>> GetTestTransactions(
            CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Запрос тестовых транзакций..");
                var result = await bankApiClient.GetRecentTransactionsAsync(cancellationToken);
                if (result is null)
                {
                    logger.LogInformation("Транзакций не найдено.");
                    return NotFound();
                }

                logger.LogInformation("Успешно.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError("{ex}", ex);
                return StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<BankTransactionResponse>>> Get(
            [FromQuery] bool isProcessed = false,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await bankFacadeService.GetTransactions(isProcessed, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError("{ex}", ex);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("saveTestTransactions")]
        public async Task<ActionResult> SaveTestTransaction(CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Запрос тестовых транзакций..");
                var transactionDTOs = await bankApiClient.GetRecentTransactionsAsync(cancellationToken);
                if (transactionDTOs is null)
                {
                    logger.LogInformation("Транзакций не найдено.");
                    return NotFound();
                }
                logger.LogInformation("Попытка записи...");
                await bankFacadeService.SaveRangeAsync(transactionDTOs, cancellationToken);
                logger.LogInformation("Данные внесены");
                return Ok();
            }
            catch(BadRequestException ex)
            {
                logger.LogError("{ex}", ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError("{ex}", ex);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("setProcessed")]
        public async Task<ActionResult> SetProcessedUpdate(
            [FromBody] SetProcessedQuery query,
            CancellationToken cancellationToken)
        {
            try
            {
                await bankFacadeService.SetProcessedUpdateAsync(
                    query.Guids,
                    query.IsProcessed,
                    cancellationToken);
                return Ok();
            }
            catch (BadRequestException ex)
            {
                logger.LogError("{ex}", ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError("{ex}", ex);
                return StatusCode(500, ex.Message);
            }
        }
    }
}

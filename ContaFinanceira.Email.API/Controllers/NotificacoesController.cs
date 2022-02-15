using ContaFinanceira.Email.Domain.Interfaces;
using ContaFinanceira.Email.Domain.Requests;
using ContaFinanceira.Email.Domain.ViewModels;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace ContaFinanceira.Email.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificacoesController : ControllerBase
    {
        private readonly ILogger<NotificacoesController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public NotificacoesController(ILogger<NotificacoesController> logger,
                                      IEmailService emailService,
                                      IConfiguration configuration)
        {
            _logger = logger;
            _emailService = emailService;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Notificar(TransacaoRequest request)
        {
            try
            {
                _logger.LogInformation("Processando envio de notificação da transação {obj}", JsonConvert.SerializeObject(request));

                var settings = new EmailSettingsVM()
                {
                    Nome = _configuration.GetSection("EmailSettings:Nome").Value,
                    Email = _configuration.GetSection("EmailSettings:Email").Value,
                    Host = _configuration.GetSection("EmailSettings:Host").Value,
                    Port = Convert.ToInt32(_configuration.GetSection("EmailSettings:Port").Value),
                    Senha = _configuration.GetSection("EmailSettings:Senha").Value,
                };

                _logger.LogDebug("Dados de remetente do e-mail: {dados}", settings);

                await _emailService.Enviar(request, settings);

                _logger.LogInformation("Notificação enviada com sucesso!");

                return new OkResult();
            }
            catch(ValidationException val)
            {
                _logger.LogWarning("Erro ao validar requisição. Detalhes: {erros}", JsonConvert.SerializeObject(val.Errors));

                return new BadRequestObjectResult(val);
            }
            catch(Exception ex)
            {
                _logger.LogError("Erro ao enviar notificação. Detalhes: {erro}", JsonConvert.SerializeObject(ex));

                return StatusCode(500, ex.Message);
            }
        }
    }
}

using ContaFinanceira.Email.Domain.Interfaces;
using ContaFinanceira.Email.Domain.Requests;
using ContaFinanceira.Email.Domain.ViewModels;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ContaFinanceira.Email.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        public async Task Enviar(TransacaoRequest request, EmailSettingsVM remetente)
        {
            _logger.LogInformation("Processando envio de email com requisições {request} e configurações {settings}",
                                   JsonConvert.SerializeObject(request),
                                   JsonConvert.SerializeObject(remetente));

            var corpoMensagem = GetCorpoMensagem(request);

            _logger.LogInformation("Corpo da mensagem: {body}", corpoMensagem);

            var mensagem = new MailMessage();
            mensagem.From = new MailAddress(remetente.Email, remetente.Nome);
            mensagem.To.Add(new MailAddress(request.Cliente.Email));
            mensagem.Subject = "[Notificação] Nova transação realizada";
            mensagem.IsBodyHtml = true;
            mensagem.Body = corpoMensagem;

            _logger.LogDebug("Detalhes de MailMessage: {mensagem}", JsonConvert.SerializeObject(mensagem));

            var smtp = new SmtpClient();
            smtp.Port = remetente.Port;
            smtp.Host = remetente.Host;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(remetente.Email, remetente.Senha);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            _logger.LogDebug("Detalhes de SMTP: {smtp}", JsonConvert.SerializeObject(smtp));

            await smtp.SendMailAsync(mensagem);

            smtp.Dispose();

            _logger.LogInformation("E-mail enviado com sucesso.");
        }

        private string GetCorpoMensagem(TransacaoRequest request)
        {
            return string.Format(@"<h1>Nova transação realizada</h1><br><br>" +
                                  "<p>Caro(a) {0},</p><br>" +
                                  "<p>Este é um e-mail automático para notificá-lo sobre uma nova transação realizada na sua conta nº {1}<p>" +
                                  "<p>Veja detalhes dessa operação abaixo:<p><br>" +
                                  "<p><b>Tipo de Operação:</b> {2}</p>" +
                                  "<p><b>Data:</b> {3}</p>" +
                                  "<p><b>Valor:</b> R$ {4}</p><br>" +
                                  "<p>Seu saldo agora é de: <b>R$ {5}</b></p>",
                                  request.Cliente.Nome,
                                  request.Conta.Id,
                                  request.Valor > 0 ? "Depósito" : "Saque",
                                  string.Format("{0}/{1}/{2} {3}:{4}", request.Data.Day, request.Data.Month, request.Data.Year, request.Data.Hour, request.Data.Minute),
                                  request.Valor.ToString().Replace(".", ","),
                                  request.Conta.Saldo.ToString().Replace(".", ","));
        }
    }
}

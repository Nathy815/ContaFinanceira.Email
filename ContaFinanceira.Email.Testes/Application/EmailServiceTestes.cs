using ContaFinanceira.Email.Application.Services;
using ContaFinanceira.Email.Domain.Requests;
using ContaFinanceira.Email.Domain.ViewModels;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContaFinanceira.Email.Testes.Application
{
    public class EmailServiceTestes
    {
        private readonly Mock<ILogger<EmailService>> _logger;

        public EmailServiceTestes()
        {
            _logger = new Mock<ILogger<EmailService>>();
        }

        [Fact]
        public async Task Enviar_Sucesso()
        {
            //Arrange
            var request = new TransacaoRequest()
            {
                Id = 1,
                Data = DateTime.Now,
                Cliente = new ClienteRequest()
                {
                    Nome = "Nathália Lopes",
                    Email = "nathalialcoimbra@gmail.com"
                },
                Conta = new ContaRequest()
                {
                    Id = 1,
                    Saldo = 50
                },
                Valor = 10
            };

            var settings = new EmailSettingsVM()
            {
                Email = "marguerite.pacocha16@ethereal.email",
                Host = "smtp.ethereal.email",
                Nome = "Nathália Lopes",
                Port = 587,
                Senha = "hSYWNEfKGREQrkmKHf"
            };

            var service = new EmailService(_logger.Object);

            //Act 
            await service.Enviar(request, settings);

            //Assert
            _logger
                .Verify(x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString().Equals("E-mail enviado com sucesso.")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()
                ), Times.Once);
        }

        [Fact]
        public void Enviar_Erro()
        {
            //Arrange
            var request = new TransacaoRequest()
            {
                Id = 1,
                Data = DateTime.Now,
                Cliente = new ClienteRequest()
                {
                    Nome = "Nathália Lopes",
                    Email = "nathalialcoimbra@gmail.com"
                },
                Conta = new ContaRequest()
                {
                    Id = 1,
                    Saldo = 50
                },
                Valor = 10
            };

            var settings = new EmailSettingsVM()
            {
                Host = "smtp.ethereal.email",
                Nome = "Nathália Lopes",
                Port = 587,
                Senha = "hSYWNEfKGREQrkmKHf"
            };

            var service = new EmailService(_logger.Object);

            //Act and Assert
            Assert.ThrowsAsync<Exception>(() => service.Enviar(request, settings));

            _logger
                .Verify(x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString().Equals("E-mail enviado com sucesso.")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()
                ), Times.Never);
        }
    }
}

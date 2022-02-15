using ContaFinanceira.Email.API.Controllers;
using ContaFinanceira.Email.Domain.Interfaces;
using ContaFinanceira.Email.Domain.Requests;
using ContaFinanceira.Email.Domain.ViewModels;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContaFinanceira.Email.Testes.API
{

    public class NotificacoesControllerTestes
    {
        private readonly Mock<ILogger<NotificacoesController>> _logger;
        private readonly Mock<IEmailService> _emailService;
        private readonly Mock<IConfiguration> _configuration;
        private TransacaoRequest _request;

        public NotificacoesControllerTestes()
        {
            _logger = new Mock<ILogger<NotificacoesController>>();
            _emailService = new Mock<IEmailService>();
            _configuration = new Mock<IConfiguration>();

            _request = new TransacaoRequest()
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
        }

        [Fact]
        public async Task Notificar_Sucesso()
        {
            //Arrange
            _emailService
                .Setup(x => x.Enviar(It.IsAny<TransacaoRequest>(), It.IsAny<EmailSettingsVM>()))
                .Returns(Task.CompletedTask);

            _configuration
                .Setup(x => x.GetSection(It.IsAny<string>()).Value)
                .Returns("1");

            var controller = new NotificacoesController(_logger.Object, _emailService.Object, _configuration.Object);

            //Act
            var result = await controller.Notificar(_request);

            //Assert
            var model = Assert.IsAssignableFrom<OkResult>(result);
            Assert.Equal(200, model.StatusCode);
        }

        [Fact]
        public async Task Notificar_Erro()
        {
            //Arrange
            _emailService
                .Setup(x => x.Enviar(It.IsAny<TransacaoRequest>(), It.IsAny<EmailSettingsVM>()))
                .Throws(new Exception("Erro ao enviar e-mail."));

            var controller = new NotificacoesController(_logger.Object, _emailService.Object, _configuration.Object);

            //Act
            var result = await controller.Notificar(_request);

            //Assert
            var model = Assert.IsAssignableFrom<ObjectResult>(result);
            Assert.Equal(500, model.StatusCode);
        }

        [Fact]
        public async Task Notificar_Erro_Validacao()
        {
            //Arrange
            _emailService
                .Setup(x => x.Enviar(It.IsAny<TransacaoRequest>(), It.IsAny<EmailSettingsVM>()))
                .Throws(new ValidationException("Por favor, informe o id da transação."));

            _configuration
                .Setup(x => x.GetSection(It.IsAny<string>()).Value)
                .Returns("1");

            var controller = new NotificacoesController(_logger.Object, _emailService.Object, _configuration.Object);

            //Act
            var result = await controller.Notificar(_request);

            //Assert
            var model = Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            Assert.Equal(400, model.StatusCode);
        }
    }
}

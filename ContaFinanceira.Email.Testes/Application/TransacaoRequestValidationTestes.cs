using ContaFinanceira.Email.Application.Validations;
using ContaFinanceira.Email.Domain.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContaFinanceira.Email.Testes.Application
{
    public class TransacaoRequestValidationTestes
    {
        [Fact]
        public async Task TransacaoRequestValidation_Sucesso()
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

            var validator = new TransacaoRequestValidation();

            //Act
            var result = await validator.ValidateAsync(request);

            //Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public async Task TransacaoRequestValidation_Erro_Id()
        {
            //Arrange
            var request = new TransacaoRequest()
            {
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

            var validator = new TransacaoRequestValidation();

            //Act
            var result = await validator.ValidateAsync(request);

            //Assert
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
            Assert.Equal("Por favor, informe o id da transação.", result.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task TransacaoRequestValidation_Erro_Data()
        {
            //Arrange
            var request = new TransacaoRequest()
            {
                Id = 1,
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


            var validator = new TransacaoRequestValidation();

            //Act
            var result = await validator.ValidateAsync(request);

            //Assert
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
            Assert.Equal("Por favor, informe a data da transação.", result.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task TransacaoRequestValidation_Erro_Valor()
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
                }
            };


            var validator = new TransacaoRequestValidation();

            //Act
            var result = await validator.ValidateAsync(request);

            //Assert
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
            Assert.Equal("Por favor, informe o valor da transação.", result.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task TransacaoRequestValidation_Erro_Cliente_Nome()
        {
            //Arrange
            var request = new TransacaoRequest()
            {
                Id = 1,
                Data = DateTime.Now,
                Cliente = new ClienteRequest()
                {
                    Email = "nathalialcoimbra@gmail.com"
                },
                Conta = new ContaRequest()
                {
                    Id = 1,
                    Saldo = 50
                },
                Valor = 10
            };


            var validator = new TransacaoRequestValidation();

            //Act
            var result = await validator.ValidateAsync(request);

            //Assert
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
            Assert.Equal("Por favor, informe o nome do cliente.", result.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task TransacaoRequestValidation_Erro_Cliente_EmailNaoInformado()
        {
            //Arrange
            var request = new TransacaoRequest()
            {
                Id = 1,
                Data = DateTime.Now,
                Cliente = new ClienteRequest()
                {
                    Nome = "Nathália Lopes",
                },
                Conta = new ContaRequest()
                {
                    Id = 1,
                    Saldo = 50
                },
                Valor = 10
            };


            var validator = new TransacaoRequestValidation();

            //Act
            var result = await validator.ValidateAsync(request);

            //Assert
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
            Assert.Equal("Por favor, informe o e-mail do cliente.", result.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task TransacaoRequestValidation_Erro_Cliente_EmailInvalido()
        {
            //Arrange
            var request = new TransacaoRequest()
            {
                Id = 1,
                Data = DateTime.Now,
                Cliente = new ClienteRequest()
                {
                    Nome = "Nathália Lopes",
                    Email = "nathalialcoimbra"
                },
                Conta = new ContaRequest()
                {
                    Id = 1,
                    Saldo = 50
                },
                Valor = 10
            };


            var validator = new TransacaoRequestValidation();

            //Act
            var result = await validator.ValidateAsync(request);

            //Assert
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
            Assert.Equal("E-mail inválido.", result.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task TransacaoRequestValidation_Erro_Conta_Id()
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
                    Saldo = 50
                },
                Valor = 10
            };


            var validator = new TransacaoRequestValidation();

            //Act
            var result = await validator.ValidateAsync(request);

            //Assert
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
            Assert.Equal("Por favor, informe o id da conta.", result.Errors.First().ErrorMessage);
        }
    }
}
using ContaFinanceira.Email.Domain.Requests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContaFinanceira.Email.Application.Validations
{
    public class TransacaoRequestValidation : AbstractValidator<TransacaoRequest>
    {
        public TransacaoRequestValidation()
        {
            RuleFor(x => x.Id)
                .NotEqual(0)
                    .WithMessage("Por favor, informe o id da transação.");

            RuleFor(x => x.Data)
                .NotEqual(DateTime.MinValue)
                    .WithMessage("Por favor, informe a data da transação.");

            RuleFor(x => x.Valor)
                .NotEqual(0)
                    .WithMessage("Por favor, informe o valor da transação.");

            RuleFor(x => x.Cliente.Nome)
                .NotEmpty()
                    .When(x => x.Cliente != null)
                    .WithMessage("Por favor, informe o nome do cliente.");

            RuleFor(x => x.Cliente.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .When(x => x.Cliente != null)
                    .WithMessage("Por favor, informe o e-mail do cliente.")
                .EmailAddress()
                    .WithMessage("E-mail inválido.");

            RuleFor(x => x.Conta.Id)
                .NotEqual(0)
                    .When(x => x.Conta != null)
                    .WithMessage("Por favor, informe o id da conta.");
        }
    }
}

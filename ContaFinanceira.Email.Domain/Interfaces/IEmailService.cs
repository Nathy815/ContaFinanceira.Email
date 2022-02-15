using ContaFinanceira.Email.Domain.Requests;
using ContaFinanceira.Email.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContaFinanceira.Email.Domain.Interfaces
{
    public interface IEmailService
    {
        Task Enviar(TransacaoRequest request, EmailSettingsVM remetente);
    }
}
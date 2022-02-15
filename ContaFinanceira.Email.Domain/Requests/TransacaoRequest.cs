using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContaFinanceira.Email.Domain.Requests
{
    public class TransacaoRequest
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public decimal Valor { get; set; }
        public ClienteRequest Cliente { get; set; }
        public ContaRequest Conta { get; set; }
    }

    public class ClienteRequest
    {
        public string Nome { get; set; }
        public string Email { get; set; }
    }

    public class ContaRequest
    {
        public int Id { get; set; }
        public decimal Saldo { get; set; }
    }
}
using System;

namespace ContaFinanceira.Email.Domain.ViewModels
{
    public class EmailSettingsVM
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
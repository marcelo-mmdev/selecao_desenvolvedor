using System;
using System.Collections.Generic;
using System.Linq;

namespace Models
{
    // Representa uma conta bancária com saldo e histórico de transações
    public class Conta
    {
        public string IdConta { get; private set; } // ID único da conta
        public string NumeroAgencia { get; private set; }
        public string NumeroConta { get; private set; }
        public decimal SaldoAtual { get; private set; }
        public string MoedaPadrao { get; private set; }

        // Lista de transações realizadas nesta conta
        public List<Transacao> HistoricoDeTransacoes { get; private set; }

        public Conta(string idConta, string numeroAgencia, string numeroConta, string moedaPadrao)
        {
            IdConta = idConta;
            NumeroAgencia = numeroAgencia;
            NumeroConta = numeroConta;
            MoedaPadrao = moedaPadrao;
            SaldoAtual = 0;
            HistoricoDeTransacoes = new List<Transacao>();
        }

        // Adiciona valor à conta (crédito)
        public void Creditar(Transacao transacao)
        {
            SaldoAtual += transacao.Valor;
            HistoricoDeTransacoes.Add(transacao);
        }

        // Remove valor da conta (débito), se houver saldo
        public void Debitar(Transacao transacao)
        {
            if (SaldoAtual < transacao.Valor)
                throw new InvalidOperationException("Saldo insuficiente.");

            SaldoAtual -= transacao.Valor;
            HistoricoDeTransacoes.Add(transacao);
        }
    }
}
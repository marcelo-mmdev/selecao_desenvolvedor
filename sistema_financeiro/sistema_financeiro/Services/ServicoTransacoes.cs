using Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    // Serviço que centraliza todas as operações entre contas
    public class ServicoTransacoes
    {
        // Simula o banco de dados com um dicionário de contas
        private readonly Dictionary<string, Conta> _contas;

        public ServicoTransacoes()
        {
            _contas = new Dictionary<string, Conta>();
        }

        // Cria uma nova conta e a armazena no dicionário
        public Conta AbrirConta(string idConta, string agencia, string numero, string moedaPadrao)
        {
            if (_contas.ContainsKey(idConta))
                throw new InvalidOperationException("Conta já existente.");

            var novaConta = new Conta(idConta, agencia, numero, moedaPadrao);
            _contas.Add(idConta, novaConta);
            return novaConta;
        }

        // Retorna o saldo atual da conta
        public decimal ConsultarSaldo(string idConta)
        {
            var conta = ObterConta(idConta);
            return conta.SaldoAtual;
        }

        // Retorna o extrato filtrado por período
        public List<Transacao> ObterExtrato(string idConta, DateTime inicio, DateTime fim)
        {
            var conta = ObterConta(idConta);
            return conta.HistoricoDeTransacoes
                        .Where(t => t.DataHora >= inicio && t.DataHora <= fim)
                        .OrderBy(t => t.DataHora)
                        .ToList();
        }

        // Registra um crédito na conta
        public void RegistrarCredito(string idConta, decimal valor, string moeda, DateTime dataHora, string? descricao = null, string? categoria = null)
        {
            if (valor <= 0) throw new ArgumentException("Valor deve ser positivo.");
            var conta = ObterConta(idConta);

            var transacao = new Transacao(dataHora, TipoTransacao.Credito, valor, moeda, descricao, categoria);
            conta.Creditar(transacao);
        }

        // Registra um débito na conta (com validação de saldo)
        public void RegistrarDebito(string idConta, decimal valor, string moeda, DateTime dataHora, string? descricao = null, string? categoria = null)
        {
            if (valor <= 0) throw new ArgumentException("Valor deve ser positivo.");
            var conta = ObterConta(idConta);

            var transacao = new Transacao(dataHora, TipoTransacao.Debito, valor, moeda, descricao, categoria);
            conta.Debitar(transacao);
        }

        // Realiza uma transferência entre contas (débito + crédito)
        public void RealizarTransferencia(string idOrigem, string idDestino, decimal valor, string moeda, DateTime dataHora, string? descricao = null)
        {
            if (valor <= 0) throw new ArgumentException("Valor deve ser positivo.");
            var contaOrigem = ObterConta(idOrigem);
            var contaDestino = ObterConta(idDestino);

            var transacaoDebito = new Transacao(dataHora, TipoTransacao.TransferenciaEnviada, valor, moeda, descricao, "Transferência");
            var transacaoCredito = new Transacao(dataHora, TipoTransacao.TransferenciaRecebida, valor, moeda, descricao, "Transferência");

            contaOrigem.Debitar(transacaoDebito);    // pode lançar exceção se saldo for insuficiente
            contaDestino.Creditar(transacaoCredito);
        }

        // Busca a conta pelo ID ou lança erro
        private Conta ObterConta(string idConta)
        {
            if (!_contas.ContainsKey(idConta))
                throw new KeyNotFoundException("Conta não encontrada.");

            return _contas[idConta];
        }
    }
}
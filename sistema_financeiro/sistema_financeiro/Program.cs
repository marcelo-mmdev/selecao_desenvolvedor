using System;
using Services; // Referência ao nosso serviço de transações
using Models;
using System.Globalization;


class Program
{
    static void Main(string[] args)
    {
        // Cria uma instância do serviço que gerencia as contas e transações
        var servico = new ServicoTransacoes();

        // Criamos duas contas, uma para o Cliente A e outra para o Cliente B
        var contaA = servico.AbrirConta("A001", "001", "12345-6", "BRL");
        var contaB = servico.AbrirConta("B001", "002", "98765-4", "BRL");

        // Credita R$1000,00 na conta A (Depósito inicial)
        servico.RegistrarCredito(
            idConta: "A001",
            valor: 1000.00m,
            moeda: "BRL",
            dataHora: DateTime.Now,
            descricao: "Depósito Inicial",
            categoria: "Depósito"
        );

        // Debita R$50,00 da conta A (Pagamento de conta de luz)
        servico.RegistrarDebito(
            idConta: "A001",
            valor: 50.00m,
            moeda: "BRL",
            dataHora: DateTime.Now,
            descricao: "Pagamento de Conta de Luz",
            categoria: "Contas"
        );

        // Transfere R$200,00 da conta A para a conta B
        servico.RealizarTransferencia(
            idOrigem: "A001",
            idDestino: "B001",
            valor: 200.00m,
            moeda: "BRL",
            dataHora: DateTime.Now,
            descricao: "Presente"
        );

        // Tenta debitar R$1000,00 da conta A (deve falhar se não tiver saldo)
        try
        {
            servico.RegistrarDebito(
                idConta: "A001",
                valor: 1000.00m,
                moeda: "BRL",
                dataHora: DateTime.Now,
                descricao: "Tentativa de débito grande",
                categoria: "Teste"
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro esperado: {ex.Message}");
        }

        // Exibe o saldo atual da conta A
        var saldoA = servico.ConsultarSaldo("A001");
        Console.WriteLine($"\nSaldo da Conta A: R$ {saldoA.ToString("F2", CultureInfo.InvariantCulture)}");

        // Exibe o extrato completo da conta A no mês atual
        var hoje = DateTime.Now;
        var inicioDoMes = new DateTime(hoje.Year, hoje.Month, 1);
        var extratoA = servico.ObterExtrato("A001", inicioDoMes, hoje);

        Console.WriteLine("\nExtrato da Conta A:");
        foreach (var t in extratoA)
        {
            Console.WriteLine($"{t.DataHora:dd/MM/yyyy HH:mm} - {t.Tipo} - R$ {t.Valor.ToString("F2", CultureInfo.InvariantCulture)} - {t.Descricao}");
        }
    }
}
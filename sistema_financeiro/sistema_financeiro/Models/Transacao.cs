using System;

namespace Models
{
    // Representa uma transação financeira
    public class Transacao
    {
        public Guid Id { get; private set; } // Identificador único da transação
        public DateTime DataHora { get; private set; } // Quando ocorreu
        public TipoTransacao Tipo { get; private set; } // Tipo da transação
        public decimal Valor { get; private set; } // Valor envolvido
        public string Moeda { get; private set; } // Ex: BRL, USD
        public string? Descricao { get; private set; } // Opcional
        public string? Categoria { get; private set; } // Opcional

        // Construtor: ao criar, já define todos os campos e gera ID automaticamente
        public Transacao(DateTime dataHora, TipoTransacao tipo, decimal valor, string moeda, string? descricao = null, string? categoria = null)
        {
            Id = Guid.NewGuid(); // Gera um identificador único
            DataHora = dataHora;
            Tipo = tipo;
            Valor = valor;
            Moeda = moeda;
            Descricao = descricao;
            Categoria = categoria;
        }
    }
}

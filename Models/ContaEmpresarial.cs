using System;

namespace SistemaBancario.Models;

/// <summary>
/// Conta com limite de empréstimo disponível. Cada empréstimo concedido
/// soma ao saldo e consome parte do limite.
/// </summary>
public class ContaEmpresarial : ContaBancaria
{
    /// <summary>
    /// Limite de empréstimo restante. Diminui a cada empréstimo concedido.
    /// </summary>
    public double LimiteEmprestimo { get; private set; }

    /// <param name="limiteEmprestimo">Limite inicial de empréstimo (deve ser >= 0).</param>
    /// <exception cref="ArgumentException">Limite negativo.</exception>
    public ContaEmpresarial(int numeroConta, string titular, double saldoInicial, double limiteEmprestimo)
        : base(numeroConta, titular, saldoInicial)
    {
        if (limiteEmprestimo < 0)
            throw new ArgumentException("O limite de empréstimo não pode ser negativo.", nameof(limiteEmprestimo));

        LimiteEmprestimo = limiteEmprestimo;
    }

    /// <summary>
    /// Concede um empréstimo: credita o valor no saldo e consome o limite correspondente.
    /// </summary>
    /// <exception cref="ArgumentException">Valor menor ou igual a zero.</exception>
    /// <exception cref="InvalidOperationException">Valor excede o limite disponível.</exception>
    public void SolicitarEmprestimo(double valor)
    {
        if (valor <= 0)
            throw new ArgumentException("O valor do empréstimo deve ser maior que zero.", nameof(valor));

        if (valor > LimiteEmprestimo)
        {
            throw new InvalidOperationException(
                $"Valor solicitado ({valor:C2}) excede o limite disponível ({LimiteEmprestimo:C2}). " +
                "Solicite um valor menor ou igual ao limite restante.");
        }

        Saldo += valor;
        LimiteEmprestimo -= valor;
        RegistrarTransacao("Empréstimo", valor);
    }

    public override void ExibirResumo()
    {
        Console.WriteLine("  [Conta Empresarial]");
        base.ExibirResumo();
        Console.WriteLine($"  Limite de empréstimo disponível: {LimiteEmprestimo:C2}");
    }
}

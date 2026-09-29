using System;

namespace SistemaBancario.Models;

/// <summary>
/// Conta sem taxa de saque, com rendimento de 0,5% aplicável sobre o saldo.
/// </summary>
public class ContaPoupanca : ContaBancaria
{
    private const double TaxaRendimento = 0.005;

    public ContaPoupanca(int numeroConta, string titular, double saldoInicial)
        : base(numeroConta, titular, saldoInicial)
    {
    }

    /// <summary>
    /// Calcula e credita o rendimento sobre o saldo atual, arredondado em duas casas decimais.
    /// </summary>
    public void AplicarRendimento()
    {
        double rendimento = Math.Round(Saldo * TaxaRendimento, 2);
        Saldo += rendimento;
        RegistrarTransacao("Rendimento", rendimento);
    }

    public override void ExibirResumo()
    {
        Console.WriteLine("  [Conta Poupança]");
        base.ExibirResumo();
        Console.WriteLine($"  Taxa de rendimento: {TaxaRendimento:P1}");
    }
}

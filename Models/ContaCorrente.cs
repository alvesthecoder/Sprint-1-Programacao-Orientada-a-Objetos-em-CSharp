using System;
using SistemaBancario.Exceptions;

namespace SistemaBancario.Models;

/// <summary>
/// Conta que cobra taxa fixa de R$ 2,50 a cada saque.
/// A taxa é cobrada inclusive em transferências, pois estas usam <see cref="Sacar"/> internamente.
/// </summary>
public class ContaCorrente : ContaBancaria
{
    private const double TaxaSaque = 2.50;

    public ContaCorrente(int numeroConta, string titular, double saldoInicial)
        : base(numeroConta, titular, saldoInicial)
    {
    }

    /// <summary>
    /// Valida se o saldo cobre o valor solicitado mais a taxa fixa antes de debitar.
    /// </summary>
    /// <exception cref="ArgumentException">Valor menor ou igual a zero.</exception>
    /// <exception cref="SaldoInsuficienteException">Saldo insuficiente para valor + taxa.</exception>
    public override void Sacar(double valor)
    {
        if (valor <= 0)
            throw new ArgumentException("O valor do saque deve ser maior que zero.", nameof(valor));

        double valorTotal = valor + TaxaSaque;

        if (valorTotal > Saldo)
        {
            throw new SaldoInsuficienteException(
                $"Saldo insuficiente. Saldo atual: {Saldo:C2}. " +
                $"Valor do saque ({valor:C2}) + taxa ({TaxaSaque:C2}) = {valorTotal:C2}.");
        }

        Saldo -= valorTotal;
        RegistrarTransacao("Saque", valor);
        RegistrarTransacao("Tarifa de saque", TaxaSaque);
    }

    public override void ExibirResumo()
    {
        Console.WriteLine("  [Conta Corrente]");
        base.ExibirResumo();
        Console.WriteLine($"  Taxa por saque: {TaxaSaque:C2}");
    }
}

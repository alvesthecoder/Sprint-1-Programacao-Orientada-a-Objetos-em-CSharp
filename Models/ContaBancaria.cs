using System;
using System.Collections.Generic;
using SistemaBancario.Interfaces;
using SistemaBancario.Exceptions;

namespace SistemaBancario.Models;

/// <summary>
/// Classe base para todos os tipos de conta bancária.
/// </summary>
/// <remarks>
/// Valores monetários usam <c>double</c> conforme requisito do exercício.
/// Em sistemas reais, usar <c>decimal</c> para evitar imprecisão de ponto flutuante.
/// </remarks>
public abstract class ContaBancaria : ITransacao
{
    private readonly List<Transacao> _historico;

    public int NumeroConta { get; }
    public string Titular { get; }
    public double Saldo { get; protected set; }

    /// <param name="numeroConta">Deve ser maior que zero.</param>
    /// <param name="titular">Não pode ser vazio ou nulo.</param>
    /// <param name="saldoInicial">Deve ser maior ou igual a zero.</param>
    /// <exception cref="ArgumentException">Parâmetro fora das regras de validação.</exception>
    protected ContaBancaria(int numeroConta, string titular, double saldoInicial)
    {
        if (numeroConta <= 0)
            throw new ArgumentException("O número da conta deve ser maior que zero.", nameof(numeroConta));

        if (string.IsNullOrWhiteSpace(titular))
            throw new ArgumentException("O nome do titular não pode ser vazio.", nameof(titular));

        if (saldoInicial < 0)
            throw new ArgumentException("O saldo inicial não pode ser negativo.", nameof(saldoInicial));

        NumeroConta = numeroConta;
        Titular = titular;
        Saldo = saldoInicial;
        _historico = new List<Transacao>();
    }

    /// <summary>
    /// Retorna o histórico de transações como coleção somente leitura.
    /// </summary>
    public IReadOnlyList<Transacao> ObterHistorico()
    {
        return _historico.AsReadOnly();
    }

    /// <inheritdoc />
    public void RegistrarTransacao(string tipo, double valor)
    {
        _historico.Add(new Transacao(tipo, valor));
    }

    /// <summary>
    /// Deposita um valor na conta.
    /// </summary>
    /// <exception cref="ArgumentException">Valor menor ou igual a zero.</exception>
    public void Depositar(double valor)
    {
        if (valor <= 0)
            throw new ArgumentException("O valor do depósito deve ser maior que zero.", nameof(valor));

        Saldo += valor;
        RegistrarTransacao("Depósito", valor);
    }

    /// <summary>
    /// Saca um valor da conta. Subclasses podem sobrescrever para aplicar regras adicionais
    /// (ex.: cobrança de taxa).
    /// </summary>
    /// <exception cref="ArgumentException">Valor menor ou igual a zero.</exception>
    /// <exception cref="SaldoInsuficienteException">Saldo insuficiente para a operação.</exception>
    public virtual void Sacar(double valor)
    {
        if (valor <= 0)
            throw new ArgumentException("O valor do saque deve ser maior que zero.", nameof(valor));

        if (valor > Saldo)
        {
            throw new SaldoInsuficienteException(
                $"Saldo insuficiente. Saldo atual: {Saldo:C2}. Valor solicitado: {valor:C2}.");
        }

        Saldo -= valor;
        RegistrarTransacao("Saque", valor);
    }

    /// <summary>
    /// Transfere um valor desta conta para a conta de destino.
    /// Usa <see cref="Sacar"/> internamente, então regras específicas de cada tipo
    /// (como taxa da Conta Corrente) são aplicadas automaticamente na origem.
    /// </summary>
    /// <exception cref="ArgumentException">Destino nulo, igual à origem ou valor inválido.</exception>
    /// <exception cref="SaldoInsuficienteException">Saldo insuficiente na conta de origem.</exception>
    public void Transferir(ContaBancaria destino, double valor)
    {
        if (destino == null)
            throw new ArgumentException("A conta de destino não pode ser nula.", nameof(destino));

        if (destino.NumeroConta == this.NumeroConta)
            throw new ArgumentException("Não é possível transferir para a mesma conta.", nameof(destino));

        if (valor <= 0)
            throw new ArgumentException("O valor da transferência deve ser maior que zero.", nameof(valor));

        int registrosAntes = _historico.Count;

        // Se o saque falhar, a exceção é propagada antes do depósito no destino.
        Sacar(valor);

        // Substitui os registros do Sacar por "Transferência enviada" para
        // que o histórico reflita a natureza da operação sem entradas duplicadas.
        int registrosAdicionados = _historico.Count - registrosAntes;
        _historico.RemoveRange(registrosAntes, registrosAdicionados);
        RegistrarTransacao("Transferência enviada", valor);

        int registrosDestinoAntes = destino._historico.Count;
        destino.Depositar(valor);
        int registrosDestinoAdicionados = destino._historico.Count - registrosDestinoAntes;
        destino._historico.RemoveRange(registrosDestinoAntes, registrosDestinoAdicionados);
        destino.RegistrarTransacao("Transferência recebida", valor);
    }

    /// <summary>
    /// Exibe os dados da conta no console. Sobrescrito pelas subclasses para
    /// incluir informações específicas de cada tipo.
    /// </summary>
    public virtual void ExibirResumo()
    {
        Console.WriteLine($"  Conta: {NumeroConta}");
        Console.WriteLine($"  Titular: {Titular}");
        Console.WriteLine($"  Saldo: {Saldo:C2}");
    }
}

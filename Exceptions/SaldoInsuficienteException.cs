using System;

namespace SistemaBancario.Exceptions;

/// <summary>
/// Lançada quando o saldo da conta é insuficiente para concluir a operação.
/// </summary>
public class SaldoInsuficienteException : Exception
{
    public SaldoInsuficienteException()
        : base("Saldo insuficiente para realizar esta operação.")
    {
    }

    public SaldoInsuficienteException(string mensagem)
        : base(mensagem)
    {
    }
}

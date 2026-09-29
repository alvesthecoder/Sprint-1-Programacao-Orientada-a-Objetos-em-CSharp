using System;

namespace SistemaBancario.Models;

/// <summary>
/// Registro imutável de uma operação financeira realizada em uma conta.
/// </summary>
public class Transacao
{
    public string Tipo { get; }
    public double Valor { get; }
    public DateTime Data { get; }

    public Transacao(string tipo, double valor)
    {
        Tipo = tipo;
        Valor = valor;
        Data = DateTime.Now;
    }
}

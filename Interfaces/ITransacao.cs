namespace SistemaBancario.Interfaces;

/// <summary>
/// Contrato para classes que mantêm histórico de operações financeiras.
/// </summary>
public interface ITransacao
{
    /// <summary>
    /// Registra uma transação no histórico.
    /// </summary>
    /// <param name="tipo">Descrição da operação (ex.: "Depósito", "Saque").</param>
    /// <param name="valor">Valor envolvido.</param>
    void RegistrarTransacao(string tipo, double valor);
}

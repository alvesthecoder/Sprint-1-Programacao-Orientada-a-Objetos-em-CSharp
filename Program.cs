using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SistemaBancario.Models;
using SistemaBancario.Interfaces;
using SistemaBancario.Exceptions;

namespace SistemaBancario;

/// <summary>
/// Ponto de entrada do sistema. Gerencia menu, leitura de dados e exibição.
/// Toda regra de negócio fica nas classes de modelo.
/// </summary>
class Program
{
    private static readonly CultureInfo _culturaPtBr = new CultureInfo("pt-BR");

    static void Main(string[] args)
    {
        CultureInfo.CurrentCulture = _culturaPtBr;
        CultureInfo.CurrentUICulture = _culturaPtBr;

        List<ContaBancaria> contas = new List<ContaBancaria>();
        bool executando = true;

        while (executando)
        {
            ExibirMenu();
            int opcao = LerInteiro("Escolha uma opção: ");

            switch (opcao)
            {
                case 1:
                    CriarConta(contas);
                    break;
                case 2:
                    RealizarDeposito(contas);
                    break;
                case 3:
                    RealizarSaque(contas);
                    break;
                case 4:
                    RealizarTransferencia(contas);
                    break;
                case 5:
                    AplicarRendimento(contas);
                    break;
                case 6:
                    SolicitarEmprestimo(contas);
                    break;
                case 7:
                    ListarContas(contas);
                    break;
                case 8:
                    executando = false;
                    Console.WriteLine("\nObrigado por utilizar o Sistema Bancário Mares. Até logo!");
                    break;
                default:
                    Console.WriteLine("\nOpção inválida. Por favor, escolha de 1 a 8.");
                    break;
            }

            if (executando)
            {
                Console.WriteLine("\nPressione qualquer tecla para continuar...");
                Console.ReadKey(true);
                Console.Clear();
            }
        }
    }

    /// <summary>
    /// Lê um inteiro do console, repetindo até receber entrada válida.
    /// </summary>
    private static int LerInteiro(string mensagem)
    {
        while (true)
        {
            Console.Write(mensagem);
            string? entrada = Console.ReadLine();

            if (int.TryParse(entrada, NumberStyles.Integer, _culturaPtBr, out int resultado))
            {
                return resultado;
            }

            Console.WriteLine("Entrada inválida. Digite um número inteiro.");
        }
    }

    /// <summary>
    /// Lê um double positivo (> 0) do console. Aceita vírgula decimal (pt-BR).
    /// </summary>
    private static double LerDoublePositivo(string mensagem)
    {
        while (true)
        {
            Console.Write(mensagem);
            string? entrada = Console.ReadLine();

            if (double.TryParse(entrada, NumberStyles.Number, _culturaPtBr, out double resultado)
                && resultado > 0)
            {
                return resultado;
            }

            Console.WriteLine("Entrada inválida. Digite um valor numérico positivo (use vírgula para decimais).");
        }
    }

    /// <summary>
    /// Lê um double não negativo (>= 0) do console. Usado para saldo inicial e limite.
    /// </summary>
    private static double LerDoubleNaoNegativo(string mensagem)
    {
        while (true)
        {
            Console.Write(mensagem);
            string? entrada = Console.ReadLine();

            if (double.TryParse(entrada, NumberStyles.Number, _culturaPtBr, out double resultado)
                && resultado >= 0)
            {
                return resultado;
            }

            Console.WriteLine("Entrada inválida. Digite um valor numérico não negativo (use vírgula para decimais).");
        }
    }

    private static string LerTextoNaoVazio(string mensagem)
    {
        while (true)
        {
            Console.Write(mensagem);
            string? entrada = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(entrada))
            {
                return entrada.Trim();
            }

            Console.WriteLine("Entrada inválida. O texto não pode ser vazio.");
        }
    }

    private static ContaBancaria? BuscarConta(List<ContaBancaria> contas, int numeroConta)
    {
        return contas.FirstOrDefault(c => c.NumeroConta == numeroConta);
    }

    private static void ExibirMenu()
    {
        Console.WriteLine();
        Console.WriteLine("=========================================================");
        Console.WriteLine("                SISTEMA BANCARIO - MARES");
        Console.WriteLine("   O sistema bancário nº 1 da Baía de Todos-os-Santos");
        Console.WriteLine("=========================================================");
        Console.WriteLine("  1. Criar Nova Conta");
        Console.WriteLine("  2. Depositar");
        Console.WriteLine("  3. Sacar");
        Console.WriteLine("  4. Transferir");
        Console.WriteLine("  5. Aplicar Rendimento (Poupanca)");
        Console.WriteLine("  6. Solicitar Emprestimo (Empresarial)");
        Console.WriteLine("  7. Listar Todas as Contas");
        Console.WriteLine("  8. Sair");
        Console.WriteLine("================================================");
    }

    private static void CriarConta(List<ContaBancaria> contas)
    {
        try
        {
            Console.WriteLine("\n--- Criar Nova Conta ---");
            Console.WriteLine("Tipo de conta:");
            Console.WriteLine("  1. Conta Corrente");
            Console.WriteLine("  2. Conta Poupanca");
            Console.WriteLine("  3. Conta Empresarial");

            int tipo = LerInteiro("Escolha o tipo (1-3): ");

            if (tipo < 1 || tipo > 3)
            {
                Console.WriteLine("Tipo de conta invalido. Operacao cancelada.");
                return;
            }

            int numeroConta = LerInteiro("Numero da conta: ");

            if (BuscarConta(contas, numeroConta) != null)
            {
                Console.WriteLine($"Ja existe uma conta com o numero {numeroConta}. Operacao cancelada.");
                return;
            }

            string titular = LerTextoNaoVazio("Nome do titular: ");
            double saldoInicial = LerDoubleNaoNegativo("Saldo inicial (R$): ");

            ContaBancaria novaConta;

            switch (tipo)
            {
                case 1:
                    novaConta = new ContaCorrente(numeroConta, titular, saldoInicial);
                    break;
                case 2:
                    novaConta = new ContaPoupanca(numeroConta, titular, saldoInicial);
                    break;
                case 3:
                    double limiteEmprestimo = LerDoubleNaoNegativo("Limite de emprestimo (R$): ");
                    novaConta = new ContaEmpresarial(numeroConta, titular, saldoInicial, limiteEmprestimo);
                    break;
                default:
                    Console.WriteLine("Tipo de conta invalido.");
                    return;
            }

            contas.Add(novaConta);
            Console.WriteLine($"\nConta criada com sucesso! Numero: {numeroConta}, Titular: {titular}.");
        }
        catch (SaldoInsuficienteException ex)
        {
            Console.WriteLine($"\nOperacao negada: {ex.Message}");
            Console.WriteLine("Dica: verifique o saldo disponivel antes de realizar a operacao.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"\nErro de validacao: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"\nOperacao invalida: {ex.Message}");
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"\nErro de formato: {ex.Message}");
        }
        catch (OverflowException ex)
        {
            Console.WriteLine($"\nErro de estouro numerico: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nErro inesperado: {ex.Message}");
        }
    }

    private static void RealizarDeposito(List<ContaBancaria> contas)
    {
        try
        {
            Console.WriteLine("\n--- Deposito ---");
            int numeroConta = LerInteiro("Numero da conta: ");
            ContaBancaria? conta = BuscarConta(contas, numeroConta);

            if (conta == null)
            {
                Console.WriteLine($"Conta {numeroConta} nao encontrada.");
                return;
            }

            double valor = LerDoublePositivo("Valor do deposito (R$): ");
            conta.Depositar(valor);

            Console.WriteLine($"\nDeposito de {valor:C2} realizado com sucesso na conta {numeroConta}.");
            Console.WriteLine($"Novo saldo: {conta.Saldo:C2}.");
        }
        catch (SaldoInsuficienteException ex)
        {
            Console.WriteLine($"\nOperacao negada: {ex.Message}");
            Console.WriteLine("Dica: verifique o saldo disponivel.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"\nErro de validacao: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"\nOperacao invalida: {ex.Message}");
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"\nErro de formato: {ex.Message}");
        }
        catch (OverflowException ex)
        {
            Console.WriteLine($"\nErro de estouro numerico: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nErro inesperado: {ex.Message}");
        }
    }

    private static void RealizarSaque(List<ContaBancaria> contas)
    {
        try
        {
            Console.WriteLine("\n--- Saque ---");
            int numeroConta = LerInteiro("Numero da conta: ");
            ContaBancaria? conta = BuscarConta(contas, numeroConta);

            if (conta == null)
            {
                Console.WriteLine($"Conta {numeroConta} nao encontrada.");
                return;
            }

            double valor = LerDoublePositivo("Valor do saque (R$): ");
            conta.Sacar(valor);

            Console.WriteLine($"\nSaque de {valor:C2} realizado com sucesso na conta {numeroConta}.");
            Console.WriteLine($"Novo saldo: {conta.Saldo:C2}.");
        }
        catch (SaldoInsuficienteException ex)
        {
            Console.WriteLine($"\nOperacao negada: {ex.Message}");
            Console.WriteLine("Dica: verifique o saldo disponivel. Contas Correntes cobram taxa de R$ 2,50 por saque.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"\nErro de validacao: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"\nOperacao invalida: {ex.Message}");
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"\nErro de formato: {ex.Message}");
        }
        catch (OverflowException ex)
        {
            Console.WriteLine($"\nErro de estouro numerico: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nErro inesperado: {ex.Message}");
        }
    }

    private static void RealizarTransferencia(List<ContaBancaria> contas)
    {
        try
        {
            Console.WriteLine("\n--- Transferencia ---");
            int numeroOrigem = LerInteiro("Numero da conta de origem: ");
            ContaBancaria? origem = BuscarConta(contas, numeroOrigem);

            if (origem == null)
            {
                Console.WriteLine($"Conta de origem {numeroOrigem} nao encontrada.");
                return;
            }

            int numeroDestino = LerInteiro("Numero da conta de destino: ");
            ContaBancaria? destino = BuscarConta(contas, numeroDestino);

            if (destino == null)
            {
                Console.WriteLine($"Conta de destino {numeroDestino} nao encontrada.");
                return;
            }

            double valor = LerDoublePositivo("Valor da transferencia (R$): ");
            origem.Transferir(destino, valor);

            Console.WriteLine($"\nTransferencia de {valor:C2} realizada com sucesso.");
            Console.WriteLine($"  Origem  (conta {numeroOrigem}): saldo {origem.Saldo:C2}");
            Console.WriteLine($"  Destino (conta {numeroDestino}): saldo {destino.Saldo:C2}");
        }
        catch (SaldoInsuficienteException ex)
        {
            Console.WriteLine($"\nOperacao negada: {ex.Message}");
            Console.WriteLine("Dica: verifique o saldo da conta de origem. Contas Correntes descontam taxa no saque.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"\nErro de validacao: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"\nOperacao invalida: {ex.Message}");
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"\nErro de formato: {ex.Message}");
        }
        catch (OverflowException ex)
        {
            Console.WriteLine($"\nErro de estouro numerico: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nErro inesperado: {ex.Message}");
        }
    }

    private static void AplicarRendimento(List<ContaBancaria> contas)
    {
        try
        {
            Console.WriteLine("\n--- Aplicar Rendimento ---");
            int numeroConta = LerInteiro("Numero da conta: ");
            ContaBancaria? conta = BuscarConta(contas, numeroConta);

            if (conta == null)
            {
                Console.WriteLine($"Conta {numeroConta} nao encontrada.");
                return;
            }

            if (conta is ContaPoupanca poupanca)
            {
                poupanca.AplicarRendimento();
                Console.WriteLine($"\nRendimento aplicado com sucesso na conta {numeroConta}.");
                Console.WriteLine($"Novo saldo: {poupanca.Saldo:C2}.");
            }
            else
            {
                Console.WriteLine("Esta operacao esta disponivel apenas para Contas Poupanca.");
            }
        }
        catch (SaldoInsuficienteException ex)
        {
            Console.WriteLine($"\nOperacao negada: {ex.Message}");
            Console.WriteLine("Dica: verifique o saldo disponivel.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"\nErro de validacao: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"\nOperacao invalida: {ex.Message}");
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"\nErro de formato: {ex.Message}");
        }
        catch (OverflowException ex)
        {
            Console.WriteLine($"\nErro de estouro numerico: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nErro inesperado: {ex.Message}");
        }
    }

    private static void SolicitarEmprestimo(List<ContaBancaria> contas)
    {
        try
        {
            Console.WriteLine("\n--- Solicitar Emprestimo ---");
            int numeroConta = LerInteiro("Numero da conta: ");
            ContaBancaria? conta = BuscarConta(contas, numeroConta);

            if (conta == null)
            {
                Console.WriteLine($"Conta {numeroConta} nao encontrada.");
                return;
            }

            if (conta is ContaEmpresarial empresarial)
            {
                Console.WriteLine($"Limite disponivel: {empresarial.LimiteEmprestimo:C2}.");
                double valor = LerDoublePositivo("Valor do emprestimo (R$): ");
                empresarial.SolicitarEmprestimo(valor);

                Console.WriteLine($"\nEmprestimo de {valor:C2} concedido com sucesso.");
                Console.WriteLine($"Novo saldo: {empresarial.Saldo:C2}.");
                Console.WriteLine($"Limite disponivel restante: {empresarial.LimiteEmprestimo:C2}.");
            }
            else
            {
                Console.WriteLine("Esta operacao esta disponivel apenas para Contas Empresariais.");
            }
        }
        catch (SaldoInsuficienteException ex)
        {
            Console.WriteLine($"\nOperacao negada: {ex.Message}");
            Console.WriteLine("Dica: verifique o saldo disponivel.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"\nErro de validacao: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"\nOperacao invalida: {ex.Message}");
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"\nErro de formato: {ex.Message}");
        }
        catch (OverflowException ex)
        {
            Console.WriteLine($"\nErro de estouro numerico: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nErro inesperado: {ex.Message}");
        }
    }

    private static void ListarContas(List<ContaBancaria> contas)
    {
        Console.WriteLine("\n--- Listar Todas as Contas ---");

        if (contas.Count == 0)
        {
            Console.WriteLine("Nenhuma conta cadastrada.");
            return;
        }

        foreach (ContaBancaria conta in contas)
        {
            Console.WriteLine("----------------------------------------");
            conta.ExibirResumo();

            IReadOnlyList<Transacao> historico = conta.ObterHistorico();

            if (historico.Count > 0)
            {
                Console.WriteLine("  Historico de transacoes:");
                foreach (Transacao transacao in historico)
                {
                    Console.WriteLine(
                        $"    {transacao.Data:dd/MM/yyyy HH:mm} | {transacao.Tipo}: {transacao.Valor:C2}");
                }
            }
            else
            {
                Console.WriteLine("  Historico: nenhuma transacao registrada.");
            }
        }

        Console.WriteLine("----------------------------------------");
        Console.WriteLine($"Total de contas: {contas.Count}");
    }
}

using System;
using Xunit;
using SistemaBancario.Models;
using SistemaBancario.Exceptions;

namespace SistemaBancario.Tests
{
    public class TransferenciaIntegrationTests
    {
        [Fact]
        public void Transferir_DeContaCorrenteParaPoupanca_AplicaTaxaEAtualizaAmbos()
        {
            // Arrange
            var origem = new ContaCorrente(1, "Origem", 200);
            var destino = new ContaPoupanca(2, "Destino", 100);
            
            // Act
            origem.Transferir(destino, 100); 
            
            // Assert
            Assert.Equal(97.50, origem.Saldo);
            Assert.Equal(200, destino.Saldo);
        }

        [Fact]
        public void Transferir_DeContaEmpresarialAposEmprestimo_FluxoCompleto()
        {
            // Arrange
            var origem = new ContaEmpresarial(1, "Empresa", 0, 1000);
            var destino = new ContaCorrente(2, "Fornecedor", 0);
            
            // Act
            origem.SolicitarEmprestimo(500); 
            origem.Transferir(destino, 500); 
            destino.Sacar(100); 
            
            // Assert
            Assert.Equal(0, origem.Saldo);
            Assert.Equal(397.50, destino.Saldo);
        }
    }
}

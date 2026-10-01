using System;
using Xunit;
using SistemaBancario.Models;
using SistemaBancario.Exceptions;

namespace SistemaBancario.Tests
{
    public class ContaCorrenteTests
    {
        [Fact]
        public void Sacar_ComSaldoSuficiente_DeduzValorETaxa()
        {
            // Arrange
            var conta = new ContaCorrente(1, "Test", 100);
            
            // Act
            conta.Sacar(50);
            
            // Assert
            Assert.Equal(47.50, conta.Saldo);
        }

        [Fact]
        public void Sacar_SemSaldoParaTaxa_LancaExcecao()
        {
            // Arrange
            var conta = new ContaCorrente(1, "Test", 50);
            
            // Act & Assert
            Assert.Throws<SaldoInsuficienteException>(() => conta.Sacar(50));
        }
    }
}

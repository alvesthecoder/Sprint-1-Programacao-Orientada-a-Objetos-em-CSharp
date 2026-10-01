using System;
using Xunit;
using SistemaBancario.Models;

namespace SistemaBancario.Tests
{
    public class ContaPoupancaTests
    {
        [Fact]
        public void AplicarRendimento_AumentaSaldoCorretamente()
        {
            // Arrange
            var conta = new ContaPoupanca(1, "Test", 1000);
            
            // Act
            conta.AplicarRendimento();
            
            // Assert
            Assert.Equal(1005, conta.Saldo);
        }
    }
}

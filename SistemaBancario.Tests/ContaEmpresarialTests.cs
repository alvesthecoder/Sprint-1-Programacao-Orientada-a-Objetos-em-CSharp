using System;
using Xunit;
using SistemaBancario.Models;

namespace SistemaBancario.Tests
{
    public class ContaEmpresarialTests
    {
        [Fact]
        public void SolicitarEmprestimo_DentroDoLimite_AumentaSaldoEReduzLimite()
        {
            // Arrange
            var conta = new ContaEmpresarial(1, "Test", 100, 500);
            
            // Act
            conta.SolicitarEmprestimo(200);
            
            // Assert
            Assert.Equal(300, conta.Saldo);
            Assert.Equal(300, conta.LimiteEmprestimo);
        }

        [Fact]
        public void SolicitarEmprestimo_AcimaDoLimite_LancaExcecao()
        {
            // Arrange
            var conta = new ContaEmpresarial(1, "Test", 100, 500);
            
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => conta.SolicitarEmprestimo(600));
        }
    }
}

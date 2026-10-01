# Sprint 1 Programação Orientada a Objetos em C#
### Sistema Bancário Mares em Console (.NET 8)
> O sistema bancário nº 1 da Baía de Todos-os-Santos

Sistema bancário interativo desenvolvido em C# com foco em boas práticas de arquitetura e aplicação da Programação Orientada a Objetos (POO) e princípios de Clean Code.

---

## Arquitetura e Pilares de POO

O projeto foi desenhado para demonstrar os fundamentos da orientação a objetos:

1. **Abstração**: A classe abstrata `ContaBancaria` define o modelo base para as contas, encapsulando saldo, número, titular e histórico, padronizando métodos como `Depositar`, `Sacar` e `Transferir`.
2. **Encapsulamento**:
   - Saldo protegido com acesso restrito a classes derivadas e leitura via propriedade pública.
   - Histórico encapsulado expondo apenas uma visualização somente leitura.
   - Atributos imutáveis (`NumeroConta`, `Titular`) com inicialização no construtor.
3. **Herança**: Especialização da classe base em três modalidades:
   - `ContaCorrente`
   - `ContaPoupanca`
   - `ContaEmpresarial`
4. **Polimorfismo**:
   - Métodos sobrescritos para personalizar regras de negócio.
   - Exibição polimórfica de extratos.
5. **Interfaces**:
   - `ITransacao`: Contrato para auditoria e histórico de operações.
6. **Tratamento de Exceções**:
   - Exceções customizadas e validações de entrada estritas.

---

## Tipos de Contas e Regras de Negócio

| Tipo de Conta | Taxa de Saque | Rendimento | Limite de Empréstimo |
|---|---|---|---|
| **Conta Corrente** | R$ 2,50 | — | — |
| **Conta Poupança** | Gratuito | 0,5% sobre saldo | — |
| **Conta Empresarial**| Gratuito | — | Definido na abertura |

---

## Estrutura do Projeto

- **Exceptions/**: Exceções de regras de negócio
- **Interfaces/**: Contratos para registro de transações
- **Models/**: Classes base e especializações de contas
- **Program.cs**: Menu interativo
- **SistemaBancario.csproj**: Configuração .NET

---

## Como Executar

### Pré-requisitos
- .NET SDK 8.0 ou superior

### Passos
1. Entre na pasta do projeto:
   ```bash
   cd "Sprint-1-Programacao-Orientada-a-Objetos-em-CSharp/SistemaBancario"
   ```
2. Restaure e compile:
   ```bash
   dotnet build
   ```
3. Execute:
   ```bash
   dotnet run
   ```

---

## Funcionalidades

- Criar Conta
- Consultar Saldo
- Realizar Depósito
- Realizar Saque
- Transferência entre Contas
- Aplicar Rendimento (Poupança)
- Solicitar Empréstimo (Empresarial)
- Extrato Detalhado
- Listar Contas

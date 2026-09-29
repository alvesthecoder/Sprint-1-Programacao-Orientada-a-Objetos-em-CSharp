# Sprint 1 Programação Orientada a Objetos em C#
### Desafio Back-End SENAI — Sistema Bancário em Console (.NET 8)

Sistema bancário interativo desenvolvido em C# com foco em boas práticas de arquitetura e aplicação rigorosa dos pilares da **Programação Orientada a Objetos (POO)** e princípios de **Clean Code**.

---

## 🏛️ Arquitetura e Pilares de POO

O projeto foi desenhado para demonstrar com clareza os fundamentos essenciais da orientação a objetos:

1. **Abstração**: A classe abstrata [`ContaBancaria`](Models/ContaBancaria.cs) define o modelo base para qualquer conta financeira, encapsulando saldo, número da conta, titular e histórico, além de padronizar métodos essenciais como `Depositar`, `Sacar` e `Transferir`.
2. **Encapsulamento**:
   - Saldo protegido (`protected double Saldo`) com acesso restrito a classes derivadas e leitura pública via propriedade (`public double Saldo`).
   - Coleção do histórico encapsulada (`private readonly List<Transacao> _historico`), expondo apenas uma visualização somente leitura (`IReadOnlyList<Transacao>`).
   - Atributos imutáveis como `NumeroConta` e `Titular` com modificador `{ get; }` público e inicialização restrita ao construtor.
3. **Herança**: Especialização da classe base em três modalidades reais de conta:
   - [`ContaCorrente`](Models/ContaCorrente.cs)
   - [`ContaPoupanca`](Models/ContaPoupanca.cs)
   - [`ContaEmpresarial`](Models/ContaEmpresarial.cs)
4. **Polimorfismo**:
   - Métodos virtuais/abstratos sobrescritos (`override`) para personalizar regras de negócio (ex.: dedução de taxa de saque na `ContaCorrente`).
   - Exibição polimórfica de extratos e listagem dinâmica de contas através da referência base `ContaBancaria`.
5. **Interfaces**:
   - [`ITransacao`](Interfaces/ITransacao.cs): Define o contrato desacoplado para auditoria e histórico de operações financeiras.
6. **Tratamento de Exceções Robustecido**:
   - Exceção customizada [`SaldoInsuficienteException`](Exceptions/SaldoInsuficienteException.cs).
   - Validações de entrada estritas com `ArgumentException`, `InvalidOperationException` e captura hierárquica em múltiplos blocos `try-catch`.

---

## 💼 Tipos de Contas e Regras de Negócio

| Tipo de Conta | Taxa de Saque | Rendimento | Limite de Empréstimo | Particularidades |
|---|---|---|---|---|
| **Conta Corrente** | R$ 2,50 por saque | — | — | A taxa fixa incide sobre qualquer saque ou transferência originada desta conta. |
| **Conta Poupança** | Gratuito | 0,5% sobre o saldo | — | Método `Render()` que calcula e credita rendimento mensal automaticamente. |
| **Conta Empresarial** | Gratuito | — | Definido na abertura | Permite operações de empréstimo (`RealizarEmprestimo`) até o limite disponível, atualizando o saldo imediatamente. |

---

## 📂 Estrutura do Projeto

```text
SistemaBancario/
├── Exceptions/
│   └── SaldoInsuficienteException.cs   # Exceção customizada de regra de negócio
├── Interfaces/
│   └── ITransacao.cs                   # Contrato para registro de auditoria e transações
├── Models/
│   ├── ContaBancaria.cs                # Classe base abstrata
│   ├── ContaCorrente.cs                # Especialização com taxa de operação
│   ├── ContaPoupanca.cs                # Especialização com rendimento
│   ├── ContaEmpresarial.cs             # Especialização com limite de empréstimo
│   └── Transacao.cs                    # Registro imutável de movimentação
├── Program.cs                          # Menu interativo em console com fluxo completo
├── SistemaBancario.csproj              # Arquivo de configuração .NET
└── README.md                           # Documentação técnica do projeto
```

---

## 🚀 Como Executar o Projeto

### Pré-requisitos
- [.NET SDK 8.0](https://dotnet.microsoft.com/download) ou superior instalado.

### Passo a Passo

1. **Clone o repositório:**
   ```bash
   git clone git@github.com:alvesthecoder/Sprint-1-Programacao-Orientada-a-Objetos-em-CSharp.git
   cd "Sprint-1-Programacao-Orientada-a-Objetos-em-CSharp/SistemaBancario"
   ```

2. **Restaure e compile a aplicação:**
   ```bash
   dotnet build
   ```

3. **Execute a aplicação:**
   ```bash
   dotnet run
   ```

---

## 📋 Funcionalidades do Sistema

- **Criar Conta**: Criação guiada de Conta Corrente, Poupança ou Empresarial com validação de dados de entrada.
- **Consultar Saldo**: Consulta rápida e formatada do saldo atual em moeda brasileira (`pt-BR`).
- **Realizar Depósito**: Crédito imediato com validação de valores positivos.
- **Realizar Saque**: Débito respeitando regras específicas e taxas de cada tipo de conta.
- **Transferência entre Contas**: Movimentação atômica entre duas contas cadastradas.
- **Aplicar Rendimento**: Exclusivo para Conta Poupança.
- **Solicitar Empréstimo**: Exclusivo para Conta Empresarial com controle de limite.
- **Extrato Detalhado**: Histórico cronológico completo de todas as operações realizadas na conta.
- **Listar Todas as Contas**: Visão geral de todas as contas ativas cadastradas no sistema bancário.

---

## 👨‍💻 Autor

Desenvolvido por **Renato Alves** ([@alvesthecoder](https://github.com/alvesthecoder)) como parte do Desafio da Sprint 1 de Programação Orientada a Objetos em C# — SENAI.

# Exportador CSV/ZIP

Aplicativo desktop WPF (.NET 8) para exportação de dados de banco SQL Server em arquivos CSV compactados em ZIP, seguindo princípios sólidos de arquitetura e design.

---

## Descrição

Este projeto implementa uma solução robusta e escalável para exportação de dados selecionados de um banco de dados SQL Server. Ele:

- Conecta dinamicamente ao banco com parâmetros fornecidos pelo usuário.
- Gera views no banco para facilitar a extração estruturada dos dados.
- Exporta os dados das views para arquivos CSV.
- Compacta os CSVs gerados em um arquivo ZIP.
- Salva o arquivo ZIP na pasta definida pelo usuário.

---

## Arquitetura e Princípios

- **Clean Architecture:** Separação clara entre camadas de apresentação, aplicação, infraestrutura e domínio.
- **SOLID:** Código organizado para facilitar manutenção, extensão e testes:
  - SRP: Cada classe e serviço com responsabilidade única.
  - OCP: Sistema aberto para extensões via abstrações e injeção de dependência.
  - LSP: Implementações substituíveis por abstrações.
  - ISP: Interfaces específicas e focadas.
  - DIP: Dependências invertidas para baixo acoplamento.

---

## Estrutura do Projeto

- **Exportador.UI:** Interface WPF desacoplada via MVVM, responsável pela interação com o usuário.
- **Exportador.Application:** Casos de uso, regras de negócio e orquestração das operações.
- **Exportador.Infrastructure:** Implementação dos acessos a dados, manipulação de arquivos, logging e integração com banco.
- **Exportador.Core:** Entidades de domínio, interfaces e modelos compartilhados entre as camadas.

---

## Tecnologias Utilizadas

| Categoria               | Tecnologias / Frameworks                         |
|------------------------|------------------------------------------------|
| Plataforma & UI        | WPF (.NET 8), Windows Forms (suporte auxiliar) |
| Padrão de Projeto      | MVVM (CommunityToolkit.Mvvm)                     |
| Injeção de Dependência | Microsoft.Extensions.DependencyInjection        |
| Logging                | Serilog (Debug e Arquivo)                        |
| XAML Behaviors         | Microsoft.Xaml.Behaviors.Wpf                     |
| Publicação             | Self-contained, single file, win-x64 runtime    |

---

## Antes de tudo
1. Crie as tabelas em um banco SQLSERVER que tenha senha.
2. Insira os dados nas tabelas.

# Scripts SQL

Todos os scripts SQL para SQL Server estão na pasta [`CodigoSQL/`](/Exportador.Infrastructure/CodigoSQL).

## Conteúdo

- `createTable.sql` — cria as tabelas.
- `insertTable.sql` — insere dados iniciais.
  
---

## Como usar

1. Configure os parâmetros de conexão ao banco via UI.
2. Selecione as tabelas para exportação.
3. Escolha a pasta destino para salvar o arquivo final.
4. O sistema cria views no banco para consulta otimizada.
5. Exporte os dados para CSV.
6. Os arquivos CSV são compactados em um ZIP.
7. Resumo dos dados.

---

## Atenção?

O windows pode bloquear a criação de um arquivo com o mesmo nome, caso este já exista na mesma pasta ao iniciar a exportação!

## Contato

Para dúvidas, sugestões ou contribuições, entre em contato:  
📧 teodorowellington@yahoo.com.br

---

> Projeto em constante evolução, focado em qualidade, escalabilidade e boas práticas.

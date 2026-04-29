# QA Playwright

Projeto de automacao E2E em C# com Playwright, NUnit e GitHub Actions.

O objetivo deste repositorio e validar um fluxo completo de compra no site Automation Exercise, com criacao de usuario, login automatico, inclusao de produtos no carrinho, checkout, pagamento, evidencias e relatorios na pipeline.

## Stack

- C# / .NET 8
- Microsoft Playwright
- NUnit
- Page Object Model
- GitHub Actions
- Relatorios TRX e Markdown
- Videos e screenshots como evidencias

## Estrutura

```text
Core/
  BaseTest.cs       Setup e teardown do Playwright
  BasePage.cs       Helpers base para Page Objects

Pages/
  LoginPage.cs
  SignupPage.cs
  ProductsPage.cs
  CartPage.cs
  CheckoutPage.cs

Tests/
  CompraTests.cs    Testes Smoke e Regression

Utils/
  FakerFactory.cs   Massa dinamica de cadastro

Flows/
  PurchaseFlow.cs
  HybridFlow.cs

.github/workflows/
  pipeline.yml      Pipeline enterprise no GitHub Actions

nunit.runsettings   Configuracao de paralelismo do NUnit
```

## Cenarios Automatizados

O teste principal executa o fluxo:

1. Acessa a tela de login/cadastro.
2. Cria um usuario novo com e-mail dinamico.
3. Valida usuario logado.
4. Acessa produtos.
5. Adiciona dois produtos ao carrinho.
6. Valida quantidade no carrinho.
7. Realiza checkout.
8. Preenche pagamento.
9. Valida pedido finalizado.

## Massa de Testes

Hoje existem dois grupos:

| Categoria | Quantidade | Uso |
| --- | ---: | --- |
| Smoke | 3 | Validacao rapida do fluxo |
| Regression | 50 | Massa maior para validacao pesada |

Total descoberto pelo NUnit:

```text
53 testes
```

Os nomes aparecem assim:

```text
RealizarCompra_Smoke_1
RealizarCompra_Smoke_2
RealizarCompra_Smoke_3
RealizarCompra_Regression_1
...
RealizarCompra_Regression_50
```

## Paralelismo

Os testes rodam em paralelo com NUnit.

Arquivo:

```text
nunit.runsettings
```

Configuracao atual:

```xml
<RunSettings>
  <NUnit>
    <NumberOfTestWorkers>4</NumberOfTestWorkers>
  </NUnit>
</RunSettings>
```

Tambem foi configurado:

```csharp
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
[Parallelizable(ParallelScope.Children)]
```

Isso evita que testes paralelos compartilhem a mesma instancia de `Page`, `Browser` ou `Context`.

## Como Rodar Localmente

Restaurar dependencias:

```bash
dotnet restore
```

Compilar:

```bash
dotnet build
```

Instalar browsers do Playwright:

```bash
pwsh bin/Debug/net8.0/playwright.ps1 install
```

Rodar todos os testes:

```bash
dotnet test --settings nunit.runsettings
```

Rodar somente Smoke:

```bash
dotnet test --filter "Category=Smoke" --settings nunit.runsettings
```

Rodar somente Regression:

```bash
dotnet test --filter "Category=Regression" --settings nunit.runsettings
```

Rodar com resultado TRX:

```bash
dotnet test --settings nunit.runsettings --results-directory TestResults --logger "trx;LogFileName=e2e.trx"
```

## Evidencias Locais

Durante a execucao, o projeto pode gerar:

```text
videos/
screenshots/
TestResults/
reports/
```

Videos sao gravados pelo Playwright.

Screenshots sao gerados quando o teste falha.

## Pipeline Enterprise

Arquivo:

```text
.github/workflows/pipeline.yml
```

Gatilhos:

- `push` na branch `main`
- `pull_request` para `main`
- execucao manual via `workflow_dispatch`

Na execucao manual e possivel informar:

| Input | Exemplo | Descricao |
| --- | --- | --- |
| `test_filter` | `Category=Smoke` | Filtro do `dotnet test` |
| `workers` | `4` | Quantidade de workers paralelos |

Se `test_filter` ficar vazio, a pipeline roda todos os testes.

## Visualizacao no GitHub Actions

A pipeline foi separada em jobs para aparecer bem no grafo do GitHub Actions:

```text
Build & Restore
      |
Parallel E2E Execution (4 workers)
      |
      +--> Publish Test Report
      |
      +--> Publish Evidence
              |
        Quality Gate
```

Jobs:

| Job | Funcao |
| --- | --- |
| Build & Restore | Restaura pacotes e compila o projeto |
| Parallel E2E Execution | Executa os testes E2E em paralelo |
| Publish Test Report | Gera relatorio Markdown a partir do TRX |
| Publish Evidence | Mostra resumo das evidencias publicadas |
| Quality Gate | Falha a pipeline se os testes E2E falharem |

## Artifacts no GitHub Actions

A pipeline publica:

| Artifact | Conteudo |
| --- | --- |
| `test-results-trx` | Resultado `.trx` dos testes |
| `test-reports` | Relatorio Markdown `e2e-summary.md` |
| `playwright-videos` | Videos `.webm` dos testes |
| `failure-screenshots` | Screenshots `.png` em falhas |

## Relatorio

O relatorio `e2e-summary.md` mostra:

- modo de execucao
- quantidade de workers
- filtro utilizado
- resultado do job de testes
- total de testes
- aprovados
- falhados
- ignorados
- lista de testes falhados, quando houver

O mesmo conteudo tambem aparece no `GITHUB_STEP_SUMMARY` da execucao.

## Azure DevOps

O projeto tambem possui pipeline para Azure DevOps.

Arquivo:

```text
azure-pipelines.yml
```

Essa pipeline segue a mesma ideia enterprise do GitHub Actions:

- build e restore;
- execucao E2E em paralelo;
- publicacao de TRX;
- videos do Playwright;
- screenshots de falha;
- Allure results, quando forem gerados;
- relatorio Markdown;
- quality gate final.

### Como Configurar no Azure DevOps

1. Acesse seu projeto no Azure DevOps.
2. Va em `Pipelines`.
3. Clique em `New pipeline`.
4. Escolha `GitHub` ou `Azure Repos Git`, dependendo de onde esta o repositorio.
5. Selecione este repositorio.
6. Escolha `Existing Azure Pipelines YAML file`.
7. Informe:

```text
/azure-pipelines.yml
```

8. Salve e execute.

### Parametros Manuais

Ao rodar manualmente no Azure DevOps, voce pode informar:

| Parametro | Exemplo | Descricao |
| --- | --- | --- |
| `testFilter` | `Category=Smoke` | Roda somente os testes filtrados |
| `workers` | `4` | Define a quantidade de workers paralelos |

Exemplos de filtro:

```text
Category=Smoke
Category=Regression
```

Se `testFilter` ficar vazio, a pipeline roda todos os 53 testes.

### Stages no Azure

A visualizacao da pipeline no Azure mostra:

```text
Build & Restore
      |
Parallel E2E Execution
      |
Publish Test Report
      |
Quality Gate
```

### Artefatos no Azure

A pipeline publica:

| Artifact | Conteudo |
| --- | --- |
| `test-results-trx` | Resultado TRX dos testes |
| `playwright-videos` | Videos gravados pelo Playwright |
| `failure-screenshots` | Screenshots quando houver falha |
| `allure-results` | Resultados Allure quando gerados |
| `test-reports` | Relatorio Markdown |

### Test Results no Azure

O step `PublishTestResults@2` envia o arquivo TRX para a aba de testes do Azure DevOps.

Assim voce consegue ver:

- testes executados;
- testes aprovados;
- testes falhados;
- duracao;
- historico por pipeline;
- detalhes de erro por caso de teste.

## Comandos Uteis

Listar testes:

```bash
dotnet test --settings nunit.runsettings --list-tests --no-build
```

Contar testes de compra:

```powershell
dotnet test --settings nunit.runsettings --list-tests --no-build | Select-String "RealizarCompra_" | Measure-Object
```

Rodar Regression com 8 workers manualmente:

```bash
dotnet test --settings nunit.runsettings --filter "Category=Regression" -- NUnit.NumberOfTestWorkers=8
```

## Observacoes

- Os testes dependem do site externo `https://automationexercise.com`.
- Como e um ambiente externo, pode haver instabilidade por anuncios, lentidao ou indisponibilidade.
- Por isso os testes usam `Retry(2)` e esperas explicitas em pontos criticos.
- Para execucoes grandes, 4 workers e um valor conservador. Aumentar demais pode sobrecarregar o runner ou o site.

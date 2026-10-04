# DesafioTarget

Resolução de três desafios de programação em C#, cada um em um projeto de console separado.

## Estrutura

```
DesafioTarget/
├── Desafio1-Comissao/
├── Desafio2-Estoque/
└── Desafio3-Juros/
```

## Como executar

Requisito: [.NET SDK 10.0](https://dotnet.microsoft.com/download) instalado.

Na raiz do repositório, rode o projeto desejado:

```
dotnet run --project Desafio1-Comissao
dotnet run --project Desafio2-Estoque
dotnet run --project Desafio3-Juros
```

Também é possível abrir o `DesafioTarget.slnx` no Visual Studio e escolher o projeto de inicialização.

---

## Desafio 1: Comissão de vendedores

Lê o arquivo `vendas.json` e calcula a comissão de cada vendedor. A regra é aplicada em cada venda, e depois as comissões são somadas por vendedor.

| Valor da venda | Comissão |
|---|---|
| Abaixo de R$ 100,00 | Sem comissão |
| De R$ 100,00 até R$ 499,99 | 1% |
| A partir de R$ 500,00 | 5% |

O resultado é uma tabela com a quantidade de vendas, o total vendido e a comissão de cada vendedor.

## Desafio 2: Movimentação de estoque

Programa de console para lançar entradas e saídas de mercadoria dos produtos do `estoque.json`.

Cada movimentação possui:
- um número identificador único (sequencial);
- uma descrição, que pode ser apenas `compra`, `venda` ou `devolução`.

Ao final de cada lançamento, o programa mostra a quantidade final em estoque do produto movimentado.

Validações:
- código de produto inexistente;
- tipo de movimentação diferente de entrada (E) ou saída (S);
- quantidade zerada, negativa ou, no caso de saída, maior que o estoque disponível;
- descrição fora das três permitidas.

Quando uma resposta é inválida, o programa repete apenas a pergunta atual, sem voltar ao início.

## Desafio 3: Juros por atraso

A partir de um valor e de uma data de vencimento, calcula os juros até a data de hoje, considerando a taxa de 2,5% ao dia.

```
juros = valor × 2,5% × dias de atraso
```

- Os juros são simples, calculados sobre o valor original.
- Se o vencimento é hoje ou uma data futura, não há atraso nem juros.
- O programa mostra o valor original, os dias de atraso, os juros e o total a pagar, e permite fazer vários cálculos em sequência.

---

## Decisões

- Valores monetários usam `decimal`, com arredondamento comercial (`MidpointRounding.AwayFromZero`).
- As regras de negócio ficam separadas da leitura de dados e da exibição.
- A leitura e a validação das entradas do usuário usam uma única função (`Ler`), reaproveitada em todas as perguntas.
- No Desafio 2, o estoque fica em memória durante a execução. Ao fechar o programa, os valores voltam aos do `estoque.json`.

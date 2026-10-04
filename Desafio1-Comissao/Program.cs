using System.Globalization;
using System.Text.Json;

CultureInfo.CurrentCulture = new CultureInfo("pt-BR");

string json = File.ReadAllText("vendas.json");

var opcoes = new JsonSerializerOptions {PropertyNameCaseInsensitive = true};
var dados = JsonSerializer.Deserialize<DadosVendas>(json, opcoes);

if (dados == null || dados.vendas.Count == 0)
{
    Console.WriteLine("Nenhuma venda encontrada.");
    return;
}

var resumo = dados.vendas
    .GroupBy(x => x.vendedor)
    .Select(x => new ResumoVendedor
    {
        vendedor = x.Key,
        quantidade = x.Count(),
        total = x.Sum(v => v.valor),
        comissao = x.Sum(v => CalcularComissao(v.valor))
    })
    .OrderByDescending(x => x.comissao)
    .ToList();

Console.WriteLine($"{"Vendedor",-18} {"Vendas",7} {"Total",15} {"Comissão",15}");
Console.WriteLine(new string('-', 57));

foreach (var item in resumo)
{
    Console.WriteLine($"{item.vendedor,-18} {item.quantidade,7} {item.total,15:C} {item.comissao,15:C}");
}

static decimal CalcularComissao(decimal valor)
{
    if (valor < 100) 
        return 0;

    if (valor < 500)
        return Math.Round(valor * 0.01m, 2, MidpointRounding.AwayFromZero);

    return Math.Round(valor * 0.05m, 2, MidpointRounding.AwayFromZero);
}

record Venda(string vendedor, decimal valor);
class DadosVendas
{
    public List<Venda> vendas { get; set; }
}

class ResumoVendedor
{
    public string vendedor { get; set; } = string.Empty;
    public int quantidade { get; set; }
    public decimal total { get; set; }
    public decimal comissao { get; set; }
}
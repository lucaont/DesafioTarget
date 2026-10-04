using System.Globalization;
using System.Text.Json;

CultureInfo.CurrentCulture = new CultureInfo("pt-BR");

string json = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "estoque.json"));
var opcoes = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
var dados = JsonSerializer.Deserialize<DadosEstoque>(json, opcoes);

if (dados == null || dados.estoque.Count == 0)
{
    Console.WriteLine("Nenhum produto encontrado.");
    return;
}

string[] descricoesValidas = { "compra", "venda", "devolução" };
var movimentacoes = new List<Movimentacao>();

while (true)
{
    Console.WriteLine("\nProdutos em estoque:");
    foreach (var p in dados.estoque)
        Console.WriteLine($"{p.codigoProduto} - {p.descricaoProduto} ({p.estoque} un.)");

    int codigo = int.Parse(Ler(
        "\nCódigo do produto (0 para sair): ",
        s => int.TryParse(s, out int c) && (c == 0 || dados.estoque.Any(p => p.codigoProduto == c)),
        "Produto não encontrado."));

    if (codigo == 0)
        break;

    var produto = dados.estoque.First(p => p.codigoProduto == codigo);

    bool entrada = Ler(
        "Tipo (E = entrada, S = saída): ",
        s => s.ToUpper() is "E" or "S",
        "Tipo inválido.").ToUpper() == "E";

    if (!entrada && produto.estoque == 0)
    {
        Console.WriteLine("Produto sem estoque para saída.");
        continue;
    }

    int limite = entrada ? int.MaxValue : produto.estoque;
    int quantidade = int.Parse(Ler(
        "Quantidade: ",
        s => int.TryParse(s, out int q) && q > 0 && q <= limite,
        entrada ? "Quantidade inválida." : $"Quantidade inválida. Disponível: {produto.estoque} un."));

    string descricao = Ler(
        "Descrição (compra, venda ou devolução): ",
        s => descricoesValidas.Contains(s, StringComparer.OrdinalIgnoreCase),
        "Movimentação inválida. Use: compra, venda ou devolução.").ToLower();

    produto.estoque += entrada ? quantidade : -quantidade;

    var movimentacao = new Movimentacao(movimentacoes.Count + 1, produto.codigoProduto, descricao, quantidade);
    movimentacoes.Add(movimentacao);

    Console.WriteLine($"\nMovimentação {movimentacao.Id} registrada ({movimentacao.Descricao}).");
    Console.WriteLine($"Estoque final de {produto.descricaoProduto}: {produto.estoque} un.");
}

static string Ler(string pergunta, Func<string, bool> valido, string mensagemErro)
{
    while (true)
    {
        Console.Write(pergunta);
        string resposta = Console.ReadLine()?.Trim() ?? "";

        if (valido(resposta))
            return resposta;

        Console.WriteLine(mensagemErro);
    }
}

record Movimentacao(int Id, int CodigoProduto, string Descricao, int Quantidade);
class Produto
{
    public int codigoProduto { get; set; }
    public string descricaoProduto { get; set; } = string.Empty;
    public int estoque { get; set; }
}
class DadosEstoque
{
    public List<Produto> estoque { get; set; } = new();
}

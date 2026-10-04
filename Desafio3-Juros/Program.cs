using System.Globalization;

CultureInfo.CurrentCulture = new CultureInfo("pt-BR");

const decimal taxaDiaria = 0.025m;

while (true)
{
    decimal valor = Ler<decimal>(
        "Valor (R$): ",
        (string s, out decimal v) => decimal.TryParse(s, out v) && v > 0,
        "Informe um valor maior que zero (ex.: 1500,50).");

    DateTime vencimento = Ler<DateTime>(
        "Data de vencimento (dd/MM/aaaa): ",
        (string s, out DateTime d) => DateTime.TryParseExact(s, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out d),
        "Data inválida. Use o formato dd/MM/aaaa.");

    // Vencimento hoje ou no futuro não tem atraso
    int diasAtraso = Math.Max(0, (DateTime.Today - vencimento).Days);
    decimal juros = Math.Round(valor * taxaDiaria * diasAtraso, 2, MidpointRounding.AwayFromZero);

    Console.WriteLine();
    Console.WriteLine($"Valor original: {valor:C}");
    Console.WriteLine($"Dias em atraso: {diasAtraso}");
    Console.WriteLine($"Juros ({taxaDiaria:P1} ao dia): {juros:C}");
    Console.WriteLine($"Total a pagar: {valor + juros:C}");

    bool calcularOutro = Ler<bool>(
        "\nCalcular outro? (S/N): ",
        (string s, out bool resposta) =>
        {
            resposta = s.ToUpper() == "S";
            return s.ToUpper() is "S" or "N";
        },
        "Digite S para sim ou N para não.");

    if (!calcularOutro)
        break;

    Console.WriteLine();
}

static T Ler<T>(string pergunta, Conversor<T> converter, string mensagemErro)
{
    while (true)
    {
        Console.Write(pergunta);

        if (converter(Console.ReadLine()?.Trim() ?? "", out T resultado))
            return resultado;

        Console.WriteLine(mensagemErro);
    }
}

delegate bool Conversor<T>(string texto, out T resultado);
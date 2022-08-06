//var notasTurma = new List<int> { 10, 5, 2, 3, 8, 7, 4, 10, 3, 2, 5, 8, 9 };

//// Verifica se existe nota de valor 1
//var existeNota1 = notasTurma.Any(n => n == 1);

//// Pega a primeira nota de valor 10
//var primeiraNota10 = notasTurma.First(n => n == 10);

//// Vai retornar apenas uma nota de valor 1
//var singleNota1 = notasTurma.Single(n => n == 1);

//// Pega a nota de valor máximo
//var max = notasTurma.Max();

//// Vai pegar a nota de valor minimo
//var min = notasTurma.Min();

//// Soma as notas
//var sum = notasTurma.Sum();

//// Vai pegar a média das notas
//var media = notasTurma.Average();

//// Ordernar
//var ordered = notasTurma.OrderBy(n => n);

//foreach (var nota in notasTurma)
//{
//    Console.WriteLine(nota);
//}

using static ObjetivoFinanceiro;

Console.WriteLine("---- PoupaDev ----\r\n");

// Inicializa uma lista do tipo objetivo
var objetivos = new List<ObjetivoFinanceiro>
{
    // Lista de objetivo financeiro sem prazo
    new ObjetivoFinanceiro("Viagem lua de mel", 15000),
    new ObjetivoFinanceiroComPrazo(new DateTime(2023,10,1), "Viagem lua de mel com prazo", 15000)
};

foreach (var objetivo in objetivos)
{
    objetivo.ImprimirResumo();
}

ExibirMenu();

// Le a opção
var opcao = Console.ReadLine();

// Vai repetir enquanto a opção escolhida não for zero
while (opcao != "0")
{
    switch (opcao)
    {
        case "1":
            // Cadastrar
            CadastrarObjetivo();
            break;
        case "2":
            // Depósito
            RealizarOperacao(TipoOperacao.Deposito);
            break;
        case "3":
            // Saque
            RealizarOperacao(TipoOperacao.Saque);
            break;
        case "4":
            // Detalhes
            ObterDetalhes();
            break;
        default:
            // Opção inválida.
            Console.WriteLine("Opção inválida.");
            break;
    }
    // Exibe o menu novamente e pede pra digitar uma opção novamente
    ExibirMenu();
    opcao = Console.ReadLine();
}

void CadastrarObjetivo()
{
    Console.WriteLine("\r\nDigite um título:");
    var titulo = Console.ReadLine();

    Console.WriteLine("\r\nDigite um valor de objetivo:");
    var valorLido = Console.ReadLine();

    //Converte o valor lido (string) para decimal
    var valor = decimal.Parse(valorLido);

    // Inicializa um objetivo financeiro com 2 parametros
    var objetivo = new ObjetivoFinanceiro(titulo, valor);

    // Adiciona objetivo
    objetivos.Add(objetivo);
    Console.WriteLine($"Objetivo ID: {objetivo.Id} foi criado com sucesso!\r\n");

}
// Método para tratar o tipo da operação (saque e depósito)
void RealizarOperacao(TipoOperacao tipo)
{
    Console.WriteLine("\r\nDigite o Id do objetivo:");
    var idLido = Console.ReadLine();
    //Converte o valor lido (string) para int
    var id = int.Parse(idLido);

    Console.WriteLine("\r\nDigite o valor da operação:");
    var valorLido = Console.ReadLine();
    //Converte o valor lido (string) para int
    var valor = int.Parse(valorLido);

    // Inicializa
    var operacao = new Operacao(valor, tipo, id);

    // Vai buscar um valor que atenda a ação se não tiver valor retorna nulo
    var objetivo = objetivos.SingleOrDefault(o => o.Id == id);

    // Adiciona operacao
    objetivo.Operacoes.Add(operacao);
}

void ObterDetalhes()
{
    Console.WriteLine("Digite o Id do objetivo:");
    var idLido = Console.ReadLine();
    //Converte o valor lido (string) para int
    var id = int.Parse(idLido);

    // Vai buscar um valor que atenda a ação se não tiver valor retorna nulo
    var objetivo = objetivos.SingleOrDefault(o => o.Id == id);

    objetivo.ImprimirResumo();
}

// Método para exibir menu
void ExibirMenu()
{
    Console.WriteLine("Digite 1 para Cadastro de Objetivo");
    Console.WriteLine("Digite 2 para realizar um depósito");
    Console.WriteLine("Digite 3 para realizar um saque");
    Console.WriteLine("Digite 4 para exibir detalhes de um objetivo");
    Console.WriteLine("Digite 0 para encerrar\r\n");
}
public enum TipoOperacao
{
    Saque = 0,
    Deposito = 1,

}
public class Operacao
{
    // Método construtor
    public Operacao(decimal valor, TipoOperacao tipo, int idObjetivo)
    {
        // Gerando id de forma aleatória
        Id = new Random().Next(0, 1000);
        Valor = valor;
        Tipo = tipo;
        IdObjetivo = idObjetivo;
    }

    public int Id { get; private set; }
    public decimal Valor { get; private set; }
    public TipoOperacao Tipo { get; private set; }

    public int IdObjetivo { get; private set; }
}


public class ObjetivoFinanceiro
{
    // Método construtor
    public ObjetivoFinanceiro(string? titulo, decimal? valorObjetivo)
    {
        // Gerando id de forma aleatória
        Id = new Random().Next(0, 1000);
        Titulo = titulo;
        ValorObjetivo = valorObjetivo;

        // Inicializa operação
        Operacoes = new List<Operacao>();
    }

    public int Id { get; private set; }

    // Pode ser null ?
    public string? Titulo { get; private set; }
    public decimal? ValorObjetivo { get; private set; }

    public List<Operacao> Operacoes { get; private set; }

    //Retorna o método 
    public decimal Saldo => ObterSaldo();

    public decimal ObterSaldo()
    {
        var totalDeposito = Operacoes
            // Vai pegar todas as operacoes que o tipo seja igual a deposito
            .Where(o => o.Tipo == TipoOperacao.Deposito)
            // Soma o valor das operações do tipo deposito
            .Sum(o => o.Valor);

        var totalSaque = Operacoes
            // Vai pegar todas as operacoes que o tipo seja igual a saque
            .Where(o => o.Tipo == TipoOperacao.Saque)
            // Soma o valor das operações do tipo saque
            .Sum(o => o.Valor);

        return totalDeposito - totalSaque;

    }

    // Virtual permite que as classes derivadas implementem/estende a capacidade do método
    public virtual void ImprimirResumo()
    {
        Console.WriteLine($"Objetivo {Titulo}, Valor: {ValorObjetivo}, com saldo atual: R${Saldo}");
    }

    public class ObjetivoFinanceiroComPrazo : ObjetivoFinanceiro
    {
        public ObjetivoFinanceiroComPrazo(DateTime prazo, string? titulo, decimal? valorObjetivo) : base(titulo, valorObjetivo)
        {
            Prazo = prazo;
        }

        // Estende o método imprimir resumo
        public DateTime Prazo { get; private set; }

        public override void ImprimirResumo()
        {
            base.ImprimirResumo();

            // Ver diferença entre as 2 datas
            var diasRestantes = (Prazo - DateTime.Now).TotalDays;
            // Ve o valor restante
            var valorRestante = ValorObjetivo - Saldo;

            Console.WriteLine($"Faltam {diasRestantes} parao seu objetivo, e faltam R${valorRestante} para completar");
        }
    }
}
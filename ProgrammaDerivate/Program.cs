using Antlr4.Runtime;
using ProgrammaDerivate;


Console.WriteLine("Qui vengono calcolate le derivate prime.");
Console.WriteLine("Per calcolare la derivata di una potenza, usa una notazione come y=(x^9).");
char[] Segni = { '+', '-', '/', '*' };
for (int i = Segni.Length-1; i > 1; i--)
{
    Console.WriteLine($"Per calcolare la derivata k{Segni[i]}f(x), usa una notazione come y=(k{Segni[i]}f(x)).");
}
for (int i = 0; i < Segni.Length; i++)
{
    Console.WriteLine($"Per calcolare la derivata f(x){Segni[i]}g(x), usa una notazione come y=((f(x)){Segni[i]}(g(x))).");

}
Console.WriteLine("inserire la tua funzione qui sotto.");
var ascolto = new Ascolto();
var f = Console.ReadLine();
var lexer =  new AntlrInputStream(f);
var l = new ExprLexer(lexer);
var tokens = new CommonTokenStream(l);
var parser = new ExprParser(tokens);
var tree = parser.expr();
var g = ascolto.Visit(tree);
Console.WriteLine("Funzione: "+f);
Console.WriteLine("Derivata: y'="+g);






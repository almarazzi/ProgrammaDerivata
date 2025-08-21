using Antlr4.Runtime;
using ProgrammaDerivate;


Console.WriteLine("Qui vengono calcolate le derivate prime.");
Console.WriteLine("Per calcolare la derivata di una potenza, usa una notazione come y=(x^9).");
Console.WriteLine("Per calcolare la derivata k/f(x), usa una notazione come y=(k/f(x)).");
Console.WriteLine("Per calcolare la derivata k*f(x), usa una notazione come y=(k/f(x)).");
Console.WriteLine("Per calcolare la derivata f(x)+g(x), usa una notazione come y=((f(x))+(g(x))).");
Console.WriteLine("Per calcolare la derivata f(x)-g(x), usa una notazione come y=((f(x))-(g(x))).");
Console.WriteLine("Per calcolare la derivata f(x)*g(x), usa una notazione come y=((f(x))*(g(x))).");
Console.WriteLine("Per calcolare la derivata f(x)/g(x), usa una notazione come y=((f(x))/(g(x))).");
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






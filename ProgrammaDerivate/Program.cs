using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using ProgrammaDerivate;


Console.WriteLine("Qui vengono calcolate le derivate prime.");
Console.WriteLine("Per calcolare la derivata di una potenza, usa una notazione come 1*x^9.");
Console.WriteLine("Per calcolare la derivata di una frazione con una potenza, usa una notazione come 1/x^9.");
Console.WriteLine("inserire la tun funzione qui sotto.");
var f = Console.ReadLine();
var lexer =  new AntlrInputStream(f);
var l = new ExprLexer(lexer);
var tokens = new CommonTokenStream(l);
var parser = new ExprParser(tokens);
var tree = parser.expr();
var ascolto = new Ascolto();
ParseTreeWalker.Default.Walk(ascolto, tree);
Console.WriteLine(ascolto.Ritornostringa());




using ProgrammaDerivate;

Console.WriteLine("Qui vengono calcolate le derivate prime.");
Console.WriteLine("nota che x non va bene si deve essere espliciti");
Console.WriteLine("Per calcolare la derivata di una potenza, usa una notazione come y=(x^9).");
char[] Segni = { '+', '-', '/', '*' };
for (int i = Segni.Length - 1; i > 1; i--)
{
    Console.WriteLine($"Per calcolare la derivata k{Segni[i]}f(x), usa una notazione come y=((k){Segni[i]}(f(x))).");
}
for (int i = 0; i < Segni.Length; i++)
{
    Console.WriteLine($"Per calcolare la derivata f(x){Segni[i]}g(x), usa una notazione come y=((f(x)){Segni[i]}(g(x))).");
}
Console.WriteLine($"Per calcolare la derivata cos(f(x)), usa una notazione come y=(cos(f(x))).");
Console.WriteLine($"Per calcolare la derivata sin(f(x)), usa una notazione come y=(sin(f(x))).");
Console.WriteLine($"Per calcolare la derivata ln(f(x)), usa una notazione come y=(ln(f(x))).");
Console.WriteLine($"Per calcolare la derivata logbase(f(x)), usa una notazione come y=(logbase(f(x))).");
Console.WriteLine($"Per calcolare la derivata tan(f(x)), usa una notazione come y=(tan(f(x))).");
Console.WriteLine($"Per calcolare la derivata e^(f(x)), usa una notazione come y=(e^(f(x))).");
Console.WriteLine($"Per calcolare la derivata di radice , usa una notazione come y=(indicesqrt(f(x))).");
Console.WriteLine("inserire la tua funzione qui sotto.");
var y = Console.ReadLine();
var ascolto = new Ascolto();
var g =ascolto.Input(y!);
Console.WriteLine("Funzione: " + y);
Console.WriteLine("Derivata: y'=" + g);
var S = ascolto.Input($"s=({g})");
Console.WriteLine("Semplificazione =" + S);
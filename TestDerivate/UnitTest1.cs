using Antlr4.Runtime;
using ProgrammaDerivate;

namespace TestDerivate
{
    public class UnitTest1
    {
        private readonly Ascolto m_ascolto = new Ascolto();


        private ExprParser.ExprContext Espressione(string d)
        {
            var inp = new AntlrInputStream(d);
            var exp = new ExprLexer(inp);
            var token = new CommonTokenStream(exp);
            var p = new ExprParser(token);
            return p.expr();
        }
        [Fact]
        public void TestPotenza()
        {

            var t = Espressione("y=(x^(9*2))");
            var g = m_ascolto.Visit(t);
            var s = "18*x^17";
            Assert.Equal(g, s);
        }
        [Fact]
        public void TestPotenzaMol()
        {

            var t = Espressione("y=((2)*(x^9))");
            var g = m_ascolto.Visit(t);
            var s = "2*9*x^8";
            Assert.Equal(g, s);
        }

        [Fact]
        public void TestPotenzaDiv()
        {

            var t = Espressione("y=((2)/(x^9))");
            var g = m_ascolto.Visit(t);
            var s = "2*-9*x^-10";
            Assert.Equal(g, s);
        }
        [Fact]
        public void TestRadice()
        {
            var t = Espressione("y=(2sqrt(x^3))");
            var g = m_ascolto.Visit(t);
            var s = "1,5*x^0,5";
            Assert.Equal(g, s);
        }
        [Fact]
        public void TestSomma()
        {

            var t = Espressione("y=((x^2)+(x^3))");
            var g = m_ascolto.Visit(t);
            var s = "2*x^1 + 3*x^2";
            Assert.Equal(g, s);
        }
        [Fact]
        public void TestMeno()
        {

            var t = Espressione("y=((x^2)-(x^3))");
            var g = m_ascolto.Visit(t);
            var s = "2*x^1 - 3*x^2";
            Assert.Equal(g, s);
        }
        [Fact]
        public void TestPer()
        {

            var t = Espressione("y=((x^2)*(x^3))");
            var g = m_ascolto.Visit(t);
            var s = "(2*x^1)*(x^3) + (x^2)*(3*x^2)";
            Assert.Equal(g, s);
        }
        [Fact]
        public void TestDiv()
        {

            var t = Espressione("y=((x^2)/(x^3))");
            var g = m_ascolto.Visit(t);
            var s = "((2*x^1)*(x^3) - (x^2)*(3*x^2))/(x^3)^2";
            Assert.Equal(g, s);
        }
        [Fact]
        public void TestFun()
        {
            string[] fun = { "sin(x)", "cos(x)", "tan(x)", $"log{10}(x)", "ln(x)", "e^(x)" };
            string[] Ris = { "cos(x)*1", "-sin(x)*1", "1/sec(x)^2*1", "1/(x*ln(10))*1", "1/x*1", "e^(x)*1" };
            for (int i = 0; i < fun.Length; i++)
            {
                var t = Espressione($"y=({fun[i]})");
                var g = m_ascolto.Visit(t);
                var s = $"{Ris[i]}";
                Assert.Equal(g, s);
            }

        }
        [Fact]
        public void TestFunzioneComposta()
        {
            string[] fun = { "sin(x^2)", "cos(x^3)" };
            string[] Ris = { "cos(x^2)*2*x^1", "-sin(x^3)*3*x^2" };
            for (int i = 0; i < fun.Length; i++)
            {

                var t = Espressione($"y=({fun[i]})");
                var g = m_ascolto.Visit(t);
                var s = $"{Ris[i]}";
                Assert.Equal(g, s);
            }

        }
        [Fact]
        public void TestSemplificazione()
        {
            var t = Espressione("y=(((3*x)/(2*x))*(5*x)-(2*10))");
            var g = m_ascolto.Visit(t);
            var s = "Semplificazione =+(7,5*x^3)-20";
            Assert.Equal(g, s);
        }
    }
}

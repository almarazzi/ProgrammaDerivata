using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using org.matheval;
using System.Text;





namespace ProgrammaDerivate
{

    internal class Ascolto : ExprBaseVisitor<string>
    {
        private readonly Expression m_expression = new Expression();
        private bool DivisioneB = false;
        private readonly StringBuilder m_output = new StringBuilder();

        public string Input(string y)
        {
            Ascolto ascolto1 = new Ascolto();
            var lexer = new AntlrInputStream(y);
            var l = new ExprLexer(lexer);
            var tokens = new CommonTokenStream(l);
            var parser = new ExprParser(tokens);
            var tree = parser.expr();
            return ascolto1.Visit(tree);
        }

        private void Stringa(Semplificazione Espressione)
        {
            if (Espressione.Funzioni != null)
            {
                m_output.Append($"{Espressione.Funzioni}");
            }
            foreach (var item in Espressione.conX)
            {
                var d = m_output.Length != 0 && (item.Value >= 0) ? "+" : "";
                m_output.Append($"{d}{Espressione.conX[item.Key]}*x^{item.Key}");
            }
            if (Espressione.Costanti != 0)
            {
                var a = (Espressione.Costanti >= 0) ? "+" : "";
                m_output.Append($"{a}{Espressione.Costanti}");
            }
        }
        public override string VisitDerivataEspressione([NotNull] ExprParser.DerivataEspressioneContext context)
        {
            var Espressione = context.expr();
            return Derivata(Espressione);
        }
        public override string VisitSemplificazioneEspressione([NotNull] ExprParser.SemplificazioneEspressioneContext context)
        {
            var Espressione = context.expr();
            var g = semplifica(Espressione);
            m_output.Clear();
            Stringa(g);
            return m_output.ToString();
        }
        private string Derivata([NotNull] ExprParser.ExprContext Espressione)
        {

            if (Espressione is ExprParser.CostanteEspressioneContext)
            {
                return "1";
            }
            if (Espressione is ExprParser.NumeroEspressioneContext)
            {
                return "0";
            }
            if (Espressione is ExprParser.ParentesiEspressioneContext Parentesi)
            {
                return Derivata(Parentesi.expr());
            }
            if (Espressione is ExprParser.RadiceEspressioneContext Radice)
            {
                m_expression.SetFomular(Radice.expr(0).GetText());
                var Indice = m_expression.Eval<double>();
                var f = Radice.expr(1).GetText().Replace("(", "").Replace(")", "").Split('^');
                m_expression.SetFomular(f[1]);
                var Esponente = m_expression.Eval<double>();
                return $"{Esponente / Indice}*x^{(Esponente / Indice) - 1}";

            }
            if (Espressione is ExprParser.PotenzaEspressioneContext Potenza)
            {
                m_expression.SetFomular(Potenza.expr(1).GetText());
                var Esponente = m_expression.Eval<double>();
                if (DivisioneB)
                    Esponente = -Esponente;

                return $"{Esponente}*x^{Esponente - 1.0}";
            }

            if (Espressione is ExprParser.SommaEspressioneContext Somma)
            {

                return $"{Derivata(Somma.expr(0))} + {Derivata(Somma.expr(1))}";
            }
            if (Espressione is ExprParser.SottrazioneEspressioneContext Sottrazione)
            {

                return $"{Derivata(Sottrazione.expr(0))} - {Derivata(Sottrazione.expr(1))}";
            }
            if (Espressione is ExprParser.MoltiplicazioneEspressioneContext Moltiplicazione)
            {
                if (Moltiplicazione.expr(0).GetText().Contains("x") && Moltiplicazione.expr(1).GetText().Contains("x"))
                {
                    return $"({Derivata(Moltiplicazione.expr(0))})*{Moltiplicazione.expr(1).GetText()} + {Moltiplicazione.expr(0).GetText()}*({Derivata(Moltiplicazione.expr(1))})";
                }
                else if (Moltiplicazione.expr(1).GetText().Contains("x"))
                {
                    m_expression.SetFomular(Moltiplicazione.expr(0).GetText());
                    var FattoreNum = m_expression.Eval<double>();
                    return $"{FattoreNum}*{Derivata(Moltiplicazione.expr(1))}";
                }
            }
            if (Espressione is ExprParser.DivisioneEspressioneContext Divisione)
            {
                if (Divisione.expr(0).GetText().Contains("x") && Divisione.expr(0).GetText().Contains("x"))
                {
                    return $"(({Derivata(Divisione.expr(0))})*{Divisione.expr(1).GetText()} - {Divisione.expr(0).GetText()}*({Derivata(Divisione.expr(1))}))/{Divisione.expr(1).GetText()}^2";
                }
                else if (Divisione.expr(1).GetText().Contains("x"))
                {
                    m_expression.SetFomular(Divisione.expr(0).GetText());
                    var FattoreNum = m_expression.Eval<double>();
                    DivisioneB = true;
                    return $"{FattoreNum}*{Derivata(Divisione.expr(1))}";
                }
            }
            if (Espressione is ExprParser.FunzioneEspressioneContext Funzione)
            {
                switch (Funzione.func().GetText())
                {
                    case "sin":
                        return $"cos({Funzione.expr().GetText()})*{Derivata(Funzione.expr())}";
                    case "cos":
                        return $"-sin({Funzione.expr().GetText()})*{Derivata(Funzione.expr())}";
                    case "ln":
                        return $"1/{Funzione.expr().GetText()}*{Derivata(Funzione.expr())}";
                    case "e^":
                        return $"e^({Funzione.expr().GetText()})*{Derivata(Funzione.expr())}";
                    case "tan":
                        return $"1/sec({Funzione.expr().GetText()})^2*{Derivata(Funzione.expr())}";
                    case string s when s == $"log{Funzione.func().NUMBER().GetText()!}":
                        return $"1/({Funzione.expr().GetText()}*ln({Funzione.func().NUMBER().GetText()}))*{Derivata(Funzione.expr())}";

                }
            }

            return "non è una derivata";
        }


        private Semplificazione semplifica([NotNull] ExprParser.ExprContext d)
        {
            var m_Semplificazione = new Semplificazione();
            if (d is ExprParser.ParentesiEspressioneContext P)
            {
                return semplifica(P.expr());
            }
            if (d is ExprParser.NumeroEspressioneContext N)
            {
                m_Semplificazione.Costanti = double.Parse(N.GetText());
                return m_Semplificazione;
            }
            if (d is ExprParser.NumNegContext NN)
            {
                m_Semplificazione.Costanti = double.Parse(NN.GetText());
                return m_Semplificazione;
            }
            if (d is ExprParser.CostanteEspressioneContext I)
            {
                m_Semplificazione.conX["1"] = 1;
                return m_Semplificazione;
            }
            if (d is ExprParser.PotenzaEspressioneContext Po)
            {
                m_Semplificazione.conX[Po.expr(1).GetText()] = 1;
                return m_Semplificazione;
            }
            if (d is ExprParser.SommaEspressioneContext S)
            {
                var s = semplifica(S.expr(0));
                var de = semplifica(S.expr(1));

                if (s.Funzioni == de.Funzioni)
                {
                    foreach (var item in s.conX.Concat(de.conX))
                    {
                        if (m_Semplificazione.conX.ContainsKey(item.Key))
                            m_Semplificazione.conX[item.Key] += item.Value;
                        else
                            m_Semplificazione.conX[item.Key] = item.Value;
                    }
                    if (s.Funzioni != null)
                    {
                        m_Semplificazione.Costanti = s.Costanti + de.Costanti;
                        var g = m_Semplificazione.conX.Select(x => x.Value + "*x^" + x.Key).ToList();
                        var f = string.Join("+", g);
                        var piu = Input($"s=({m_Semplificazione.Costanti}+{f}/2)");
                        var meno = Input($"s=({m_Semplificazione.Costanti}-{f}/2)");
                        m_Semplificazione.Funzioni = $"2{s.Funzioni}({piu})*{((de.Funzioni == "sin") ? "cos" : "cos")}({meno})";
                        m_Semplificazione.Costanti = 0;
                        m_Semplificazione.conX.Clear();
                    }
                }

                m_Semplificazione.Costanti = s.Costanti + de.Costanti;
                return m_Semplificazione;
            }
            if (d is ExprParser.SottrazioneEspressioneContext Sot)
            {
                var s = semplifica(Sot.expr(0));
                var de = semplifica(Sot.expr(1));
                foreach (var item in s.conX.Concat(de.conX))
                {
                    if (m_Semplificazione.conX.ContainsKey(item.Key))
                        m_Semplificazione.conX[item.Key] -= item.Value;
                    else
                        m_Semplificazione.conX[item.Key] = -item.Value;
                }
                m_Semplificazione.Costanti = s.Costanti - de.Costanti;
                return m_Semplificazione;
            }
            if (d is ExprParser.MoltiplicazioneEspressioneContext M)
            {
                var s = semplifica(M.expr(0));
                var de = semplifica(M.expr(1));
                m_Semplificazione.Funzioni = s.Funzioni ?? de.Funzioni;
                var n = ((s.Costanti != 0) ? s.Costanti : 1) * ((de.Costanti != 0) ? de.Costanti : 1);
                if (s.conX.Count == 0 && de.conX.Count == 0)
                {
                    m_Semplificazione.Costanti = n;
                    return m_Semplificazione;
                }
                var nX = s.conX.Values.Concat(de.conX.Values).Aggregate((x, z) => x * z);
                var e = s.conX.Keys.Concat(de.conX.Keys).Select(x => int.Parse(x)).Sum();
                m_Semplificazione.conX[e.ToString()] = nX * n;
                return m_Semplificazione;
            }
            if (d is ExprParser.DivisioneEspressioneContext D)
            {
                var s = semplifica(D.expr(0));
                var de = semplifica(D.expr(1));
                var n = ((s.Costanti != 0) ? s.Costanti : 1) * (1 / ((de.Costanti != 0) ? de.Costanti : 1));
                if (s.conX.Count == 0 && de.conX.Count == 0)
                {
                    m_Semplificazione.Costanti = n;
                    return m_Semplificazione;
                }
                var nX = s.conX.Values.Concat(de.conX.Values).Aggregate((x, z) => x * (1 / z));
                var e = s.conX.Keys.Concat(de.conX.Keys).Select(x => int.Parse(x)).Sum();
                m_Semplificazione.conX[e.ToString()] = nX * n;
                return m_Semplificazione;
            }
            if (d is ExprParser.FunzioneEspressioneContext Funzione)
            {
                m_Semplificazione = semplifica(Funzione.expr());
                m_Semplificazione.Funzioni = Funzione.func().GetText();
                return m_Semplificazione;
            }
            return m_Semplificazione;
        }
    }
    class Semplificazione
    {
        public double Costanti;
        public Dictionary<string, double> conX = new Dictionary<string, double>();
        public string Funzioni;
    }

}
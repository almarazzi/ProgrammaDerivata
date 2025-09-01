using Antlr4.Runtime.Misc;
using org.matheval;
using System.Text;





namespace ProgrammaDerivate
{

    public class Ascolto : ExprBaseVisitor<string>
    {
        private readonly Expression m_expression = new Expression();
        private bool DivisioneB = false;
        private readonly StringBuilder m_output = new StringBuilder();

        public override string VisitDerivataEspressione([NotNull] ExprParser.DerivataEspressioneContext context)
        {
            var Espressione = context.expr();
            var g = semplifica(Espressione);
            m_output.Append("Semplificazione= ");
            foreach (var item in g.conX)
            {
                m_output.Append($"({g.conX[item.Key]}*x^{item.Key})+");
            }
            m_output.Append($"{g.Costanti}");
            return m_output.ToString();
            // return Derivata(Espressione);


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
            if (d is ExprParser.CostanteEspressioneContext I)
            {
                m_Semplificazione.conX["1"] = 1;
                return m_Semplificazione;
            }
            if (d is ExprParser.PotenzaEspressioneContext Pot)
            {
                m_Semplificazione.conX[Pot.expr(1).GetText()] = 1;
                return m_Semplificazione;
            }
            if (d is ExprParser.SommaEspressioneContext S)
            {
                var s = semplifica(S.expr(0));
                var de = semplifica(S.expr(1));
                foreach (var item in s.conX.Concat(de.conX))
                {
                    if (m_Semplificazione.conX.ContainsKey(item.Key))
                        m_Semplificazione.conX[item.Key] += item.Value;
                    else
                        m_Semplificazione.conX[item.Key] = item.Value;
                }
                m_Semplificazione.Costanti = s.Costanti + de.Costanti;
                return m_Semplificazione;
            }
            if (d is ExprParser.SottrazioneEspressioneContext Po)
            {
                var s = semplifica(Po.expr(0));
                var de = semplifica(Po.expr(1));
                foreach (var item in s.conX.Concat(de.conX))
                {
                    if (m_Semplificazione.conX.ContainsKey(item.Key))
                        m_Semplificazione.conX[item.Key] -= item.Value;
                    else
                        m_Semplificazione.conX[item.Key] = item.Value;
                }
                m_Semplificazione.Costanti = s.Costanti - de.Costanti;
                return m_Semplificazione;
            }
            if (d is ExprParser.MoltiplicazioneEspressioneContext M)
            {
                var s = semplifica(M.expr(0));
                var de = semplifica(M.expr(1));

                var nX = s.conX.Values.Concat(de.conX.Values).Aggregate((x, z) => x * z);
                var n = ((s.Costanti != 0) ? s.Costanti : 1) * ((de.Costanti != 0) ? de.Costanti : 1);
                var e = s.conX.Keys.Concat(de.conX.Keys).Select(x => int.Parse(x)).Sum();
                m_Semplificazione.conX[e.ToString()] = nX * n;
                if (e.ToString() == "0")
                    m_Semplificazione.Costanti = s.Costanti * de.Costanti;
                return m_Semplificazione;
            }
            if (d is ExprParser.DivisioneEspressioneContext D)
            {
                var s = semplifica(D.expr(0));
                var de = semplifica(D.expr(1));

                var nX = s.conX.Values.Concat(de.conX.Values).Aggregate((x, z) => x * (1 / z));
                var n = ((s.Costanti != 0) ? s.Costanti : 1) * (1 / ((de.Costanti != 0) ? de.Costanti : 1));
                var e = s.conX.Keys.Concat(de.conX.Keys).Select(x => int.Parse(x)).Sum();
                m_Semplificazione.conX[e.ToString()] = nX * n;
                if (e.ToString() == "0")

                    m_Semplificazione.Costanti = n;

                return m_Semplificazione;
            }
            return m_Semplificazione;
        }
    }
    class Semplificazione
    {
        public double Costanti;
        public Dictionary<string, double> conX = new Dictionary<string, double>();
    }

}
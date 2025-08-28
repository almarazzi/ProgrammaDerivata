using Antlr4.Runtime.Misc;
using org.matheval;





namespace ProgrammaDerivate
{

    public class Ascolto : ExprBaseVisitor<string>
    {
        private readonly Expression m_expression = new Expression();
        private bool DivisioneB = false;

        public override string VisitDerivataEspressione([NotNull] ExprParser.DerivataEspressioneContext context)
        {
            var Espressione = context.expr();
            return Derivata(Espressione);


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
    }
}
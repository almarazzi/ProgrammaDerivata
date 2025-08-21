using Antlr4.Runtime.Misc;
using org.matheval;




namespace ProgrammaDerivate
{

    public class Ascolto : ExprBaseVisitor<string>
    {
        private readonly Expression m_expression = new Expression();

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

            if (Espressione is ExprParser.PotenzaEspressioneContext Potenza)
            {
                var Base = Potenza.expr(0);
                m_expression.SetFomular(Potenza.expr(1).GetText());
                var Esponente = m_expression.Eval<double>();
                var FattoreNum = 1.0;
                if (Base is ExprParser.MoltiplicazioneEspressioneContext MoltiplicazioneC)
                {
                    FattoreNum = double.Parse(MoltiplicazioneC.expr(0).GetText());

                } else if (Base is ExprParser.DivisioneEspressioneContext DivisioneC)
                {
                    FattoreNum = double.Parse(DivisioneC.expr(0).GetText());
                    Esponente = -Esponente;
                }
           
                return $"{FattoreNum* Esponente}*x^{Esponente - 1.0}";
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

                return $"({Derivata(Moltiplicazione.expr(0))})*{Moltiplicazione.expr(1).GetText()} + {Moltiplicazione.expr(0).GetText()}*({Derivata(Moltiplicazione.expr(1))})";
            }
            if (Espressione is ExprParser.DivisioneEspressioneContext Divisione)
            {

                return $"(({Derivata(Divisione.expr(0))})*{Divisione.expr(1).GetText()} - {Divisione.expr(0).GetText()}*({Derivata(Divisione.expr(1))}))/({Divisione.expr(1).GetText()})^2";
            }
            return "";
        }


    }
}

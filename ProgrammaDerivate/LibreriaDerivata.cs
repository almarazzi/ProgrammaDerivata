using org.matheval;
using System.Text;



namespace ProgrammaDerivate
{

    internal class Ascolto : ExprBaseListener
    {
        private readonly StringBuilder sb = new StringBuilder();
        private readonly Expression M_Expression = new Expression();
        private double M_FattoreNumerico;
        private double M_Potenza;
        private string M_Costante = "";
        private bool M_Divizione = false;


        public override void EnterDivPer(ExprParser.DivPerContext context)
        {

            M_Expression.SetFomular(context.numeri().GetText());
            M_FattoreNumerico = M_Expression.Eval<double>();
            if (context.GetChild(1).GetText() == "/")
            {
                M_Divizione = true;
            }
            
        }

        public override void EnterPotenza(ExprParser.PotenzaContext context)
        {
            if (M_Divizione)
            {
                M_Expression.SetFomular(context.numeri().GetText() + "*-1");
            }
            else
            {
                M_Expression.SetFomular(context.numeri().GetText());
            }
            M_Potenza = M_Expression.Eval<double>();
            M_Costante = context.ID().GetText();
            Potenza();
        }



        internal void Potenza()
        {
            var p = M_Potenza - 1.0;
            var d = M_Potenza * M_FattoreNumerico;
            sb.AppendLine($"y'={d}{M_Costante}^{p}");

        }

        public string Ritornostringa()
        {
            return sb.ToString();
        }
    }
}

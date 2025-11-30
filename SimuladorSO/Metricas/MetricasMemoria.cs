namespace SimuladorSO.Metricas
{
    public class MetricasMemoria
    {
        public int FaltasPagina { get; set; }
        public int AcessosMemoria { get; set; }
        public int HitsTLB { get; set; }
        public int MissesTLB { get; set; }

        public MetricasMemoria()
        {
            FaltasPagina = 0;
            AcessosMemoria = 0;
            HitsTLB = 0;
            MissesTLB = 0;
        }

        public double CalcularTaxaFaltaPagina()
        {
            return AcessosMemoria > 0 ? (double)FaltasPagina / AcessosMemoria * 100 : 0;
        }

        public double CalcularTaxaAcertoTLB()
        {
            int total = HitsTLB + MissesTLB;
            return total > 0 ? (double)HitsTLB / total * 100 : 0;
        }

        public override string ToString()
        {
            return $"Faltas de Página: {FaltasPagina} ({CalcularTaxaFaltaPagina():F2}%), TLB: {CalcularTaxaAcertoTLB():F2}% acerto";
        }
    }
}
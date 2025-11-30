namespace SimuladorSO.Metricas
{
    public class MetricasDispositivo
    {
        public string NomeDispositivo { get; set; }
        public int TempoOcupado { get; set; }
        public int TempoTotal { get; set; }
        public int NumeroRequisicoes { get; set; }

        public MetricasDispositivo(string nomeDispositivo)
        {
            NomeDispositivo = nomeDispositivo;
            TempoOcupado = 0;
            TempoTotal = 0;
            NumeroRequisicoes = 0;
        }

        public double CalcularUtilizacao()
        {
            return TempoTotal > 0 ? (double)TempoOcupado / TempoTotal * 100 : 0;
        }

        public override string ToString()
        {
            return $"{NomeDispositivo}: Utilização={CalcularUtilizacao():F2}%, Requisições={NumeroRequisicoes}";
        }
    }
}

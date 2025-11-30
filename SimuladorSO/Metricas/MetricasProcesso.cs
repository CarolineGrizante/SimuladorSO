namespace SimuladorSO.Metricas
{
    public class MetricasProcesso
    {
        public string PIDSimbolico { get; set; }
        public int TempoRetorno { get; set; }
        public int TempoEspera { get; set; }
        public int TempoResposta { get; set; }
        public int TempoCPU { get; set; }

        public MetricasProcesso(string pidSimbolico)
        {
            PIDSimbolico = pidSimbolico;
            TempoRetorno = 0;
            TempoEspera = 0;
            TempoResposta = 0;
            TempoCPU = 0;
        }

        public override string ToString()
        {
            return $"{PIDSimbolico}: Retorno={TempoRetorno}, Espera={TempoEspera}, Resposta={TempoResposta}, CPU={TempoCPU}";
        }
    }
}
namespace SimuladorSO.EntradaSaida
{
    public class RequisicaoES
    {
        public string PIDProcesso { get; set; }
        public string NomeDispositivo { get; set; }
        public int TempoRequisitado { get; set; }
        public int TempoRestante { get; set; }
        public bool Bloqueante { get; set; }
        public int TempoInicio { get; set; }
        public int TempoFim { get; set; }

        public RequisicaoES(string pidProcesso, string nomeDispositivo, int tempoRequisitado, bool bloqueante)
        {
            PIDProcesso = pidProcesso;
            NomeDispositivo = nomeDispositivo;
            TempoRequisitado = tempoRequisitado;
            TempoRestante = tempoRequisitado;
            Bloqueante = bloqueante;
            TempoInicio = -1;
            TempoFim = -1;
        }

        public override string ToString()
        {
            return $"Req: {PIDProcesso} -> {NomeDispositivo} ({TempoRestante}/{TempoRequisitado} ticks)";
        }
    }
}
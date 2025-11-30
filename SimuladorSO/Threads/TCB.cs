namespace SimuladorSO.Threads
{
    public class TCB
    {
        public int TID { get; set; }
        public int PIDProcesso { get; set; }
        public EstadoThread Estado { get; set; }
        public int ContadorPrograma { get; set; }
        public int TempoCPU { get; set; }
        public int TempoChegada { get; set; }

        public TCB(int tid, int pidProcesso, int tempoChegada)
        {
            TID = tid;
            PIDProcesso = pidProcesso;
            Estado = EstadoThread.Novo;
            ContadorPrograma = 0;
            TempoCPU = 0;
            TempoChegada = tempoChegada;
        }

        public override string ToString()
        {
            return $"TID: {TID} | Processo: {PIDProcesso} | Estado: {Estado} | CPU: {TempoCPU}";
        }
    }
}
namespace SimuladorSO.Threads
{
    public class ThreadSimulada
    {
        public TCB TCB { get; private set; }

        public ThreadSimulada(int tid, int pidProcesso, int tempoChegada)
        {
            TCB = new TCB(tid, pidProcesso, tempoChegada);
        }

        public void MudarEstado(EstadoThread novoEstado)
        {
            TCB.Estado = novoEstado;
        }

        public void ExecutarCiclo()
        {
            TCB.TempoCPU++;
            TCB.ContadorPrograma++;
        }
    }
}
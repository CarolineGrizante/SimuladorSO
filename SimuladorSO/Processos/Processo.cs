namespace SimuladorSO.Processos
{
    public class Processo
    {
        public PCB PCB { get; private set; }

        public Processo(int pid, string pidSimbolico, int prioridade, int tempoChegada)
        {
            PCB = new PCB(pid, pidSimbolico, prioridade, tempoChegada);
        }

        public void MudarEstado(EstadoProcesso novoEstado)
        {
            PCB.Estado = novoEstado;
        }

        public void ExecutarCiclo()
        {
            PCB.TempoCPU++;
            PCB.ContadorPrograma++;
        }

        public void AbrirArquivo(string caminho)
        {
            if (!PCB.ArquivosAbertos.Contains(caminho))
            {
                PCB.ArquivosAbertos.Add(caminho);
            }
        }

        public void FecharArquivo(string caminho)
        {
            PCB.ArquivosAbertos.Remove(caminho);
        }
    }
}

using SimuladorSO.Processos;

namespace SimuladorSO.Escalonamento
{
    public class PrioridadePreemptivo : IAlgoritmoEscalonamento
    {
        public string Nome => "Prioridade (Preemptivo)";

        public Processo? SelecionarProximoProcesso(FilaProntos fila)
        {
            // Seleciona o processo com maior prioridade (menor número = maior prioridade)
            var processos = fila.ObterTodos();

            if (processos.Count == 0)
                return null;

            return processos.OrderBy(p => p.PCB.Prioridade)
                           .ThenBy(p => p.PCB.TempoChegada)
                           .FirstOrDefault();
        }
    }
}

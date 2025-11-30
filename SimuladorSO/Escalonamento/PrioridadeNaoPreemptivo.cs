using SimuladorSO.Processos;

namespace SimuladorSO.Escalonamento
{
    public class PrioridadeNaoPreemptivo : IAlgoritmoEscalonamento
    {
        public string Nome => "Prioridade (Não Preemptivo)";

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

using SimuladorSO.Processos;

namespace SimuladorSO.Escalonamento
{
    public class RoundRobin : IAlgoritmoEscalonamento
    {
        public string Nome => "Round Robin";

        public Processo? SelecionarProximoProcesso(FilaProntos fila)
        {
            // Round Robin seleciona o primeiro processo da fila
            // A rotação é feita pelo escalonador ao recolocar o processo no final da fila
            return fila.ObterPrimeiro();
        }
    }
}

using SimuladorSO.Processos;
using System.Diagnostics;

namespace SimuladorSO.Escalonamento
{
    public class FCFS : IAlgoritmoEscalonamento
    {
        public string Nome => "FCFS (First-Come, First-Served)";

        public Processo? SelecionarProximoProcesso(FilaProntos fila)
        {
            // FCFS seleciona o primeiro processo da fila (ordem de chegada)
            return fila.ObterPrimeiro();
        }
    }
}

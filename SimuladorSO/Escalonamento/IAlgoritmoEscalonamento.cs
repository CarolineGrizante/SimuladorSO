using SimuladorSO.Processos;

namespace SimuladorSO.Escalonamento
{
    public interface IAlgoritmoEscalonamento
    {
        Processo? SelecionarProximoProcesso(FilaProntos fila);
        string Nome { get; }
    }
}

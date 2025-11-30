using System.Collections.Generic;

namespace SimuladorSO.Processos
{
    public class PCB
    {
        public int PID { get; set; }
        public string PIDSimbolico { get; set; }
        public EstadoProcesso Estado { get; set; }
        public int Prioridade { get; set; }
        public int ContadorPrograma { get; set; }
        public int TempoChegada { get; set; }
        public int TempoInicio { get; set; }
        public int TempoFinalizacao { get; set; }
        public int TempoCPU { get; set; }
        public int TempoEspera { get; set; }
        public int QuantumRestante { get; set; }

        // Tabela de páginas
        public int TabelaPaginasID { get; set; }

        // Arquivos abertos
        public List<string> ArquivosAbertos { get; set; }

        // Threads do processo
        public List<int> ThreadsIDs { get; set; }

        public PCB(int pid, string pidSimbolico, int prioridade, int tempoChegada)
        {
            PID = pid;
            PIDSimbolico = pidSimbolico;
            Estado = EstadoProcesso.Novo;
            Prioridade = prioridade;
            ContadorPrograma = 0;
            TempoChegada = tempoChegada;
            TempoInicio = -1;
            TempoFinalizacao = -1;
            TempoCPU = 0;
            TempoEspera = 0;
            QuantumRestante = 0;
            TabelaPaginasID = -1;
            ArquivosAbertos = new List<string>();
            ThreadsIDs = new List<int>();
        }

        public override string ToString()
        {
            return $"PID: {PID} ({PIDSimbolico}) | Estado: {Estado} | Prioridade: {Prioridade} | CPU: {TempoCPU}";
        }
    }
}
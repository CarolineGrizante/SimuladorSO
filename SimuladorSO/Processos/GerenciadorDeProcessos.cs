using SimuladorSO.Nucleo;
using SimuladorSO.Utilitarios;

namespace SimuladorSO.Processos
{
    public class GerenciadorDeProcessos
    {
        private Kernel _kernel;
        private Dictionary<int, Processo> _processos;
        private Dictionary<string, int> _mapeamentoPID;

        public GerenciadorDeProcessos(Kernel kernel)
        {
            _kernel = kernel;
            _processos = new Dictionary<int, Processo>();
            _mapeamentoPID = new Dictionary<string, int>();
        }

        public Processo CriarProcesso(string pidSimbolico, int prioridade)
        {
            int pid = GeradorIDs.GerarPID();
            int tempoChegada = _kernel.Relogio.TempoAtual;

            Processo processo = new Processo(pid, pidSimbolico, prioridade, tempoChegada);
            processo.MudarEstado(EstadoProcesso.Pronto);

            _processos[pid] = processo;
            _mapeamentoPID[pidSimbolico] = pid;

            _kernel.RegistradorEventos.RegistrarEvento($"Processo criado: {pidSimbolico} (PID={pid}, Prioridade={prioridade})");

            return processo;
        }

        public void RemoverProcesso(int pid)
        {
            if (_processos.ContainsKey(pid))
            {
                Processo processo = _processos[pid];
                _mapeamentoPID.Remove(processo.PCB.PIDSimbolico);
                _processos.Remove(pid);

                _kernel.RegistradorEventos.RegistrarEvento($"Processo removido: PID={pid}");
            }
        }

        public void FinalizarProcesso(string pidSimbolico)
        {
            if (_mapeamentoPID.ContainsKey(pidSimbolico))
            {
                int pid = _mapeamentoPID[pidSimbolico];
                Processo processo = _processos[pid];

                processo.MudarEstado(EstadoProcesso.Finalizado);
                processo.PCB.TempoFinalizacao = _kernel.Relogio.TempoAtual;

                _kernel.RegistradorEventos.RegistrarEvento($"Processo finalizado: {pidSimbolico} (PID={pid})");
            }
        }

        public void MudarEstado(int pid, EstadoProcesso novoEstado)
        {
            if (_processos.ContainsKey(pid))
            {
                _processos[pid].MudarEstado(novoEstado);
                _kernel.RegistradorEventos.RegistrarEvento($"Processo PID={pid} mudou para estado: {novoEstado}");
            }
        }

        public Processo? ObterProcesso(int pid)
        {
            return _processos.ContainsKey(pid) ? _processos[pid] : null;
        }

        public Processo? ObterProcessoPorSimbolico(string pidSimbolico)
        {
            if (_mapeamentoPID.ContainsKey(pidSimbolico))
            {
                int pid = _mapeamentoPID[pidSimbolico];
                return _processos[pid];
            }
            return null;
        }

        public List<Processo> ListarProcessos()
        {
            return _processos.Values.ToList();
        }

        public List<Processo> ListarProcessosPorEstado(EstadoProcesso estado)
        {
            return _processos.Values.Where(p => p.PCB.Estado == estado).ToList();
        }

        public void ExibirPCB(int pid)
        {
            if (_processos.ContainsKey(pid))
            {
                Processo processo = _processos[pid];
                PCB pcb = processo.PCB;

                Console.WriteLine("\n========== PCB COMPLETO ==========");
                Console.WriteLine($"PID: {pcb.PID}");
                Console.WriteLine($"PID Simbólico: {pcb.PIDSimbolico}");
                Console.WriteLine($"Estado: {pcb.Estado}");
                Console.WriteLine($"Prioridade: {pcb.Prioridade}");
                Console.WriteLine($"Contador de Programa: {pcb.ContadorPrograma}");
                Console.WriteLine($"Tempo de Chegada: {pcb.TempoChegada}");
                Console.WriteLine($"Tempo de Início: {pcb.TempoInicio}");
                Console.WriteLine($"Tempo de Finalização: {pcb.TempoFinalizacao}");
                Console.WriteLine($"Tempo de CPU: {pcb.TempoCPU}");
                Console.WriteLine($"Tempo de Espera: {pcb.TempoEspera}");
                Console.WriteLine($"Quantum Restante: {pcb.QuantumRestante}");
                Console.WriteLine($"Tabela de Páginas ID: {pcb.TabelaPaginasID}");
                Console.WriteLine($"Arquivos Abertos: {string.Join(", ", pcb.ArquivosAbertos)}");
                Console.WriteLine($"Threads: {string.Join(", ", pcb.ThreadsIDs)}");
                Console.WriteLine("==================================\n");
            }
            else
            {
                Console.WriteLine($"Processo PID={pid} não encontrado.");
            }
        }
    }
}
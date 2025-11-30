using SimuladorSO.Nucleo;
using SimuladorSO.Processos;

namespace SimuladorSO.Escalonamento
{
    public class Escalonador
    {
        private Kernel _kernel;
        private FilaProntos _filaProntos;
        private IAlgoritmoEscalonamento _algoritmo;
        private TrocaDeContexto _trocaContexto;
        private Processo? _processoAtual;

        public FilaProntos FilaProntos => _filaProntos;
        public TrocaDeContexto TrocaContexto => _trocaContexto;
        public Processo? ProcessoAtual => _processoAtual;
        public IAlgoritmoEscalonamento Algoritmo => _algoritmo;
        public Escalonador(Kernel kernel)
        {
            _kernel = kernel;
            _filaProntos = new FilaProntos();
            _algoritmo = new RoundRobin(); // Algoritmo padrão
            _trocaContexto = new TrocaDeContexto();
            _processoAtual = null;
        }

        public void TrocarAlgoritmo(string nomeAlgoritmo)
        {
            switch (nomeAlgoritmo.ToUpper())
            {
                case "FCFS":
                    _algoritmo = new FCFS();
                    break;
                case "RR":
                    _algoritmo = new RoundRobin();
                    break;
                case "PRIORIDADE_PREEMPTIVO":
                    _algoritmo = new PrioridadePreemptivo();
                    break;
                case "PRIORIDADE_NAO_PREEMPTIVO":
                    _algoritmo = new PrioridadeNaoPreemptivo();
                    break;
                default:
                    Console.WriteLine($"Algoritmo desconhecido: {nomeAlgoritmo}");
                    return;
            }

            _kernel.RegistradorEventos.RegistrarEvento($"Algoritmo de escalonamento alterado para: {_algoritmo.Nome}");
        }

        public void AdicionarProcessoNaFila(Processo processo)
        {
            if (processo.PCB.Estado == EstadoProcesso.Pronto)
            {
                _filaProntos.Adicionar(processo);
            }
        }

        public void ExecutarCiclo()
        {
            // Adicionar todos os processos prontos à fila
            var processosProntos = _kernel.GerenciadorProcessos.ListarProcessosPorEstado(EstadoProcesso.Pronto);
            foreach (var processo in processosProntos)
            {
                AdicionarProcessoNaFila(processo);
            }

            // Se não há processo em execução, selecionar um novo
            if (_processoAtual == null || _processoAtual.PCB.Estado != EstadoProcesso.Executando)
            {
                SelecionarProximoProcesso();
            }

            // Executar o processo atual
            if (_processoAtual != null && _processoAtual.PCB.Estado == EstadoProcesso.Executando)
            {
                _processoAtual.ExecutarCiclo();

                if (_processoAtual.PCB.TempoInicio == -1)
                {
                    _processoAtual.PCB.TempoInicio = _kernel.Relogio.TempoAtual;
                }

                _kernel.Relogio.Tick();

                // Decrementar quantum se for Round Robin
                if (_algoritmo is RoundRobin)
                {
                    _processoAtual.PCB.QuantumRestante--;

                    if (_processoAtual.PCB.QuantumRestante <= 0)
                    {
                        // Quantum esgotado, retornar à fila
                        _processoAtual.MudarEstado(EstadoProcesso.Pronto);
                        _filaProntos.Adicionar(_processoAtual);
                        _processoAtual = null;
                    }
                }

                // Verificar preempção para prioridade preemptiva
                if (_algoritmo is PrioridadePreemptivo)
                {
                    var processoMaiorPrioridade = _algoritmo.SelecionarProximoProcesso(_filaProntos);

                    if (processoMaiorPrioridade != null &&
                        processoMaiorPrioridade.PCB.Prioridade < _processoAtual.PCB.Prioridade)
                    {
                        // Preemptar processo atual
                        _processoAtual.MudarEstado(EstadoProcesso.Pronto);
                        _filaProntos.Adicionar(_processoAtual);
                        _processoAtual = null;
                    }
                }
            }
            else
            {
                _kernel.Relogio.Tick();
            }
        }

        public void ExecutarCiclos(int quantidade)
        {
            for (int i = 0; i < quantidade; i++)
            {
                ExecutarCiclo();
            }
        }

        public void ExecutarAteFinalizarTodos()
        {
            int maxIteracoes = 10000;
            int iteracoes = 0;

            while (iteracoes < maxIteracoes)
            {
                var processosAtivos = _kernel.GerenciadorProcessos.ListarProcessos()
                    .Where(p => p.PCB.Estado != EstadoProcesso.Finalizado)
                    .ToList();

                if (processosAtivos.Count == 0)
                    break;

                ExecutarCiclo();
                iteracoes++;
            }

            if (iteracoes >= maxIteracoes)
            {
                Console.WriteLine("Atenção: Limite de iterações atingido.");
            }
        }

        private void SelecionarProximoProcesso()
        {
            Processo? proximoProcesso = _algoritmo.SelecionarProximoProcesso(_filaProntos);

            if (proximoProcesso != null)
            {
                _filaProntos.Remover(proximoProcesso);

                if (_processoAtual != null && _processoAtual.PCB.PID != proximoProcesso.PCB.PID)
                {
                    _trocaContexto.RegistrarTroca();
                }

                _processoAtual = proximoProcesso;
                _processoAtual.MudarEstado(EstadoProcesso.Executando);
                _processoAtual.PCB.QuantumRestante = _kernel.Configuracoes.Quantum;

                _kernel.RegistradorEventos.RegistrarEvento(
                    $"Processo selecionado: PID={_processoAtual.PCB.PID} ({_processoAtual.PCB.PIDSimbolico})"
                );
            }
        }

        public void LimparProcessoAtual()
        {
            _processoAtual = null;
        }

        public void MostrarFilaProntos()
        {
            Console.WriteLine("\n===== FILA DE PRONTOS =====");
            var processos = _filaProntos.ObterTodos();

            if (processos.Count == 0)
            {
                Console.WriteLine("Fila vazia.");
            }
            else
            {
                foreach (var processo in processos)
                {
                    Console.WriteLine(processo.PCB.ToString());
                }
            }
            Console.WriteLine("===========================\n");
        }
    }
}
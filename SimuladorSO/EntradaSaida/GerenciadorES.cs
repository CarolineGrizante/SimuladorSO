using SimuladorSO.Nucleo;
using SimuladorSO.Processos;

namespace SimuladorSO.EntradaSaida
{
    public class GerenciadorES
    {
        private Kernel _kernel;
        private Dictionary<string, IDispositivo> _dispositivos;
        private Dictionary<string, Queue<RequisicaoES>> _filasDispositivos;
        private List<Interrupcao> _interrupcoes;

        public GerenciadorES(Kernel kernel)
        {
            _kernel = kernel;
            _dispositivos = new Dictionary<string, IDispositivo>();
            _filasDispositivos = new Dictionary<string, Queue<RequisicaoES>>();
            _interrupcoes = new List<Interrupcao>();

            // Criar dispositivos padrão
            CriarDispositivo("DISCO", "bloco", 30);
            CriarDispositivo("TECLADO", "caractere", 10);
            CriarDispositivo("IMPRESSORA", "bloco", 40);
        }

        public void CriarDispositivo(string nome, string tipo, int tempoOperacao)
        {
            IDispositivo dispositivo;

            if (tipo.ToLower() == "bloco")
            {
                dispositivo = new DispositivoDeBloco(nome, tempoOperacao);
            }
            else
            {
                dispositivo = new DispositivoDeCaractere(nome, tempoOperacao);
            }

            _dispositivos[nome] = dispositivo;
            _filasDispositivos[nome] = new Queue<RequisicaoES>();

            _kernel.RegistradorEventos.RegistrarEvento($"Dispositivo criado: {nome} (Tipo: {tipo})");
        }

        public void CriarRequisicao(string pidSimbolico, string nomeDispositivo, int tempo, bool bloqueante = true)
        {
            if (!_dispositivos.ContainsKey(nomeDispositivo))
            {
                Console.WriteLine($"Dispositivo {nomeDispositivo} não encontrado.");
                return;
            }

            Processo? processo = _kernel.GerenciadorProcessos.ObterProcessoPorSimbolico(pidSimbolico);

            if (processo == null)
            {
                Console.WriteLine($"Processo {pidSimbolico} não encontrado.");
                return;
            }

            RequisicaoES requisicao = new RequisicaoES(pidSimbolico, nomeDispositivo, tempo, bloqueante);
            _filasDispositivos[nomeDispositivo].Enqueue(requisicao);

            if (bloqueante)
            {
                processo.MudarEstado(EstadoProcesso.Bloqueado);
            }

            _kernel.RegistradorEventos.RegistrarEvento(
                $"Requisição I/O criada: {pidSimbolico} -> {nomeDispositivo} ({tempo} ticks)"
            );
        }

        public void ProcessarTick()
        {
            foreach (var dispositivo in _dispositivos.Values)
            {
                // Se dispositivo está ocupado, processar operação
                if (dispositivo.Ocupado)
                {
                    dispositivo.ProcessarTick();

                    // Verificar se operação finalizou
                    RequisicaoES? requisicaoFinalizada = dispositivo.FinalizarOperacao();

                    if (requisicaoFinalizada != null)
                    {
                        requisicaoFinalizada.TempoFim = _kernel.Relogio.TempoAtual;

                        // Gerar interrupção
                        GerarInterrupcao("I/O", dispositivo.Nome,
                            $"Operação finalizada para {requisicaoFinalizada.PIDProcesso}");

                        // Desbloquear processo se for bloqueante
                        if (requisicaoFinalizada.Bloqueante)
                        {
                            Processo? processo = _kernel.GerenciadorProcessos.ObterProcessoPorSimbolico(
                                requisicaoFinalizada.PIDProcesso);

                            if (processo != null && processo.PCB.Estado == EstadoProcesso.Bloqueado)
                            {
                                processo.MudarEstado(EstadoProcesso.Pronto);
                            }
                        }
                    }
                }
                // Se dispositivo está livre e há requisições na fila, iniciar próxima
                else if (_filasDispositivos[dispositivo.Nome].Count > 0)
                {
                    RequisicaoES proximaRequisicao = _filasDispositivos[dispositivo.Nome].Dequeue();
                    proximaRequisicao.TempoInicio = _kernel.Relogio.TempoAtual;
                    dispositivo.IniciarOperacao(proximaRequisicao);

                    _kernel.RegistradorEventos.RegistrarEvento(
                        $"I/O iniciado: {proximaRequisicao.PIDProcesso} em {dispositivo.Nome}"
                    );
                }
            }
        }

        public void ProcessarTicks(int quantidade)
        {
            for (int i = 0; i < quantidade; i++)
            {
                ProcessarTick();
                _kernel.Relogio.Tick();
            }
        }

        private void GerarInterrupcao(string tipo, string origem, string mensagem)
        {
            Interrupcao interrupcao = new Interrupcao(tipo, origem, mensagem, _kernel.Relogio.TempoAtual);
            _interrupcoes.Add(interrupcao);
            _kernel.RegistradorEventos.RegistrarEvento($"INTERRUPÇÃO: {interrupcao.Mensagem}");
        }

        public void ListarDispositivos()
        {
            Console.WriteLine("\n===== DISPOSITIVOS I/O =====");

            foreach (var dispositivo in _dispositivos.Values)
            {
                string status = dispositivo.Ocupado ? "OCUPADO" : "LIVRE";
                int filaSize = _filasDispositivos[dispositivo.Nome].Count;

                Console.WriteLine($"{dispositivo.Nome}: {status} | Fila: {filaSize} requisições");
            }

            Console.WriteLine("============================\n");
        }

        public void MostrarFilasDispositivos()
        {
            Console.WriteLine("\n===== FILAS DE DISPOSITIVOS =====");

            foreach (var kvp in _filasDispositivos)
            {
                Console.WriteLine($"\n{kvp.Key}:");

                if (kvp.Value.Count == 0)
                {
                    Console.WriteLine("  (vazia)");
                }
                else
                {
                    foreach (var req in kvp.Value)
                    {
                        Console.WriteLine($"  {req}");
                    }
                }
            }

            Console.WriteLine("\n=================================\n");
        }

        public void MostrarInterrupcoes()
        {
            Console.WriteLine("\n===== INTERRUPÇÕES GERADAS =====");

            if (_interrupcoes.Count == 0)
            {
                Console.WriteLine("Nenhuma interrupção gerada.");
            }
            else
            {
                foreach (var interrupcao in _interrupcoes)
                {
                    Console.WriteLine(interrupcao.ToString());
                }
            }

            Console.WriteLine("================================\n");
        }
    }
}

using System;
using System.IO;

namespace SimuladorSO.Nucleo
{
    public class CarregadorWorkload
    {
        private Kernel _kernel;

        public CarregadorWorkload(Kernel kernel)
        {
            _kernel = kernel;
        }

        public void CarregarArquivo(string caminhoArquivo)
        {
            if (!File.Exists(caminhoArquivo))
            {
                Console.WriteLine($"Arquivo não encontrado: {caminhoArquivo}");
                return;
            }

            Console.WriteLine($"\n=== Carregando workload: {caminhoArquivo} ===\n");

            string[] linhas = File.ReadAllLines(caminhoArquivo);

            foreach (string linha in linhas)
            {
                string linhaLimpa = linha.Trim();

                // Ignorar linhas vazias e comentários
                if (string.IsNullOrEmpty(linhaLimpa) || linhaLimpa.StartsWith("#"))
                    continue;

                ProcessarComando(linhaLimpa);
            }

            Console.WriteLine("\n=== Workload carregado com sucesso ===\n");
        }

        private void ProcessarComando(string comando)
        {
            string[] partes = comando.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (partes.Length == 0)
                return;

            string cmd = partes[0].ToUpper();

            try
            {
                switch (cmd)
                {
                    case "SET_SEED":
                        if (partes.Length >= 2)
                            _kernel.Configuracoes.Seed = int.Parse(partes[1]);
                        break;

                    case "SET_QUANTUM":
                        if (partes.Length >= 2)
                            _kernel.Configuracoes.Quantum = int.Parse(partes[1]);
                        break;

                    case "SET_ESCALONADOR":
                        if (partes.Length >= 2)
                        {
                            _kernel.Configuracoes.AlgoritmoEscalonamento = partes[1];
                            _kernel.Escalonador.TrocarAlgoritmo(partes[1]);
                        }
                        break;

                    case "SET_TAMANHO_PAGINA":
                        if (partes.Length >= 2)
                            _kernel.Configuracoes.TamanhoPagina = int.Parse(partes[1]);
                        break;

                    case "SET_FRAMES":
                        if (partes.Length >= 2)
                            _kernel.Configuracoes.NumeroMolduras = int.Parse(partes[1]);
                        break;

                    case "CRIAR_PROCESSO":
                        if (partes.Length >= 3)
                        {
                            string pid = partes[1];
                            int prioridade = int.Parse(partes[2]);
                            _kernel.GerenciadorProcessos.CriarProcesso(pid, prioridade);
                        }
                        break;

                    case "CRIAR_THREAD":
                        if (partes.Length >= 2)
                        {
                            string pidProcesso = partes[1];
                            _kernel.GerenciadorThreads.CriarThread(pidProcesso);
                        }
                        break;

                    case "MEM_ALOCAR":
                        if (partes.Length >= 3)
                        {
                            string pid = partes[1];
                            int tamanho = int.Parse(partes[2]);
                            _kernel.GerenciadorMemoria.AlocarMemoria(pid, tamanho);
                        }
                        break;

                    case "MEM_ACESSO":
                        if (partes.Length >= 3)
                        {
                            string pid = partes[1];
                            int endereco = Convert.ToInt32(partes[2], 16);
                            _kernel.GerenciadorMemoria.AcessarMemoria(pid, endereco);
                        }
                        break;

                    case "CPU_TICK":
                        if (partes.Length >= 2)
                        {
                            int ticks = int.Parse(partes[1]);
                            _kernel.Escalonador.ExecutarCiclos(ticks);
                        }
                        break;

                    case "IO_REQ":
                        if (partes.Length >= 4)
                        {
                            string pid = partes[1];
                            string dispositivo = partes[2];
                            int tempo = int.Parse(partes[3]);
                            _kernel.GerenciadorES.CriarRequisicao(pid, dispositivo, tempo);
                        }
                        break;

                    case "IO_TICK":
                        if (partes.Length >= 2)
                        {
                            int ticks = int.Parse(partes[1]);
                            _kernel.GerenciadorES.ProcessarTicks(ticks);
                        }
                        break;

                    case "ARQ_CRIAR":
                        if (partes.Length >= 2)
                        {
                            string caminho = partes[1];
                            _kernel.SistemaArquivos.CriarArquivo(caminho);
                        }
                        break;

                    case "ARQ_ESCREVER":
                        if (partes.Length >= 4)
                        {
                            string pid = partes[1];
                            string caminho = partes[2];
                            int tamanho = int.Parse(partes[3]);
                            _kernel.SistemaArquivos.EscreverArquivo(pid, caminho, tamanho);
                        }
                        break;

                    case "ARQ_LER":
                        if (partes.Length >= 4)
                        {
                            string pid = partes[1];
                            string caminho = partes[2];
                            int tamanho = int.Parse(partes[3]);
                            _kernel.SistemaArquivos.LerArquivo(pid, caminho, tamanho);
                        }
                        break;

                    case "ARQ_ABRIR":
                        if (partes.Length >= 3)
                        {
                            string pid = partes[1];
                            string caminho = partes[2];
                            _kernel.SistemaArquivos.AbrirArquivo(pid, caminho);
                        }
                        break;

                    case "ARQ_FECHAR":
                        if (partes.Length >= 3)
                        {
                            string pid = partes[1];
                            string caminho = partes[2];
                            _kernel.SistemaArquivos.FecharArquivo(pid, caminho);
                        }
                        break;

                    case "ARQ_APAGAR":
                        if (partes.Length >= 2)
                        {
                            string caminho = partes[1];
                            _kernel.SistemaArquivos.ApagarArquivo(caminho);
                        }
                        break;

                    case "FINALIZAR":
                        if (partes.Length >= 2)
                        {
                            string pid = partes[1];
                            _kernel.GerenciadorProcessos.FinalizarProcesso(pid);
                        }
                        break;

                    case "GERAR_METRICAS":
                        _kernel.GerenciadorMetricas.ExibirTodasMetricas();
                        break;

                    default:
                        Console.WriteLine($"Comando desconhecido: {cmd}");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar comando '{comando}': {ex.Message}");
            }
        }
    }
}
using SimuladorSO.Nucleo;
using SimuladorSO.Processos;

namespace SimuladorSO.Interface
{
    public class MenuProcessos
    {
        private Kernel _kernel;

        public MenuProcessos(Kernel kernel)
        {
            _kernel = kernel;
        }

        public void Executar()
        {
            bool continuar = true;

            while (continuar)
            {
                ExibirMenu();
                string? opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        CriarProcesso();
                        break;
                    case "2":
                        ListarProcessos();
                        break;
                    case "3":
                        MudarEstadoProcesso();
                        break;
                    case "4":
                        RemoverProcesso();
                        break;
                    case "5":
                        VerPCB();
                        break;
                    case "6":
                        AbrirArquivo();
                        break;
                    case "7":
                        FecharArquivo();
                        break;
                    case "0":
                        continuar = false;
                        break;
                    default:
                        Console.WriteLine("\nOpção inválida!");
                        break;
                }
            }
        }

        private void ExibirMenu()
        {
            Console.WriteLine("\n------------- GERENCIAR PROCESSOS -------------");
            Console.WriteLine("1) Criar novo processo");
            Console.WriteLine("2) Listar processos (e estados)");
            Console.WriteLine("3) Mudar estado de processo (bloquear, prontificar, finalizar)");
            Console.WriteLine("4) Remover processo");
            Console.WriteLine("5) Ver PCB completo de um processo");
            Console.WriteLine("6) Abrir arquivo dentro de um processo");
            Console.WriteLine("7) Fechar arquivo dentro de um processo");
            Console.WriteLine("0) Voltar");
            Console.WriteLine("------------------------------------------------");
            Console.Write("Escolha uma opção: ");
        }

        private void CriarProcesso()
        {
            Console.Write("\nPID simbólico: ");
            string? pid = Console.ReadLine();

            Console.Write("Prioridade (0-10): ");
            if (int.TryParse(Console.ReadLine(), out int prioridade))
            {
                _kernel.GerenciadorProcessos.CriarProcesso(pid ?? "P", prioridade);
                Console.WriteLine("Processo criado com sucesso!");
            }
            else
            {
                Console.WriteLine("Prioridade inválida!");
            }
        }

        private void ListarProcessos()
        {
            Console.WriteLine("\n===== LISTA DE PROCESSOS =====");
            var processos = _kernel.GerenciadorProcessos.ListarProcessos();

            if (processos.Count == 0)
            {
                Console.WriteLine("Nenhum processo criado.");
            }
            else
            {
                foreach (var processo in processos)
                {
                    Console.WriteLine(processo.PCB.ToString());
                }
            }
            Console.WriteLine("==============================\n");
        }

        private void MudarEstadoProcesso()
        {
            Console.Write("\nPID do processo: ");
            if (int.TryParse(Console.ReadLine(), out int pid))
            {
                Console.WriteLine("Estados: 0=Novo, 1=Pronto, 2=Executando, 3=Bloqueado, 4=Finalizado");
                Console.Write("Novo estado: ");

                if (int.TryParse(Console.ReadLine(), out int estado) && estado >= 0 && estado <= 4)
                {
                    _kernel.GerenciadorProcessos.MudarEstado(pid, (EstadoProcesso)estado);
                    Console.WriteLine("Estado alterado com sucesso!");
                }
                else
                {
                    Console.WriteLine("Estado inválido!");
                }
            }
            else
            {
                Console.WriteLine("PID inválido!");
            }
        }

        private void RemoverProcesso()
        {
            Console.Write("\nPID do processo: ");
            if (int.TryParse(Console.ReadLine(), out int pid))
            {
                _kernel.GerenciadorProcessos.RemoverProcesso(pid);
                Console.WriteLine("Processo removido!");
            }
            else
            {
                Console.WriteLine("PID inválido!");
            }
        }

        private void VerPCB()
        {
            Console.Write("\nPID do processo: ");
            if (int.TryParse(Console.ReadLine(), out int pid))
            {
                _kernel.GerenciadorProcessos.ExibirPCB(pid);
            }
            else
            {
                Console.WriteLine("PID inválido!");
            }
        }

        private void AbrirArquivo()
        {
            Console.Write("\nPID simbólico do processo: ");
            string? pid = Console.ReadLine();

            Console.Write("Caminho do arquivo: ");
            string? caminho = Console.ReadLine();

            if (!string.IsNullOrEmpty(pid) && !string.IsNullOrEmpty(caminho))
            {
                _kernel.SistemaArquivos.AbrirArquivo(pid, caminho);
            }
        }

        private void FecharArquivo()
        {
            Console.Write("\nPID simbólico do processo: ");
            string? pid = Console.ReadLine();

            Console.Write("Caminho do arquivo: ");
            string? caminho = Console.ReadLine();

            if (!string.IsNullOrEmpty(pid) && !string.IsNullOrEmpty(caminho))
            {
                _kernel.SistemaArquivos.FecharArquivo(pid, caminho);
            }
        }
    }
}
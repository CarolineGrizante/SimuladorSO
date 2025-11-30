using SimuladorSO.Nucleo;
using SimuladorSO.Threads;

namespace SimuladorSO.Interface
{
    public class MenuThreads
    {
        private Kernel _kernel;

        public MenuThreads(Kernel kernel)
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
                        CriarThread();
                        break;
                    case "2":
                        ListarThreads();
                        break;
                    case "3":
                        MudarEstadoThread();
                        break;
                    case "4":
                        RemoverThread();
                        break;
                    case "5":
                        VerTCB();
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
            Console.WriteLine("\n------------- GERENCIAR THREADS ----------------");
            Console.WriteLine("1) Criar thread em um processo");
            Console.WriteLine("2) Listar threads");
            Console.WriteLine("3) Mudar estado da thread");
            Console.WriteLine("4) Remover thread");
            Console.WriteLine("5) Ver TCB completo");
            Console.WriteLine("0) Voltar");
            Console.WriteLine("-------------------------------------------------");
            Console.Write("Escolha uma opção: ");
        }

        private void CriarThread()
        {
            Console.Write("\nPID simbólico do processo: ");
            string? pid = Console.ReadLine();

            if (!string.IsNullOrEmpty(pid))
            {
                _kernel.GerenciadorThreads.CriarThread(pid);
                Console.WriteLine("Thread criada com sucesso!");
            }
        }

        private void ListarThreads()
        {
            Console.WriteLine("\n===== LISTA DE THREADS =====");
            var threads = _kernel.GerenciadorThreads.ListarThreads();

            if (threads.Count == 0)
            {
                Console.WriteLine("Nenhuma thread criada.");
            }
            else
            {
                foreach (var thread in threads)
                {
                    Console.WriteLine(thread.TCB.ToString());
                }
            }
            Console.WriteLine("============================\n");
        }

        private void MudarEstadoThread()
        {
            Console.Write("\nTID da thread: ");
            if (int.TryParse(Console.ReadLine(), out int tid))
            {
                Console.WriteLine("Estados: 0=Novo, 1=Pronto, 2=Executando, 3=Bloqueado, 4=Finalizado");
                Console.Write("Novo estado: ");

                if (int.TryParse(Console.ReadLine(), out int estado) && estado >= 0 && estado <= 4)
                {
                    _kernel.GerenciadorThreads.MudarEstado(tid, (EstadoThread)estado);
                    Console.WriteLine("Estado alterado com sucesso!");
                }
                else
                {
                    Console.WriteLine("Estado inválido!");
                }
            }
        }

        private void RemoverThread()
        {
            Console.Write("\nTID da thread: ");
            if (int.TryParse(Console.ReadLine(), out int tid))
            {
                _kernel.GerenciadorThreads.RemoverThread(tid);
                Console.WriteLine("Thread removida!");
            }
        }

        private void VerTCB()
        {
            Console.Write("\nTID da thread: ");
            if (int.TryParse(Console.ReadLine(), out int tid))
            {
                _kernel.GerenciadorThreads.ExibirTCB(tid);
            }
        }
    }
}
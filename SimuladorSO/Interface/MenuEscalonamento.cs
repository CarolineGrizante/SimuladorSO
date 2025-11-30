using SimuladorSO.Nucleo;

namespace SimuladorSO.Interface
{
    public class MenuEscalonamento
    {
        private Kernel _kernel;

        public MenuEscalonamento(Kernel kernel)
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
                        TrocarAlgoritmo();
                        break;
                    case "2":
                        ConfigurarQuantum();
                        break;
                    case "3":
                        _kernel.Escalonador.ExecutarCiclo();
                        Console.WriteLine("1 ciclo de CPU executado.");
                        break;
                    case "4":
                        _kernel.Escalonador.ExecutarAteFinalizarTodos();
                        Console.WriteLine("Todos os processos foram finalizados.");
                        break;
                    case "5":
                        _kernel.Escalonador.MostrarFilaProntos();
                        break;
                    case "6":
                        MostrarTrocasContexto();
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
            Console.WriteLine("\n----------------- ESCALONADOR ------------------");
            Console.WriteLine("1) Trocar algoritmo de escalonamento");
            Console.WriteLine("    a) FCFS");
            Console.WriteLine("    b) Round Robin");
            Console.WriteLine("    c) Prioridades (preemptivo)");
            Console.WriteLine("    d) Prioridades (não preemptivo)");
            Console.WriteLine("2) Configurar quantum");
            Console.WriteLine("3) Executar 1 ciclo de CPU");
            Console.WriteLine("4) Executar até todos finalizarem");
            Console.WriteLine("5) Mostrar fila de prontos");
            Console.WriteLine("6) Ver contagem de trocas de contexto");
            Console.WriteLine("0) Voltar");
            Console.WriteLine("-------------------------------------------------");
            Console.Write("Escolha uma opção: ");
        }

        private void TrocarAlgoritmo()
        {
            Console.WriteLine("\nAlgoritmos disponíveis:");
            Console.WriteLine("a) FCFS");
            Console.WriteLine("b) RR (Round Robin)");
            Console.WriteLine("c) PRIORIDADE_PREEMPTIVO");
            Console.WriteLine("d) PRIORIDADE_NAO_PREEMPTIVO");
            Console.Write("Escolha: ");

            string? escolha = Console.ReadLine();

            string algoritmo = escolha?.ToLower() switch
            {
                "a" => "FCFS",
                "b" => "RR",
                "c" => "PRIORIDADE_PREEMPTIVO",
                "d" => "PRIORIDADE_NAO_PREEMPTIVO",
                _ => ""
            };

            if (!string.IsNullOrEmpty(algoritmo))
            {
                _kernel.Escalonador.TrocarAlgoritmo(algoritmo);
            }
            else
            {
                Console.WriteLine("Opção inválida!");
            }
        }

        private void ConfigurarQuantum()
        {
            Console.Write("\nNovo valor de quantum: ");
            if (int.TryParse(Console.ReadLine(), out int quantum) && quantum > 0)
            {
                _kernel.Configuracoes.Quantum = quantum;
                Console.WriteLine($"Quantum configurado para {quantum}.");
            }
            else
            {
                Console.WriteLine("Valor inválido!");
            }
        }

        private void MostrarTrocasContexto()
        {
            Console.WriteLine($"\n===== TROCAS DE CONTEXTO =====");
            Console.WriteLine($"Número de trocas: {_kernel.Escalonador.TrocaContexto.ContadorTrocas}");
            Console.WriteLine($"Sobrecarga total: {_kernel.Escalonador.TrocaContexto.SobrecargaTotal} ticks");
            Console.WriteLine($"==============================\n");
        }
    }
}

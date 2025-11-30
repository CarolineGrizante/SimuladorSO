using SimuladorSO.Nucleo;

namespace SimuladorSO.Interface
{
    public class MenuMemoria
    {
        private Kernel _kernel;

        public MenuMemoria(Kernel kernel)
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
                        MostrarTabelaPaginas();
                        break;
                    case "2":
                        AlocarMemoria();
                        break;
                    case "3":
                        LiberarMemoria();
                        break;
                    case "4":
                        _kernel.GerenciadorMemoria.MostrarMapaMolduras();
                        break;
                    case "5":
                        AlternarTLB();
                        break;
                    case "6":
                        _kernel.GerenciadorMemoria.MostrarEstatisticasTLB();
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
            Console.WriteLine("\n------------------ MEMÓRIA ----------------------");
            Console.WriteLine("1) Mostrar tabela de páginas / segmentos");
            Console.WriteLine("2) Alocar memória para processo");
            Console.WriteLine("3) Liberar memória");
            Console.WriteLine("4) Mostrar mapa de molduras");
            Console.WriteLine("5) Ativar/Desativar TLB simulada");
            Console.WriteLine("6) Ver estatísticas de faltas de página");
            Console.WriteLine("0) Voltar");
            Console.WriteLine("-------------------------------------------------");
            Console.Write("Escolha uma opção: ");
        }

        private void MostrarTabelaPaginas()
        {
            Console.Write("\nPID simbólico do processo: ");
            string? pid = Console.ReadLine();

            if (!string.IsNullOrEmpty(pid))
            {
                _kernel.GerenciadorMemoria.MostrarTabelaPaginas(pid);
            }
        }

        private void AlocarMemoria()
        {
            Console.Write("\nPID simbólico do processo: ");
            string? pid = Console.ReadLine();

            Console.Write("Tamanho em bytes: ");
            if (int.TryParse(Console.ReadLine(), out int tamanho) && !string.IsNullOrEmpty(pid))
            {
                _kernel.GerenciadorMemoria.AlocarMemoria(pid, tamanho);
                Console.WriteLine("Memória alocada!");
            }
        }

        private void LiberarMemoria()
        {
            Console.Write("\nPID simbólico do processo: ");
            string? pid = Console.ReadLine();

            if (!string.IsNullOrEmpty(pid))
            {
                _kernel.GerenciadorMemoria.LiberarMemoria(pid);
                Console.WriteLine("Memória liberada!");
            }
        }

        private void AlternarTLB()
        {
            _kernel.Configuracoes.TLBAtivada = !_kernel.Configuracoes.TLBAtivada;
            string status = _kernel.Configuracoes.TLBAtivada ? "ATIVADA" : "DESATIVADA";
            Console.WriteLine($"\nTLB {status}");
        }
    }
}

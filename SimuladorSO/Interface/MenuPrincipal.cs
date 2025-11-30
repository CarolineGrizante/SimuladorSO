using SimuladorSO.Nucleo;

namespace SimuladorSO.Interface
{
    public class MenuPrincipal
    {
        private Kernel _kernel;
        private MenuProcessos _menuProcessos;
        private MenuThreads _menuThreads;
        private MenuEscalonamento _menuEscalonamento;
        private MenuMemoria _menuMemoria;
        private MenuES _menuES;
        private MenuArquivo _menuArquivo;
        private MenuMetricas _menuMetricas;
        private MenuConfiguracoes _menuConfiguracoes;

        public MenuPrincipal(Kernel kernel)
        {
            _kernel = kernel;
            _menuProcessos = new MenuProcessos(kernel);
            _menuThreads = new MenuThreads(kernel);
            _menuEscalonamento = new MenuEscalonamento(kernel);
            _menuMemoria = new MenuMemoria(kernel);
            _menuES = new MenuES(kernel);
            _menuArquivo = new MenuArquivo(kernel);
            _menuMetricas = new MenuMetricas(kernel);
            _menuConfiguracoes = new MenuConfiguracoes(kernel);
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
                        _menuProcessos.Executar();
                        break;
                    case "2":
                        _menuThreads.Executar();
                        break;
                    case "3":
                        _menuEscalonamento.Executar();
                        break;
                    case "4":
                        _menuMemoria.Executar();
                        break;
                    case "5":
                        _menuES.Executar();
                        break;
                    case "6":
                        _menuArquivo.Executar();
                        break;
                    case "7":
                        _menuMetricas.Executar();
                        break;
                    case "8":
                        _menuConfiguracoes.Executar();
                        break;
                    case "0":
                        continuar = false;
                        Console.WriteLine("\nEncerrando simulador...");
                        break;
                    default:
                        Console.WriteLine("\nOpção inválida!");
                        break;
                }
            }
        }

        private void ExibirMenu()
        {
            Console.WriteLine("\n=============== SISTEMA OPERACIONAL – SIMULADOR ===============");
            Console.WriteLine("1) Gerenciar Processos");
            Console.WriteLine("2) Gerenciar Threads");
            Console.WriteLine("3) Escalonador de CPU");
            Console.WriteLine("4) Gerenciamento de Memória");
            Console.WriteLine("5) Entrada e Saída (I/O)");
            Console.WriteLine("6) Sistema de Arquivos");
            Console.WriteLine("7) Estatísticas e Métricas");
            Console.WriteLine("8) Configurações do Simulador");
            Console.WriteLine("0) Sair");
            Console.WriteLine("===============================================================");
            Console.Write("Escolha uma opção: ");
        }
    }
}

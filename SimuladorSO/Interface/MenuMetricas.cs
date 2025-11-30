using SimuladorSO.Nucleo;

namespace SimuladorSO.Interface
{
    public class MenuMetricas
    {
        private Kernel _kernel;

        public MenuMetricas(Kernel kernel)
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
                        _kernel.GerenciadorMetricas.ExibirTempoRetorno();
                        break;
                    case "2":
                        _kernel.GerenciadorMetricas.ExibirTempoEspera();
                        break;
                    case "3":
                        _kernel.GerenciadorMetricas.ExibirTempoResposta();
                        break;
                    case "4":
                        _kernel.GerenciadorMetricas.ExibirUtilizacaoCPU();
                        break;
                    case "5":
                        Console.WriteLine("\nFuncionalidade de utilização por dispositivo em desenvolvimento.");
                        break;
                    case "6":
                        _kernel.GerenciadorMetricas.ExibirThroughput();
                        break;
                    case "7":
                        _kernel.GerenciadorMetricas.ExibirTrocasContexto();
                        break;
                    case "8":
                        _kernel.GerenciadorMetricas.ExibirTrocasContexto();
                        break;
                    case "9":
                        ExportarLog();
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
            Console.WriteLine("\n------------------ ESTATÍSTICAS ----------------------");
            Console.WriteLine("1) Tempo de retorno por processo");
            Console.WriteLine("2) Tempo de espera em pronto");
            Console.WriteLine("3) Tempo de resposta");
            Console.WriteLine("4) Utilização da CPU");
            Console.WriteLine("5) Utilização por dispositivo");
            Console.WriteLine("6) Throughput");
            Console.WriteLine("7) Número de trocas de contexto");
            Console.WriteLine("8) Sobrecarga total do escalonamento");
            Console.WriteLine("9) Exportar log completo");
            Console.WriteLine("0) Voltar");
            Console.WriteLine("------------------------------------------------------");
            Console.Write("Escolha uma opção: ");
        }

        private void ExportarLog()
        {
            Console.Write("\nCaminho do arquivo (ex: log.txt): ");
            string? caminho = Console.ReadLine();

            if (!string.IsNullOrEmpty(caminho))
            {
                _kernel.GerenciadorMetricas.ExportarLog(caminho);
            }
        }
    }
}
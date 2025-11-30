using SimuladorSO.Nucleo;
using SimuladorSO.Utilitarios;

namespace SimuladorSO.Interface
{
    public class MenuConfiguracoes
    {
        private Kernel _kernel;

        public MenuConfiguracoes(Kernel kernel)
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
                        DefinirSemente();
                        break;
                    case "2":
                        ConfigurarTamanhoPagina();
                        break;
                    case "3":
                        ConfigurarNumeroMolduras();
                        break;
                    case "4":
                        ConfigurarTemposDispositivos();
                        break;
                    case "5":
                        CarregarWorkload();
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
            Console.WriteLine("\n------------------ CONFIGURAÇÕES ----------------------");
            Console.WriteLine("1) Definir semente determinística");
            Console.WriteLine("2) Configurar tamanho de página");
            Console.WriteLine("3) Configurar número de molduras");
            Console.WriteLine("4) Configurar tempos de dispositivos");
            Console.WriteLine("5) Carregar workload");
            Console.WriteLine("0) Voltar");
            Console.WriteLine("------------------------------------------------------");
            Console.Write("Escolha uma opção: ");
        }

        private void DefinirSemente()
        {
            Console.Write("\nSemente: ");
            if (int.TryParse(Console.ReadLine(), out int semente))
            {
                _kernel.Configuracoes.Seed = semente;
                GeradorAleatorio.DefinirSemente(semente);
                Console.WriteLine($"Semente definida para {semente}.");
            }
        }

        private void ConfigurarTamanhoPagina()
        {
            Console.Write("\nTamanho da página (bytes): ");
            if (int.TryParse(Console.ReadLine(), out int tamanho) && tamanho > 0)
            {
                _kernel.Configuracoes.TamanhoPagina = tamanho;
                Console.WriteLine($"Tamanho de página configurado para {tamanho} bytes.");
            }
        }

        private void ConfigurarNumeroMolduras()
        {
            Console.Write("\nNúmero de molduras: ");
            if (int.TryParse(Console.ReadLine(), out int numero) && numero > 0)
            {
                _kernel.Configuracoes.NumeroMolduras = numero;
                Console.WriteLine($"Número de molduras configurado para {numero}.");
            }
        }

        private void ConfigurarTemposDispositivos()
        {
            Console.WriteLine("\nConfigurar tempos de dispositivos:");

            Console.Write("Tempo DISCO (ticks): ");
            if (int.TryParse(Console.ReadLine(), out int tempoDisco))
            {
                _kernel.Configuracoes.TempoDisco = tempoDisco;
            }

            Console.Write("Tempo TECLADO (ticks): ");
            if (int.TryParse(Console.ReadLine(), out int tempoTeclado))
            {
                _kernel.Configuracoes.TempoTeclado = tempoTeclado;
            }

            Console.Write("Tempo IMPRESSORA (ticks): ");
            if (int.TryParse(Console.ReadLine(), out int tempoImpressora))
            {
                _kernel.Configuracoes.TempoImpressora = tempoImpressora;
            }

            Console.WriteLine("Tempos configurados!");
        }

        private void CarregarWorkload()
        {
            Console.Write("\nCaminho do arquivo de workload: ");
            string? caminho = Console.ReadLine();

            if (!string.IsNullOrEmpty(caminho))
            {
                _kernel.CarregadorWorkload.CarregarArquivo(caminho);
            }
        }
    }
}

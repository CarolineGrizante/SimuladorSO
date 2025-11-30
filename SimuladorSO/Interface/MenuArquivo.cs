using SimuladorSO.Nucleo;

namespace SimuladorSO.Interface
{
    public class MenuArquivo
    {
        private Kernel _kernel;

        public MenuArquivo(Kernel kernel)
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
                        _kernel.SistemaArquivos.ListarDiretorio();
                        break;
                    case "2":
                        CriarArquivo();
                        break;
                    case "3":
                        CriarDiretorio();
                        break;
                    case "4":
                        AbrirArquivo();
                        break;
                    case "5":
                        LerArquivo();
                        break;
                    case "6":
                        EscreverArquivo();
                        break;
                    case "7":
                        FecharArquivo();
                        break;
                    case "8":
                        ApagarArquivo();
                        break;
                    case "9":
                        MudarDiretorio();
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
            Console.WriteLine("\n---------------- SISTEMA DE ARQUIVOS ----------------");
            Console.WriteLine("1) Listar diretório atual");
            Console.WriteLine("2) Criar arquivo");
            Console.WriteLine("3) Criar diretório");
            Console.WriteLine("4) Abrir arquivo");
            Console.WriteLine("5) Ler arquivo");
            Console.WriteLine("6) Escrever arquivo");
            Console.WriteLine("7) Fechar arquivo");
            Console.WriteLine("8) Apagar arquivo");
            Console.WriteLine("9) Mudar diretório");
            Console.WriteLine("0) Voltar");
            Console.WriteLine("------------------------------------------------------");
            Console.Write("Escolha uma opção: ");
        }

        private void CriarArquivo()
        {
            Console.Write("\nCaminho do arquivo (ex: /docs/teste.txt): ");
            string? caminho = Console.ReadLine();

            if (!string.IsNullOrEmpty(caminho))
            {
                _kernel.SistemaArquivos.CriarArquivo(caminho);
            }
        }

        private void CriarDiretorio()
        {
            Console.Write("\nNome do diretório: ");
            string? nome = Console.ReadLine();

            if (!string.IsNullOrEmpty(nome))
            {
                _kernel.SistemaArquivos.CriarDiretorio(nome);
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

        private void LerArquivo()
        {
            Console.Write("\nPID simbólico do processo: ");
            string? pid = Console.ReadLine();

            Console.Write("Caminho do arquivo: ");
            string? caminho = Console.ReadLine();

            Console.Write("Tamanho a ler (bytes): ");
            if (int.TryParse(Console.ReadLine(), out int tamanho) &&
                !string.IsNullOrEmpty(pid) && !string.IsNullOrEmpty(caminho))
            {
                _kernel.SistemaArquivos.LerArquivo(pid, caminho, tamanho);
            }
        }

        private void EscreverArquivo()
        {
            Console.Write("\nPID simbólico do processo: ");
            string? pid = Console.ReadLine();

            Console.Write("Caminho do arquivo: ");
            string? caminho = Console.ReadLine();

            Console.Write("Tamanho a escrever (bytes): ");
            if (int.TryParse(Console.ReadLine(), out int tamanho) &&
                !string.IsNullOrEmpty(pid) && !string.IsNullOrEmpty(caminho))
            {
                _kernel.SistemaArquivos.EscreverArquivo(pid, caminho, tamanho);
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

        private void ApagarArquivo()
        {
            Console.Write("\nCaminho do arquivo: ");
            string? caminho = Console.ReadLine();

            if (!string.IsNullOrEmpty(caminho))
            {
                _kernel.SistemaArquivos.ApagarArquivo(caminho);
            }
        }

        private void MudarDiretorio()
        {
            Console.Write("\nNome do diretório (ou '..' para voltar): ");
            string? nome = Console.ReadLine();

            if (!string.IsNullOrEmpty(nome))
            {
                _kernel.SistemaArquivos.MudarDiretorio(nome);
            }
        }
    }
}
using SimuladorSO.Nucleo;

namespace SimuladorSO.Interface
{
    public class MenuES
    {
        private Kernel _kernel;

        public MenuES(Kernel kernel)
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
                        _kernel.GerenciadorES.ListarDispositivos();
                        break;
                    case "2":
                        CriarRequisicaoBloqueante();
                        break;
                    case "3":
                        CriarRequisicaoNaoBloqueante();
                        break;
                    case "4":
                        _kernel.GerenciadorES.ProcessarTick();
                        Console.WriteLine("1 tick de I/O processado.");
                        break;
                    case "5":
                        _kernel.GerenciadorES.MostrarFilasDispositivos();
                        break;
                    case "6":
                        _kernel.GerenciadorES.MostrarInterrupcoes();
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
            Console.WriteLine("\n------------------- DISPOSITIVOS I/O -------------------");
            Console.WriteLine("1) Listar dispositivos (bloco / caractere)");
            Console.WriteLine("2) Criar requisição bloqueante");
            Console.WriteLine("3) Criar requisição não bloqueante");
            Console.WriteLine("4) Processar 1 tick de I/O");
            Console.WriteLine("5) Ver filas de dispositivos");
            Console.WriteLine("6) Ver interrupções geradas");
            Console.WriteLine("0) Voltar");
            Console.WriteLine("---------------------------------------------------------");
            Console.Write("Escolha uma opção: ");
        }

        private void CriarRequisicaoBloqueante()
        {
            Console.Write("\nPID simbólico do processo: ");
            string? pid = Console.ReadLine();

            Console.Write("Dispositivo (DISCO, TECLADO, IMPRESSORA): ");
            string? dispositivo = Console.ReadLine();

            Console.Write("Tempo (ticks): ");
            if (int.TryParse(Console.ReadLine(), out int tempo) &&
                !string.IsNullOrEmpty(pid) && !string.IsNullOrEmpty(dispositivo))
            {
                _kernel.GerenciadorES.CriarRequisicao(pid, dispositivo.ToUpper(), tempo, true);
                Console.WriteLine("Requisição bloqueante criada!");
            }
        }

        private void CriarRequisicaoNaoBloqueante()
        {
            Console.Write("\nPID simbólico do processo: ");
            string? pid = Console.ReadLine();

            Console.Write("Dispositivo (DISCO, TECLADO, IMPRESSORA): ");
            string? dispositivo = Console.ReadLine();

            Console.Write("Tempo (ticks): ");
            if (int.TryParse(Console.ReadLine(), out int tempo) &&
                !string.IsNullOrEmpty(pid) && !string.IsNullOrEmpty(dispositivo))
            {
                _kernel.GerenciadorES.CriarRequisicao(pid, dispositivo.ToUpper(), tempo, false);
                Console.WriteLine("Requisição não bloqueante criada!");
            }
        }
    }
}
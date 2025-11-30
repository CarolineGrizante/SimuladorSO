using SimuladorSO.Nucleo;
using SimuladorSO.Interface;

namespace SimuladorSO
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Se houver argumento "teste", executar teste automatizado
            if (args.Length > 0 && args[0] == "teste")
            {
                ProgramaTeste.TestarWorkload();
            }
            else
            {
                // Modo interativo normal
                Kernel kernel = new Kernel();
                MenuPrincipal menu = new MenuPrincipal(kernel);

                menu.Executar();
            }
        }
    }
}
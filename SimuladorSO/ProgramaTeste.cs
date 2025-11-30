using SimuladorSO.Nucleo;

namespace SimuladorSO
{
    class ProgramaTeste
    {
        public static void TestarWorkload()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Kernel kernel = new Kernel();

            Console.WriteLine("╔════════════════════════════════════════════════╗");
            Console.WriteLine("║     TESTE DO SIMULADOR DE SO - WORKLOAD        ║");
            Console.WriteLine("╚════════════════════════════════════════════════╝\n");

            // Carregar workload
            Console.WriteLine("📂 Carregando workload_exemplo.txt...\n");
            kernel.CarregadorWorkload.CarregarArquivo("workload_exemplo.txt");

            Console.WriteLine("\n" + new string('═', 60));
            Console.WriteLine("RESUMO DOS PROCESSOS");
            Console.WriteLine(new string('═', 60) + "\n");

            // Exibir processos
            var processos = kernel.GerenciadorProcessos.ListarProcessos();

            if (processos.Count == 0)
            {
                Console.WriteLine("❌ ERRO: Nenhum processo encontrado no sistema!");
            }
            else
            {
                Console.WriteLine($"✅ Total de processos: {processos.Count}\n");

                foreach (var p in processos)
                {
                    Console.WriteLine($"Processo: {p.PCB.PIDSimbolico}");
                    Console.WriteLine($"  PID: {p.PCB.PID}");
                    Console.WriteLine($"  Estado: {p.PCB.Estado}");
                    Console.WriteLine($"  Prioridade: {p.PCB.Prioridade}");
                    Console.WriteLine($"  Tempo de CPU: {p.PCB.TempoCPU}");
                    Console.WriteLine($"  Tempo de Chegada: {p.PCB.TempoChegada}");
                    Console.WriteLine($"  Tempo de Início: {p.PCB.TempoInicio}");
                    Console.WriteLine($"  Tempo de Finalização: {p.PCB.TempoFinalizacao}");
                    Console.WriteLine();
                }
            }

            Console.WriteLine(new string('═', 60));
            Console.WriteLine("TEMPO DO SISTEMA");
            Console.WriteLine(new string('═', 60));
            Console.WriteLine($"Relógio do sistema: {kernel.Relogio.TempoAtual} ticks\n");

            Console.WriteLine(new string('═', 60));
            Console.WriteLine("TESTE CONCLUÍDO");
            Console.WriteLine(new string('═', 60));
        }


    }
}
using SimuladorSO.Processos;
using SimuladorSO.Threads;
using SimuladorSO.Escalonamento;
using SimuladorSO.Memoria;
using SimuladorSO.EntradaSaida;
using SimuladorSO.SistemaDeArquivos;
using SimuladorSO.Metricas;

namespace SimuladorSO.Nucleo
{
    public class Kernel
    {
        public Configuracoes Configuracoes { get; private set; }
        public Relogio Relogio { get; private set; }
        public RegistradorDeEventos RegistradorEventos { get; private set; }
        public CarregadorWorkload CarregadorWorkload { get; private set; }

        public GerenciadorDeProcessos GerenciadorProcessos { get; private set; }
        public GerenciadorDeThreads GerenciadorThreads { get; private set; }
        public Escalonador Escalonador { get; private set; }
        public GerenciadorDeMemoria GerenciadorMemoria { get; private set; }
        public GerenciadorES GerenciadorES { get; private set; }
        public SistemaDeArquivos.SistemaDeArquivos SistemaArquivos { get; private set; }
        public GerenciadorDeMetricas GerenciadorMetricas { get; private set; }

        public Kernel()
        {
            Configuracoes = new Configuracoes();
            Relogio = new Relogio();
            RegistradorEventos = new RegistradorDeEventos(Relogio);

            GerenciadorProcessos = new GerenciadorDeProcessos(this);
            GerenciadorThreads = new GerenciadorDeThreads(this);
            Escalonador = new Escalonador(this);
            GerenciadorMemoria = new GerenciadorDeMemoria(this);
            GerenciadorES = new GerenciadorES(this);
            SistemaArquivos = new SistemaDeArquivos.SistemaDeArquivos(this);
            GerenciadorMetricas = new GerenciadorDeMetricas(this);

            CarregadorWorkload = new CarregadorWorkload(this);
        }
    }
}

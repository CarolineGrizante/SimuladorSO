using System;
using System.Collections.Generic;
using System.Linq;
using SimuladorSO.Nucleo;
using SimuladorSO.Processos;

namespace SimuladorSO.Metricas
{
    public class GerenciadorDeMetricas
    {
        private Kernel _kernel;
        private Dictionary<string, MetricasProcesso> _metricasProcessos;
        private Dictionary<string, MetricasDispositivo> _metricasDispositivos;
        private MetricasMemoria _metricasMemoria;

        public GerenciadorDeMetricas(Kernel kernel)
        {
            _kernel = kernel;
            _metricasProcessos = new Dictionary<string, MetricasProcesso>();
            _metricasDispositivos = new Dictionary<string, MetricasDispositivo>();
            _metricasMemoria = new MetricasMemoria();
        }

        public void ColetarMetricas()
        {
            // Coletar métricas de processos
            var processos = _kernel.GerenciadorProcessos.ListarProcessos();

            foreach (var processo in processos)
            {
                string pid = processo.PCB.PIDSimbolico;

                if (!_metricasProcessos.ContainsKey(pid))
                {
                    _metricasProcessos[pid] = new MetricasProcesso(pid);
                }

                MetricasProcesso metricas = _metricasProcessos[pid];

                // Tempo de retorno = Tempo de finalização - Tempo de chegada
                if (processo.PCB.TempoFinalizacao > 0)
                {
                    metricas.TempoRetorno = processo.PCB.TempoFinalizacao - processo.PCB.TempoChegada;
                }

                // Tempo de espera = Tempo de retorno - Tempo de CPU
                if (metricas.TempoRetorno > 0)
                {
                    metricas.TempoEspera = metricas.TempoRetorno - processo.PCB.TempoCPU;
                    if (metricas.TempoEspera < 0) metricas.TempoEspera = 0;
                }

                // Tempo de resposta = Tempo de início - Tempo de chegada
                if (processo.PCB.TempoInicio > 0)
                {
                    metricas.TempoResposta = processo.PCB.TempoInicio - processo.PCB.TempoChegada;
                }

                metricas.TempoCPU = processo.PCB.TempoCPU;
            }

            // Coletar métricas de memória
            _metricasMemoria.FaltasPagina = _kernel.GerenciadorMemoria.FaltasPagina;
            _metricasMemoria.HitsTLB = _kernel.GerenciadorMemoria.TLB.Hits;
            _metricasMemoria.MissesTLB = _kernel.GerenciadorMemoria.TLB.Misses;
        }

        public void ExibirTempoRetorno()
        {
            ColetarMetricas();

            Console.WriteLine("\n===== TEMPO DE RETORNO POR PROCESSO =====");

            foreach (var metricas in _metricasProcessos.Values)
            {
                Console.WriteLine($"{metricas.PIDSimbolico}: {metricas.TempoRetorno} ticks");
            }

            if (_metricasProcessos.Count > 0)
            {
                double media = _metricasProcessos.Values.Average(m => m.TempoRetorno);
                Console.WriteLine($"\nMédia: {media:F2} ticks");
            }

            Console.WriteLine("=========================================\n");
        }

        public void ExibirTempoEspera()
        {
            ColetarMetricas();

            Console.WriteLine("\n===== TEMPO DE ESPERA EM PRONTO =====");

            foreach (var metricas in _metricasProcessos.Values)
            {
                Console.WriteLine($"{metricas.PIDSimbolico}: {metricas.TempoEspera} ticks");
            }

            if (_metricasProcessos.Count > 0)
            {
                double media = _metricasProcessos.Values.Average(m => m.TempoEspera);
                Console.WriteLine($"\nMédia: {media:F2} ticks");
            }

            Console.WriteLine("=====================================\n");
        }

        public void ExibirTempoResposta()
        {
            ColetarMetricas();

            Console.WriteLine("\n===== TEMPO DE RESPOSTA =====");

            foreach (var metricas in _metricasProcessos.Values)
            {
                Console.WriteLine($"{metricas.PIDSimbolico}: {metricas.TempoResposta} ticks");
            }

            if (_metricasProcessos.Count > 0)
            {
                double media = _metricasProcessos.Values.Average(m => m.TempoResposta);
                Console.WriteLine($"\nMédia: {media:F2} ticks");
            }

            Console.WriteLine("=============================\n");
        }

        public void ExibirUtilizacaoCPU()
        {
            int tempoTotal = _kernel.Relogio.TempoAtual;
            int tempoOcupado = _kernel.GerenciadorProcessos.ListarProcessos()
                .Sum(p => p.PCB.TempoCPU);

            double utilizacao = tempoTotal > 0 ? (double)tempoOcupado / tempoTotal * 100 : 0;

            Console.WriteLine("\n===== UTILIZAÇÃO DA CPU =====");
            Console.WriteLine($"Tempo Total: {tempoTotal} ticks");
            Console.WriteLine($"Tempo Ocupado: {tempoOcupado} ticks");
            Console.WriteLine($"Utilização: {utilizacao:F2}%");
            Console.WriteLine("=============================\n");
        }

        public void ExibirThroughput()
        {
            int tempoTotal = _kernel.Relogio.TempoAtual;
            int processosFinalizados = _kernel.GerenciadorProcessos.ListarProcessosPorEstado(EstadoProcesso.Finalizado).Count;

            double throughput = tempoTotal > 0 ? (double)processosFinalizados / tempoTotal : 0;

            Console.WriteLine("\n===== THROUGHPUT =====");
            Console.WriteLine($"Processos Finalizados: {processosFinalizados}");
            Console.WriteLine($"Tempo Total: {tempoTotal} ticks");
            Console.WriteLine($"Throughput: {throughput:F4} processos/tick");
            Console.WriteLine("======================\n");
        }

        public void ExibirTrocasContexto()
        {
            Console.WriteLine("\n===== TROCAS DE CONTEXTO =====");
            Console.WriteLine($"Número de Trocas: {_kernel.Escalonador.TrocaContexto.ContadorTrocas}");
            Console.WriteLine($"Sobrecarga Total: {_kernel.Escalonador.TrocaContexto.SobrecargaTotal} ticks");
            Console.WriteLine("==============================\n");
        }

        public void ExibirMetricasMemoria()
        {
            ColetarMetricas();

            Console.WriteLine("\n===== MÉTRICAS DE MEMÓRIA =====");
            Console.WriteLine($"Faltas de Página: {_metricasMemoria.FaltasPagina}");
            Console.WriteLine($"Hits TLB: {_metricasMemoria.HitsTLB}");
            Console.WriteLine($"Misses TLB: {_metricasMemoria.MissesTLB}");
            Console.WriteLine($"Taxa de Acerto TLB: {_metricasMemoria.CalcularTaxaAcertoTLB():F2}%");
            Console.WriteLine("===============================\n");
        }

        public void ExibirTodasMetricas()
        {
            Console.WriteLine("\n╔════════════════════════════════════════════════╗");
            Console.WriteLine("║        RELATÓRIO COMPLETO DE MÉTRICAS          ║");
            Console.WriteLine("╚════════════════════════════════════════════════╝\n");

            ExibirTempoRetorno();
            ExibirTempoEspera();
            ExibirTempoResposta();
            ExibirUtilizacaoCPU();
            ExibirThroughput();
            ExibirTrocasContexto();
            ExibirMetricasMemoria();
        }

        public void ExportarLog(string caminhoArquivo)
        {
            _kernel.RegistradorEventos.ExportarLog(caminhoArquivo);
        }
    }
}
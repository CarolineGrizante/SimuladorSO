using SimuladorSO.Nucleo;
using SimuladorSO.Processos;
using SimuladorSO.Utilitarios;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SimuladorSO.Threads
{
    public class GerenciadorDeThreads
    {
        private Kernel _kernel;
        private Dictionary<int, ThreadSimulada> _threads;

        public GerenciadorDeThreads(Kernel kernel)
        {
            _kernel = kernel;
            _threads = new Dictionary<int, ThreadSimulada>();
        }

        public ThreadSimulada? CriarThread(string pidSimbolico)
        {
            Processo? processo = _kernel.GerenciadorProcessos.ObterProcessoPorSimbolico(pidSimbolico);

            if (processo == null)
            {
                Console.WriteLine($"Processo {pidSimbolico} não encontrado.");
                return null;
            }

            int tid = GeradorIDs.GerarTID();
            int tempoChegada = _kernel.Relogio.TempoAtual;

            ThreadSimulada thread = new ThreadSimulada(tid, processo.PCB.PID, tempoChegada);
            thread.MudarEstado(EstadoThread.Pronto);

            _threads[tid] = thread;
            processo.PCB.ThreadsIDs.Add(tid);

            _kernel.RegistradorEventos.RegistrarEvento($"Thread criada: TID={tid} no processo {pidSimbolico} (PID={processo.PCB.PID})");

            return thread;
        }

        public void RemoverThread(int tid)
        {
            if (_threads.ContainsKey(tid))
            {
                ThreadSimulada thread = _threads[tid];

                // Remover da lista de threads do processo
                Processo? processo = _kernel.GerenciadorProcessos.ObterProcesso(thread.TCB.PIDProcesso);
                if (processo != null)
                {
                    processo.PCB.ThreadsIDs.Remove(tid);
                }

                _threads.Remove(tid);
                _kernel.RegistradorEventos.RegistrarEvento($"Thread removida: TID={tid}");
            }
        }

        public void MudarEstado(int tid, EstadoThread novoEstado)
        {
            if (_threads.ContainsKey(tid))
            {
                _threads[tid].MudarEstado(novoEstado);
                _kernel.RegistradorEventos.RegistrarEvento($"Thread TID={tid} mudou para estado: {novoEstado}");
            }
        }

        public ThreadSimulada? ObterThread(int tid)
        {
            return _threads.ContainsKey(tid) ? _threads[tid] : null;
        }

        public List<ThreadSimulada> ListarThreads()
        {
            return _threads.Values.ToList();
        }

        public List<ThreadSimulada> ListarThreadsDoProcesso(int pid)
        {
            return _threads.Values.Where(t => t.TCB.PIDProcesso == pid).ToList();
        }

        public void ExibirTCB(int tid)
        {
            if (_threads.ContainsKey(tid))
            {
                ThreadSimulada thread = _threads[tid];
                TCB tcb = thread.TCB;

                Console.WriteLine("\n========== TCB COMPLETO ==========");
                Console.WriteLine($"TID: {tcb.TID}");
                Console.WriteLine($"PID do Processo: {tcb.PIDProcesso}");
                Console.WriteLine($"Estado: {tcb.Estado}");
                Console.WriteLine($"Contador de Programa: {tcb.ContadorPrograma}");
                Console.WriteLine($"Tempo de CPU: {tcb.TempoCPU}");
                Console.WriteLine($"Tempo de Chegada: {tcb.TempoChegada}");
                Console.WriteLine("==================================\n");
            }
            else
            {
                Console.WriteLine($"Thread TID={tid} não encontrada.");
            }
        }
    }
}
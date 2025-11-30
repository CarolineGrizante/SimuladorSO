using System;
using System.Collections.Generic;
using System.IO;

namespace SimuladorSO.Nucleo
{
    public class RegistradorDeEventos
    {
        private List<string> _eventos;
        private Relogio _relogio;

        public RegistradorDeEventos(Relogio relogio)
        {
            _eventos = new List<string>();
            _relogio = relogio;
        }

        public void RegistrarEvento(string evento)
        {
            string eventoComTempo = $"[T={_relogio.TempoAtual}] {evento}";
            _eventos.Add(eventoComTempo);
            Console.WriteLine(eventoComTempo);
        }

        public void ExportarLog(string caminhoArquivo)
        {
            try
            {
                File.WriteAllLines(caminhoArquivo, _eventos);
                Console.WriteLine($"Log exportado para: {caminhoArquivo}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao exportar log: {ex.Message}");
            }
        }

        public List<string> ObterEventos()
        {
            return new List<string>(_eventos);
        }

        public void Limpar()
        {
            _eventos.Clear();
        }
    }
}
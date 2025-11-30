using System.Collections.Generic;
using System.Linq;

namespace SimuladorSO.Memoria
{
    public class TLB
    {
        private List<EntradaTLB> _entradas;
        private int _tamanhoMaximo;
        private int _hits;
        private int _misses;

        public int Hits => _hits;
        public int Misses => _misses;

        public TLB(int tamanho)
        {
            _tamanhoMaximo = tamanho;
            _entradas = new List<EntradaTLB>();
            _hits = 0;
            _misses = 0;
        }

        public int? Buscar(string pidProcesso, int numeroPagina)
        {
            var entrada = _entradas.FirstOrDefault(e =>
                e.PIDProcesso == pidProcesso && e.NumeroPagina == numeroPagina);

            if (entrada != null)
            {
                _hits++;
                return entrada.NumeroMoldura;
            }

            _misses++;
            return null;
        }

        public void Adicionar(string pidProcesso, int numeroPagina, int numeroMoldura, int tempoAtual)
        {
            // Remover entrada antiga se existir
            _entradas.RemoveAll(e => e.PIDProcesso == pidProcesso && e.NumeroPagina == numeroPagina);

            // Se TLB estiver cheia, remover entrada mais antiga (LRU)
            if (_entradas.Count >= _tamanhoMaximo)
            {
                var maisAntiga = _entradas.OrderBy(e => e.TempoAcesso).First();
                _entradas.Remove(maisAntiga);
            }

            // Adicionar nova entrada
            _entradas.Add(new EntradaTLB(pidProcesso, numeroPagina, numeroMoldura, tempoAtual));
        }

        public void Limpar()
        {
            _entradas.Clear();
        }

        public void LimparProcesso(string pidProcesso)
        {
            _entradas.RemoveAll(e => e.PIDProcesso == pidProcesso);
        }

        public double TaxaAcerto()
        {
            int total = _hits + _misses;
            return total > 0 ? (double)_hits / total * 100 : 0;
        }

        public List<EntradaTLB> ObterEntradas()
        {
            return new List<EntradaTLB>(_entradas);
        }
    }
}
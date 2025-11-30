using SimuladorSO.Processos;

namespace SimuladorSO.Escalonamento
{
    public class FilaProntos
    {
        private List<Processo> _fila;

        public FilaProntos()
        {
            _fila = new List<Processo>();
        }

        public void Adicionar(Processo processo)
        {
            if (!_fila.Contains(processo))
            {
                _fila.Add(processo);
            }
        }

        public void Remover(Processo processo)
        {
            _fila.Remove(processo);
        }

        public Processo? RemoverPrimeiro()
        {
            if (_fila.Count > 0)
            {
                Processo processo = _fila[0];
                _fila.RemoveAt(0);
                return processo;
            }
            return null;
        }

        public List<Processo> ObterTodos()
        {
            return new List<Processo>(_fila);
        }

        public int Tamanho()
        {
            return _fila.Count;
        }

        public bool EstaVazia()
        {
            return _fila.Count == 0;
        }

        public void Ordenar(IComparer<Processo> comparador)
        {
            _fila.Sort(comparador);
        }

        public Processo? ObterPrimeiro()
        {
            return _fila.Count > 0 ? _fila[0] : null;
        }
    }
}

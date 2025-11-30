using System.Collections.Generic;

namespace SimuladorSO.SistemaDeArquivos
{
    public class ManipuladorArquivo
    {
        private Dictionary<string, List<FCB>> _arquivosAbertos;

        public ManipuladorArquivo()
        {
            _arquivosAbertos = new Dictionary<string, List<FCB>>();
        }

        public void AbrirArquivo(string caminho, string pidProcesso, int modoAbertura)
        {
            if (!_arquivosAbertos.ContainsKey(caminho))
            {
                _arquivosAbertos[caminho] = new List<FCB>();
            }

            FCB fcb = new FCB(caminho, pidProcesso, modoAbertura);
            _arquivosAbertos[caminho].Add(fcb);
        }

        public void FecharArquivo(string caminho, string pidProcesso)
        {
            if (_arquivosAbertos.ContainsKey(caminho))
            {
                _arquivosAbertos[caminho].RemoveAll(fcb => fcb.PIDProcesso == pidProcesso);

                if (_arquivosAbertos[caminho].Count == 0)
                {
                    _arquivosAbertos.Remove(caminho);
                }
            }
        }

        public bool EstaAberto(string caminho)
        {
            return _arquivosAbertos.ContainsKey(caminho) && _arquivosAbertos[caminho].Count > 0;
        }

        public List<FCB> ObterArquivosAbertos()
        {
            List<FCB> todos = new List<FCB>();

            foreach (var lista in _arquivosAbertos.Values)
            {
                todos.AddRange(lista);
            }

            return todos;
        }
    }
}
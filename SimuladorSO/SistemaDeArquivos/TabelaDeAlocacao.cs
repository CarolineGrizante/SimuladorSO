using System.Collections.Generic;

namespace SimuladorSO.SistemaDeArquivos
{
    public class TabelaDeAlocacao
    {
        private Dictionary<string, List<int>> _blocos;

        public TabelaDeAlocacao()
        {
            _blocos = new Dictionary<string, List<int>>();
        }

        public void AlocarBlocos(string caminho, int quantidadeBlocos)
        {
            if (!_blocos.ContainsKey(caminho))
            {
                _blocos[caminho] = new List<int>();
            }

            // Simular alocação de blocos
            for (int i = 0; i < quantidadeBlocos; i++)
            {
                _blocos[caminho].Add(_blocos[caminho].Count);
            }
        }

        public void LiberarBlocos(string caminho)
        {
            if (_blocos.ContainsKey(caminho))
            {
                _blocos.Remove(caminho);
            }
        }

        public int ObterQuantidadeBlocos(string caminho)
        {
            return _blocos.ContainsKey(caminho) ? _blocos[caminho].Count : 0;
        }
    }
}
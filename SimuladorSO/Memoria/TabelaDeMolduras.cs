using System.Collections.Generic;
using System.Linq;

namespace SimuladorSO.Memoria
{
    public class TabelaDeMolduras
    {
        private Dictionary<int, Moldura> _molduras;

        public TabelaDeMolduras(int numeroMolduras)
        {
            _molduras = new Dictionary<int, Moldura>();

            for (int i = 0; i < numeroMolduras; i++)
            {
                _molduras[i] = new Moldura(i);
            }
        }

        public Moldura? AlocarMoldura(string pidProcesso, int numeroPagina, PoliticaAlocacao politica)
        {
            // Para simplificação, usamos First Fit
            var molduraLivre = _molduras.Values.FirstOrDefault(m => m.Livre);

            if (molduraLivre != null)
            {
                molduraLivre.Alocar(pidProcesso, numeroPagina);
                return molduraLivre;
            }

            return null;
        }

        public void LiberarMoldura(int numeroMoldura)
        {
            if (_molduras.ContainsKey(numeroMoldura))
            {
                _molduras[numeroMoldura].Liberar();
            }
        }

        public void LiberarMoldurasDoProcesso(string pidProcesso)
        {
            foreach (var moldura in _molduras.Values)
            {
                if (moldura.PIDProcesso == pidProcesso)
                {
                    moldura.Liberar();
                }
            }
        }

        public List<Moldura> ObterTodasMolduras()
        {
            return _molduras.Values.ToList();
        }

        public int ContarMoldurasLivres()
        {
            return _molduras.Values.Count(m => m.Livre);
        }
    }
}
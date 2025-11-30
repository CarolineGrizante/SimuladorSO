using System.Collections.Generic;

namespace SimuladorSO.Memoria
{
    public class TabelaDePaginas
    {
        public int TabelaID { get; set; }
        public string PIDProcesso { get; set; }
        public Dictionary<int, Pagina> Paginas { get; set; }

        public TabelaDePaginas(int tabelaID, string pidProcesso)
        {
            TabelaID = tabelaID;
            PIDProcesso = pidProcesso;
            Paginas = new Dictionary<int, Pagina>();
        }

        public Pagina ObterOuCriarPagina(int numeroPagina)
        {
            if (!Paginas.ContainsKey(numeroPagina))
            {
                Paginas[numeroPagina] = new Pagina(numeroPagina);
            }
            return Paginas[numeroPagina];
        }

        public Pagina? ObterPagina(int numeroPagina)
        {
            return Paginas.ContainsKey(numeroPagina) ? Paginas[numeroPagina] : null;
        }
    }
}

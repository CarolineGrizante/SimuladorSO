namespace SimuladorSO.Memoria
{
    public class Pagina
    {
        public int NumeroPagina { get; set; }
        public int NumeroMoldura { get; set; }
        public bool Presente { get; set; }
        public bool Modificada { get; set; }
        public bool Referenciada { get; set; }
        public int TempoAcesso { get; set; }

        public Pagina(int numeroPagina)
        {
            NumeroPagina = numeroPagina;
            NumeroMoldura = -1;
            Presente = false;
            Modificada = false;
            Referenciada = false;
            TempoAcesso = 0;
        }

        public override string ToString()
        {
            return $"Pág {NumeroPagina} -> Moldura {NumeroMoldura} | Presente: {Presente}";
        }
    }
}

namespace SimuladorSO.Memoria
{
    public class Moldura
    {
        public int NumeroMoldura { get; set; }
        public bool Livre { get; set; }
        public string? PIDProcesso { get; set; }
        public int NumeroPagina { get; set; }

        public Moldura(int numeroMoldura)
        {
            NumeroMoldura = numeroMoldura;
            Livre = true;
            PIDProcesso = null;
            NumeroPagina = -1;
        }

        public void Alocar(string pidProcesso, int numeroPagina)
        {
            Livre = false;
            PIDProcesso = pidProcesso;
            NumeroPagina = numeroPagina;
        }

        public void Liberar()
        {
            Livre = true;
            PIDProcesso = null;
            NumeroPagina = -1;
        }

        public override string ToString()
        {
            if (Livre)
                return $"Moldura {NumeroMoldura}: LIVRE";
            else
                return $"Moldura {NumeroMoldura}: {PIDProcesso} (Pág {NumeroPagina})";
        }
    }
}

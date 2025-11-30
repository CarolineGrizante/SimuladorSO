namespace SimuladorSO.Memoria
{
    public class EntradaTLB
    {
        public int NumeroPagina { get; set; }
        public int NumeroMoldura { get; set; }
        public string PIDProcesso { get; set; }
        public int TempoAcesso { get; set; }

        public EntradaTLB(string pidProcesso, int numeroPagina, int numeroMoldura, int tempoAcesso)
        {
            PIDProcesso = pidProcesso;
            NumeroPagina = numeroPagina;
            NumeroMoldura = numeroMoldura;
            TempoAcesso = tempoAcesso;
        }

        public override string ToString()
        {
            return $"{PIDProcesso}: Pág {NumeroPagina} -> Moldura {NumeroMoldura}";
        }
    }
}

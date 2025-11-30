namespace SimuladorSO.Nucleo
{
    public class Configuracoes
    {
        public int Seed { get; set; } = 0;
        public int Quantum { get; set; } = 4;
        public int TamanhoPagina { get; set; } = 1024;
        public int NumeroMolduras { get; set; } = 16;
        public string AlgoritmoEscalonamento { get; set; } = "RR";

        // Tempos de dispositivos (em ticks)
        public int TempoDisco { get; set; } = 30;
        public int TempoTeclado { get; set; } = 10;
        public int TempoImpressora { get; set; } = 40;

        public bool TLBAtivada { get; set; } = true;
        public int TamanhoTLB { get; set; } = 8;
    }
}
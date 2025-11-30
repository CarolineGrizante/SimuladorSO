namespace SimuladorSO.SistemaDeArquivos
{
    public class FCB
    {
        public string Caminho { get; set; }
        public string PIDProcesso { get; set; }
        public int ModoAbertura { get; set; } // 0=leitura, 1=escrita, 2=ambos
        public int PosicaoAtual { get; set; }

        public FCB(string caminho, string pidProcesso, int modoAbertura)
        {
            Caminho = caminho;
            PIDProcesso = pidProcesso;
            ModoAbertura = modoAbertura;
            PosicaoAtual = 0;
        }

        public override string ToString()
        {
            string modo = ModoAbertura == 0 ? "R" : ModoAbertura == 1 ? "W" : "RW";
            return $"{Caminho} ({modo}) - Processo: {PIDProcesso}";
        }
    }
}
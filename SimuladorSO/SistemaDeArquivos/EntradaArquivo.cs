namespace SimuladorSO.SistemaDeArquivos
{
    public class EntradaArquivo
    {
        public string Nome { get; set; }
        public int Tamanho { get; set; }
        public int TempoModificacao { get; set; }
        public string Conteudo { get; set; }
        public bool Aberto { get; set; }

        public EntradaArquivo(string nome)
        {
            Nome = nome;
            Tamanho = 0;
            TempoModificacao = 0;
            Conteudo = "";
            Aberto = false;
        }

        public override string ToString()
        {
            return $"[ARQ] {Nome} ({Tamanho} bytes)";
        }
    }
}
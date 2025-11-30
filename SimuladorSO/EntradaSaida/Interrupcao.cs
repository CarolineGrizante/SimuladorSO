namespace SimuladorSO.EntradaSaida
{
    public class Interrupcao
    {
        public string Tipo { get; set; }
        public string Origem { get; set; }
        public string Mensagem { get; set; }
        public int Tempo { get; set; }

        public Interrupcao(string tipo, string origem, string mensagem, int tempo)
        {
            Tipo = tipo;
            Origem = origem;
            Mensagem = mensagem;
            Tempo = tempo;
        }

        public override string ToString()
        {
            return $"[T={Tempo}] INT {Tipo} de {Origem}: {Mensagem}";
        }
    }
}

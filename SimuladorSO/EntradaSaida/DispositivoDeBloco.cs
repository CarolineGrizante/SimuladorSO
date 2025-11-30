namespace SimuladorSO.EntradaSaida
{
    public class DispositivoDeBloco : IDispositivo
    {
        public string Nome { get; private set; }
        public int TempoOperacao { get; set; }
        public bool Ocupado { get; private set; }

        private RequisicaoES? _requisicaoAtual;

        public DispositivoDeBloco(string nome, int tempoOperacao)
        {
            Nome = nome;
            TempoOperacao = tempoOperacao;
            Ocupado = false;
            _requisicaoAtual = null;
        }

        public void IniciarOperacao(RequisicaoES requisicao)
        {
            _requisicaoAtual = requisicao;
            Ocupado = true;
        }

        public void ProcessarTick()
        {
            if (_requisicaoAtual != null && Ocupado)
            {
                _requisicaoAtual.TempoRestante--;
            }
        }

        public RequisicaoES? FinalizarOperacao()
        {
            if (_requisicaoAtual != null && _requisicaoAtual.TempoRestante <= 0)
            {
                RequisicaoES requisicaoFinalizada = _requisicaoAtual;
                _requisicaoAtual = null;
                Ocupado = false;
                return requisicaoFinalizada;
            }
            return null;
        }
    }
}

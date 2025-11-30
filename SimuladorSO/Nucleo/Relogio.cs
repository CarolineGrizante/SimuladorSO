namespace SimuladorSO.Nucleo
{
    public class Relogio
    {
        private int _tempoAtual;

        public int TempoAtual => _tempoAtual;

        public Relogio()
        {
            _tempoAtual = 0;
        }

        public void Tick()
        {
            _tempoAtual++;
        }

        public void Tick(int quantidade)
        {
            _tempoAtual += quantidade;
        }

        public void Resetar()
        {
            _tempoAtual = 0;
        }
    }
}
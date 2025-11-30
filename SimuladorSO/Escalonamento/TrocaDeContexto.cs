namespace SimuladorSO.Escalonamento
{
    public class TrocaDeContexto
    {
        private int _contadorTrocas;
        private int _sobrecargaTotal;
        private const int CUSTO_TROCA = 1; // Custo em ticks de cada troca de contexto

        public int ContadorTrocas => _contadorTrocas;
        public int SobrecargaTotal => _sobrecargaTotal;

        public TrocaDeContexto()
        {
            _contadorTrocas = 0;
            _sobrecargaTotal = 0;
        }

        public void RegistrarTroca()
        {
            _contadorTrocas++;
            _sobrecargaTotal += CUSTO_TROCA;
        }

        public void Resetar()
        {
            _contadorTrocas = 0;
            _sobrecargaTotal = 0;
        }
    }
}

using System;

namespace SimuladorSO.Utilitarios
{
    public class GeradorAleatorio
    {
        private static Random? _random;

        public static void DefinirSemente(int semente)
        {
            _random = new Random(semente);
        }

        public static int ProximoInteiro(int min, int max)
        {
            if (_random == null)
                _random = new Random();

            return _random.Next(min, max);
        }

        public static double ProximoDouble()
        {
            if (_random == null)
                _random = new Random();

            return _random.NextDouble();
        }
    }
}
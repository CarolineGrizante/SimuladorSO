namespace SimuladorSO.Utilitarios
{
    public static class GeradorIDs
    {
        private static int _proximoPID = 1;
        private static int _proximoTID = 1;
        private static int _proximoTabelaID = 1;

        public static int GerarPID()
        {
            return _proximoPID++;
        }

        public static int GerarTID()
        {
            return _proximoTID++;
        }

        public static int GerarTabelaID()
        {
            return _proximoTabelaID++;
        }

        public static void Resetar()
        {
            _proximoPID = 1;
            _proximoTID = 1;
            _proximoTabelaID = 1;
        }
    }
}
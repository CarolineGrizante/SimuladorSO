using System.Collections.Generic;

namespace SimuladorSO.SistemaDeArquivos
{
    public class EntradaDiretorio
    {
        public string Nome { get; set; }
        public Dictionary<string, EntradaArquivo> Arquivos { get; set; }
        public Dictionary<string, EntradaDiretorio> Subdiretorios { get; set; }

        public EntradaDiretorio(string nome)
        {
            Nome = nome;
            Arquivos = new Dictionary<string, EntradaArquivo>();
            Subdiretorios = new Dictionary<string, EntradaDiretorio>();
        }

        public override string ToString()
        {
            return $"[DIR] {Nome}/";
        }
    }
}
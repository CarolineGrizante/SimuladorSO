using SimuladorSO.Nucleo;
using SimuladorSO.Processos;
using System;
using System.Diagnostics;
using System.Linq;

namespace SimuladorSO.SistemaDeArquivos
{
    public class SistemaDeArquivos
    {
        private Kernel _kernel;
        private EntradaDiretorio _raiz;
        private EntradaDiretorio _diretorioAtual;
        private ManipuladorArquivo _manipuladorArquivo;
        private TabelaDeAlocacao _tabelaAlocacao;

        public SistemaDeArquivos(Kernel kernel)
        {
            _kernel = kernel;
            _raiz = new EntradaDiretorio("/");
            _diretorioAtual = _raiz;
            _manipuladorArquivo = new ManipuladorArquivo();
            _tabelaAlocacao = new TabelaDeAlocacao();

            // Criar diretórios padrão
            _raiz.Subdiretorios["docs"] = new EntradaDiretorio("docs");
            _raiz.Subdiretorios["bin"] = new EntradaDiretorio("bin");
        }

        public void CriarArquivo(string caminho)
        {
            var partes = caminho.Split('/');
            string nomeArquivo = partes[partes.Length - 1];

            EntradaDiretorio? diretorio = ObterDiretorio(caminho);

            if (diretorio != null)
            {
                if (!diretorio.Arquivos.ContainsKey(nomeArquivo))
                {
                    diretorio.Arquivos[nomeArquivo] = new EntradaArquivo(nomeArquivo);
                    _kernel.RegistradorEventos.RegistrarEvento($"Arquivo criado: {caminho}");
                }
                else
                {
                    Console.WriteLine($"Arquivo {caminho} já existe.");
                }
            }
        }

        public void CriarDiretorio(string caminho)
        {
            var partes = caminho.Split('/');
            string nomeDiretorio = partes[partes.Length - 1];

            if (!_diretorioAtual.Subdiretorios.ContainsKey(nomeDiretorio))
            {
                _diretorioAtual.Subdiretorios[nomeDiretorio] = new EntradaDiretorio(nomeDiretorio);
                _kernel.RegistradorEventos.RegistrarEvento($"Diretório criado: {nomeDiretorio}");
            }
            else
            {
                Console.WriteLine($"Diretório {nomeDiretorio} já existe.");
            }
        }

        public void AbrirArquivo(string pidSimbolico, string caminho)
        {
            Processo? processo = _kernel.GerenciadorProcessos.ObterProcessoPorSimbolico(pidSimbolico);

            if (processo == null)
            {
                Console.WriteLine($"Processo {pidSimbolico} não encontrado.");
                return;
            }

            EntradaArquivo? arquivo = ObterArquivo(caminho);

            if (arquivo != null)
            {
                _manipuladorArquivo.AbrirArquivo(caminho, pidSimbolico, 2);
                arquivo.Aberto = true;
                processo.AbrirArquivo(caminho);

                _kernel.RegistradorEventos.RegistrarEvento($"Arquivo aberto: {caminho} por {pidSimbolico}");
            }
            else
            {
                Console.WriteLine($"Arquivo {caminho} não encontrado.");
            }
        }

        public void FecharArquivo(string pidSimbolico, string caminho)
        {
            Processo? processo = _kernel.GerenciadorProcessos.ObterProcessoPorSimbolico(pidSimbolico);

            if (processo == null)
            {
                Console.WriteLine($"Processo {pidSimbolico} não encontrado.");
                return;
            }

            _manipuladorArquivo.FecharArquivo(caminho, pidSimbolico);
            processo.FecharArquivo(caminho);

            EntradaArquivo? arquivo = ObterArquivo(caminho);
            if (arquivo != null && !_manipuladorArquivo.EstaAberto(caminho))
            {
                arquivo.Aberto = false;
            }

            _kernel.RegistradorEventos.RegistrarEvento($"Arquivo fechado: {caminho} por {pidSimbolico}");
        }

        public void LerArquivo(string pidSimbolico, string caminho, int tamanho)
        {
            EntradaArquivo? arquivo = ObterArquivo(caminho);

            if (arquivo != null)
            {
                _kernel.RegistradorEventos.RegistrarEvento(
                    $"Leitura de arquivo: {caminho} ({tamanho} bytes) por {pidSimbolico}"
                );
            }
            else
            {
                Console.WriteLine($"Arquivo {caminho} não encontrado.");
            }
        }

        public void EscreverArquivo(string pidSimbolico, string caminho, int tamanho)
        {
            EntradaArquivo? arquivo = ObterArquivo(caminho);

            if (arquivo != null)
            {
                arquivo.Tamanho = tamanho;
                arquivo.TempoModificacao = _kernel.Relogio.TempoAtual;

                // Alocar blocos
                int blocos = (tamanho / 512) + 1;
                _tabelaAlocacao.AlocarBlocos(caminho, blocos);

                _kernel.RegistradorEventos.RegistrarEvento(
                    $"Escrita em arquivo: {caminho} ({tamanho} bytes) por {pidSimbolico}"
                );
            }
            else
            {
                Console.WriteLine($"Arquivo {caminho} não encontrado.");
            }
        }

        public void ApagarArquivo(string caminho)
        {
            var partes = caminho.Split('/');
            string nomeArquivo = partes[partes.Length - 1];

            EntradaDiretorio? diretorio = ObterDiretorio(caminho);

            if (diretorio != null && diretorio.Arquivos.ContainsKey(nomeArquivo))
            {
                diretorio.Arquivos.Remove(nomeArquivo);
                _tabelaAlocacao.LiberarBlocos(caminho);

                _kernel.RegistradorEventos.RegistrarEvento($"Arquivo apagado: {caminho}");
            }
            else
            {
                Console.WriteLine($"Arquivo {caminho} não encontrado.");
            }
        }

        public void ListarDiretorio()
        {
            Console.WriteLine($"\n===== Diretório: {_diretorioAtual.Nome} =====");

            foreach (var subdir in _diretorioAtual.Subdiretorios.Values)
            {
                Console.WriteLine(subdir.ToString());
            }

            foreach (var arquivo in _diretorioAtual.Arquivos.Values)
            {
                Console.WriteLine(arquivo.ToString());
            }

            Console.WriteLine("===============================\n");
        }

        public void MudarDiretorio(string nome)
        {
            if (nome == "..")
            {
                // Voltar para raiz (simplificado)
                _diretorioAtual = _raiz;
            }
            else if (_diretorioAtual.Subdiretorios.ContainsKey(nome))
            {
                _diretorioAtual = _diretorioAtual.Subdiretorios[nome];
            }
            else
            {
                Console.WriteLine($"Diretório {nome} não encontrado.");
            }
        }

        private EntradaDiretorio? ObterDiretorio(string caminho)
        {
            var partes = caminho.Split('/').Where(p => !string.IsNullOrEmpty(p)).ToArray();

            if (partes.Length == 0)
                return _raiz;

            EntradaDiretorio atual = _raiz;

            for (int i = 0; i < partes.Length - 1; i++)
            {
                if (atual.Subdiretorios.ContainsKey(partes[i]))
                {
                    atual = atual.Subdiretorios[partes[i]];
                }
                else
                {
                    return null;
                }
            }

            return atual;
        }

        private EntradaArquivo? ObterArquivo(string caminho)
        {
            var partes = caminho.Split('/').Where(p => !string.IsNullOrEmpty(p)).ToArray();

            if (partes.Length == 0)
                return null;

            string nomeArquivo = partes[partes.Length - 1];
            EntradaDiretorio? diretorio = ObterDiretorio(caminho);

            if (diretorio != null && diretorio.Arquivos.ContainsKey(nomeArquivo))
            {
                return diretorio.Arquivos[nomeArquivo];
            }

            return null;
        }
    }
}
using SimuladorSO.Nucleo;
using SimuladorSO.Processos;
using SimuladorSO.Utilitarios;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SimuladorSO.Memoria
{
    public class GerenciadorDeMemoria
    {
        private Kernel _kernel;
        private TabelaDeMolduras _tabelaMolduras;
        private Dictionary<string, TabelaDePaginas> _tabelasPaginas;
        private TLB _tlb;
        private int _faltasPagina;

        public int FaltasPagina => _faltasPagina;
        public TLB TLB => _tlb;

        public GerenciadorDeMemoria(Kernel kernel)
        {
            _kernel = kernel;
            _tabelaMolduras = new TabelaDeMolduras(kernel.Configuracoes.NumeroMolduras);
            _tabelasPaginas = new Dictionary<string, TabelaDePaginas>();
            _tlb = new TLB(kernel.Configuracoes.TamanhoTLB);
            _faltasPagina = 0;
        }

        public void AlocarMemoria(string pidSimbolico, int tamanhoBytes)
        {
            Processo? processo = _kernel.GerenciadorProcessos.ObterProcessoPorSimbolico(pidSimbolico);

            if (processo == null)
            {
                Console.WriteLine($"Processo {pidSimbolico} não encontrado.");
                return;
            }

            int tamanhoPagina = _kernel.Configuracoes.TamanhoPagina;
            int numeroPaginas = (int)Math.Ceiling((double)tamanhoBytes / tamanhoPagina);

            // Criar tabela de páginas se não existir
            if (!_tabelasPaginas.ContainsKey(pidSimbolico))
            {
                int tabelaID = GeradorIDs.GerarTabelaID();
                _tabelasPaginas[pidSimbolico] = new TabelaDePaginas(tabelaID, pidSimbolico);
                processo.PCB.TabelaPaginasID = tabelaID;
            }

            _kernel.RegistradorEventos.RegistrarEvento(
                $"Memória alocada para {pidSimbolico}: {tamanhoBytes} bytes ({numeroPaginas} páginas)"
            );
        }

        public void LiberarMemoria(string pidSimbolico)
        {
            if (_tabelasPaginas.ContainsKey(pidSimbolico))
            {
                _tabelaMolduras.LiberarMoldurasDoProcesso(pidSimbolico);
                _tabelasPaginas.Remove(pidSimbolico);
                _tlb.LimparProcesso(pidSimbolico);

                _kernel.RegistradorEventos.RegistrarEvento($"Memória liberada para {pidSimbolico}");
            }
        }

        public void AcessarMemoria(string pidSimbolico, int enderecoLogico)
        {
            if (!_tabelasPaginas.ContainsKey(pidSimbolico))
            {
                Console.WriteLine($"Processo {pidSimbolico} não possui tabela de páginas.");
                return;
            }

            int tamanhoPagina = _kernel.Configuracoes.TamanhoPagina;
            int numeroPagina = enderecoLogico / tamanhoPagina;
            int deslocamento = enderecoLogico % tamanhoPagina;

            int? numeroMoldura = null;

            // Tentar buscar na TLB
            if (_kernel.Configuracoes.TLBAtivada)
            {
                numeroMoldura = _tlb.Buscar(pidSimbolico, numeroPagina);
            }

            // Se não encontrou na TLB, buscar na tabela de páginas
            if (numeroMoldura == null)
            {
                TabelaDePaginas tabela = _tabelasPaginas[pidSimbolico];
                Pagina pagina = tabela.ObterOuCriarPagina(numeroPagina);

                if (!pagina.Presente)
                {
                    // Falta de página
                    _faltasPagina++;
                    _kernel.RegistradorEventos.RegistrarEvento(
                        $"Falta de página: {pidSimbolico} - Página {numeroPagina}"
                    );

                    // Alocar moldura
                    Moldura? moldura = _tabelaMolduras.AlocarMoldura(
                        pidSimbolico, numeroPagina, PoliticaAlocacao.FirstFit
                    );

                    if (moldura != null)
                    {
                        pagina.NumeroMoldura = moldura.NumeroMoldura;
                        pagina.Presente = true;
                        numeroMoldura = moldura.NumeroMoldura;
                    }
                    else
                    {
                        Console.WriteLine("Erro: Sem molduras livres disponíveis!");
                        return;
                    }
                }
                else
                {
                    numeroMoldura = pagina.NumeroMoldura;
                }

                // Atualizar TLB
                if (_kernel.Configuracoes.TLBAtivada && numeroMoldura != null)
                {
                    _tlb.Adicionar(pidSimbolico, numeroPagina, numeroMoldura.Value, _kernel.Relogio.TempoAtual);
                }

                pagina.Referenciada = true;
                pagina.TempoAcesso = _kernel.Relogio.TempoAtual;
            }

            int enderecoFisico = numeroMoldura.Value * tamanhoPagina + deslocamento;

            _kernel.RegistradorEventos.RegistrarEvento(
                $"Acesso à memória: {pidSimbolico} - End. Lógico: 0x{enderecoLogico:X4} -> End. Físico: 0x{enderecoFisico:X4}"
            );
        }

        public void MostrarTabelaPaginas(string pidSimbolico)
        {
            if (!_tabelasPaginas.ContainsKey(pidSimbolico))
            {
                Console.WriteLine($"Processo {pidSimbolico} não possui tabela de páginas.");
                return;
            }

            Console.WriteLine($"\n===== TABELA DE PÁGINAS - {pidSimbolico} =====");
            TabelaDePaginas tabela = _tabelasPaginas[pidSimbolico];

            foreach (var pagina in tabela.Paginas.Values)
            {
                Console.WriteLine(pagina.ToString());
            }
            Console.WriteLine("==========================================\n");
        }

        public void MostrarMapaMolduras()
        {
            Console.WriteLine("\n===== MAPA DE MOLDURAS =====");
            var molduras = _tabelaMolduras.ObterTodasMolduras();

            foreach (var moldura in molduras)
            {
                Console.WriteLine(moldura.ToString());
            }

            Console.WriteLine($"\nMolduras livres: {_tabelaMolduras.ContarMoldurasLivres()}");
            Console.WriteLine("============================\n");
        }

        public void MostrarEstatisticasTLB()
        {
            Console.WriteLine("\n===== ESTATÍSTICAS TLB =====");
            Console.WriteLine($"Hits: {_tlb.Hits}");
            Console.WriteLine($"Misses: {_tlb.Misses}");
            Console.WriteLine($"Taxa de Acerto: {_tlb.TaxaAcerto():F2}%");
            Console.WriteLine("============================\n");
        }
    }
}
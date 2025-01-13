using Models.Entities;
using Microsoft.EntityFrameworkCore;
using Data;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Collections;



namespace Services
{
    public class LusiumService
    {
        private readonly LusiumDbContext _context;

        public LusiumService(LusiumDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ValidaDadosAdministrador(string nome, string email, string pp)
        {
            // name validation
            if (!Utils.IsNameValid(nome))
            {
                return false;
            }

            // email validation
            if (!Utils.IsEmailValid(email))
            {
                return false;
            }

            // password validation
            if (!Utils.IsPasswordValid(pp))
            {
                return false;
            }

            return !await _context.Administrador.AnyAsync(a => a.Email == email);
        }

        public async Task<Administrator> RegistaAdministrador(string nome, string email, string pp)
        {
            var admin = new Administrator
            {
                Nome = nome,
                Email = email,
                PalavraPasse = pp
            };

            _context.Administrador.Add(admin);
            await _context.SaveChangesAsync();

            return admin;
        }

        public async Task<bool> ValidaDadosInstituicao(string nome, string nif, string numAssoc, string email, string morada, string pp)
        {
            // address validation
            if (morada == "")
            {
                return false;
            }

            // name validation
            if (!Utils.IsNameValid(nome))
            {
                return false;
            }

            // nif validation
            if (!Utils.IsNifValid(nif))
            {
                return false;
            }

            // numAssoc validation
            if (!Utils.IsNumAssocValid(numAssoc))
            {
                return false;
            }

            // email validation
            if (!Utils.IsEmailValid(email))
            {
                return false;
            }

            // password validation
            if (!Utils.IsPasswordValid(pp))
            {
                return false;
            }

            return !await _context.Instituicao.AnyAsync(i => i.Email == email);
        }

        public async Task<Instituition> RegistaInstituicao(string nome, string nif, string numAssoc, string email, string morada, string pp)
        {
            var inst = new Instituition
            {
                Nome = nome,
                NIF = nif,
                NumeroAssociacao = int.Parse(numAssoc),
                Email = email,
                Morada = morada,
                Creditos = 0,
                PalavraPasse = pp
            };

            _context.Instituicao.Add(inst);
            await _context.SaveChangesAsync();

            return inst;
        }
        
        public async Task<bool> ValidaDadosColaborador(string nome, string email, DateOnly dataDeNascimento, string codInstituicaoAssociada, string pp)
        {
            // institution code validation
            if (codInstituicaoAssociada == "")
            {
                return false;
            }

            // name validation
            if (!Utils.IsNameValid(nome))
            {
                return false;
            }

            // email validation
            if (!Utils.IsEmailValid(email))
            {
                return false;
            }

            // birth date validation
            if (!Utils.IsDateValid(dataDeNascimento))
            {
                return false;
            }

            // password validation
            if (!Utils.IsPasswordValid(pp))
            {
                return false;
            }

            if (await _context.Colaborador.AnyAsync(i => i.Email == email))
            {
                return false;
            }

            return await _context.Instituicao.AnyAsync(i => i.ID == int.Parse(codInstituicaoAssociada));
        }
        
        public async Task<Collaborator> RegistaColaborador(string nome, string email, DateOnly dataDeNascimento, string codInstituicaoAssociada, string pp)
        {
            var colab = new Collaborator
            {
                Nome = nome,
                Email = email,
                DataNascimento = dataDeNascimento,
                InstituicaoID = int.Parse(codInstituicaoAssociada),
                PalavraPasse = pp
            };

            _context.Colaborador.Add(colab);
            await _context.SaveChangesAsync();

            return colab;
        }

        public async Task<bool> ValidaCredenciais(string email, string pp)
        {
            // email validation
            if (!Utils.IsEmailValid(email))
            {
                return false;
            }

            // password validation
            if (!Utils.IsPasswordValid(pp))
            {
                return false;
            }

            // at least one user with the given credentials
            return await _context.Administrador.AnyAsync(a => a.Email == email && a.PalavraPasse == pp) ||
                   await _context.Instituicao.AnyAsync(i => i.Email == email && i.PalavraPasse == pp) ||
                   await _context.Colaborador.AnyAsync(c => c.Email == email && c.PalavraPasse == pp);
        }

        public async Task<bool> ValidaNovaPP(string email, string novaPP)
        {
            // password validation
            if (!Utils.IsPasswordValid(novaPP))
            {
                return false;
            }

            var nameParam = new SqlParameter("@Nome", SqlDbType.VarChar, 45) { Direction = ParameterDirection.Output };
            var roleParam = new SqlParameter("@Role", SqlDbType.VarChar, 20) { Direction = ParameterDirection.Output };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC VerificarIniciarSessao @Email = {0}, @PalavraPasse = {1}, @Nome = @Nome OUTPUT, @Role = @Role OUTPUT",
                email,
                novaPP,
                nameParam,
                roleParam
            );

            return nameParam.Value.ToString() == "" || roleParam.Value.ToString() == "";
        }

        public async Task AtualizaPP(string codUtilizador, string novaPP)
        {
            var admin = await _context.Administrador.FirstOrDefaultAsync(a => a.ID == int.Parse(codUtilizador));
            if (admin == null)
            {
                return;
            }
            
            admin.PalavraPasse = novaPP;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ValidaNovosDadosAdministrador(string codAdministrador, string novoNome, string novaPP)
        {
            var administrador = await _context.Administrador.FirstOrDefaultAsync(a => a.ID == int.Parse(codAdministrador));
            
            // no administrator found
            if (administrador == null)
            {
                return false;
            }

            if (administrador.Nome != novoNome && !Utils.IsNameValid(novoNome))
            {
                return false;
            
            }

            if (administrador.PalavraPasse != novaPP && !Utils.IsPasswordValid(novaPP))
            {
                return false;
            }

            return true;
        }

        public async Task<bool> ValidaPP(string email, string pp)
        {
            if (!Utils.IsPasswordValid(pp))
            {
                return false;
            }

            var nameParam = new SqlParameter("@Nome", SqlDbType.VarChar, 45) { Direction = ParameterDirection.Output };
            var roleParam = new SqlParameter("@Role", SqlDbType.VarChar, 20) { Direction = ParameterDirection.Output };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC VerificarIniciarSessao @Email = {0}, @PalavraPasse = {1}, @Nome = @Nome OUTPUT, @Role = @Role OUTPUT",
                email,
                pp,
                nameParam,
                roleParam
            );

            return nameParam.Value.ToString() != "" && roleParam.Value.ToString() != "";
        }

        public async Task AtualizaDadosAdministrador(string codAdministrador, string novoNome, string novaPP)
        {
            var admin = await _context.Administrador.FirstOrDefaultAsync(a => a.ID == int.Parse(codAdministrador));
            if (admin == null)
            {
                return;
            }
            // removes the need to call SaveChangesAsync on the DB
            if (admin.Nome == novoNome && admin.PalavraPasse == novaPP)
            {
                return;
            }
            
            if (admin.Nome != novoNome)
            {
                admin.Nome = novoNome;
            }
            
            if (admin.PalavraPasse != novaPP)
            {
                admin.PalavraPasse = novaPP;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ValidaNovosDadosInstituicao(string codInstituicao, string novoNome, string novaMorada, string novaPP)
        {
            var instituicao = await _context.Instituicao.FirstOrDefaultAsync(i => i.ID == int.Parse(codInstituicao));
            // no institution found
            if (instituicao == null)
            {
                return false;
            }

            if (instituicao.Nome != novoNome && !Utils.IsNameValid(novoNome))
            {
                return false;
            }

            if (instituicao.Morada != novaMorada && novaMorada == "")
            {
                return false;
            }

            if (instituicao.PalavraPasse != novaPP && !Utils.IsPasswordValid(novaPP))
            {
                return false;
            }

            return true;
        }

        public async Task AtualizaDadosInstituicao(string codInstituicao, string novoNome, string novaMorada, string novaPP)
        {
            var inst = await _context.Instituicao.FirstOrDefaultAsync(i => i.ID == int.Parse(codInstituicao));
            if (inst == null)
            {
                return;
            }
            // removes the need to call SaveChangesAsync on the DB
            if (inst.Nome == novoNome && inst.Morada == novaMorada && inst.PalavraPasse == novaPP)
            {
                return;
            }

            if (inst.Nome != novoNome)
            {
                inst.Nome = novoNome;
            }

            if (inst.Morada != novaMorada)
            {
                inst.Morada = novaMorada;
            }

            if (inst.PalavraPasse != novaPP)
            {
                inst.PalavraPasse = novaPP;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ValidaNovosDadosColaborador(string codColaborador, string novoNome, DateOnly novaDataNascimento, string codNovaInstituicao, string novaPP)
        {
            var colab = await _context.Colaborador.FirstOrDefaultAsync(c => c.ID == int.Parse(codColaborador));
            // no collaborator found
            if (colab == null)
            {
                return false;
            }

            if (colab.Nome != novoNome && !Utils.IsNameValid(novoNome))
            {
                return false;
            }

            if (colab.DataNascimento != novaDataNascimento && !Utils.IsDateValid(novaDataNascimento))
            {
                return false;
            }

            if (colab.InstituicaoID != int.Parse(codNovaInstituicao) && !await _context.Instituicao.AnyAsync(i => i.NumeroAssociacao == int.Parse(codNovaInstituicao)))
            {
                return false;
            }

            if (colab.PalavraPasse != novaPP && !Utils.IsPasswordValid(novaPP))
            {
                return false;
            }

            return true;
        }

        public async Task AtualizaDadosColaborador(string codColaborador, string novoNome, DateOnly novaDataNascimento, string codNovaInstituicao, string novaPP)
        {
            var colab = await _context.Colaborador.FirstOrDefaultAsync(c => c.ID == int.Parse(codColaborador));
            if (colab == null)
            {
                return;
            }
            // removes the need to call SaveChangesAsync on the DB
            if (colab.Nome == novoNome && colab.DataNascimento == novaDataNascimento && colab.InstituicaoID == int.Parse(codNovaInstituicao) && colab.PalavraPasse == novaPP)
            {
                return;
            }

            if (colab.Nome != novoNome)
            {
                colab.Nome = novoNome;
            }

            if (colab.DataNascimento != novaDataNascimento)
            {
                colab.DataNascimento = novaDataNascimento;
            }

            if (colab.InstituicaoID != int.Parse(codNovaInstituicao))
            {
                colab.InstituicaoID = int.Parse(codNovaInstituicao);
            }

            if (colab.PalavraPasse != novaPP)
            {
                colab.PalavraPasse = novaPP;
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveUtilizador(string codUtilizador, string tipoUtilizador)
        {
            // in case the user is an administrator
            if (tipoUtilizador == "Admin")
            {
                var admin = await _context.Administrador.FirstOrDefaultAsync(a => a.ID == int.Parse(codUtilizador));
                if (admin != null)
                {
                    _context.Administrador.Remove(admin);
                    await _context.SaveChangesAsync();
                }
                return;
            }

            // in case the user is an institution
            if (tipoUtilizador == "Institution")
            {
                var inst = await _context.Instituicao.FirstOrDefaultAsync(i => i.ID == int.Parse(codUtilizador));
                if (inst != null)
                {
                    // remove all the entries on table ManualDaInstituicao whose IDInstituicao is the same as inst.ID
                    await _context.Database.ExecuteSqlRawAsync("DELETE FROM ManualDaInstituicao WHERE IDInstituicao = {0}", inst.ID);

                    _context.Instituicao.Remove(inst);
                    await _context.SaveChangesAsync();
                }
                return;
            }

            // in case the user is a collaborator
            if (tipoUtilizador == "Collaborator")
            {
                var colab = await _context.Colaborador.FirstOrDefaultAsync(c => c.ID == int.Parse(codUtilizador));
                if (colab != null)
                {
                    _context.Colaborador.Remove(colab);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task RemoveColaboradoresInstituicao(string codInstituicao)
        {
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM Colaborador WHERE Instituicao = {0}", int.Parse(codInstituicao));
        }

        public async Task<Dictionary<string, IUser>> ListaUtilizadores(string tipoUtilizador)
        {
            if (tipoUtilizador == "Admin")
            {
                return await _context.Administrador.ToDictionaryAsync(a => a.ID.ToString(), a => (IUser)a);
            }

            if (tipoUtilizador == "Institution")
            {
                return await _context.Instituicao.ToDictionaryAsync(i => i.ID.ToString(), i => (IUser)i);
            }

            if (tipoUtilizador == "Collaborator")
            {
                return await _context.Colaborador.ToDictionaryAsync(c => c.ID.ToString(), c => (IUser)c);
            }

            return new Dictionary<string, IUser>();
        }

        public async Task<Dictionary<string, IUser>> FiltraUtilizadores(string nome, string tipoUtilizador) 
        {
            if (tipoUtilizador == "Admin")
            {
                return await _context.Administrador.Where(a => a.Nome.Contains(nome)).ToDictionaryAsync(a => a.ID.ToString(), a => (IUser)a);
            }

            if (tipoUtilizador == "Institution")
            {
                return await _context.Instituicao.Where(i => i.Nome.Contains(nome)).ToDictionaryAsync(i => i.ID.ToString(), i => (IUser)i);
            }

            if (tipoUtilizador == "Collaborator")
            {
                return await _context.Colaborador.Where(c => c.Nome.Contains(nome)).ToDictionaryAsync(c => c.ID.ToString(), c => (IUser)c);
            }
            
            return new Dictionary<string, IUser>();
        }

        /*
            Criar classe de comparação para ordenar instituições:
            class NomeInstituicaoComparer : IComparer<Instituition>
            {
                public int Compare(Instituition x, Instituition y)
                {
                    return x.Nome.CompareTo(y.Nome);
                }
            }
        */
        public async Task<Dictionary<string, IUser>> OrdenaUtilizadores(IComparer<IUser> comparer, string tipoUtilizador)
        {
            if (tipoUtilizador == "Institution")
            {
                return await _context.Instituicao.OrderBy(i => i, comparer).ToDictionaryAsync(i => i.ID.ToString(), i => (IUser)i);
            }

            if (tipoUtilizador == "Collaborator")
            {
                return await _context.Colaborador.OrderBy(c => c, comparer).ToDictionaryAsync(c => c.ID.ToString(), c => (IUser)c);
            }

            return new Dictionary<string, IUser>();
        }
    

        public async Task<Dictionary<string, Product>> ListaProdutos()
        {
            return await _context.Produto.ToDictionaryAsync(p => p.ID.ToString(), p => p);
        }

        // exibePaginaManual(codManual: String) : PaginaManual ???????????????????????????????

        // iterarPaginaManual(codManual: String, comando: String) : PaginaManual ???????????????????????????????

        public async Task<bool> DisponibilidadeProduto(string codProduto) {
            var codProdutoParam = new SqlParameter("@codProduto", SqlDbType.Int) { Value = int.Parse(codProduto) };
            var disponivelParam = new SqlParameter("@Disponivel", SqlDbType.Bit) { Direction = ParameterDirection.Output };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC VerificarDisponibilidadeProduto @codProduto = @codProduto, @Disponivel = @Disponivel OUTPUT",
                codProdutoParam,
                disponivelParam
            );

            return (bool)disponivelParam.Value;
        }

        public async Task<bool> VerificaSaldo(string codInstituicao, string codProduto)
        {
            var codInstituicaoParam = new SqlParameter("@codInstituicao", SqlDbType.Int) { Value = int.Parse(codInstituicao) };
            var codProdutoParam = new SqlParameter("@codProduto", SqlDbType.Int) { Value = int.Parse(codProduto) };
            var saldoSuficienteParam = new SqlParameter("@SaldoSuficiente", SqlDbType.Bit) { Direction = ParameterDirection.Output };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC VerificarSaldoInstituicao @codInstituicao = @codInstituicao, @codProduto = @codProduto, @SaldoSuficiente = @SaldoSuficiente OUTPUT",
                codInstituicaoParam,
                codProdutoParam,
                saldoSuficienteParam
            );

            return (bool)saldoSuficienteParam.Value;
        }

        public async Task ProcessaCompra(string codInstituicao, string codProduto) {
            var codInstituicaoParam = new SqlParameter("@codInstituicao", SqlDbType.Int) { Value = int.Parse(codInstituicao) };
            var codProdutoParam = new SqlParameter("@codProduto", SqlDbType.Int) { Value = int.Parse(codProduto) };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC ProcessarCompra @codInstituicao = @codInstituicao, @codProduto = @codProduto",
                codInstituicaoParam,
                codProdutoParam
            );
        }

        public bool ValidaMorada(string morada)
        {
            return morada != "";
        }

        public async Task<Dictionary<string, Purchase>> ListaComprasEfetuadas(string codInstituicao)
        {
            return await _context.Compra.Where(c => c.InstituicaoID == int.Parse(codInstituicao)).ToDictionaryAsync(c => c.NumeroCompra.ToString(), c => c);
        }

        public async Task<Purchase?> ExibeCompraEfetuada(string codCompra)
        {
           return await _context.Compra.FirstOrDefaultAsync(c => c.NumeroCompra == int.Parse(codCompra));
        }

        // listaManuais(codUtilizador: String) : Map\<String, Manual>
        
        public bool ValidaQuantidadeCreditos(int quantidade)
        {
            return quantidade >= 0;
        }

        public async Task AtualizaQuantidadeCreditos(string codInstituicao, int quantidade)
        {
            var inst = await _context.Instituicao.FirstOrDefaultAsync(i => i.ID == int.Parse(codInstituicao));
            if (inst == null)
            {
                return;
            }

            inst.Creditos = quantidade;
            await _context.SaveChangesAsync();
        }

        public bool ValidaQuantidadePecas(int quantidade)
        {
            return quantidade >= 0;
        }

        public async Task AtualizaQuantidadePecas(string codPeca, int quantidade)
        {
            var peca = await _context.Peca.FirstOrDefaultAsync(p => p.ID == int.Parse(codPeca));
            if (peca == null)
            {
                return;
            }

            peca.Quantidade = quantidade;
            await _context.SaveChangesAsync();
        }

        public async Task<Dictionary<string, Piece>> ListaPecas()
        {
            return await _context.Peca.ToDictionaryAsync(p => p.ID.ToString(), p => p);
        }

        public async Task<List<Product>> GetProducts()
        {
            return await _context.Produto.ToListAsync();
        }

        public async Task<(string? Name, string? Role)> LoginVerifyAsync(string email, string password)
        {
            var nameParam = new SqlParameter("@Nome", SqlDbType.VarChar, 45) { Direction = ParameterDirection.Output };
            var roleParam = new SqlParameter("@Role", SqlDbType.VarChar, 20) { Direction = ParameterDirection.Output };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC VerificarIniciarSessao @Email = {0}, @PalavraPasse = {1}, @Nome = @Nome OUTPUT, @Role = @Role OUTPUT",
                email,
                password,
                nameParam,
                roleParam
            );

            return (nameParam.Value.ToString(), roleParam.Value.ToString());
        }
    }
}
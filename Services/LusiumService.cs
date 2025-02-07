﻿using Models.Entities;
using Microsoft.EntityFrameworkCore;
using Data;
using Microsoft.Data.SqlClient;
using System.Data;

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

            return !await _context.Instituicao.AnyAsync(i => i.Email == email); ;
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
        public async Task<Collaborator?> ObterColaborador(string codColaborador)
        {
            var id = int.Parse(codColaborador);

            return await _context.Colaborador.FirstOrDefaultAsync(c => c.ID == id);
        }

        public async Task<Instituition?> ObterInstituicao(string codInstituicao)
        {
            var id = int.Parse(codInstituicao);

            return await _context.Instituicao.FirstOrDefaultAsync(i => i.ID == id);
        }

        public async Task<Administrator?> ObterAdministrador(string codAdministrador)
        {
            var id = int.Parse(codAdministrador);

            return await _context.Administrador.FirstOrDefaultAsync(a => a.ID == id);
        }

        public async Task<Product?> ObterProduto(string codProduto)
        {
            var id = int.Parse(codProduto);

            return await _context.Produto.FirstOrDefaultAsync(p => p.ID == id);
        }

        public async Task<List<PiecePerProduct>> ObterPecasNecessariasPorProduto(int produtoId)
        {
            return await _context.PecaProduto
                .Where(pp => pp.IDProduto == produtoId)
                .Select(pp => new PiecePerProduct
                {
                    IDPeca = pp.IDPeca,
                    Quantidade = pp.Quantidade
                })
                .ToListAsync();
        }

        public async Task<List<Piece>> ObterPecas()
        {
            return await _context.Peca.ToListAsync();
        }

        public async Task<int> CalcularCreditosDispendidos(int instituicaoID)
        {
            return await _context.Compra
                .Where(c => c.InstituicaoID == instituicaoID)
                .Join(
                    _context.Produto,
                    compra => compra.ProdutoAssociado,
                    produto => produto.ID,
                    (compra, produto) => produto.Custo
                )
                .SumAsync(custo => custo);
        }

        public async Task<DataTable> ValidaCredenciais(string email, string password)
        {
            DataTable dataTable = new DataTable();

            string query = @"EXEC VerificarIniciarSessao @Email = @EmailParam, @PalavraPasse = @PasswordParam";

            using (var connection = _context.Database.GetDbConnection())
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;

                    var emailParam = command.CreateParameter();
                    emailParam.ParameterName = "@EmailParam";
                    emailParam.Value = email;
                    command.Parameters.Add(emailParam);

                    var passwordParam = command.CreateParameter();
                    passwordParam.ParameterName = "@PasswordParam";
                    passwordParam.Value = password;
                    command.Parameters.Add(passwordParam);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        dataTable.Load(reader);
                    }
                }
            }

            return dataTable;
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

            var isValidParam = new SqlParameter("@IsValid", SqlDbType.Bit) { Direction = ParameterDirection.Output };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC ValidaPP @Email = {0}, @PalavraPasse = {1}, @IsValid = @IsValid OUTPUT",
                email,
                pp,
                isValidParam
            );

            return (bool)isValidParam.Value;
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

        public async Task<bool> ValidaNovosDadosInstituicao(string codInstituicao, string novoNome, string novaMorada, string novaPP, int novoCreditos)
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

            if (novoCreditos < 0)
            {
                return false;
            }

            return true;
        }
        public async Task AtualizaDadosInstituicao(string codInstituicao, string novoNome, string novaMorada, string novaPP, int novoCreditos)
        {
            var inst = await _context.Instituicao.FirstOrDefaultAsync(i => i.ID == int.Parse(codInstituicao));
            if (inst == null)
            {
                return;
            }
            // removes the need to call SaveChangesAsync on the DB
            if (inst.Nome == novoNome && inst.Morada == novaMorada && inst.PalavraPasse == novaPP && inst.Creditos == novoCreditos)
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

            if (inst.Creditos != novoCreditos)
            {
                inst.Creditos = novoCreditos;
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

            if (colab.InstituicaoID != int.Parse(codNovaInstituicao) && !await _context.Instituicao.AnyAsync(i => i.ID == int.Parse(codNovaInstituicao)))
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
            int userId = int.Parse(codUtilizador);

            switch (tipoUtilizador)
            {
                case "Admin":
                    var admin = await _context.Administrador.FindAsync(userId);
                    if (admin != null)
                    {
                        _context.Administrador.Remove(admin);
                        await _context.SaveChangesAsync();
                    }
                    break;

                case "Institution":
                    var instituicao = await _context.Instituicao
                        .Include(i => i.Colaboradores) // Inclui colaboradores associados
                        .Include(i => i.ManualInstituicoes) // Inclui as entradas em ManualInstituicao
                        .FirstOrDefaultAsync(i => i.ID == userId);

                    if (instituicao != null)
                    {
                        // Remove os colaboradores associados
                        if (instituicao.Colaboradores != null && instituicao.Colaboradores.Any())
                        {
                            _context.Colaborador.RemoveRange(instituicao.Colaboradores);
                        }

                        // Remove as entradas de ManualInstituicao associadas
                        if (instituicao.ManualInstituicoes != null && instituicao.ManualInstituicoes.Any())
                        {
                            _context.ManualInstituicao.RemoveRange(instituicao.ManualInstituicoes);
                        }

                        // Remove a instituição
                        _context.Instituicao.Remove(instituicao);
                        await _context.SaveChangesAsync();
                    }
                    break;

                case "Collaborator":
                    var colaborador = await _context.Colaborador.FindAsync(userId);
                    if (colaborador != null)
                    {
                        _context.Colaborador.Remove(colaborador);
                        await _context.SaveChangesAsync();
                    }
                    break;

                default:
                    throw new ArgumentException("Tipo de utilizador inválido.");
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
            return await _context.Produto
                                 .Include(p => p.PecaProdutos)
                                 .ThenInclude(pp => pp.Peca)
                                 .ToDictionaryAsync(p => p.ID.ToString(), p => p);
        }

        public async Task<IUser?> ObterUtilizador(string codUtilizador, string tipoUtilizador)
        {
            int userId = int.Parse(codUtilizador);

            switch (tipoUtilizador)
            {
            case "Admin":
                return await _context.Administrador.FindAsync(userId) as IUser;

            case "Institution":
                return await _context.Instituicao
                .Include(i => i.Colaboradores)
                .Include(i => i.ManualInstituicoes)
                .FirstOrDefaultAsync(i => i.ID == userId) as IUser;

            case "Collaborator":
                return await _context.Colaborador.FindAsync(userId) as IUser;

            default:
                throw new ArgumentException("Tipo de utilizador inválido.");
            }
        }

        public async Task<Manual?> ObterManualPorId(int id)
        {
            return await _context.Manual
                .Include(m => m.Paginas) // Inclua as páginas se necessário
                .FirstOrDefaultAsync(m => m.ID == id);
        }

        public async Task<List<Page>> ListarPaginasPorManual(int manualId)
        {
            return await _context.Pagina
                .Where(p => p.ManualAssociado == manualId)
                .ToListAsync();
        }

        public async Task<bool> DisponibilidadeProduto(string codProduto)
        {
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

        public async Task ProcessaCompra(string codInstituicao, string codProduto, string enderecoEntrega)
        {
            var codInstituicaoParam = new SqlParameter("@codInstituicao", SqlDbType.Int) { Value = int.Parse(codInstituicao) };
            var codProdutoParam = new SqlParameter("@codProduto", SqlDbType.Int) { Value = int.Parse(codProduto) };
            var enderecoEntregaParam = new SqlParameter("@EnderecoEntrega", SqlDbType.VarChar, 45) { Value = enderecoEntrega };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC ProcessaCompra @codInstituicao = @codInstituicao, @codProduto = @codProduto, @EnderecoEntrega = @EnderecoEntrega",
                codInstituicaoParam,
                codProdutoParam,
                enderecoEntregaParam
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

        public async Task<Dictionary<string, Administrator>> ListaAdministradores()
        {
            return await _context.Administrador.ToDictionaryAsync(admin => admin.ID.ToString(), admin => admin);
        }

        public async Task<Dictionary<string, Instituition>> ListaInstituicoes()
        {
            return await _context.Instituicao.ToDictionaryAsync(inst => inst.ID.ToString(), inst => inst);
        }
        public async Task<Dictionary<string, Collaborator>> ListaColaboradores(int codInstituicao)
        {
            var colaboradores = await _context.Colaborador.Where(c => c.InstituicaoID == codInstituicao).ToListAsync();

            return colaboradores.ToDictionary(c => c.ID.ToString(), c => c);
        }

        public async Task<Dictionary<string, Manual>> ListaManuais(string codUtilizador, string tipoUtilizador)
        {
            var codUtilizadorParam = new SqlParameter("@codUtilizador", SqlDbType.Int) { Value = int.Parse(codUtilizador) };
            var tipoUtilizadorParam = new SqlParameter("@tipoUtilizador", SqlDbType.VarChar, 20) { Value = tipoUtilizador };

            var manuais = new Dictionary<string, Manual>();

            using (var connection = _context.Database.GetDbConnection())
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "EXEC ListarManuais @codUtilizador, @tipoUtilizador";
                    command.CommandType = CommandType.Text;

                    command.Parameters.Add(codUtilizadorParam);
                    command.Parameters.Add(tipoUtilizadorParam);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var id = reader["ID"].ToString();

                            if (!string.IsNullOrEmpty(id))
                            {
                                manuais[id] = new Manual
                                {
                                    ID = Convert.ToInt32(reader["ID"])!,
                                    Capa = reader["Capa"]?.ToString()!,
                                    Nome = reader["Nome"]?.ToString()!,
                                    Descricao = reader["Descricao"]?.ToString()!
                                };
                            }
                        }
                    }
                }
            }

            return manuais;
        }

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

        // Extras
        public async Task validateAdmin(string codUtilizador)
        {
            if (int.TryParse(codUtilizador, out int id))
            {
                string sql = "UPDATE Administrador SET Validado = 1 WHERE ID = @p0";
                await _context.Database.ExecuteSqlRawAsync(sql, id);
            }
            else
            {
                Console.WriteLine("Error validating admin: " + id);
            }
        }

    }
}
using Models.Entities;
using Microsoft.EntityFrameworkCore;
using Data;
using Microsoft.Data.SqlClient;
using System.Data;
using System.ComponentModel.DataAnnotations;

namespace Services
{
    public class LusiumService
    {
        private readonly LusiumDbContext _context;

        public LusiumService(LusiumDbContext context)
        {
            _context = context;
        }

        private bool IsNameValid(string name)
        {
            if (name == "")
            {
                return false;
            }

            foreach (char c in name)
            {
                if (!char.IsLetter(c))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsEmailValid(string email)
        {
            if (email == "")
            {
                return false;
            }

            var emailAttribute = new EmailAddressAttribute();
            if (!emailAttribute.IsValid(email))
            {
                return false;
            }
            
            if (email.IndexOf('@') == 0 || email.IndexOf('.') == 0 || email.IndexOf('@') == email.Length - 1 || email.IndexOf('.') == email.Length - 1)
            {
                return false;
            }

            return true;
        }

        private bool IsPasswordValid(string password)
        {
            if (password.Length < 8)
            {
                return false;
            }

            bool hasNumber = false;
            bool hasSpecialChar = false;
            foreach (char c in password)
            {
                if (char.IsDigit(c))
                {
                    hasNumber = true;
                }
                else if (!char.IsLetterOrDigit(c))
                {
                    hasSpecialChar = true;
                }
            }

            if (!hasNumber || !hasSpecialChar)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> ValidaDadosAdministrador(string nome, string email, string pp)
        {
            // name validation
            if (!IsNameValid(nome))
            {
                return false;
            }

            // email validation
            if (!IsEmailValid(email))
            {
                return false;
            }

            // password validation
            if (!IsPasswordValid(pp))
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
            if (nif == "" || numAssoc == "" || morada == "")
            {
                return false;
            }

            // name validation
            if (!IsNameValid(nome))
            {
                return false;
            }

            // NIF validation
            if (nif.Length != 9)
            {
                return false;
            }

            if (!int.TryParse(nif, out _))
            {
                return false;
            }

            // numAssoc validation
            if (numAssoc.Length != 9)
            {
                return false;
            }

            if (!int.TryParse(numAssoc, out _))
            {
                return false;
            }

            // email validation
            if (!IsEmailValid(email))
            {
                return false;
            }

            // password validation
            if (!IsPasswordValid(pp))
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
                NumeroDeAssociacao = int.Parse(numAssoc),
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
            if (codInstituicaoAssociada == "")
            {
                return false;
            }

            // name validation
            if (!IsNameValid(nome))
            {
                return false;
            }

            // email validation
            if (!IsEmailValid(email))
            {
                return false;
            }

            // birth date validation
            if (dataDeNascimento.Day < 1 || dataDeNascimento.Day > 31 || dataDeNascimento.Month < 1 || dataDeNascimento.Month > 12 || dataDeNascimento.Year < 1900 || dataDeNascimento.Year > DateTime.Now.Year - 18)
            {
                return false;
            }

            // password validation
            if (!IsPasswordValid(pp))
            {
                return false;
            }

            if (await _context.Colaborador.AnyAsync(i => i.Email == email))
            {
                return false;
            }

            return await _context.Instituicao.AnyAsync(i => i.NumeroDeAssociacao == int.Parse(codInstituicaoAssociada));
        }
        
        public async Task<Collaborator> RegistaColaborador(string nome, string email, DateOnly dataDeNascimento, string codInstituicaoAssociada, string pp)
        {
            var colab = new Collaborator
            {
                Nome = nome,
                Email = email,
                DataDeNascimento = dataDeNascimento,
                Instituicao = int.Parse(codInstituicaoAssociada),
                PalavraPasse = pp
            };

            _context.Colaborador.Add(colab);
            await _context.SaveChangesAsync();

            return colab;
        }

        public async Task<bool> ValidaCredenciais(string email, string pp)
        {
            // email validation
            if (!IsEmailValid(email))
            {
                return false;
            }

            // password validation
            if (!IsPasswordValid(pp))
            {
                return false;
            }

            // at least one user with the given credentials
            return await _context.Administrador.AnyAsync(a => a.Email == email && a.PalavraPasse == pp) ||
                   await _context.Instituicao.AnyAsync(i => i.Email == email && i.PalavraPasse == pp) ||
                   await _context.Colaborador.AnyAsync(c => c.Email == email && c.PalavraPasse == pp);
        }

        //public async Task AutenticaUtilizador(string codUtilizador)
        //{}

        public async Task<bool> ValidaNovaPP(string codUtilizador, string novaPP)
        {
            // password validation
            if (!IsPasswordValid(novaPP))
            {
                return false;
            }

            // the new password is different from the current one
            return !await _context.Administrador.AnyAsync(a => a.ID == int.Parse(codUtilizador) && a.PalavraPasse == novaPP);
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

        //public async Task RemoverAutenticacao(string codUtilizador)
        //{}

        public async Task<bool> ValidaNovosDadosAdministrador(string codAdministrador, string novoNome, string novaPP)
        {
            var administrador = await _context.Administrador.FirstOrDefaultAsync(a => a.ID == int.Parse(codAdministrador));
            
            // no administrator found
            if (administrador == null)
            {
                return false;
            }

            if (administrador.Nome != novoNome && !IsNameValid(novoNome))
            {
                return false;
            
            }

            if (administrador.PalavraPasse != novaPP && !IsPasswordValid(novaPP))
            {
                return false;
            }

            return true;
        }

        public async Task<bool> ValidaPP(string codUtilizador, string pp)
        {
            if (!IsPasswordValid(pp))
            {
                return false;
            }

            return await _context.Administrador.AnyAsync(a => a.ID == int.Parse(codUtilizador) && a.PalavraPasse == pp);
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

            if (instituicao.Nome != novoNome && !IsNameValid(novoNome))
            {
                return false;
            }

            if (instituicao.Morada != novaMorada && novaMorada == "")
            {
                return false;
            }

            if (instituicao.PalavraPasse != novaPP && !IsPasswordValid(novaPP))
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

            if (colab.Nome != novoNome && !IsNameValid(novoNome))
            {
                return false;
            }

            if (colab.DataDeNascimento != novaDataNascimento && (novaDataNascimento.Day < 1 || novaDataNascimento.Day > 31 || novaDataNascimento.Month < 1 || novaDataNascimento.Month > 12 || novaDataNascimento.Year < 1900 || novaDataNascimento.Year > DateTime.Now.Year - 18))
            {
                return false;
            }

            if (colab.Instituicao != int.Parse(codNovaInstituicao) && !await _context.Instituicao.AnyAsync(i => i.NumeroDeAssociacao == int.Parse(codNovaInstituicao)))
            {
                return false;
            }

            if (colab.PalavraPasse != novaPP && !IsPasswordValid(novaPP))
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
            if (colab.Nome == novoNome && colab.DataDeNascimento == novaDataNascimento && colab.Instituicao == int.Parse(codNovaInstituicao) && colab.PalavraPasse == novaPP)
            {
                return;
            }

            if (colab.Nome != novoNome)
            {
                colab.Nome = novoNome;
            }

            if (colab.DataDeNascimento != novaDataNascimento)
            {
                colab.DataDeNascimento = novaDataNascimento;
            }

            if (colab.Instituicao != int.Parse(codNovaInstituicao))
            {
                colab.Instituicao = int.Parse(codNovaInstituicao);
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
            if (tipoUtilizador == "Administrador")
            {
                var admin = await _context.Administrador.FirstOrDefaultAsync(a => a.ID == int.Parse(codUtilizador));
                if (admin != null)
                {
                    _context.Administrador.Remove(admin);
                    await _context.SaveChangesAsync();
                    return;
                }
                return;
            }

            // in case the user is an institution
            if (tipoUtilizador == "Instituição")
            {
                var inst = await _context.Instituicao.FirstOrDefaultAsync(i => i.ID == int.Parse(codUtilizador));
                if (inst != null)
                {
                    // remove all the entries on table ManualDaInstituicao whose IDInstituicao is the same as inst.ID
                    await _context.Database.ExecuteSqlRawAsync("DELETE FROM ManualDaInstituicao WHERE IDInstituicao = {0}", inst.ID);

                    _context.Instituicao.Remove(inst);
                    await _context.SaveChangesAsync();
                    return;
                }
                return;
            }

            // in case the user is a collaborator
            if (tipoUtilizador == "Colaborador")
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
            if (tipoUtilizador == "Administrador")
            {
                return await _context.Administrador.ToDictionaryAsync(a => a.ID.ToString(), a => (IUser)a);
            }

            if (tipoUtilizador == "Instituição")
            {
                return await _context.Instituicao.ToDictionaryAsync(i => i.ID.ToString(), i => (IUser)i);
            }

            if (tipoUtilizador == "Colaborador")
            {
                return await _context.Colaborador.ToDictionaryAsync(c => c.ID.ToString(), c => (IUser)c);
            }

            return new Dictionary<string, IUser>();
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
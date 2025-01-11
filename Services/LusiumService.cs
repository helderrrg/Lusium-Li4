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

        private bool IsEmailValid(string email)
        {
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
            if (nome == "" || email == "" || pp == "")
            {
                return false;
            }

            // name validation
            foreach (char c in nome)
            {
                if (!char.IsLetter(c))
                {
                    return false;
                }
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
            if (nome == "" || nif == "" || numAssoc == "" || email == "" || morada == "" || pp == "")
            {
                return false;
            }

            // name validation
            foreach (char c in nome)
            {
                if (!char.IsLetter(c))
                {
                    return false;
                }
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
            if (nome == "" || email == "" || codInstituicaoAssociada == "" || pp == "")
            {
                return false;
            }

            // name validation
            foreach (char c in nome)
            {
                if (!char.IsLetter(c))
                {
                    return false;
                }
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

        public async void AutenticaUtilizador(string codUtilizador)
        {}

        public async Task<bool> ValidaNovaPP(string codUtilizador, string novaPP)
        {
            // password validation
            if (!IsPasswordValid(novaPP))
            {
                return false;
            }

            // the new password is different from the current one
            var codUtilizadorInt = int.Parse(codUtilizador);
            return !await _context.Administrador.AnyAsync(a => a.ID == codUtilizadorInt && a.PalavraPasse == novaPP);
        }

        public async void AtualizaPP(string codUtilizador, string novaPP)
        {
            var admin = await _context.Administrador.FirstOrDefaultAsync(a => a.ID == int.Parse(codUtilizador));
            if (admin != null)
            {
                admin.PalavraPasse = novaPP;
                await _context.SaveChangesAsync();
            }
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
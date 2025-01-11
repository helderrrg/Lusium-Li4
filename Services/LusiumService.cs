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
            var emailAttribute = new EmailAddressAttribute();
            if (!emailAttribute.IsValid(email))
            {
                return false;
            }
            
            if (email.IndexOf('@') == 0 || email.IndexOf('.') == 0 || email.IndexOf('@') == email.Length - 1 || email.IndexOf('.') == email.Length - 1)
            {
                return false;
            }

            // password validation
            if (pp.Length < 8)
            {
                return false;
            }

            bool hasNumber = false;
            bool hasSpecialChar = false;
            foreach (char c in pp)
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

            var admin = await _context.Administrador.FirstOrDefaultAsync(a => a.Email == email);
            return admin == null;
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
            var emailAttribute = new EmailAddressAttribute();
            if (!emailAttribute.IsValid(email))
            {
                return false;
            }
            
            if (email.IndexOf('@') == 0 || email.IndexOf('.') == 0 || email.IndexOf('@') == email.Length - 1 || email.IndexOf('.') == email.Length - 1)
            {
                return false;
            }

            // password validation
            if (pp.Length < 8)
            {
                return false;
            }

            bool hasNumber = false;
            bool hasSpecialChar = false;
            foreach (char c in pp)
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

            var inst = await _context.Instituicao.FirstOrDefaultAsync(i => i.Email == email);
            return inst == null;
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
            var emailAttribute = new EmailAddressAttribute();
            if (!emailAttribute.IsValid(email))
            {
                return false;
            }
            
            if (email.IndexOf('@') == 0 || email.IndexOf('.') == 0 || email.IndexOf('@') == email.Length - 1 || email.IndexOf('.') == email.Length - 1)
            {
                return false;
            }

            // birth date validation
            if (dataDeNascimento.Day < 1 || dataDeNascimento.Day > 31 || dataDeNascimento.Month < 1 || dataDeNascimento.Month > 12 || dataDeNascimento.Year < 1900 || dataDeNascimento.Year > DateTime.Now.Year - 18)
            {
                return false;
            }

            // password validation
            if (pp.Length < 8)
            {
                return false;
            }

            bool hasNumber = false;
            bool hasSpecialChar = false;
            foreach (char c in pp)
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

            var colab = await _context.Colaborador.FirstOrDefaultAsync(i => i.Email == email);
            if (colab != null)
            {
                return false;
            }

            var inst = await _context.Instituicao.FirstOrDefaultAsync(i => i.NumeroDeAssociacao == int.Parse(codInstituicaoAssociada));
            return inst != null;
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
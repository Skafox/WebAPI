using Domain.Enum;
using Domain.Model;
using Microsoft.IdentityModel.Tokens;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace Service
{
    public class JWTAuthenticationService : ITokenService
    {
        // Chave privada deve ficar somente no servidor e bem protegida
        public const string CommunicationKey = "you-will-be-baked-and-then-there-will-be-cake";

        // Audiência do jwt
        public const string Audience = "jwt-audience-exercicio";

        // Issue do jwt
        public const string Issuer = "jwt-issue-exercicio";

        private IPermissionService _permissionService {get; set;}

        public JWTAuthenticationService(IPermissionService permissionService)
        {
            this._permissionService = permissionService;
        }

        // Chave de segurança simétrica a partir da CommunicationKey
        public SecurityKey SigningKey => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(CommunicationKey));

        public string AuthenticateUser(User user)
        {            
            // Monta lista de permissões disponíveis
            Dictionary<string, SystemPermissionEnum> rolesName = this._permissionService.GetPermissionDictionary();

            // Avalia quais permissões o usuário possui
            IEnumerable<string> userPermissions = rolesName
                .Where(d => d.Key == user.Permission.ToString())
                // Monta coleção com apenas valor da enum de permissões
                .Select(d => ((int)d.Value).ToString());

            // Retorna token do usuário para gravação no storage do browser
            return GenerateTokenForUser(user, userPermissions);
        }

        /// <summary>
        /// Método utilizado para gerar token para usuário
        /// </summary>
        public string GenerateTokenForUser(User user, IEnumerable<string> userPermissions)
        {
            // Define o tempo de expiração do token de acordo com a configuração de RememberMe (Manter Logado)
            var tempoExpiracaoMinutos = 2880;

            // Gera credenciais para o token
            var signingCredentials = new SigningCredentials(SigningKey,
               SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest);

            // Cria header do JWT
            var header = new JwtHeader(signingCredentials);

            // Carrega as permissões que o usuário em questão possui no sistema
            List<Claim> claimsIdentity = GetPermissionUserClaims(user, userPermissions);

            // Cria payload com dados do JWT
            var payload = new JwtPayload(Issuer, Audience, claimsIdentity, null, DateTime.Now.AddMinutes(tempoExpiracaoMinutos));

            // Monta JWT
            var secToken = new JwtSecurityToken(header, payload);
            var handler = new JwtSecurityTokenHandler();

            // Obtem string para ser enviada para o cliente
            var tokenString = handler.WriteToken(secToken);

            return tokenString;
        }

        /// <summary>
        /// Retorna uma lista de Claim do tipo Role com as permissões de um Usuário
        /// </summary>
        private List<Claim> GetPermissionUserClaims(User usuario, IEnumerable<string> userPermissions)
        {
            // Cria uma coleção de Claims para cada permissão
            List<Claim> claimsIdentity = new List<Claim>();

            // Cria as demais claims do usuário, além das permissões configuradas no sistema           
            claimsIdentity.Add(new Claim(ClaimTypes.NameIdentifier, usuario.ID.ToString()));
            claimsIdentity.Add(new Claim(ClaimTypes.Name, usuario.Username ?? string.Empty));
            //claimsIdentity.Add(new Claim(ClaimTypes.Email, usuario.EmailAddress ?? string.Empty));

            // Para cada role recuperado do usuário, adiciona um claim ao token
            foreach (var permission in userPermissions)
            {
                // Cria um claim para o role em questao
                Claim claim = new Claim(ClaimTypes.Role, permission);

                // Adiciona se não existe
                if (!claimsIdentity.Any(c => c.Type == ClaimTypes.Role && c.Value == permission))
                {
                    claimsIdentity.Add(claim);
                }
            }

            return claimsIdentity;
        }

        /// <summary>
        /// Retorna um TokenValidationParameters para validar o Token JWT
        /// </summary>
        /// <returns></returns>
        public TokenValidationParameters GetTokenValidationParameters()
        {
            // Cleanup
            return new TokenValidationParameters
            {
                ValidAudience = Audience,
                ValidIssuer = Issuer,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                LifetimeValidator = LifetimeValidator,
                IssuerSigningKey = this.SigningKey
            };
        }

        /// <summary>
        /// Retorna se é válido o tempo de vida do token
        /// </summary>
        /// <returns></returns>
        private bool LifetimeValidator(
            DateTime? notBefore,
            DateTime? expires,
            SecurityToken securityToken,
            TokenValidationParameters validationParameters)
        {
            var valid = false;

            // Additional checks can be performed on the SecurityToken or the validationParameters.
            if ((expires.HasValue && DateTime.UtcNow < expires)
             && (notBefore.HasValue && DateTime.UtcNow > notBefore))
            { valid = true; }

            return valid;
        }

    }
}

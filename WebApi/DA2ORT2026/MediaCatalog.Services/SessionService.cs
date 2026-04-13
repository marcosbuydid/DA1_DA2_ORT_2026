
using MediaCatalog.DataAccess.Interfaces;
using MediaCatalog.Domain;
using MediaCatalog.Services.Exceptions;
using MediaCatalog.Services.Interfaces;
using MediaCatalog.Services.Models;
using MediaCatalog.Services.Settings;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace MediaCatalog.Services
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISecureDataService _secureDataService;
        private readonly SystemSettings _settings;
        private readonly ITokenService _jwtService;
        private JsonElement tokenPayload;
        private UserDetailDTO _loggedUser;
        private SessionDTO? _currentSession;

        public SessionService(ISessionRepository sessionRepository,
            IUserRepository userRepository, ISecureDataService secureDataService,
            IOptions<SystemSettings> options, ITokenService jwtService)
        {
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
            _secureDataService = secureDataService;
            _settings = options.Value;
            _jwtService = jwtService;
        }

        public string Authenticate(string email, string password)
        {
            User user = ValidateUserCredentials(email, password);

            if (user != null)
            {
                string token = _jwtService.GenerateToken(user.Name, user.Email,
                    _settings.SecretKey, _settings.TokenExpMinutes);

                Session session = new Session() { Token = token, User = user, 
                    CreatedAt = DateTime.UtcNow };
                _sessionRepository.AddSession(session);

                return token;
            }

            return null;
        }

        public SessionDTO? ValidateSession(string token)
        {
            bool validSession = _jwtService.ValidateToken(token, _settings.SecretKey,
                _settings.Issuer, _settings.Audience, out tokenPayload);

            if (!validSession)
            {
                ClearSessionData();
                return null;
            }

            //extract user info from token payload
            string? userName = tokenPayload.GetProperty("name").GetString();
            string? userLastName = tokenPayload.GetProperty("lastName").GetString();
            string? userEmail = tokenPayload.GetProperty("email").GetString();
            int? userRoleId = tokenPayload.GetProperty("roleId").GetInt32();
            string? userRoleName = tokenPayload.GetProperty("roleName").GetString();

            _loggedUser = new UserDetailDTO()
            {
                Name = userName,
                LastName = userLastName,
                Email = userEmail,
                RoleId = (int)userRoleId
            };

            //create the sessionDTO
            _currentSession = new SessionDTO()
            {
                Token = token,
                LoggedUser = _loggedUser,
                LoggedUserRoleName = userRoleName
            };

            return _currentSession;
        }

        private User ValidateUserCredentials(string email, string password)
        {
            User? user = _userRepository.GetUsers()
                  .FirstOrDefault(u => u.Email == email);

            if (user is null || !_secureDataService.CompareHashes(user.Password, password))
            {
                throw new ServiceException("User or password is incorrect, try again");
            }

            return user;
        }

        private void ClearSessionData()
        {
            _currentSession = null;
        }
    }
}

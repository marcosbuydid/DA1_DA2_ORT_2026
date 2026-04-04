
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

        public void Authenticate(string email, string password)
        {
            if (_currentSession == null)
            {
                User user = ValidateUserCredentials(email, password);

                if (user != null)
                {
                    string token = _jwtService.GenerateToken(user.Name, user.Email, _settings.SecretKey);

                    Session session = new Session() { Token = token, User = user };
                    _sessionRepository.AddSession(session);

                    _loggedUser = new UserDetailDTO()
                    {
                        Name = user.Name,
                        LastName = user.LastName,
                        Email = user.Email,
                        RoleId = (int)user.Role.Id
                    };

                    _currentSession = new SessionDTO()
                    {
                        Token = token,
                        LoggedUser = _loggedUser,
                        LoggedUserRoleName = user.Role.Name
                    };
                }
            }
        }

        public SessionDTO? ValidateSession(string token)
        {
            bool validSession = _jwtService.ValidateToken(token, _settings.SecretKey, out tokenPayload);

            if (!validSession)
            {
                ClearSessionData();
            }

            return _currentSession;
        }

        public void SignOut()
        {
            ClearSessionData();
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

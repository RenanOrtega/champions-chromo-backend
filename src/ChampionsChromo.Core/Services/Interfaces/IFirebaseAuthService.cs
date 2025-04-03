using FirebaseAdmin.Auth;

namespace ChampionsChromo.Core.Services.Interfaces;

public interface IFirebaseAuthService
{
    Task<UserRecord> VerifyAndGetUserAsync(string token);
}

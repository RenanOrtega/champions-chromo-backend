using ChampionsChromo.Core.Services.Interfaces;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;

namespace ChampionsChromo.Core.Services;

public class FirebaseAuthService : IFirebaseAuthService
{
    private readonly FirebaseAuth _firebaseAuth;

    public FirebaseAuthService(IConfiguration configuration)
    {
        try
        {
            // Verificar se já existe uma instância do FirebaseApp
            if (FirebaseApp.DefaultInstance == null)
            {
                // Primeiro tenta usar a configuração do appsettings.json
                string credentialPath = configuration["Firebase:CredentialsPath"];

                if (!string.IsNullOrEmpty(credentialPath))
                {
                    // Verifica se o caminho é relativo (sem diretório)
                    if (!Path.IsPathRooted(credentialPath))
                    {
                        // Constrói o caminho absoluto baseado no diretório da aplicação
                        credentialPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, credentialPath);
                    }

                    Console.WriteLine($"Tentando carregar credenciais do Firebase de: {credentialPath}");

                    // Verifica se o arquivo existe
                    if (File.Exists(credentialPath))
                    {
                        // Carrega explicitamente o arquivo de credenciais
                        var credential = GoogleCredential.FromFile(credentialPath);
                        FirebaseApp.Create(new AppOptions
                        {
                            Credential = credential
                        });

                        Console.WriteLine("Credenciais do Firebase carregadas com sucesso!");
                    }
                    else
                    {
                        throw new FileNotFoundException($"Arquivo de credenciais não encontrado em: {credentialPath}");
                    }
                }
                else
                {
                    // Tenta usar a variável de ambiente como fallback
                    string envCredentialPath = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");

                    if (!string.IsNullOrEmpty(envCredentialPath))
                    {
                        Console.WriteLine($"Usando variável de ambiente GOOGLE_APPLICATION_CREDENTIALS: {envCredentialPath}");

                        // Quando usando a variável de ambiente, o FirebaseApp irá encontrar o arquivo automaticamente
                        FirebaseApp.Create(new AppOptions());

                        Console.WriteLine("Credenciais do Firebase carregadas com sucesso via variável de ambiente!");
                    }
                    else
                    {
                        throw new InvalidOperationException("Não foi possível encontrar configurações para Firebase. Configure em appsettings.json ou defina a variável GOOGLE_APPLICATION_CREDENTIALS.");
                    }
                }
            }

            _firebaseAuth = FirebaseAuth.DefaultInstance;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao inicializar FirebaseApp: {ex.Message}");
            throw;
        }
    }

    public async Task<UserRecord> VerifyAndGetUserAsync(string token)
    {
        try
        {
            FirebaseToken decodedToken = await _firebaseAuth.VerifyIdTokenAsync(token);
            string uid = decodedToken.Uid;
            return await _firebaseAuth.GetUserAsync(uid);
        }
        catch (Exception ex)
        {
            throw new UnauthorizedAccessException("Token inválido ou expirado", ex);
        }
    }
}


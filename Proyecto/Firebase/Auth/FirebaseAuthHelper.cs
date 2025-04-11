using Firebase.Auth.Providers;
using Firebase.Auth;

namespace Proyecto.Firebase
{
    public static class FirebaseAuthHelper
    {
        public const string firebaseAppId = "proyecto-grupo1-47c47";
        public const string firebaseApiKey = "AIzaSyCOzUdW1vD9xqoUIJsidIGFQ70KSZV3sXE";

        public static FirebaseAuthClient setFirebaseAuthClient()
        {
            var auth = new FirebaseAuthClient(new FirebaseAuthConfig()
            {
                ApiKey = firebaseApiKey,
                AuthDomain = $"{ firebaseAppId }.firebaseapp.com",
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider()
                }
            });

            return auth;
        }
    }
}
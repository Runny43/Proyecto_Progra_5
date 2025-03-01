using Firebase.Auth.Providers;
using Firebase.Auth;

namespace Proyecto.Firebase
{
    public static class FirebaseAuthHelper
    {
        public const string firebaseAppId = "proyecto-grupo1-f4be6";
        public const string firebaseApiKey = "AIzaSyAXrKaucDXX_9sR8ucXpqrJ6E7nsJeEyYA";

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

using Firebase.Auth;
using Google.Cloud.Firestore;
using Proyecto.Firebase;
namespace Proyecto.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }
    }

    public class UserHelper
    {
        public async Task<UserModel> getUserInfo(string email)
        {
            Query query = FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId).Collection("User").WhereEqualTo("email", email);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            Dictionary<string, object> data = querySnapshot.Documents[0].ToDictionary();

            UserModel user = new UserModel
            {
                Email = data["email"].ToString(),
                Name = data["name"].ToString(),
                Type = data["type"].ToString()
            };

            return user;
        }

        public async void postUserWithEmailAndPassword(string email, string password, string displayName, string type)
        {
            UserCredential userCredential = await FirebaseAuthHelper.setFirebaseAuthClient().CreateUserWithEmailAndPasswordAsync(email, password, displayName);

            DocumentReference docRef = await FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId).Collection("User").AddAsync(
                    new Dictionary<string, object>
                        {
                            {"email", email },
                            {"name", displayName },
                            {"type", type},
                        });
        }

    }
}

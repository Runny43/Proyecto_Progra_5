using Firebase.Auth;
using Google.Api;
using Google.Cloud.Firestore;
using Proyecto.Firebase;
using Proyecto.Mic;
using static Proyecto.Mic.AppHelper;
namespace Proyecto.Models
{
    public class UserModel
    {
        public string uuid { get; set; }
        public string Email { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public List<Propertie> Properties { get; set; }
    }

    public class Propertie
    {
        public string CondoName { get; set; }
        public int CondoNumber { get; set; }
    }
    public class UserHelper
    {
        
        public static async Task<UserModel> getUserInfo(string email)
        {
            Query query = FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId).Collection("User").WhereEqualTo("email", email);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            Dictionary<string, object> data = querySnapshot.Documents[0].ToDictionary();

            UserModel user = new UserModel
            {
                uuid = querySnapshot.Documents[0].Id.ToString(),
                Email = data["email"].ToString(),
                Name = data["name"].ToString(),
                Type = data["type"].ToString(),
            };

            user.Properties = new List<Propertie>();

            try
            {
                List<Object> propertieList = (List<Object>)data["properties"];

                foreach (Object propertie in propertieList)
                {
                    Dictionary<string, object> propertieData = (Dictionary<string, object>)propertie;

                    user.Properties.Add(new Propertie
                    {
                        CondoName = propertieData["condo"].ToString(),
                        CondoNumber = Convert.ToInt16(propertieData["number"])
                    });
                }
            }
            catch
            {
            }

            return user;
        }

        public static async Task<List<UserModel>> getOwners()
        {
            List<UserModel> ownerList = new List<UserModel>();

            Query query = FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId).Collection("User");
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            foreach (var item in querySnapshot)
            {
                Dictionary<string, object> data = item.ToDictionary();

                UserModel owner = new UserModel
                {
                    uuid = item.Id,
                    Name = data["name"].ToString(),
                    Email = data["email"].ToString(),
                    Type = data["type"].ToString(),
                };


                try
                {
                    List<Object> propertieList = (List<Object>)data["properties"];

                    if (propertieList.Count > 0)
                    {
                        owner.Properties = new List<Propertie>();

                        foreach (Object propertie in propertieList)
                        {
                            Dictionary<string, object> propertieData = (Dictionary<string, object>)propertie;

                            owner.Properties.Add(new Propertie
                            {
                                CondoName = propertieData["condo"].ToString(),
                                CondoNumber = Convert.ToInt16(propertieData["number"])
                            });
                        }
                    }
                }
                catch
                {

                }

                ownerList.Add(owner);

            }

            return ownerList;
        }


        //public async void postUserWithEmailAndPassword(string email, string password, string displayName, string type, string selCondo, int selCondoNumber)
        //{
        //    UserCredential userCredential = await FirebaseAuthHelper.setFirebaseAuthClient().CreateUserWithEmailAndPasswordAsync(email, password, displayName);

        //    List<Dictionary<string, object>> objectProperties = new List<Dictionary<string, object>>
        //        {
        //            new Dictionary<string, object>
        //            {
        //                { "condo", selCondo },
        //                { "number", selCondoNumber }
        //            }
        //        };

        //    DocumentReference docRef = await FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId).Collection("User").AddAsync(
        //            new Dictionary<string, object>
        //                {
        //                    {"email", email },
        //                    {"name", displayName },
        //                    {"type", type},
        //                    {"properties", objectProperties }
        //                });

        //    AppHelper.EmailHelper.SendEmail(email, displayName, password, selCondo, selCondoNumber);
        //}

        public static async void postUserWithEmailAndPassword(string email, string password, string displayName, string type, string selCondo, int selCondoNumber)
        {
            UserCredential userCredential = await FirebaseAuthHelper.setFirebaseAuthClient().CreateUserWithEmailAndPasswordAsync(email, password, displayName);

            List<Dictionary<string, object>> objectProperties = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object>
                    {
                        { "condo", selCondo },
                        { "number", selCondoNumber }
                    }
                };

            DocumentReference docRef = await FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId).Collection("User").AddAsync(
                    new Dictionary<string, object>
                        {
                            {"email", email },
                            {"name", displayName },
                            {"type", type},
                            {"properties", objectProperties }
                        });

            EmailHelper.SendEmail(email, displayName, password, selCondo, selCondoNumber);
        }


        public static async void editOwner(string uuid, string email, string displayName, string selCondo, int selCondoNumber)
        {
            try
            {
                List<Dictionary<string, object>> objectProperties = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object>
                    {
                        { "condo", selCondo },
                        { "number", selCondoNumber }
                    }
                };

                DocumentReference docRef = FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId).Collection("User").Document(uuid);
                Dictionary<string, object> dataToUpdate = new Dictionary<string, object>
                {
                    {"name", displayName },
                    {"properties", objectProperties }
                };

                WriteResult result = await docRef.UpdateAsync(dataToUpdate);

                Thread.Sleep(3000);
            }
            catch
            {

            }

        }
    }
}

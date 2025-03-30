using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Proyecto.Firebase;

namespace Proyecto.Models
{
    public class Condominium
    {
        public int Id { get; set; }

        public String Name { get; set; }

        public String Address { get; set; }

        public int Count { get; set; }

        public String Photo { get; set; }
    }

    public class CondominiumHelper
    {
        public static async Task<List<Condominium>> getCondominiums()
        {
            List<Condominium> condominiumList = new List<Condominium>();

            Query query = FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId).Collection("Condominium");
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            foreach (var item in querySnapshot)
            {
                Dictionary<string, object> data = item.ToDictionary();

                condominiumList.Add(new Condominium
                {
                    Name = data["Name"].ToString(),
                    Address = data["Address"].ToString(),
                    Count = Convert.ToInt32(data["Count"]),
                    Photo = data["Photo"].ToString(),
                });
            }

            return condominiumList;
        }

        public async Task<bool> saveCondominium(Condominium condominium)
        {
            try
            {
                FirestoreDb db = FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId);

                CollectionReference coll = db.Collection("Condominium");

                Dictionary<string, object> newCondo = new Dictionary<string, object>
                {
                     { "Name", condominium.Name },
                        { "Address", condominium.Address },
                        { "Count", condominium.Count },
                        { "Photo", condominium.Photo }
                };

                await coll.AddAsync(newCondo);

                //DocumentReference docRef = await FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId).Collection("Condominiums").AddAsync(
                //    new Dictionary<string, object>
                //    {
                //        { "Name", condominium.Name },
                //        { "Address", condominium.Address },
                //        { "Count", condominium.Count },
                //        { "Photo", condominium.Photo } 
                //    });
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

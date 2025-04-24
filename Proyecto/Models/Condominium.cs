using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Proyecto.Firebase;

namespace Proyecto.Models
{
    public class Condominium
    {
        public string Id { get; set; }

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
                    Id= item.Id,
                    Name = data["Name"].ToString(),
                    Address = data["Address"].ToString(),
                    Count = Convert.ToInt32(data["Count"]),
                    Photo = data["Photo"].ToString(),
                });
            }

            return condominiumList;
        }
        public static async Task<List<Condominium>> getCondominium(string name)
        {
            List<Condominium> condominiumList = new List<Condominium>();

            Query query = FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId).Collection("Condominium").WhereEqualTo("Name", name);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();


            foreach(var item in querySnapshot)
            {
                Dictionary<string, object> data = item.ToDictionary();

                condominiumList.Add(new Condominium
                {
                    Id= item.Id,
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

                
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static async Task<bool> DeleteCondo(string uuid)
        {
            try
            {

                DocumentReference docRef = FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId).Collection("Condominium").Document(uuid);
                await docRef.DeleteAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar owner: {ex.Message}");
                return false;
            }
        }
        public static async void editCondo(string id, string name, string address, int count, string photo)
        {
            try
            {
                DocumentReference docRef = FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId).Collection("Condominium").Document(id);
                Dictionary<string, object> dataToUpdate = new Dictionary<string, object>
                {
                    { "Name", name },
                        { "Address", address },
                        { "Count", count },
                        { "Photo", photo }
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

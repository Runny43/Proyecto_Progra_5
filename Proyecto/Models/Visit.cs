using Google.Cloud.Firestore;
using Proyecto.Firebase;

namespace Proyecto.Models
{
    public class VisitModel
    {
        public string uuid { get; set; }
        public string Id_Card { get; set; }
        public string Name { get; set; }
        public string To { get; set; }
        public string Type { get; set; }
        public List<VisitorVehicle> Vehicles { get; set; }
    }
    public class VisitorVehicle
    {
        public string Plate { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
    }

    public static async Task<VisitModel> getVisit(string name)
        {
            Query query = FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId).Collection("Visits").WhereEqualTo("name", name);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            Dictionary<string, object> data = querySnapshot.Documents[0].ToDictionary();

            VisitModel visit = new VisitModel
            {
                uuid = querySnapshot.Documents[0].Id.ToString(),
                Id_Card = data["id_Card"].ToString(),
                To= data["name"].ToString(),
                Name = data["name"].ToString(),
                Type = data["type"].ToString(),
            };

           

            try
            {

                List<Object> vehicleList = (List<Object>)data["vehicles"];

                foreach (Object vehicle in vehicleList)
                {
                    Dictionary<string, object> vehicleData = (Dictionary<string, object>)vehicle;

                    visit.Vehicles.Add(new VisitorVehicle
                    {
                        Plate = vehicleData["plate"].ToString(),
                        Brand = vehicleData["brand"].ToString(),
                        Model = vehicleData["model"].ToString(),
                        Color = vehicleData["color"].ToString(),
                    });
                }
            }
            catch
            {
            }

            return visit;
        }

    }
}

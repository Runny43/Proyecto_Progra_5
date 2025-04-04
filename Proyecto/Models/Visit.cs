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
    public class VisitHelper
    {
        public static async Task<List<VisitModel>> getVisit(string name)
        {
            List<VisitModel> visitInfo = new List<VisitModel>(); 

            Query query = FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId).Collection("Visits").WhereEqualTo("To", name);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            VisitModel visit = new VisitModel();

            foreach (var item in querySnapshot)
            {
                Dictionary<string, object> data = item.ToDictionary();

                visitInfo.Add(new VisitModel
                {
                    Id_Card= data["id_Card"].ToString(),
                    To = data["To"].ToString(),
                    Name= data["name"].ToString(),
                    Type= data["type"].ToString(),
                });

                
                visit.Vehicles = new List<VisitorVehicle>();

                try
                {

                    List<Object> vehicleList = (List<Object>)data["vehicle"];

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
                    visitInfo.Add(visit);
                }

                catch
                {
                }
            }

            
            

            return visitInfo;
        }
        public static async Task<List<VisitModel>> getAllVisits()
        {
            List<VisitModel> visitsList = new List<VisitModel>();

            Query query = FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId).Collection("Visits");
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            foreach (var item in querySnapshot)
            {
                Dictionary<string, object> data = item.ToDictionary();

                VisitModel visitor = new VisitModel
                {
                    uuid = item.Id,
                    Id_Card = data["id_Card"].ToString(),
                    To = data["name"].ToString(),
                    Name = data["name"].ToString(),
                    Type = data["type"].ToString(),
                };


                try
                {
                    List<Object> vehicleList = (List<Object>)data["vehicle"];

                    if (vehicleList.Count > 0)
                    {
                        visitor.Vehicles = new List<VisitorVehicle>();

                        foreach (Object vehicle in vehicleList)
                        {
                            Dictionary<string, object> vehicleData = (Dictionary<string, object>)vehicle;

                            visitor.Vehicles.Add(new VisitorVehicle
                            {
                                Plate = vehicleData["plate"].ToString(),
                                Brand = vehicleData["brand"].ToString(),
                                Model = vehicleData["model"].ToString(),
                                Color = vehicleData["color"].ToString(),
                            });
                        }
                    }
                }
                catch
                {

                }

                visitsList.Add(visitor);

            }

            return visitsList;
        }
    }
}

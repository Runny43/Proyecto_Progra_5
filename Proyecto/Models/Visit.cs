using Google.Cloud.Firestore;
using Proyecto.Firebase;
using Proyecto.Mic;

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
            

            foreach (var item in querySnapshot)
            {
                Dictionary<string, object> data = item.ToDictionary();
                VisitModel visit = new VisitModel
                {
                    uuid = item.Id,
                    Id_Card = data["id_Card"].ToString(),
                    To = data["To"].ToString(),
                    Name = data["name"].ToString(),
                    Type = data["type"].ToString(),
                };

                
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

        public static async Task<List<VisitModel>> getDelivery(string name)
        {
            List<VisitModel> deliveryInfo = new List<VisitModel>();

            Query query = FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId).Collection("Deliverys").WhereEqualTo("To", name);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();


            foreach (var item in querySnapshot)
            {
                Dictionary<string, object> data = item.ToDictionary();
                VisitModel visit = new VisitModel
                {
                    uuid = item.Id,
                    To = data["To"].ToString(),
                    Name = data["name"].ToString(),
                    Type = data["type"].ToString(),
                };
                deliveryInfo.Add(visit);
                
            }

            return deliveryInfo;
        }

        public static async Task<List<VisitModel>> getDeliveryToEdit(string name)
        {
            List<VisitModel> deliveryInfo = new List<VisitModel>();

            Query query = FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId).Collection("Deliverys").WhereEqualTo("name", name);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();


            foreach (var item in querySnapshot)
            {
                Dictionary<string, object> data = item.ToDictionary();
                VisitModel visit = new VisitModel
                {
                    uuid = item.Id,
                    To = data["To"].ToString(),
                    Name = data["name"].ToString(),
                    Type = data["type"].ToString(),
                };
                deliveryInfo.Add(visit);

            }

            return deliveryInfo;
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

        public static async void postDelivery(string to, string name, string type)
        {
            
            DocumentReference docRef = await FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId).Collection("Deliverys").AddAsync(
                    new Dictionary<string, object>
                        {
                            {"To", to },
                            {"name", name},
                            {"type", type}
                        });
        }

        public static async void editDelivery(string uuid, string To, string name)
        {
            try
            {
                DocumentReference docRef = FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId).Collection("Deliverys").Document(uuid);
                Dictionary<string, object> dataToUpdate = new Dictionary<string, object>
                {
                    {"To", To },
                    {"name", name },
                    {"type", "delivery" }
                };

                WriteResult result = await docRef.UpdateAsync(dataToUpdate);

                Thread.Sleep(3000);
            }
            catch
            {

            }

        }

        public static async void RemoveCondoFromUser(string uuid, string selCondo, int selCondoNumber)
        {
            try
            {
                Dictionary<string, object> objectProperties = new Dictionary<string, object>
                {
                    { "condo", selCondo },
                    { "number", selCondoNumber }
                };

                DocumentReference docRef = FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId).Collection("User").Document(uuid);
                Dictionary<string, object> dataToUpdate = new Dictionary<string, object>
                {
                    {"properties", FieldValue.ArrayRemove(objectProperties) }
                };

                WriteResult result = await docRef.UpdateAsync(dataToUpdate);
            }
            catch
            {

            }
        }
    }
}

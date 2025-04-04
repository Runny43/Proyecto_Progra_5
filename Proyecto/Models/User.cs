using Firebase.Auth;
using Google.Api;
using Google.Cloud.Firestore;
using Proyecto.Firebase;
using Proyecto.Mic;
using System;
using static Proyecto.Mic.AppHelper;
namespace Proyecto.Models
{
    public class UserModel
    {
        public string uuid { get; set; }
        public string id_Card { get; set; }
        public string Email { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public List<Propertie> Properties { get; set; }

        public List<Vehicle> Vehicles { get; set; }
    }

    public class Propertie
    {
        public string CondoName { get; set; }
        public int CondoNumber { get; set; }
    }

    public class Vehicle
    {
        public string Plate { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
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
                id_Card= data["id_Card"].ToString(),
                Email = data["email"].ToString(),
                Name = data["name"].ToString(),
                Type = data["type"].ToString(),
            };

            user.Properties = new List<Propertie>();
            user.Vehicles = new List<Vehicle>();
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
                List<Object> vehicleList = (List<Object>)data["vehicles"];

                foreach (Object vehicle in vehicleList)
                {
                    Dictionary<string, object> vehicleData = (Dictionary<string, object>)vehicle;

                    user.Vehicles.Add(new Vehicle
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
                    id_Card= data["id_Card"].ToString(),
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

                    List<Object> vehicleList = (List<Object>)data["vehicles"];

                    if(vehicleList.Count > 0)
                    {
                        owner.Vehicles= new List<Vehicle>();
                        foreach (Object vehicle in vehicleList)
                        {
                            Dictionary<string, object> vehicleData = (Dictionary<string, object>)vehicle;

                            owner.Vehicles.Add(new Vehicle
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

        public static async void postUserWithEmailAndPassword(string card, string plate, string brand, string model, string color, string email, string password, string displayName, string type, string selCondo, int selCondoNumber)
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
            List<Dictionary<string, object>> objectVehicles = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object>
                    {
                        { "plate", plate },
                        { "brand", brand },
                        { "model", model },
                        { "color", color },
                    }
                };
            DocumentReference docRef = await FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId).Collection("User").AddAsync(
                    new Dictionary<string, object>
                        {
                            {"id_Card", card},
                            {"email", email },
                            {"name", displayName },
                            {"type", type},
                            {"properties", objectProperties },
                            {"vehicles", objectVehicles}
                        });

            EmailHelper.SendEmail(card, email, displayName, password, selCondo, selCondoNumber);
        }

        public static async void editOwner(string uuid, string plate, string brand, string model, string color, string card, string email, string displayName, string selCondo, int selCondoNumber)
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
                List<Dictionary<string, object>> objectVehicles = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object>
                    {
                        { "plate", plate },
                        { "brand", brand },
                        { "model", model },
                        { "color", color },
                    }
                };
                DocumentReference docRef = FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId).Collection("User").Document(uuid);
                Dictionary<string, object> dataToUpdate = new Dictionary<string, object>
                {
                    {"id_Card", card },
                    {"name", displayName },
                    {"properties", objectProperties},
                    {"vehicles", objectVehicles}
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

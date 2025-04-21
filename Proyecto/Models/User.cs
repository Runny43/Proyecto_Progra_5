using Firebase.Auth;
using Google.Api;
using Google.Cloud.Firestore;
using Proyecto.Firebase;
using Proyecto.Mic;
using System;
using System.Drawing.Drawing2D;
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

        public List<Assignment> Assignment { get; set; }
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

    public class Assignment
    {
        public string CondoAssignment { get; set; }
    }
    public class UserHelper
    {

        public static async Task<UserModel> getUserInfo(string email)
        {
            Query query = FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId).Collection("User").WhereEqualTo("email", email);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();


            if (querySnapshot.Documents.Count == 0)
            {
                // No encontró ningún usuario con ese email
                return null;
            }


            Dictionary<string, object> data = querySnapshot.Documents[0].ToDictionary();

            UserModel user = new UserModel
            {
                uuid = querySnapshot.Documents[0].Id.ToString(),
                id_Card = data["id_Card"].ToString(),
                Email = data["email"].ToString(),
                Name = data["name"].ToString(),
                Type = data["type"].ToString(),
            };

            user.Properties = new List<Propertie>();
            user.Vehicles = new List<Vehicle>();
            user.Assignment = new List<Assignment>();
            if (user.Type == "security")
            {
                try
                {
                    List<Object> assignmentList = (List<Object>)data["assignment"];

                    foreach (Object assignment in assignmentList)
                    {
                        Dictionary<string, object> assignmentData = (Dictionary<string, object>)assignment;

                        user.Assignment.Add(new Assignment
                        {
                            CondoAssignment = assignmentData["condoAssignment"].ToString(),

                        });
                    }
                }
                catch
                {
                }
            }
            else
            {
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
                    id_Card = data["id_Card"].ToString(),
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

                    if (vehicleList.Count > 0)
                    {
                        owner.Vehicles = new List<Vehicle>();
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
                            {"email", email },
                            {"name", displayName },
                            {"id_Card", card},
                            {"type", type},
                            {"properties", objectProperties },
                            {"vehicles", objectVehicles}
                        });

            //EmailHelper.SendEmail(email, displayName, password, card, selCondo, selCondoNumber, plate, brand, model, color);
            EmailHelper.SendEmail(card, email, displayName, password, selCondo, selCondoNumber, plate, brand, model, color);
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


        public static async Task<List<UserModel>> getSecurity()
        {
            List<UserModel> securityList = new List<UserModel>();
            Query query = FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId).Collection("User");
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            foreach (var item in querySnapshot)
            {
                Dictionary<string, object> data = item.ToDictionary();

                UserModel security = new UserModel
                {
                    uuid = item.Id,
                    Name = data["name"].ToString(),
                    Email = data["email"].ToString(),
                    Type = data["type"].ToString(),
                };

                try
                {
                    List<Object> assignmentList = (List<Object>)data["assignment"];
                    if (assignmentList.Count > 0)
                    {
                        security.Assignment = new List<Assignment>();

                        foreach (Object assignment in assignmentList)
                        {
                            Dictionary<string, object> assignmentData = (Dictionary<string, object>)assignment;

                            security.Assignment.Add(new Assignment
                            {
                                CondoAssignment = assignmentData["condoAssignment"].ToString()
                            });

                        }
                    }
                }
                catch
                {

                }
                securityList.Add(security);

            }
            return securityList;

        }

        public static async void postSecurityWithEmailAndPassword(string email, string password, string displayName, string card, string type, string selCondo)
        {
            UserCredential userCredential = await FirebaseAuthHelper.setFirebaseAuthClient().CreateUserWithEmailAndPasswordAsync(email, password, displayName);
            List<Dictionary<string, object>> objectAssignment = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                        { "condoAssignment", selCondo }

                }
            };
            DocumentReference docRef = await FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId).Collection("User").AddAsync(
                new Dictionary<string, object>
                {
                    {"email", email },
                    {"name", displayName },
                    {"id_Card",card },
                    {"type", type},
                    {"assignment", objectAssignment}
                });

            EmailHelper.SendSecurityEmail(email, displayName, password, card, selCondo);
           
        }

        public static async void editSecurity(string uuid, string email, string displayName, string card, string selCondo)
        {
     
            try
            {
                List<Dictionary<string, object>> objectAssignment = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object>
                    {
                        { "condoAssignment", selCondo },
                    }
                };
                
                DocumentReference docRef = FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId).Collection("User").Document(uuid);
                Dictionary<string, object> dataToUpdate = new Dictionary<string, object>
                {
                    {"name", displayName },
                    {"id_Card",card },
                    {"assignment", objectAssignment}
                };

                WriteResult result = await docRef.UpdateAsync(dataToUpdate);
                Thread.Sleep(3000);
            }
            catch
            {

            }
        }

        public static async Task<bool> DeleteOwner(string uuid, string email)
        {
            try
            {
                // 1. Eliminar de Firestore
                DocumentReference docRef = FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId)
                                                   .Collection("User")
                                                   .Document(uuid);
                await docRef.DeleteAsync();

                //// 2. Eliminar de Authentication (Versión 4.1.0)
                //var auth = FirebaseAuth.DefaultInstance;
                //var userRecord = await auth.GetUserByEmailAsync(email);
                //await auth.DeleteUserAsync(userRecord.Uid);

                //// Eliminar de Authentication
                //var authClient = FirebaseAuthHelper.setFirebaseAuthClient();
                //var userRecord = await authClient.GetUserByEmailAsync(email);
                //await authClient.DeleteUserAsync(userRecord.Uid);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar owner: {ex.Message}");
                return false;
            }
        }

        public static async Task<bool> DeleteSecurity(string uuid, string email)
        {
            try
            {
                // Eliminar de Firestore
                DocumentReference docRef = FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId).Collection("User").Document(uuid);
                await docRef.DeleteAsync();

                //// Eliminar de Authentication
                //var authClient = FirebaseAuthHelper.setFirebaseAuthClient();
                //var userRecord = await authClient.GetUserByEmailAsync(email);
                //await authClient.DeleteUserAsync(userRecord.Uid);

                return true;
            }
            catch (Exception ex)
            {
                // Puedes loggear el error aquí si lo necesitas
                Console.WriteLine($"Error al eliminar security: {ex.Message}");
                return false;
            }
        }




    }
}

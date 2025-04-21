using Google.Cloud.Firestore;
using System;

namespace Proyecto.Models
{
    public class EasyPass
    {
        public string Code { get; set; }
        public Timestamp CreationDate { get; set; }
        public Timestamp ExpirationDate { get; set; }


        public static async Task<bool> ValidateEasyPassCode(string code)
        {
            // Obtener la instancia de Firestore
            FirestoreDb db = FirestoreDb.Create("proyecto-grupo1-47c47");

            // Buscar el código en la colección "EasyPasses"
            QuerySnapshot snapshot = await db.Collection("EasyPasses")
                .WhereEqualTo("Code", code)
                .GetSnapshotAsync();

            // Si no hay documentos, el código no es válido
            if (snapshot.Count == 0)
            {
                return false;
            }

            // Obtener el primer documento (ya que el código es único)
            var easyPassDocument = snapshot.Documents[0];

            // Verificar si el código ha expirado
            Timestamp expirationDate = easyPassDocument.GetValue<Timestamp>("ExpirationDate");
            DateTime expirationDateTime = expirationDate.ToDateTime();
            if (expirationDateTime < DateTime.UtcNow)
            {
                return false; // El código ha expirado
            }

            return true; // El código es válido
        }
    }






}

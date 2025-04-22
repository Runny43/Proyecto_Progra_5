using System.Net.Mail;
using System.Net;
using System.Text;
using QRCoder;
using System.Drawing;
using System.Reflection;
using Google.Cloud.Firestore;


namespace Proyecto.Mic
{
    public class AppHelper
    {
        public static string CreatePassword()
        {
            int len = 10;
            string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

            Random rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);

            StringBuilder res = new StringBuilder();

            while (0 <= len--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }

            return res.ToString();
        }

        public static string CreateEasyPassCode()
        {
            int len = 3;
            string valid = "1234567890";

            Random rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);

            StringBuilder res = new StringBuilder();

            while (0 <= len--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }

            return res.ToString();
        }
    }
    public static class EmailHelper
    {
        public static void SendEmail(string card, string email, string displayName, string pwd, string selCondo, int selCondoNumber, string plate, string brand, string model, string color)
        {
            string sender = "firebaserrm@gmail.com";
            string senderPwd = "oyuv xrwy qvjt wzbx";

            //string sender = "";
            //string senderPwd = "";

            using (MailMessage mm = new MailMessage(sender, email))
            {
                mm.Subject = "Bienvenido al Sistema Automatico de Condominios";
                mm.IsBodyHtml = true;

                using (var sr = new StreamReader("wwwroot/templates/welcome.html"))
                {
                    string body = sr.ReadToEnd().Replace("{usuario}", displayName);
                    body = body.Replace("{cedula}", card);
                    body = body.Replace("{email}", email);
                    body = body.Replace("{password}", pwd);
                    body = body.Replace("{condominio}", selCondo);
                    body = body.Replace("{numero_casa}", selCondoNumber.ToString());
                    body = body.Replace("{placa}", plate);
                    body = body.Replace("{marca}", brand);
                    body = body.Replace("{modelo}", model);
                    body = body.Replace("{color}", color);

                    mm.Body = body;
                }

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential(sender, senderPwd);
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
            }
        }


        public static void SendSecurityEmail(string email, string displayName, string pwd, string card, string selCondo)
        {
            string sender = "firebaserrm@gmail.com";
            string senderPwd = "oyuv xrwy qvjt wzbx";

            //string sender = "";
            //string senderPwd = "";

            using (MailMessage mm = new MailMessage(sender, email))
            {
                mm.Subject = "Bienvenido al Sistema Automatico de Condominios";
                mm.IsBodyHtml = true;

                using (var sr = new StreamReader("wwwroot/templates/welcomeSecurity.html"))
                {
                    string body = sr.ReadToEnd().Replace("{usuario}", displayName);
                    body = body.Replace("{email}", email);
                    body = body.Replace("{password}", pwd);
                    body = body.Replace("{cedula}", card);
                    body = body.Replace("{condominio}", selCondo);
                    mm.Body = body;
                }

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential(sender, senderPwd);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);
            }
        }
    }


    public static class FirebaseHelper
    {
        public static async Task GuardarEasyPass(string code)
        {
            FirestoreDb db = FirestoreDb.Create("proyecto-grupo1-47c47");

            // Obtener la fecha actual y la fecha de expiración
            var now = Timestamp.FromDateTime(DateTime.UtcNow);
            var expires = Timestamp.FromDateTime(DateTime.UtcNow.AddHours(12));

            // Crear el objeto EasyPass con los datos necesarios
            var easyPass = new
            {
                Code = code,
                CreationDate = now,
                ExpirationDate = expires
            };

            // Referencia a la colección "EasyPasses" en Firestore
            CollectionReference colRef = db.Collection("EasyPasses");

            // Agregar el nuevo documento a la colección
            await colRef.AddAsync(easyPass);
        }
    }

    public static class QRGenerator
    {

        public static string GenerateQRCode(string code)
        {
            //string content = AppHelper.CreateEasyPassCode();
            string path = "wwwroot/qr/" + code + ".png";

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData data = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(data);
            Bitmap bit = qrCode.GetGraphic(20);
            bit.Save(path);

            return path.Replace("wwwroot", string.Empty);
        }


    }


}
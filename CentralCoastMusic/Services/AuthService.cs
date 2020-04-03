using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentralCoastMusic.Services
{
    public class AuthService
    {
        private string jsonCred = AppSettings.AppSetting["authJsonCred"];
        /// <summary>
        /// Gets the token to do edits/deletes/logging in
        /// </summary>
        /// <param name="idToken"></param>
        /// <returns></returns>
        public async Task<string> Google(string idToken)
        {
            //need to figure out how to log out of this though if even necessary
            try
            {
                var defaultApp = FirebaseApp.DefaultInstance;

                if (defaultApp == null)
                {
                    FirebaseApp.Create(new AppOptions()
                    {
                        Credential = GoogleCredential.FromJson(jsonCred),
                    });
                }

                var decoded = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
                var uid = decoded.Uid;

                return uid;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public void Clear()
        {
            //Unsure if this would ever need to be used
            FirebaseApp.DefaultInstance.Delete();
        }

    }
}

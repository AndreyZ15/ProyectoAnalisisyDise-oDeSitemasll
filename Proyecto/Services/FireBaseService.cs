using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;

namespace Proyecto.Services
{
    internal class FireBaseService
    {
        private readonly FirebaseClient _firebaseClient;

        public FireBaseService()
        {
            // Reemplaza esta URL con la URL de tu proyecto Firebase
            _firebaseClient = new FirebaseClient("https://tu-proyecto-firebase.firebaseio.com/");
        }

        protected FirebaseClient GetClient()
        {
            return _firebaseClient;
        }




    }
}

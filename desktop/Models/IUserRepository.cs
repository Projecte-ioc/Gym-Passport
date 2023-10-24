using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Gym_Passport.Models
{
    public interface IUserRepository
    {
        /// <summary>
        /// Verifica si el usuario pasado por el parámetro credential está en la base de datos.
        /// </summary>
        /// <param name="credential">Instancia de NetworkCredential</param>
        /// <returns>true si está en la base de datos, false si no lo está.</returns>
        bool AuthenticateUser(NetworkCredential credential);

        /// <summary>
        /// Obtiene los datos de un usuario a partir de su username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Un UserModel con los datos del usuario si este se encuentra en la base de datos, null si no.</returns>
        UserModel GetByUsername(string username);
    }
}

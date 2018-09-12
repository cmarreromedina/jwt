using System;
using System.Net;
using System.Threading;
using System.Web.Http;
using WebApiJWT.Classes;
using WebApiJWT.Models;
using System.Linq;
using System.Security.Cryptography;

namespace WebApiJWT.Controllers
{


    [AllowAnonymous]
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {

        private PruebaWsEntities db = new PruebaWsEntities(); 

        [HttpGet]
        [Route("echoping")]
        public IHttpActionResult EchoPing()
        {
            return Ok(true);
        }

        [HttpGet]
        [Route("echouser")]
        public IHttpActionResult EchoUser()
        {
            var identity = Thread.CurrentPrincipal.Identity;
            return Ok($" IPrincipal-user: {identity.Name} - IsAuthenticated: {identity.IsAuthenticated}");
        }

        [HttpPost]
        [Route("authenticate")]
        public  IHttpActionResult Authenticate(LoginRequest login)
        {
            if (login == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            Usuario usuario = null;
            usuario = db.Usuario.Where(x => x.NombreUsuario == login.username).First();

            if (usuario == null)
            {
                return NotFound();
            }

            using (MD5 md5Hash = MD5.Create())
            {

                var md5 = new Md5Encrypt();
                string contraseña =  md5.GetMd5Hash(md5Hash, login.password);

                bool isCredentialValid = (contraseña.ToUpper()  == usuario.Contraseña);
                if (isCredentialValid)
                {
                    var token = TokenGenerator.GenerateTokenJwt(login.username);
                    return Ok(token);
                }
                else
                {
                    return Unauthorized();
                }


            }


        }
    }
}

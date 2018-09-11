using System;
using System.Net;
using System.Threading;
using System.Web.Http;
using WebApiJWT.Classes;
using WebApiJWT.Models;
using System.Linq;

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
                return NotFound();
            var contraseña = db.Usuario.SqlQuery("select Cast(DecryptByPassPhrase('@NombreUsuario', @Contraseña) As varchar(200)) As 'password' from Usuario",usuario);
            

            //bool isCredentialValid = (login.password == contraseña);
            //if (isCredentialValid)
            //{
            //    var token = TokenGenerator.GenerateTokenJwt(login.Username);
            //    return Ok(token);
            //}
            //else
            //{
            //    return Unauthorized();
            //}
            //return Ok(token);
            return Ok();
        }
    }
}

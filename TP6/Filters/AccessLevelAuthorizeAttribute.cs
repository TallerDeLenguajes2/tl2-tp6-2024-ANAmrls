using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TP6.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AccessLevelAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _requiredAccessLevels;

        //El constructor recibe un array de niveles de accesso permitidos

        public AccessLevelAuthorizeAttribute(params string[] requiredAccessLevels)
        {
            _requiredAccessLevels = requiredAccessLevels;
        }


        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userAccessLevel = GetUserAccessLevel(context);

            //Primero verificamos si el usuario está autenticado
            if (!IsAuthenticated(context))
            {
                //Si no lo está, redirigir a la pagina de logueo
                context.Result = new RedirectToActionResult("Index", "Login", null);
                return;
            }

            //Si está autenticado, verificamos si tiene el nivel de acceso adecuado
            if (_requiredAccessLevels == null || !_requiredAccessLevels.Contains(userAccessLevel))
            {
                //redirigir a la vista personalizada 403 en vez de solo devolver 403
                context.Result = new RedirectToActionResult("Error403", "Error", null);
                return;
            }

            //Si está autenticado y tiene el acceso adecuado, la acción continuará
        }

        private static string? GetUserAccessLevel(AuthorizationFilterContext context)
        {
            return context.HttpContext.Session.GetString("AccessLevel");
        }

        private static bool IsAuthenticated(AuthorizationFilterContext context)
        {
            return context.HttpContext.Session.GetString("IsAuthenticated") == "true";
        }
    }
}

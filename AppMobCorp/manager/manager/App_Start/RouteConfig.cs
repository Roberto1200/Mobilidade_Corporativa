using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Manager
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            // BotDetect requests must not be routed
            routes.IgnoreRoute("{*botdetect}",
              new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });
            /*Sub Categoria*/
            routes.MapRoute(
                name: "Subcategoria", // Route name
                url: "CanaisDigitais/Subcategoria/{id}",
                defaults: new { controller = "CanaisDigitais", action = "Subcategorias", id = UrlParameter.Optional }
            ); routes.MapRoute(
                 name: "Editar Subcategoria", // Route name
                 url: "CanaisDigitais/Subcategoria/Edit/{id}/{idp}",
                 defaults: new { controller = "CanaisDigitais", action = "EditSub", id = UrlParameter.Optional, idp = UrlParameter.Optional }
             ); routes.MapRoute(
                  name: "Deletar Subcategoria", // Route name
                  url: "CanaisDigitais/Subcategoria/Delete/{id}/{idp}",
                  defaults: new { controller = "CanaisDigitais", action = "DeleteSub", id = UrlParameter.Optional, idp = UrlParameter.Optional }
              );
            /********************************************************************************************************/
            /*Conteudo*/
            routes.MapRoute(
                name: "Conteudo", // Route name
                url: "CanaisDigitais/Subcategoria/Conteudo/{id}",
                defaults: new { controller = "CanaisDigitais", action = "Conteudo", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                 name: "Editar Conteudo", // Route name
                 url: "CanaisDigitais/Subcategoria/Conteudo/Edit/{id}/{idp}",
                 defaults: new { controller = "CanaisDigitais", action = "EditCont", id = UrlParameter.Optional, idp = UrlParameter.Optional }
             );
            routes.MapRoute(
                  name: "Deletar Conteudo", // Route name
                  url: "CanaisDigitais/Subcategoria/Conteudo/Delete/{id}/{idp}",
                  defaults: new { controller = "CanaisDigitais", action = "DeleteCont", id = UrlParameter.Optional, idp = UrlParameter.Optional }
              );
            /********************************************************************************************************************/
            /*Rotiamento do fórum*/
            routes.MapRoute(
                name: "Topico", // Route name
                url: "Forum/Topicos/{id}",
                defaults: new { controller = "Forum", action = "IndexTopicos", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Editar Topico", // Route name
                url: "Forum/Topicos/Edit/{id}/{idp}",
                defaults: new { controller = "Forum", action = "EditTopicos", id = UrlParameter.Optional, idp = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Deletar Topico", // Route name
                url: "Forum/Topicos/Delete/{id}/{idp}",
                defaults: new { controller = "Forum", action = "DeleteTopicos", id = UrlParameter.Optional, idp = UrlParameter.Optional }
            );
            /*********************************************************************************************************/
            routes.MapRoute(
                name: "Posts", // Route name
                url: "Forum/Topicos/Posts/{id}",
                defaults: new { controller = "Forum", action = "IndexPosts", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Editar Posts", // Route name
                url: "Forum/Topicos/Posts/Edit/{id}/{idp}",
                defaults: new { controller = "Forum", action = "EditPosts", id = UrlParameter.Optional, idp = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Deletar Posts", // Route name
                url: "Forum/Topicos/Posts/Delete/{id}/{idp}",
                defaults: new { controller = "Forum", action = "DeletePosts", id = UrlParameter.Optional, idp = UrlParameter.Optional }
            );
            /*********************************************************************************************************/
            /*
             * Roteamento dos tipos da a genda
             */
            routes.MapRoute(
               name: "Agenda tipos", // Route name
               url: "Agenda/Tipos/{id}",
               defaults: new { controller = "Agenda", action = "IndexTipos", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                name: "Editar Agenda tipos", // Route name
                url: "Agenda/Tipos/Edit/{id}",
                defaults: new { controller = "Agenda", action = "EditTipos", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Deletar agenda tipos", // Route name
                url: "Agenda/Tipos/Delete/{id}",
                defaults: new { controller = "Agenda", action = "DeleteTipos", id = UrlParameter.Optional }
            );
            /*******************************************************************************************************/
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            
        }
    }
}
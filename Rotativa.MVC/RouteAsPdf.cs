// Decompiled with JetBrains decompiler
// Type: Rotativa.MVC.RouteAsPdf
// Assembly: Rotativa.MVC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AFE8B199-049E-4B91-92C6-F1D7392E2549
// Assembly location: C:\Users\g.tuzzolino\Desktop\Nuova cartella (2)\Rotativa.MVC.dll

using System.Web.Mvc;
using System.Web.Routing;

namespace Rotativa.MVC
{
    public class RouteAsPdf : AsPdfResultBase
    {
        private RouteValueDictionary routeValuesDict;
        private object routeValues;
        private string routeName;

        public RouteAsPdf(string routeName)
        {
            this.routeName = routeName;
        }

        public RouteAsPdf(string routeName, RouteValueDictionary routeValues)
          : this(routeName)
        {
            this.routeValuesDict = routeValues;
        }

        public RouteAsPdf(string routeName, object routeValues)
          : this(routeName)
        {
            this.routeValues = routeValues;
        }

        protected override string GetUrl(ControllerContext context)
        {
            UrlHelper urlHelper = new UrlHelper(context.RequestContext);
            string empty = string.Empty;
            string str = this.routeValues != null ? (this.routeValues == null ? urlHelper.RouteUrl(this.routeName) : urlHelper.RouteUrl(this.routeName, this.routeValues)) : urlHelper.RouteUrl(this.routeName, this.routeValuesDict);
            return string.Format("{0}://{1}{2}", (object)context.HttpContext.Request.Url.Scheme, (object)context.HttpContext.Request.Url.Authority, (object)str);
        }
    }
}

// Decompiled with JetBrains decompiler
// Type: Rotativa.MVC.ActionAsPdf
// Assembly: Rotativa.MVC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AFE8B199-049E-4B91-92C6-F1D7392E2549
// Assembly location: C:\Users\g.tuzzolino\Desktop\Nuova cartella (2)\Rotativa.MVC.dll

using System.Web.Mvc;
using System.Web.Routing;

namespace Rotativa.MVC
{
    public class ActionAsPdf : AsPdfResultBase
    {
        private RouteValueDictionary routeValuesDict;
        private object routeValues;
        private string action;

        public ActionAsPdf(string action)
        {
            this.action = action;
        }

        public ActionAsPdf(string action, RouteValueDictionary routeValues)
          : this(action)
        {
            this.routeValuesDict = routeValues;
        }

        public ActionAsPdf(string action, object routeValues)
          : this(action)
        {
            this.routeValues = routeValues;
        }

        protected override string GetUrl(ControllerContext context)
        {
            UrlHelper urlHelper = new UrlHelper(context.RequestContext);
            string empty = string.Empty;
            string str = this.routeValues != null ? (this.routeValues == null ? urlHelper.Action(this.action) : urlHelper.Action(this.action, this.routeValues)) : urlHelper.Action(this.action, this.routeValuesDict);
            return string.Format("{0}://{1}{2}", (object)context.HttpContext.Request.Url.Scheme, (object)context.HttpContext.Request.Url.Authority, (object)str);
        }
    }
}

// Decompiled with JetBrains decompiler
// Type: Rotativa.MVC.UrlAsPdf
// Assembly: Rotativa.MVC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AFE8B199-049E-4B91-92C6-F1D7392E2549
// Assembly location: C:\Users\g.tuzzolino\Desktop\Nuova cartella (2)\Rotativa.MVC.dll

using System.Web.Mvc;

namespace Rotativa.MVC
{
    public class UrlAsPdf : AsPdfResultBase
    {
        private readonly string _url;

        public UrlAsPdf(string url)
        {
            this._url = url ?? string.Empty;
        }

        protected override string GetUrl(ControllerContext context)
        {
            string lower = this._url.ToLower();
            if (lower.StartsWith("http://") || lower.StartsWith("https://"))
                return this._url;
            return string.Format("{0}://{1}{2}", (object)context.HttpContext.Request.Url.Scheme, (object)context.HttpContext.Request.Url.Authority, (object)this._url);
        }
    }
}

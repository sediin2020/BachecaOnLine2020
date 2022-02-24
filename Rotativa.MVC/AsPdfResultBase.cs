// Decompiled with JetBrains decompiler
// Type: Rotativa.MVC.AsPdfResultBase
// Assembly: Rotativa.MVC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AFE8B199-049E-4B91-92C6-F1D7392E2549
// Assembly location: C:\Users\g.tuzzolino\Desktop\Nuova cartella (2)\Rotativa.MVC.dll

using Rotativa.Core;
using Rotativa.Core.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace Rotativa.MVC
{
    public abstract class AsPdfResultBase : ActionResult
    {
        private const string ContentType = "application/pdf";

        public AsPdfResultBase()
        {
            DriverOptions driverOptions = new DriverOptions();
            driverOptions.WkhtmltopdfPath = string.Empty;
            driverOptions.PageMargins = new Margins();
            this.RotativaOptions = driverOptions;
        }

        public DriverOptions RotativaOptions { get; set; }

        public string FileName { get; set; }

        [Obsolete("Use BuildPdf(this.ControllerContext) method instead and use the resulting binary data to do what needed.")]
        public string SaveOnServerPath { get; set; }

        public byte[] BuildPdf(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (this.RotativaOptions.WkhtmltopdfPath == string.Empty)
                this.RotativaOptions.WkhtmltopdfPath = HttpContext.Current.Server.MapPath("~/Rotativa");
            byte[] bytes = this.CallTheDriver(context);
            if (!string.IsNullOrEmpty(this.SaveOnServerPath))
                File.WriteAllBytes(this.SaveOnServerPath, bytes);
            return bytes;
        }

        public byte[] BuildPdf(string controller, string action, object model = null)
        {
            byte[] bytes = this.CallTheDriver(controller, action, model);

            return bytes;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            byte[] buffer = this.BuildPdf(context);
            this.PrepareResponse(context.HttpContext.Response).OutputStream.Write(buffer, 0, buffer.Length);
        }

        protected abstract string GetUrl(ControllerContext context);

        protected virtual byte[] CallTheDriver(ControllerContext context)
        {
            this.GetWkParams(context);
            return WkhtmltopdfDriver.Convert(this.RotativaOptions);
        }
 
        protected virtual byte[] CallTheDriver(string controller, string action, object model =  null)
        {
           // this.GetWkParams(context);
            return WkhtmltopdfDriver.Convert(this.RotativaOptions);
        }

       protected HttpResponseBase PrepareResponse(HttpResponseBase response)
        {
            response.ContentType = "application/pdf";
            if (!string.IsNullOrEmpty(this.FileName))
                response.AddHeader("Content-Disposition", string.Format("attachment; filename=\"{0}\"", (object)AsPdfResultBase.SanitizeFileName(this.FileName)));
            response.AddHeader("Content-Type", "application/pdf");
            return response;
        }

        private void GetWkParams(ControllerContext context)
        {
            this.RotativaOptions.URL = this.GetUrl(context);
            HttpCookie httpCookie = (HttpCookie)null;
            if (context.HttpContext.Request.Cookies != null && ((IEnumerable<string>)context.HttpContext.Request.Cookies.AllKeys).Contains<string>(FormsAuthentication.FormsCookieName))
                httpCookie = context.HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (httpCookie == null)
                return;
            this.RotativaOptions.Cookies.Add(httpCookie.Name, httpCookie.Value);
        }

        private static string SanitizeFileName(string name)
        {
            string pattern = string.Format("[{0}]+", (object)Regex.Escape(new string(Path.GetInvalidPathChars()) + new string(Path.GetInvalidFileNameChars())));
            return Regex.Replace(name, pattern, "_");
        }
    }
}

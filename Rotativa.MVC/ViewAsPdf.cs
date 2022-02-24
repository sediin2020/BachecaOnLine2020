// Decompiled with JetBrains decompiler
// Type: Rotativa.MVC.ViewAsPdf
// Assembly: Rotativa.MVC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AFE8B199-049E-4B91-92C6-F1D7392E2549
// Assembly location: C:\Users\g.tuzzolino\Desktop\Nuova cartella (2)\Rotativa.MVC.dll

using RazorEngineServices;
using Rotativa.Core;
using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Rotativa.MVC
{
    public class ViewAsPdf : AsPdfResultBase
    {
        private string _viewName;
        private string _masterName;

        public string ViewName
        {
            get
            {
                return this._viewName ?? string.Empty;
            }
            set
            {
                this._viewName = value;
            }
        }

        public string MasterName
        {
            get
            {
                return this._masterName ?? string.Empty;
            }
            set
            {
                this._masterName = value;
            }
        }

        public object Model { get; set; }

        public ViewAsPdf()
        {
            this.MasterName = string.Empty;
            this.ViewName = string.Empty;
            this.Model = (object)null;
        }

        public ViewAsPdf(string viewName)
          : this()
        {
            this.ViewName = viewName;
        }

        public ViewAsPdf(object model)
          : this()
        {
            this.Model = model;
        }

        public ViewAsPdf(string viewName, object model)
          : this()
        {
            this.ViewName = viewName;
            this.Model = model;
        }

        public ViewAsPdf(string viewName, string masterName, object model)
          : this(viewName, model)
        {
            this.MasterName = masterName;
        }

        protected override string GetUrl(ControllerContext context)
        {
            return string.Empty;
        }

        protected virtual ViewEngineResult GetView(ControllerContext context, string viewName, string masterName)
        {
            return ViewEngines.Engines.FindView(context, this.ViewName, this.MasterName);
        }

        protected override byte[] CallTheDriver(string controller, string action, object model = null)
        {
            try
            {
                return WkhtmltopdfDriver.ConvertHtml(this.RotativaOptions, RazorEngineProvider.RenderViewToString(controller,  action, model));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //   config.ReferenceResolver = new MyIReferenceResolver();

        protected override byte[] CallTheDriver(ControllerContext context)
        {
            try
            {
                context.Controller.ViewData.Model = this.Model;
                if (string.IsNullOrEmpty(this.ViewName))
                    this.ViewName = context.RouteData.GetRequiredString("action");
                using (StringWriter stringWriter = new StringWriter())
                {
                    ViewEngineResult view = this.GetView(context, this.ViewName, this.MasterName);
                    if (view.View == null)
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.AppendLine();
                        foreach (string searchedLocation in view.SearchedLocations)
                            stringBuilder.AppendLine(searchedLocation);
                        throw new InvalidOperationException(string.Format("The view '{0}' or its master was not found, searched locations: {1}", (object)this.ViewName, (object)stringBuilder));
                    }
                    ViewContext viewContext = new ViewContext(context, view.View, context.Controller.ViewData, context.Controller.TempData, (TextWriter)stringWriter);
                    view.View.Render(viewContext, (TextWriter)stringWriter);
                    StringBuilder stringBuilder1 = stringWriter.GetStringBuilder();
                    string str = string.Format("{0}://{1}", (object)HttpContext.Current.Request.Url.Scheme, (object)HttpContext.Current.Request.Url.Authority);
                    stringBuilder1.Replace(" href=\"/", string.Format(" href=\"{0}/", (object)str));
                    stringBuilder1.Replace(" src=\"/", string.Format(" src=\"{0}/", (object)str));
                    return WkhtmltopdfDriver.ConvertHtml(this.RotativaOptions, stringBuilder1.ToString());
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

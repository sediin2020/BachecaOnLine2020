using Fondir.Bacheca.Dom.Abstract;
using Fondir.Bacheca.Dom.Repository;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fondir.Bacheca.WebUI.Infrastucture
{
    public class NinjectControllerFactory : DefaultControllerFactory, IDisposable
    {
        IKernel kernel = null;

        public NinjectControllerFactory()
        {
            kernel = new StandardKernel();
            AddBindings();
        }

        private void AddBindings()
        {
            kernel.Bind<ISelectlist>().To<Selectlist>();
            kernel.Bind<IIniziative>().To<Iniziative>();
            kernel.Bind<IDettaglioIniziativa>().To<DettaglioIniziativa>();
            kernel.Bind<IDettaglioModuliXIniziativa>().To<DettaglioModuliXIniziativa>();
            kernel.Bind<IDettaglioEdizioniXIniziativa>().To<DettaglioEdizioniXIniziativa>();
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)kernel.Get(controllerType);
        }

        public T GetController<T>()
        {
            return (T)kernel.GetService(typeof(T));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool p)
        {
            if (p)
            {
                kernel.Dispose();
                kernel = null;
            }
        }
    }
}
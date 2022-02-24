using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fondir.Bacheca.WebUI.Helpers
{
    public static class Helper
    {
        public static SelectList SelectListStrutture(this HtmlHelper helper)
        {
            Infrastucture.NinjectControllerFactory a = new Infrastucture.NinjectControllerFactory();
            return new SelectList(a.GetController<Dom.Abstract.ISelectlist>().Select("bachecaOnLine_ddlStruttura"), "id", "descrizione");
        }

        public static SelectList SelectListSettore(this HtmlHelper helper)
        {
            Infrastucture.NinjectControllerFactory a = new Infrastucture.NinjectControllerFactory();
            return new SelectList(a.GetController<Dom.Abstract.ISelectlist>().Select("bachecaOnLine_ddlComparto"), "id", "descrizione");
        }

        public static SelectList SelectListTipoIniziativa(this HtmlHelper helper)
        {
            Infrastucture.NinjectControllerFactory a = new Infrastucture.NinjectControllerFactory();
            return new SelectList(a.GetController<Dom.Abstract.ISelectlist>().Select("bachecaOnLine_ddlTipoIniziativa"), "id", "descrizione");
        }

        public static SelectList SelectListAreaTematica(this HtmlHelper helper)
        {
            Infrastucture.NinjectControllerFactory a = new Infrastucture.NinjectControllerFactory();
            return new SelectList(a.GetController<Dom.Abstract.ISelectlist>().Select("bachecaOnLine_ddlAreaTematica"), "id", "descrizione");
        }

        public static SelectList SelectListTematica(this HtmlHelper helper)
        {
            Infrastucture.NinjectControllerFactory a = new Infrastucture.NinjectControllerFactory();
            return new SelectList(a.GetController<Dom.Abstract.ISelectlist>().Select("bachecaOnLine_ddlTematica"), "id", "descrizione");
        }

        public static SelectList SelectListRegione(this HtmlHelper helper)
        {
            Infrastucture.NinjectControllerFactory a = new Infrastucture.NinjectControllerFactory();
            return new SelectList(a.GetController<Dom.Abstract.ISelectlist>().Select("bachecaOnLine_ddlRegione"), "id", "descrizione");
        }

        public static SelectList SelectListSede(this HtmlHelper helper)
        {
            Infrastucture.NinjectControllerFactory a = new Infrastucture.NinjectControllerFactory();
            return new SelectList(a.GetController<Dom.Abstract.ISelectlist>().Select("bachecaOnLine_ddlSedeDidattica"), "id", "descrizione");
        }

        public static bool IsTipologiaOneToOne(this HtmlHelper helper, string tipo)
        {
            return ConfigurationManager.AppSettings["chiaveOneToOne"] == tipo;
        }

        public static bool IsTipologiaSeminari(this HtmlHelper helper, string tipo)
        {
            return ConfigurationManager.AppSettings["chiaveSeminari"] == tipo;
        }

        public static bool IsTipologiaMaster(this HtmlHelper helper, string tipo)
        {
            return ConfigurationManager.AppSettings["chiaveMaster"] == tipo;
        }

        public static string ToIntero(this HtmlHelper helper, decimal valore)
        {
            try
            {
                return ((int)valore).ToString();
            }
            catch (Exception)
            {
                return "0";
            }

        }

        public static bool IsSettoreCreditizioFinanziario(this HtmlHelper helper, string settore)
        {
            try
            {
                return ConfigurationManager.AppSettings["chiaveSettoreCF"] == settore;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsSettoreCommercio(this HtmlHelper helper, string settore)
        {
            try
            {
                return ConfigurationManager.AppSettings["chiaveSettoreCTS"] == settore;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsSettoreEntrambi(this HtmlHelper helper, string settore)
        {
            try
            {
                return ConfigurationManager.AppSettings["chiaveSettoreEntrambi"] == settore;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
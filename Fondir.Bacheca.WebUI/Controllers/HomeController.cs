using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sediin.MVC.HtmlHelpers;
using Fondir.Bacheca.WebUI.Models;
using Fondir.Bacheca.Dom.Abstract;
using Fondir.Bacheca.Dom.Entities;

using entities = Fondir.Bacheca.Dom.Entities;
using System.Threading;
using Rotativa.MVC;
using Rotativa.Core;
using Rotativa.Core.Options;
using System.Configuration;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Reflection;

//using System.Web.Http.Cors;

namespace Fondir.Bacheca.WebUI.Controllers
{
    //[EnableCors(origins: "http://www.example.com", headers: "*", methods: "*")]

    public class HomeController : Controller
    {
        ISelectlist repositoryStrutture;

        IIniziative repositoryIniziative;

        IDettaglioIniziativa repositoryDettaglioIniziativa;

        IDettaglioModuliXIniziativa repositoryDettaglioModuliIniziativa;

        IDettaglioEdizioniXIniziativa repositoryDettaglioEdizioniIniziativa;

        public HomeController(ISelectlist _repositoryStrutture, IIniziative _repositoryIniziative, IDettaglioIniziativa _repositoryDettaglioIniziativa, IDettaglioModuliXIniziativa _repositoryDettaglioModuliIniziativa, IDettaglioEdizioniXIniziativa _repositoryDettaglioEdizioniIniziativa)
        {
            repositoryStrutture = _repositoryStrutture;
            repositoryIniziative = _repositoryIniziative;
            repositoryDettaglioIniziativa = _repositoryDettaglioIniziativa;
            repositoryDettaglioModuliIniziativa = _repositoryDettaglioModuliIniziativa;
            repositoryDettaglioEdizioniIniziativa = _repositoryDettaglioEdizioniIniziativa;
        }

        public ActionResult Index()
        {
            return View();
        }

        class PagingIniziative
        {
            public int Totale { get; set; }
            public int Skip { get; set; }
            public int Take { get; set; }
            public int PageCount { get; set; }
            public int PageIndex { get; set; }
            public bool HashPaging { get; set; }

            public List<IniziativaViewModel> Iniziative { get; set; }

        }

        public JsonResult Iniziative(IniziativaRicercaModel m, int pageIndex = 0, bool? random = false)
        {
            var _model = IniziativePaging(m, pageIndex, random);

            return Json(new
            {
                totaleRecord = _model.Totale,
                record = _model.Skip,
                pageCount = _model.PageCount,
                pageIndex = _model.PageIndex,
                iniziative = _model.Iniziative,
                hashPaging=_model.HashPaging
            }, JsonRequestBehavior.AllowGet);
        }


        private PagingIniziative IniziativePaging(IniziativaRicercaModel m, int? pageIndex = 0, bool? random = false)
        {
            try
            {
                var _take = 0; // view == "IniziativeDataList" ? 6 : 3;
                var _skip = 0;

                if (pageIndex.HasValue)
                {
                    _take = 6;

                    if (pageIndex.GetValueOrDefault() == 0)
                    {
                        pageIndex = 2;
                    }
                    else
                    {
                        _skip = pageIndex.GetValueOrDefault() * 3;
                        _take = 3;
                        pageIndex++;
                    }
                }


                Func<string[], int?[]> toInt = delegate (string[] a)
                {
                    try
                    {
                        if (a == null)
                            return null;

                        return a.Select(d => (int?)int.Parse(d)).ToList().ToArray();
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                };

                //chiama vista iniziative
                var _list = GetIniziative();

                //ordine prima del skip e take
                string param = null;
                string order = null;
                PropertyInfo propertyInfo = null;

                if (!string.IsNullOrWhiteSpace(m.OrderBy))
                {
                    param = m.OrderBy.Split(' ').FirstOrDefault();
                    order = m.OrderBy.Split(' ').LastOrDefault();
                    propertyInfo = typeof(IniziativaViewModel).GetProperty(param);
                }

                //if (param == "DataInizio" || param == "DataFine")
                //{
                //    _list = from a in _list
                //            where !string.IsNullOrWhiteSpace(a.Periodo)
                //            select a;

                //    _list = _list.Except(_listX);
                //}

                //se non e primo avvio
                if (!random.GetValueOrDefault())
                {
                    //if (m.AreaTemarica != null && m.AreaTemarica.Contains("9"))
                    //{
                    //    List<string> _l = m.AreaTemarica.ToList();

                    //    _l.Add("1");
                    //    _l.Add("2");

                    //    m.AreaTemarica = _l.ToArray();

                    //}

                    var _parolachiave = m.ChiaveRicerca;

                    var _settore = toInt(m.Settore);

                    var _areaTematica = toInt(m.AreaTematica);

                    var _tematica = toInt(m.Tematica);

                    var _strutture = toInt(m.Strutture);
                    
                    var _sedeDidattica = toInt(m.SedeDidattica);

                    var _q = from a in _list
                             where !string.IsNullOrWhiteSpace(m.TipoIniziativa) ? (int?)int.Parse(m.TipoIniziativa) == a.TipoInizitivaID : true
                             where !string.IsNullOrWhiteSpace(m.Regione) ? (int?)int.Parse(m.Regione) == a.RegioneID : true
                             where _strutture != null ? _strutture.Contains(a.StrutturaID) : true
                             where _settore != null ? _settore.Contains(a.SettoreID) : true
                             where _areaTematica != null ? _areaTematica.Contains(a.IdAreaTematica) : true
                             where _tematica != null ? _tematica.Contains(a.IdTematica) : true
                             where _sedeDidattica != null ? _sedeDidattica.Contains(a.sede_didattica) : true
                             where !string.IsNullOrWhiteSpace(_parolachiave) ? a.ParoleChiaveNew.Contains(_parolachiave) : true
                             select a;

                    _list = _q;
                }

                //output list
                var _resultList = new List<IniziativaViewModel>();

                var _totale = 9;

                if (!random.GetValueOrDefault())
                {
                    if (propertyInfo != null)
                    {
                        if (order == "asc")
                            _list = _list.OrderBy(x => propertyInfo.GetValue(x, null));
                        else
                            _list = _list.OrderByDescending(x => propertyInfo.GetValue(x, null));
                    }

                    _totale = _list.Count();

                    if (_take > 0)
                        _resultList = _list.Skip(_skip).Take(_take).ToList();
                    else
                        _resultList = _list.ToList();

                }
                else
                {
                    _totale = 9;

                    var _listTemp = new List<IniziativaViewModel>();

                    Func<int, List<IniziativaViewModel>> getResult = delegate (int tipo)
                    {
                        try
                        {
                            var _l = _list.Where(s => s.TipoInizitivaID == tipo);

                            for (int i = 0; i < 3; i++)
                            {
                                Random r = new Random();
                                var _index = r.Next(0, _l.Count());
                                _listTemp.Add(_l.ElementAt(_index));
                            }

                            return null;
                        }
                        catch
                        {
                            return null;
                        }
                    };

                    getResult(1);
                    getResult(2);
                    getResult(3);

                    foreach (var item in _listTemp.ToList())
                    {
                        Random r = new Random();
                        var _index = r.Next(0, _listTemp.Count());
                        _resultList.Add(_listTemp.ElementAt(_index));

                        _listTemp.RemoveAt(_index);
                    }

                    if (propertyInfo != null)
                    {
                        if (order == "asc")
                            _resultList = _resultList.OrderBy(x => propertyInfo.GetValue(x, null)).ToList();
                        else
                            _resultList = _resultList.OrderByDescending(x => propertyInfo.GetValue(x, null)).ToList();
                    }
                }

                var pageCount = (int)Math.Round((double)_totale / 3);

                PagingIniziative model = new PagingIniziative();
                model.PageCount = pageCount;
                model.Iniziative = _resultList;
                model.PageIndex = pageIndex.GetValueOrDefault();
                model.Skip = _skip;
                model.Take = _take;
                model.Totale = _totale;
                model.HashPaging = (_skip+_take) < _totale;

                return model;

            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public ActionResult IniziativePDF(IniziativaRicercaModel m, bool? random = false)
        {
            try
            {
                var _model = IniziativePaging(m, null, random);

                if (_model == null)
                    throw new Exception("Dati non trovati!");


                var _pdf = new ViewAsPdf("IniziativePDF", _model.Iniziative)
                {
                    RotativaOptions = new DriverOptions()
                    {
                        PageOrientation = Orientation.Landscape,
                        PageSize = Size.A4,
                        //PageMargins = new Margins(20,20,0,0),
                    }
                };

                var binary = _pdf.BuildPdf(this.ControllerContext);

                return File(binary, "application/pdf", "Iniziative.pdf");

            }
            catch (Exception ex)
            {
                return this.Content(ex.Message);
            }
        }

        private IEnumerable<IniziativaViewModel> GetIniziative()
        {
            try
            {
                Func<string, int, DateTime?> _getData = delegate (string a, int index)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(a))
                            return null;

                        var _data = a.Split('-')[index];

                        return Convert.ToDateTime(_data.Trim());
                    }
                    catch
                    {
                        return null;
                    }
                };

                Func<string, string> _isValidPeriodo = delegate (string a)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(a))
                            return a;

                        return Convert.ToDateTime(_getData(a, 1)) < DateTime.Now ? "" : a;
                    }
                    catch
                    {
                        return a;
                    }
                };

                Func<string, long?> _getCodiceIniziativaNumerico = delegate (string a)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(a))
                            return null;

                        return Convert.ToInt64(new string(a.Where(c => (Char.IsDigit(c) || c == '.' || c == ',')).ToArray()));
                    }
                    catch
                    {
                        return null;
                    }
                };

                return (from x in repositoryIniziative.Select()
                        select new IniziativaViewModel
                        {
                            IniziativaID = x.idIniziativa,
                            Luogo = "",
                            //Periodo = _isValidPeriodo(x.PeriodoSvolgimento),
                            Periodo = x.PeriodoSvolgimento,
                            Tipo = x.tipologiainiziativa,
                            Titolo = x.titolo,
                            TipoInizitivaID = x.tipo_iniziativa,
                            StrutturaID = x.idUtente,
                            Struttura = x.struttura,
                            IdAreaTematica = x.IdAreaTematica,
                            IdTematica = x.IdTematica,
                            //TematicaID = x.IdTematica,
                            CodiceIniziativa = x.codiceIniziativa,
                            RegioneID = x.regione_sede,
                            SettoreID = x.idComparto,
                            DataInizio = _getData(x.PeriodoSvolgimento, 0),
                            DataFine = _getData(x.PeriodoSvolgimento, 1),
                            CodiceIniziativaNumerico = _getCodiceIniziativaNumerico(x.codiceIniziativa),
                            sede_didattica = x.sede_didattica,
                            ParoleChiaveNew = x.ParoleChiaveNew
                        });

                //List<IniziativaViewModel> _lista = new List<IniziativaViewModel>();
                //for (int i = 0; i < 50; i++)
                //{

                //    IniziativaViewModel a = new IniziativaViewModel();
                //    a.Periodo = DateTime.Now.AddDays(1).ToShortDateString() + " al " + DateTime.Now.AddDays(10).ToShortDateString();
                //    a.Luogo = "Roma";
                //    a.Tipo = "Evento " + i;
                //    a.Titolo = "Titolo del evento";

                //    _lista.Add(a);
                //}

                //return _lista;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult DettaglioIniziativaPDF(int id)
        {
            try
            {
                var _model = repositoryDettaglioIniziativa.Detail(id);

                if (_model == null)
                    throw new Exception("Dati non trovati!");

                _model.iniziativaID = id;

                var _pdf = new ViewAsPdf("DettaglioIniziativaPDF", _model)
                {
                    RotativaOptions = new DriverOptions()
                    {
                        PageOrientation = Orientation.Portrait,
                        PageSize = Size.A4,
                        //PageMargins = new Margins(20,20,0,0),
                    }
                };

                var binary = _pdf.BuildPdf(this.ControllerContext);

                return File(binary, "application/pdf", "DettaglioIniziativa.pdf");

            }
            catch (Exception ex)
            {
                return this.Content(ex.Message);
            }
        }

        public ActionResult DettaglioIniziativa(int id)
        {
            try
            {

                var _model = repositoryDettaglioIniziativa.Detail(id);

                if (_model == null)
                    throw new Exception();

                DettaglioIniziativaView model = new DettaglioIniziativaView();
                model.Iniziativ = _model;
                _model.iniziativaID = id;
                model.Brochure = repositoryIniziative.Brochure(id);

                return PartialView(model);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult DettaglioModuliXIniziativa(int id)
        {
            var _model = repositoryDettaglioModuliIniziativa.Select(id);
            return PartialView(_model);
        }

        public ActionResult DettaglioEdizioniXIniziativa(int id)
        {
            var _model = repositoryDettaglioEdizioniIniziativa.Select(id);
            return PartialView(_model);
        }

        public ActionResult ScaricaIniziativaCompleta(string id, string settore, int uid, int tipo)
        {
            try
            {
                var _path = ConfigurationManager.AppSettings["percorsoStampeDefinitive"];
                var _filePrefix = "";
                var _filename = "";

                switch (settore)
                {
                    //E:\www\inetpub\Fondir.Bacheca2017.Web\allegati\Stampe_definitive_CF
                    case "CF":
                        _path = Path.Combine(_path, "Stampe_definitive_CF");
                        _filePrefix = "stampa_definitiva_cf_{0}_{1}_{2}.pdf";
                        _filename = "Stampa_definitive_CF.pdf";
                        break;
                    case "CTS":
                        _path = Path.Combine(_path, "Stampe_definitive_CTS");
                        _filePrefix = "stampa_definitiva_cts_{0}_{1}_{2}.pdf";
                        _filename = "Stampa_definitive_CTS.pdf";
                        break;

                    default:

                        break;
                }

                var _file = Path.Combine(_path, string.Format(_filePrefix, uid, id, tipo));

                if (!System.IO.File.Exists(_file))
                    throw new Exception("File non trovato!");

                var _scheda = SplitPDFSchedaIniziativa(_file);

                return File(_scheda.ToArray(), "application/pdf", _filename);
            }
            catch (Exception ex)
            {
                return this.Content(ex.Message);

            }
        }

        public ActionResult ScaricaIniziativaCompleta2020(string id, string codiceIniziativa, string nomeFile, int tipo)
        {
            try
            {
                var _path = ConfigurationManager.AppSettings["percorsoStampeDefinitive"];
                var _filePrefix = "";
                var _filename = "";

                _path = Path.Combine(_path, "Stampe_definitive_2020");
                _filePrefix = nomeFile;
                //_filePrefix = "INIZIATIVA_FORMATIVA_{0}.pdf";
                _filename = "Stampa_definitiva_2020.pdf";
                
                var _file = Path.Combine(_path, _filePrefix);
                //var _file = Path.Combine(_path, string.Format(_filePrefix, codiceIniziativa.Replace("/", "_")));

                if (!System.IO.File.Exists(_file))
                    throw new Exception("File non trovato!");

                var _scheda = SplitPDFSchedaIniziativa(_file);

                return File(_scheda.ToArray(), "application/pdf", _filename);
            }
            catch (Exception ex)
            {
                return this.Content(ex.Message);
            }
        }
        public ActionResult ScaricaBrochure(string filename)
        {
            try
            {
                var _filename = Path.GetFileName(filename);

                var _path = ConfigurationManager.AppSettings["percorsoBrochure"];

                var _file = Path.Combine(_path, filename);

                if (!System.IO.File.Exists(_file))
                    throw new Exception("File non trovato!");

                return File(_file, MimeMapping.GetMimeMapping(_filename), _filename);
            }
            catch (Exception ex)
            {
                return this.Content(ex.Message);

            }
        }
        public MemoryStream SplitPDFSchedaIniziativa(string inputPath)
        {
            try
            {
                PdfReader reader = new PdfReader(inputPath);

                PdfReader finalPdf;
                Document pdfContainer;
                PdfWriter pdfCopy;
                MemoryStream msFinalPdf = new MemoryStream();

                finalPdf = new PdfReader(inputPath);
                pdfContainer = new Document();
                pdfCopy = new PdfSmartCopy(pdfContainer, msFinalPdf);

                pdfContainer.Open();

                //for (int k = 0; k < reader.NumberOfPages; k++)
                //{
                //    finalPdf = new PdfReader(inputPath);
                for (int i = 2; i < finalPdf.NumberOfPages - 1; i++)
                {
                    ((PdfSmartCopy)pdfCopy).AddPage(pdfCopy.GetImportedPage(finalPdf, i));
                }

                pdfCopy.FreeReader(finalPdf);

                //}
                finalPdf.Close();
                pdfCopy.Close();
                pdfContainer.Close();

                return msFinalPdf;
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}

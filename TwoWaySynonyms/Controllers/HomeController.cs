using DbDomain.Models;
using DbDomain.ViewModels;
using Logger_ToFile;
using Repository.Repos;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using TwoWaySynonyms.Filters;

namespace TwoWaySynonyms.Controllers
{
    [AuthorizeByPass]
    public class HomeController : Controller
    {
        LoggToFile log = new LoggToFile(System.Reflection.Assembly.GetExecutingAssembly().FullName);
        static int countBadPassword = 5;
        [AllowAnonymous]
        public ActionResult Index(string message)
        {
            string method = System.Reflection.MethodBase.GetCurrentMethod().ToString();
            try
            {
                log.Info(method, "Wejście na stronę logowania.");          
                ViewBag.Message = message;
                AuthorizeByPassAttribute.Pass = ""; //Wylogowanie :)
                if (Request.IsAjaxRequest())
                    return PartialView(); // Jesli po wylogowaniu próba submit
                return View();
            }
            catch (Exception ex)
            {
                log.Error(method, ex.Message + Environment.NewLine + ex.InnerException);
                return RedirectToAction("Error", new { errMessage = ex.Message });
            }

        }
        [AllowAnonymous]
        public ActionResult Error(string errMessage)
        {
            string method = System.Reflection.MethodBase.GetCurrentMethod().ToString();
            log.Info(method, "Wejście na stronę Error. " + errMessage);
            ViewBag.ErrMessage = errMessage;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Pass(string pass, string action)
        {
            string method = System.Reflection.MethodBase.GetCurrentMethod().ToString();
            log.Info(method, "Sprawdzenie Hasła.");
            AuthorizeByPassAttribute.Pass = pass;
            return RedirectToAction(action);
        }
        public ActionResult Synonyms()
        {
            string method = System.Reflection.MethodBase.GetCurrentMethod().ToString();
            try
            {
                log.Info(method, "Synonyms - GET");
                return View();
            }
            catch (Exception ex)
            {
                log.Error(method, ex.Message + Environment.NewLine + ex.InnerException);
                return RedirectToAction("Error", new { errMessage = ex.Message });
            }
          
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Synonyms(Synonym s)
        {
            string method = "async Synonyms(Synonym s)";
            try
            {
                log.Info(method, "Synonyms POST - start");
                if (ModelState.IsValid)
                {
                    log.Info(method, "Synonyms POST - ModelState.IsValid");
                    using (SynonymsRepo repo = new SynonymsRepo())
                    {                        
                        if (s != null)
                        {
                            int? id = await repo.AddSynonym(s);
                            if (id > -1)
                            {
                                log.Info(method, $"Synonyms POST - Dodane do bazy, id={id}");
                                return PartialView("_SynonymsList", await repo.GetSynonymVM(id));
                            }
                        }
                        log.Warn(method, "Synonyms POST - Nie dodane");
                        return PartialView("_SynonymsList", new SynonymViewModel() { Message = "Problem z dodawaniem" });
                    }
                }                
                ViewBag.Message = "Błędne dane - Wyrażenie może mić max 50 znaków";
                log.Warn(method, ViewBag.Message);
                return PartialView("_SynonymsList");
            }
            catch (Exception ex)
            {
                log.Error(method, ex.Message + Environment.NewLine + ex.InnerException);
                return PartialView("_SynonymsList", new SynonymViewModel() { Message = ex.Message });
            }
        }
        [AllowAnonymous]
        public ActionResult PassError()
        {
            string method = System.Reflection.MethodBase.GetCurrentMethod().ToString();
            log.Warn(method, $"Błędne hasło, pozostało {countBadPassword}");
            countBadPassword--;
            if (countBadPassword == 0)
            {
                countBadPassword = 5;
                log.Warn(method, "Błędne hasło, przekierowanie do Error");
                return RedirectToAction("Error", new { errMessage = "Błędne Hasło" });
            }
            return RedirectToAction("Index", new { message = $"Błędne hasło, spróbuj jeszcze raz! Pozostało szans:{countBadPassword}" });
        }     
    }
}
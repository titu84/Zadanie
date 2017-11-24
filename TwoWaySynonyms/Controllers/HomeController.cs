using DbDomain.Models;
using DbDomain.ViewModels;
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
        static int countBadPassword = 5;
        [AllowAnonymous]
        public ActionResult Index(string message)
        {
            try
            {                
                ViewBag.Message = message;
                AuthorizeByPassAttribute.Pass = ""; //Wylogowanie :)
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { errMessage = ex.Message });
            }

        }
        [AllowAnonymous]
        public ActionResult Error(string errMessage)
        {
            ViewBag.ErrMessage = errMessage;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Pass(string pass, string action)
        {
            AuthorizeByPassAttribute.Pass = pass;
            return RedirectToAction(action);
        }
        public ActionResult Synonyms()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { errMessage = ex.Message });
            }
          
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Synonyms(Synonym s)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (SynonymsRepo repo = new SynonymsRepo())
                    {                        
                        if (s != null)
                        {
                            int? id = await repo.AddSynonym(s);
                            if (id > -1)
                            {
                                return PartialView("_SynonymsList", await repo.GetSynonymVM(id));
                            }
                        }
                        return PartialView("_SynonymsList", new SynonymViewModel() { Message = "Problem z dodawaniem" });
                    }
                }
                ViewBag.Message = "Błędne dane - Wyrażenie może mić max 50 znaków";
                return PartialView("_SynonymsList");
            }
            catch (Exception ex)
            {
                return PartialView("_SynonymsList", new SynonymViewModel() { Message = ex.Message });
            }
        }
        [AllowAnonymous]
        public ActionResult PassError()
        {
            countBadPassword--;
            if (countBadPassword == 0)
            {
                countBadPassword = 5;
                return RedirectToAction("Error", new { errMessage = "Błędne Hasło" });
            }
            return RedirectToAction("Index", new { message = $"Błędne hasło, spróbuj jeszcze raz! Pozostało szans:{countBadPassword}" });
        }
    }
}
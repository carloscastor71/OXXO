using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OXXO.Controllers
{
    public class CategorizacionController : Controller
    {
        // GET: CategorizacionController
        public ActionResult Index()
        {
            return View();
        }

        // GET: CategorizacionController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CategorizacionController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategorizacionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategorizacionController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CategorizacionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategorizacionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CategorizacionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

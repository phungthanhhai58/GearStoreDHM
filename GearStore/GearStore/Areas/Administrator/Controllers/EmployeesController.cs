using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GearStore.Models;
using GearStore.Areas.Administrator.Models;
using GearStore.Areas.Administrator.Controllers.CustomAttributes;

namespace GearStore.Areas.Administrator.Controllers
{
    public class EmployeesController : Controller
    {
        private ElectronicComponentsSMEntities _dataContext = new ElectronicComponentsSMEntities();
        // GET: Administrator/Employees
        [AdminAuthorize(Order = 4)]
        public async Task<ActionResult> Index(int? page)
        {
            if (page < 1)
            {
                return RedirectToAction(nameof(Index));
            }
            var employees = _dataContext.Employees.Include(e => e.JobTitle);
            var count = employees.Count();
            var n = 10;
            var maxPage = count % n == 0 ? count / n : count / n + 1;
            page = page ?? 1;
            if (page > maxPage && count != 0)
            {
                return HttpNotFound();
            }
            ViewBag.MaxPage = maxPage;
            ViewBag.NowPage = page;
            var skip = page.Value * n - n;
            return View(await employees.OrderBy(p => p.FullName).Skip(skip).Take(n).ToListAsync());
        }
        // GET: Administrator/Employees/Details/5
        [AdminAuthorize(Order = 4)]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = await _dataContext.Employees.FindAsync(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }
        // GET: Administrator/Employees/Create
        [AdminAuthorize(Order = 4)]
        public ActionResult Create()
        {
            ViewBag.JobTitleID = new SelectList(_dataContext.JobTitles, "JobTitleID", "Name");
            return View();
        }

        // POST: Administrator/Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AdminAuthorize(Order = 4)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EmployeeID,Username,Password,Email,FullName,BirthDate,Gender,Address,PhoneNumber,JobTitleID")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _dataContext.Employees.Add(employee);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.JobTitleID = new SelectList(_dataContext.JobTitles, "JobTitleID", "Name", employee.JobTitleID);
            return View(employee);
        }
        // GET: Administrator/Employees/Edit/5
        [AdminAuthorize(Order = 1)]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = await _dataContext.Employees.FindAsync(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.JobTitleID = new SelectList(_dataContext.JobTitles, "JobTitleID", "Name", employee.JobTitleID);
            return View(employee);
        }
        // POST: Administrator/Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AdminAuthorize(Order = 1)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EmployeeID,Username,Password,Email,FullName,BirthDate,Gender,Address,PhoneNumber,JobTitleID")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _dataContext.Entry(employee).State = EntityState.Modified;
                await _dataContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.JobTitleID = new SelectList(_dataContext.JobTitles, "JobTitleID", "Name", employee.JobTitleID);
            return View(employee);
        }
        // GET: Administrator/Employees/Delete/5
        [AdminAuthorize(Order = 1)]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = await _dataContext.Employees.FindAsync(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }
        // POST: Administrator/Employees/Delete/5
        [AdminAuthorize(Order = 1)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Employee employee = await _dataContext.Employees.FindAsync(id);
            _dataContext.Employees.Remove(employee);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
       
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dataContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

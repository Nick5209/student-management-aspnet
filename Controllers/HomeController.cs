using System.Diagnostics;
using CRUDWebAppInASPCore.Models;
using CRUDWebAppInASPCore.Storage;
using Microsoft.AspNetCore.Mvc;

namespace CRUDWebAppInASPCore.Controllers;

public class HomeController(IStudentStore students) : Controller
{
    public async Task<IActionResult> Index() => View(await students.ListAsync());

    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create([Bind("Name,Gender,Age,Standerd")] Student student)
    {
        if (!ModelState.IsValid)
        {
            return View(student);
        }
        await students.AddAsync(student);
        TempData["Inserted_Success"] = "Student added.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int? id)
    {
        var student = id.HasValue ? await students.FindAsync(id.Value) : null;
        return student is null ? NotFound() : View(student);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        var student = id.HasValue ? await students.FindAsync(id.Value) : null;
        return student is null ? NotFound() : View(student);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Name,Gender,Age,Standerd")] Student student)
    {
        if (!id.HasValue || id != student.Id)
        {
            return NotFound();
        }
        if (!ModelState.IsValid)
        {
            return View(student);
        }
        if (!await students.UpdateAsync(id.Value, student))
        {
            return NotFound();
        }
        TempData["Updated_Success"] = "Student updated.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        var student = id.HasValue ? await students.FindAsync(id.Value) : null;
        return student is null ? NotFound() : View(student);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirm(int? id)
    {
        if (!id.HasValue || !await students.DeleteAsync(id.Value))
        {
            return NotFound();
        }
        TempData["Deleted_Success"] = "Student deleted.";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View(new ErrorViewModel
    {
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
    });
}

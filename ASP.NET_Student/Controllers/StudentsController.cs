using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASP.NET_Student.Data;
using ASP.NET_Student.Models;

namespace ASP.NET_Student.Controllers
{
    public class StudentsController : Controller
    {
        private readonly AppDbContext _context;

        public StudentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            return View(await _context.Students.ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Score,Discipline")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Score,Discipline")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }

        public IActionResult TopBottomStudents()
        {
            var topStudents = _context.Students.OrderByDescending(s => s.Score).Take(5).ToList();
            var bottomStudents = _context.Students.OrderBy(s => s.Score).Take(5).ToList();

            ViewBag.TopStudents = topStudents;
            ViewBag.BottomStudents = bottomStudents;

            return View();
        }

        public async Task<IActionResult> CalculateTotalScore()
        {
            var totalScore = await _context.Students.SumAsync(s => s.Score);
            ViewBag.TotalScore = totalScore;
            return View("TotalScore"); 
        }

        public async Task<IActionResult> ExportToTextFile()
        {
            try
            {
                var students = await _context.Students.ToListAsync();

                
                var memoryStream = new MemoryStream();

                
                using (var writer = new StreamWriter(memoryStream, leaveOpen: true))
                {
                    foreach (var student in students)
                    {
                        await writer.WriteLineAsync($"{student.Name}, {student.Score},{student.Discipline}");
                    }
                    await writer.FlushAsync();
                }

                
                memoryStream.Position = 0;

                
                return File(memoryStream, "text/plain", "students_report.txt");
            }
            catch (Exception ex)
            {
                return Content($"Error: {ex.Message}");
            }
        }



    }
}

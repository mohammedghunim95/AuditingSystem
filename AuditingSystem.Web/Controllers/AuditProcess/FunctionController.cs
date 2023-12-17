﻿using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AuditingSystem.Web.Controllers.AuditProcess
{
    public class FunctionController : Controller
    {
        private readonly IBaseRepository<Department> _departmentRepository;
        private readonly IBaseRepository<Function> _functionRepository;
        public FunctionController(
            IBaseRepository<Function> functionRepository,
            IBaseRepository<Department> departmentRepository)
        {
            _functionRepository = functionRepository;
            _departmentRepository = departmentRepository;
        }
        public async Task<IActionResult> Index()
        {
            var functions = await _functionRepository.ListAsync(
                  c => c.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  c => c.Department);

            return View(functions);
        }

        public async Task<IActionResult> Add()
        {
            var department = _departmentRepository.ListAsync(
                  u => u.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  null).Result;

            ViewBag.DepartmentId = new SelectList(department, "Id", "Name");

            return View();
        }


        public async Task<IActionResult> Edit(int id)
        {
            var function = await _functionRepository.FindByAsync(id);

            var department = _departmentRepository.ListAsync(
                  u => u.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  null).Result;

            ViewBag.DepartmentId = new SelectList(department, "Id", "Name", function.DepartmentId);

            return View(function);
        }
    }
}

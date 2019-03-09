using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreDemo.Models;
using CoreDemo.Services;
using CoreDemo.Setting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CoreDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICinemaService _cinemaService;
        private readonly IOptions<ConnectionOptions> _options;

        public HomeController(ICinemaService cinemaService,IOptions<ConnectionOptions> Options)//构造函数注入(要先在startup类里先在ioc容器内注册)
        {
            _cinemaService = cinemaService;
            _options = Options;
        }

        public async Task<IActionResult> Index()//返回类型是一个接口对单元测试也比较方便  这个Controller对应的页
        {
            ViewBag.Title = "电影院列表";

            return View(await _cinemaService.GetllAllAsync());
        }


        public IActionResult Add()
        {
            ViewBag.Title = "添加电影院";
            return View(new Cinema());
        }

        [HttpPost]
        public async Task<IActionResult> Add(Cinema model)
        {
            if (ModelState.IsValid)//检查model的有效性
            {
                await _cinemaService.AddAsync(model);
            }

            return RedirectToAction("Index");//添加完成后跳转到电影院列表页面
        }

        public async Task<IActionResult> Edit(int cinemaId)
        {
            var cinema = await _cinemaService.GetByIdAsync(cinemaId);
            return View(cinema);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Cinema model)
        {
            if (ModelState.IsValid)
            {
                var exist = await _cinemaService.GetByIdAsync(id);
                if (exist == null)
                {
                    return NotFound();
                }

                exist.Name = model.Name;
                exist.Location = model.Location;
                exist.Capacity = model.Capacity;
            }

            return RedirectToAction("Index");
        }
    }
}
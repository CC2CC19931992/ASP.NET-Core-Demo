using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreDemo.Services;
using CoreDemo.Setting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CoreDemo
{
    public class Startup
    {

        private readonly IConfiguration _configuration;


        public Startup(IConfiguration configuration)//注入IConfiguration到Startup里
        {
            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)//用来配置依赖注入，注入到ioc容器
        {
            services.AddMvc();//注册mvc中间件
            services.AddSingleton<ICinemaService, CinemaMemoryService>();//在Ioc容器中注册服务，生命周期是Singleton
            services.AddSingleton<IMovieService, MovieMemoryService>();
            //注册ConnectionOptions配置，用来获取数据库连接选项，这边只需从配置文件里读取ConnectionStrings的键值对，
            services.Configure<ConnectionOptions>(_configuration.GetSection("ConnectionStrings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> logger)//配置管道
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();//显示错误的堆栈信息，建议只在开发环境使用
            }
            if (env.IsProduction())
            {
                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                });
            }

            app.UseStatusCodePages();//显示错误的中间件
            //app.UseStatusCodePagesWithRedirects();//显示自定义的错误页
            app.UseStaticFiles();//使用静态文件的中间件（在wwwroot里的）

            //配置mvc中间件
            app.UseMvc(route=> {
                route.MapRoute(
                    name:"default",
                    template:"{controller=Home}/{action=Index}/{id?}"
                    );
            });

            

            //这里再定义一个中间件 这个时候是不会执行，因为在前面的Run里
            //已经执行了context的返回了,
            //如果想要返回结果 上面第一个中间件应该改成下面注释的这一段
            //app.Use(async (context, next) =>
            //{
            //    logger.LogInformation("M1 Start");
            //    await context.Response.WriteAsync("Hello World!");
            //    await next();//这里的next表示接下来还有中间件需要跑的
            //    logger.LogInformation("M1 End");
            //});

            //app.Run(async (context) =>
            //{
            //    logger.LogInformation("M2 Start");
            //    await context.Response.WriteAsync("Another World!");
            //    logger.LogInformation("M2 End");
            //});
        }
    }
}

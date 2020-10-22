using Luckyu.DataAccess;
using Luckyu.Log;
using Luckyu.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UEditor.Core;

namespace Luckyu.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            LogDBConnection.SetConnectString(AppSettingsHelper.GetConnectionString("DbLog"));
            BaseConnection.SetConnectString(AppSettingsHelper.GetConnectionString("DbMain"));

            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddSignalR();
            services.AddScoped<SignalRHelper>();

            // �����ļ���
            var baseFilePath = AppSettingsHelper.GetAppSetting("AnnexPath");
            if (!Directory.Exists(baseFilePath))
            {
                Directory.CreateDirectory(baseFilePath);
            }

            // UEditor
            services.AddUEditorService("wwwroot/lib/ueditor/config/config.json");

            var builder = services.AddControllersWithViews(options =>
           {
               options.Filters.Add(typeof(AuthorizeFilter));
               options.Filters.Add(typeof(ExceptionFilter));
           }).AddNewtonsoftJson(options =>
           {
               options.SerializerSettings.ContractResolver = new DefaultContractResolver();
               options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;  // ����ѭ������
               options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";   // ����ʱ���ʽ
           });

            // ��̬���� ģ�����
            var fileNames = Directory.GetFiles(AppContext.BaseDirectory, "Luckyu.Module.*.dll");
            foreach (var name in fileNames)
            {
                var assembly = Assembly.LoadFile(name);
                builder.AddApplicationPart(assembly);
            }
            if (LuckyuHelper.IsDebug())
            {
                // ��ͼ����ʱ����  �������
                builder.AddRazorRuntimeCompilation(options =>
                {
                    foreach (var name in fileNames)
                    {
                        if (name.EndsWith(".Views.dll"))
                        {
                            continue;
                        }
                        var file = new FileInfo(name);
                        var libraryPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", file.Name.Remove(file.Name.Length - 4)));
                        options.FileProviders.Add(new PhysicalFileProvider(libraryPath));
                    }
                });
            }

            var fileNames1 = Directory.GetFiles(AppContext.BaseDirectory, "Luckyu.App.*.dll");
            foreach (var name in fileNames1)
            {
                //Assembly.LoadFile(name);
                System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromAssemblyPath(name);
            }


            // IP�����б�
            //services.Configure<ForwardedHeadersOptions>(options =>
            //{
            //    options.KnownProxies.Add(IPAddress.Parse("10.0.0.100"));
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // ���÷������
            // using Microsoft.AspNetCore.HttpOverrides;
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseAuthentication();
            // ���÷������

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            //��ȡViews�ļ����µ�js��css
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(FileHelper.Combine(Directory.GetCurrentDirectory(), "Views")),
                RequestPath = new PathString("/Views"),
                ContentTypeProvider = new FileExtensionContentTypeProvider(
                            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                            {
                                { ".js", "application/javascript" },
                                { ".css", "text/css" },
                            })
            });
            //��ȡAreas  Views�ļ����µ�js��css
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Areas")),
                RequestPath = new PathString("/Areas"),
                ContentTypeProvider = new FileExtensionContentTypeProvider(
                        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                        {
                            { ".js", "application/javascript" },
                            { ".css", "text/css" },
                        })
            });
            // UEditor �ϴ�
            var uePath = AppSettingsHelper.GetAppSetting("UEPath");
            if (!Directory.Exists(uePath))
            {
                Directory.CreateDirectory(uePath);
            }
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(uePath),
                RequestPath = "/ueupload",
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=36000");
                }
            });

            app.UseSession();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapHub<MessageHub>("/messagehub");

            });
        }
    }
}

﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.Reflection;
using System.Text;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Supabase;
using System.Text;
using Hangfire.MemoryStorage;
using WebAPIRestaurantManagement.Settings;
using WebAPIRestaurantManagement.Services.Authorization;
using System.Net;
using WebAPIRestaurantManagement.Services.Configuration;
using WebAPIRestaurantManagement.Services.SupabaseClient;
using WebAPIRestaurantManagement.Services.CategoriesItems;
using WebAPIRestaurantManagement.Services.MenuItems;
using WebAPIRestaurantManagement.Services.Tables;
using WebAPIRestaurantManagement.Services.Roles;

namespace WebAPIRestaurantManagement
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

            services.AddHangfire(x => x.UseMemoryStorage());
            services.AddHangfireServer();
            //Configuration from AppSettings
            services.Configure<JWT>(Configuration.GetSection("Authentication"));
            //If using IIS
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            // 

            // 
            //User Manager Service

            //---------------

            //Adding Authentiation - JWT
            ///// Connect DataBase Supabase
            ///
            // Tạo Supabase client cho dự án 1
            var supabaseClient = new Client(
                    Configuration["Authentication:SUPABASE_URL"],  // Thay bằng URL của bạn
                    Configuration["Authentication:SUPABASE_KEY"],  // API Key của bạn
                    new SupabaseOptions
                    {
                        AutoRefreshToken = true,
                        AutoConnectRealtime = true,
                        Schema = "RestaurantManagement"
                    });

            //// Thêm vào DI container
            services.AddSingleton(supabaseClient);
           
            //services.AddScoped<Supabase.Client>(_ =>
            //    new Supabase.Client(
            //        Configuration["Authentication:SUPABASE_URL"],  // Thay bằng URL của bạn
            //        Configuration["Authentication:SUPABASE_KEY"],  // API Key của bạn
            //        new SupabaseOptions
            //        {
            //            AutoRefreshToken = true,
            //            AutoConnectRealtime = true,
            //            Schema = "QLSinhVien"
            //        }));
            // Cấu hình xác thực JWT
            services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Authentication:JWTSecret"])),
                        ValidIssuer = Configuration["Authentication:ValidIsuser"],
                        ValidAudience = Configuration["Authentication:ValidAudience"],
                    };
                });
            services.AddAuthorization();
            ////// Hoàn thành đăng ký Supabase
            ///
            // Cấu hình Swagger
            services.AddSwaggerGen(Swagger =>
            {
                // Cấu hình JWT Authorization trong Swagger
                Swagger.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "Specify the authorization token."
                });
                Swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "RestaurantManagement.API", Version = "v1" });
                Swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "bearer"
                        }
                    },
                    new string[] {}
                }
            });
            });
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddHttpClient();
            #region Authorization Service
            services.AddScoped<IConfigurationService, ConfigurationService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ISupabaseClientService, SupabaseClientService>();
            services.AddScoped<ICategoriesItemsServices, CategoriesItemsService>();
            services.AddScoped<IMenuItemServices, MenuItemServices>();
            services.AddScoped<ITableServices, TableServices>();
            services.AddScoped<IRoleServices, RoleServices>();
            #endregion
            // Add services to the container.
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            // Enable Swagger
            //---------------
            // Make sure swagger UI requires a Bearer token specified

            services.AddSwaggerGen(options =>
            {
                // Đảm bảo Swagger sử dụng tài liệu XML
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
            // Set the comments path for the Swagger JSON and UI.

            //services.AddScoped<IUserClientService, UserClientService>();

            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHangfireDashboard();

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    app.UseSwagger();
            //    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OnlineOffice.API v1"));
            //}
            //Publish to Production Environment
            app.UseDeveloperExceptionPage();
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V2");
                c.RoutePrefix = string.Empty; // Đặt Swagger UI ở trang chủ
            });
            // Configure the HTTP request pipeline.
            app.UseHttpsRedirection();

            app.UseRouting();
            // global cors policy
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthentication();

            app.UseAuthorization();
            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == StatusCodes.Status404NotFound
                && !Path.HasExtension(context.Request.Path.Value))
                {
                    context.Request.Path = "/index.html";
                    await next();
                }
            });
            app.UseDefaultFiles();
            app.UseStaticFiles();
            // app.UseMiddleware<CustomeMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var cultureInfo = new CultureInfo("vi-VN");
            //cultureInfo.NumberFormat.CurrencySymbol = "đ";
            var dateTimeFormat = new DateTimeFormatInfo();
            dateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            cultureInfo.DateTimeFormat = dateTimeFormat;
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }
    }
}

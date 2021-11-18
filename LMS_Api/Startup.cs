using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.AutoMapper;
using DataAccess.Concrete;
using Entities.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace LMS_Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
               .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling
                                        = ReferenceLoopHandling.Ignore);

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<AppUser, IdentityRole>(options=> {
                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddCors();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "LMS API",
                    Description = "API for a Learning Management System"
                });
            });

            services.Configure<JWTConfig>(Configuration.GetSection("JWTConfig"));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(0),
                ValidIssuer = Configuration["JWTConfig:Issuer"],
                ValidAudience = Configuration["JWTConfig:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWTConfig:Key"]))
            };

            services.AddSingleton(tokenValidationParameters);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = true;
                    o.SaveToken = true;
                    o.TokenValidationParameters = tokenValidationParameters;
                });

            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IGroupDal, EFGroupDal>();

            services.AddScoped<ISubjectService, SubjectService>();
            services.AddScoped<ISubjectDal, EFSubjectDal>();

            services.AddScoped<ILessonService, LessonService>();
            services.AddScoped<ILessonDal, EFLessonDal>();

            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IQuestionDal, EFQuestionDal>();

            services.AddScoped<IOptionService, OptionService>();
            services.AddScoped<IOptionDal, EFOptionDal>();

            services.AddScoped<IQuizService, QuizService>();
            services.AddScoped<IQuizDal, EFQuizDal>();

            services.AddScoped<IAssignmentMaterialService, AssignmentMaterialService>();
            services.AddScoped<IAssignmentMaterialDal, EFAssignmentMaterialDal>();

            services.AddScoped<IAssignmentService, AssignmentService>();
            services.AddScoped<IAssignmentDal, EFAssignmentDal>();

            services.AddScoped<IAssignmentAppUserService, AssignmentAppUserService>();
            services.AddScoped<IAssignmentAppUserDal, EFAssignmentAppUserDal>();

            services.AddScoped<IAssignmentAppUserMaterialService, AssignmentAppUserMaterialService>();
            services.AddScoped<IAssignmentAppUserMaterialDal, EFAssingnmentAppUserMaterialDal>();

            services.AddScoped<IGroupMaxPointService, GroupMaxPointService>();
            services.AddScoped<IGroupMaxPointDal, EFGroupMaxPointDal>();

            services.AddScoped<IAppUserGroupPointService, AppUserGroupPointService>();
            services.AddScoped<IAppUserGroupPointDal, EFAppUserGroupPointDal>();

            services.AddScoped<IAppUserGroupService, AppUserGroupService>();
            services.AddScoped<IAppUserGroupDal, EFAppUserGroupDal>();

            services.AddScoped<ITheoryService, TheoryService>();
            services.AddScoped<ITheoryDal, EFTheoryDal>();

            services.AddScoped<ITheoryAppUserService, TheoryAppUserService>();
            services.AddScoped<ITheoryAppUserDal, EFTheoryAppUserDal>();

            services.AddScoped<IGroupSubmissionService, GroupSubmissionService>();
            services.AddScoped<IGroupSubmissionDal, EFGroupSubmissionDal>();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "LMS API");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials()
                .WithExposedHeaders("count"));

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

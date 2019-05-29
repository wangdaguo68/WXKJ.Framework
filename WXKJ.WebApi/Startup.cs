using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog.Extensions.Logging;
using NLog.Web;
using Swashbuckle.AspNetCore.Swagger;

namespace WXKJ.WebApi
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddJsonOptions(options =>
            {
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            }).AddFluentValidation();

            #region 跨域处理
            //https://docs.microsoft.com/en-us/aspnet/core/security/cors
            //全局添加跨域功能 [DisableCors]不使用
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowSpecificOrigin"));
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            });
            #endregion

            #region API文档
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "网信科技API Demo", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme()
                {
                    Description = "Authorization format : Bearer {token}",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                //c.OperationFilter<HttpHeaderOperation>(); // 添加httpHeader参数
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, "OA.WebApi.xml");
                //c.IncludeXmlComments(xmlPath);
                //var xmlPath1 = Path.Combine(AppContext.BaseDirectory, "OA.Models.xml");
                //c.IncludeXmlComments(xmlPath1);
            });
            #endregion
            #region 压缩、缓存
            //gzip压缩
            services.AddResponseCompression();
            //缓存
            services.AddResponseCaching();
            #endregion

            services.AddAutoMapper();
            services.AddMvc().AddJsonOptions(opt =>
            {
                // 忽略循环引用
                opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                // 过滤 NULL 属性
                opt.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                // 设置时间格式
                opt.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });

            #region JsonWebToken

            //服务器使用私钥加密了数据  客户端使用公钥解密数据【也即客户端加密后的jwt只有公钥才能解开】
            //客户端想和服务器通信的时候  使用自己的公钥进行加密数据  只有服务器能够解密真实数据
            //var keyDir = PlatformServices.Default.Application.ApplicationBasePath;
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer(jwtBearerOptions =>
            //{
            //    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        //验证签名
            //        ValidateIssuerSigningKey = true,
            //        //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(LimitConst.SymmetricSecret)),
            //        IssuerSigningKey = RsaUtils.GetRsaSecurityKey(keyDir, false),
            //        SaveSigninToken = true,
            //        //验证发行者是否相等
            //        ValidateIssuer = true,
            //        ValidIssuer = "CMPISSUER",
            //        //验证受众是否相等
            //        ValidateAudience = true,
            //        //获取或设置表示有效受众的字符串，该字符串将用于检查令牌的受众。
            //        ValidAudience = "CMPSUBSYSTEM",  //受众的名字
            //        //验证Token有效期
            //        ValidateLifetime = true, //validate the expiration and not before values in the token
            //        ClockSkew = TimeSpan.FromMinutes(0), //2 minute tolerance for the expiration date
            //    };
            //});
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // 异常中间件
            //app.UseMiddleware(typeof(ExceptionHandlerMiddleWare));
            //app.UserDI();
            //日志中间件
            loggerFactory.AddNLog();//添加NLog
            env.ConfigureNLog("NLog.config");//读取Nlog配置文件  
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.ShowExtensions();
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "网信科技OA API V1");

                // Display
                c.DefaultModelExpandDepth(2);
                c.DefaultModelRendering(ModelRendering.Model);
                //c.DefaultModelsExpandDepth(-1);//不显示model
                c.DisplayOperationId();
                c.DisplayRequestDuration();
                c.DocExpansion(DocExpansion.None);
                //c.EnableDeepLinking();
                //c.EnableFilter();

                // Other
                c.DocumentTitle = "网上办公 API文档";
                //css注入
                //c.InjectStylesheet("/swagger-common.css");//自定义样式
                //c.InjectStylesheet("/buzyload/app.min.css");//等待load遮罩层样式
            });
            app.UseCors(opt =>
            {
                opt.AllowAnyOrigin();
                opt.AllowAnyHeader();
                opt.AllowAnyMethod();
            });
            app.UseMvc();
        }
    }
}

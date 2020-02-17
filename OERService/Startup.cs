using System;
using System.Collections.Generic;
using System.Linq;
using System.IO.Compression;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace OERService
{
    public class Startup
    {
        public Startup(IConfiguration configuration )
        {
            try
            {    
                Configuration = configuration;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                services.AddCors(o => o.AddPolicy("OERCorsPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                }));
                services.AddMvc();

                //Keyclock atuth

                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                }).AddJwtBearer(o =>
                {
                    o.Authority = Configuration["Jwt:Authority"];

                    o.Audience = Configuration["Jwt:Audience"];

                    o.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = c =>
                        {
                            c.NoResult();

                            c.Response.StatusCode = 500;

                            c.Response.ContentType = "text/plain";
                          
                            return c.Response.WriteAsync(c.Exception.ToString());
                            
                        }

                    };

                });
                //end auth section


                services.Configure<MvcOptions>(options =>
                {
                    options.Filters.Add(new CorsAuthorizationFilterFactory("OERCorsPolicy"));
                });

                services.AddResponseCompression(options =>
                {
                    options.Providers.Add<GzipCompressionProvider>();
                    options.MimeTypes =
                        ResponseCompressionDefaults.MimeTypes.Concat(
                            new[] { "image/svg+xml" });
                });

                services.Configure<GzipCompressionProviderOptions>(options =>
                {
                    options.Level = CompressionLevel.Fastest;
                });

                // Register the Swagger generator, defining 1 or more Swagger documents
                services.AddSwaggerGen(c =>
                {
                    c.AddSecurityDefinition("oauth2", new ApiKeyScheme
                    {
                        Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                        In = "header",
                        Name = "Authorization",
                        Type = "apiKey"
                    });
                    c.SwaggerDoc("v1", new Info { Title = "OER Service API", Version = "v1" });
                    var xmlDocPath = System.AppDomain.CurrentDomain.BaseDirectory + @"OERService.xml";
                    c.IncludeXmlComments(xmlDocPath);
                });
                
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            try
            {
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
                // specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "OER API V1");
                });

                //if (env.IsDevelopment())
                //{
                    app.UseDeveloperExceptionPage();
               // }
                // Enable Cors
                app.UseCors("OERCorsPolicy");
                app.UseMvc();
                app.Run(async (context) =>
                {
                 
                    await context.Response.WriteAsync("OER Service!");
                });
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }
    }
}

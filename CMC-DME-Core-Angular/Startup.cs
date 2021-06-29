using CMC_DME_Core_Angular.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace CMC_DME_Core_Angular
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
      //Inject AppSettings
      services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));

      services.AddControllers();
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "CMC_DME_Core_Angular", Version = "v1" });
      });

      services.AddDbContext<AccountDbContext>(optionns => optionns.UseSqlServer(Configuration.GetConnectionString("DevConnection")));
      //services.AddDbContext<UserDBContext>(optionns => optionns.UseSqlServer(Configuration.GetConnectionString("DevConnection")));
      services.AddDbContext<ApplicationDbContext>(optionns => optionns.UseSqlServer(Configuration.GetConnectionString("DevConnection")));

      services.AddIdentity<ApplicationUser, IdentityRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();

      services.Configure<IdentityOptions>(options =>
      {
        options.Password.RequireDigit = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 8;
      }
            );

      services.AddCors();

      //JWT Token configuration

      var jwtSecretkey = Encoding.UTF8.GetBytes(Configuration["ApplicationSettings:JWT_Secret_Code"].ToString());

      services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(options =>
      {
        options.SaveToken = false;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
          ValidateIssuer = false,
          ValidateAudience = false,
          ClockSkew = System.TimeSpan.Zero,
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(jwtSecretkey)
        };
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      app.UseCors(options => options.WithOrigins(Configuration["ApplicationSettings:Client_URL"].ToString()).AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CMC_DME_Core_Angular v1"));
      }

      app.UseRouting();

      app.UseAuthentication();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}

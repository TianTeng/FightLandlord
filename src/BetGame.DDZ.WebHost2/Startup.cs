using FreeSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;

namespace BetGame.DDZ.WebHost2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Console.WriteLine(configuration["connectionString"].ToString());
            var fsql = new FreeSql.FreeSqlBuilder()
                .UseConnectionString(FreeSql.DataType.Sqlite, configuration["connectionString"].ToString())
                .UseAutoSyncStructure(true)
                .UseNoneCommandParameter(true)
                .Build();
            fsql.Aop.ConfigEntityProperty += new EventHandler<FreeSql.Aop.ConfigEntityPropertyEventArgs>((_, e) =>
            {
                if (fsql.Ado.DataType == FreeSql.DataType.MySql || fsql.Ado.DataType == FreeSql.DataType.OdbcMySql) return;
                if (e.Property.PropertyType.IsEnum == false) return;
                e.ModifyResult.MapType = typeof(string);
            });
            fsql.Aop.CurdBefore += new EventHandler<FreeSql.Aop.CurdBeforeEventArgs>((_, e) => Trace.WriteLine(e.Sql));
            BaseEntity.Initialization(fsql);

            Fsql = fsql;
            Configuration = configuration;

            RedisHelper.Initialization(new CSRedis.CSRedisClient(configuration["redis"]));

            Newtonsoft.Json.JsonConvert.DefaultSettings = () => {
                var st = new Newtonsoft.Json.JsonSerializerSettings();
                st.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                st.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
                st.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.RoundtripKind;
                return st;
            };
        }

        public IFreeSql Fsql { get; }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<CustomExceptionFilter>();
            services.AddControllersWithViews().AddNewtonsoftJson();
            services.AddRazorPages();

            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseResponseCompression();

            app.UseDeveloperExceptionPage();

            app.UseImServer(new ImServerOptions
            {
                Redis = RedisHelper.Instance,
                Servers = new[] { Configuration["imserver"] }, //¼¯ÈºÅäÖÃ
                Server = Configuration["imserver"]
            });

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapFallbackToFile("index.html");
            });
            //app.UseDefaultFiles();
            //app.UseStaticFiles();

            ImHelper.Initialization(new ImClientOptions
            {
                Redis = RedisHelper.Instance,
                Servers = Configuration["imserver"].Split(';')
            });

            ImHelper.Instance.OnSend += (s, e) =>
                Console.WriteLine($"ImClient.SendMessage(server={e.Server},data={JsonConvert.SerializeObject(e.Message)})");
        }
    }
}

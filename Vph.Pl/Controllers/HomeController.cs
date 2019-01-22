using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using StravaSharp;
using Vph.Pl.Models;

namespace Vph.Pl.Controllers
{
    public class HomeController : Controller
    {
        public static string ClientId => "31962";
        public static string ClientSecret => "a3a8d1afccc76cdc1a3f63b2f472d93e712f057c";

        public ActionResult Index()
        {
            var authenticator = CreateAuthenticator();
            var viewModel = new HomeViewModel
            {
                AccessToken = authenticator.AccessToken,
                IsAuthenticated = authenticator.IsAuthenticated
            };
            return View(viewModel);
        }

        Authenticator CreateAuthenticator()
        {
            var redirectUrl = $"{Request.Scheme}://{Request.Host}/Home/Callback";
            var config = new RestSharp.Portable.OAuth2.Configuration.RuntimeClientConfiguration
            {
                IsEnabled = false,
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                RedirectUri = redirectUrl,
                Scope = "view_private,write",
            };
            var client = new Client(new RequestFactory(), config);

            return new Authenticator(client, HttpContext);
        }

        public async Task<ActionResult> Authenticate()
        {
            var authenticator = CreateAuthenticator();
            var loginUri = await authenticator.GetLoginLinkUri();

            return Redirect(loginUri.AbsoluteUri);
        }

        public ActionResult LogOut()
        {
            var authenticator = CreateAuthenticator();
            authenticator.AccessToken = string.Empty;

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Callback()
        {
            var authenticator = CreateAuthenticator();
            await authenticator.OnPageLoaded(new Uri(Request.GetEncodedUrl()));
            return RedirectToAction("Index");
        }

        public static string GetCurrentPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\";

        [HttpPost]
        public async Task<ActionResult> CreateActivity(DateTime startDate, DateTime endDate)
        {

            if (endDate.Date < startDate.Date || startDate == DateTime.MinValue || endDate == DateTime.MinValue)
                throw new InvalidOperationException("You're an idiot.");
            if (endDate.Date.Subtract(startDate.Date).Days > 30)
                throw new InvalidOperationException("Just do a month at a time.");

            var currentDate = startDate;
            var model = new CreateActivityResultModel();
            while (currentDate.Date <= endDate.Date)
            {
                try
                {
                    var authenticator = CreateAuthenticator();
                    var client = new StravaSharp.Client(authenticator);
                    var originalFilePath = Path.Combine(GetCurrentPath, "Biking-timestamped.gpx");
                    var dateStamp = currentDate.ToString("yyyy-MM-dd");
                    var originalFileText = System.IO.File.ReadAllText(originalFilePath).Replace("YYYY-MM-DD", dateStamp);

                    using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(originalFileText)))
                    {
                        await client.Activities.Upload(ActivityType.Ride, DataType.Gpx, stream, $"BikeRide{dateStamp}.gpx",
                            $"Bike Ride {dateStamp}");
                    }

                    model.Successes++;
                }
                catch (Exception)
                {
                    model.Failures++;
                }
                currentDate = currentDate.AddDays(1);
            }

            return View(model);
        }

        public async Task<ActionResult> Activities()
        {
            var authenticator = CreateAuthenticator();
            var client = new StravaSharp.Client(authenticator);
            var activities = await client.Activities.GetAthleteActivities();
            var models = activities.Select(activity => new ActivityViewModel(activity)).ToList();

            return View(models);
        }
    }
}

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Vph.Pl
{
    public class LocalEntryPoint
    {
        public static void Main() =>
            WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()
                .Build()
                .Run();
    }
}

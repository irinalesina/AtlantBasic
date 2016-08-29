using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AtlantWeb.Startup))]
namespace AtlantWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}

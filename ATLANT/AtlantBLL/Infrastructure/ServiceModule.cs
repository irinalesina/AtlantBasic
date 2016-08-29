using System;
using System.Collections.Generic;
using System.Linq;
using Ninject.Modules;
using AtlantDB.Interfaces;
using AtlantDB.Repositories;

namespace AtlantBLL.Infrastructure
{
    public class ServiceModule : NinjectModule
    {
        private string connectionString;
        public ServiceModule(string connection)
        {
            connectionString = connection;
        }
        public override void Load()
        {
            Bind<IAtlantDBRepository>().To<AtlantDBRepository>().WithConstructorArgument(connectionString);
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace VR.Infrastructure.Modules
{
    public class ModulesStartup
    {
        private List<IModule> _modules = new List<IModule>();
       
        public void ConfigureServices(IServiceCollection services)
        {
            foreach (IModule module in _modules)
            {
                module.ConfigureServices(services);
            }
        }

        public void RegisterModule<TModule>()
        {
            var module = (IModule)Activator.CreateInstance<TModule>();
            _modules.Add(module);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;

namespace RTCV.PluginHost
{
    public class Host
    {
        [ImportMany(typeof(IPlugin))]
        private IEnumerable<IPlugin> plugins;

        private CompositionContainer _container;

        private void initialize(string[] pluginDirs, RTCSide side)
        {
            var catalog = new AggregateCatalog();
            foreach (var dir in pluginDirs)
            {
                catalog.Catalogs.Add(new DirectoryCatalog(dir));
            }
            _container = new CompositionContainer(catalog);

            try
            {
                this._container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                Console.WriteLine(compositionException.ToString());
            }

        }

        public void Start(string[] pluginDirs, RTCSide side)
        {
            initialize(pluginDirs, side);
            Console.WriteLine("wtf");

            foreach (var p in plugins)
            {
                if (p.SupportedSide == side || p.SupportedSide == RTCSide.Both)
                {
                    Console.WriteLine($"Loading {p.Name}");
                    Console.WriteLine(p.Start(side) ? $"Loaded {p.Name} successfully" : $"Failed to load {p.Name}");
                }
            }
        }
    }
}

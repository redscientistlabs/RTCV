using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using NLog;

namespace RTCV.PluginHost
{
    public class Host
    {
        [ImportMany(typeof(IPlugin))]
        private IEnumerable<IPlugin> plugins;

        private CompositionContainer _container;
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private void initialize(string[] pluginDirs, RTCSide side)
        {
            var catalog = new AggregateCatalog();
            foreach (var dir in pluginDirs)
            {
                if(Directory.Exists(dir))
                    catalog.Catalogs.Add(new DirectoryCatalog(dir));
            }
            _container = new CompositionContainer(catalog);

            try
            {
                this._container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                logger.Error(compositionException, "Composition failed in plugin initialization");
            }

        }

        public void Start(string[] pluginDirs, RTCSide side)
        {
            initialize(pluginDirs, side);

            foreach (var p in plugins)
            {
                if (p.SupportedSide == side || p.SupportedSide == RTCSide.Both)
                {
                    logger.Info($"Loading {p.Name}");
                    if (p.Start(side))
                    {
                        logger.Info("Loaded {pluginName} successfully", p.Name);
                    }
                    else
                    {
                        logger.Error("Failed to load {pluginName}", p.Name);
                    }

                }
            }
        }
    }
}

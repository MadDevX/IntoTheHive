using DarkRift.Server;
using System;

namespace ServerPlugins
{
    public class ExamplePlugin : Plugin
    {
        public override bool ThreadSafe => false;

        public override Version Version => new Version(1, 0, 0);

        public ExamplePlugin(PluginLoadData pluginLoadData) : base(pluginLoadData)
        {
        }

    }
}

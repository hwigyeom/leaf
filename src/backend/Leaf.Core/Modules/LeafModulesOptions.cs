using System;
using System.IO;

namespace Leaf.Modules
{
    public class LeafModulesOptions
    {
        public string BasePath { get; set; } = Directory.GetCurrentDirectory();

        public Action Shutdown { get; set; } = null;
    }
}
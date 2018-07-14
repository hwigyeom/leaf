using System;
using System.Collections.Generic;

namespace Leaf.Environments
{
    public class GlobalEnvironments
    {
        public string Culture { get; set; } = "ko";

        public string Title { get; set; } = "Leaf";

        public string SubTitle { get; set; } = "Leaf Framework";

        public Dictionary<string, string> Keys { get; set; } =
            new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
    }
}
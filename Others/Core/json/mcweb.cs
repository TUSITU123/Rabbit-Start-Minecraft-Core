namespace SquareMinecraftLauncher.Core.json1
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    internal class mcweb
    {
        public class Root
        {
            public List<VersionsItem> versions { get; set; }
        }

        public class VersionsItem
        {
            public string id { get; set; }

            public string releaseTime { get; set; }

            public string time { get; set; }

            public string type { get; set; }

            public string url { get; set; }
        }
    }
}


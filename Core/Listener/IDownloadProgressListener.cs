using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastX.Core.Listener
{
    public interface IDownloadProgressListener
    {
        void OnDownloadSize(long size);
    }
}

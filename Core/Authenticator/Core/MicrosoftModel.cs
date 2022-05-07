using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastX.Core.Authenticator.Core
{
    public class xzItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        string _File, _ly, _xzwz;
        double _jd;
        public string File { get { return _File; } set { _File = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("File")); } }
        public double Template
        {
            get
            {
                return _jd;
            }
            set
            {
                _jd = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Template"));
            }
        }
        public string ly { get { return _ly; } set { _ly = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ly")); } }
        public string xzwz { get { return _xzwz; } set { _xzwz = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("xzwz")); } }
        public string url { get; set; }
        public string Path { get; set; }
        internal xzItem(string File, double jd, string ly, string xzwz, string url, string Path)
        {
            this._File = File;
            this._jd = jd;
            this._ly = ly;
            this._xzwz = xzwz;
            this.url = url;
            this.Path = Path;
        }
    }
}

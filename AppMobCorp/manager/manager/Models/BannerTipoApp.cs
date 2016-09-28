using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manager.Models
{
    public class BannerTipoApp
    {
        public int id { get; set; }

        public string tipo_app { get; set; }

        public BannerTipoApp()
        {
            this.id = 0;
        }
    }
}
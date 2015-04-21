using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace DomoticService
{
    public partial class DomoticService : ServiceBase
    {
        ServiceHost host;
        public DomoticService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            host = new ServiceHost(typeof(DomoticHostServer.DomoticService));
            host.Open();
        }

        protected override void OnStop()
        {
            if(host!=null)
                host.Close();
        }
    }
}

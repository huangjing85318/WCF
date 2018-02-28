using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
namespace HJcens.DemoWCF.Service
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class WebService
    {
        [WebGet(UriTemplate = "GetUser", ResponseFormat = WebMessageFormat.Json)]
        public string GetUser()
        {
            return "理工大";
        }
        [WebInvoke(UriTemplate = "GetID?id={id}", Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        public string GetID(string id)
        {
            // TODO: Remove the instance of SampleItem with the given id from the collection
            return "test id" + id;
        }
    }
}

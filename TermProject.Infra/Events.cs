using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TermProject.Infra
{
    public class CreateMapEvent : PubSubEvent<object> { }
    public class CreateUserControlEvent : PubSubEvent<object> { }
    public class CreateMakerEvent : PubSubEvent<Google.Apis.Drive.v3.Data.File> { }
}

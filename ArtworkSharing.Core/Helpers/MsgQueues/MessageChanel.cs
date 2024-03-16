using ArtworkSharing.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtworkSharing.Core.Helpers.MsgQueues
{
    public class MessageChanel
    {
        public string ExchangeName { get; set; }
        public string RoutingKey { get; set; }
        public string QueueName { get; set; }

        public MessageChanel PaidRaise()
        {
            MessageChanel _mesageChanel = new MessageChanel();
            _mesageChanel.QueueName = Queue.PaidRaiseQueue;
            _mesageChanel.ExchangeName = Exchange.PaidRaise;
            _mesageChanel.RoutingKey = ArtworkSharing.Core.Domain.Enums.RoutingKey.PaidRaise;
            return _mesageChanel;
        }
    
    }
}

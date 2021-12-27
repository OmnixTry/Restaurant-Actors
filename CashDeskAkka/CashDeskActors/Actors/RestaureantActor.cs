using Akka.Actor;
using CashDeskActors.Models;
using CashDeskActors.Models.Msg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashDeskActors.Actors
{
	public class RestaurantActor: ReceiveActor
	{
		public List<IActorRef> CashDesks { get; private set; }
		public int ShortestQueueLength = int.MaxValue;
		public IActorRef ShortestQueueActor = null;
		public CashDeskModel ShortestQueueModel = new CashDeskModel(-1, 11);
		public Dictionary<int, QueueLengthReply> queueLengths = new Dictionary<int, QueueLengthReply>();
		//public List<IActorRef> ActiveClients { get; private set; }
		//public List<IActorRef> ProcessedClients { get; private set; }

		public RestaurantActor(List<IActorRef> cashDesks)
		{
			CashDesks = cashDesks;
			//ActiveClients = activeClients;
			//ProcessedClients = new List<IActorRef>();

			Receive<ShortestQueueReq>(req => {
				ShortestQueueLengthReply();
			});

			Receive<QueueLengthReply>(req => {
				QueueLengthReply(req);
			});

			foreach (var item in CashDesks)
			{
				Context.System.Scheduler.ScheduleTellRepeatedly(
					TimeSpan.FromSeconds(0),
					TimeSpan.FromSeconds(1),
					item,
					new QueueLengthReq(),
					Self
				);
			}
		}

		private void ShortestQueueLengthReply()
		{
			var x = queueLengths.Values.OrderBy(x => x.DeskModel.QueueLength);
			if (x.Count() > 0)
			{
				var res = x.First();
				Sender.Tell(new ShortestQueueLengthReply(res.Length, res.Desk, res.DeskModel));
			}
		}

		private void QueueLengthReply(QueueLengthReply reply)
		{
			if (queueLengths.ContainsKey(reply.DeskModel.Id))
			{
				queueLengths[reply.DeskModel.Id] = reply;
			}
			else
			{
				queueLengths.Add(reply.DeskModel.Id, reply);
			}
			/*if(ShortestQueueLength > reply.Length)
			{
				ShortestQueueActor = Sender;
				ShortestQueueModel = reply.DeskModel;
				ShortestQueueLength = reply.Length;
			}*/
		}

	}
}

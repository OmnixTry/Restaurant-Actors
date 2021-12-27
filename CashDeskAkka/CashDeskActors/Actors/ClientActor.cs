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
	public class ClientActor : ReceiveActor
	{
		private readonly ClientModel clientModel;
		private readonly IActorRef restaurant;
		private IActorRef currentDesk;

		public ClientActor(ClientModel clientModel, IActorRef restaurant)
		{
			this.clientModel = clientModel;
			this.restaurant = restaurant;


			Receive<ShortestQueueLengthReply>(rep =>
			{
				LengthReply(rep);
			});
			Receive<EnterQueueReply>(rep =>
			{
				QueueEntered(rep);
			});
			Receive<FinishProcessingReq>(rep =>
			{
				Finish();
			});

			//restaurant.Tell(new ShortestQueueReq());
			Random random = new Random();
			Context.System.Scheduler.ScheduleTellRepeatedly(
					TimeSpan.FromSeconds(random.Next(5)),
					TimeSpan.FromSeconds(3),
					restaurant,
					new ShortestQueueReq(),
					Self
				);
		}

		private void LengthReply(ShortestQueueLengthReply reply)
		{
			//Console.WriteLine($"{DateTime.Now} Received Length");
			Random rand = new Random();
			if (reply.DeskModel == null)
			{
				Console.WriteLine("NULL!!");
			}
			if (reply.DeskModel.QueueLength < clientModel.NumberInQueue - 2)
			{
				//Console.WriteLine($"{reply.DeskModel.Id}, {reply.DeskModel.QueueLength}");
				//Console.WriteLine($"{DateTime.Now} Actor {clientModel.ClientId} Entering Queue {reply.DeskModel.Id}");
				currentDesk?.Tell(new LeaveQueueReq(clientModel));
				EnterQueue(reply.Desk);
			}
		}

		private void EnterQueue(IActorRef cashDesk)
		{
			cashDesk.Tell(new EnterQueueReq(clientModel));
			currentDesk = cashDesk;
		}

		private void QueueEntered(EnterQueueReply reply)
		{
			//Console.WriteLine($"{DateTime.Now} Queue Entered");
			clientModel.NumberInQueue = reply.QueueLength;
		}
		private void Finish()
		{
			Console.WriteLine($"{DateTime.Now} Shopping DONE!!!");
			clientModel.NumberInQueue = -1;
		}
	}
}

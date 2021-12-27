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
	public class CashDeskActor: ReceiveActor
	{
		
		private readonly CashDeskModel deskModel;
		//private readonly IActorRef restaurantActor;
		private ICancelable processingRequest;
		public CashDeskActor(int id, int clientProcessingTime)
		{
			this.deskModel = new CashDeskModel(id, clientProcessingTime);


			Receive<QueueLengthReq>(req =>
			{
				QueueLengthRequest(Sender);
			});
			Receive<EnterQueueReq>(req =>
			{
				EnterQueueRequest(req);
			});
			Receive<LeaveQueueReq>(req =>
			{
				LeaveQueueRequest(req);
			});
			Receive<ProcessClientReq>(req =>
			{
				ProcessOneClient();
			});

			var time = TimeSpan.FromSeconds(5);
			
		}

		private void QueueLengthRequest(IActorRef sender)
		{			
			var length = deskModel.QueueLength;			
			//Console.WriteLine($"{deskModel.Id}, {deskModel.QueueLength} sent");
			sender.Tell(new QueueLengthReply(length, Self, deskModel));
		}

		private void EnterQueueRequest(EnterQueueReq req)
		{
			//Console.WriteLine($"{DateTime.Now} Client {req.Client.ClientId} Enters");
			Sender.Tell(new EnterQueueReply(deskModel.QueueLength));
			if (deskModel.QueueLength == 0)
			{
				ProcessAllClients();
			}
			deskModel.AddClient((req.Client, Sender));
		}

		private void LeaveQueueRequest(LeaveQueueReq req)
		{
			//Sender.Tell(new EnterQueueReply(deskModel.QueueLength));
			//Console.WriteLine($"{DateTime.Now} Client {req.Client.ClientId} Leaves");
			deskModel.RemoveClient(req.Client);
		}

		private void ProcessAllClients()
		{
			processingRequest?.Cancel();
			processingRequest = new Cancelable(Context.System.Scheduler);

			var time = TimeSpan.FromSeconds(deskModel.ClientProcessingTime);
			Context.System.Scheduler.ScheduleTellRepeatedly(
					time,
					time,
					Self,
					new ProcessClientReq(),
					Self,
					processingRequest
				);
		}

		private void ProcessOneClient()
		{
			var client = deskModel.DequeClient();
			Console.WriteLine($"{DateTime.Now} Client {client.Item1.ClientId} Is processed by  {deskModel.Id}");
			client.Item2.Tell(new FinishProcessingReq());
			if(deskModel.QueueLength == 0)
			{
				processingRequest.Cancel();
			}

		}
	}
}

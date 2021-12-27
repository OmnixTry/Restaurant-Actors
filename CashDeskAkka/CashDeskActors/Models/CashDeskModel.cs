using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashDeskActors.Models
{
	public class CashDeskModel
	{
		public int Id { get; }
		public int ClientProcessingTime { get; }
		public int QueueLength { get { return Queue.Count; } }
		public List<(ClientModel, IActorRef)> Queue { get; private set; }

		public CashDeskModel(int id, int clientProcessingTime)
		{
			ClientProcessingTime = clientProcessingTime;
			Queue = new List<(ClientModel, IActorRef)>();
			Id = id;
		}

		public int AddClient((ClientModel, IActorRef) client)
		{
			Queue.Add(client);
			return QueueLength;
		}

		public (ClientModel, IActorRef) DequeClient()
		{
			var client = Queue.First();
			Queue = Queue.Skip(1).ToList();
			return client;
		}

		public void RemoveClient(ClientModel clientModel)
		{
			Queue = Queue.Where(x => x.Item1.ClientId != clientModel.ClientId).ToList();
		}
	}
}

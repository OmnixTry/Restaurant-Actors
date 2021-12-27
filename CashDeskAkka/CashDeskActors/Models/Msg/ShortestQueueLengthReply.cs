using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashDeskActors.Models.Msg
{
	class ShortestQueueLengthReply
	{		
		public int Length { get; }
		public IActorRef Desk { get; }
		public CashDeskModel DeskModel { get; }

		public ShortestQueueLengthReply(int length, IActorRef desk, CashDeskModel deskModel)
		{
			Length = length;
			Desk = desk;
			DeskModel = deskModel;
		}
	}
}

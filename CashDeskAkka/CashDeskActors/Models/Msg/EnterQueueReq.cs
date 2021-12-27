using CashDeskActors.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashDeskActors.Models.Msg
{
	class EnterQueueReq
	{
		public ClientModel Client { get; }

		public EnterQueueReq(ClientModel client)
		{
			this.Client = client;
		}
	}
}

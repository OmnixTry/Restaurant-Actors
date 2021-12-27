using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashDeskActors.Models.Msg
{
	public class EnterQueueReply
	{
		public int QueueLength { get; }

		public EnterQueueReply(int queueLength)
		{
			QueueLength = queueLength;
		}
	}
}

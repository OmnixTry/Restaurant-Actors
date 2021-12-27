using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashDeskActors.Models.Msg
{
	class QueueMoveReply
	{
		public int NewLength { get; }

		public QueueMoveReply(int newLength)
		{
			NewLength = newLength;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashDeskActors.Models
{
	public class ClientModel
	{
		public int ClientId { get; }
		public int NumberInQueue { get; set; } = int.MaxValue;
		public ClientModel(int clientId)
		{
			ClientId = clientId;
		}
	}
}

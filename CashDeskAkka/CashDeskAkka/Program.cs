using Akka.Actor;
using CashDeskActors.Actors;
using CashDeskActors.Models;
using System;
using System.Collections.Generic;

namespace CashDeskAkka
{
	class Program
	{
		static void Main(string[] args)
		{
			const int numOfDesks = 10;
			const int numOfClients = 100;
			ActorSystem system = ActorSystem.Create("Restaurant");
			
			

			List<IActorRef> desks = new List<IActorRef>();
			for (int i = 0; i < numOfDesks; i++)
			{
				var model = new ClientModel(i);
				desks.Add(system.ActorOf(Props.Create(() => new CashDeskActor(i, 5))));
			}

			var restaurant = system.ActorOf(Props.Create(() => new RestaurantActor(desks)));

			List<IActorRef> clients = new List<IActorRef>();
			for (int i = 0; i < numOfClients; i++)
			{
				var model = new ClientModel(i);
				clients.Add(system.ActorOf(Props.Create(() => new ClientActor(model, restaurant))));
			}

			Console.ReadLine();
			Console.ReadLine();
			Console.ReadLine();
			Console.ReadLine();
		}
	}
}

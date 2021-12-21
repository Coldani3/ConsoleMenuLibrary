using System;

namespace ConsoleMenuLibrary
{
	public class Renderer
	{
		public MenuManager MenuManager;

		public Renderer()
		{

		}

		public virtual void Render(MenuManager manager)
		{
			Console.Clear();
			manager.ActiveMenu.Display();
		}
	}
}
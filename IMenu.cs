using System;

namespace ConsoleMenuLibrary
{
	public interface IMenu
	{
		void OnInput(ConsoleKeyInfo input, MenuManager manager);
		void Display();
	}
}
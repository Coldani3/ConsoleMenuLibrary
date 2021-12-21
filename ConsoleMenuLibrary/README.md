To run, simply keep an instance of any MenuManager and pass Console.ReadKey() and MenuManager to the current Menu's OnInput method. Then call a Renderer instance's Render method with that same MenuManager passed as a parameter.

**Example:**
```c#
SampleMenu menu = new SampleMenu();
Renderer renderer = new Renderer();
MenuManager manager = new MenuManager(renderer, menu);
bool ranOnce = false;

while (Running)
{
	ConsoleKeyInfo input = Console.ReadKey(true);
	if (input.Key == ConsoleKey.Escape) 
	{
		Running = false;
	}

	if (ranOnce) 
	{
		MenuManager.ActiveMenu.OnInput(input, manager);
	}
	else 
	{
		ranOnce = true;
	}
	
	renderer.Render(manager);
	System.Threading.Thread.Sleep(100);
}
```
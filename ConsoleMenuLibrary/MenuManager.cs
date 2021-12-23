using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace ConsoleMenuLibrary
{
	public class MenuManager
	{
		public IMenu ActiveMenu { get; private set; }
		private IMenu PreviousMenu { get; set; }
		//data saved between menus
		private Dictionary<string, object> PersistentMenuData = new Dictionary<string, object>();
		protected Renderer Renderer;
		protected bool Paused = false;
		protected bool Running;
		protected int Framerate = 10;

		public MenuManager(Renderer renderer, IMenu defaultMenu)
		{
			this.Renderer = renderer;
			this.ActiveMenu = defaultMenu;
		}

		protected virtual void MenuThreadMethod()
        {
			while (this.Running)
            {
				if (!this.Paused)
                {
					ConsoleKeyInfo input = Console.ReadKey(true);
					if (input.Key == ConsoleKey.Escape)
					{
						Running = false;
					}

					this.ActiveMenu.OnInput(input, this);

					this.Renderer.Render(this);
					System.Threading.Thread.Sleep(1000 / this.Framerate);
				}
            }
        }

		public virtual void Start(Action manageMenuMethod)
        {
			if (!this.Running)
			{
				this.Running = true;
				Task menuThread = new Task(manageMenuMethod);
				menuThread.Start();
			}
        }

		public virtual void Start()
        {
			this.Start(this.MenuThreadMethod);
        }

		public virtual void Pause()
        {
			this.Paused = true;
        }

		public virtual void Resume()
        {
			this.Paused = false;
        }

		public virtual void Stop()
        {
			this.Running = false;
        }

		public virtual void ChangeMenu(IMenu newMenu)
		{
			this.PreviousMenu = this.ActiveMenu;
			this.ActiveMenu = newMenu;
			System.Console.CursorVisible = false;
			this.Renderer.Render(this);
		}

		public virtual void SetPersistentMenuData(string key, object data)
		{
			if (!this.PersistentMenuData.ContainsKey(key)) 
			{
				this.PersistentMenuData.Add(key, data);
			}
			else 
			{
				this.PersistentMenuData[key] = data;
			}
		}

		public object GetPersistentMenuData(string key)
		{
			if (this.PersistentMenuData.ContainsKey(key))
			{
				return this.PersistentMenuData[key];
			}
			else 
			{
				return null;
			}
		}

		public virtual void ClearPersistentMenuData()
		{
			this.PersistentMenuData.Clear();
		}

		public virtual IMenu GetPreviousMenu()
		{
			return PreviousMenu;
		}

		public virtual void SetRenderer(Renderer newRenderer)
		{
			this.Renderer = newRenderer;
		}

		public void SetFramerate(int newFramerate)
        {
			this.Framerate = newFramerate;
        }
	}
}
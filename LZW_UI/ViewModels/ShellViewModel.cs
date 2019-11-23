using Autofac;
using Caliburn.Micro;

namespace LZW_UI.ViewModels
{
	public class ShellViewModel : Conductor<object>
	{
		public ShellViewModel(IContainer container)
		{
			this.container = container;

			ActivateItem(container.Resolve<StartViewModel>());
		}


		private readonly IContainer container;
	}
}

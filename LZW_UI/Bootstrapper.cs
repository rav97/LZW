using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Autofac;
using Caliburn.Micro;
using LZW_UI.ViewModels;

namespace LZW_UI
{
	public class Bootstrapper : BootstrapperBase
	{
		public Bootstrapper()
		{
			Initialize();
		}

		protected override void Configure()
		{
			builder.Register(c => container).AsSelf();
			builder.RegisterType<WindowManager>().As<IWindowManager>().SingleInstance();
			builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

			GetViewModels().ToList()
				.ForEach(viewModelType =>
				{
					builder.RegisterType(viewModelType);
				});

			container = builder.Build();
		}

		protected override void OnStartup(object sender, StartupEventArgs e)
		{
			DisplayRootViewFor<ShellViewModel>();
		}

		protected override object GetInstance(Type service, string key)
		{
			object instance;
			if (string.IsNullOrWhiteSpace(key))
			{
				if (container.TryResolve(service, out instance))
					return instance;
			}
			else
			{
				if (container.TryResolveNamed(key, service, out instance))
					return instance;
			}
			throw new Exception($"Could not locate any instances of contract {key ?? service.Name}.");
		}

		protected override IEnumerable<object> GetAllInstances(Type serviceType)
		{
			return container.Resolve(typeof(IEnumerable<>).MakeGenericType(serviceType)) as IEnumerable<object>;
		}

		protected override void BuildUp(object instance)
		{
			container.InjectProperties(instance);
		}

		protected override void OnExit(object sender, EventArgs e)
		{
			container.Dispose();
		}

		private IEnumerable<Type> GetViewModels()
		{
			return GetType().Assembly.GetTypes()
				.Where(type => type.IsClass)
				.Where(type => type.Name.EndsWith("ViewModel"));
		}

		private ContainerBuilder builder = new ContainerBuilder();
		private IContainer container;
	}
}
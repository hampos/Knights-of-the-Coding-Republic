﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ilida.mobile
{
	public class AccidentListViewModel : BaseViewModel
	{
		INavigationService _nav;
		IClientService _client;
		private bool _timerActive = true;

		public AccidentListViewModel(INavigationService nav, IClientService client)
		{
			_nav = nav;
			_client = client;

			SubmitCommand = new Command(async () => await Submit());
			SelectCommand = new Command<Accident>(async (a) => await Select(a));
		}

		public ICommand SubmitCommand { get; set; }
		public ICommand SelectCommand { get; set; }

		private ICollection<Accident> _accidents;
		public ICollection<Accident> Accidents
		{
			get
			{
				return _accidents;
			}
			set
			{
				_accidents = value;
				OnPropertyChanged(nameof(Accidents));
			}
		}

		private Accident _selectedItem;
		public Accident SelectedItem
		{
			get
			{
				return _selectedItem;
			}
			set
			{
				_selectedItem = value;
				if (_selectedItem == null)
				{
					return;
				}
				SelectCommand.Execute(_selectedItem);
				_selectedItem = null;
				OnPropertyChanged(nameof(SelectedItem));
			}
		}

		public async Task Submit()
		{
			_timerActive = true;
			await _nav.PushAsync<SubmitAccidentViewModel>();
		}

		public async Task Select(Accident a)
		{
			var avm = new AccidentViewModel(_nav)
			{
				AccidentId = a.AccidentId,
				Status = a.Status
			};
			_timerActive = true;
			await _nav.PushAsync(avm);
		}

		public override async void Activated()
		{
			base.Activated();
			_timerActive = true;
			StartTimer();

			Accidents = await _client.GetAccidents();
			//	new List<Accident>()
			//{
			//	new Accident(){ AccidentId="891291", Date="11/09/2016 14:30", Status="Προς Διακανονισμο"},
			//	new Accident(){ AccidentId="732291", Date="12/03/2015 11:00", Status="Ολοκληρώθηκε"}
			//};
		}

		public bool TimerHandle()
		{
			Task.Factory.StartNew(async () =>
			{
				Accidents = await _client.GetAccidents();
			});
			return _timerActive;
		}

		public void StartTimer()
		{
			Xamarin.Forms.Device.StartTimer(new TimeSpan(0, 0, 5), () => TimerHandle());
		}

	}
}

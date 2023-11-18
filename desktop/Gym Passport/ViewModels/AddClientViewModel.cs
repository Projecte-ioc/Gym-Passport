using Gym_Passport.Commands;
using Gym_Passport.Services;
using Gym_Passport.Stores;
using System.Windows.Input;

namespace Gym_Passport.ViewModels
{
    public class AddClientViewModel : ViewModelBase
    {
		private string _name;
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
				OnPropertyChanged(nameof(Name));
			}
		}

		private string _username;
		public string Username
		{
			get
			{
				return _username;
			}
			set
			{
				_username = value;
				OnPropertyChanged(nameof(Username));
			}
		}

		private string _role;
		public string Role
		{
			get
			{
				return _role;
			}
			set
			{
				_role = value;
				OnPropertyChanged(nameof(Role));
			}
		}

		public ICommand SubmitCommand { get; }
		public ICommand CancelCommand { get; }

        public AddClientViewModel(UsersStore peopleStore, INavigationService closeNavigationService)
        {
			SubmitCommand = new AddUserCommand(this, peopleStore, closeNavigationService);
            CancelCommand = new NavigateCommand(closeNavigationService);
        }
    }
}

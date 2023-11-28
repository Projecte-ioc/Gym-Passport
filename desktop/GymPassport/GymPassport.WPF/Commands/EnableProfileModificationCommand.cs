using GymPassport.WPF.ViewModels;

namespace GymPassport.WPF.Commands
{
    public class EnableProfileModificationCommand : CommandBase
    {
        private readonly ProfileViewModel _profileViewModel;

        public EnableProfileModificationCommand(ProfileViewModel profileViewModel)
        {
            _profileViewModel = profileViewModel;
        }

        public override void Execute(object? parameter)
        {
            if (!_profileViewModel.IsEnabled)
            {
                _profileViewModel.IsEnabled = true;
                _profileViewModel.btnToggleModificationText = "Bloquejar modificació";
            }
            else
            {
                _profileViewModel.IsEnabled = false;
                _profileViewModel.btnToggleModificationText = "Desbloquejar modificació";
            }
        }
    }
}

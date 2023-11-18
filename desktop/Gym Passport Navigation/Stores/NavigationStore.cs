using Gym_Passport_Navigation.ViewModels;
using System;

namespace Gym_Passport_Navigation.Stores
{
    public class NavigationStore
    {
        public event Action CurrentViewModelChanged;

        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel?.Dispose();
                _currentViewModel = value;
                OnCurrentViewModelChanged();
            }
        }

        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
    }
}

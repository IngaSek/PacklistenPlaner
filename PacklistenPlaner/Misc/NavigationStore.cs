using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacklistenPlaner;

public class NavigationStore
{
    private ObservableObject currentViewModel;
    public ObservableObject CurrentViewModel
    {
        get { return currentViewModel; }
        set
        {
            currentViewModel = value;
            NotifyCurrentViewModelChanged();
        }
    }
    public event Action CurrentViewModelChanged;
    private void NotifyCurrentViewModelChanged()
    {
        CurrentViewModelChanged?.Invoke();
    }

    public void NavigateTo(ObservableObject newViewModel)
    {
        CurrentViewModel = newViewModel;
    }
}

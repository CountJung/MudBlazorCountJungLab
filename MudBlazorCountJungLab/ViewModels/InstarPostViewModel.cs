using MudBlazorCountJungLab.Interfaces;
using MudBlazorCountJungLab.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MudBlazorCountJungLab.ViewModels
{
    public class InstarPostViewModel : INotifyPropertyChanged , IFormModel
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual bool Set<T>(ref T member, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(member, value))
                return false;

            member = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private InstarPostItem? currentInstarPostItem;
        public InstarPostItem? CurrentInstarPostItem { get => currentInstarPostItem ??= new(); set => Set(ref currentInstarPostItem, value); }

        private List<InstarPostItem>? instarPostItems;
        public List<InstarPostItem>? InstarPostItems { get => instarPostItems ??= new(); set => Set(ref instarPostItems, value); }

        public void SubmitPost()
        {
            //New case
            if(CurrentInstarPostItem!.Id.Equals(Guid.Empty))
            {
                CurrentInstarPostItem.Id = Guid.NewGuid();
                CurrentInstarPostItem.RecordDateTime = DateTime.Now;
                instarPostItems?.Add(CurrentInstarPostItem);
            }
            OnPropertyChanged(nameof(InstarPostItems));
        }
    }
}

using MudBlazorCountJungLab.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MudBlazorCountJungLab.ViewModels
{
    public class InstarPostViewModel : INotifyPropertyChanged
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

        public void SavePostItems(InstarPostItem instarPostItem)
        {
            //New case
            if(instarPostItem.Id.Equals(Guid.Empty))
            {
                instarPostItem.Id = Guid.NewGuid();
                instarPostItem.RecordDateTime = DateTime.Now;
                instarPostItems?.Add(instarPostItem);
            }
            OnPropertyChanged(nameof(InstarPostItems));
        }
    }
}

using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace AdaptationManagement.Models
{
    public class ModuleViewModel : INotifyPropertyChanged
    {
        private bool isSelected;
        private Mentor selectedMentor;

        public AdaptationModule Module { get; set; }
        public List<Mentor> AvailableMentors { get; set; }

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        public Mentor SelectedMentor
        {
            get => selectedMentor;
            set
            {
                if (selectedMentor != value)
                {
                    selectedMentor = value;
                    OnPropertyChanged(nameof(SelectedMentor));
                }
            }
        }

        public string Name => Module?.Name;
        public string Position => Module?.Position;
        public string Status => Module?.Status;
        public List<string> Developers => Module?.Developers;
        public List<string> Approvers => Module?.Approvers;
        public string MainApprover => Module?.MainApprover;
        public DateTime DeadlineDate => Module?.DeadlineDate ?? DateTime.MinValue;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
﻿using OpenTracker.Interfaces;
using OpenTracker.Models.AccessibilityLevels;
using OpenTracker.Models.Sections;
using OpenTracker.Models.UndoRedo;
using ReactiveUI;
using System;
using System.ComponentModel;
using System.Globalization;

namespace OpenTracker.ViewModels.PinnedLocations.SectionIcons
{
    /// <summary>
    /// This is the ViewModel of the section icon control representing an item section.
    /// </summary>
    public class ItemSectionIconVM : SectionIconVMBase, IClickHandler
    {
        private readonly IItemSection _section;

        public string ImageSource
        {
            get
            {
                if (_section.IsAvailable())
                {
                    switch (_section.Accessibility)
                    {
                        case AccessibilityLevel.None:
                        case AccessibilityLevel.Inspect:
                            {
                                return "avares://OpenTracker/Assets/Images/chest0.png";
                            }
                        case AccessibilityLevel.Partial:
                        case AccessibilityLevel.SequenceBreak:
                        case AccessibilityLevel.Normal:
                            {
                                return "avares://OpenTracker/Assets/Images/chest1.png";
                            }
                    }
                }

                return "avares://OpenTracker/Assets/Images/chest2.png";
            }
        }
        public string AvailableCount =>
            _section.Available.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="section">
        /// The item section to be represented.
        /// </param>
        public ItemSectionIconVM(IItemSection section)
        {
            _section = section ?? throw new ArgumentNullException(nameof(section));

            _section.PropertyChanged += OnSectionChanged;
        }

        /// <summary>
        /// Subscribes to the PropertyChanged event on the IItemSection interface.
        /// </summary>
        /// <param name="sender">
        /// The sending object of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the PropertyChanged event.
        /// </param>
        private void OnSectionChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ISection.Accessibility))
            {
                UpdateImage();
            }

            if (e.PropertyName == nameof(ISection.Available))
            {
                UpdateImage();
                this.RaisePropertyChanged(nameof(AvailableCount));
            }
        }

        /// <summary>
        /// Raises the PropertyChanged event for the ImageSource property.
        /// </summary>
        private void UpdateImage()
        {
            this.RaisePropertyChanged(nameof(ImageSource));
        }

        /// <summary>
        /// Handles left clicks and collects the section.
        /// </summary>
        /// <param name="force">
        /// A boolean representing whether the logic should be ignored.
        /// </param>
        public void OnLeftClick(bool force)
        {
            UndoRedoManager.Instance.Execute(new CollectSection(_section, force));
        }

        /// <summary>
        /// Handles right click and uncollects the section.
        /// </summary>
        /// <param name="force">
        /// A boolean representing whether the logic should be ignored.
        /// </param>
        public void OnRightClick(bool force)
        {
            UndoRedoManager.Instance.Execute(new UncollectSection(_section));
        }
    }
}

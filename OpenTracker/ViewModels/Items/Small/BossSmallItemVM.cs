﻿using OpenTracker.Interfaces;
using ReactiveUI;
using System;
using System.ComponentModel;
using OpenTracker.Models.Requirements;
using OpenTracker.Models.BossPlacements;
using OpenTracker.ViewModels.BossSelect;

namespace OpenTracker.ViewModels.Items.Small
{
    /// <summary>
    /// This is the ViewModel for the small Items panel control representing the boss.
    /// </summary>
    public class BossSmallItemVM : SmallItemVMBase, IClickHandler
    {
        private readonly IBossPlacement _bossPlacement;
        private readonly IRequirement _requirement;

        public bool Visible =>
            _requirement.Met;
        public string ImageSource =>
            _bossPlacement.Boss.HasValue ? "avares://OpenTracker/Assets/Images/Bosses/" +
            $"{_bossPlacement.Boss.ToString().ToLowerInvariant()}1.png" :
            "avares://OpenTracker/Assets/Images/Bosses/" +
            $"{_bossPlacement.DefaultBoss.ToString().ToLowerInvariant()}0.png";

        public BossSelectPopupVM BossSelect { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bossPlacement">
        /// The boss section to which the boss belongs.
        /// </param>
        public BossSmallItemVM(IBossPlacement bossPlacement)
        {
            _bossPlacement = bossPlacement ?? throw new ArgumentNullException(nameof(bossPlacement));
            _requirement = RequirementDictionary.Instance[RequirementType.BossShuffleOn];
            BossSelect = BossSelectVMFactory.GetBossSelectPopupVM(_bossPlacement);

            _bossPlacement.PropertyChanged += OnBossChanged;
            _requirement.PropertyChanged += OnRequirementChanged;
        }

        /// <summary>
        /// Subscribes to the PropertyChanged event on the IRequirement interface.
        /// </summary>
        /// <param name="sender">
        /// The sending object of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the PropertyChanged event.
        /// </param>
        private void OnRequirementChanged(object sender, PropertyChangedEventArgs e)
        {
            this.RaisePropertyChanged(nameof(Visible));
        }

        /// <summary>
        /// Subscribes to the PropertyChanged event on the ISection interface.
        /// </summary>
        /// <param name="sender">
        /// The sending object of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the PropertyChanged event.
        /// </param>
        private void OnBossChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IBossPlacement.Boss))
            {
                this.RaisePropertyChanged(nameof(ImageSource));
            }
        }

        /// <summary>
        /// Handles left clicks and opens the boss select popup.
        /// </summary>
        /// <param name="force">
        /// A boolean representing whether the logic should be ignored.
        /// </param>
        public void OnLeftClick(bool force = false)
        {
            BossSelect.PopupOpen = true;
        }

        /// <summary>
        /// Handles right clicks.
        /// </summary>
        /// <param name="force">
        /// A boolean representing whether the logic should be ignored.
        /// </param>
        public void OnRightClick(bool force = false)
        {
        }
    }
}

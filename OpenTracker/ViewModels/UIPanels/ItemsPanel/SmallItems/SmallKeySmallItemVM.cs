﻿using OpenTracker.Interfaces;
using OpenTracker.Models;
using OpenTracker.Models.Items;
using OpenTracker.Models.Requirements;
using OpenTracker.Models.UndoRedo;
using ReactiveUI;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace OpenTracker.ViewModels.UIPanels.ItemsPanel.SmallItems
{
    /// <summary>
    /// This is the ViewModel of the small Items panel control representing a small key.
    /// </summary>
    public class SmallKeySmallItemVM : SmallItemVMBase, IClickHandler
    {
        private readonly IRequirement _requirement;
        private readonly IItem _item;

        public bool Visible =>
            _requirement.Met;
        public bool TextVisible =>
            _item.Current > 0;
        public string ItemNumber
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(_item.Current.ToString(CultureInfo.InvariantCulture));
                sb.Append(_item.Current == _item.Maximum ? "*" : "");

                return sb.ToString();
            }
        }
        public string TextColor
        {
            get
            {
                if (_item == null)
                {
                    return "#ffffff";
                }

                if (_item.Current == _item.Maximum)
                {
                    return AppSettings.Instance.EmphasisFontColor;
                }

                return "#ffffff";
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="item">
        /// The item of the key to be represented.
        /// </param>
        public SmallKeySmallItemVM(IItem item)
        {
            _item = item ?? throw new ArgumentNullException(nameof(item));
            _requirement = RequirementDictionary.Instance[RequirementType.SmallKeyShuffleOn];

            AppSettings.Instance.PropertyChanged += OnAppSettingsChanged;
            _item.PropertyChanged += OnItemChanged;
            _requirement.PropertyChanged += OnRequirementChanged;
        }

        /// <summary>
        /// Subscribes to the PropertyChanged event on the AppSettings class.
        /// </summary>
        /// <param name="sender">
        /// The sending object of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the PropertyChanged event.
        /// </param>
        private void OnAppSettingsChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AppSettings.EmphasisFontColor))
            {
                UpdateTextColor();
            }
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
        /// Subscribes to the PropertyChanged event on the IItem interface.
        /// </summary>
        /// <param name="sender">
        /// The sending object of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the PropertyChanged event.
        /// </param>
        private void OnItemChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateTextColor();
            this.RaisePropertyChanged(nameof(TextVisible));
            this.RaisePropertyChanged(nameof(ItemNumber));
        }

        /// <summary>
        /// Raises the PropertyChanged event for the TextColor properties.
        /// </summary>
        private void UpdateTextColor()
        {
            this.RaisePropertyChanged(nameof(TextColor));
        }

        /// <summary>
        /// Handles left clicks and adds an item.
        /// </summary>
        /// <param name="force">
        /// A boolean representing whether the logic should be ignored.
        /// </param>
        public void OnLeftClick(bool force = false)
        {
            UndoRedoManager.Instance.Execute(new AddItem(_item));
        }

        /// <summary>
        /// Handles right clicks and removes an item.
        /// </summary>
        /// <param name="force">
        /// A boolean representing whether the logic should be ignored.
        /// </param>
        public void OnRightClick(bool force = false)
        {
            UndoRedoManager.Instance.Execute(new RemoveItem(_item));
        }
    }
}
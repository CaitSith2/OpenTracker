﻿using Avalonia;
using Avalonia.Layout;
using OpenTracker.Interfaces;
using OpenTracker.Models.Connections;
using OpenTracker.Models.Locations;
using OpenTracker.Models.Modes;
using OpenTracker.Models.Settings;
using OpenTracker.Models.UndoRedo;
using ReactiveUI;
using System;
using System.ComponentModel;

namespace OpenTracker.ViewModels.Maps
{
    /// <summary>
    /// This is the ViewModel of the control representing a connection between entrance locations.
    /// </summary>
    public class MapConnectionVM : ViewModelBase, IClickHandler, IPointerOver
    {
        private readonly MapAreaVM _mapArea;

        private bool _highlighted;
        public bool Highlighted
        {
            get => _highlighted;
            private set => this.RaiseAndSetIfChanged(ref _highlighted, value);
        }

        public static bool Visible =>
            Mode.Instance.EntranceShuffle > EntranceShuffle.None;

        public Connection Connection { get; }

        public Point Start =>
            MapAreaVM.Orientation switch
            {
                Orientation.Vertical => Connection.Location1.Map == MapID.DarkWorld ?
                    new Point(Connection.Location1.X + 23, Connection.Location1.Y + 2046) :
                    new Point(Connection.Location1.X + 23, Connection.Location1.Y + 13),
                _ => Connection.Location1.Map == MapID.DarkWorld ?
                    new Point(Connection.Location1.X + 2046, Connection.Location1.Y + 23) :
                    new Point(Connection.Location1.X + 13, Connection.Location1.Y + 23)
            };
        public Point End =>
            MapAreaVM.Orientation switch
            {
                Orientation.Vertical => Connection.Location2.Map == MapID.DarkWorld ?
                    new Point(Connection.Location2.X + 23, Connection.Location2.Y + 2046) :
                    new Point(Connection.Location2.X + 23, Connection.Location2.Y + 13),
                _ => Connection.Location2.Map == MapID.DarkWorld ?
                    new Point(Connection.Location2.X + 2046, Connection.Location2.Y + 23) :
                    new Point(Connection.Location2.X + 13, Connection.Location2.Y + 23)
            };
        public string Color =>
            Highlighted ? "#ffffffff" : AppSettings.Instance.Colors.ConnectorColor;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connection">
        /// The connection data.
        /// </param>
        /// <param name="mapArea">
        /// The map area ViewModel parent class.
        /// </param>
        public MapConnectionVM(Connection connection, MapAreaVM mapArea)
        {
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _mapArea = mapArea ?? throw new ArgumentNullException(nameof(mapArea));

            PropertyChanged += OnPropertyChanged;
            _mapArea.PropertyChanged += OnMapAreaChanged;
            Mode.Instance.PropertyChanged += OnModeChanged;
            AppSettings.Instance.Colors.PropertyChanged += OnColorsChanged;
        }

        /// <summary>
        /// Subscribes to the PropertyChanged event on itself.
        /// </summary>
        /// <param name="sender">
        /// The sending object of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the PropertyChanged event.
        /// </param>
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Highlighted))
            {
                UpdateColor();
            }
        }

        /// <summary>
        /// Subscribes to the PropertyChanged event on the MapAreaControlVM class.
        /// </summary>
        /// <param name="sender">
        /// The sending object of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the PropertyChanged event.
        /// </param>
        private void OnMapAreaChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MapAreaVM.Orientation))
            {
                this.RaisePropertyChanged(nameof(Start));
                this.RaisePropertyChanged(nameof(End));
            }
        }

        /// <summary>
        /// Subscribes to the PropertyChanged event on the Mode class.
        /// </summary>
        /// <param name="sender">
        /// The sending object of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the PropertyChanged event.
        /// </param>
        private void OnModeChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Mode.EntranceShuffle))
            {
                this.RaisePropertyChanged(nameof(Visible));
            }
        }

        /// <summary>
        /// Subscribes to the PropertyChanged event on the ColorSettings class.
        /// </summary>
        /// <param name="sender">
        /// The sending object of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the PropertyChanged event.
        /// </param>
        private void OnColorsChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ColorSettings.ConnectorColor))
            {
                UpdateColor();
            }
        }

        /// <summary>
        /// Raises the PropertyChanged event for the Color property.
        /// </summary>
        private void UpdateColor()
        {
            this.RaisePropertyChanged(nameof(Color));
        }

        /// <summary>
        /// Handles left clicks.
        /// </summary>
        /// <param name="force">
        /// A boolean representing whether the logic should be ignored.
        /// </param>
        public void OnLeftClick(bool force = false)
        {
        }

        /// <summary>
        /// Handles right clicks and removes the connection.
        /// </summary>
        /// <param name="force">
        /// A boolean representing whether the logic should be ignored.
        /// </param>
        public void OnRightClick(bool force = false)
        {
            UndoRedoManager.Instance.Execute(new RemoveConnection(Connection));
        }

        /// <summary>
        /// Handles pointer entering the control and highlights the control.
        /// </summary>
        public void OnPointerEnter()
        {
            Highlighted = true;
        }

        /// <summary>
        /// Handles pointer exiting the control and unhighlights the control.
        /// </summary>
        public void OnPointerLeave()
        {
            Highlighted = false;
        }
    }
}

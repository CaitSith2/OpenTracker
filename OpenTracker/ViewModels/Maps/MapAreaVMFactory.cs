﻿using OpenTracker.Models.Locations;
using OpenTracker.ViewModels.Maps.MapLocations;
using System;
using System.Collections.ObjectModel;

namespace OpenTracker.ViewModels.Maps
{
    /// <summary>
    /// This is the class for creating map area control ViewModel classes.
    /// </summary>
    internal static class MapAreaVMFactory
    {
        /// <summary>
        /// Returns an observable collection of map control ViewModel instances.
        /// </summary>
        /// <returns>
        /// An observable collection of map control ViewModel instances.
        /// </returns>
        internal static ObservableCollection<MapVM> GetMapControlVMs()
        {
            ObservableCollection<MapVM> maps = new ObservableCollection<MapVM>();

            foreach (MapID map in Enum.GetValues(typeof(MapID)))
            {
                maps.Add(new MapVM(map));
            }

            return maps;
        }

        /// <summary>
        /// Returns an observable collection of map location control ViewModel instances.
        /// </summary>
        /// <returns>
        /// An observable collection of map location control ViewModel instances.
        /// </returns>
        internal static ObservableCollection<MapLocationVMBase> GetMapLocationControlVMs()
        {
            ObservableCollection<MapLocationVMBase> mapLocations =
                new ObservableCollection<MapLocationVMBase>();

            foreach (LocationID id in Enum.GetValues(typeof(LocationID)))
            {
                foreach (var mapLocation in LocationDictionary.Instance[id].MapLocations)
                {
                    mapLocations.Add(MapLocationVMFactory.GetMapLocationControlVM(mapLocation));
                }
            }

            return mapLocations;
        }
    }
}

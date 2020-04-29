﻿using System;
using System.Collections;
using System.Reflection;

namespace SIS.MvcFramework.Mapping
{
    public static class ModelMapper
    {
        private static void MapProperty(object originInstance, object destinationInstance,
        PropertyInfo originProperty, PropertyInfo destinationProperty)
        {
            if (destinationProperty == null)
            {
                return;
            }

            if (originProperty.PropertyType.IsPrimitive //primitive types
                || originProperty.PropertyType == typeof(string)
                || originProperty.PropertyType == typeof(decimal))
            {
                if (originProperty.PropertyType != typeof(string)
                    && destinationProperty.PropertyType == typeof(string))
                {
                    destinationProperty.SetValue(destinationInstance, 
                        originProperty.GetValue(originInstance).ToString());
                }
                else if (originProperty.PropertyType != destinationProperty.PropertyType)
                {
                    destinationProperty.SetValue(destinationInstance,
                        System.Convert.ChangeType(originProperty.GetValue(originInstance)
                        , destinationProperty.PropertyType));
                }
                else if (originProperty.PropertyType == destinationProperty.PropertyType)
                {
                    destinationProperty.SetValue(destinationInstance,
                        originProperty.GetValue(originInstance));
                }
            }
            else if (typeof(IEnumerable).IsAssignableFrom(destinationProperty.PropertyType))//Colections
            {
                var originCollection = (IEnumerable)originProperty.GetValue(originInstance);
                var destinationElementType = destinationProperty.GetValue(destinationInstance)
                    .GetType()
                    .GetGenericArguments()[0];

                var destinationCollection = (IList)Activator
                    .CreateInstance(destinationProperty.PropertyType);

                foreach (var originElement in originCollection)
                {
                    destinationCollection.Add(MapObject(originElement, destinationElementType));
                }

                destinationProperty.SetValue(destinationInstance, destinationCollection);
            }
            else
            {
                var originValue = originProperty.GetValue(originInstance); //complex object
                var destinationValue = MapObject(originValue, destinationProperty.PropertyType); //Recursive mapping

                destinationProperty.SetValue(destinationInstance, destinationValue);
            }
        }

        private static object MapObject(object origin, Type destinationType)
        {
            var destinationInstance = Activator.CreateInstance(destinationType);

            foreach (var originProperty in origin.GetType().GetProperties())
            {
                string propertyName = originProperty.Name;
                PropertyInfo destinationProperty = destinationInstance.GetType().GetProperty(propertyName);

                MapProperty(origin, destinationInstance, originProperty, destinationProperty);
            }

            return destinationInstance;
        }

        public static TDestination ProjectTo<TDestination>(object origin)
        {
            return (TDestination)MapObject(origin, typeof(TDestination));
        }
    }
}

using System;
using System.Reflection;

namespace Ultility
{
    public class MyMapper
    {
        /// <summary>
        /// Tạo 1 đối tượng mới kiểu TDestination bằng cách Map từ đối tượng source kiểu TSource
        /// </summary>
        /// <typeparam name="TSource">Kiểu nguồn</typeparam>
        /// <typeparam name="TDestination">Kiểu đích</typeparam>
        /// <param name="source">Đối tượng nguồn Map</param>
        /// <returns></returns>
        public static TDestination Map<TSource, TDestination>(TSource source) => Map(source, Activator.CreateInstance<TDestination>());

        /// <summary>
        /// Map đối tượng source kiểu TSource vào đối tượng có sẵn destinationInit kiểu TSource , kết quả được trả về
        /// </summary>
        /// <typeparam name="TSource">Kiểu nguồn</typeparam>
        /// <typeparam name="TDestination">Kiểu đích</typeparam>
        /// <param name="source">Đối tượng nguồn Map</param>
        /// <param name="destinationInit">Đối tượng đích cần Map</param>
        /// <returns></returns>
        public static TDestination Map<TSource, TDestination>(TSource source, TDestination destinationInit)
        {
            return Map(source, destinationInit, (propSource, destination) =>
            {
                var propDestination = destination.GetType().GetProperty(propSource.Name);
                if (propDestination != null && propDestination.ToString() == propSource.ToString())
                {
                    propDestination.SetValue(destination, propSource.GetValue(source));
                }
            });
        }

        /// <summary>
        /// [Tùy biến] Map đối tượng source kiểu TSource vào đối tượng có sẵn destinationInit kiểu TSource , kết quả được trả về
        /// </summary>
        /// <typeparam name="TSource">Kiểu nguồn</typeparam>
        /// <typeparam name="TDestination">Kiểu đích</typeparam>
        /// <param name="source">Đối tượng nguồn Map</param>
        /// <param name="destinationInit">Đối tượng đích cần Map</param>
        /// <param name="MapAction">Mô tả hành vi Map với Thuộc tính đối tượng nguồn và Object là đối tượng đích được Map</param>
        /// <returns></returns>
        public static TDestination Map<TSource, TDestination>(TSource source, TDestination destinationInit,
            Action<PropertyInfo, object> MapAction)
        {
            TDestination destination = destinationInit;

            foreach (var propSource in source.GetType().GetProperties())
            {
                MapAction(propSource, destination);
            }

            return destination;
        }

        /// <summary>
        /// [Tùy biến] Map đối tượng source kiểu TSource vào đối tượng có sẵn destinationInit kiểu TSource , kết quả được trả về
        /// </summary>
        /// <typeparam name="TSource">Kiểu nguồn</typeparam>
        /// <typeparam name="TDestination">Kiểu đích</typeparam>
        /// <param name="source">Đối tượng nguồn Map</param>
        /// <param name="destinationInit">Đối tượng đích cần Map</param>
        /// <param name="MapAction">Mô tả hành vi Map với Thuộc tính đối tượng nguồn, Thuộc tính đối tượng đích CÙNG TÊN và Object là đối tượng đích được Map</param>
        /// <returns></returns>
        public static TDestination Map<TSource, TDestination>(TSource source, TDestination destinationInit,
            Action<PropertyInfo, PropertyInfo, object> MapAction)
        {
            TDestination destination = destinationInit;

            foreach (var propSource in source.GetType().GetProperties())
            {
                var propDestinationSameName = destination.GetType().GetProperty(propSource.Name);
                if (propDestinationSameName != null)
                {
                    MapAction(propSource, propDestinationSameName, destination);
                }
            }

            return destination;
        }
    }
}

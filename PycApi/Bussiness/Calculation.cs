using PycApi.Model;
using System.Collections.Generic;
using System;
using System.Linq;

namespace PycApi.Bussiness
{
    public static class Calculation
    {
        public static ContainersWithDistance GetDistance(Container container1, Container container2)
        {
            //değerler radyana dönüştürülür
            var lat1 = container1.Latitude * Math.PI / 180;
            var lon1 = container1.Longitude * Math.PI / 180;
            var lat2 = container2.Latitude * Math.PI / 180;
            var lon2 = container2.Longitude * Math.PI / 180;

            var dlat = lat2 - lat1; //koordinatlar farkı
            var dlon = lon2 - lon1; //koordinatlar farkı

            var distance = 2 * Math.Asin
                               (
                                  Math.Sqrt
                                  (
                                     Math.Pow(Math.Sin(dlat / 2), 2) +
                                     Math.Cos(lat1) *
                                     Math.Cos(lat2) *
                                     Math.Pow(Math.Sin(dlon / 2), 2)
                                  )
                               ) * 6371; // değerler km cinsine dönüşütürülür.

            var containers = new List<Container>() { container1, container2 };
            var containersWithDistance = new ContainersWithDistance(containers, distance);

            return containersWithDistance;
        }

        // Kümeleme yapılır.
        // konteynerların ve uzaklıkların listesi çekilir.
        // uzaklık mesafeleri küçükten büyüğe olacak şekilde sıralanır
        // belirtilen küme sayısına uygun şekilde kümelenir.
        public static List<List<ContainersWithDistance>> Cluster(List<ContainersWithDistance> list, int clusterCount)
        {
            var index = 0;
            var clusteredList = list.OrderBy(c => c.Distance) // uzaklık mesafelerine göre sıralama yapılır.
                                    .GroupBy(s => index++ / clusterCount) // belirtilen küme sayısı değerine göre kümeleme yapılır.
                                    .Select(g => g.ToList()) // kümeler  listeye dönüştürülür.
                                    .ToList(); // kümelerin listesi oluşturulur.

            return clusteredList; //sonuç döndürülür
        }
    }
}

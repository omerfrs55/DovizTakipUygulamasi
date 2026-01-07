# Döviz Takip Konsol Uygulaması

Bu proje, Görsel Programlama dersi kapsamında geliştirilmiş, gerçek zamanlı veri çeken ve bu veriler üzerinde filtreleme, sıralama ve istatistiksel işlemler yapan bir C# Konsol uygulamasıdır.

## Proje Bilgileri

* **Geliştiren:** Ömer Faruk SAĞLAM
* **Ders Sorumlusu:** Emrah SARIÇİÇEK
* **Programlama Dili:** C#
* **Proje Türü:** Konsol Uygulaması (Console Application)

## Proje Tanımı ve Amacı

Bu uygulamanın amacı, "Frankfurter FREE API" servisini kullanarak anlık döviz kurlarını çekmek ve kullanıcıya bu veriler üzerinde LINQ sorguları ile işlem yapma imkanı sunmaktır. Uygulama, nesne tabanlı programlama prensiplerine (OOP) ve katmanlı mimari mantığına uygun olarak tasarlanmıştır.

## Teknik Özellikler ve Kullanılan Teknolojiler

Proje geliştirilirken aşağıdaki teknoloji ve kütüphaneler kullanılmıştır:

* **HttpClient:** API üzerinden HTTP istekleri atmak ve veri çekmek için kullanılmıştır.
* **Async / Await:** API çağrılarının asenkron olarak yapılması ve arayüzün donmaması için kullanılmıştır.
* **LINQ (Language Integrated Query):** Veri listeleme, filtreleme, sıralama ve istatistik hesaplamaları (Where, Select, OrderBy, Average, Max, Min) için kullanılmıştır.
* **Newtonsoft.Json:** API'den gelen JSON formatındaki veriyi C# nesnelerine (Deserialize) dönüştürmek için kullanılmıştır.
* **Try-Catch Blokları:** Olası internet bağlantı hataları ve veri tipi uyuşmazlıklarını (örn: sayı yerine harf girilmesi) yönetmek için kullanılmıştır.

## Uygulama Özellikleri (Menü İşlevleri)

Uygulama kullanıcıya aşağıdaki işlevleri sunmaktadır:

1.  **Tüm Dövizleri Listele:** API'den çekilen güncel kur listesini tablo formatında gösterir.
2.  **Koda Göre Arama:** Kullanıcının girdiği döviz koduna (örn: USD, EUR) göre arama yapar. Büyük/küçük harf duyarlılığı yoktur.
3.  **Değere Göre Filtreleme:** Kullanıcının belirlediği bir kur değerinin üzerindeki dövizleri listeler.
4.  **Sıralama:** Döviz kurlarını "Küçükten Büyüğe" veya "Büyükten Küçüğe" şeklinde sıralama imkanı sunar.
5.  **İstatistiksel Özet:** Toplam döviz sayısı, en yüksek kur, en düşük kur ve ortalama kur bilgisini hesaplayarak gösterir.

## Proje Dosya Yapısı

Proje, Sorumlulukların Ayrılığı (Separation of Concerns) ilkesine göre dosyalara bölünmüştür:

* **Program.cs:** Uygulamanın giriş noktasıdır. Kullanıcı menüsü, renkli ekran çıktıları ve kullanıcı etkileşimi burada yönetilir.
* **DovizService.cs:** İş mantığı katmanıdır. API bağlantısı ve LINQ sorguları bu sınıfta bulunur.
* **Currency.cs:** Uygulama içinde kullanılan temel döviz modelidir.
* **CurrencyResponse.cs:** API'den gelen JSON verisini karşılamak için kullanılan veri transfer modelidir.

## Kurulum ve Çalıştırma

Projeyi kendi bilgisayarınızda çalıştırmak için aşağıdaki adımları izleyebilirsiniz:

1.  Projeyi bilgisayarınıza indirin veya kopyalayın.
2.  Proje klasöründe terminali açın.
3.  Gerekli JSON kütüphanesini yüklemek için şu komutu çalıştırın:
    `dotnet add package Newtonsoft.Json`
4.  Projeyi derlemek ve çalıştırmak için şu komutu kullanın:
    `dotnet run`

## Veri Kaynağı

Uygulama, döviz verilerini aşağıdaki açık kaynak API üzerinden temin etmektedir:
`https://api.frankfurter.app/latest?from=TRY`